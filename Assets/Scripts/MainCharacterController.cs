using System;
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

    public Vector3 CharacterMoving() {
        Vector3 direction = new Vector3();

        return direction;
    }

    #region A*
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
        if(x < 0 || x > 2){
            return false;
        }
        string findB = curX + "" + curY + "b";
        string findD = curX + "" + curY + "d";
        string findU = curX + "" + curY + "u";
        if (y > curY) {
            GameObject ladder = GameObject.Find(findD);
            if (ladder == null) ladder = GameObject.Find(findB);
            if (ladder == null) return false;
        } 
        if (y < curY) {
            GameObject ladder = GameObject.Find(findU);
            if (ladder == null) ladder = GameObject.Find(findB);
            if (ladder == null) return false;            
        }
        return true;
    }
    #endregion
}
