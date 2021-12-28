using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;

public class HelpText : MonoBehaviour
{
    public bool isShowing;
    private float targetAlpha;
    //    private bool isActive;
    private float fadeSpeed;
    private TextMeshProUGUI imageComp;
    public LocalizeStringEvent locComp;
    private float textTimer;

    // Start is called before the first frame update
    void Start()
    {
        imageComp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (textTimer > 0)
        {
            textTimer -= Time.deltaTime;
            if (textTimer <= 0)
            {
                //Hide help
                Hide(5f);
            }
        }

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
        if (imageColor.a > 0) isShowing = true; else isShowing = false;
    }

    public void showHelpMsg(string msg, float timer)
    {
        imageComp.text = msg;
        Show(5f);
        textTimer = timer;
    }

    public void showHelpMsgLocale(string msg, float timer)
    {
        //imageComp.text = msg;
        if (!PlayerController.Instance.isTasted)
        {
            locComp.StringReference.TableEntryReference = msg;
            Show(5f);
            textTimer = timer;
        }
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
