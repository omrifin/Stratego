using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SC_Controller : MonoBehaviour {

    private static SC_Controller instance;
    #region SingleTone
    public static SC_Controller Instance{
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

  

        //print("controller awake!");
    }

    void OnMouseDown()
    {
        //screenPoint = Camera.main.WorldToScreenPoint(scanPos);


        //offset = scanPos - Camera.main.ScreenToWorldPoint(
        //    new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

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
        if (Input.GetKeyDown(KeyCode.R))
        {
            DeployEnemyPieces();
            gameBoard.fillPieces = 40;

        }

    }

}
