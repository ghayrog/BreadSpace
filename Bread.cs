using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : MonoBehaviour
{
    public float magnetDistance = 1;
    public float blinkTimer = 0;
    public GameObject breadcrumbIcon;
    // Start is called before the first frame update
    void Start()
    {
        blinkTimer = Random.Range(1f, 5f);
    }

    private void Update()
    {
        if (blinkTimer > 0)
        {
            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0)
            {
                //Play blink animation
                GetComponent<Animator>().Play("Bread_blink");
                blinkTimer = Random.Range(2f, 7f);
            }
        }
    }

    void FixedUpdate()
    {
        //Magnet
        Vector3 magnetDirection = PlayerController.Instance.transform.position - transform.position;
        if ((magnetDirection.magnitude < magnetDistance) && (PlayerController.Instance.bread < PlayerController.Instance.maxBread) && (!PlayerController.Instance.isBusy) && (!PlayerController.Instance.isTasted))
        {
            GetComponent<Rigidbody2D>().AddForce(magnetDirection.normalized * 0.01f, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject == PlayerController.Instance.gameObject) && (PlayerController.Instance.bread < PlayerController.Instance.maxBread) && (!PlayerController.Instance.isBusy))
        {
            PlayerController.Instance.bread += 1;
            Destroy(gameObject);
        }

        GameObject baseObj = GameObject.Find("Base");
        if (collision.gameObject == baseObj)
        {
            Vector3 magnetDirection = transform.position - baseObj.transform.position;
            GetComponent<Rigidbody2D>().AddForce(magnetDirection.normalized * 0.5f, ForceMode2D.Impulse);
        }
    }

}
