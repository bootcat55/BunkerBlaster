using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bunker : MonoBehaviour
{
    [HideInInspector]
    public int score, KillsSoldier, KillsPlanes;
    public Text scoreText, KillsSoldierText, KillsPlanesText, ammunitionText, modeShootText;
    public Transform cannon, point;
    public GameObject bulletPrefab, losePrefab, explosion, bomPrefab;
    public Image[] Bombs;
    private Vector2 lookDirection;
    private float lookAngle;
    public float velocity, velocityBoom;
    public float time, timeBurst = 0.5f;
    private float timeReset;
    public Transform limit;
    private Spawner spawner;
    private Ovni ovni;
    private bool pause;
    public float timeDestroyBullet;
    public int numBooms, ammunition;
    private int burst = 3;
    private int ModeShoot;

    //Audio
    public AudioClip audioBullet;
    public AudioSource audioSource;
    public float volume;

    private void Start() {
        spawner = GameObject.FindObjectOfType<Spawner>();
        Cursor.visible = false;
        timeReset = time;
        UpdateText();
        enabledImageBoom();
    }
    public void enabledImageBoom()
    {
        for (int i = 0; i < Bombs.Length; i++)
        {
            Bombs[i].enabled = false;
        }
        for (int i = 0; i < numBooms; i++)
        {
            Bombs[i].enabled = true;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown("p") && Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }else if(Input.GetKeyDown("p") && Time.timeScale == 0){
            Time.timeScale = 1;
        }

        ovni = GameObject.FindObjectOfType<Ovni>();

        if(!spawner.loseGame && !pause && Time.timeScale == 1)
        {
            lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);
            lookAngle = Mathf.Atan2(cannon.position.y - lookDirection.y, cannon.position.x - lookDirection.x) * Mathf.Rad2Deg;

            if((lookDirection.y >= limit.position.y))
            {
                cannon.rotation = Quaternion.Euler(new Vector3(0f, 0f, lookAngle + 90f));
            }

            time -= Time.deltaTime;
            ammunitionText.text = ammunition.ToString("D4");

            /*if(Input.GetMouseButtonDown(0)){
                burst = 3;
            }*/

            timeBurst -= Time.deltaTime;
            
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if(ModeShoot >= 2)
                {
                    ModeShoot = 0;
                }else{
                    ModeShoot++;
                }
                
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if(ModeShoot <= 0)
                {
                    ModeShoot = 2;
                }else{
                    ModeShoot--;
                }
            }

            switch (ModeShoot)
            {
                case 0: //infinity
                    modeShootText.text = "infinity";
                    if(Input.GetMouseButton(0) && !spawner.loseGame && ammunition > 0 && Time.timeScale == 1)
                    {
                        if(time <= 0)
                        {
                            ammunition --;
                            Cursor.visible = false;
                            Shoot();
                            time = timeReset;
                        }
                    }
                    break;
                case 1:// 3
                    modeShootText.text = "3";
                    if(Input.GetMouseButton(0) && !spawner.loseGame && ammunition > 0 && Time.timeScale == 1)
                    {
                        if(time <= 0 && burst > 0)
                        {
                            ammunition --;
                            burst --;
                            Cursor.visible = false;
                            Shoot();
                            time = timeReset;
                        }else{
                            if(timeBurst <= 0)
                            {
                                burst = 3;
                                timeBurst = 0.5f;
                            }
                        }
                    }else{
                        burst = 3;
                        timeBurst = 0.5f;
                    }
                    break;
                case 2:// 1
                    modeShootText.text = "1";
                    if(Input.GetMouseButtonDown(0) && !spawner.loseGame &&  ammunition > 0 && Time.timeScale == 1)
                    {
                        if(time <= 0)
                        {
                            ammunition --;
                            Cursor.visible = false;
                            Shoot();
                            time = timeReset;
                        }
                    }
                    break;
                default:
                    ModeShoot = 0;
                    break;
            }

            

            if(Input.GetMouseButtonDown(1) && !spawner.loseGame && Time.timeScale == 1)
            {
                if(numBooms > 0 && GameObject.FindObjectsOfType<Boom>().Length <= 0)
                {
                    Cursor.visible = false;
                    Boom();
                    numBooms--;
                    enabledImageBoom();
                }
            }
        }else if(spawner.loseGame){
            GameObject explo = Instantiate(explosion, transform.position, Quaternion.identity);
            GameObject obj = Instantiate(losePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if(ovni != null /*|| spawner.Intermission.activeSelf || spawner.PanelWave.activeSelf*/)
        {
            pause  = true;
            cannon.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }else{
            pause  = false;
        }
    }

    public Transform BunkerTransform()
    {
        return GetComponent<Transform>();
    }

    public void UpdateText()
    {
        scoreText.text = score.ToString("D5");
        KillsSoldierText.text = KillsSoldier.ToString("D4");
        KillsPlanesText.text = KillsPlanes.ToString("D3");
    }

    void Shoot()
    { 
        /*if(score > 0)
        {
            score -= 1; 
            UpdateText();
        }*/
        AudioBullet();
        GameObject bullet = Instantiate(bulletPrefab, point.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = cannon.up  * velocity;
        Destroy(bullet, timeDestroyBullet);
    }

    void Boom()
    {
        GameObject boom = Instantiate(bomPrefab, point.position, Quaternion.identity);
        boom.GetComponent<Rigidbody2D>().velocity = cannon.up  * velocityBoom;
    }

    public void AudioBullet()
    {
       // if (!audioSource.isPlaying)
        //{
            audioSource.PlayOneShot(audioBullet, volume);
        //}
    }
}
