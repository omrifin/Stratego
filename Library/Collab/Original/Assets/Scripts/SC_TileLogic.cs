using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_TileLogic : MonoBehaviour {

    //public Color;
    public SC_DefiendVariables.TileStatus currentStatus;
    public int num = 2;
    bool soldierDragToThisTile = true;

    
   

    private void Awake()
    {
        //print("initialized!");
        currentStatus = SC_DefiendVariables.TileStatus.Empty;

    }
    void Start () {
		
	}

    private void OnMouseDrag()
    {
        //this.GetComponent<SpriteRenderer>().color = new Color(120f, 0f, 0f, 1f);
    }
    private void OnMouseEnter()
    {
        this.GetComponent<SpriteRenderer>().color = new Color(120f,0f,0f,1f);
    }
 
    private void OnMouseExit()
    {
   
        this.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 255f);
        soldierDragToThisTile = false;
    }
    public void changeStatus(SC_DefiendVariables.TileStatus newStatus)
    {
        currentStatus = newStatus;
    }


    void Update () {
		
	}
    public void OnCollisionEnter2D(Collision2D coll)
    {
        //print("collision enter from SC_Tile with = "+coll.transform.name);
        //print(collision.rigidbody.name);
        //this.GetComponent<SpriteRenderer>().color = new Color(120f, 0f, 0f, 1f);
        if (soldierDragToThisTile == false) checkSoldierCollider(coll);
        
        

    }


    private void checkSoldierCollider(Collision2D coll)
    {
        if(currentStatus == SC_DefiendVariables.TileStatus.Empty)
        {
            print("from tile = im empty");
            currentStatus = SC_DefiendVariables.TileStatus.BlueOccupied;

        }
        else if(currentStatus == SC_DefiendVariables.TileStatus.BlueOccupied)
        {
            print("from tile = blueOccupied");
            coll.transform.GetComponent<SC_PieceLogic>().occupiedTile();
        }
        else if(currentStatus == SC_DefiendVariables.TileStatus.RedOccupied)
        {

        }

        soldierDragToThisTile = true;
    }




    void OnCollisionExit2D(Collision2D coll)
    {
        //print("collision exit, collision name = " + coll.transform.name);

    }



}
