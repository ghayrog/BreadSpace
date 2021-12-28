using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour
{
    public bool isOn;
    public bool isHit;
    private Animator anim;
    private float hitTimer;
    //Singleton
    private static Aura instance;
    public static Aura Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<Aura>();
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;
            if (hitTimer <= 0)
            {
                isHit = false;
                hitTimer = 0;
            }
        }
        anim.SetBool("isOn", isOn);
        anim.SetBool("isHit", isHit);
        if (isHit && hitTimer==0) hitTimer = 0.2f;
    }


}
