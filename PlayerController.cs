using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Animator logoAnimator;
    public HelpText helperText;
    public AdsInitializer adsInitializer;
    [Header("Character Settings")]
    public float fuel;
    public float targetFuel;
    public float maxFuel;
    public float bread;
    public float maxBread;
    public int score;
    public bool isBusy;
    public float busyTimer;
    public bool isTeleported;
    public float teleTimer;
    public bool isTasted;
    public bool isVictory;
    public Vector3 pushDirection;
    [Header("Cursor Settings")]
    public Camera cameraView;
    public bool isInterrupted;
    //public bool isControlingBase;
    [SerializeField] private bool isAiming;
    [SerializeField] private float aimRadius;
    [SerializeField] private LayerMask rayCursorMask; //Mask to detect target from camera
    [SerializeField] private float rayCameraLength; //Length to detect target from camera
    [Header("Sounds")]
    public AudioClip engine1;
    public AudioClip engine2;
    public AudioClip squeak1;
    public AudioClip squeak2;
    public AudioClip tasted;


    //Singleton
    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<PlayerController>();
            return instance;
        }
    }

    private Vector3 clickPosition; // Position of mouse cursor in world coordinates
    private RaycastHit rayCameraResult; //Needed for raycast but never used
    private Vector3 mouseClickPosition; //Start drag position
    private Vector3 mouseVector; //End drag position
    private GameObject aim; //Arrow mesh
    private double aimAngle; //Angle for arrow rotation based on mouse drag
    private Quaternion aimOrientation; //Save initial rotation of arrow
    private Quaternion playerOrientation; //Save initial rotation of player
    private bool displayRefuel;

    private float fuelModifier = 33;
    private float mouseClamp = 0.3f;

    public bool firstClickFlag;

    private float tapTimer;
    [SerializeField] private float tapTimerMax;
    private AudioSource audioSource;
    private float refuelTimer;
    [SerializeField] private float refuelTimerMax;

    // Start is called before the first frame update
    void Start()
    {
        aim = GameObject.Find("Aim");
        aimOrientation = aim.transform.rotation;
        playerOrientation = transform.rotation;
        tapTimer = 0;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundFX(string strFX)
    {
        AudioClip clipFX = null;
        switch (strFX)
        {
            case "squeak1":
                clipFX = squeak1;
                break;
            case "squeak2":
                clipFX = squeak2;
                break;
            case "tasted":
                clipFX = tasted;
                break;
            default:
                clipFX = null;
                break;
        }
        audioSource.clip = clipFX;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (tapTimer > 0)
        {
            tapTimer -= Time.deltaTime;
        }
        else
        {
            tapTimer = 0;
        }

        if (refuelTimer > 0)
        {
            refuelTimer -= Time.deltaTime;
            if (refuelTimer <= 0)
            {
                refuelTimer = 0;
                if (fuel == 0) GameObject.Find("ButtonRefuel").GetComponent<ButtonContinue>().Show(3f);
                displayRefuel = true;
            }
        }
        if (fuel > 0 && displayRefuel)
        {
            GameObject.Find("ButtonRefuel").GetComponent<ButtonContinue>().Hide(3f);
            displayRefuel = false;
        }
        //Set mass with bread
        gameObject.GetComponent<Rigidbody2D>().mass = 1 + bread / maxBread;

        //Check if mouse button is pressed then start dragging
        if (Input.GetMouseButtonDown(0) && !isBusy && !isVictory && tapTimer==0)
        {
            clickPosition = cameraView.ScreenToWorldPoint(Input.mousePosition);
            clickPosition = new Vector3(clickPosition.x, clickPosition.y, 0);
            //Debug.Log("Raycasting: "+ clickPosition + "   "+transform.position);
            if ((clickPosition-transform.position).magnitude<aimRadius)
            {
                mouseClickPosition = Input.mousePosition;
                isAiming = true;
                aim.SetActive(true);
                animator.SetBool("isPreparing", true);
                logoAnimator.SetBool("isHidden", true);

                targetFuel = fuel;
                tapTimer = tapTimerMax;
                //if (!firstClickFlag) helperText.Hide(5f);
            }
        }

        //Check if mouse button is held then calculate magnitude
        if (Input.GetMouseButton(0) && isAiming)
        {
            mouseVector = mouseClickPosition - Input.mousePosition;
            float mX = mouseVector.x / Screen.width;
            float mY = mouseVector.y / Screen.width; // Screen.height;
            
            mouseVector = new Vector3(mX, mY, 0);
            mouseVector = Vector3.ClampMagnitude(mouseVector, mouseClamp);
            targetFuel = fuel - mouseVector.magnitude * fuelModifier;

            aim.transform.position = new Vector3(transform.position.x, transform.position.y, -1);

            aimAngle = Math.Atan2(mouseVector.x , mouseVector.y) / Math.PI * 180;
            aim.transform.rotation = aimOrientation;
            aim.transform.Rotate(Vector3.up, (float)aimAngle, Space.Self);
            aim.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f + mouseVector.magnitude * 10);

            if (mouseVector.magnitude>0.05f)
            {
                transform.rotation = playerOrientation;
                transform.Rotate(Vector3.back, (float)aimAngle, Space.Self);
            }
        }
        else
        {
            //Hide arrow
            aim.SetActive(false);
        }

        //Check if mouse button is released then stop dragging and launch
        if ((Input.GetMouseButtonUp(0)||isInterrupted) && isAiming)
        {
            firstClickFlag = true;
            isInterrupted = false;
            animator.SetBool("isPreparing", false);
            if (fuel == 0)
            {
                animator.SetBool("NoFuel", true);
            }
            else
            {
                animator.SetBool("NoFuel", false);
            }


            if (fuel == 0)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector3(mouseVector.normalized.x * 0.3f, mouseVector.normalized.y * 0.3f, 0), ForceMode2D.Impulse);
                switch (UnityEngine.Random.Range((int)0, (int)2))
                {
                    case 0:
                        PlaySoundFX("squeak1");
                        break;
                    case 1:
                        PlaySoundFX("squeak2");
                        break;
                }
            }
            else
            {
                if (targetFuel < 0)
                {
                    mouseVector = Vector3.ClampMagnitude(mouseVector, fuel / fuelModifier);
                    targetFuel = 0;
                }
                GetComponent<Rigidbody2D>().AddForce(new Vector3(mouseVector.x * 10, mouseVector.y * 10, 0), ForceMode2D.Impulse);
                switch (UnityEngine.Random.Range((int)0, (int)2))
                {
                    case 0:
                        GetComponent<AudioSource>().clip = engine1;
                        GetComponent<AudioSource>().Play();
                        break;
                    case 1:
                        GetComponent<AudioSource>().clip = engine2;
                        GetComponent<AudioSource>().Play();
                        break;
                }

            }
            if (fuel > 0 && targetFuel <= 0) refuelTimer = refuelTimerMax;
            fuel = targetFuel;
            if (fuel < 0) fuel = 0;
            isAiming = false;
        }

        //Landing on base
        if (isBusy)
        {
            animator.SetBool("isBusy", true);

            //Check timer
            if (busyTimer > 0)
            {
                busyTimer -= Time.deltaTime;
                if (busyTimer <= 0)
                {
                    isBusy = false;
                    
                    busyTimer = 0;
                }
            }
        }
        else
        {
            if (!isTeleported && !isTasted) animator.SetBool("isBusy", false);
        }

        //Teleporting
        if (isTeleported)
        {
            animator.SetBool("isBusy", true);

            //Check timer
            if (teleTimer > 0)
            {
                teleTimer -= Time.deltaTime;
                if (teleTimer <= 0)
                {
                    isTeleported = false;
                    GetComponent<Rigidbody2D>().AddForce(pushDirection * 3f, ForceMode2D.Impulse);
                    teleTimer = 0;
                }
            }
        }
        else
        {
            if (!isBusy && !isTasted) animator.SetBool("isBusy", false);
        }

        //Death
        if (isTasted)
        {
            animator.SetBool("isBusy", true);
        }

        //CheckBanner();
    }

    private void CheckBanner()
    {
        if (transform.position.y > 7)
        {
            adsInitializer.BannerDown();
        }
        if (transform.position.y < -7)
        {
            adsInitializer.BannerUp();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
}
