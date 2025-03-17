using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThis : MonoBehaviour
{
    public float timeDestroy;
    private GameObject destroy;
    
    void Update()
    {
        Destroy(gameObject, timeDestroy);
    }
}
