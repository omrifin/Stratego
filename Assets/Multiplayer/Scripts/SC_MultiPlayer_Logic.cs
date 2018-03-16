using AssemblyCSharp;
using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
//scout can move how much he wants
//when strength hit another strength both of them r dieing
//spy can kill marshel and marshel can kill spy


public class SC_MultiPlayer_Logic : MonoBehaviour
{
    public class point
    {
        public int row = -1;
        public int col = -1;
    }

    public SC_MultiPlayer_gameBoard[][] gameBoard;
    SC_MultiPlayer_PieceLogic selectedPiece;
    bool pieceHasBeenSelected = false;
    public static SC_DefiendVariables.Turn currentTurn;
    SC_MultiPlayer_PieceLogic currentHoldingPiece;
    SC_DefiendVariables.Point[] validCoordinates;
    public List<SC_MultiPlayer_PieceLogic> enemyArray;
    SC_DefiendVariables.TileStatus currentTurnPlayer;
    int enemyArrayCount = 40;
    SC_MultiPlayer_PieceLogic redWinnerPiece;
    bool roomMatch = false;


    SC_MultiPlayer_gameBoard currentSelectedPiece;

    #region SingleTon
    public static SC_MultiPlayer_Logic instance;
    public bool isMyTurn;

    public static SC_MultiPlayer_Logic Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_MultiPlayer_Logic").GetComponent<SC_MultiPlayer_Logic>();
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
        gameBoard = new SC_MultiPlayer_gameBoard[10][];
        for (int i = 0; i < 10; i++)
            gameBoard[i] = new SC_MultiPlayer_gameBoard[10];
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                gameBoard[i][j] = new SC_MultiPlayer_gameBoard();
        currentSelectedPiece = new SC_MultiPlayer_gameBoard();
        validCoordinates = new SC_DefiendVariables.Point[4];
        for (int i = 0; i < 4; i++)
            validCoordinates[i] = new SC_DefiendVariables.Point();
        enemyArray = new List<SC_MultiPlayer_PieceLogic>();





        currentTurn = (SC_DefiendVariables.Turn)UnityEngine.Random.Range(0, 2);     //change it later
        initMultiplayer();

    }

    public void CheckIfUserWantsToFight(SC_MultiPlayer_PieceLogic redPiece, SC_MultiPlayer_TileLogic tile)
    {
        print("redPiece= " + redPiece.name + " maybe blue= " + currentSelectedPiece.piece.name);

        if (currentSelectedPiece.piece != null)
        {
            if ((Math.Abs(currentSelectedPiece.piece.currentTileRow - redPiece.currentTileRow) == 0) && (Math.Abs(currentSelectedPiece.piece.currentTileCol - redPiece.currentTileCol) == 1)
            || (Math.Abs(currentSelectedPiece.piece.currentTileRow - redPiece.currentTileRow) == 1) && (Math.Abs(currentSelectedPiece.piece.currentTileCol - redPiece.currentTileCol) == 0))

                fight(tile);
            SwitchTurn();
        }
    }

    void Start()
    {

        //DeployEnemyPieces();
        DeployEnemyPieces();
        print(" enemyArray.Add(_tmpEnemy)= " + enemyArray.Count);
       
    }



    private void SearchTile(SC_MultiPlayer_TileLogic tile)
    {
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
            {
                if (gameBoard[i][j].tile == tile)
                    gameBoard[i][j].tileStatus = SC_DefiendVariables.TileStatus.Empty;
            }
    }




    public void DeployEnemyPieces()
    {
        bool findSlot = true;
        int _colRand, _rowRand;
        int RandCount = 0;
        int j = 6, k = 0;
        print("SC_MultiPlayer_Logic.DeployEnemy()");
        for (int i = 0; i < 40; i++)
        {
            SC_MultiPlayer_PieceLogic _tmpEnemy = SC_MultiPlayer_Globals.Instance.EnemyPieces["Enemy (" + i + ")"];
            SC_MultiPlayer_Logic.Instance.gameBoard[j][k].tileStatus = SC_DefiendVariables.TileStatus.RedOccupied;
            SC_MultiPlayer_Logic.Instance.gameBoard[j][k].piece = _tmpEnemy;
            _tmpEnemy.transform.position = SC_MultiPlayer_Logic.Instance.gameBoard[j][k].tile.transform.position;
            _tmpEnemy.currentTileRow = j;
            _tmpEnemy.currentTileCol = k;

            if (k == 9)
            {
                k = 0;
                j++;
            }
            else k++;




            enemyArray.Add(_tmpEnemy);
            findSlot = true;
        }

    }


    public void UserPressedPiece(SC_MultiPlayer_PieceLogic piece)
    {
        if (currentTurn == SC_DefiendVariables.Turn.blueTurn)
        {
            SC_MultiPlayer_View.Instance.HideValidPlacements();
            ResetValidCoordinates();
            currentSelectedPiece.piece = piece;
            //if (piece.onTile)
            //    currentSelectedPiece.tile = gameBoard[piece.currentTileRow][piece.currentTileCol].tile;
            if (SC_MultiPlayer_Globals.GamePhase == SC_MultiPlayer_Globals.GameSituation.setPieces)
            {
                MarkValidTiles();

            }

            else if ((SC_MultiPlayer_Globals.GamePhase == SC_MultiPlayer_Globals.GameSituation.Running))//&& (piece.canMove == SC_DefiendVariables.Moveable.can))
            {
                //if (piece.canMove == SC_DefiendVariables.Moveable.can)
                if (piece.whoAmI == SC_DefiendVariables.whoAmI.Blue)
                    MarkValidTilesAtRunning();
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
    public void SendMove(SC_MultiPlayer_TileLogic tile)
    {
        if (isMyTurn)
        {
            print("SendMove");
            Dictionary<string, object> _moveToSend = new Dictionary<string, object>();

            point _from = new point();
            point _to = new point();
            _to.row = tile.row;
            _to.col = tile.col;
            _from.row = currentSelectedPiece.piece.currentTileRow;
            _from.col = currentSelectedPiece.piece.currentTileCol;
            _to.row = tile.Row;
            _to.col = tile.Col;

            SC_MultiPlayer_Globals.SoldierRank strength = currentSelectedPiece.piece.pieceStrengh;
            _moveToSend.Add("fromRow", _from.row);
            _moveToSend.Add("fromCol", _from.col);

            _moveToSend.Add("toRow", _to.row);
            _moveToSend.Add("toCol", _to.col);

            _moveToSend.Add("strength", strength);

            string _jsonToSend = MiniJSON.Json.Serialize(_moveToSend);
            Debug.Log(_jsonToSend);
            WarpClient.GetInstance().sendMove(_jsonToSend);
        }

    }

    //opponet move

    public void OnMoveCompleted(MoveEvent _Move)
    {
        Debug.Log("OnMoveCompleted " + _Move.getMoveData() + " " + _Move.getNextTurn() + " " + _Move.getSender());
        if (_Move.getSender() !=SC_MultiPlayer_Globals.userName && _Move.getMoveData() != null)
        {
          
            Dictionary<string, object> _recievedData = MiniJSON.Json.Deserialize(_Move.getMoveData()) as Dictionary<string, object>;
            if (_recievedData != null)
            {
                //int _index = int.Parse(_recievedData["Data"].ToString());
                //  SubmitLogic(_index);
                print("_recievedData= fromCol" + _recievedData["fromCol"] + " toCol= " + _recievedData["toCol"]);
                ProccessMove(_recievedData);
            }
        }

       // print("_Move.getNextTurn()" + _Move.getNextTurn() + "    +++     userName" + SC_MultiPlayer_Globals.userName);
        //if (_Move.getNextTurn() == SC_MultiPlayer_Globals.userName)
        //    currentTurn = SC_DefiendVariables.Turn.RedTurn;
        //else currentTurn = SC_DefiendVariables.Turn.blueTurn;

        if (_Move.getNextTurn() == SC_MultiPlayer_Globals.userName)
            isMyTurn = true;
        else isMyTurn = false;
    }

    private void ProccessMove(Dictionary<string, object> recievedData)
    {
        int _toCol, _fromCol, _toRow, _fromRow;
        //SC_MultiPlayer_Globals.SoldierRank strength = (SC_MultiPlayer_Globals.SoldierRank) recievedData["strength"];
        SC_MultiPlayer_Globals.SoldierRank strength = (SC_MultiPlayer_Globals.SoldierRank)Enum.Parse(typeof(SC_MultiPlayer_Globals.SoldierRank),recievedData["strength"].ToString());



        _fromCol = 9 - Convert.ToInt32(recievedData["fromCol"]);
        _fromRow = 9 - Convert.ToInt32(recievedData["fromRow"]);
        _toRow = 9 - Convert.ToInt32(recievedData["toRow"]);
        _toCol = 9 - Convert.ToInt32(recievedData["toCol"]);

        print("fromRow= " + _fromRow+" fromCol= "+_fromCol + " toRow" + _toRow+ " toCol= "+_toCol);
        currentSelectedPiece.piece = gameBoard[_fromRow][_fromCol].piece;
        EnemyTurn(_toRow, _toCol, _fromRow, _fromCol);



    }

    public void UserPressedTile(SC_MultiPlayer_TileLogic tile)
    {
        print("currentTile is: " + gameBoard[tile.Row][tile.Col].tileStatus);
        if (currentSelectedPiece.piece != null)
        {
            if ((SC_MultiPlayer_Globals.GamePhase == SC_MultiPlayer_Globals.GameSituation.setPieces) && (tile.Row < 4))
            {
                if (gameBoard[tile.Row][tile.Col].tileStatus == SC_DefiendVariables.TileStatus.Empty)
                    if (currentSelectedPiece.piece.currentTileRow != -1 && currentSelectedPiece.piece.currentTileCol != -1)
                        SC_MultiPlayer_Globals.instance.numOfDeployedBluePieces++;
                MovePieceWhenEmpty(tile);
                if (SC_MultiPlayer_Globals.instance.numOfDeployedBluePieces == 40)
                    SC_MultiPlayer_View.Instance.StartButton.SetActive(true);
            }

            else if (SC_MultiPlayer_Globals.GamePhase == SC_MultiPlayer_Globals.GameSituation.Running)
            {
                SendMove(tile);
                if (currentTurn == SC_DefiendVariables.Turn.blueTurn)
                {
                    for (int i = 0; i < 4; i++)
                    {
                       // print("[row]= " + validCoordinates[i].Row + " [Col]= " + validCoordinates[i].Col);

                        if (currentTurn == SC_DefiendVariables.Turn.blueTurn)
                            if ((validCoordinates[i].Row == tile.Row) && (validCoordinates[i].Col == tile.Col))
                            {

                               
                                //print("[Tile.row]= " + tile.Row + " [Tile.Col]= " + tile.Col);
                                pieceHasBeenSelected = true;
                                if (gameBoard[tile.Row][tile.Col].tileStatus == SC_DefiendVariables.TileStatus.Empty)
                                    MovePieceWhenEmpty(tile);
                                else if (gameBoard[tile.Row][tile.Col].tileStatus == SC_DefiendVariables.TileStatus.RedOccupied)
                                {

                                    fight(tile);
                                    ResetValidCoordinates();
                                }


                                //SC_MultiPlayer_View.Instance.movePieceToNewLocation(currentSelectedPiece.piece, tile);
                                ResetValidCoordinates();
                              //  SwitchTurn();
                            }
                    }
                }
            }
        }
    }

    public void SwitchTurn()
    {
        print("you called me!");
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

        }
        ResetValidCoordinates();


    }

    public void TurnTimeOut()
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

            if (gameBoard[moveTo.Row][moveTo.Col].tileStatus == SC_DefiendVariables.TileStatus.Empty)
            {
                gameBoard[_row][_col].piece = null;
                gameBoard[_row][_col].tileStatus = SC_DefiendVariables.TileStatus.Empty;
                gameBoard[moveTo.Row][moveTo.Col].tileStatus = SC_DefiendVariables.TileStatus.RedOccupied;
                gameBoard[moveTo.Row][moveTo.Col].piece = currentSelectedPiece.piece;
                currentSelectedPiece.piece.currentTileRow = moveTo.Row;
                currentSelectedPiece.piece.currentTileCol = moveTo.Col;
                SC_MultiPlayer_View.Instance.movePieceToNewLocation(currentSelectedPiece.piece, gameBoard[moveTo.Row][moveTo.Col].tile.GetComponent<SC_MultiPlayer_TileLogic>());
            }
            else if (gameBoard[moveTo.Row][moveTo.Col].tileStatus == SC_DefiendVariables.TileStatus.BlueOccupied)
            {
                fight(gameBoard[moveTo.Row][moveTo.Col].tile.GetComponent<SC_MultiPlayer_TileLogic>());
            }

            SC_MultiPlayer_View.Instance.HideValidPlacements();
            SwitchTurn();


        }
        print("enemyArray.Count= " + enemyArray.Count);
    }


    public bool HasValidMove(SC_MultiPlayer_PieceLogic piece)
    {
        if (piece.canMove == SC_DefiendVariables.Moveable.cant) return true;
        int numberOfPossiblie = 0;
        currentSelectedPiece.piece = piece;
        //currentSelectedPiece.tile = gameBoard[piece.currentTileRow][piece.currentTileCol].tile;
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


    public void fight(SC_MultiPlayer_TileLogic tile)
    {
        SC_MultiPlayer_PieceLogic attack = currentSelectedPiece.piece;
        SC_MultiPlayer_PieceLogic defender = gameBoard[tile.Row][tile.Col].piece;

        print("currentAttacker= " + currentTurn + "[" + attack.currentTileRow + "][" + attack.currentTileCol + "].strengh = " + attack.pieceStrengh);
        print("currentAttacker= " + currentTurn + "[" + defender.currentTileRow + "][" + defender.currentTileCol + "].strengh = " + defender.pieceStrengh);
        if ((attack.pieceStrengh == SC_MultiPlayer_Globals.SoldierRank.spy) && (defender.pieceStrengh == SC_MultiPlayer_Globals.SoldierRank.ten))
        {
            //spy captured your marshell
            KillPiece(defender, tile);
            MovePieceWhenDefeatEnemy(attack, tile);

        }
        else if ((defender.pieceStrengh == SC_MultiPlayer_Globals.SoldierRank.bomb) && (attack.pieceStrengh == SC_MultiPlayer_Globals.SoldierRank.three))
        {
            //defuse the bomb!
            KillPiece(defender, tile);
            MovePieceWhenDefeatEnemy(attack, tile);
        }

        else if (defender.pieceStrengh == SC_MultiPlayer_Globals.SoldierRank.flag)
        {
            print("GAMEOVER");
            SC_MultiPlayer_Globals.GamePhase = SC_MultiPlayer_Globals.GameSituation.Finished;
            SC_MultiPlayer_View.Instance.EndGamePanel.SetActive(true);
            if (defender.whoAmI == SC_DefiendVariables.whoAmI.Blue)

                SC_MultiPlayer_View.Instance.EndGameText.GetComponent<Text>().text = "Winner: Red";    //Red won!
            SC_MultiPlayer_View.Instance.EndGameText.GetComponent<Text>().text = "Winner: Blue";  //blue won!


        }

        else if (defender.pieceStrengh == SC_MultiPlayer_Globals.SoldierRank.bomb)
        {
            //red steps on your bomb! he need to die!
            KillPiece(defender, tile);
            KillPiece(attack, tile);
        }
        else if (defender.pieceStrengh == attack.pieceStrengh)
        {
            //same strength! they r both need to die
            KillPiece(defender, tile);
            KillPiece(attack, tile);
        }
        else
        {
            if (attack.pieceStrengh > defender.pieceStrengh)
            {
                KillPiece(defender, tile);
                MovePieceWhenDefeatEnemy(attack, tile);
            }
            else
            {
                KillPiece(attack, tile);
                MovePieceWhenDefeatEnemy(defender, tile);
            }
        }

        SwitchTurn();
        currentSelectedPiece.piece = null;
        SC_MultiPlayer_View.Instance.HideValidPlacements();



    }
    //private IEnumerator ShowRedPieceStrengh(SC_MultiPlayer_PieceLogic piece)
    //{
    //    yield return new WaitForSeconds(5);

    //}

    private void ShowRedPieceStrength()
    {

        string strength = "";
        switch (redWinnerPiece.pieceStrengh)
        {
            case SC_MultiPlayer_Globals.SoldierRank.spy:
                strength = "S";
                break;
            case SC_MultiPlayer_Globals.SoldierRank.two:
                strength = "2";
                break;
            case SC_MultiPlayer_Globals.SoldierRank.three:
                strength = "M";
                break;
            case SC_MultiPlayer_Globals.SoldierRank.four:
                strength = "4";
                break;
            case SC_MultiPlayer_Globals.SoldierRank.five:
                strength = "5";
                break;
            case SC_MultiPlayer_Globals.SoldierRank.six:
                strength = "6";
                break;
            case SC_MultiPlayer_Globals.SoldierRank.seven:
                strength = "7";
                break;
            case SC_MultiPlayer_Globals.SoldierRank.eight:
                strength = "8";
                break;
            case SC_MultiPlayer_Globals.SoldierRank.nine:
                strength = "9";
                break;
            case SC_MultiPlayer_Globals.SoldierRank.ten:
                strength = "10";
                break;
            case SC_MultiPlayer_Globals.SoldierRank.bomb:
                strength = "B";         //make bomb
                break;
            case SC_MultiPlayer_Globals.SoldierRank.flag:
                strength = "f";
                break;
        }

        redWinnerPiece.GetComponentInChildren<TextMesh>().text = strength;
        //piece.GetComponent<Text>().text = "";
        //return null;
    }


    private void MovePieceWhenDefeatEnemy(SC_MultiPlayer_PieceLogic attack, SC_MultiPlayer_TileLogic tile)
    {
        gameBoard[attack.currentTileRow][attack.currentTileCol].piece = null;
        gameBoard[tile.Row][tile.Col].tileStatus = gameBoard[attack.currentTileRow][attack.currentTileCol].tileStatus;
        gameBoard[attack.currentTileRow][attack.currentTileCol].tileStatus = SC_DefiendVariables.TileStatus.Empty;
        attack.currentTileRow = tile.Row;
        attack.currentTileCol = tile.Col;
        gameBoard[tile.Row][tile.Col].piece = attack;


        if (attack.whoAmI == SC_DefiendVariables.whoAmI.Red)
        {
            SC_MultiPlayer_View.Instance.movePieceToNewLocation(attack, tile);
            //if(attack.whoAmI == SC_DefiendVariables.whoAmI.Red)
            //StartCoroutine(ShowRedPieceStrengh(attack));
            redWinnerPiece = attack;
            ShowRedPieceStrength();

        }

    }

    private void KillPiece(SC_MultiPlayer_PieceLogic defender, SC_MultiPlayer_TileLogic tile)
    {
        gameBoard[defender.currentTileRow][defender.currentTileCol].tileStatus = SC_DefiendVariables.TileStatus.Empty;
        gameBoard[defender.currentTileRow][defender.currentTileCol].piece = null;
        gameBoard[tile.Row][tile.Col].tileStatus = SC_DefiendVariables.TileStatus.Empty;
        gameBoard[tile.Row][tile.Col].piece = null;
        defender.currentTileRow = -1;
        defender.currentTileCol = -1;
        SC_MultiPlayer_View.Instance.KillPiece(defender);
        if (defender.whoAmI == SC_DefiendVariables.whoAmI.Red)
        {
            enemyArray.Remove(defender);
            //enemyArrayCount--;
        }



    }



    private void GameOver(SC_MultiPlayer_TileLogic tile)
    {
        //print(v);
    }

    public void UserPressedSC_MultiPlayer_Deck()
    {
        if (currentSelectedPiece.piece != null)
        {
            gameBoard[currentSelectedPiece.piece.currentTileRow][currentSelectedPiece.piece.currentTileCol].piece = null;
            gameBoard[currentSelectedPiece.piece.currentTileRow][currentSelectedPiece.piece.currentTileCol].tileStatus = SC_DefiendVariables.TileStatus.Empty;
            currentSelectedPiece.piece.currentTileRow = -1;
            currentSelectedPiece.piece.currentTileCol = -1;
            SC_MultiPlayer_Globals.instance.numOfDeployedBluePieces--;
            if (SC_MultiPlayer_Globals.instance.numOfDeployedBluePieces < 40)
                SC_MultiPlayer_View.Instance.StartButton.SetActive(false);

        }
    }



    private void MarkValidTiles()
    {
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 10; j++)
                if (gameBoard[i][j].tileStatus == SC_DefiendVariables.TileStatus.Empty)
                    SC_MultiPlayer_View.Instance.ShowValidPlacements(gameBoard[i][j].tile);
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
            if ((CheckIfTileIsNotWater(_row + 1, _col) && (_row + 1 < 10) && (gameBoard[_row + 1][_col].tileStatus != gameBoard[_row][_col].tileStatus)))//SC_DefiendVariables.TileStatus.BlueOccupied)))
            {   //to the right
                SC_MultiPlayer_View.Instance.ShowValidPlacements(gameBoard[_row + 1][_col].tile);
                validCoordinates[0].Row = _row + 1;
                validCoordinates[0].Col = _col;
            }
            if ((CheckIfTileIsNotWater(_row - 1, _col) && (_row - 1 >= 0) && (gameBoard[_row - 1][_col].tileStatus != gameBoard[_row][_col].tileStatus)))//// to the left
            {
                SC_MultiPlayer_View.Instance.ShowValidPlacements(gameBoard[_row - 1][_col].tile);
                validCoordinates[1].Row = _row - 1;
                validCoordinates[1].Col = _col;
            }
            if ((CheckIfTileIsNotWater(_row, _col + 1) && (_col + 1 < 10) && (gameBoard[_row][_col + 1].tileStatus != gameBoard[_row][_col].tileStatus)))//  //higher
            {
                SC_MultiPlayer_View.Instance.ShowValidPlacements(gameBoard[_row][_col + 1].tile);
                validCoordinates[2].Row = _row;
                validCoordinates[2].Col = _col + 1;
            }
            if ((CheckIfTileIsNotWater(_row, _col - 1) && (_col - 1 >= 0) && (gameBoard[_row][_col - 1].tileStatus != gameBoard[_row][_col].tileStatus)))//  //lower
            {
                SC_MultiPlayer_View.Instance.ShowValidPlacements(gameBoard[_row][_col - 1].tile);
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

    public void MovePieceWhenEmpty(SC_MultiPlayer_TileLogic tile)
    {
        gameBoard[tile.Row][tile.Col].tileStatus = SC_DefiendVariables.TileStatus.BlueOccupied;
        gameBoard[tile.Row][tile.Col].piece = currentSelectedPiece.piece;
        SC_MultiPlayer_View.Instance.movePieceToNewLocation(currentSelectedPiece.piece, tile);
        if (currentSelectedPiece.piece.currentTileRow != -1)
        {
            gameBoard[currentSelectedPiece.piece.currentTileRow][currentSelectedPiece.piece.currentTileCol].piece = null;
            gameBoard[currentSelectedPiece.piece.currentTileRow][currentSelectedPiece.piece.currentTileCol].tileStatus = SC_DefiendVariables.TileStatus.Empty;
        }
        currentSelectedPiece.piece.currentTileRow = tile.Row;
        currentSelectedPiece.piece.currentTileCol = tile.Col;

        //gameBoard[currentSelectedPiece.piece.currentTileRow = 
        currentSelectedPiece.piece = null;
        currentSelectedPiece.tile = null;

        SC_MultiPlayer_View.Instance.HideValidPlacements();
    }


    #region MultiPlayer

    void OnEnable()
    {
        Listener.OnConnect += OnConnect;
        Listener.OnRoomsInRange += OnRoomsInRange;
        Listener.OnCreateRoom += OnCreateRoom;
        Listener.OnGetLiveRoomInfo += OnGetLiveRoomInfo;
        Listener.OnUserJoinRoom += OnUserJoinRoom;
        Listener.OnGameStarted += OnGameStarted;
        Listener.OnMoveCompleted += OnMoveCompleted;


    }

    void OnDisable()
    {
        Listener.OnConnect -= OnConnect;
        Listener.OnRoomsInRange -= OnRoomsInRange;
        Listener.OnCreateRoom -= OnCreateRoom;
        Listener.OnGetLiveRoomInfo -= OnGetLiveRoomInfo;
        Listener.OnUserJoinRoom -= OnUserJoinRoom;
        Listener.OnGameStarted -= OnGameStarted;
        Listener.OnMoveCompleted -= OnMoveCompleted;



    }
    public void onRoomDestroyed(RoomData eventObj)
    {
        Debug.Log("onRoomDestroyed");
    }

    public void onUserLeftRoom(RoomData eventObj, string username)
    {
        Debug.Log("onUserLeftRoom : " + username);
    }




    public void initMultiplayer()
    {
        WarpClient.initialize(SC_MultiPlayer_Globals.apiKey, SC_MultiPlayer_Globals.secretApiKey);
        WarpClient.GetInstance().AddConnectionRequestListener(SC_MultiPlayer_Globals.listener);
        WarpClient.GetInstance().AddChatRequestListener(SC_MultiPlayer_Globals.listener);
        WarpClient.GetInstance().AddUpdateRequestListener(SC_MultiPlayer_Globals.listener);
        WarpClient.GetInstance().AddLobbyRequestListener(SC_MultiPlayer_Globals.listener);
        WarpClient.GetInstance().AddNotificationListener(SC_MultiPlayer_Globals.listener);
        WarpClient.GetInstance().AddRoomRequestListener(SC_MultiPlayer_Globals.listener);
        WarpClient.GetInstance().AddZoneRequestListener(SC_MultiPlayer_Globals.listener);
        WarpClient.GetInstance().AddTurnBasedRoomRequestListener(SC_MultiPlayer_Globals.listener);

        SC_MultiPlayer_Globals.userName = System.DateTime.UtcNow.Ticks.ToString();
        Debug.Log(SC_MultiPlayer_Globals.userName);
        WarpClient.GetInstance().Connect(SC_MultiPlayer_Globals.userName);
    }


    public void Btn_PlayLogic()
    {
        Debug.Log("Btn_PlayLogic");
        SC_MultiPlayer_Globals.GamePhase = SC_MultiPlayer_Globals.GameSituation.Running;
        SC_MultiPlayer_View.Instance.StartButton.SetActive(false);
        WarpClient.GetInstance().GetRoomsInRange(1, 2);

    }


    public void OnConnect(bool _IsSuccess)
    {
        Debug.Log("OnConnect: " + _IsSuccess);
        //GameObject.Find("Btn_Play").GetComponent<Button>().interactable = _IsSuccess;
    }

    public void OnRoomsInRange(bool _IsSuccess, MatchedRoomsEvent eventObj)
    {
        Debug.Log("OnRoomsInRange: " + _IsSuccess + " " + eventObj.getRoomsData());
        if (_IsSuccess)
        {
            SC_MultiPlayer_Globals.rooms = new List<string>();
            foreach (var roomData in eventObj.getRoomsData())
            {
                Debug.Log("Getting Live info on room " + roomData.getId());
                Debug.Log("Room Owner " + roomData.getRoomOwner());
                SC_MultiPlayer_Globals.rooms.Add(roomData.getId());
            }

            SC_MultiPlayer_Globals.index = 0;
            if (SC_MultiPlayer_Globals.index < SC_MultiPlayer_Globals.rooms.Count)
            {
                Debug.Log("Getting Live Info on room: " + SC_MultiPlayer_Globals.rooms[SC_MultiPlayer_Globals.index]);
                WarpClient.GetInstance().GetLiveRoomInfo(SC_MultiPlayer_Globals.rooms[SC_MultiPlayer_Globals.index]);
            }
            else
            {
                currentTurn = SC_DefiendVariables.Turn.blueTurn;
                print("I start the game!");
                isMyTurn = true;
                Debug.Log("No rooms were availible, create a room");
                WarpClient.GetInstance().CreateTurnRoom("Room Name", SC_MultiPlayer_Globals.userName, 2, SC_DefiendVariables.bet, 60);
            }
            //this bool responsible to aware us if there is a room that Match the bet.

            if (!roomMatch)
            {
                currentTurn = SC_DefiendVariables.Turn.blueTurn;
                print("I start the game!");
                isMyTurn = true;
                Debug.Log("No rooms were availible, create a room");
                WarpClient.GetInstance().CreateTurnRoom("Room Name", SC_MultiPlayer_Globals.userName, 2, SC_DefiendVariables.bet, 60);
            }
        }
        else GameObject.Find("Btn_Play").GetComponent<Button>().interactable = true;
    }



    public void OnCreateRoom(bool _IsSuccess, string _RoomId)
    {

        Debug.Log("OnCreateRoom " + _IsSuccess + " " + _RoomId);
        if (_IsSuccess)
        {
            WarpClient.GetInstance().JoinRoom(_RoomId);

            //so i can get events when other users join my room
            WarpClient.GetInstance().SubscribeRoom(_RoomId);
            SC_MultiPlayer_PieceLogic.playersInteractable = false;
            SC_MultiPlayer_Globals.Instance.unityObjects["TXT_WaitingStart"].SetActive(true);
        }
    }



    public void OnGetLiveRoomInfo(LiveRoomInfoEvent eventObj)
    {
        Debug.Log("OnGetLiveRoomInfo " + eventObj.getData().getId() + " " + eventObj.getResult() + " " + eventObj.getJoinedUsers().Length);
        Dictionary<string, object> _temp = eventObj.getProperties();
        print("bet = " + SC_DefiendVariables.bet);
        Debug.Log(_temp.Count + " " + _temp["bet"] + " " + SC_DefiendVariables.bet["bet"].ToString());

        if (eventObj.getResult() == 0 && eventObj.getJoinedUsers().Length == 1 &&
    _temp["bet"].ToString() == SC_DefiendVariables.bet["bet"].ToString())
        {
            print("entering the room!");
            currentTurn = SC_DefiendVariables.Turn.RedTurn;
            print("Enemy start the game!");
            WarpClient.GetInstance().JoinRoom(eventObj.getData().getId());
            WarpClient.GetInstance().SubscribeRoom(eventObj.getData().getId());
        }

        if (_temp["bet"].ToString() == SC_DefiendVariables.bet["bet"].ToString()) roomMatch = true;

    }

    public void OnUserJoinRoom(RoomData eventObj, string _UserName)
    {
        Debug.Log("OnUserJoinRoom ");// + " " + _UserName);
        //SC_MenuView.Instance.SetInfoText("OnUserJoinRoom " + " " + _UserName);
        if (_UserName != eventObj.getRoomOwner())
        {
            WarpClient.GetInstance().startGame();
            print("startGame!");
        }
    }

    public void EnemyTurn(int moveToRow, int moveToCol, int _FromRow, int _FromCol)
    {
        print("EnemyTurn");
        if (gameBoard[moveToRow][moveToCol].tileStatus == SC_DefiendVariables.TileStatus.Empty)
        {
            print("Empty");
            gameBoard[_FromRow][_FromCol].piece = null;
            gameBoard[_FromRow][_FromCol].tileStatus = SC_DefiendVariables.TileStatus.Empty;
            gameBoard[moveToRow][moveToCol].tileStatus = SC_DefiendVariables.TileStatus.RedOccupied;
            gameBoard[moveToRow][moveToCol].piece = currentSelectedPiece.piece;
            currentSelectedPiece.piece.currentTileRow = moveToRow;
            currentSelectedPiece.piece.currentTileCol = moveToCol;
            SC_MultiPlayer_View.Instance.movePieceToNewLocation(currentSelectedPiece.piece, gameBoard[moveToRow][moveToCol].tile.GetComponent<SC_MultiPlayer_TileLogic>());
        }
        else if (gameBoard[moveToRow][moveToCol].tileStatus == SC_DefiendVariables.TileStatus.BlueOccupied)
        {
            fight(gameBoard[moveToRow][moveToCol].tile.GetComponent<SC_MultiPlayer_TileLogic>());
        }


        //SwitchTurn();
    }

    public void OnGameStarted(string _Sender, string _RoomId, string _NextTurn)
    {
        Debug.Log("OnGameStarted!");// + _Sender + " " + _RoomId + " " + _NextTurn);
        SC_MultiPlayer_PieceLogic.playersInteractable = true;
        SC_MultiPlayer_Globals.Instance.unityObjects["TXT_WaitingStart"].SetActive(false);
        //SC_MenuView.Instance.SetInfoText("OnGameStarted " + _Sender + " " + _RoomId + " " + _NextTurn);
        //SC_MenuGlobals.Instance.unityObjects["Screen_Menu"].SetActive(false);
        //SC_MenuGlobals.Instance.unityObjects["Screen_Game"].SetActive(true);
    }

    #endregion

}

/*
 
	public void DoSlotLogic(int _Index)
	{
		if (currentTurn == SC_DefiendVariables.Turn.blueTurn) 
		{
			Debug.Log ("DoSlotLogic " + _Index);
			if (gameStatus [_Index] == GameEnums.SlotState.Empty)
			{
				isMyTurn = false;
				Dictionary<string,object> _toSend = new Dictionary<string, object> ();
				_toSend.Add ("UserName", SC_MenuGlobals.userName);
				_toSend.Add ("Data",_Index);
				_toSend.Add ("State",playerState);

				string _jsonToSend = MiniJSON.Json.Serialize (_toSend);
				Debug.Log (_jsonToSend);
				WarpClient.GetInstance ().sendMove (_jsonToSend);
				SubmitLogic (_Index);
			}
		}
	}
    }
     /*

    	public void OnMoveCompleted(MoveEvent _Move)
	{
		Debug.Log ("OnMoveCompleted " + _Move.getMoveData() + " " + _Move.getNextTurn() + " " + _Move.getSender());
		if (_Move.getSender () != SC_MenuGlobals.userName && _Move.getMoveData() != null)
		{
			Dictionary<string,object> _recievedData = MiniJSON.Json.Deserialize (_Move.getMoveData()) as Dictionary<string,object>;
			if (_recievedData != null) 
			{
				int _index = int.Parse (_recievedData ["Data"].ToString());
				SubmitLogic (_index);
			}
		}

		if(_Move.getNextTurn() == SC_MenuGlobals.userName)
			isMyTurn = true;
		else isMyTurn = false;
	}


     
     */
