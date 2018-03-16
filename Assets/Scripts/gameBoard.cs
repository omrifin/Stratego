using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameBoard 
{
    public SC_DefiendVariables.TileStatus tileStatus;      //Empty, BlueOccupied, RedOccupied
    public SC_PieceLogic piece;
    public static int fillPieces=0;
    public GameObject tile;

    public gameBoard()
    {
        tileStatus = SC_DefiendVariables.TileStatus.Empty;
        piece = null;
        tile = null;
    }

}
