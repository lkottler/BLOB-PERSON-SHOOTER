using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifespan : MonoBehaviour
{
    [SerializeField]
    public float lifetime;
    void Start()
    {
        StartCoroutine(lifespan());
    }

    IEnumerator lifespan()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

}
