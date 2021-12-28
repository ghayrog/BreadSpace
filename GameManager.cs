using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public HelpText helperText;
    public GameObject upgradeButton;
    public GameObject catObject;
    public GameObject cam;
    [Header("Parameters")]
    public int level = 0;
    private int levelCap = 3;
    const int level1Target = 200;
    const int level2Target = 1000;
    const int level3Target = 5000;
    private int currentTarget;
    public float fuelTimer;
    public float breadTimer;
    public int awardType; //1 - continue 2 - refuel
    [Header("Prefabs")]
    public GameObject breadPrefab;
    public GameObject fuelPrefab;

    private bool firstClickFlag;
    private bool firstFullFlag;
    private bool firstDropFlag;
    private bool firstNoFuel;
    private bool firstCat;
    private bool firstPortal;
    private bool firstLowFuel;

    private GameObject[] portals;
    private float scanTimer;
    private float creditTimer;
    private int currentCredit = 1;
    private float betaTimer = 30f;
    private int awardAmount = 100;
    private float helpTextY = 300f;
    private bool showBetaWarning;
    private GameObject music;

    //Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<GameManager>();
            return instance;
        }
    }

    public void AwardAfterAd()
    {
        ResumeGame();
        switch (awardType)
        {
            case 1:
                helperText.showHelpMsgLocale("Award", 10f);
                PlayerController.Instance.score += awardAmount;
                break;
            case 2:
                DropFuel();
                break;
            default:
                break;
        }
    }

    public void SaveGameOptions()
    {
        int targetLocaleIndex = 0;
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[i])
            {
                targetLocaleIndex = i;
            }
        }
        //LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[targetLocaleIndex];
        bool mute = music.GetComponent<AudioSource>().mute;
        PlayerPrefs.SetInt("LocaleIndex", targetLocaleIndex);
        PlayerPrefs.SetInt("MuteMusic", mute ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Game Options saved");
    }

    public void LoadGameOptions()
    {
        if (PlayerPrefs.HasKey("LocaleIndex"))
        {
            int targetLocaleIndex = PlayerPrefs.GetInt("LocaleIndex");
            while (LocalizationSettings.SelectedLocale == null)
            { 
            }
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[targetLocaleIndex];

            bool mute = (PlayerPrefs.GetInt("MuteMusic") == 1);
            music.GetComponent<AudioSource>().mute = mute;
            Debug.Log("Game Options loaded");
        }
        else Debug.Log("Game Options not loaded");

    }

    // Start is called before the first frame update
    void Start()
    {
        ShakeBreadcrumbs();
        //helperText.showHelpMsg("Tap the Mousetronaut\r\nand hold to aim", 10f);
        helperText.showHelpMsgLocale("TapHelp",10f);
        portals = GameObject.FindGameObjectsWithTag("Portal");
        music = GameObject.Find("Music");
        LoadGameOptions();
    }

    private void Awake()
    {
        //Debug.Log(LocalizationSettings.AvailableLocales.Locales.Count);
    }

    public void showCredits()
    {
        creditTimer = 10.5f;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public bool IsPaused()
    {
        if (Time.timeScale == 0) return true; else return false;
    }

    public void DropFuel()
    {
        float randomAngle = Random.Range(-Mathf.PI, Mathf.PI);
        float xPos = PlayerController.Instance.transform.position.x + 1 * Mathf.Cos(randomAngle);
        float yPos = PlayerController.Instance.transform.position.y + 1 * Mathf.Sin(randomAngle);
        Vector3 pos = new Vector3(xPos, yPos, 0);
        GameObject newFuel = Instantiate(fuelPrefab, pos, Quaternion.identity);
        float fuelAmount = PlayerController.Instance.maxFuel / 3;
        newFuel.GetComponent<FuelScript>().fuelVolume = fuelAmount;

    }

    // Update is called once per frame
    void Update()
    {
        //Helper text
        if (Input.GetMouseButtonDown(0) && !firstClickFlag)
        {
            //helperText.Hide(5f);
        }
        if (Input.GetMouseButtonUp(0) && !firstClickFlag && PlayerController.Instance.firstClickFlag)
        {
            firstClickFlag = true;
            //helperText.showHelpMsg("Collect the breadcrumbs\r\nto fill the bar below", 10f);
            helperText.showHelpMsgLocale("CollectHelp", 10f);
        }
        if ((PlayerController.Instance.bread == PlayerController.Instance.maxBread))// && !firstFullFlag)
        {
            if (!firstFullFlag)
            {
                firstFullFlag = true;
                //helperText.showHelpMsg("Out of capacity!\r\nReturn to your base\r\nto exchange breadcrumbs for breadits", 10f);
                helperText.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, helpTextY, 0);
                helperText.showHelpMsgLocale("NoStorageHelp", 10f);
            }
        }
        else firstFullFlag = false;
        if ((PlayerController.Instance.isBusy) && !firstDropFlag)
        {
            firstDropFlag = true;
            //helperText.showHelpMsg("Collect more breadcrumbs\r\nto upgrade your base", 10f);
            helperText.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, helpTextY, 0);
            showBetaWarning = true;
            helperText.showHelpMsgLocale("CollectMoreHelp", 10f);
        }
        if (PlayerController.Instance.fuel <= 0.25 * PlayerController.Instance.maxFuel)
        {
            if (!firstLowFuel)
            {
                firstLowFuel = true;
                //helperText.showHelpMsg("To get fuel in your suit\r\neither return to your base\r\nor pick up a fuel can", 10f);
                helperText.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, helpTextY, 0);
                helperText.showHelpMsgLocale("FuelWarning", 10f);
            }
        }
        else firstLowFuel = false;
        if ((PlayerController.Instance.fuel == 0))// && !firstNoFuel)
        {
            if (!firstNoFuel)
            {
                firstNoFuel = true;
                //helperText.showHelpMsg("To get fuel in your suit\r\neither return to your base\r\nor pick up a fuel can", 10f);
                helperText.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, helpTextY, 0);
                helperText.showHelpMsgLocale("FuelHelp", 10f);

            }
        }
        else firstNoFuel = false;
        if (Mathf.Abs(catObject.transform.position.x) <= 10 && Mathf.Abs(catObject.transform.position.y) <= 10 && !firstCat)
        {
            firstCat = true;
            //helperText.showHelpMsg("Stay away from\r\nthe red giant!", 10f);
            helperText.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, helpTextY, 0);
            helperText.showHelpMsgLocale("CatHelp", 10f);
            scanTimer = 12f;
            catObject.GetComponent<Cat>().PlaySoundFX("appear");
            
        }
        if (scanTimer > 0)
        {
            scanTimer -= Time.deltaTime;
            if (scanTimer <= 0)
            {
                helperText.showHelpMsgLocale("ScannerHelp", 10f);
            }
        }
        if (!firstPortal)
        {
            foreach (GameObject portal in portals)
            {
                if ((PlayerController.Instance.transform.position - portal.transform.position).magnitude < 3)
                {
                    firstPortal = true;
                    helperText.showHelpMsgLocale("PortalHelp", 10f);
                }
            }
        }


        //Win credits
        if (creditTimer > 0)
        {
            creditTimer -= Time.deltaTime;
            if (creditTimer <= 0)
            {
                helperText.showHelpMsgLocale("Credits" + currentCredit, 10f);
                creditTimer = 10.5f;
                if (currentCredit < 10) currentCredit++; else currentCredit = 1;
            }
        }

        //Beta warning
        if (betaTimer > 0)
        {
            betaTimer -= Time.deltaTime;
            if (betaTimer <= 0)
            {
                betaTimer = 45f;
                if (!helperText.isShowing && showBetaWarning) helperText.showHelpMsgLocale("BetaWarning", 10f);
            }
        }

        //Check fuel spawn timer
        if (fuelTimer > 0)
        {
            fuelTimer -= Time.deltaTime;
            if (fuelTimer <= 0)
            {
                fuelTimer = 0;

                //Create a new fuel can
                float r = Random.Range(6f, 8f);
                float angle = Random.Range(0, 2 * Mathf.PI);
                if (cam.transform.position.magnitude > 0.1f)
                {
                    angle = Mathf.Atan2(-cam.transform.position.y, -cam.transform.position.x);
                    angle += Random.Range(-Mathf.PI/2, Mathf.PI/2);
                }

                float xPos = r * Mathf.Cos(angle);
                float yPos = r * Mathf.Sin(angle);
                Vector3 pos = new Vector3(xPos, yPos, 0);
                GameObject newFuel = Instantiate(fuelPrefab, pos, Quaternion.identity);
                float fuelAmount = 30 + level * 5 + Random.Range(0f,10f);
                newFuel.GetComponent<FuelScript>().fuelVolume = fuelAmount;
            }
        }
        else
        {
            //Start fuel timer if less than 3 fuel cans exist
            GameObject[] findFuel = GameObject.FindGameObjectsWithTag("Fuel");
            if (findFuel.Length < 3)
            {
                fuelTimer = Random.Range(5f, 10f);
            }
        }

        //Check bread spawn timer
        if (breadTimer > 0)
        {
            breadTimer -= Time.deltaTime;
            if (breadTimer <= 0)
            {
                breadTimer = 0;
                GameObject findBread = GameObject.FindGameObjectWithTag("Breadcomet");
                if (findBread == null)
                {
                    //Create a new bread comet
                    Vector3 pos = new Vector3(20, 20, 0);
                    Instantiate(breadPrefab, pos, Quaternion.identity);
                }
            }
        }


        currentTarget = level1Target;
        switch (level)
        {
            case 1:
                currentTarget = level2Target;
                break;
            case 2:
                currentTarget = level3Target;
                break;
        }

        if (currentTarget <= PlayerController.Instance.score && level<levelCap)
        {
            //Allow upgrade
            upgradeButton.GetComponent<ButtonUpgrade>().Show(5f);
        }
        else
        {
            //No upgrade
            upgradeButton.GetComponent<ButtonUpgrade>().Hide(5f);
        }
    }

    public void AdvanceLevel()
    {
        level += 1;
        PlayerController.Instance.score -= currentTarget;
        ShakeCam.Instance.ShakeCamera(2f, 0.3f);
        if (level == 1)
        {
            catObject.GetComponent<Cat>().catState = 1;
            PlayerController.Instance.maxBread = 15;
            PlayerController.Instance.maxFuel = 150;
            GameObject.Find("Base").GetComponent<Base>().SetTargetAlpha(1,1,0,0);
            //helperText.showHelpMsg("Well done!\r\nYour fuel and bread capacities\r\nincrease with each upgrade", 10f);
            helperText.showHelpMsgLocale("LevelupHelp", 10f);
        }
        if (level == 2)
        {
            PlayerController.Instance.maxBread = 20;
            PlayerController.Instance.maxFuel = 200;
            catObject.GetComponent<Cat>().catSpeed *= 1.5f;
            GameObject.Find("Base").GetComponent<Base>().SetTargetAlpha(1, 1, 1,0);
            GameObject.Find("Cat").GetComponent<Cat>().chaseRadius += 2;
            helperText.showHelpMsgLocale("Munch", 10f);
        }
        if (level == 3)
        {
            
            PlayerController.Instance.maxBread = 30;
            PlayerController.Instance.maxFuel = 250;
            //PlayerController.Instance.animator.SetBool("lastLevel",true);
            catObject.GetComponent<Cat>().catState = 3;
            GameObject.Find("Base").GetComponent<Base>().SetTargetAlpha(1, 1, 1, 1);
            Aura.Instance.isOn = true;
            //GameObject.Find("BaseRigidbodyCollider").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            helperText.showHelpMsgLocale("Force", 10f);
        }
    }

    public void SpawnBreadComet()
    { 

    }

    public void ShakeBreadcrumbs()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Breadcrumb");
        foreach (GameObject breadcrumb in gameObjects)
        {
            float xShake = Random.Range(-1f,1f);
            float yShake = Random.Range(-1f, 1f);
            breadcrumb.transform.position += new Vector3(xShake,yShake,0);
        }
    }
}
