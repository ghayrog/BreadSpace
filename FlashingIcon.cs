using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingIcon : MonoBehaviour
{
    private bool isFlashing;
    // Start is called before the first frame update
    void Start()
    {
        isFlashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Instance.fuel <= 0.25 * PlayerController.Instance.maxFuel || PlayerController.Instance.bread == PlayerController.Instance.maxBread)
        {
            if (!isFlashing)
            {
                GetComponent<Animator>().SetBool("isFlashing", true);
            }
            isFlashing = true;
        }
        else
        {
            if (isFlashing)
            {
                GetComponent<Animator>().SetBool("isFlashing", false);
            }
            isFlashing = false;
        }
    }
}
