using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_menu_logic : MonoBehaviour {
    public static Dictionary<string, GameObject> unityObjects;

    //hold the current Screen and previous screen -> enum! 
    private SC_DefinedVariables.ScreenEnum currentScreen;
    private SC_DefinedVariables.ScreenEnum prevScreen;

    static SC_menu_logic instance;

    //SINGLETONE! return instance
    public static SC_menu_logic Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("Menu_logic").GetComponent<SC_menu_logic>();
                return instance;
        }
    }


    void Awake()
    {
        unityObjects = new Dictionary<string, GameObject>();
        GameObject[] tempArr = GameObject.FindGameObjectsWithTag("menuObject");

        foreach (GameObject go in tempArr)
            unityObjects.Add(go.name, go);
        
        currentScreen = SC_DefinedVariables.ScreenEnum.MainScreen;

        unityObjects["Label_Options_AmountOfMoney"].GetComponent<Text>().text = "Bet 0$";
        unityObjects["Screen main menu"].SetActive(true);
        unityObjects["Button_back"].SetActive(false);
        unityObjects["Screen single player"].SetActive(false);
        unityObjects["Screen multiplayer player"].SetActive(false);
        unityObjects["Screen options"].SetActive(false);
        unityObjects["Screen student info"].SetActive(false);
        unityObjects["Screen loading"].SetActive(false);
        
    }

    void Start () {




    }

    void Update () {
		
	}

    public void ChangeScreen(SC_DefinedVariables.ScreenEnum toScreen)
    {
        prevScreen = currentScreen;
        currentScreen = toScreen;

        switch (prevScreen) { 
                case SC_DefinedVariables.ScreenEnum.MainScreen:
            unityObjects["Screen main menu"].SetActive(false);
                unityObjects["Button_back"].SetActive(true);
                break;
        case SC_DefinedVariables.ScreenEnum.MultiPlayer:
            unityObjects["Screen multiplayer player"].SetActive(false);
            break;
        case SC_DefinedVariables.ScreenEnum.SinglePlayer:
            unityObjects["Screen single player"].SetActive(false);
            break;
        case SC_DefinedVariables.ScreenEnum.StudentInfo:
            unityObjects["Screen student info"].SetActive(false);
            break;
        case SC_DefinedVariables.ScreenEnum.Options:
            unityObjects["Screen options"].SetActive(false);
                unityObjects["MainMenu_Options_Button"].SetActive(true);
                break;
        case SC_DefinedVariables.ScreenEnum.Loading:
                unityObjects["Screen loading"].SetActive(false);
                break;
        }

            switch (toScreen)
        {
            case SC_DefinedVariables.ScreenEnum.MainScreen:
                unityObjects["Screen main menu"].SetActive(true);
                unityObjects["Button_back"].SetActive(false);
                changeTitleText("Main Menu");
                break;
            case SC_DefinedVariables.ScreenEnum.Options:
                unityObjects["Screen options"].SetActive(true);
                unityObjects["MainMenu_Options_Button"].SetActive(false);
                changeTitleText("Options");

                break;
            case SC_DefinedVariables.ScreenEnum.SinglePlayer:
                unityObjects["Screen single player"].SetActive(true);
                changeTitleText("Single Player");
                break;
            case SC_DefinedVariables.ScreenEnum.MultiPlayer:
                unityObjects["Screen multiplayer player"].SetActive(true);
                prevScreen = SC_DefinedVariables.ScreenEnum.MainScreen;
                changeTitleText("Multi Player");
                break;
            case SC_DefinedVariables.ScreenEnum.StudentInfo:
                unityObjects["Screen student info"].SetActive(true);
                prevScreen = SC_DefinedVariables.ScreenEnum.MainScreen;
                changeTitleText("Student Info");
                break;
            case SC_DefinedVariables.ScreenEnum.Loading:
                unityObjects["Screen loading"].SetActive(true);
                changeTitleText(" ");
                print("toScreen = Loading");
                break;

        }
        
    }


    private void changeTitleText(string v)
    {
        unityObjects["Title"].GetComponent<Text>().text = v;
    }

    public SC_DefinedVariables.ScreenEnum getPrevScreen()
    {
        return prevScreen;
    }

   
}



