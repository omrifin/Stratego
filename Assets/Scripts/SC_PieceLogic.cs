using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_PieceLogic : MonoBehaviour
{

    bool OnValidTile;
    public SC_Globals.SoldierRank pieceStrengh;
    public bool onTile;
    public Vector2 OriginalCoordinates;
    Vector2 startPoint;
    float x, y;
    bool drag = false;
    SC_DefiendVariables.TileStatus tileStatus;
    bool initializeAssignToTile = false;
    bool iAmMoving = false;
    public SC_DefiendVariables.whoAmI whoAmI;
    public int currentTileRow=-1;
    public int currentTileCol=-1;
    public SC_DefiendVariables.Moveable canMove= SC_DefiendVariables.Moveable.can;





    private void init()
    {
        OnValidTile = false;
        onTile = false;
        OriginalCoordinates = new Vector2(transform.position.x, transform.position.y);
        startPoint = transform.position;
        if (pieceStrengh == SC_Globals.SoldierRank.bomb) this.canMove = SC_DefiendVariables.Moveable.cant;
        if (pieceStrengh == SC_Globals.SoldierRank.flag) this.canMove = SC_DefiendVariables.Moveable.cant;

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
           SC_Controller.Instance.UserPressedPiece(this);
      else if(SC_Globals.GamePhase == SC_Globals.GameSituation.Running)
            SC_Controller.Instance.UserWantsToFIght(this);
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
        transform.position = startPoint;
    }

 
}



