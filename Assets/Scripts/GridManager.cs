using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MainCharacterController;

public class GridManager : MonoBehaviour
{
    private int rows = 100;
    float tileSizeX = 5.9f;
    float tileSizeY = 3;
    public GameObject LeftRoomPrefab;
    public GameObject MiddleRoomPrefab;
    public GameObject RightRoomPrefab;
    public GameObject CharacterPrefab;
    public GameObject LadderPrefab;
    public GameObject Character;
    private Vector3 TouchPosition;
    private Vector3 ClickPosition;
    public MainCharacterController CharacterController;
    // Start is called before the first frame update
    void Start()
    {
        Character = (GameObject)Instantiate(CharacterPrefab);
        CharacterController = Character.GetComponent<MainCharacterController>();
        GenerateGrid();
    }
    void Update() {
        ClickEvent();
    }

    private void GenerateGrid()
    {
        int LastFLadder = -1; // Last row 1st ladder location
        int LastSLadder = -1; // Last row 2nd ladder location
        int NewLadder = -1;     
        for (int row = 0; row < rows; row++)
        {
            GameObject LeftRoom = (GameObject)Instantiate(LeftRoomPrefab, transform);
            GameObject MiddleRoom = (GameObject)Instantiate(MiddleRoomPrefab, transform);
            GameObject RightRoom = (GameObject)Instantiate(RightRoomPrefab, transform);
            RoomController LeftRoomController = LeftRoom.GetComponent<RoomController>();
            RoomController MidRoomController = MiddleRoom.GetComponent<RoomController>();
            RoomController RightRoomController = RightRoom.GetComponent<RoomController>();
            LeftRoom.transform.position = new Vector2(0 * tileSizeX, row * -tileSizeY);
            LeftRoomController.Y = row;
            LeftRoomController.X = 0;
            LeftRoom.name = 0 + "" + row;
            MiddleRoom.transform.position = new Vector2(1 * tileSizeX, row * -tileSizeY);
            MidRoomController.Y = row;
            MidRoomController.X = 1;
            MiddleRoom.name = 1 + "" + row;
            RightRoom.transform.position = new Vector2(2 * tileSizeX, row * -tileSizeY);
            RightRoomController.Y = row;
            RightRoomController.X = 2;
            RightRoom.name = 2 + "" + row;
            if(row == 0) {
                Character.transform.position = CharacterController.NewPositionCharacter(RightRoom);
                CharacterController.curPosition = Character.transform.position;
            }

            #region Ladder create
            LastSLadder = NewLadder; // Connect with 2nd ladder from previous room
            NewLadder = UnityEngine.Random.Range(0, 3); // Random new ladder location
            if(NewLadder == LastFLadder) {
                GameObject LastDownLadderObject = GameObject.Find(LastSLadder + "" + (row - 1) + "d");
                LastSLadder = LastFLadder;
                GameObject LastUpLadderObject = null;
                if(LastDownLadderObject != null) {
                    Destroy(LastDownLadderObject); // Destroy last row 2nd ladder               
                    LastUpLadderObject = GameObject.Find(LastFLadder + "" + (row - 1) + "u");
                    if(LastUpLadderObject != null)
                        LastUpLadderObject.name = LastFLadder + "" + (row - 1) + "b"; // Change last row 1st ladder name
                }
            }
            GameObject Ladder1 = null;
            GameObject Ladder2 = null;
            if(row != 0)
                Ladder1 = (GameObject)Instantiate(LadderPrefab, transform);
            if(NewLadder != LastSLadder && row != rows - 1) {
                Ladder2 = (GameObject)Instantiate(LadderPrefab, transform);; // Destroy if new ladder same room with connect ladder
            }
            if(Ladder1 != null){
                Ladder1.transform.SetParent(GameObject.Find(LastSLadder + "" + row).transform);
                Ladder1.transform.position = Vector3.Scale(new Vector3(0, 0, 0), new Vector3(1.2f, 1.2f, 0)) + Ladder1.transform.parent.transform.position;
                Ladder1.name = LastSLadder + "" + row + "u";
                if(Ladder2 == null) Ladder1.name = LastSLadder + "" + row +"b";
            }
            if(Ladder2 != null){
                Ladder2.transform.SetParent(GameObject.Find(NewLadder + "" + row).transform);
                Ladder2.name = NewLadder + "" + row + "d";
                Ladder2.transform.position = Vector3.Scale(new Vector3(0, 0, 0), new Vector3(1.2f, 1.2f, 0)) + Ladder2.transform.parent.transform.position;
            }
            LastFLadder = LastSLadder;
            #endregion
        }
    }

    private void ClickEvent(){
        if (Input.GetMouseButtonDown(0))
        {
            ClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 TouchPosition2D = new Vector2(ClickPosition.x, ClickPosition.y);
            RaycastHit2D TheTouch = Physics2D.Raycast(TouchPosition2D, Camera.main.transform.forward);
            if (TheTouch.collider != null) {
                 GameObject ObjectTouched = TheTouch.transform.gameObject;
                 if(ObjectTouched != Character.transform.parent.gameObject) {
                    Debug.Log("start: " + Character.transform.parent.gameObject.name + ", target: " + ObjectTouched.name);
                    CharacterController.CharacterSteps(Character.transform.parent.gameObject, ObjectTouched);
                 }
             }
        }
    }

    #region TouchEvent
    // private void TouchEvent(){
    //     if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
    //     {
    //         TouchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
    //         Vector2 TouchPosition2D = new Vector2(TouchPosition.x, TouchPosition.y);
    //         RaycastHit2D TheTouch = Physics2D.Raycast(TouchPosition2D, Camera.main.transform.forward);
    //         if (TheTouch.collider != null) {
    //              GameObject ObjectTouched = TheTouch.transform.gameObject;
    //              if(ObjectTouched != Character.transform.parent.gameObject) {
    //                  CharacterSteps(Character.transform.parent.gameObject, ObjectTouched);
    //              }
    //          }
    //     }
    // }
    #endregion
    
}
