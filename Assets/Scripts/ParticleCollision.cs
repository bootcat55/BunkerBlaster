using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    void OnParticleCollision(GameObject collision)
    {
        if(collision.gameObject.CompareTag("Soldier"))
        {
            collision.GetComponent<Soldier>().Dead();
        }

        if(collision.gameObject.CompareTag("Missile"))
        {
            collision.GetComponent<Missile>().Dead();
        }
    }
}
