using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Globals : MonoBehaviour {

    public  Dictionary<string, GameObject> unityObjects;
    public Dictionary<string, SC_PieceLogic> EnemyPieces;

    public static GameSituation currentSitutation;

    public enum SoldierRank
    {
        spy, two, three, four, five, six, seven, eight, nine, ten, bomb, flag
    }

    public enum objectType
    {
        Blue,
        Red,
        Tile
    }
    public enum GameSituation
    {
        setPieces,
        Running
    }

    #region singleTone
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
        currentSitutation = GameSituation.setPieces;

    }
    void Start () {
        unityObjects = new Dictionary<string, GameObject>();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("unityObject");
        foreach (GameObject go in objects)
            unityObjects.Add(go.name, go);
        print("Amount of Object is = " + unityObjects.Count);
        initializeEnemy();
    }
	
    private void initializeEnemy()
    {
        EnemyPieces = new Dictionary<string, SC_PieceLogic>();
        for(int i = 0; i < 40; i++)
        {
            EnemyPieces.Add("Enemy (" + i + ")", unityObjects["Enemy (" + i + ")"].GetComponent<SC_PieceLogic>());
        }
        print("number of enemies: "+EnemyPieces.Count);
    }
}
