using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public float timePlane, timeMissile, timeWave, timePlaneDown;
    public GameObject planePrefab, planeDownPrefab, missilePrefab, ovniPrefab;
    public int numPlanes, numMissiles;
    private float timePlaneReset, timeMissileReset, timePlaneDownReset;
    [HideInInspector]
    public float timeResetWave;
    public Transform A, D, spawnOvni, ADown, DDown;
    private GameObject plane, missile;
    private Soldier[] soldier;
    [HideInInspector]
    public bool loseGame, ovniSpawn;
    private bool loseToSoldier;
    public GameObject GameOver, Intermission, PanelMissile, PanelWave;
    public Text WaveText;
    private bool MissileLose;
    private byte acolor;
    private int wave = 1;
    private Missile[] Miss;
    private Plane[] plan;
    public Bunker bunker;

    public AudioClip audioMissile;
    public AudioSource audioSource;
    public float volume;

    private void Start()
    {
        timePlaneReset = timePlane;
        timeMissileReset = timeMissile;
        timeResetWave = timeWave;
        timePlaneDownReset = timePlaneDown;
    }

    private void Update() {

        if(!loseGame)
        {
            timePlane -= Time.deltaTime;
            timePlaneDown -= Time.deltaTime;
            timeMissile -= Time.deltaTime;
            timeWave -= Time.deltaTime;

            if(timeWave > 0)
            {
                if(wave > 1)
                {
                    SpawnPlaneDown();
                }
                if(wave > 3)
                {
                    SpawnMissile();
                }
                SpawnPlane();
               
            }else{
                soldier = GameObject.FindObjectsOfType<Soldier>();
                Miss = GameObject.FindObjectsOfType<Missile>();
                plan = GameObject.FindObjectsOfType<Plane>();

                if(soldier.Length == 0 && Miss.Length == 0 && plan.Length == 0 && !loseToSoldier && !ovniSpawn)
                {
                    Debug.Log("Win");
                    WaveText.text = "ASSAULT WAVE " + (wave++) + " COMPLETE";
                    StartCoroutine(PanelWaveComplete());
                    timeWave = timeResetWave;
                }else {
                    int num = soldier.Length;
                    int i = 0;
                    foreach (var item in soldier)
                    {
                        if(item.stop)
                        {
                            i++;
                        }
                    }
                    if(num == i)//si todos se han detenido todos
                    {
                        if(i >= 3 || loseToSoldier)
                        {
                            Lose();
                        }else if(!loseToSoldier && !ovniSpawn && Miss.Length == 0 && plan.Length == 0){
                            Debug.Log("Ovni");
                            StartCoroutine(PanelIntermission());
                            WaveText.text = "ASSAULT WAVE " + (wave++) + " COMPLETE";
                            StartCoroutine(PanelWaveComplete());
                            ovniSpawn = true;
                        }
                    }
                }
            }

            if(loseToSoldier && soldier.Length == 0)
            {
                Debug.Log("Lose");
                GameOver.SetActive(true);
                Cursor.visible = true;
                loseGame = true;
            }
        }

        if(MissileLose)
        {
            StartCoroutine(PanelLoserMissile());
        }
    }

    void SpawnPlane()
    {
        if (timePlane <= 0)//tiempo en segundos
        {
            for (int i = 0; i < numPlanes; i++)
            {
                int rand = Random.Range(0, 2);
                if(rand == 1)
                {
                    plane = Instantiate(planePrefab, A.position, Quaternion.identity);
                    plane.GetComponent<Plane>().SpawnA = true;
                }else{//Spawn in D
                    plane = Instantiate(planePrefab, D.position, Quaternion.identity);
                    plane.GetComponent<Plane>().SpawnD = true;
                }
                plane.transform.parent = transform;
                timePlane = timePlaneReset;
                
            }
        }
    }

    void SpawnPlaneDown()
    {
        if (timePlaneDown <= 0)//tiempo en segundos
        {
            for (int i = 0; i < numPlanes; i++)
            {
                int rand = Random.Range(0, 2);
                if(rand == 1)
                {
                    plane = Instantiate(planeDownPrefab, ADown.position, Quaternion.identity);
                    plane.GetComponent<Plane>().SpawnADown = true;
                }else{//Spawn in D
                    plane = Instantiate(planeDownPrefab, DDown.position, Quaternion.identity);
                    plane.GetComponent<Plane>().SpawnDDown = true;
                }
                plane.transform.parent = transform;
                timePlaneDown = timePlaneDownReset;
            }
        }
    }

    void SpawnOvni()
    {
        GameObject ovni = Instantiate(ovniPrefab, spawnOvni.position, Quaternion.identity);
    }

    void SpawnMissile()
    {
        if (timeMissile <= 0)//tiempo en segundos
        {
            for (int i = 0; i < numMissiles; i++)
            {
                int rand = Random.Range(0, 2);
                if(rand == 1)
                {
                    missile = Instantiate(missilePrefab, A.position, Quaternion.identity);
                    missile.GetComponent<Missile>().SpawnA = true;
                }else{//Spawn in D
                    missile = Instantiate(missilePrefab, D.position, Quaternion.identity);
                    missile.GetComponent<Missile>().SpawnA = false;
                }
                missile.transform.parent = transform;
                timeMissile = timeMissileReset;
            }
        }
    }

    IEnumerator PanelLoserMissile()
    {
        loseGame = true;
        PanelMissile.SetActive(true);
        acolor++;
        PanelMissile.GetComponent<Image>().color = new Color32(255,255,255,acolor);
        yield return new WaitForSeconds(2f);
        GameOver.SetActive(true);
        MissileLose = false;
        PanelMissile.SetActive(false);
    }


    IEnumerator PanelIntermission()
    {
        yield return new WaitForSeconds(3f);
        Intermission.SetActive(true);
        yield return new WaitForSeconds(3f);
        Intermission.SetActive(false);
        SpawnOvni(); 
    }

    IEnumerator PanelWaveComplete()
    {
        PanelWave.SetActive(true);
        yield return new WaitForSeconds(3f);
        if(bunker.numBooms < 6)
        {
            bunker.numBooms++;
            bunker.enabledImageBoom();
        }
        PanelWave.SetActive(false);
    }

    

    public void Lose()
    {
        loseToSoldier = true;
        foreach (var item in soldier)
        {
            item.lose();
        }
        Debug.Log("Lose to soldier");
    }

    public void LoseMissile()
    {
        MissileLose = true;
        Debug.Log("LoseMissile");
        AudioMissile();
    }

    public void Cargar()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Resert()
    {
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        Application.Quit();;
    }


    public void AudioMissile()
    {
        audioSource.PlayOneShot(audioMissile, volume);
    }
}
