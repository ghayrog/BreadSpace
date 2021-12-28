using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    private float targetAlpha;
//    private bool isActive;
    private float fadeSpeed;
    private Image imageComp;

    // Start is called before the first frame update
    void Start()
    {
        imageComp = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Color imageColor = imageComp.color;
        if (imageColor.a > targetAlpha)
        {
            imageColor.a -= Time.deltaTime * fadeSpeed;
            if (imageColor.a < targetAlpha) imageColor.a = targetAlpha;
        }
        if (imageColor.a < targetAlpha)
        {
            imageColor.a += Time.deltaTime * fadeSpeed;
            if (imageColor.a > targetAlpha) imageColor.a = targetAlpha;
        }
        imageComp.color = imageColor;
    }

    public void Hide(float speed)
    {
        targetAlpha = 0;
//        isActive = false;
        fadeSpeed = speed;
    }

    public void Show(float speed)
    {
        targetAlpha = 1;
 //       isActive = true;
        fadeSpeed = speed;
    }
}
