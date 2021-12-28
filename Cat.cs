using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public Animator animator;
    public int catState = 0; //0-inactive 1-patrol 2-chase 3-lastfight
    public float waitTime = 1.7f;
    public float catTimer;
    public float catSpeed = 5f;
    public float patrolTimer;
    public float patrolMaxTime;
    public GameObject baseObject;
    public float chaseRadius = 6;
    public int catHitpoints = 3;
    [Header("Sounds")]
    public AudioClip catHit;
    public AudioClip catAppear;
    public AudioClip catChasePlayer;
    public AudioClip catLostPlayer;

    private float shootTimer;
    private GameObject catCenter;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        float r = 20;
        float angle = Random.Range(0f, Mathf.PI*2f);
        transform.position = new Vector3(r*Mathf.Cos(angle),r*Mathf.Sin(angle),transform.position.z);
        catCenter = GameObject.Find("Cat_Center");
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundFX(string strFX)
    {
        AudioClip clipFX = null;
        switch (strFX)
        {
            case "hit":
                clipFX = catHit;
                break;
            case "appear":
                clipFX = catAppear;
                break;
            case "chase":
                clipFX = catChasePlayer;
                break;
            case "lost":
                clipFX = catLostPlayer;
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
        //Timer for movement
        if (catTimer > 0)
        {
            catTimer -= Time.deltaTime;
            if (catTimer <= 0) catTimer = 0;
        }

        //Timer for patrol
        if (patrolTimer > 0)
        {
            patrolTimer -= Time.deltaTime;
            if (patrolTimer <= 0) patrolTimer = 0;
        }

        //Timer for base recharge
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0) shootTimer = 0;
        }


        //Magnet
        Vector3 centerPosition = catCenter.transform.position;
        Vector3 magnetDirection = centerPosition - PlayerController.Instance.transform.position;
        Vector3 toBase = baseObject.transform.position - centerPosition;

        if (PlayerController.Instance.isTasted)
        {
            if (magnetDirection.magnitude > 0.01f) PlayerController.Instance.transform.position += magnetDirection.normalized * 3f * Time.deltaTime;
        }

        //Chasing player rotate
        if (catState == 2)
        {
            if (!PlayerController.Instance.isTasted)
            {
                Vector3 relativePos = PlayerController.Instance.transform.position - centerPosition;
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, relativePos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * catSpeed * 3);
            }
            if (toBase.magnitude < 2)
            {
                catState = 1;
                PlaySoundFX("lost");
            }

            if (PlayerController.Instance.isBusy || magnetDirection.magnitude>9 || PlayerController.Instance.isTasted)
            {
                catState = 1;
                PlaySoundFX("lost");
            }
        }

        //Patrol
        if (catState == 1)
        {
            if (toBase.magnitude > 7)
            {
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, toBase);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * catSpeed * 3);
            }
            else if (toBase.magnitude < 5)
            {
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, -toBase);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * catSpeed * 3);
            }
            else
            {
                if (patrolTimer == 0)
                {
                    Vector3 tangent = new Vector3(-toBase.y, toBase.x, toBase.z);
                    if (Random.Range((int)0, (int)2) == 0)
                    {
                        tangent = -tangent;
                    }
                    Quaternion rotation = Quaternion.LookRotation(Vector3.forward, tangent);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * catSpeed * 3);
                }
            }
            Vector3 playerToBase = PlayerController.Instance.transform.position - baseObject.transform.position;
            if (magnetDirection.magnitude < chaseRadius && !PlayerController.Instance.isBusy && !PlayerController.Instance.isTasted && toBase.magnitude > 2 && playerToBase.magnitude>2)
            {
                catState = 2;
                PlaySoundFX("chase");
            }

        }

        //Last Fight
        if (catState == 3)
        {
            //Speed is proportional to distance
            catSpeed = 10+(9 - toBase.magnitude)*2;
            if (catSpeed < 10) catSpeed = 10;

            if (transform.position.x < -9)
            {
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, Vector3.right);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * catSpeed * 3);
            }
            else if (transform.position.x > 9)
            {
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, Vector3.left);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * catSpeed * 3);
            }
            else if (transform.position.y < -9)
            {
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * catSpeed * 3);
            }
            else if (transform.position.y > 9)
            {
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, Vector3.down);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * catSpeed * 3);
            }
            else if (toBase.magnitude < 4.2f)
            {
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, -toBase);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * catSpeed * 3);
            }
            else
            {
                if (!PlayerController.Instance.isTasted && magnetDirection.magnitude<4.5f)
                {

                    Vector3 relativePos = PlayerController.Instance.transform.position - centerPosition;
                    Quaternion rotation = Quaternion.LookRotation(Vector3.forward, relativePos);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * catSpeed * 3);
                }
            }
        }


        //Prepare to fly 1 sec
        if (catTimer > 0 && catTimer < 1)
        {
            animator.SetBool("isPreparing", true);
        }

        //Finish fly 0.8 sec
        if (catTimer < waitTime-0.8)
        { 
            animator.SetBool("isFlying", false );
        }
    }

    void FixedUpdate()
    {
        //Magnet
        Vector3 centerPosition = catCenter.transform.position;
        Vector3 magnetDirection = centerPosition - PlayerController.Instance.transform.position;

        //Patrol
        if (catState == 1)
        {
            float currentAngle = transform.rotation.eulerAngles.z + 90;// / Mathf.PI * 180;
            if (currentAngle > 360) currentAngle -= 360;
            if (catTimer == 0)
            {
                catTimer = waitTime;
                animator.SetBool("isPreparing", false);
                animator.SetBool("isFlying", true);
                GetComponent<Rigidbody2D>().AddForce(new Vector3(Mathf.Cos((currentAngle) / 180 * Mathf.PI), Mathf.Sin((currentAngle) / 180 * Mathf.PI), 0) * catSpeed/2, ForceMode2D.Impulse);
            }
        }


        //Chasing player
        if (catState == 2)
        {
            float currentAngle = transform.rotation.eulerAngles.z + 90;// / Mathf.PI * 180;
            if (currentAngle > 360) currentAngle -= 360;
            if (catTimer ==0)
            {
                catTimer = waitTime;
                animator.SetBool("isPreparing", false);
                animator.SetBool("isFlying", true);
                if (magnetDirection.magnitude > 1f)
                {
                    GetComponent<Rigidbody2D>().AddForce(new Vector3(Mathf.Cos((currentAngle) / 180 * Mathf.PI), Mathf.Sin((currentAngle) / 180 * Mathf.PI), 0)*catSpeed, ForceMode2D.Impulse);
                }
            }
        }

        //Last Fight
        if (catState == 3)
        {
            float currentAngle = transform.rotation.eulerAngles.z + 90;// / Mathf.PI * 180;
            if (currentAngle > 360) currentAngle -= 360;
            if (catTimer == 0)
            {
                catTimer = waitTime;
                animator.SetBool("isPreparing", false);
                animator.SetBool("isFlying", true);
                GetComponent<Rigidbody2D>().AddForce(new Vector3(Mathf.Cos((currentAngle) / 180 * Mathf.PI), Mathf.Sin((currentAngle) / 180 * Mathf.PI), 0) * catSpeed / 2, ForceMode2D.Impulse);
            }

            if ((baseObject.transform.position - centerPosition).magnitude < 3 && shootTimer==0 && !PlayerController.Instance.isTasted)
            {

                catHitpoints -= 1;
                PlaySoundFX("hit");
                Aura.Instance.isHit = true;
                GetComponent<AudioSource>().clip = catHit;
                GetComponent<AudioSource>().Play();
                shootTimer = 10f;
                if (catHitpoints > 0)
                {
                    if ((baseObject.transform.position - centerPosition).magnitude > 0.1f)
                    {
                        GetComponent<Rigidbody2D>().AddForce((transform.position - baseObject.transform.position).normalized * 40, ForceMode2D.Impulse);
                        GetComponent<Rigidbody2D>().AddTorque(200f);
                        //Debug.Log("Push1");
                        ShakeCam.Instance.ShakeCamera(2f, 0.3f);
                    }
                    else
                    {
                        GetComponent<Rigidbody2D>().AddForce(Vector3.right.normalized * 40, ForceMode2D.Impulse);
                        GetComponent<Rigidbody2D>().AddTorque(200f);
                        //Debug.Log("Push2");
                        ShakeCam.Instance.ShakeCamera(2f, 0.3f);
                    }
                }
                else
                {
                    PlayerController.Instance.isVictory = true;
                    catState = 0;
                    GameObject.Find("WinText").GetComponent<WinText>().StartMoving();
                    GameObject.Find("HelpText").GetComponent<HelpText>().showHelpMsgLocale("Win", 10f);
                    GameManager.Instance.showCredits();
                    GameObject.Find("ButtonDonate").GetComponent<ButtonContinue>().Show(3f);
                    GameObject.Find("ButtonRestart").GetComponent<ButtonContinue>().Show(3f);
                    GameObject.Find("ButtonRefuel").GetComponent<ButtonContinue>().Hide(3f);
                    if ((baseObject.transform.position - transform.position).magnitude > 0.1f)
                    {
                        GetComponent<Rigidbody2D>().AddForce((transform.position - baseObject.transform.position).normalized * 100, ForceMode2D.Impulse);
                        GetComponent<Rigidbody2D>().AddTorque(1000f);
                        ShakeCam.Instance.ShakeCamera(2f, 0.3f);
                    }
                    else
                    {
                        GetComponent<Rigidbody2D>().AddForce(Vector3.right.normalized * 100, ForceMode2D.Impulse);
                        GetComponent<Rigidbody2D>().AddTorque(1000f);
                        ShakeCam.Instance.ShakeCamera(2f, 0.3f);
                    }

                }
            }
        }


        //Patrol Timer
        if (patrolTimer == 0)
        {
            patrolTimer = patrolMaxTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerController.Instance.gameObject && !PlayerController.Instance.isBusy && !PlayerController.Instance.isTeleported && !PlayerController.Instance.isVictory)
        {
            //Taste player
            SoundFX.Instance.PlaySoundFX("tasted");
            PlaySoundFX("chase");
            PlayerController.Instance.PlaySoundFX("tasted");
            ShakeCam.Instance.ShakeCamera(2f, 1.2f);
            GetComponent<Animator>().Play("Cat_open");
            catTimer = waitTime*5;
            PlayerController.Instance.isTasted = true;
            PlayerController.Instance.GetComponent<CapsuleCollider2D>().enabled = false;
            GameObject.Find("ButtonRefuel").GetComponent<ButtonContinue>().Hide(3f);
            GameObject.Find("HelpText").GetComponent<HelpText>().Hide(3f);
            GameObject.Find("Tasted").GetComponent<ImageFader>().Show(3f);
            GameObject.Find("ButtonContinue").GetComponent<ButtonContinue>().Show(3f);
            GameObject.Find("ButtonRestart").GetComponent<ButtonContinue>().Show(3f);
        }
    }
}
