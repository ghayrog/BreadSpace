using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinText : MonoBehaviour
{
    [SerializeField] private float MovingSpeed;
    private bool isMoving;
    // Start is called before the first frame update
    void Start()
    {
        //StartMoving();
    }

    public void StartMoving()
    {
        //Debug.Log("Text movement started");
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.position += new Vector3(-Time.deltaTime*MovingSpeed, 0, 0);
            //Debug.Log("Moving text...");
        }

        if (transform.position.x < -500000) {
            StopMoving();
        }
    }
}
