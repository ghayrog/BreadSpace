using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadComet : MonoBehaviour
{
    public float x0;
    public float y0;
    public float xScale;
    public float yScale;
    public float speed;
    public float angle;
    public float shiftAngle;
    public GameObject breadPrefab;
    public AudioClip hit1;
    public AudioClip hit2;
    public AudioClip hit3;
    // Start is called before the first frame update
    void Start()
    {
        //radius += Random.Range(-1f, 1f);
        //speed = Random.Range(-1f, 1f);
        //angle = Random.Range(0f, 2 * Mathf.PI);
    }

    private void Awake()
    {
        x0 = Random.Range((int)-1,(int)2)*10;
        y0 = Random.Range((int)-1, (int)2) * 10;
        if (x0 == 0 && y0 == 0) x0 = 10;
        if (x0 != 0 && y0 != 0) x0 = 0;
        xScale = Random.Range(8f,12f);
        yScale = Random.Range(2f, 7f);
        speed = Random.Range(0.5f,1.5f)*(Random.Range((int)0, (int)2)*2-1);
        if (x0 == 0)
        {
            if (y0 == 10) angle = 90; else angle = 270;
        }
        else
        {
            if (x0 == 10) angle = 0; else angle = 180;
        }
        shiftAngle = Random.Range(-Mathf.PI / 4, Mathf.PI/4);
    }
    // Update is called once per frame
    void Update()
    {
        angle += speed * Time.deltaTime;
        if (angle > 2 * Mathf.PI) angle -= 2 * Mathf.PI;
        float x1 = xScale * Mathf.Cos(angle);
        float y1 = yScale * Mathf.Sin(angle);
        float x2 = x1 * Mathf.Cos(shiftAngle) + y1 * Mathf.Sin(shiftAngle);
        float y2 = -x1 * Mathf.Sin(shiftAngle) + y1 * Mathf.Cos(shiftAngle);
        transform.position = new Vector3(x2 + x0, y2 + y0, 0);

    }

    //Destroys bread and created breadcrumbs
    public void DestroyBread()
    {
        int breadNumber = Random.Range(10, 15);
        float breadAngle;
        float breadRadius = 1.5f;
        float breadForce = 0.5f;
        GameObject breadcrumb;

        for (int i = 0; i < breadNumber; i++)
        {
            breadAngle = Random.Range(0f, 2 * Mathf.PI);
            breadRadius += Random.Range(-0.25f, 0.25f);
            Vector3 breadVector = new Vector3(breadRadius * Mathf.Cos(breadAngle), breadRadius * Mathf.Sin(breadAngle), transform.position.z);
            breadcrumb = Instantiate(breadPrefab, transform.position + breadVector, Quaternion.identity);
            breadcrumb.transform.parent = GameObject.Find("Breadcrumbs").transform;
            breadcrumb.GetComponent<Rigidbody2D>().AddForce(breadVector.normalized * breadForce*Random.Range(1f, 2f), ForceMode2D.Impulse);
        }
        GameManager.Instance.breadTimer = Random.Range(5f, 20f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        switch (Random.Range((int)0, (int)3))
        {
            case 0:
                GetComponent<AudioSource>().clip = hit1;
                GetComponent<AudioSource>().Play();
                break;
            case 1:
                GetComponent<AudioSource>().clip = hit2;
                GetComponent<AudioSource>().Play();
                break;
            case 2:
                GetComponent<AudioSource>().clip = hit3;
                GetComponent<AudioSource>().Play();
                break;
        }
        Destroy(gameObject,2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        //Collide with player
        if (collision.gameObject == PlayerController.Instance.gameObject)
        {
            PlayerController.Instance.isInterrupted = true;
            PlayerController.Instance.GetComponent<Rigidbody2D>().AddForce((PlayerController.Instance.transform.position - transform.position).normalized * 3, ForceMode2D.Impulse);

            ShakeCam.Instance.ShakeCamera(2f, 0.3f);
            DestroyBread();
        }

        //Collide with planet
        Planet objPlanet = collision.gameObject.GetComponent<Planet>();
        if (objPlanet != null)
        {
            ShakeCam.Instance.ShakeCamera(2f, 0.3f);
            DestroyBread();
        }

        //Collide with base
        //Base objBase = collision.gameObject.GetComponent<Base>();
        if (collision.gameObject.name=="BaseRigidbodyCollider")
        {
            ShakeCam.Instance.ShakeCamera(2f, 0.3f);
            DestroyBread();
        }

    }
}

