using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_MultiPlayer_PieceLogic : MonoBehaviour
{

    bool OnValidTile;
    public SC_MultiPlayer_Globals.SoldierRank pieceStrengh;
    public bool onTile;
    public Vector2 OriginalCoordinates;
    Vector2 startPoint;
    float x, y;
    bool drag = false;
    SC_DefiendVariables.TileStatus tileStatus;
    bool initializeAssignToTile = false;
    bool iAmMoving = false;
    public SC_DefiendVariables.whoAmI whoAmI;
    public int currentTileRow = -1;
    public int currentTileCol = -1;
    public SC_DefiendVariables.Moveable canMove = SC_DefiendVariables.Moveable.can;
    public static bool playersInteractable = true;





    private void init()
    {
        OnValidTile = false;
        onTile = false;
        OriginalCoordinates = new Vector2(transform.position.x, transform.position.y);
        startPoint = transform.position;
        if (pieceStrengh == SC_MultiPlayer_Globals.SoldierRank.bomb) this.canMove = SC_DefiendVariables.Moveable.cant;
        if (pieceStrengh == SC_MultiPlayer_Globals.SoldierRank.flag) this.canMove = SC_DefiendVariables.Moveable.cant;

    }
    void Start()
    {
        SC_MultiPlayer_PieceLogic.playersInteractable = true;
    }
    private void Awake()
    {
        init();
    }



    private void OnMouseDown()
    {
        print("clicked me");
        //if (SC_MultiPlayer_PieceLogic.playersInteractable == true)
        //{
            if ((SC_MultiPlayer_Logic.currentTurn == SC_DefiendVariables.Turn.blueTurn) || (SC_MultiPlayer_Globals.GamePhase == SC_MultiPlayer_Globals.GameSituation.setPieces))//&&(whoAmI == SC_DefiendVariables.whoAmI.Blue))
            {
                if (whoAmI == SC_DefiendVariables.whoAmI.Blue)
                    SC_MultiPlayer_Controller.Instance.UserPressedPiece(this);
                else if (SC_MultiPlayer_Globals.GamePhase == SC_MultiPlayer_Globals.GameSituation.Running)
                    SC_MultiPlayer_Controller.Instance.UserWantsToFIght(this);
            }
            else print("cant touch me!");
       // }
        //else print("cant touch me!");
        
    }


    public void checkTileValid(Collision2D coll)
    {

    }

    public void occupiedTile()
    {

    }

    public void goBack()
    {
        print("SC_MultiPlayer_PieceLogic.GoBack()");
        transform.position = startPoint;
    }


}



