using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MultiPlayer_Globals : MonoBehaviour
{

    public static string apiKey = "f2aa1a1802060a9fab3409bad89b28f260e33d65576f95ede6532da207f3fd14";
    public static string secretApiKey= "e4f7b58a65a60ab54f82fa223b2648817ffdc85de063f38776ea3690313e09d2";

    public int numOfDeployedBluePieces;
    public Dictionary<string, GameObject> unityObjects;
    public Dictionary<string, SC_MultiPlayer_PieceLogic> EnemyPieces;


    public static Listener listener;
    public static string userName = string.Empty;
    public static List<string> rooms;
    public static int index = 0;

    public static GameSituation GamePhase;

    public enum SoldierRank
    {
        spy, two, three, four, five, six, seven, eight, nine, ten, bomb, flag
    }

    public enum ObjectType
    {
        Blue,
        Red,
        Tile
    }
    public enum GameSituation
    {
        setPieces,
        Running,
        Finished
    }

    #region singleTon
    public static SC_MultiPlayer_Globals instance;
    public static SC_MultiPlayer_Globals Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_MultiPlayer_Globals").GetComponent<SC_MultiPlayer_Globals>();
            return instance;
        }
    }
    #endregion


    private void Awake()
    {
        GamePhase = GameSituation.setPieces;
        numOfDeployedBluePieces = 0;
        unityObjects = new Dictionary<string, GameObject>();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("unityObjects");
        foreach (GameObject go in objects)
        {
            unityObjects.Add(go.name, go);
            //print("go.name= " + go.name);
        }
        print("Amount of Object is = " + unityObjects.Count);
        InitializeEnemy();
        listener = new Listener();
        rooms = new List<string>();
    }

    void Start()
    {
        SC_MultiPlayer_Logic.currentTurn = SC_DefiendVariables.Turn.blueTurn;
        print("blue turn!");
    
    }

    private void InitializeEnemy()
    {
        EnemyPieces = new Dictionary<string, SC_MultiPlayer_PieceLogic>();
        for (int i = 0; i < 40; i++)
        {
            //EnemyPieces.Add("Enemy (" + i + ")", unityObjects["Enemy (" + i + ")"].GetComponent<SC_MultiPlayer_PieceLogic>());
            EnemyPieces.Add("Enemy (" + i + ")", GameObject.Find("Enemy (" + i + ")").GetComponent<SC_MultiPlayer_PieceLogic>());
        }
    }
}
