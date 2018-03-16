using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_Controller : MonoBehaviour
{

    bool randomHasOccoured = false;
    private static SC_Controller instance;
    GameObject _menu;
    GameObject _resumeButton;

    public void UserPressedDeck()
    {
        SC_Logic.Instance.UserPressedDeck();
    }

    #region SingleTone
    public static SC_Controller Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_Controller").GetComponent<SC_Controller>();
            return instance;
        }

    }

    #endregion
    private void Start()
    {
        blueTeam = new Dictionary<string, GameObject>();
        for (int i = 0; i < 40; i++)
        {
            if (SC_Globals.Instance.unityObjects["Soldier (" + i + ")"] == null) print("its a null");
            blueTeam.Add("Soldier (" + i + ")", SC_Globals.Instance.unityObjects["Soldier (" + i + ")"]);
        }
    }
    private void Awake()
    {
        _menu = GameObject.Find("Panel_EndGame");
        _resumeButton = GameObject.Find("Button_ResumeGame");


        //print("controller awake!");
    }

    public void UserWantsToFIght(SC_PieceLogic sC_PieceLogic)
    {
        SC_Logic.instance.CheckIfUserWantsToFight(sC_PieceLogic, SC_Logic.Instance.GameBoard[sC_PieceLogic.currentTileRow][sC_PieceLogic.currentTileCol].tile.GetComponent<SC_TileLogic>());
    }

    void OnMouseDown()
    {
        //screenPoint = Camera.main.WorldToScreenPoint(scanPos);


        //offset = scanPos - Camera.main.ScreenToWorldPoint(
        //    new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

    }

    public void ButtonPlayGame()
    {
        SC_Globals.GamePhase = SC_Globals.GameSituation.Running;
        if(SC_Logic.currentTurn == SC_DefiendVariables.Turn.RedTurn)
            SC_Logic.Instance.RedTurn();
        SC_View.Instance.StartButton.SetActive(false);

    }

    public void UserPressedPiece(SC_PieceLogic sC_PieceLogic)
    {
        SC_Logic.Instance.UserPressedPiece(sC_PieceLogic);
    }

    Dictionary<string, GameObject> blueTeam;


    public void DeployEnemyPieces()
    {
        bool findSlot = true;
        int _colRand, _rowRand;
        for (int i = 0; i < 40; i++)
        {
            SC_PieceLogic _tmpEnemy = blueTeam["Soldier (" + i + ")"].GetComponent<SC_PieceLogic>();
            while (findSlot)
            {

                _rowRand = UnityEngine.Random.Range(0, 4);
                _colRand = UnityEngine.Random.Range(0, 10);
                //print("rowRand= " + _rowRand + " _colRand= " + _colRand);

                if (SC_Logic.Instance.GameBoard[_rowRand][_colRand].tileStatus == SC_DefiendVariables.TileStatus.Empty)
                {
                    findSlot = false;
                    SC_Logic.Instance.GameBoard[_rowRand][_colRand].tileStatus = SC_DefiendVariables.TileStatus.BlueOccupied;
                    SC_Logic.Instance.GameBoard[_rowRand][_colRand].piece = _tmpEnemy;
                    _tmpEnemy.transform.position = SC_Logic.Instance.GameBoard[_rowRand][_colRand].tile.transform.position;
                    _tmpEnemy.currentTileRow = _rowRand;
                    _tmpEnemy.currentTileCol = _colRand;

                }
                //break;

            }
            findSlot = true;
        }
    }

    public void UserPressedTile(SC_TileLogic sC_TileLogic)
    {
        SC_Logic.Instance.UserPressedTile(sC_TileLogic);
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
                        SC_Logic.Instance.GameBoard[i][j].piece.goBack();
                
            }
            DeployEnemyPieces();
            SC_Globals.instance.numOfDeployedBluePieces = 40;
            if (SC_Globals.instance.numOfDeployedBluePieces == 40)
                SC_View.Instance.StartButton.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            SC_Globals.GamePhase = SC_Globals.GameSituation.Running;
            print("SC_Globals.GamePhase = Running");
            if (SC_Logic.currentTurn == SC_DefiendVariables.Turn.RedTurn)
                SC_Logic.Instance.RedTurn();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
                _menu.SetActive(true);
            _resumeButton.SetActive(true);
                
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
        SC_View.Instance.EndGamePanel.SetActive(false);
        SC_Logic.instance.enemyArray.Clear();   //clean the enemy array
        for(int j=6; j<10;j++)
        {
            for(int i=0;i<10;i++)
            {
                Destroy(SC_Logic.Instance.GameBoard[j][i].piece);
                SC_Logic.Instance.GameBoard[j][i].tileStatus = SC_DefiendVariables.TileStatus.Empty;
            }


        }
        SC_Logic.instance.DeployEnemyPieces();
        SC_Globals.GamePhase = SC_Globals.GameSituation.setPieces;
    }

}
