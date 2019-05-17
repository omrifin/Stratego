using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
//scout can move how much he wants
//when strength hit another strength both of them r dieing
//spy can kill marshel and marshel can kill spy


public class SC_Logic : MonoBehaviour
{
    public gameBoard[][] GameBoard;
    SC_PieceLogic selectedPiece;
    bool pieceHasBeenSelected = false;
    public static SC_DefiendVariables.Turn currentTurn;
    SC_PieceLogic currentHoldingPiece;
    SC_DefiendVariables.Point[] validCoordinates;
    public List<SC_PieceLogic> enemyArray;
    SC_DefiendVariables.TileStatus currentTurnPlayer;
    int enemyArrayCount = 40;
    SC_PieceLogic redWinnerPiece;


    gameBoard currentSelectedPiece;

    #region Single Tone
    public static SC_Logic instance;
    public static SC_Logic Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_Logic").GetComponent<SC_Logic>();
            return instance;
        }
    }
    #endregion

    private void Awake()
    {
        init();
    }

    void init()
    {
        GameBoard = new gameBoard[10][];
        for (int i = 0; i < 10; i++)
            GameBoard[i] = new gameBoard[10];
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                GameBoard[i][j] = new gameBoard();
        currentSelectedPiece = new gameBoard();
        validCoordinates = new SC_DefiendVariables.Point[4];
        for (int i = 0; i < 4; i++)
            validCoordinates[i] = new SC_DefiendVariables.Point();
        enemyArray = new List<SC_PieceLogic>();




        currentTurn = (SC_DefiendVariables.Turn)UnityEngine.Random.Range(0, 2);
        print("CurrentTurn see if its working= " + currentTurn);


    }

    public void CheckIfUserWantsToFight(SC_PieceLogic redPiece, SC_TileLogic tile)
    {
        print("redPiece= " + redPiece.name + " maybe blue= " + currentSelectedPiece.piece.name);

        if (currentSelectedPiece.piece != null)
        {
            if ((Math.Abs(currentSelectedPiece.piece.currentTileRow - redPiece.currentTileRow) == 0) && (Math.Abs(currentSelectedPiece.piece.currentTileCol - redPiece.currentTileCol) == 1)
            || (Math.Abs(currentSelectedPiece.piece.currentTileRow - redPiece.currentTileRow) == 1) && (Math.Abs(currentSelectedPiece.piece.currentTileCol - redPiece.currentTileCol) == 0))

                fight(tile);
           
        }
    }

    void Start()
    {

        DeployEnemyPieces();
        print(" enemyArray.Add(_tmpEnemy)= " + enemyArray.Count);
    }



    private void SearchTile(SC_TileLogic tile)
    {
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
            {
                if (GameBoard[i][j].tile == tile)
                    GameBoard[i][j].tileStatus = SC_DefiendVariables.TileStatus.Empty;
            }
    }




    public void DeployEnemyPieces()
    {
        //EnemyArray = new SC_PieceLogic[40];
        bool findSlot = true;
        int _colRand, _rowRand;
        int RandCount = 0;
        print("SC_Logic.DeployEnemy()");
        for (int i = 0; i < 40; i++)
        {
            SC_PieceLogic _tmpEnemy = SC_Globals.Instance.EnemyPieces["Enemy (" + i + ")"];
            while (findSlot)
            {

                _rowRand = UnityEngine.Random.Range(6, 10);
                _colRand = UnityEngine.Random.Range(0, 10);
                //print("rowRand= " + _rowRand + " _colRand= " + _colRand);

                if (RandCount > 300)
                {
                    for (int j = 6; j < 10; j++)    //rows
                    {
                        for (int k = 0; k < 10; k++)       //columns
                        {
                            if (SC_Logic.Instance.GameBoard[j][k].tileStatus == SC_DefiendVariables.TileStatus.Empty)
                            {
                                SC_PieceLogic _tmpEnemyAfterRand = SC_Globals.Instance.EnemyPieces["Enemy (" + i + j + k + ")"];
                                findSlot = false;
                                SC_Logic.Instance.GameBoard[j][k].tileStatus = SC_DefiendVariables.TileStatus.RedOccupied;
                                SC_Logic.Instance.GameBoard[j][k].piece = _tmpEnemyAfterRand;
                                _tmpEnemyAfterRand.transform.position = SC_Logic.Instance.GameBoard[j][k].tile.transform.position;
                                _tmpEnemyAfterRand.currentTileRow = j;
                                _tmpEnemyAfterRand.currentTileCol = k;
                            }
                        }
                    }
                }

                if (RandCount < 300 && SC_Logic.Instance.GameBoard[_rowRand][_colRand].tileStatus == SC_DefiendVariables.TileStatus.Empty)
                {
                    findSlot = false;
                    SC_Logic.Instance.GameBoard[_rowRand][_colRand].tileStatus = SC_DefiendVariables.TileStatus.RedOccupied;
                    SC_Logic.Instance.GameBoard[_rowRand][_colRand].piece = _tmpEnemy;
                    _tmpEnemy.transform.position = SC_Logic.Instance.GameBoard[_rowRand][_colRand].tile.transform.position;
                    _tmpEnemy.currentTileRow = _rowRand;
                    _tmpEnemy.currentTileCol = _colRand;


                }
                //break;

            }
            enemyArray.Add(_tmpEnemy);
            findSlot = true;
        }

    }


    public void UserPressedPiece(SC_PieceLogic piece)
    {
        {
            if (currentTurn == SC_DefiendVariables.Turn.blueTurn)
            {
                SC_View.Instance.HideValidPlacements();
                ResetValidCoordinates();
                currentSelectedPiece.piece = piece;
               
                if (SC_Globals.GamePhase == SC_Globals.GameSituation.setPieces)
                {
                    MarkValidTiles();

                }

                else if ((SC_Globals.GamePhase == SC_Globals.GameSituation.Running))//&& (piece.canMove == SC_DefiendVariables.Moveable.can))
                {
                    //if (piece.canMove == SC_DefiendVariables.Moveable.can)
                    if (piece.whoAmI == SC_DefiendVariables.whoAmI.Blue)
                        MarkValidTilesAtRunning();
                    else if (piece.whoAmI == SC_DefiendVariables.whoAmI.Red)
                        fight(GameBoard[piece.currentTileRow][piece.currentTileCol].tile.GetComponent<SC_TileLogic>());
                }

            }
        }
    }

    private void ResetValidCoordinates()
    {
        for (int i = 0; i < 4; i++)
        {
            validCoordinates[i].Row = -1;
            validCoordinates[i].Col = -1;
        }

    }

    public void UserPressedTile(SC_TileLogic tile)
    {
        print("currentTile is: " + GameBoard[tile.Row][tile.Col].tileStatus);
        if (currentSelectedPiece.piece != null)
        {
            if ((SC_Globals.GamePhase == SC_Globals.GameSituation.setPieces) && (tile.Row < 4))
            {
                if (GameBoard[tile.Row][tile.Col].tileStatus == SC_DefiendVariables.TileStatus.Empty)
                    if (currentSelectedPiece.piece.currentTileRow != -1 && currentSelectedPiece.piece.currentTileCol != -1)
                        SC_Globals.instance.numOfDeployedBluePieces++;
                MovePieceWhenEmpty(tile);
                if (SC_Globals.instance.numOfDeployedBluePieces == 40)
                    SC_View.Instance.StartButton.SetActive(true);
            }

            else if (SC_Globals.GamePhase == SC_Globals.GameSituation.Running)
            {
                if (currentTurn == SC_DefiendVariables.Turn.blueTurn)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        print("[row]= " + validCoordinates[i].Row + " [Col]= " + validCoordinates[i].Col);

                        if (currentTurn == SC_DefiendVariables.Turn.blueTurn)
                            if ((validCoordinates[i].Row == tile.Row) && (validCoordinates[i].Col == tile.Col))
                            {
                              
                                pieceHasBeenSelected = true;
                                if (GameBoard[tile.Row][tile.Col].tileStatus == SC_DefiendVariables.TileStatus.Empty)
                                {
                                    MovePieceWhenEmpty(tile);
                                    SwitchTurn();
                                   
                                }
                                else if (GameBoard[tile.Row][tile.Col].tileStatus == SC_DefiendVariables.TileStatus.RedOccupied)
                                {
                                    fight(tile);
                                    ResetValidCoordinates();
                                  
                                }

                                
                                //SC_View.Instance.movePieceToNewLocation(currentSelectedPiece.piece, tile);
                                ResetValidCoordinates();
                              
                            }
                    }
                }
            }
        }
    }

    public void SwitchTurn()
    {
        if (currentTurn == SC_DefiendVariables.Turn.RedTurn)
        {
            currentTurn = SC_DefiendVariables.Turn.blueTurn;
            print("currentTurn = " + currentTurn);
        }
        //else if(pieceHasBeenSelected == true)
        else
        {
            currentTurn = SC_DefiendVariables.Turn.RedTurn;
            print("currentTurn = " + currentTurn);
            pieceHasBeenSelected = false;
            RedTurn();
        }
        ResetValidCoordinates();


    }

    public void RedTurn()
    {
        int _row;
        int _col;
        ResetValidCoordinates();
        bool _catchedAvaliblePiece = true, _rnd = true;
        int[] _arrayOfOptions = new int[4];
        int _rndNumber = 0;
        int randomMove = 0;
        SC_DefiendVariables.Point moveTo;
        moveTo = new SC_DefiendVariables.Point();

        if (currentTurn == SC_DefiendVariables.Turn.RedTurn)
        {
            print("before the while");
            while (_catchedAvaliblePiece)
            {
                _rndNumber = UnityEngine.Random.Range(0, enemyArray.Count);
                // _rndNumber = UnityEngine.Random.Range(0, enemyArrayCount);
                _catchedAvaliblePiece = HasValidMove(enemyArray[_rndNumber]);
            }
            print("_rndNumber= " + _rndNumber);
            currentSelectedPiece.piece = enemyArray[_rndNumber];

            while (_rnd)
            {
                randomMove = UnityEngine.Random.Range(0, 4);
                if (validCoordinates[randomMove].Row != -1)
                    _rnd = false;
            }
            //randomMove contain the currect coordinate to move to.
            moveTo.Col = validCoordinates[randomMove].Col;
            moveTo.Row = validCoordinates[randomMove].Row;

            _row = currentSelectedPiece.piece.currentTileRow;
            _col = currentSelectedPiece.piece.currentTileCol;

            if (GameBoard[moveTo.Row][moveTo.Col].tileStatus == SC_DefiendVariables.TileStatus.Empty)
            {
                GameBoard[_row][_col].piece = null;
                GameBoard[_row][_col].tileStatus = SC_DefiendVariables.TileStatus.Empty;
                GameBoard[moveTo.Row][moveTo.Col].tileStatus = SC_DefiendVariables.TileStatus.RedOccupied;
                GameBoard[moveTo.Row][moveTo.Col].piece = currentSelectedPiece.piece;
                currentSelectedPiece.piece.currentTileRow = moveTo.Row;
                currentSelectedPiece.piece.currentTileCol = moveTo.Col;
                SC_View.Instance.movePieceToNewLocation(currentSelectedPiece.piece, GameBoard[moveTo.Row][moveTo.Col].tile.GetComponent<SC_TileLogic>());
            }
            else if (GameBoard[moveTo.Row][moveTo.Col].tileStatus == SC_DefiendVariables.TileStatus.BlueOccupied)
            {
                fight(GameBoard[moveTo.Row][moveTo.Col].tile.GetComponent<SC_TileLogic>());
            }

            SC_View.Instance.HideValidPlacements();
            SwitchTurn();


        }
        print("enemyArray.Count= " + enemyArray.Count);

    }
    public bool HasValidMove(SC_PieceLogic piece)
    {
        if (piece.canMove == SC_DefiendVariables.Moveable.cant) return true;
        int numberOfPossiblie = 0;
        currentSelectedPiece.piece = piece;
        //currentSelectedPiece.tile = GameBoard[piece.currentTileRow][piece.currentTileCol].tile;
        MarkValidTilesAtRunning();
        for (int i = 0; i < 4; i++)
            if (validCoordinates[i].Row != -1)
            {
                numberOfPossiblie++;
            }
        if (numberOfPossiblie > 0)
            return false;
        return true;




    }


    public void fight(SC_TileLogic tile)
    {
        SC_PieceLogic attack = currentSelectedPiece.piece;
        SC_PieceLogic defender = GameBoard[tile.Row][tile.Col].piece;

        print("currentAttacker= " + currentTurn + "[" + attack.currentTileRow + "][" + attack.currentTileCol + "].strengh = " + attack.pieceStrengh);
        print("currentAttacker= " + currentTurn + "[" + defender.currentTileRow + "][" + defender.currentTileCol + "].strengh = " + defender.pieceStrengh);
        if ((attack.pieceStrengh == SC_Globals.SoldierRank.spy) && (defender.pieceStrengh == SC_Globals.SoldierRank.ten))
        {
            //spy captured your marshell
            KillPiece(defender, tile);
            MovePieceWhenDefeatEnemy(attack, tile);
            SwitchTurn();

        }
        else if ((defender.pieceStrengh == SC_Globals.SoldierRank.bomb) && (attack.pieceStrengh == SC_Globals.SoldierRank.three))
        {
            //defuse the bomb!
            KillPiece(defender, tile);
            MovePieceWhenDefeatEnemy(attack, tile);
            SwitchTurn();
        }

        else if (defender.pieceStrengh == SC_Globals.SoldierRank.flag)
        {
            print("GAMEOVER");
            SC_Globals.GamePhase = SC_Globals.GameSituation.Finished;
            SC_View.Instance.EndGamePanel.SetActive(true);
            if (defender.whoAmI == SC_DefiendVariables.whoAmI.Blue)

                SC_View.Instance.EndGameText.GetComponent<Text>().text = "Winner: Red";    //Red won!
            SC_View.Instance.EndGameText.GetComponent<Text>().text = "Winner: Blue";  //blue won!


        }

        else if (defender.pieceStrengh == SC_Globals.SoldierRank.bomb)
        {
            //red steps on your bomb! he need to die!
            KillPiece(defender, tile);
            KillPiece(attack, tile);
            SwitchTurn();
        }
        else if (defender.pieceStrengh == attack.pieceStrengh)
        {
            //same strength! they r both need to die
            KillPiece(defender, tile);
            KillPiece(attack, tile);
            SwitchTurn();
        }
        else
        {
            if (attack.pieceStrengh > defender.pieceStrengh)
            {
                KillPiece(defender, tile);
                MovePieceWhenDefeatEnemy(attack, tile);
                SwitchTurn();
            }
            else
            {
                KillPiece(attack, tile);
                MovePieceWhenDefeatEnemy(defender, tile);
                SwitchTurn();
                if (defender.whoAmI == SC_DefiendVariables.whoAmI.Blue)
                    GameBoard[tile.Row][tile.Col].tileStatus = SC_DefiendVariables.TileStatus.BlueOccupied;

                else
                    GameBoard[tile.Row][tile.Col].tileStatus = SC_DefiendVariables.TileStatus.RedOccupied;

            }
        }

       
        currentSelectedPiece.piece = null;
        SC_View.Instance.HideValidPlacements();



    }
    //private IEnumerator ShowRedPieceStrengh(SC_PieceLogic piece)
    //{
    //    yield return new WaitForSeconds(5);

    //}

    private void ShowRedPieceStrength()
    {
        
        string strength="";
        switch (redWinnerPiece.pieceStrengh)
        {
            case SC_Globals.SoldierRank.spy:
                strength = "S";
                break;
            case SC_Globals.SoldierRank.two:
                strength = "2";
                break;
            case SC_Globals.SoldierRank.three:
                strength = "M";
                break;
            case SC_Globals.SoldierRank.four:
                strength = "4";
                break;
            case SC_Globals.SoldierRank.five:
                strength = "5";
                break;
            case SC_Globals.SoldierRank.six:
                strength = "6";
                break;
            case SC_Globals.SoldierRank.seven:
                strength = "7";
                break;
            case SC_Globals.SoldierRank.eight:
                strength = "8";
                break;
            case SC_Globals.SoldierRank.nine:
                strength = "9";
                break;
            case SC_Globals.SoldierRank.ten:
                strength = "10";
                break;
            case SC_Globals.SoldierRank.bomb:
                strength = "B";         //make bomb
                break;
            case SC_Globals.SoldierRank.flag:
                strength = "f";
                break;
        }



        //piece.GetComponent<Text>().text = strength;
        redWinnerPiece.GetComponentInChildren<TextMesh>().text = strength;
        //piece.GetComponent<Text>().text = "";
        //return null;
    }

    
    private void MovePieceWhenDefeatEnemy(SC_PieceLogic attack,SC_TileLogic tile)
    {
        GameBoard[attack.currentTileRow][attack.currentTileCol].piece = null;
        GameBoard[tile.Row][tile.Col].tileStatus = GameBoard[attack.currentTileRow][attack.currentTileCol].tileStatus;
        GameBoard[attack.currentTileRow][attack.currentTileCol].tileStatus = SC_DefiendVariables.TileStatus.Empty;
        attack.currentTileRow = tile.Row;
        attack.currentTileCol = tile.Col;
        GameBoard[tile.Row][tile.Col].piece = attack;
        if (attack.whoAmI == SC_DefiendVariables.whoAmI.Blue)
            SC_View.Instance.movePieceToNewLocation(attack, tile);


        if (attack.whoAmI == SC_DefiendVariables.whoAmI.Red)
        {
            SC_View.Instance.movePieceToNewLocation(attack, tile);
            //if(attack.whoAmI == SC_DefiendVariables.whoAmI.Red)
            //StartCoroutine(ShowRedPieceStrengh(attack));
            redWinnerPiece = attack;
            ShowRedPieceStrength();

        }

    }

    private void KillPiece(SC_PieceLogic defender,SC_TileLogic tile)
    {
        print("KillPiece");
        GameBoard[defender.currentTileRow][defender.currentTileCol].tileStatus = SC_DefiendVariables.TileStatus.Empty;
        GameBoard[defender.currentTileRow][defender.currentTileCol].piece = null;
        GameBoard[tile.Row][tile.Col].tileStatus = SC_DefiendVariables.TileStatus.Empty;
        GameBoard[tile.Row][tile.Col].piece = null;
        defender.currentTileRow = -1;
        defender.currentTileCol = -1;
        SC_View.Instance.KillPiece(defender);
        if (defender.whoAmI == SC_DefiendVariables.whoAmI.Red)
        {
            enemyArray.Remove(defender);
            //enemyArrayCount--;
        }



    }



    private void GameOver(SC_TileLogic tile)
    {
        //print(v);
    }

    public void UserPressedDeck()
    {
        if (currentSelectedPiece.piece != null)
        {
            GameBoard[currentSelectedPiece.piece.currentTileRow][currentSelectedPiece.piece.currentTileCol].piece = null;
            GameBoard[currentSelectedPiece.piece.currentTileRow][currentSelectedPiece.piece.currentTileCol].tileStatus= SC_DefiendVariables.TileStatus.Empty;
            currentSelectedPiece.piece.currentTileRow = -1;
            currentSelectedPiece.piece.currentTileCol = -1;
            SC_Globals.instance.numOfDeployedBluePieces--;
            if (SC_Globals.instance.numOfDeployedBluePieces < 40)
                SC_View.Instance.StartButton.SetActive(false);

        }
    }

   

    private void MarkValidTiles(){
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 10; j++)
                    if (GameBoard[i][j].tileStatus == SC_DefiendVariables.TileStatus.Empty)
                        SC_View.Instance.ShowValidPlacements(GameBoard[i][j].tile);
    }
   

    //Mark all valid tiles for the specific tile and save it on the Array: validCoordinates
    #region CheckTilesValidAfter Running
    private void MarkValidTilesAtRunning()
    {
        int _row = currentSelectedPiece.piece.currentTileRow;
        int _col = currentSelectedPiece.piece.currentTileCol;
        //print("currentSelectedPiece.piece.canMove = " + currentSelectedPiece.piece.canMove);
        if (currentSelectedPiece.piece.canMove == SC_DefiendVariables.Moveable.can)
        {
            if ((CheckIfTileIsNotWater(_row + 1, _col) && (_row + 1 < 10) && (GameBoard[_row + 1][_col].tileStatus != GameBoard[_row][_col].tileStatus)))//SC_DefiendVariables.TileStatus.BlueOccupied)))
            {   //to the right
                SC_View.Instance.ShowValidPlacements(GameBoard[_row + 1][_col].tile);
                validCoordinates[0].Row = _row + 1;
                validCoordinates[0].Col = _col;
            }
            if ((CheckIfTileIsNotWater(_row - 1, _col) && (_row - 1 >= 0) && (GameBoard[_row - 1][_col].tileStatus != GameBoard[_row][_col].tileStatus)))//// to the left
            {
                SC_View.Instance.ShowValidPlacements(GameBoard[_row - 1][_col].tile);
                validCoordinates[1].Row = _row - 1;
                validCoordinates[1].Col = _col;
            }
            if ((CheckIfTileIsNotWater(_row, _col + 1) && (_col + 1 < 10) && (GameBoard[_row][_col + 1].tileStatus != GameBoard[_row][_col].tileStatus)))//  //higher
            {
                SC_View.Instance.ShowValidPlacements(GameBoard[_row][_col + 1].tile);
                validCoordinates[2].Row = _row;
                validCoordinates[2].Col = _col + 1;
            }
            if ((CheckIfTileIsNotWater(_row, _col - 1) && (_col - 1 >= 0) && (GameBoard[_row][_col - 1].tileStatus != GameBoard[_row][_col].tileStatus)))//  //lower
            {
                SC_View.Instance.ShowValidPlacements(GameBoard[_row][_col - 1].tile);
                validCoordinates[3].Row = _row;
                validCoordinates[3].Col = _col - 1;

            }
        }
    }
    public bool CheckIfTileIsNotWater(int i, int j)
    {

        if (i == 4 && j == 2 || i == 4 && j == 3 || i == 5 && j == 2 || i == 5 && j == 3 || i == 4 && j == 6 || i == 5 && j == 7 || i == 5 && j == 6 || i == 4 && j == 7)
            return false;
        return true;
    }
    #endregion

    public void MovePieceWhenEmpty(SC_TileLogic tile)
    {
        GameBoard[tile.Row][tile.Col].tileStatus = SC_DefiendVariables.TileStatus.BlueOccupied;
        GameBoard[tile.Row][tile.Col].piece = currentSelectedPiece.piece;
        SC_View.Instance.movePieceToNewLocation(currentSelectedPiece.piece, tile);
        if (currentSelectedPiece.piece.currentTileRow != -1)
        {
            GameBoard[currentSelectedPiece.piece.currentTileRow][currentSelectedPiece.piece.currentTileCol].piece = null;
            GameBoard[currentSelectedPiece.piece.currentTileRow][currentSelectedPiece.piece.currentTileCol].tileStatus = SC_DefiendVariables.TileStatus.Empty;
        }
        currentSelectedPiece.piece.currentTileRow = tile.Row;
        currentSelectedPiece.piece.currentTileCol = tile.Col;

        //GameBoard[currentSelectedPiece.piece.currentTileRow = 
        currentSelectedPiece.piece = null;
        currentSelectedPiece.tile = null;

        SC_View.Instance.HideValidPlacements();
    }

   
}

