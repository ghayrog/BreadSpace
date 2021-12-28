using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatHealthBar : MonoBehaviour
{
    public GameObject catObject;
    public SpriteRenderer health1;
    public SpriteRenderer health2;
    public SpriteRenderer health3;
    public SpriteRenderer health4;
    public SpriteRenderer health5;

    private Vector3 shift = new Vector3(0,1.5f,0);
    private int health;
    private Color healthShow;
    private Color healthHide;
    // Start is called before the first frame update
    void Start()
    {
        healthShow = health1.color;
        healthHide = healthShow;
        healthHide.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = catObject.transform.position + shift;
        health = catObject.GetComponent<Cat>().catHitpoints;
        if (GameManager.Instance.level < 3)
        {
            health1.color = healthHide;
            health2.color = healthHide;
            health3.color = healthHide;
            health4.color = healthHide;
            health5.color = healthHide;
        }
        else
        {
            switch (health)
            {
                case 1:
                    health1.color = healthShow;
                    health2.color = healthHide;
                    health3.color = healthHide;
                    health4.color = healthHide;
                    health5.color = healthHide;
                    break;
                case 2:
                    health1.color = healthShow;
                    health2.color = healthShow;
                    health3.color = healthHide;
                    health4.color = healthHide;
                    health5.color = healthHide;
                    break;
                case 3:
                    health1.color = healthShow;
                    health2.color = healthShow;
                    health3.color = healthShow;
                    health4.color = healthHide;
                    health5.color = healthHide;
                    break;
                case 4:
                    health1.color = healthShow;
                    health2.color = healthShow;
                    health3.color = healthShow;
                    health4.color = healthShow;
                    health5.color = healthHide;
                    break;
                case 5:
                    health1.color = healthShow;
                    health2.color = healthShow;
                    health3.color = healthShow;
                    health4.color = healthShow;
                    health5.color = healthShow;
                    break;
                default:
                    health1.color = healthHide;
                    health2.color = healthHide;
                    health3.color = healthHide;
                    health4.color = healthHide;
                    health5.color = healthHide;
                    break;
            }
        }
    }
}
