using UnityEngine;
using System.Collections;

public enum SquareType { None, Robot, Human, Religious, Science, Freedom, Law, Score, Time };

public class MatchSquare : MonoBehaviour {

    public int posX, posY;
    public SquareType type;

    public Color[] colors;

	// Use this for initialization
	void Start () {
        System.Array types = System.Enum.GetValues(typeof(SquareType));
        type = (SquareType)types.GetValue(Random.Range(1, types.Length));
        name = type.ToString();
        Material mat = GetComponentInChildren<MeshRenderer>().material;
        mat.color = colors[(int)type - 1];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}