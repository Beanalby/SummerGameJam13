using UnityEngine;
using System.Collections;

public class BonusEffect : MonoBehaviour {
    public TileDetail type;
    public float duration;
    [HideInInspector]
    public GUIStyle style;

    private float startTime;
    private float startY = 100;
    private float endY = 200;
    private Color color;

    public void Start() {
        startTime = Time.time;
        color = type.color;
    }

    public void OnGUI() {
        if (Time.time > startTime + duration) {
            Destroy(gameObject);
            return;
        }
        float percent = (Time.time - startTime) / duration;
        float y = Mathf.Lerp(startY, endY, percent);
        color.a = 1 - percent;
        style.normal.textColor = color;
        GUI.Label(new Rect(0, y, 300, 50), "BONUS " + type.bonusName, style);
    }
}
