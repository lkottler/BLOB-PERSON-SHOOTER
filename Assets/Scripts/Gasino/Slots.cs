using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slots : MonoBehaviour
{

    public float[] minMaxSpeed = { 1f, 3f };

    public Reel[] reel;
    bool startSpin = false;

    // Update is called once per frame

    public void ButtonPress()
    {
        startSpin = true;
    }
    void Update()
    {
        if (!startSpin)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startSpin = true;
                StartCoroutine(Spinning());
            }
        }
    }

    IEnumerator Spinning()
    {
        foreach (Reel spinner in reel)
        {
            spinner.spin = true;
        }

        for (int i = 0; i < reel.Length; i++)
        {
            yield return new WaitForSeconds(Random.Range(minMaxSpeed[0], minMaxSpeed[1]));
            reel[i].spin = false;
            reel[i].RandomPosition();
        }

        startSpin = false;
    }

}
