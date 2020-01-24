using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public PageableDialog pdPrefab;
    public Canvas canvas; // dialogs must be on some canvas

    public PageableDialog CreatePageableDialog(){
        pdPrefab.enabled = true;
        PageableDialog newPageableDialog = Instantiate(pdPrefab, canvas.transform);        
        pdPrefab.enabled = false;
        return newPageableDialog;
    }

    private static DialogManager instance;
    public static DialogManager Instance() {
        if(!instance){
            instance = FindObjectOfType(typeof (DialogManager)) as DialogManager;
            if(!instance)
                Debug.Log("There need to be at least one active DialogsManager on the scene");
        }

        return instance;
    }
}