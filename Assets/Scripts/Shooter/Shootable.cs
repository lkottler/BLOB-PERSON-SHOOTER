using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{

    public int currentHealth = 3;
    public void Damage(int dmgAmount)
    {
        currentHealth -= dmgAmount;

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
