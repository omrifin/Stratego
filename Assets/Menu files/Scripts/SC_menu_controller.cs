using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_menu_controller : MonoBehaviour {
    

    public void MainMenu_SinglePlayer_Button()
    {
       
        SceneManager.LoadScene("Single Player", LoadSceneMode.Single);
    }

    public void MainMenu_MultiPlayer_Button()
    {
        SC_menu_logic.Instance.ChangeScreen(SC_DefinedVariables.ScreenEnum.MultiPlayer);
        Debug.Log("MILTI");
    }

    public void MainMenu_StudentInfo_Button()
    {
        SC_menu_logic.Instance.ChangeScreen(SC_DefinedVariables.ScreenEnum.StudentInfo);

    }

    public void MainMenu_Options_Button()
    {
        SC_menu_logic.Instance.ChangeScreen(SC_DefinedVariables.ScreenEnum.Options);

    }

    public void Screen_Main_Btn_Back()
    {
        SC_menu_logic.Instance.ChangeScreen(SC_menu_logic.Instance.getPrevScreen());
    }

    public void Button_back()
    {
        SC_menu_logic.Instance.ChangeScreen(SC_menu_logic.Instance.getPrevScreen());
    }
    public void Screen_Loading()
    {
        SC_menu_logic.Instance.ChangeScreen(SC_DefinedVariables.ScreenEnum.Loading);
    }
    public void MultiPlayer_MoneySlider()
    {
        float _tmp;
        _tmp = SC_menu_logic.unityObjects["MoneySlider"].GetComponent<Slider>().value;
        SC_menu_logic.unityObjects["Label_Options_AmountOfMoney"].GetComponent<Text>().text = "Bet " + _tmp+ "$";
    }

    public void moveToMultiplayer()
    {
        SC_DefiendVariables.bet.Add("bet", SC_menu_logic.unityObjects["MoneySlider"].GetComponent<Slider>().value.ToString()); 
        SceneManager.LoadScene("MultiPlayer", LoadSceneMode.Single);

    }
}
