using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MultiPlayer_gameBoard
{
    public SC_DefiendVariables.TileStatus tileStatus;      //Empty, BlueOccupied, RedOccupied
    public SC_MultiPlayer_PieceLogic piece;
    public static int fillPieces = 0;
    public GameObject tile;

    public SC_MultiPlayer_gameBoard()
    { 
        tileStatus = SC_DefiendVariables.TileStatus.Empty;
        piece = null;
        tile = null;
    }

}
