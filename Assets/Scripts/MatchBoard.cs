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
            TestForMatch(current);
        }
	}

    void ApplyMatch(MatchSquare square1, MatchSquare square2, MatchSquare square3) {
        Debug.Log("Got Match betwen " +
            square1.name + " (" + square1.posX + "," + square1.posY + "), " +
            square2.name + " (" + square2.posX + "," + square2.posY + "), " +
            square3.name + " (" + square3.posX + "," + square3.posY + ")!");

    }

    void TestForMatch(MatchSquare square) {
        int x = square.posX;
        int y = square.posY;
        Debug.Log("+++ Testing " + square.name + " (" + x + "," + y + ") for matches");
        if (x > 1)
            TestSameType(square, squares[x - 2, y], squares[x - 1, y]);
        if(x > 0 && x < boardSize-1)
            TestSameType(square, squares[x - 1, y], squares[x + 1, y]);
        if(x < boardSize-2)
            TestSameType(square, squares[x + 2, y], squares[x + 1, y]);

        if (y > 1)
            TestSameType(square, squares[x, y - 2], squares[x, y - 1]);
        if(y > 0 && y < boardSize-1)
            TestSameType(square, squares[x, y - 1], squares[x, y + 1]);
        if(y < boardSize-2)
            TestSameType(square, squares[x, y + 2], squares[x, y + 1]);
    }

    bool TestSameType(MatchSquare square1, MatchSquare square2, MatchSquare square3) {
        if (square1.type == square2.type && square1.type == square3.type) {
            ApplyMatch(square1, square2, square3);
            return true;
        }
        return false;
    }
}
