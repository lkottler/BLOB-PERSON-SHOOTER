using System.Collections;
using UnityEngine;

public class LineFade : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private float width;
    [SerializeField] private float endWidth;
    [SerializeField] private float speed = 1f;

    LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //move towards zero
        width = Mathf.Lerp(width, endWidth, Time.deltaTime * speed);
        color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * speed);
        
        //update color and width
        lr.startWidth = width;
        lr.startColor = color;
        lr.endWidth = width;
        lr.endColor = color;
    }
}
