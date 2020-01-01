using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomController : MonoBehaviour
{
    public int Y;
    public int X;
    public bool MapRevealed = false;
    public bool Fog = true;
    public GameObject MapCoverPrefab;
    private GameObject MapCover;
    //SpriteRenderer renderer;
    void Start() 
    {
        //renderer = gameObject.GetComponent<SpriteRenderer>();
        if(MapRevealed == false)
        {
            MapCover = (GameObject)Instantiate(MapCoverPrefab, transform);
            MapCover.transform.position = transform.position;
        }
    }

    void Update()
    {
        if (MapRevealed == true)
        {
            GameObject.Destroy(MapCover);
        }
        if (Fog == true && MapRevealed == true)
        {
            foreach(SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>()) {
                Color originColor = renderer.color;
                renderer.material.color = new Color(originColor.r, originColor.g, originColor.b, 0.5f);
            }
        }
        else
        {
            foreach(SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>()) {
                Color originColor = renderer.color;
                renderer.material.color = new Color(originColor.r, originColor.g, originColor.b, 1.0f);
            }
        }
    }
}
