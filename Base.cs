using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer base1;
    public SpriteRenderer base2;
    public SpriteRenderer base3;
    public SpriteRenderer base4;
    private float targetAlpha1 = 1;
    private float targetAlpha2 = 0;
    private float targetAlpha3 = 0;
    private float targetAlpha4 = 0;
    private float fadeSpeed = 2f;

    void Start()
    {
        
    }

    public void SetTargetAlpha(float a1, float a2, float a3, float a4)
    {
        targetAlpha1 = a1;
        targetAlpha2 = a2;
        targetAlpha3 = a3;
        targetAlpha4 = a4;
    }
    private void Update()
    {
        Color imageColor = base1.color;
        if (imageColor.a > targetAlpha1)
        {
            imageColor.a -= Time.deltaTime * fadeSpeed;
            if (imageColor.a < targetAlpha1) imageColor.a = targetAlpha1;
        }
        if (imageColor.a < targetAlpha1)
        {
            imageColor.a += Time.deltaTime * fadeSpeed;
            if (imageColor.a > targetAlpha1) imageColor.a = targetAlpha1;
        }
        base1.color = imageColor;

        imageColor = base2.color;
        if (imageColor.a > targetAlpha2)
        {
            imageColor.a -= Time.deltaTime * fadeSpeed;
            if (imageColor.a < targetAlpha2) imageColor.a = targetAlpha2;
        }
        if (imageColor.a < targetAlpha2)
        {
            imageColor.a += Time.deltaTime * fadeSpeed;
            if (imageColor.a > targetAlpha2) imageColor.a = targetAlpha2;
        }
        base2.color = imageColor;

        imageColor = base3.color;
        if (imageColor.a > targetAlpha3)
        {
            imageColor.a -= Time.deltaTime * fadeSpeed;
            if (imageColor.a < targetAlpha3) imageColor.a = targetAlpha3;
        }
        if (imageColor.a < targetAlpha3)
        {
            imageColor.a += Time.deltaTime * fadeSpeed;
            if (imageColor.a > targetAlpha3) imageColor.a = targetAlpha3;
        }
        base3.color = imageColor;

        imageColor = base4.color;
        if (imageColor.a > targetAlpha4)
        {
            imageColor.a -= Time.deltaTime * fadeSpeed;
            if (imageColor.a < targetAlpha4) imageColor.a = targetAlpha4;
        }
        if (imageColor.a < targetAlpha4)
        {
            imageColor.a += Time.deltaTime * fadeSpeed;
            if (imageColor.a > targetAlpha4) imageColor.a = targetAlpha4;
        }
        base4.color = imageColor;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Magnet
        Vector3 magnetDirection = transform.position - PlayerController.Instance.transform.position;
        if (PlayerController.Instance.isBusy)
        {
            PlayerController.Instance.GetComponent<Rigidbody2D>().AddForce(magnetDirection.normalized * 0.1f, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject == PlayerController.Instance.gameObject) && (PlayerController.Instance.bread > 0 || PlayerController.Instance.fuel== 0))
        {
            SoundFX.Instance.PlaySoundFX("tasted");
            PlayerController.Instance.fuel = PlayerController.Instance.maxFuel;
            PlayerController.Instance.targetFuel = PlayerController.Instance.maxFuel;
            PlayerController.Instance.score += (int)(PlayerController.Instance.bread*10);
            PlayerController.Instance.bread = 0;
            PlayerController.Instance.isBusy = true;
            PlayerController.Instance.busyTimer = 3f;
        }
    }
}
