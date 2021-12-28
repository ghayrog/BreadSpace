using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelScript : MonoBehaviour
{

    public float fuelVolume = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerController.Instance.gameObject)
        {
            float fuelSum = PlayerController.Instance.fuel + fuelVolume;
            if (fuelSum <= PlayerController.Instance.maxFuel) PlayerController.Instance.fuel = fuelSum; else PlayerController.Instance.fuel = PlayerController.Instance.maxFuel;
            PlayerController.Instance.targetFuel = PlayerController.Instance.fuel;
            Destroy(gameObject);
        }
    }
}