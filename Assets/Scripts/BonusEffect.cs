using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour {
    public TileType type;
    public float duration;
    private float startTime;

    private string message;
    public void Start() {
        startTime = Time.time;
    }
    public void OnGUI() {
        if (Time.time > startTime + duration) {
            Destroy(gameObject);
        }
    }
}
