using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_View : MonoBehaviour {


    private static SC_View instance;
    //GameObject [][]btn;
    GameObject tile;
    GameObject holder;
    GameObject WaterTile;

    #region SingleTone
    public static SC_View Instance {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_View").GetComponent<SC_View>();
            return instance;
        }

    }
    #endregion


    private void Awake()
    {
        tile = GameObject.Find("Grass1_Cus2");
        WaterTile = GameObject.Find("WaterTile");
        holder = GameObject.Find("TileHolder");


        init();
    }
    void Start()
    {
  
    }
    private void init()
    {
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
            {
                if (i == 4 && j == 2 || i == 4 && j == 3 || i == 5 && j == 2 || i == 5 && j == 3 || i == 4 && j == 6 || i == 5 && j == 7 || i == 5 && j == 6 || i==4 && j==7    )
                {
                    GameObject Water = Instantiate(WaterTile, new Vector2(j + WaterTile.transform.position.x, i + WaterTile.transform.position.y), Quaternion.identity);
                    Water.transform.parent = holder.transform;

                }
                else
                {
                    GameObject _tmp = Instantiate(tile, new Vector2(j + tile.transform.position.x, i + tile.transform.position.y), Quaternion.identity);
                    _tmp.name = _tmp.name + i + j;

                    _tmp.transform.parent = holder.transform;

                }
            }
        WaterTile.SetActive(false);
    }


    void Update () {
		
	}
    public void showEmptySlot(int i,int j)
    {
        SC_TileLogic _tmpTile = SC_Globals.Instance.unityObjects["Grass1_Cus2(Clone)" + i + j].GetComponent<SC_TileLogic>();
        _tmpTile.GetComponent<SpriteRenderer>().color = new Color(120f, 0f, 0f, 1f);

    }
}
