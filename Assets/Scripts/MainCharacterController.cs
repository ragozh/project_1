using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    public int Y;
    public int X;
    Vector3 NewPosition;
    void Update(){
        Vector2 position = transform.position;
        position.x = position.x + 0.01f;
        transform.position = position;
    }

    public void PositionCharacter(GameObject NewParent){
        transform.SetParent(NewParent.transform);
        Y = NewParent.GetComponent<RoomController>().Y;
        X = NewParent.GetComponent<RoomController>().X;
        //NewPosition = NewCharacterPosition();
        Debug.Log("Move to: " + Y + X);
        transform.position = NewCharacterPosition();
    }
    public Vector3 NewCharacterPosition(){
        Vector3 scale = new Vector3(1.2f, 1.2f, 0);
        if (X == 0) {
            return Vector3.Scale(new Vector3(1.5f, -1.1f, 0), scale) + transform.parent.gameObject.transform.position;
        } else if (X == 1) {
            return Vector3.Scale(new Vector3(0.5f, -1.1f, 0), scale) + transform.parent.gameObject.transform.position;
        } else if (X == 2) {
            return Vector3.Scale(new Vector3(-1.5f, -1.1f, 0), scale) + transform.parent.gameObject.transform.position;
        }
        return transform.position;
    }
}
