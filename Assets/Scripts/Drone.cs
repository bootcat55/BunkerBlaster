using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Drone : MonoBehaviour
{

    public float yMin, yMax;
    private float xMin, xMax;
    private float xPos, yPos;
    private Vector3 droneVelocity = Vector3.zero;

    private Bunker tank;

    public GameObject bulletPrefab, explosion;
    public float timeDestroyBullet;
    public float velocity;

    public AudioClip audioBullet;
    public AudioSource audioSource;
    public float volume;

    public float shoot_cooldown;
    private float cooldownTimestamp;

    private Spawner spawner;


    void Start()
    { 
        xMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10)).x;
        xMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 10)).x;
        xPos = Random.Range(xMin, xMax);
        yPos = Random.Range(yMin, yMax);

        tank = GameObject.FindObjectOfType<Bunker>();
        spawner = GameObject.FindObjectOfType<Spawner>();
    }

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(xPos, yPos, 0), ref droneVelocity, Random.Range(1f, 2f)); //current pos to target pos in a smooth movement
        if(Mathf.Abs(xPos - transform.position.x) <= 0.1 && Mathf.Abs(yPos - transform.position.y) <= 0.1)
        {
            droneVelocity = Vector3.zero;
            xPos = Random.Range(xMin, xMax);
            yPos = Random.Range(yMin, yMax);
        }

        if (Time.time >= cooldownTimestamp) //shoot if cooldown finished
        {
            Shoot();
            cooldownTimestamp = Time.time + shoot_cooldown;
        }

        if (spawner.loseGame)
        {
            Destroy(gameObject);
        }


    }

    void Shoot()
    {
        AudioBullet();
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(tank.transform.position.x - transform.position.x, tank.transform.position.y - transform.position.y) * velocity;
        Destroy(bullet, timeDestroyBullet);
    }

    public void AudioBullet()
    {
        // if (!audioSource.isPlaying)
        //{
        audioSource.PlayOneShot(audioBullet, volume);
        //}
    }

    void probability()
    {
        float probabilidad = Random.value;
        if (probabilidad > 0.6f)
        {
            tank.ammunition += 10;
        }
        else if (probabilidad > 0.3f)
        {
            tank.ammunition += 8;
        }
        else if (probabilidad > 0)
        {
            tank.ammunition += 5;
        }
    }

    void DestroyObject()
    {
        tank.KillsPlanes += 1;
        tank.score += 2;
        tank.UpdateText();
        GameObject explo = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
           probability();
           DestroyObject();            
        }
        if (collision.gameObject.CompareTag("Boom"))
        {
            Destroy(gameObject);
        }
    }
}
