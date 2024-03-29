using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public float FireDamageTime = 1f;
    private float timer;
    public int FireDamage = 20;
    void Start()
    {
        timer = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        timer += Time.deltaTime;
        
        Character character = other.GetComponent<Character>();
        if (timer >= FireDamageTime && character != null)
        {
            character.Damaged(FireDamage);
            timer = 0;
        }
    }

}
