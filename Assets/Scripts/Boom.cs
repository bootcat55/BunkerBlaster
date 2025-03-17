using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public float timeDestroyBoom, timeCollision;
    private GameObject DestroyAll;
    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;

    private void Start() {
        DestroyAll = GameObject.Find("DestroyAll");
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float angle = Mathf.Atan2(rb2D.velocity.y, rb2D.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        

        timeDestroyBoom -= Time.deltaTime;
        if(timeDestroyBoom <= 0)
        {
            timeCollision -= Time.deltaTime;
            DestroyAll.transform.GetChild(0).gameObject.SetActive(true);
            spriteRenderer.enabled = false;
            if(timeCollision <= 0)
            {
                DestroyAll.transform.GetChild(0).gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
