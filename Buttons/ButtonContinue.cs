using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonContinue : MonoBehaviour
{
    public GameObject capsule;
    public GameObject textMesh;
    public GameObject subMesh;
    public int buttonType; //1-continue 2-restart 3-donate 4-refuel
    private float targetAlpha;
    private bool isActive;
    private float fadeSpeed;
    private Image capsuleComp;
    private TextMeshProUGUI textComp;
    private TextMeshProUGUI subComp;
    private Vector3 startScale;
    // Start is called before the first frame update
    void Start()
    {
        capsuleComp = capsule.GetComponent<Image>();
        textComp = textMesh.GetComponent<TextMeshProUGUI>();
        subComp = subMesh.GetComponent<TextMeshProUGUI>();
        startScale = gameObject.transform.localScale;
        gameObject.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Color capsuleColor = capsuleComp.color;
        Color textColor = textComp.color;
        Color subColor = subComp.color;
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
        textColor.a = capsuleColor.a;
        subColor.a = capsuleColor.a;
        capsuleComp.color = capsuleColor;
        textComp.color = textColor;
        subComp.color = subColor;
    }

    public void Hide(float speed)
    {
        targetAlpha = 0;
        isActive = false;
        fadeSpeed = speed;
        gameObject.transform.localScale = Vector3.zero;
    }

    public void Show(float speed)
    {
        targetAlpha = 1;
        isActive = true;
        fadeSpeed = speed;
        gameObject.transform.localScale = startScale;
    }

    public void OnMouseDown()
    {
        //Debug.Log("Clicking button");
        if (isActive)
        {
            switch (buttonType)
            {
                case 1: //continue
                    //Debug.Log("Continue clicked");
                    SoundFX.Instance.PlaySoundFX("button");
                    GameManager.Instance.awardType = 1;
                    GameManager.Instance.PauseGame();
                    if (AdsInitializer.Instance.IsAllowedRewarded()) AdsInitializer.Instance.ShowAd();
                    PlayerController.Instance.fuel = PlayerController.Instance.maxFuel;
                    PlayerController.Instance.targetFuel = PlayerController.Instance.maxFuel;
                    PlayerController.Instance.bread = 0;
                    PlayerController.Instance.isTasted = false;
                    PlayerController.Instance.transform.position = Vector3.zero;
                    PlayerController.Instance.GetComponent<CapsuleCollider2D>().enabled = true;
                    PlayerController.Instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    PlayerController.Instance.animator.SetBool("isBusy", false);
                    GameObject.Find("Tasted").GetComponent<ImageFader>().Hide(3f);
                    GameObject.Find("ButtonContinue").GetComponent<ButtonContinue>().Hide(3f);
                    GameObject.Find("ButtonRestart").GetComponent<ButtonContinue>().Hide(3f);
                    break;
                case 2: //restart
                    SoundFX.Instance.PlaySoundFX("button");
                    SceneManager.LoadScene("SampleScene");
                    break;
                case 3: //donate
                    //Debug.Log("URL clicked");
                    SoundFX.Instance.PlaySoundFX("button");
                    Application.OpenURL("https://www.havoksun.com");
                    break;
                case 4: //refuel
                    SoundFX.Instance.PlaySoundFX("button");
                    GameManager.Instance.awardType = 2;
                    GameManager.Instance.PauseGame();
                    Hide(3f);
                    Debug.Log("Refuel Pressed");
                    if (AdsInitializer.Instance.IsAllowedRewarded()) AdsInitializer.Instance.ShowAd();
                    break;
                default:
                    break;
            }
        }
    }
}
