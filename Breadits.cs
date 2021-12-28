using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breadits : MonoBehaviour
{
    public int breadits;

    private UpdateUI updateUI;
    // Start is called before the first frame update
    void Start()
    {
        updateUI = GameObject.Find("Canvas").GetComponent<UpdateUI>();
    }

    // Update is called once per frame
    void Update()
    {
        breadits = (int)updateUI.displayedScore;
    }
}
