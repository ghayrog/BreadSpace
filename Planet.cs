using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float radius;
    public float speed;
    public float angle;
    public AudioClip hit1;
    public AudioClip hit2;
    public AudioClip hit3;
    // Start is called before the first frame update
    void Start()
    {
        radius += Random.Range(-1f,1f);
        speed = (0.5f + Random.Range(-0.25f, 0.25f)) * (2 * Random.Range((int)0, (int)2)-1);
        angle = Random.Range(0f, 2*Mathf.PI);
    }

    // Update is called once per frame
    void Update()
    {
        angle += speed * Time.deltaTime;
        if (angle > 2 * Mathf.PI) angle -= 2 * Mathf.PI;
        transform.position = new Vector3(radius*Mathf.Cos(angle),radius*Mathf.Sin(angle),0);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == PlayerController.Instance.gameObject)
        {
            PlayerController.Instance.isInterrupted = true;
            PlayerController.Instance.GetComponent<Rigidbody2D>().AddForce((PlayerController.Instance.transform.position - transform.position).normalized*speed*10, ForceMode2D.Impulse);
            ShakeCam.Instance.ShakeCamera(2f, 0.3f);
            //SoundFX.Instance.PlaySoundFX("planet");
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
        }
    }
}
