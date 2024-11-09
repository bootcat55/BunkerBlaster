using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private Bunker bunker;
    private Transform A, D;
    public float speed;
    [HideInInspector]
    public bool SpawnA, falling;
    public float timeSpawn;
    private Rigidbody2D rb2D;
    private float rotationMissile;
    private Spawner spawner;
    public GameObject Fire, explosion;

    public AudioClip audioFly, audioFalling;
    public AudioSource audioSource;
    public float volume;

    private void Start() {
        bunker = GameObject.FindObjectOfType<Bunker>();
        spawner = GameObject.FindObjectOfType<Spawner>();
        rb2D = GetComponent<Rigidbody2D>();
        float rand = Random.Range(0f, 5f);
        timeSpawn += rand;
        A = GameObject.FindGameObjectWithTag("A").GetComponent<Transform>();
        D = GameObject.FindGameObjectWithTag("D").GetComponent<Transform>();
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            GameObject.FindObjectOfType<Spawner>().LoseMissile();
            Destroy(gameObject);
        }

        if(collision.gameObject.CompareTag("Bullet"))
        {
            Dead();
        }
    }

    public void Dead()
    {
        bunker.KillsPlanes += 1;
        bunker.score += 3;
        bunker.UpdateText();
        GameObject explo = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void Update()
    {
        timeSpawn -= Time.deltaTime;
        if(!falling)
        {
            if(SpawnA)
            {
                transform.position = Vector2.MoveTowards(transform.position, D.position, speed * Time.deltaTime);
                transform.localScale = new Vector3(1,1,1);
                AudioFly();
            }else{ //Spawn in D
                transform.position = Vector2.MoveTowards(transform.position, A.position, speed * Time.deltaTime);
                transform.localScale = new Vector3(-1,1,1);
                AudioFly();
            }
        }

        if(spawner.loseGame)
        {
            Destroy(gameObject);
        }
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Spawner"))
        {
            if(timeSpawn <= 0.4f)
            {
                Fire.SetActive(false);
            }

            if(timeSpawn <=0)
            {
                falling = true;
                Debug.Log("Caer");
                AudioFalling();
                StartCoroutine(rotateMissile());
            }
        }
    }

    IEnumerator rotateMissile()
    {
        yield return new WaitForSeconds(0.5f);
        rb2D.gravityScale = 0.05f;
        if(SpawnA)
        {
            if(rotationMissile > -90f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rotationMissile--));
            }
        }else{
            if(rotationMissile < 90f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rotationMissile++));
            }
        }
        yield return new WaitForSeconds(0.5f);
    }

    public void AudioFly()
    {
        if (!audioSource.isPlaying && Time.timeScale == 1)
        {
            audioSource.PlayOneShot(audioFly, volume);
        }

        if(Time.timeScale == 0)
        {
            audioSource.mute = true;
        }else{
            audioSource.mute = false;
        }
    }

    public void AudioFalling()
    {
        if (!audioSource.isPlaying && Time.timeScale == 1)
        {
            audioSource.PlayOneShot(audioFalling, volume);
        }

        if(Time.timeScale == 0)
        {
            audioSource.mute = true;
        }else{
            audioSource.mute = false;
        }
    }
}
