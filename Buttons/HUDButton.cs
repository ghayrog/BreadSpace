using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class HUDButton : MonoBehaviour
{
    public int buttonType; //1-language 2-exit
    [SerializeField] private float doubleClickTimer = 0;
    // Start is called before the first frame update

    private GameObject music;
    void Start()
    {
        music = GameObject.Find("Music");
    }

    // Update is called once per frame
    void Update()
    {
        //Timer for double click
        if (doubleClickTimer > 0)
        {
            doubleClickTimer -= Time.deltaTime;
            if (doubleClickTimer <= 0)
            {
                doubleClickTimer = 0;
                if (buttonType ==2)
                {
                    music.GetComponent<AudioSource>().mute = !music.GetComponent<AudioSource>().mute;
                    //GameObject.Find("HelpText").GetComponent<HelpText>().showHelpMsgLocale("Exit", 10f);
                    GameManager.Instance.SaveGameOptions();
                }
            }
        }
    }

    public void OnMouseDown()
    {
        switch (buttonType)
        {
            case 1:
                int targetLocaleIndex = 0;
                for (int i = 0; i< LocalizationSettings.AvailableLocales.Locales.Count;i++)
                {
                    if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[i])
                    {
                        targetLocaleIndex = i + 1;
                    }
                }
                if (targetLocaleIndex>= LocalizationSettings.AvailableLocales.Locales.Count) targetLocaleIndex = 0;
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[targetLocaleIndex];
                SoundFX.Instance.PlaySoundFX("button");
                GameManager.Instance.SaveGameOptions();
                break;
            case 2:
                if (doubleClickTimer > 0)
                { 
                    Application.Quit();
                    Debug.Log("Application.Quit");
                }
                else
                {
                    doubleClickTimer = 0.3f;
                    //                    music.GetComponent<AudioSource>().mute = !music.GetComponent<AudioSource>().mute;
                    GameObject.Find("HelpText").GetComponent<HelpText>().showHelpMsgLocale("Exit", 10f);
                }
                SoundFX.Instance.PlaySoundFX("button");
                break;
            default:
                break;
        }
    }
}
