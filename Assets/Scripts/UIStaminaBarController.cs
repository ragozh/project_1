using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStaminaBarController : MonoBehaviour
{
    public static UIStaminaBarController instance { get; private set; }
    public Text staminaText;
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
    public void SetValue(int curSta, int maxSta)
    {			
        float value = curSta / (float) maxSta;		      
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
        staminaText.text = curSta + "/" + maxSta;
    }
}
