using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_DefiendVariables : MonoBehaviour {

    public class Point
    {
        public static float bet;
        int row;
        int col;
        public int Row
        {
            get
            {
                return row;
            }
            set
            {
                row = value;
            }
        }
        public int Col
        {
            get
            {
                return col;
            }
            set
            {
                col = value;
            }
        }

        public Point()
        {
            row = -1;
            col = -1;
        }

    }
    public static Dictionary<string, object> bet  = new Dictionary<string, object>();
    public enum Moveable
    {
        can,cant
    }
    public enum TileStatus
    {
        RedOccupied,
        BlueOccupied,
        Empty
    }
    public enum Turn
    {
        blueTurn,
        RedTurn
    }
    public enum whoAmI
    {
        Red,Blue
    }


}
