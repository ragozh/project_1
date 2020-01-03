using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    #region Stats of Character
    private int maxHP;
    public int curHP;
    public int maxStamina;
    public int curStamina;
    public int curXP;
    #endregion
    #region Moving Character Variables
    public int Y;
    public int X;
    private bool FacingRight = true;
    public List<GameObject> listPosition = new List<GameObject>();
    public Vector3 curPosition;
    public int curPositionIdx = 0;
    Camera mainCam;
    float newCamPositionY;
    #endregion
    void Start() 
    {
        mainCam = Camera.main;
    }
    void Update()
    {
        if (listPosition.Count > 0) {
            if (transform.position != curPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, curPosition, 4*Time.deltaTime);
                Vector3 camCurPosition = mainCam.transform.position;
                Vector3 newCamPosition = new Vector3(camCurPosition.x, newCamPositionY, -10);
                mainCam.transform.position = Vector3.MoveTowards(camCurPosition, newCamPosition, 4*Time.deltaTime);
            } else {
                if (curPositionIdx < listPosition.Count - 1) {
                    curPositionIdx++;
                    curPosition = NewPositionCharacter(listPosition[curPositionIdx]);
                    newCamPositionY = listPosition[curPositionIdx].transform.position.y;
                }
            }
        }

    }
    public void GenerateCharacterStats()
    {
        maxHP = 100;
        curHP = maxHP;
    }
    public Vector3 NewPositionCharacter(GameObject NewParent)
    {
        if(transform.parent != null)
        {
            int oldX = transform.parent.gameObject.GetComponent<RoomController>().X;
            int oldY = transform.parent.gameObject.GetComponent<RoomController>().Y;
            GameObject oldParent = GameObject.Find(oldX + "" + oldY);
            oldParent.GetComponent<RoomController>().Fog = true;
        }
        transform.SetParent(NewParent.transform);
        curHP -= 2;
        UIHealthBarController.instance.SetValue(curHP / (float) maxHP);
        TextPopUpController.Create(transform.position, "-2");
        RoomController newRoomController = NewParent.GetComponent<RoomController>();
        Y = newRoomController.Y;
        X = newRoomController.X;
        newRoomController.MapRevealed = true;
        newRoomController.Fog = false;
        newRoomController.EnterRoom();
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
    
    public void CharacterSteps(GameObject StartRoom, GameObject TargetRoom){
        RoomController StartRoomController = StartRoom.GetComponent<RoomController>();
        RoomController TargetRoomController = TargetRoom.GetComponent<RoomController>();
        // reverse the start and target to get linked node at order
        Node path = FindWay(TargetRoomController.X, TargetRoomController.Y, StartRoomController.X, StartRoomController.Y);
        if (path != null) {
            do {
                listPosition.Add(GameObject.Find(path.x + "" + path.y));
                path = path.parent;
            }
             while (path != null);
        } else {
            Debug.Log("I dont know how to get there.");
        }
    }

    private void Flip()
	{
		FacingRight = !FacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    #region A*
    // a* algorithm for path finder
    public class Node
    {
        public int x;
        public int y;
        public Node parent;
        public int g;
        public int h;
        public int f;
        public Node(int _x, int _y, int targetX, int targetY, int _h = 0, int _g = 0)
        {
            x = _x;
            y = _y;
            h = calH(targetX, targetY);
            g = _g;
        }

        public int calH(int targetX, int targetY)
        {
            return (x - targetX)*(x - targetX) + (y - targetY)*(y - targetY);
        }
    }
    int getMinF(List<Node> list) // get current shortest path
    {
        int minF = 1000000;
        int index = -1;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].f < minF) {
                minF = list[i].f;
                index = i;
            }
        }
        return index;
    }
    int findNode(int nodeX, int nodeY, List<Node> list) // get index of `node` in `list`
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].x == nodeX && list[i].y == nodeY) {
                return i;
            }
        }
        return -1;
    }
    int[] dy = {-1, 0, 1, 0};
    int[] dx = {0, -1, 0, 1};
    public Node FindWay(int startX, int startY, int targetX, int targetY)
    {
        List<Node> OPEN = new List<Node>();
        List<Node> CLOSE = new List<Node>();
        Node startNode = new Node(startX, startY, targetX, targetY);
        startNode.g = 0;
        startNode.f = startNode.h;
        OPEN.Add(startNode);
        while (OPEN.Count > 0) {
            int curIndex = getMinF(OPEN);
            Node curNode = OPEN[curIndex];
            OPEN.RemoveAt(curIndex);
            if (curNode.x == targetX && curNode.y == targetY) {
                return curNode;
            }
            for (int i = 0; i < 4; i++)
            {
                int x = curNode.x + dx[i];
                int y = curNode.y + dy[i];
                Node Mi = null;
                if (checkRoomMovement(x, y, curNode.x, curNode.y)) {
                    int dMi = curNode.g + 1;
                    int openIdx = findNode(x, y, OPEN);
                    int closeIdx = findNode(x, y, CLOSE);
                    if (openIdx < 0 && closeIdx < 0) {
                        Mi = new Node(x, y, targetX, targetY);
                        Mi.g = dMi;
                        Mi.f = Mi.g + Mi.h;
                        Mi.parent = curNode;
                        OPEN.Add(Mi);
                    }
                }
            }
            CLOSE.Add(curNode);
        }
        return null;
    }
    
    bool checkRoomMovement(int x, int y, int curX, int curY)
    {
        if (x < 0 || x > 2)
        {
            return false;
        }

        string findB = curX + "" + curY + "b";
        string findD = curX + "" + curY + "d";
        string findU = curX + "" + curY + "u";
        GameObject ladder;
        if (y > curY) {
            ladder = GameObject.Find(findD);
            if (ladder == null) ladder = GameObject.Find(findB);
            if (ladder == null) return false;
        } 
        if (y < curY) {
            ladder = GameObject.Find(findU);
            if (ladder == null) ladder = GameObject.Find(findB);
            if (ladder == null) return false;            
        }
        return true;
    }
    #endregion
}
