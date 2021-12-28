using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    public GameObject followObject;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, followObject.transform.position.y, transform.position.z);
    }
}
