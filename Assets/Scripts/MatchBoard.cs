using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Position
{
    public int x;
    public int y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Position GetOpposite() {
        return new Position(-this.x, -this.y);
    }

    public override int GetHashCode()
    {
        int hashcode = 0;
        hashcode += x.GetHashCode();
        hashcode += y.GetHashCode();
        return hashcode;
    }
    public static Position operator +(Position p1, Position p2) {
        return new Position(p1.x + p2.x, p1.y + p2.y);
    }
    public static Position operator -(Position p1, Position p2) {
        return new Position(p1.x - p2.x, p1.y - p2.y);
    }
    public override string ToString() {
        return "(" + x + "," + y + ")";
    }
}

public class MatchBoard : MonoBehaviour {

    public GUISkin skin;

    public Tile SquarePrefab;
    const int boardSize = 9;

    public bool isPlaying = true;

    public Tile[,] board = null;
    private int squareMask;

    private float noMoveCooldown = 3f;
    private float noMoveLastCheck = 0f;
    private string resetMessage = null;
    private float resetMessageStart = -1f;
    private float resetMessageDuration = 2f;
    private Interpolate.Function resetEase = Interpolate.Ease(Interpolate.EaseType.EaseOutCirc);

    private float dragThreshold = 700;
    Vector3 dragStart = Vector3.zero;
    Tile dragTile = null;

    private GameDriver driver;
    private GameObject selection;

    // Use this for initialization
    void Start () {
        driver = GameObject.Find("GameDriver").GetComponent<GameDriver>();
        squareMask = 1 << LayerMask.NameToLayer("Square");
        selection = transform.FindChild("Selection").gameObject;
        selection.SetActive(false);
        InitBoard();
    }

    void InitBoard() {
        Random.seed = 123; // +++
        if(board == null) {
            board = new Tile[boardSize, boardSize];
        }
        for (int x = 0; x < boardSize; x++) {
            for (int y = 0; y < boardSize; y++) {
                CreateTile(x, y, y);
            }
        }
        // don't let it check for no moves while it's falling
        noMoveLastCheck = Time.time + 1.5f;
    }

    // Update is called once per frame
    void Update() {
        if(isPlaying) {
            HashSet<Tile> matches = GetAllMatches();
            if(matches.Count != 0) {
                HandleMatches(TileType.None, matches);
            } else {
                HandleNoMoves();
            }
            HandleDrag();
            if(Input.GetKeyDown(KeyCode.Space)) {
                //StartCoroutine(ResetBoard("Tweaking!"));
                List<Position> newMove = FindMatch();
                if(newMove == null) {
                    Debug.LogError("No move :(");
                } else {
                    StartCoroutine(SwapTile(
                        board[newMove[0].x, newMove[0].y], newMove[1]));
                }
            }
        }
    }

    private void HandleDrag() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, squareMask)) {
                dragTile = hit.collider.GetComponent<Tile>();
                dragStart = Input.mousePosition;
                selection.transform.parent = dragTile.transform;
                selection.transform.localPosition = Vector3.zero;
                selection.SetActive(true);
            }
        }
        if (Input.GetMouseButtonUp(0) && dragTile != null) {
            dragTile = null;
            selection.transform.parent = transform;
            selection.SetActive(false);
            dragStart = Vector3.zero;
        }
        if (dragTile != null) {
            Vector3 diff = Input.mousePosition - dragStart;
            if (diff.sqrMagnitude > dragThreshold) {
                // move the tile in whatever direction is largest
                if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y)) {
                    StartCoroutine(SwapTile(dragTile,
                        new Position(diff.x > 0 ? 1 : -1, 0)));
                } else {
                    StartCoroutine(SwapTile(dragTile,
                        new Position(0, diff.y > 0 ? 1 : -1)));
                }
                dragTile = null;
                dragStart = Vector3.zero;
            }
        }
    }

    /// <summary>
    /// Occasionally called to watch for the situation of no possible moves.
    /// If found, it clears out the board and refills.
    /// </summary>
    void HandleNoMoves() {
        if(noMoveLastCheck + noMoveCooldown > Time.time || board == null)
            return;
        noMoveLastCheck = Time.time;
        if(!HasPotentialMatch()) {
            StartCoroutine(ResetBoard("No more moves, resetting..."));
        }
    }

    /// <summary>
    /// Creates a new tile above the board.  x,y defines where it should
    /// eventually be, offset defines now high up the tile should start.
    /// </summary>
    private Tile CreateTile(int x, int y, int offset) {
        GameObject obj = Instantiate(SquarePrefab.gameObject) as GameObject;
        obj.transform.parent = transform;
        obj.transform.position = new Vector3(x, boardSize + offset, 0);
        Tile tile = obj.GetComponent<Tile>();
        // tile normally as to travel full board size.  Distance is
        // lessened when its position is higher, increased by offset.
        int distance = boardSize - y + offset;
        MoveTile(tile, new Position(x, boardSize+offset),
            new Position(0, -distance));
        return tile;
    }

    /// <summary>
    /// Attempts to swap a tile in the given direction.  Makes sure
    /// it's not off the board, finds the appropriate other tile and
    /// moves it too, moves it back if it isn't a match, etc.
    /// </summary>
    /// <param name="current"></param>
    /// <param name="delta"></param>
    /// <returns></returns>
    IEnumerator SwapTile(Tile current, Position delta) {
        Position currentPos = GetPosition(current);
        Position otherPos = currentPos + delta;
        if(otherPos.x < 0 || otherPos.x >= boardSize
                || otherPos.y < 0 || otherPos.y >= boardSize) {
            yield break; // trying to moving outside board
        }
        Tile other = board[otherPos.x, otherPos.y];
        if (other == null) {
            yield break; // matched gap that hasn't filled yet
        }
        if (current.IsBusy || other.IsBusy) {
            yield break; // currently moving or falling
        }

        MoveTile(current, currentPos, delta);
        MoveTile(other, otherPos, delta.GetOpposite(), true);
        while (current.IsBusy || other.IsBusy) {
            yield return 0;
        }

        selection.transform.parent = transform;
        selection.SetActive(false);
        // get the list of squares involved in a match by swapping those
        HashSet<Tile> matches = GetAllMatches();
        if (matches.Count == 0) {
            // didn't work, move the squares back
            MoveTile(current, currentPos+delta, delta.GetOpposite());
            MoveTile(other, otherPos-delta, delta, true);
            yield break;
        } else {
            HandleMatches(current.type, matches);
        }
    }

    /// <summary>
    /// Internal funciton to invoke the actual movement of a tile.
    /// Assumes position is valid, tile exists, etc.
    /// Used by SwapTile & refillBoard
    /// </summary>
    private void MoveTile(Tile tile, Position from, Position delta,
            bool isBackground=false) {
        tile.MoveBy(from, delta, isBackground);
        Position target = from + delta;
        board[target.x, target.y] = tile;
        tile.UpdateName(target);
    }

    void HandleMatches(TileType primaryType, HashSet<Tile> matches) {
        Dictionary<TileType, int> score = new Dictionary<TileType,int>();
        if (primaryType != TileType.None) {
            // don't call matchedTiles if the tile they dragged isn't in matches
            bool primaryMatch = false;
            foreach (Tile tile in matches) {
                if (tile.type == primaryType) {
                    primaryMatch = true;
                    break;
                }
            }
            if (primaryMatch) {
                driver.MatchedTiles(primaryType);
            }
        }

        foreach (Tile tile in matches) {
            if (score.ContainsKey(tile.type)) {
                score[tile.type] += 1;
            } else {
                score[tile.type] = 1;
            }
            Position pos = GetPosition(tile);
            board[pos.x, pos.y] = null;
            tile.Matched();
        }
        foreach (TileType type in score.Keys) {
            driver.AddStats(type, score[type], type == primaryType);
        }
        RefillBoard();
    }

    void RefillBoard() {
        // for each of the columns, figure out what needs to be moved/made
        for (int x = 0; x < boardSize; x++) {
            int numHoles = 0;
            // from the bottom up, move things down as needed
            // the number of gaps we see drives the number
            // of tiles we make from the top
            for (int y = 0; y < boardSize; y++) {
                Tile tile = board[x,y];
                if (tile == null) {
                    numHoles++;
                } else {
                    if (numHoles != 0) {
                        MoveTile(tile, new Position(x, y),
                            new Position(0, -numHoles));
                    }
                }
            }
            // if we found any holes, create new tiles at the top
            for (int i = 0; i < numHoles; i++) {
                // create the tile off the top of the board and let
                // it fall down.  Additional tiles for multiple holes
                // start higher & higher
                int targetY = boardSize - numHoles + i;
                CreateTile(x, targetY, i);
            }
        }
    }

    // removes all tiles and re-fills the board
    IEnumerator ResetBoard(string message) {
        resetMessage = message;
        resetMessageStart = Time.time;

        for(int x = 0; x < boardSize; x++) {
            for(int y = 0; y < boardSize; y++) {
                Tile tile = board[x, y];
                if(tile != null) {
                    board[x, y] = null;
                    tile.Die();
                }
            }
        }
        yield return new WaitForSeconds(1.5f);
        InitBoard();
    }

    HashSet<Tile> GetAllMatches() {
        // returns any possible matches on the board
        HashSet<Tile> matches = new HashSet<Tile>();

        for (int x = 0; x < boardSize; x++) {
            for (int y = 0; y < boardSize; y++) {
                if(x < boardSize - 2) {
                    matches.UnionWith(GetMatch(
                        board[x,y], board[x+1,y], board[x+2,y]));
                }
                if (y < boardSize - 2) {
                    matches.UnionWith(GetMatch(
                        board[x,y], board[x,y+1], board[x,y+2]));
                }
            }
        }
        return matches;
    }

    Tile[] GetMatch(Tile square1, Tile square2, Tile square3) {
        if (IsMatch(square1, square2, square3)) {
            return new Tile[] { square1, square2, square3 };
        } else {
            return new Tile[0];
        }
    }
    bool IsMatch(Tile square1, Tile square2, Tile square3) {
        if (square1 == null || square2 == null || square3 == null) {
            return false;
        }
        if (square1.IsBusy || square2.IsBusy || square3.IsBusy) {
            return false;
        }
        if (square1.type != square2.type || square1.type != square3.type) {
            return false;
        }
        return true;
    }

    List<Position> FindMatch() {
        // returns true if it's possible to make a match through a swap
        
        // for any given position, we look for any of the following
        for (int x = 0; x < boardSize; x++) {
            for (int y = 0; y < boardSize; y++) {
                // we check for a number of potential positions, where
                // X is the current postion, x is a tile we'll be checking
                // for the same type, and . is a tile that doesn't matter
                Tile tile = board[x,y];
                int max = boardSize - 1;
                // -------------------------------------
                // horizontal match, tile on right joins
                // xx.X
                if (x >= 3)
                    if (IsMatch(tile, board[x - 2, y], board[x - 3, y]))
                        return new List<Position>() {
                            new Position(x,y),
                            new Position(-1,0) };
                // ..X
                // xx.
                if (x >= 2 && y >= 1)
                    if (IsMatch(tile, board[x - 1, y - 1], board[x - 2, y - 1]))
                        return new List<Position>() {
                            new Position(x,y),
                            new Position(0, -1) };
                // xx.
                // ..X
                if (x >= 2 && y <= max - 1)
                    if (IsMatch(tile, board[x - 1, y + 1], board[x - 2, y + 1]))
                        return new List<Position>() {
                            new Position(x,y),
                            new Position(0,1) };

                // -------------------------------------
                // horizontal match, tile on left joins
                // X.xx
                if (x <= max-3)
                    if (IsMatch(tile, board[x + 2, y], board[x + 3, y]))
                        return new List<Position>() {
                            new Position(x,y),
                            new Position(1,0) };
                // X..
                // .xx
                if (x <= max - 2 && y >= 1)
                    if (IsMatch(tile, board[x + 1, y - 1], board[x + 2, y - 1]))
                        return new List<Position>() {
                            new Position(x,y),
                            new Position(0,-1) };
                // .xx
                // X..
                if (x <= max - 2 && y <= max - 1)
                    if (IsMatch(tile, board[x + 1, y + 1], board[x + 2, y + 1]))
                        return new List<Position>() {
                            new Position(x,y),
                            new Position(0,1) };

                // -------------------------------------
                // horizontal match, tile in middle joins
                // .X.
                // x.x
                if (x >= 1 && x <= max - 1 && y >= 1)
                    if (IsMatch(tile, board[x - 1, y - 1], board[x + 1, y - 1]))
                        return new List<Position>() {
                            new Position(x,y),
                            new Position(0,-1) };
                // x.x
                // .X.
                if (x >= 1 && x <= max - 1 && y <= max - 1)
                    if (IsMatch(tile, board[x - 1, y + 1], board[x + 1, y + 1]))
                        return new List<Position>() {
                            new Position(x,y),
                            new Position(0,1) };

                // -------------------------------------
                // vertical match, tile on top joins
                // X
                // .
                // x
                // x
                if (y >= 3)
                    if (IsMatch(tile, board[x, y - 2], board[x, y - 3]))
                        return new List<Position>() {
                            new Position(x,y),
                            new Position(0,-1) };
                // X.
                // .x
                // .x
                if (x <= max - 1 && y >= 2)
                    if (IsMatch(tile, board[x + 1, y - 1], board[x + 1, y - 2]))
                        return new List<Position>() {
                            new Position(x,y),
                            new Position(1,0) };
                // .X
                // x.
                // x.
                if (x >= 1 && y >= 2)
                    if (IsMatch(tile, board[x - 1, y - 1], board[x - 1, y - 2]))
                        return new List<Position>() {
                            new Position(x,y),
                            new Position(-1,0) };
                // -------------------------------------
                // vertical match, tile on bottom joins
                // x
                // x
                // .
                // X
                if (y <= max - 3)
                    if (IsMatch(tile, board[x, y + 2], board[x, y + 3]))
                        return new List<Position>() {
                            new Position(x,y),
                            new Position(0,1) };
                // .x
                // .x
                // X.
                if (x <= max - 1 && y <= max - 2)
                    if (IsMatch(tile, board[x + 1, y + 1], board[x + 1, y + 2]))
                        return new List<Position>() {
                            new Position(x,y),
                            new Position(1,0) };
                // x.
                // x.
                // .X
                if (x >= 1 && y <= max - 2)
                    if (IsMatch(tile, board[x - 1, y + 1], board[x - 1, y + 2]))
                        return new List<Position>() {
                            new Position(x,y),
                            new Position(-1,0) };

                // -------------------------------------
                // vertical match, tile in middle joins
                // x.
                // .X
                // x.
                if (x >= 1 && y >= 1 && y <= max - 1)
                    if (IsMatch(tile, board[x - 1, y - 1], board[x - 1, y + 1]))
                        return new List<Position>() {
                            new Position(x,y),
                            new Position(-1,0) };
                // .x
                // X.
                // .x
                if (x <= max - 1 && y >= 1 && y <= max - 1)
                    if (IsMatch(tile, board[x + 1, y - 1], board[x + 1, y + 1]))
                        return new List<Position>() {
                            new Position(x,y),
                            new Position(1,0) };
            }
        }

        return null;
    }
    bool HasPotentialMatch() {
        return FindMatch() != null;
    }

    /// <summary>
    /// Returns the current board position of a given square
    /// </summary>
    public Position GetPosition(Tile square) {
        for (int i = 0; i < boardSize; i++) {
            for (int j = 0; j < boardSize; j++) {
                if (board[i, j] == square) {
                    return new Position(i, j);
                }
            }
        }
        Debug.LogError("Couldn't find position for " + square.name);
        return new Position(-1, -1);
    }

    public void OnGUI() {
        GUI.skin = skin;
        DrawResetMessage();
    }

    void DrawResetMessage() {
        if(resetMessage != null) {
            float elapsed = Time.time - resetMessageStart;
            if(elapsed > resetMessageDuration) {
                resetMessage = null;
                resetMessageStart = -1;
            } else {
                Vector3 msgStart = new Vector3(0, Screen.height / 4, 0);
                Vector3 msgDelta = new Vector3(0, Screen.height / 4, 0);
                Vector3 pos = Interpolate.Ease(resetEase,
                    msgStart, msgDelta, elapsed, resetMessageDuration);
                GUI.Label(new Rect(pos.x, pos.y, Screen.width, 100),
                    resetMessage);
            }
        }
    }
    public void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(boardSize/2, boardSize/2, 0),
            new Vector3(boardSize, boardSize, .1f));
    }
}
