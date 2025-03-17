using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    private Transform A, D, ADown, DDown;
    public float speed, speedDownPlane;
    //[HideInInspector]
    public bool SpawnA, SpawnD, SpawnADown, SpawnDDown;
    public GameObject soldierPrefab, Remain, explosion;
    public Transform Door;
    private Bunker bunker;
    public float timeSpawn, timeSpawnDownPlane;
    private float timeReset, timeResetDownPlane;
    private Spawner spawner;

    public AudioClip audioFly;
    public AudioSource audioSource;
    public float volume;

    void Start()
    {
        spawner = GameObject.FindObjectOfType<Spawner>();
        float rand = Random.Range(0f, 0.9f);
        timeSpawn += rand;

        float rand2 = Random.Range(0f, 0.5f);
        timeResetDownPlane += rand2;

        timeReset = timeSpawn;
        timeResetDownPlane = timeSpawnDownPlane;
        A = GameObject.FindGameObjectWithTag("A").GetComponent<Transform>();
        D = GameObject.FindGameObjectWithTag("D").GetComponent<Transform>();
        ADown = GameObject.FindGameObjectWithTag("ADown").GetComponent<Transform>();
        DDown = GameObject.FindGameObjectWithTag("DDown").GetComponent<Transform>();
        bunker = GameObject.FindObjectOfType<Bunker>();
    }

    void Update()
    {
        timeSpawn -= Time.deltaTime;
        timeSpawnDownPlane -= Time.deltaTime;
        if(SpawnA)
        {
            AudioFly();
            transform.position = Vector2.MoveTowards(transform.position, D.position, speed * Time.deltaTime);
            transform.localScale = new Vector3(1,1,1);
        }else if(SpawnD){ //Spawn in D
            AudioFly();
            transform.position = Vector2.MoveTowards(transform.position, A.position, speed * Time.deltaTime);
            transform.localScale = new Vector3(-1,1,1);
        }

        if(SpawnADown)
        {
            AudioFly();
            transform.position = Vector2.MoveTowards(transform.position, DDown.position, speedDownPlane * Time.deltaTime);
            transform.localScale = new Vector3(1,1,1);
        }else if(SpawnDDown){ //Spawn in D
            AudioFly();
            transform.position = Vector2.MoveTowards(transform.position, ADown.position, speedDownPlane * Time.deltaTime);
            transform.localScale = new Vector3(-1,1,1);
        }

        if(spawner.loseGame)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            probability();
            DestroyObject();
        }
        if(collision.gameObject.CompareTag("Boom"))
        {
            DestroyObject();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Spawner"))
        {
            if(timeSpawn <=0 && (SpawnA || SpawnD))
            {
                SpawnSoldier();
                float rand = Random.Range(0f, 0.9f);
                timeReset += rand;
                timeSpawn = timeReset;
            }

            if(timeSpawnDownPlane <=0 && (SpawnADown || SpawnDDown))
            {
                SpawnSoldier();
                float rand2 = Random.Range(0f, 0.3f);
                timeResetDownPlane += rand2;
                timeSpawnDownPlane = timeResetDownPlane;
            }
        }

        if(SpawnA)
        {
            if(other.gameObject.CompareTag("D"))
            {
                Destroy(gameObject);
            }
        }else if(!SpawnA)
        {
            if(other.gameObject.CompareTag("A"))
            {
                Destroy(gameObject);
            }
        }

        if(SpawnADown)
        {
            if(other.gameObject.CompareTag("DDown"))
            {
                Destroy(gameObject);
            }
        }else if(!SpawnADown)
        {
            if(other.gameObject.CompareTag("ADown"))
            {
                Destroy(gameObject);
            }
        }
    }

    void SpawnSoldier(){
        GameObject soldier = Instantiate(soldierPrefab, Door.position, Quaternion.identity);
        soldier.transform.parent = transform.parent;
    }

    void probability()
    {
        float probabilidad = Random.value;
        if(probabilidad > 0.6f)
        {
            bunker.ammunition += 10;
        }else if(probabilidad > 0.3f )
        {
            bunker.ammunition += 8;
        }else if(probabilidad > 0)
        {
            bunker.ammunition += 5;
        }
    }

    void DestroyObject()
    {
        GameObject remain = Instantiate(Remain, transform.position, Quaternion.identity);
        bunker.KillsPlanes += 1;
        bunker.score += 2;
        bunker.UpdateText();
        GameObject explo = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
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
}
