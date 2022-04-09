using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reel : MonoBehaviour
{

    public bool spin = false;

    public int speed = 1500;

    private int maxHeight = 600;

    // Update is called once per frame
    void Update()
    {
        if (spin)
        {
            foreach(Transform image in transform)
            {
                image.transform.Translate(Vector3.down * Time.smoothDeltaTime * speed, Space.World);

                if (image.transform.position.y <= 0)
                {
                    image.transform.position = new Vector3(image.transform.position.x,
                        image.transform.position.y + maxHeight, image.transform.position.z);
                }
            }
        }
    }

    public void RandomPosition()
    {
        List<int> parts = new List<int>();

        for(int i = 0; i < 6; i++)
        {
            parts.Add(i * 100 - 300);
            Debug.Log(i * 100 - 300);
        }

        foreach (Transform image in transform)
        {
            int rand = Random.Range(0, parts.Count);

            image.transform.position = new Vector3(
                image.transform.position.x,
                parts[rand] + transform.parent.GetComponent<RectTransform>().transform.position.y,
                image.transform.position.z);

            parts.RemoveAt(rand);
        }
    }

}
