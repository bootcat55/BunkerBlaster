using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ovni : MonoBehaviour
{
    private Soldier[] soldier;//
    public float speed;
    public GameObject Ray;
    public float stop;
    private Transform Pos;//
    private Spawner spawner;

    public AudioClip audioRay;
    public AudioSource audioSource;
    public float volume;

    void Update()
    {
        float Infinito = Mathf.Infinity;
        Soldier Resultado = null;
        soldier = GameObject.FindObjectsOfType<Soldier>();
        spawner = GameObject.FindObjectOfType<Spawner>();

        foreach (Soldier PuntoInicial in soldier)
        {
            float Distancia = (PuntoInicial.transform.position - this.transform.position).sqrMagnitude;
            if (Distancia < Infinito)
            {
                Infinito = Distancia;
                Resultado = PuntoInicial;
            }
        }

        if(soldier.Length > 0)
        {
            Debug.DrawLine(this.transform.position, Resultado.transform.position);
            Pos = Resultado.transform; 

            if(!Ray.activeSelf)
            {
                if (Vector2.Distance(transform.position, Pos.position) < stop)//Parar
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector3 (Pos.position.x, Pos.position.y+1f, Pos.position.z), speed * Time.deltaTime);
                }else{
                    transform.position = Vector2.MoveTowards(transform.position, Pos.position, speed * Time.deltaTime);
                }
            }

            if(transform.position.x == Pos.position.x)
            {
                Ray.SetActive(true);
                AudioRay();
            }else{
                Ray.SetActive(false);
            }
        }

        if(soldier.Length == 0)
        {
            spawner.ovniSpawn = false;
            spawner.timeWave = spawner.timeResetWave;
            Destroy(gameObject);
        }
    }

    public void AudioRay()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioRay, volume);
        }
    }   
}
