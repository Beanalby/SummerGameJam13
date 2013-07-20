using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchBoard : MonoBehaviour {

    public MatchSquare SquarePrefab;
    const int boardSize = 7;

    public MatchSquare[,] squares;

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
            }
        }
	}

    // Update is called once per frame
    void Update() {
	
	}
}
