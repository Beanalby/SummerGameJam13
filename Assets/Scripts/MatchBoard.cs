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

    public MatchSquare SquarePrefab;
    const int boardSize = 7;

    public MatchSquare[,] squares;
    private int squareMask;

    MatchSquare current = null;

	// Use this for initialization
	void Start () {
        squares = new MatchSquare[boardSize,boardSize];
        for (int i = 0; i < boardSize; i++) {
            for (int j = 0; j < boardSize; j++) {
                GameObject obj = Instantiate(SquarePrefab.gameObject) as GameObject;
                obj.transform.parent = transform;
                obj.transform.position = new Vector3(i, j, 0);
                squares[i,j] = obj.GetComponent<MatchSquare>();
            }
        }
        squareMask = 1 << LayerMask.NameToLayer("Square");
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, squareMask)) {
                Debug.Log("I hit " + hit.collider.name + " at " + hit.transform.position);
                current = hit.collider.GetComponent<MatchSquare>();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            HashSet<MatchSquare> matches = GetMatches(current);
            if (matches.Count != 0) {
                string msg = "+++ Got matches: ";
                foreach (MatchSquare sq in matches) {
                    msg += sq.name + " ";
                }
                Debug.Log(msg);
            } else {
                Debug.Log("No matches :(");
            }
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            StartCoroutine(MoveSquare(current, new Position(0, -1)));
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            StartCoroutine(MoveSquare(current, new Position(0, 1)));
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            StartCoroutine(MoveSquare(current, new Position(-1, 0)));
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            StartCoroutine(MoveSquare(current, new Position(1, 0)));
        }
	}

    IEnumerator MoveSquare(MatchSquare square, Position delta) {
        Position currentPos = GetPosition(current);
        Position otherPos = currentPos + delta;
        if(otherPos.x < 0 || otherPos.x >= boardSize
                || otherPos.y < 0 || otherPos.y >= boardSize) {
            yield break; // trying to moving outside board
        }
        MatchSquare other = squares[otherPos.x, otherPos.y];
        if (other == null) {
            yield break; // matched gap that hasn't filled yet
        }
        if (current.IsBusy || other.IsBusy) {
            yield break; // currently moving or falling
        }

        current.MoveBy(delta, false);
        other.MoveBy(delta.GetOpposite(), true);
        while (current.IsBusy || other.IsBusy) {
            yield return 0;
        }
        //yield return new WaitForSeconds(MatchSquare.moveDuration);

        // update their positions in our board
        squares[otherPos.x, otherPos.y] = current;
        current.UpdateName();
        squares[currentPos.x, currentPos.y] = other;
        other.UpdateName();
        
        // WTF TODO test for matches
    }

    void ApplyMatch(MatchSquare square1, MatchSquare square2, MatchSquare square3) {
        Debug.Log("Got Match betwen " +
            square1.name + ", " +
            square2.name + ", " +
            square3.name + "!");

    }

    HashSet<MatchSquare> GetMatches(MatchSquare square) {
        HashSet<MatchSquare> matches = new HashSet<MatchSquare>();
        Position pos = GetPosition(square);
        int x = pos.x, y = pos.y;

        Debug.Log("+++ Testing " + square.name + " (" + x + "," + y + ") for matches");
        if (x > 1)
            matches.UnionWith(TestMatch(square, squares[x - 2, y], squares[x - 1, y]));
        if(x > 0 && x < boardSize-1)
            matches.UnionWith(TestMatch(square, squares[x - 1, y], squares[x + 1, y]));
        if (x < boardSize - 2)
            matches.UnionWith(TestMatch(square, squares[x + 2, y], squares[x + 1, y]));

        if (y > 1)
            matches.UnionWith(TestMatch(square, squares[x, y - 2], squares[x, y - 1]));
        if(y > 0 && y < boardSize-1)
            matches.UnionWith(TestMatch(square, squares[x, y - 1], squares[x, y + 1]));
        if(y < boardSize-2)
            matches.UnionWith(TestMatch(square, squares[x, y + 2], squares[x, y + 1]));

        return matches;
    }

    /// <summary>
    /// Returns the current board position of a given square
    /// </summary>
    public Position GetPosition(MatchSquare square) {
        for (int i = 0; i < boardSize; i++) {
            for (int j = 0; j < boardSize; j++) {
                if (squares[i, j] == square) {
                    return new Position(i, j);
                }
            }
        }
        Debug.LogError("Couldn't find position for " + square.name);
        return new Position(-1, -1);
    }

   MatchSquare[] TestMatch(MatchSquare square1, MatchSquare square2, MatchSquare square3) {
       if (square1 == null || square2 == null || square3 == null) {
           return new MatchSquare[0];
       }
       if (square1.IsBusy || square2.IsBusy || square3.IsBusy) {
           return new MatchSquare[0];
       }
       if (square1.type != square2.type || square1.type != square3.type) {
           return new MatchSquare[0];
       }
       return new MatchSquare[] { square1, square2, square3 };
    }
}
