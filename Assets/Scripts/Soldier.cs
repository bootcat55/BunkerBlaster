using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    private Bunker bunker;
    public GameObject ParticleDead;
    [HideInInspector]
    public bool parachute = true;
    private Rigidbody2D rb2D;
    [HideInInspector]
    public bool stop;
    public Animator animator;
    public float speedLose;
    private Spawner spawner;

    void Start()
    {
        spawner = GameObject.FindObjectOfType<Spawner>();
        bunker = GameObject.FindObjectOfType<Bunker>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        
        if(transform.position.x > bunker.BunkerTransform().position.x)
        {
            transform.localScale = new Vector3(-1,1,1);
        }

        if(!parachute)
        {
            rb2D.gravityScale = 1f;
        }

        if(spawner.loseGame)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Boom"))
        {
            Dead();
        }

        if(collision.gameObject.CompareTag("Bullet"))
        {
            probability();
            Dead();
        }

        if(collision.gameObject.CompareTag("Ground") && !parachute)
        {
            probability();
            Dead();
        }

        if(collision.gameObject.CompareTag("Ovni"))
        {
            Dead();
        }

        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Stop") && parachute)
        {
            stop = true;
            rb2D.gravityScale = 1f;
            animator.SetBool("idle", true);
        }

        if(other.gameObject.CompareTag("Parachute"))
        {
            animator.SetBool("parachute", true);
            //rb2D.gravityScale = 1;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("ray"))
        {
            rb2D.gravityScale = 0;
            transform.position = Vector2.MoveTowards(transform.position, other.GetComponent<Transform>().parent.position, 0.5f * Time.deltaTime);
        }
    }

    void probability()
    {
        float probabilidad = Random.value;
        if(probabilidad > 0.6f)
        {
            bunker.ammunition += 3;
        }else if(probabilidad > 0.3f )
        {
            bunker.ammunition += 2;
        }else if(probabilidad > 0)
        {
            bunker.ammunition += 1;
        }
    }

    public void Dead()
    {
        bunker.KillsSoldier += 1;
        bunker.score += 1;
        bunker.UpdateText();
        GameObject bullet = Instantiate(ParticleDead, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void lose()
    {
        animator.SetBool("walk", true);

        if(transform.position.x > bunker.BunkerTransform().position.x)
        {
            transform.localScale = new Vector3(-1,1,1);
        }

        transform.position = Vector2.MoveTowards(transform.position, bunker.BunkerTransform().position, speedLose * Time.deltaTime);
        if(transform.position == bunker.BunkerTransform().position)
        {
            Destroy(gameObject);
        }
    }
}
