using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    private GameObject iconBase;
    private GameObject iconBread;
    private GameObject iconCat;
    private GameObject iconPlanet1;
    private GameObject iconPlanet2;
    private GameObject iconPlanet3;
    private GameObject objectBase;
    private GameObject objectBread;
    private GameObject objectCat;
    private GameObject objectPlanet1;
    private GameObject objectPlanet2;
    private GameObject objectPlanet3;

    private Camera cam;

    private float xBound;
    public float yBound = 5f;
    private float borderSize = 0.15f;
    // Start is called before the first frame update
    void Start()
    {
        iconBase = GameObject.Find("Icon_base");
        iconBread = GameObject.Find("Icon_bread");
        iconCat = GameObject.Find("Icon_cat");
        iconPlanet1 = GameObject.Find("Icon_planet1");
        iconPlanet2 = GameObject.Find("Icon_planet2");
        iconPlanet3 = GameObject.Find("Icon_planet3");
        objectBase = GameObject.Find("Base");
        //Bread object is dynamic!
        objectCat = GameObject.Find("Cat");
        objectPlanet1 = GameObject.Find("Planet1");
        objectPlanet2 = GameObject.Find("Planet2");
        objectPlanet3 = GameObject.Find("Planet3");

        cam = gameObject.GetComponent<Camera>();
    }

    void UpdateIcon(GameObject objObj, GameObject icoObj)
    {
        Vector3 dir = objObj.transform.position - transform.position;
        Vector3 dirPlayer = objObj.transform.position - PlayerController.Instance.transform.position;
        if (Mathf.Abs(dir.x) < xBound && Mathf.Abs(dir.y) < yBound)
        {
            Color col = icoObj.GetComponent<SpriteRenderer>().color;
            col.a = 0;
            icoObj.GetComponent<SpriteRenderer>().color = col;
        }
        else
        {
            Vector3 xClamped = new Vector3(xBound * Mathf.Sign(dir.x), dir.y * xBound / Mathf.Abs(dir.x), -transform.position.z);
            
            Vector3 xTarget = new Vector3(xClamped.x - borderSize * Mathf.Sign(xClamped.x), xClamped.y / xClamped.x * (xClamped.x - borderSize * Mathf.Sign(xClamped.x)), xClamped.z);
            Vector3 yClamped = new Vector3(dir.x * yBound / Mathf.Abs(dir.y), yBound * Mathf.Sign(dir.y), -transform.position.z);
            Vector3 yTarget = new Vector3(yClamped.x / yClamped.y * (yClamped.y - borderSize * Mathf.Sign(yClamped.y)), yClamped.y - borderSize * Mathf.Sign(yClamped.y), yClamped.z);
            if (xClamped.magnitude < yClamped.magnitude)
            {

                icoObj.transform.position = transform.position + xTarget;
            }
            else
            {
                icoObj.transform.position = transform.position + yTarget;
            }
            Color col = icoObj.GetComponent<SpriteRenderer>().color;
            col.a = 1 - Mathf.Log(Vector3.ClampMagnitude(dirPlayer, 20).magnitude - 2)/Mathf.Log(18); // (Vector3.ClampMagnitude(dirPlayer,20).magnitude-3)/17;
            icoObj.GetComponent<SpriteRenderer>().color = col;
        }
    }

    // Update is called once per frame
    void Update()
    {
        xBound = yBound * cam.aspect;
        //STATIC ICONS
        UpdateIcon(objectBase, iconBase);
        if (objectCat.GetComponent<Cat>().catState==0)
        {
            Color col = iconCat.GetComponent<SpriteRenderer>().color;
            col.a = 0;
            iconCat.GetComponent<SpriteRenderer>().color = col;
        }
        else UpdateIcon(objectCat, iconCat);

        UpdateIcon(objectPlanet1, iconPlanet1);
        UpdateIcon(objectPlanet2, iconPlanet2);
        UpdateIcon(objectPlanet3, iconPlanet3);

        //BREAD ICON
        objectBread = GameObject.FindGameObjectWithTag("Breadcomet");
        if (objectBread != null)
        {
            UpdateIcon(objectBread, iconBread);
        }
        else
        {
            Color col = iconBread.GetComponent<SpriteRenderer>().color;
            col.a = 0;
            iconBread.GetComponent<SpriteRenderer>().color = col;
        }

        //BREADCRUMBS ICONS
        GameObject[] breadcrumbs = GameObject.FindGameObjectsWithTag("Breadcrumb");
        foreach (GameObject breadcrumb in breadcrumbs)
        {
            UpdateIcon(breadcrumb, breadcrumb.GetComponent<Bread>().breadcrumbIcon);
            if (Mathf.Abs(breadcrumb.transform.position.x) > 10 || Mathf.Abs(breadcrumb.transform.position.y) > 10)
            {
                Color col = breadcrumb.GetComponent<Bread>().breadcrumbIcon.GetComponent<SpriteRenderer>().color;
                col.a = 0;
                breadcrumb.GetComponent<Bread>().breadcrumbIcon.GetComponent<SpriteRenderer>().color = col;
            }
        }
    }
}
