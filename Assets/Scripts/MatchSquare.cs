using UnityEngine;
using System.Collections;

public enum SquareType { None, Robot, Human, Religious, Science, Freedom, Law, Score, Time };

public class MatchSquare : MonoBehaviour {

    public const float moveDuration = .5f;

    public SquareType type;

    public Color[] colors;

    MatchBoard board;

    private float moveStarted = -1f;
    private Vector3 moveFrom, moveBy;
    private Interpolate.Function ease;

    public bool IsBusy {
        get { return moveStarted != -1f; }
    }

	void Start () {
        board = GameObject.Find("Board").GetComponent<MatchBoard>();
        ease = Interpolate.Ease(Interpolate.EaseType.EaseOutCubic);
        // choose a random square type
        System.Array types = System.Enum.GetValues(typeof(SquareType));
        type = (SquareType)types.GetValue(Random.Range(1, types.Length));

        UpdateName();
        Material mat = GetComponentInChildren<MeshRenderer>().material;
        mat.color = colors[(int)type - 1];
	}
	
	// Update is called once per frame
	void Update () {
        HandleMovement();
	}

    void HandleMovement() {
        if (moveStarted == -1) {
            return;
        }
        float elapsed = Time.time - moveStarted;
        transform.position = Interpolate.Ease(ease, moveFrom, moveBy,
            elapsed, moveDuration);
        if (elapsed / moveDuration > 1) {
            transform.position = moveFrom + moveBy;
            moveStarted = -1;
            Debug.Log(name + " done, now at " + transform.position);
        }
    }

    public void UpdateName() {
        Position pos = board.GetPosition(this);
        name = pos.ToString() + type.ToString();
    }

    public void MoveBy(Position delta, bool moveBackground) {
        moveFrom = transform.position;
        //if (moveBackground) {
        //    moveFrom.z += .5f;
        //}
        moveBy = new Vector3(delta.x, delta.y, 0);
        moveStarted = Time.time;
        Debug.Log(name + " moving from " + moveFrom + " by " + moveBy);
    }
}
