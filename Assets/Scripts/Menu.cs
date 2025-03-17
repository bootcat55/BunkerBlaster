using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject[] GUI, SettingsObjects;
    private int NumSelect = 0, NumSelectSettings;
    public GameObject On, Off, FullScreenON, FullScreenOff, SettingsPanel;
    private DontDestroy dontDestroy;
    private float num;
    public float speedMouse;
    private Vector3 lookDirection;

    void Awake()
    {
        Screen.fullScreen = true;
        Cursor.visible = false;
        for (int i = 0; i < GUI.Length; i++)
        {
            GUI[i].SetActive(false);
        }
        GUI[0].SetActive(true);

        for (int j = 0; j < SettingsObjects.Length; j++)
        {
            SettingsObjects[j].SetActive(false);
        }
        SettingsObjects[0].SetActive(true);
    }

    private void Start() {
        dontDestroy = GameObject.FindObjectOfType<DontDestroy>();
        if(dontDestroy.sound)
        {
            On.SetActive(true);
            Off.SetActive(false);
        }else{
            Off.SetActive(true);
            On.SetActive(false);
        }

        if(Screen.fullScreen == true)
        {
            FullScreenON.SetActive(true);
            FullScreenOff.SetActive(false);
            Screen.fullScreen = true;
        }else{
            FullScreenOff.SetActive(true);
            FullScreenON.SetActive(false);
            Screen.fullScreen = false;
        }
    }

    public void salir() 
    {
        Debug.Log("Salir");
        Application.Quit();
    }

    public void Cargar()
    {
        SceneManager.LoadScene("Game");
    }

    public void Sound()
    {
        if(On.activeSelf)
        {
            dontDestroy.AudioOFF();
            Off.SetActive(true);
            On.SetActive(false);
        }else{
            dontDestroy.AudioON();
            Off.SetActive(false);
            On.SetActive(true);
        }
    }

    public void Full()
    {
        if(FullScreenON.activeSelf)
        {
            FullScreenOff.SetActive(true);
            FullScreenON.SetActive(false);
            Screen.fullScreen = false;
        }else{
            FullScreenOff.SetActive(false);
            FullScreenON.SetActive(true);
            Screen.fullScreen = true;
        }
    }

    public void Settings()
    {
        SettingsPanel.SetActive(true);
    }

    void ClosePanelSettings(){
        SettingsPanel.SetActive(false);
    }

    private void Update() {
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(!SettingsPanel.activeSelf)
        {
            StartPanel();
        }else{
            SettingsPanelStart();
        }
    }

    void SettingsPanelStart()
    {
        if(lookDirection.y < (num - speedMouse)) 
        {
            NextSettings();
            Cursor.visible = false;
            num = lookDirection.y;
        }
        else if (lookDirection.y > (num + speedMouse))
        {
            PreviousSettings();
            Cursor.visible = false;
            num = lookDirection.y;
        }

        if(NumSelectSettings == 0 && Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Return))
        {
            Sound();
        }
        else if(NumSelectSettings == 1 && Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Return))
        {
            Full();
        }else if(NumSelectSettings == 2 && Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Return))
        {
            ClosePanelSettings();
        }
    }

    void StartPanel()
    {
        if(lookDirection.y < (num - speedMouse)) 
        {
            Next();
            Cursor.visible = false;
            num = lookDirection.y;
        }
        else if (lookDirection.y > (num + speedMouse))
        {
            Previous();
            Cursor.visible = false;
            num = lookDirection.y;
        }

        if(NumSelect == 0 && Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Return))
        {
            Cargar();
        }
        else if(NumSelect == 1 && Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Return))
        {
            Settings();
        }else if(NumSelect == 2 && Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Return))
        {
            salir();
        }
    }

    public void Next()
    {
        if(NumSelect >= GUI.Length-1)
        {
            //NumSelect = -1;
        }else{
            NumSelect++;
        }
        
        for (int i = 0; i < GUI.Length; i++)
        {
            GUI[i].SetActive(false);
        }
        GUI[NumSelect].SetActive(true);
    }

    public void Previous()
    {
        if(NumSelect <= 0)
        {
            //NumSelect = GUI.Length;
        }else{
            NumSelect--;
        }

        for (int i = 0; i < GUI.Length; i++)
        {
            GUI[i].SetActive(false);
        }
        GUI[NumSelect].SetActive(true);
    }

    public void NextSettings()
    {
        if(NumSelectSettings >= SettingsObjects.Length-1)
        {
            //NumSelectSettings = -1;
        }else{
            NumSelectSettings++;
        }
        
        for (int i = 0; i < SettingsObjects.Length; i++)
        {
            SettingsObjects[i].SetActive(false);
        }
        SettingsObjects[NumSelectSettings].SetActive(true);
    }

    public void PreviousSettings()
    {
        if(NumSelectSettings <= 0)
        {
            //NumSelectSettings = SettingsObjects.Length;
        }else{
            NumSelectSettings--;
        }

        for (int i = 0; i < SettingsObjects.Length; i++)
        {
            SettingsObjects[i].SetActive(false);
        }
        SettingsObjects[NumSelectSettings].SetActive(true);
    }
}
