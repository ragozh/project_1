using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Vector3 TouchPosition;
    public Vector3 ClickPosition;
    // Start is called before the first frame update
    void Start()
    {
        Character = (GameObject)Instantiate(CharacterPrefab);
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
            LeftRoom.transform.position = new Vector2(0 * tileSizeX, row * -tileSizeY);
            LeftRoom.GetComponent<RoomController>().Y = row;
            LeftRoom.GetComponent<RoomController>().X = 0;
            LeftRoom.name = row + "" + 0;
            MiddleRoom.transform.position = new Vector2(1 * tileSizeX, row * -tileSizeY);
            MiddleRoom.GetComponent<RoomController>().Y = row;
            MiddleRoom.GetComponent<RoomController>().X = 1;
            MiddleRoom.name = row + "" + 1;
            RightRoom.transform.position = new Vector2(2 * tileSizeX, row * -tileSizeY);
            RightRoom.GetComponent<RoomController>().Y = row;
            RightRoom.GetComponent<RoomController>().X = 2;
            RightRoom.name = row + "" + 2;
            if(row == 1) {
                Character.GetComponent<MainCharacterController>().PositionCharacter(RightRoom);
            }

            #region Ladder create
            LastSLadder = NewLadder; // Connect with 2nd ladder from previous room
            NewLadder = UnityEngine.Random.Range(0, 2); // Random new ladder location
            if(NewLadder == LastFLadder) {
                LastSLadder = LastFLadder;
                GameObject LastDownLadderObject = GameObject.Find((row - 1) + "d");
                GameObject LastUpLadderObject = null;
                if(LastDownLadderObject != null) {
                    Destroy(LastDownLadderObject); // Destroy last row 2nd ladder               
                    LastUpLadderObject = GameObject.Find((row - 1) + "u");
                    if(LastUpLadderObject != null)
                        LastUpLadderObject.name = (row - 1) + "b"; // Change last row 1st ladder name
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
                Ladder1.transform.SetParent(GameObject.Find(row + "" + LastSLadder).transform);
                Ladder1.transform.position = Vector3.Scale(new Vector3(0, 0, 0), new Vector3(1.2f, 1.2f, 0)) + Ladder1.transform.parent.transform.position;
                Ladder1.name = row + "u";
                if(Ladder2 == null) Ladder1.name = row + "b";
            }
            if(Ladder2 != null){
                Ladder2.transform.SetParent(GameObject.Find(row + "" + NewLadder).transform);
                Ladder2.name = row + "d";
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
                     CharacterPositioning(Character.transform.parent.gameObject, ObjectTouched);
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
    //                  CharacterPositioning(Character.transform.parent.gameObject, ObjectTouched);
    //              }
    //          }
    //     }
    // }
    #endregion
    
    private void CharacterPositioning(GameObject StartRoom, GameObject TargetRoom){
        int sx = StartRoom.GetComponent<RoomController>().X;
        int sy = StartRoom.GetComponent<RoomController>().Y;
        int tx = TargetRoom.GetComponent<RoomController>().X;
        int ty = TargetRoom.GetComponent<RoomController>().Y;
        int LadderX = StartRoom.GetComponent<RoomController>().LadderRoom[0];
        bool isLadder = LadderX == sx;
        if (tx != sx) {
            int stepX = 0;
            if (tx > sx) {
                stepX = sx + 1;
            } else {
                stepX = sx - 1;
            }
            GameObject stepRoom = GameObject.Find(sy + "" + stepX);
            Character.GetComponent<MainCharacterController>().PositionCharacter(stepRoom);
            if (stepRoom == TargetRoom) return;
            CharacterPositioning(stepRoom, TargetRoom);
        } else if (isLadder) {
            int stepY = 0;
            if (ty > sy) {
                stepY = sy + 1;
            } else {
                stepY = sy - 1;
            }
            GameObject stepRoom = GameObject.Find(stepY + "" + sx);
            Character.GetComponent<MainCharacterController>().PositionCharacter(stepRoom);
            if (stepRoom == TargetRoom) return;
            CharacterPositioning(stepRoom, TargetRoom);
        } else {
            GameObject LadderRoom = GameObject.Find(sy + "" + LadderX);
            CharacterPositioning(StartRoom, LadderRoom);
            if (LadderRoom == TargetRoom) return;
            CharacterPositioning(LadderRoom, TargetRoom);
        }
    }
}
