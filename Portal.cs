using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject targetObject;
    public float magnetDistance = 1;
    public bool isDisabled = false;
    public float disTimer = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (disTimer > 0)
        {
            disTimer -= Time.deltaTime;
            if (disTimer <= 0)
            {
                //Enable
                isDisabled = false;
                disTimer = 0;
            }
        }
    }

    void FixedUpdate()
    {
        //Magnet
        Vector3 magnetDirection = transform.position - PlayerController.Instance.transform.position;
        if ((magnetDirection.magnitude < magnetDistance) && (isDisabled==false))
        {
            if (PlayerController.Instance.isTeleported==false)
            {
                PlayerController.Instance.isTeleported = true;
                PlayerController.Instance.teleTimer = 2f;
            }
            PlayerController.Instance.GetComponent<Rigidbody2D>().AddForce(magnetDirection.normalized * 0.1f, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject == PlayerController.Instance.gameObject) && (isDisabled==false))
        {
            targetObject.GetComponent<Portal>().isDisabled = true;
            targetObject.GetComponent<Portal>().disTimer = 4f;
            PlayerController.Instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            PlayerController.Instance.transform.position = targetObject.transform.position;
            Vector3 forceDirection = transform.position.normalized;
            PlayerController.Instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            PlayerController.Instance.pushDirection = forceDirection;
            SoundFX.Instance.PlaySoundFX("teleport");
        }
    }
}
