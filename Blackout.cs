using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackout : MonoBehaviour
{
    private float targetAlpha;
    private float fadeSpeed;
    private Image capsuleComp;

    //Singleton
    private static Blackout instance;
    public static Blackout Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<Blackout>();
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        capsuleComp = gameObject.GetComponent<Image>();
        Color capsuleColor = capsuleComp.color;
        capsuleColor.a = 1;
        capsuleComp.color = capsuleColor;
        Hide(2f);
    }

    // Update is called once per frame
    void Update()
    {
        Color capsuleColor = capsuleComp.color;
        if (capsuleColor.a > targetAlpha)
        {
            capsuleColor.a -= Time.deltaTime * fadeSpeed;
            if (capsuleColor.a < targetAlpha) capsuleColor.a = targetAlpha;
        }
        if (capsuleColor.a < targetAlpha)
        {
            capsuleColor.a += Time.deltaTime * fadeSpeed;
            if (capsuleColor.a > targetAlpha) capsuleColor.a = targetAlpha;
        }
        capsuleComp.color = capsuleColor;
    }

    public void Hide(float speed)
    {
        targetAlpha = 0;
        fadeSpeed = speed;
    }

    public void Show(float speed)
    {
        targetAlpha = 1;
        fadeSpeed = speed;
    }

}
