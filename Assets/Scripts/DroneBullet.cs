using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBullet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Drone") && !collider.gameObject.CompareTag("Soldier"))
        {
            Destroy(gameObject);
        }
    }
}
