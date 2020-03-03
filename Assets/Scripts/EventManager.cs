using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    bool isDialogOff = true;
    #region Click the rooms to make character move
    public void clickTheRoomPositionToMoveCharacter(GameObject Character)
    {
        MainCharacterController CharacterController = getCharacterController(Character);
        RaycastHit2D TheTouch = getTheRaycastHitOfTheTouch();
        if (TheTouch.collider != null) {
            GameObject ObjectTouched = TheTouch.transform.gameObject;
            if(isTouchedOtherRoom(ObjectTouched, Character.transform.parent.gameObject)
                && isDialogOff) {
                CharacterController.CharacterSteps(Character.transform.parent.gameObject, ObjectTouched);
                //CharacterController.Test();
            }
        }
    }
    MainCharacterController getCharacterController(GameObject Character)
    {        
        return Character.GetComponent<MainCharacterController>();
    }
    RaycastHit2D getTheRaycastHitOfTheTouch()
    {
        Vector3 ClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 TouchPosition2D = new Vector2(ClickPosition.x, ClickPosition.y);
        return Physics2D.Raycast(TouchPosition2D, Camera.main.transform.forward);
    }
    bool isTouchedOtherRoom(GameObject ObjectTouched, GameObject CurrentRoom)
    {
        return ObjectTouched != CurrentRoom;
    }
    #endregion

    #region PassTheRoom
    public void PassTheRoom(GameObject room)
    {
        reduceTheCharacterStamina();
        setStaminaBarValue();
        makeFloatingStaminaMinus(room.transform.position);
    }
    void reduceTheCharacterStamina()
    {
        MainCharacterData.curStamina -= MainCharacterData.moveCost;
    }
    void setStaminaBarValue()
    {
        UIStaminaBarController.instance.SetValue(MainCharacterData.curStamina, MainCharacterData.maxStamina);
    }
    void makeFloatingStaminaMinus(Vector3 position){
        TextPopUpController.Create(position, "-" + MainCharacterData.moveCost, Color.white, 8);
    }
    #endregion

    public void EnterRoom(GameObject room,bool isTarget = false){
        // Ambush chance
        
        if(isAmbush()){
            Debug.Log("Ambushed");
            EncounterMobs(true);
        } else {
            //dialog
            Debug.Log("Start dialog");
            PageableDialog dialog = DialogManager.Instance().CreatePageableDialog();
            dialog.AddPage("You reached a new room, what to do?");
            dialog.OnNextBtnAction("Search The Room", () => { 
                Debug.Log("FINISHED");
                dialog.Hide();
                isDialogOff = dialog.isDialogIsOff();
            });
            dialog.SetTitle("New Room");
            dialog.Show();
            isDialogOff = dialog.isDialogIsOff();
            Debug.Log("Start dialog");
        }
        Debug.Log("Entered the room: " + room.name);
        if(isTarget)
        {
            Debug.Log("Target Room");
        }
        room.GetComponent<RoomController>().isClear = true;
    }
    bool isAmbush(){
        int ambush = UnityEngine.Random.Range(0, 10);
        return ambush <= 1;
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
