using UnityEngine;
using System.Collections;

public enum TileType { None, Freedom, Human, Law, Religious, Robot, Science, Score, Time };

public class Tile : MonoBehaviour {

    public const float moveDuration = .5f;

    public TileType type;

    public Color[] colors;
    public Texture[] textures;

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
        System.Array types = System.Enum.GetValues(typeof(TileType));
        type = (TileType)types.GetValue(Random.Range(1, types.Length));

        UpdateName();
        Material mat = GetComponentInChildren<MeshRenderer>().material;
        //mat.color = colors[(int)type - 1];
        mat.mainTexture = textures[(int)type - 1];
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
            // we may or may not have been in the background, but we finish
            // back on the 0 z plane.
            Vector3 pos = transform.position = moveFrom + moveBy;
            pos.z = 0;
            transform.position = pos;
            moveStarted = -1;
        }
    }

    public void Matched() {
        Destroy(gameObject);
    }
    /// <summary>
    /// Invoked when a board's position changes to update its name
    /// </summary>
    public void UpdateName() {
        Position pos = board.GetPosition(this);
        UpdateName(pos);
    }

    public void UpdateName(Position pos) {
        name = pos.ToString() + type.ToString();
    }

    /// <summary>
    /// Invoked when it needs to swap to a different location
    /// </summary>
    public void MoveBy(Position from, Position delta, bool isBackground) {
        Debug.Log(name + " Moving by " + delta + ", background=" + isBackground);
        moveFrom = new Vector3(from.x, from.y, 0);
        // isBackground makes tiles that the user DIDN'T click on move
        // slighitly behind the one they did click on, so whatever they
        // interacted with will show in front of the other one
        if(isBackground) {
            moveFrom.z += .1f;
        }
        transform.position = moveFrom;
        moveBy = new Vector3(delta.x, delta.y, 0);
        moveStarted = Time.time;
    }
}
