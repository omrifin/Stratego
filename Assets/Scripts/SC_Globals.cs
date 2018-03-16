using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Globals : MonoBehaviour {

    public int numOfDeployedBluePieces;
    public  Dictionary<string, GameObject> unityObjects;
    public Dictionary<string, SC_PieceLogic> EnemyPieces;

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
    public static SC_Globals instance;
    public static SC_Globals Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_Globals").GetComponent<SC_Globals>();
            return instance;
        }
    }
#endregion


    private void Awake()
    {
        GamePhase = GameSituation.setPieces;
    }

    void Start () {
        numOfDeployedBluePieces = 0;
        unityObjects = new Dictionary<string, GameObject>();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("unityObject");
        foreach (GameObject go in objects)
        {
            unityObjects.Add(go.name, go);
            print("go.name= " + go.name);
        }
        print("Amount of Object is = " + unityObjects.Count);
        InitializeEnemy();
    }
	
    private void InitializeEnemy()
    {
        EnemyPieces = new Dictionary<string, SC_PieceLogic>();
        for(int i = 0; i < 40; i++)
        {
            EnemyPieces.Add("Enemy (" + i + ")", unityObjects["Enemy (" + i + ")"].GetComponent<SC_PieceLogic>());
        }
        print("number of enemies: "+EnemyPieces.Count);
    }
}
