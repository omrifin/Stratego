using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Globals : MonoBehaviour {

    public  Dictionary<string, GameObject> unityObjects;
    public static GameSituation currentSitutation;

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
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
