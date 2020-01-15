using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{   
    public void PassTheRoom(GameObject room)
    {
        MainCharacterData.curStamina -= MainCharacterData.moveCost;
        UIStaminaBarController.instance.SetValue(MainCharacterData.curStamina, MainCharacterData.maxStamina);
        TextPopUpController.Create(room.transform.position, "-" + MainCharacterData.moveCost, Color.white, 8);        
        // RoomController roomController = room.GetComponent<RoomController>();        
        // if(!roomController.isClear){
        //     EnterRoom(room);
        // }
    }

    public void EnterRoom(GameObject room,bool isTarget = false){
        // Ambush chance
        int ambush = UnityEngine.Random.Range(0, 10);
        if(ambush <= 1){
            Debug.Log("Ambushed");
            EncounterMobs(true);
        } else {
            //dialog
            FindObjectOfType<DialogManager>().StartDialog(GameAssets.i.dialog);
        }
        Debug.Log("Entered the room: " + room.name);
        if(isTarget)
        {
            Debug.Log("Target Room");
        }
        room.GetComponent<RoomController>().isClear = true;
    }
    // Params: character's data, isAmbushed, 
    private bool EncounterMobs(bool ambush = false)
    {
        bool winState = true;
        // Battle system
        int result = UnityEngine.Random.Range(0,100);
        if(result <= 1)
        {
            // 2% lose
            winState = false;
        }
        // End battle
        return winState;
    }

    private void SearchTheRoom()
    {
        
    }
}
