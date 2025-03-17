using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreFinal : MonoBehaviour
{
    public Text Score, KillsSoldier, KillsPlanesAndMissiles;
    public Bunker bunker;
    void Update()
    {
        Cursor.visible = true;
        if(gameObject.activeSelf){
            Score.text = bunker.score.ToString();
            KillsSoldier.text = bunker.KillsSoldier.ToString();
            KillsPlanesAndMissiles.text = bunker.KillsPlanes.ToString();
        }
    }
}
