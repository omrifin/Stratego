using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_PieceLogic : MonoBehaviour
{

    bool OnValidTile;
    public SoldierRank currentStatus;
    bool onTile;
    Vector2 OriginalCoordinates;
    Vector2 startPoint;
    float x, y;
    bool drag = false;
    SC_DefiendVariables.TileStatus tileStatus;
    bool initializeAssignToTile = false;
    bool iAmMoving = false;


    
   public enum SoldierRank
    {
        spy,two,three,four,five,six,seven,eight,nine,ten,bomb,flag
    }

    private void init()
    {
        string num = this.GetComponentInChildren<TextMesh>().text;
        OnValidTile = false;
        onTile = false;
        OriginalCoordinates = new Vector2(transform.position.x, transform.position.y);
        startPoint = transform.position;
    }
	void Start () {
		
	}
    private void Awake()
    {
        init();
    }



    private void OnMouseDown()
    {


    }
    private void OnCollisionEnter2D(Collision2D coll)
    {


    }

    public void checkTileValid(Collision2D coll)
    {
    
    }

    //fight() gets called when the tile is occupied by another enemy soldier
    //parameter player is  - "blue player"
    private void fight(GameObject player)
    {
        print("fight!");
    }

    private void OnMouseDrag()
    {
   
    }
    private void OnMouseExit()
    {
       // transform.position = startPoint;

    }
    public void occupiedTile()
    {

    }
    

    private void OnMouseUpAsButton()
    {


    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("name = " + this.name);
            SC_Logic.Instance.checkValidSlots(this);
        }
    }


}



