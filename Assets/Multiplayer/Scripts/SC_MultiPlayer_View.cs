using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_MultiPlayer_View : MonoBehaviour
{


    private static SC_MultiPlayer_View instance;
    //GameObject [][]btn;
    GameObject tile;
    GameObject holder;
    GameObject WaterTile;
    public GameObject StartButton;
    public GameObject EndGamePanel;
    public Text EndGameText;

    #region SingleTone
    public static SC_MultiPlayer_View Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_MultiPlayer_View").GetComponent<SC_MultiPlayer_View>();
            return instance;
        }

    }
    #endregion


    private void Awake()
    {
     
    }
    void Start()
    {
        init();

    }




    private void init()
    {
        tile = SC_MultiPlayer_Globals.Instance.unityObjects["Grass1_Cus2"];
        //EndGameText = GameObject.Find("EndGameText").GetComponent<Text>();
        EndGameText = SC_MultiPlayer_Globals.Instance.unityObjects["EndGameText"].GetComponent<Text>();
        //tile = GameObject.Find("Grass1_Cus2");
        WaterTile = GameObject.Find("WaterTile");
        holder = GameObject.Find("TileHolder");
        StartButton = GameObject.Find("StartButton");
        EndGamePanel = GameObject.Find("Panel_EndGame");
        if (SC_MultiPlayer_Globals.Instance.unityObjects["TXT_WaitingStart"] != null) SC_MultiPlayer_Globals.Instance.unityObjects["TXT_WaitingStart"].SetActive(false);
        //SC_MultiPlayer_Globals.Instance.unityObjects["Canvas"].SetActive(false);




        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
            {
                if (i == 4 && j == 2 || i == 4 && j == 3 || i == 5 && j == 2 || i == 5 && j == 3 || i == 4 && j == 6 || i == 5 && j == 7 || i == 5 && j == 6 || i == 4 && j == 7)
                {
                    GameObject Water = Instantiate(WaterTile, new Vector2(j + WaterTile.transform.position.x, i + WaterTile.transform.position.y), Quaternion.identity);
                    Water.transform.parent = holder.transform;

                }
                else
                {
                    
                    GameObject _tmp = Instantiate(tile, new Vector2(j + tile.transform.position.x, i + tile.transform.position.y), Quaternion.identity);
                    //print(" _tmp.name + i + j= " + _tmp.name + i + j);
                    _tmp.name = _tmp.name + i + j;
                    _tmp.transform.parent = holder.transform;
                    _tmp.GetComponent<SC_MultiPlayer_TileLogic>().Row = i;
                    _tmp.GetComponent<SC_MultiPlayer_TileLogic>().Col = j;
                    SC_MultiPlayer_Logic.Instance.gameBoard[i][j].tile = _tmp;
                    //print("board is being initialized!!!!");
                }
            }

        WaterTile.SetActive(false);
        StartButton.SetActive(false);
        EndGamePanel.SetActive(false);
        //SC_MultiPlayer_Globals.Instance.unityObjects["TXT_WaitingStart"].SetActive(false);
    }

    public void movePieceToNewLocation(SC_MultiPlayer_PieceLogic piece, SC_MultiPlayer_TileLogic tile)
    {
        //Vector3.MoveTowards(tile.transform.position, piece.transform.position,50f);
        //piece.transform.position = Vector2.MoveTowards(piece.transform.position, tile.transform.position, 50f);
        piece.transform.position = tile.transform.position;

    }
    private void FixedUpdate()
    {
        //float step = speed * Time.deltaTime;
        //piece.transform.position = Vector2.MoveTowards(piece.transform.position, tile.transform.position, 0.8f);

    }

    public void HideValidPlacements()
    {
        GameObject _tmp;
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
            {
                _tmp = SC_MultiPlayer_Logic.Instance.gameBoard[i][j].tile;
                if (_tmp != null)
                    _tmp.GetComponent<SC_MultiPlayer_TileLogic>().GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 255f);
            }

        //print("Trying color it back!");
    }

    public void ShowValidPlacements(GameObject tile)
    {

        tile.GetComponent<SpriteRenderer>().color = new Color(120f, 0f, 0f, 1f);

    }

    internal void KillPiece(SC_MultiPlayer_PieceLogic defender)
    {
        defender.goBack();
        SC_MultiPlayer_Globals.Instance.unityObjects[defender.name].SetActive(false);
    }
}
