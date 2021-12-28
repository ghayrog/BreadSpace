using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    public GameObject objFuelBar;
    public GameObject objTargetBar;
    public GameObject objBreadBar;
    public GameObject scoreText;
    public GameObject missionText;
    
    private float scaleSpeed = 1f;
    private float blinkSpeed = 1f;
    private int alphaIncrease;
    private Color initialColor;

    private Image imageBread;
    public float displayedScore;

    const string mission1 = "Earn 200 breadits\r\nto upgrade the base";
    const string mission2 = "Earn 1000 breadits\r\nto upgrade the base";
    const string mission3 = "Earn 5000 breadits\r\nto finish the base";
    const string mission4 = "Search and destroy\r\nthe Red Giant";

    private bool showMission = false;

    // Start is called before the first frame update
    void Start()
    {
        imageBread = objBreadBar.GetComponent<Image>();
        initialColor = imageBread.color;
    }

    // Update is called once per frame
    void Update()
    {
        //score
        if (displayedScore < PlayerController.Instance.score)
        {
            displayedScore += Time.deltaTime*30;
            showMission = true;
        }
        if (displayedScore > PlayerController.Instance.score)
        {
            displayedScore = PlayerController.Instance.score;
            showMission = true;
        }
        if (PlayerController.Instance.isVictory) showMission = false;
        TextMeshPro scoreTMP = scoreText.GetComponent<TextMeshPro>();
        LocalizeStringEvent scoreLoc = scoreText.GetComponent<LocalizeStringEvent>();

        TextMeshPro missionTMP = missionText.GetComponent<TextMeshPro>();
        LocalizeStringEvent missionLoc = missionText.GetComponent<LocalizeStringEvent>();
        if (showMission)
        {
            /*
            scoreTMP.text = "Breadits: " + (int)displayedScore;
            switch (GameManager.Instance.level)
            {
                case 0:
                    missionTMP.text = mission1;
                    break;
                case 1:
                    missionTMP.text = mission2;
                    break;
                case 2:
                    missionTMP.text = mission3;
                    break;
                case 3:
                    missionTMP.text = mission4;
                    break;
                default:
                    missionTMP.text = "";
                    break;
            }
            */
            scoreLoc.StringReference.TableEntryReference = "Breadits";
            scoreLoc.RefreshString();
            //Debug.Log("Score Refreshed");
            switch (GameManager.Instance.level)
            {
                case 0:
                    missionLoc.StringReference.TableEntryReference = "Mission1";
                    break;
                case 1:
                    missionLoc.StringReference.TableEntryReference = "Mission2";
                    break;
                case 2:
                    missionLoc.StringReference.TableEntryReference = "Mission3";
                    break;
                case 3:
                    missionLoc.StringReference.TableEntryReference = "Mission4";
                    break;
                default:
                    missionLoc.StringReference.TableEntryReference = "Empty";
                    break;
            }
        }
        else
        {
            scoreLoc.StringReference.TableEntryReference = "Empty";
            missionLoc.StringReference.TableEntryReference = "Empty";
            //Debug.Log("Score Erased");
            //scoreTMP.text = "";
            //missionTMP.text = "";
        }
        //if (GameManager.Instance.level < 3)
        //{
            //Change score opacity
            float distScore = (PlayerController.Instance.transform.position - scoreText.transform.parent.gameObject.transform.position).magnitude;
            float opacity = 1 - (distScore - 1) / (3 - 1);
            if (opacity > 1) opacity = 1;
            if (opacity < 0) opacity = 0;
            Color textColor;
            textColor = scoreTMP.color;
            textColor.a = opacity;
            scoreTMP.color = textColor;

            textColor = missionTMP.color;
            textColor.a = opacity;
            missionTMP.color = textColor;


            //Scale fuel bar
            float deltaScale;
            float targetScale;
            RectTransform barScaleFuel = objFuelBar.GetComponent<RectTransform>();
            RectTransform barScaleTarget = objTargetBar.GetComponent<RectTransform>();
            RectTransform barScaleBread = objBreadBar.GetComponent<RectTransform>();
            if (barScaleFuel != null)
            {
                deltaScale = PlayerController.Instance.fuel / PlayerController.Instance.maxFuel - barScaleFuel.localScale.x;
                targetScale = barScaleFuel.localScale.x + Time.deltaTime * scaleSpeed * Mathf.Sign(deltaScale);
                if (Mathf.Abs(deltaScale) < Time.deltaTime * scaleSpeed)
                {
                    targetScale = PlayerController.Instance.fuel / PlayerController.Instance.maxFuel;
                }
                barScaleFuel.localScale = new Vector3(targetScale, 1, 1);
            }
            if (barScaleTarget != null)
            {
                if (PlayerController.Instance.targetFuel < 0)
                {
                    barScaleTarget.localScale = new Vector3(0, 1, 1);
                }
                else
                {
                    barScaleTarget.localScale = new Vector3(PlayerController.Instance.targetFuel / PlayerController.Instance.maxFuel, 1, 1);
                }
            }

            //Scale bread bar
            Color currentColor;
            if (barScaleBread != null)
            {
                deltaScale = PlayerController.Instance.bread / PlayerController.Instance.maxBread - barScaleBread.localScale.x;
                targetScale = barScaleBread.localScale.x + Time.deltaTime * scaleSpeed * Mathf.Sign(deltaScale);
                if (Mathf.Abs(deltaScale) < Time.deltaTime * scaleSpeed)
                {
                    targetScale = PlayerController.Instance.bread / PlayerController.Instance.maxBread;
                }
                barScaleBread.localScale = new Vector3(targetScale, 1, 1);
                //Blink bread bar if full
                if (targetScale == 1)
                {
                    currentColor = imageBread.color;
                    if (currentColor.a >= initialColor.a)
                    {
                        alphaIncrease = -1;
                    }
                    if (currentColor.a <= 0.05f)
                    {
                        alphaIncrease = 1;
                    }

                    float targetAlpha = currentColor.a + alphaIncrease * Time.deltaTime * blinkSpeed;
                    if (targetAlpha >= initialColor.a) targetAlpha = initialColor.a;
                    if (targetAlpha <= 0.05f) targetAlpha = 0.05f;

                    currentColor.a = targetAlpha;
                    imageBread.color = currentColor;
                }
                else
                {
                    imageBread.color = initialColor;
                }
            }

        //}

    }
}
