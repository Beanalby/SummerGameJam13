using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
            Debug.Log("Creating row " + i);
            for (int j = 0; j < boardSize; j++) {
                Debug.Log("Creating item " + i + "," + j);
                GameObject obj = Instantiate(SquarePrefab.gameObject) as GameObject;
                obj.transform.position = new Vector3(i, j, 0);
                squares[i,j] = obj.GetComponent<MatchSquare>();
                squares[i, j].posX = i;
                squares[i, j].posY = j;
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
                    msg += sq.name + " (" + sq.posX + "," + sq.posY + ") ";
                }
                Debug.Log(msg);
            } else {
                Debug.Log("No matches :(");
            }
        }
	}

    void ApplyMatch(MatchSquare square1, MatchSquare square2, MatchSquare square3) {
        Debug.Log("Got Match betwen " +
            square1.name + " (" + square1.posX + "," + square1.posY + "), " +
            square2.name + " (" + square2.posX + "," + square2.posY + "), " +
            square3.name + " (" + square3.posX + "," + square3.posY + ")!");

    }

    HashSet<MatchSquare> GetMatches(MatchSquare square) {
        HashSet<MatchSquare> matches = new HashSet<MatchSquare>();
        int x = square.posX;
        int y = square.posY;
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

    MatchSquare[] TestMatch(MatchSquare square1, MatchSquare square2, MatchSquare square3) {
        if (square1.type == square2.type && square1.type == square3.type) {
            return new MatchSquare[] { square1, square2, square3 };
        } else {
            return new MatchSquare[0];
        }
    }
}
