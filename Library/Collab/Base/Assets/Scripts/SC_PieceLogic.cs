using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_PieceLogic : MonoBehaviour
{

    bool OnValidTile;
    public SC_Globals.SoldierRank pieceStrengh;
    bool onTile;
    Vector2 OriginalCoordinates;
    Vector2 startPoint;
    float x, y;
    bool drag = false;
    SC_DefiendVariables.TileStatus tileStatus;
    bool initializeAssignToTile = false;
    bool iAmMoving = false;
    public SC_DefiendVariables.whoAmI whoAmI;
    public int currentTileRow=-1;
    public int currentTileCol=-1;





    private void init()
    {
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
        if (whoAmI == SC_DefiendVariables.whoAmI.Blue)
        {
            //SC_Logic.Instance.checkValidSlots(this);
            SC_Controller.Instance.UserPressedPiece(this);
        }
    }


    public void checkTileValid(Collision2D coll)
    {
    
    }

    public void occupiedTile()
    {

    }
    
    public void goBack()
    {
        print("SC_PieceLogic.GoBack()");
        //transform.position = startPoint;
    }

 
}



