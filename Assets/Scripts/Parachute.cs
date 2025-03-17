using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parachute : MonoBehaviour
{
    public Soldier soldier;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Bullet"))
        {
            soldier.parachute = false;
            soldier.animator.SetBool("falling", true);
            Destroy(gameObject);
        }

        if(other.gameObject.CompareTag("Stop") && soldier.stop)
        {
            Destroy(gameObject);
        }

    }
}
