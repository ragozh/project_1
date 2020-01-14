using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBarController : MonoBehaviour
{
    public static UIHealthBarController instance { get; private set; }
    public Text healthText;
    public Image mask;
    float originalSize;    
    void Awake() 
    {
        instance = this;
    }

    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }
    public void SetValue(int curHP, int maxHP)
    {	
        float value = curHP / (float) maxHP;			      
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
        healthText.text = curHP + "/" + maxHP;
    }
}
