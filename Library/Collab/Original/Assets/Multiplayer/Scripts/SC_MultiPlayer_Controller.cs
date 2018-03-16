using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_MultiPlayer_Controller : MonoBehaviour
{

    bool randomHasOccoured = false;
    private static SC_MultiPlayer_Controller instance;
    GameObject _menu;
    GameObject _resumeButton;

    public void UserPressedSC_MultiPlayer_Deck()
    {
        SC_MultiPlayer_Logic.Instance.UserPressedSC_MultiPlayer_Deck();
    }

    #region SingleTone
    public static SC_MultiPlayer_Controller Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_MultiPlayer_Controller").GetComponent<SC_MultiPlayer_Controller>();
            return instance;
        }

    }

    #endregion
    private void Start()
    {
         blueTeam = new Dictionary<string, GameObject>();
        for (int i = 0; i < 40; i++)
        {
            if (SC_MultiPlayer_Globals.Instance.unityObjects["Soldier (" + i + ")"] == null) print("its a null");
            blueTeam.Add("Soldier (" + i + ")", SC_MultiPlayer_Globals.Instance.unityObjects["Soldier (" + i + ")"]);
        }
    }
    private void Awake()
    {
        _menu = GameObject.Find("Panel_EndGame");
        _resumeButton = GameObject.Find("Button_ResumeGame");


        //print("controller awake!");
    }

    public void UserWantsToFIght(SC_MultiPlayer_PieceLogic SC_MultiPlayer_PieceLogic)
    {
        SC_MultiPlayer_Logic.instance.CheckIfUserWantsToFight(SC_MultiPlayer_PieceLogic, SC_MultiPlayer_Logic.Instance.gameBoard[SC_MultiPlayer_PieceLogic.currentTileRow][SC_MultiPlayer_PieceLogic.currentTileCol].tile.GetComponent<SC_MultiPlayer_TileLogic>());
    }

    void OnMouseDown()
    {
        //screenPoint = Camera.main.WorldToScreenPoint(scanPos);


        //offset = scanPos - Camera.main.ScreenToWorldPoint(
        //    new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

    }

    public void ButtonPlayGame()
    {
        SC_MultiPlayer_Globals.GamePhase = SC_MultiPlayer_Globals.GameSituation.Running;
        SC_MultiPlayer_View.Instance.StartButton.SetActive(false);

    }

    public void UserPressedPiece(SC_MultiPlayer_PieceLogic SC_MultiPlayer_PieceLogic)
    {
        SC_MultiPlayer_Logic.Instance.UserPressedPiece(SC_MultiPlayer_PieceLogic);
    }

    Dictionary<string, GameObject> blueTeam;


    public void DeployEnemyPieces()
    {
      
        bool findSlot = true;
        int _colRand, _rowRand;
        for (int i = 0; i < 40; i++)
        {
            SC_MultiPlayer_PieceLogic _tmpEnemy = blueTeam["Soldier (" + i + ")"].GetComponent<SC_MultiPlayer_PieceLogic>();
            while (findSlot)
            {

                _rowRand = UnityEngine.Random.Range(0, 4);
                _colRand = UnityEngine.Random.Range(0, 10);
                //print("rowRand= " + _rowRand + " _colRand= " + _colRand);

                if (SC_MultiPlayer_Logic.Instance.gameBoard[_rowRand][_colRand].tileStatus == SC_DefiendVariables.TileStatus.Empty)
                {
                    findSlot = false;
                    SC_MultiPlayer_Logic.Instance.gameBoard[_rowRand][_colRand].tileStatus = SC_DefiendVariables.TileStatus.BlueOccupied;
                    SC_MultiPlayer_Logic.Instance.gameBoard[_rowRand][_colRand].piece = _tmpEnemy;
                    _tmpEnemy.transform.position = SC_MultiPlayer_Logic.Instance.gameBoard[_rowRand][_colRand].tile.transform.position;
                    _tmpEnemy.currentTileRow = _rowRand;
                    _tmpEnemy.currentTileCol = _colRand;

                }
                //break;

            }
            findSlot = true;
        }
    }

    public void UserPressedTile(SC_MultiPlayer_TileLogic SC_MultiPlayer_TileLogic)
    {
        SC_MultiPlayer_Logic.Instance.UserPressedTile(SC_MultiPlayer_TileLogic);
    }

    private void FixedUpdate()
    {
        checkKeysInput();

    }

    public void checkKeysInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (randomHasOccoured)
            {
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 10; j++)
                        SC_MultiPlayer_Logic.Instance.gameBoard[i][j].piece.goBack();

            }
            DeployEnemyPieces();
            SC_MultiPlayer_Globals.instance.numOfDeployedBluePieces = 40;
            if (SC_MultiPlayer_Globals.instance.numOfDeployedBluePieces == 40)
                SC_MultiPlayer_View.Instance.StartButton.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            SC_MultiPlayer_Globals.GamePhase = SC_MultiPlayer_Globals.GameSituation.Running;
            print("SC_MultiPlayer_Globals.GamePhase = Running");
           
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _menu.SetActive(true);
            _resumeButton.SetActive(true);

        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SC_MultiPlayer_Logic.Instance.Btn_PlayLogic();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            print("CurrentTurn= " + SC_MultiPlayer_Logic.currentTurn + " GamePhase= " + SC_MultiPlayer_Globals.GamePhase);
        }
    
    }
    public void returnToGame()
    {
        _menu.SetActive(false);
        //_resumeButton.SetActive(false);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Main Project", LoadSceneMode.Single);
    }

    public void Restart()
    {
        SC_MultiPlayer_View.Instance.EndGamePanel.SetActive(false);
        SC_MultiPlayer_Logic.instance.enemyArray.Clear();   //clean the enemy array
        for (int j = 6; j < 10; j++)
        {
            for (int i = 0; i < 10; i++)
            {
                Destroy(SC_MultiPlayer_Logic.Instance.gameBoard[j][i].piece);
                SC_MultiPlayer_Logic.Instance.gameBoard[j][i].tileStatus = SC_DefiendVariables.TileStatus.Empty;
            }


        }
        SC_MultiPlayer_Logic.instance.DeployEnemyPieces();
        SC_MultiPlayer_Globals.GamePhase = SC_MultiPlayer_Globals.GameSituation.setPieces;
    }

}
