using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopUpController : MonoBehaviour
{
    public static TextPopUpController Create(Vector3 position, string text)
    {
        Transform damagePopUpText = Instantiate(GameAssets.i.TextPopUp, position, Quaternion.identity);
        TextPopUpController textPopUpController = damagePopUpText.GetComponent<TextPopUpController>();
        textPopUpController.Setup(text);
        return textPopUpController;
    }
    private TextMeshPro textMesh;
    private float disapearTimer;
    private Color textColor;
    void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
    }
    public void Setup(string text)
    {
        textMesh.SetText(text);
        textColor = textMesh.color;
        disapearTimer = 1f;
    }

    private void Update() {
        float moveYSpeed = 3f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        disapearTimer -= Time.deltaTime;
        if (disapearTimer <= 0) {
            float disapearSpeed = 3f;
            textColor.a -= disapearSpeed*Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a <= 0) {
                Destroy(gameObject);
            }
        }
    }
}
