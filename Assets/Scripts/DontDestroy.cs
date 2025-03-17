using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    [HideInInspector]
    public bool sound = true;
    static bool created = false;

    void Awake() {
        if (!created) {
            DontDestroyOnLoad (this.gameObject);
            created = true;
        } else {
            Destroy (this.gameObject);
        }
    }

    public void AudioON()
    {
        AudioListener.pause = false;
        sound = true;
        Debug.Log("ON");
    }

    public void AudioOFF()
    {
        AudioListener.pause = true;
        sound = false;
        Debug.Log("OFF");
    }
}
