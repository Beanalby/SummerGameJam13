using UnityEngine;
using System.Collections;

public class BonusEffect : MonoBehaviour {
    public TileDetail type;
    public float duration;
    public float amount = -1f;

    [HideInInspector]
    public GUIStyle style;

    private string message;
    private float startTime;
    private float startY = 75;
    private float endY = 100;
    private Color color;

    public void Start() {
        startTime = Time.time;
        color = type.color;
        if (amount != -1f) {
            message = "BONUS +" + amount + " " + type.bonusName;
        } else {
            message = "BONUS " + type.bonusName;
        }
    }

    public void OnGUI() {
        if (Time.time > startTime + duration) {
            Destroy(gameObject);
            return;
        }
        float percent = (Time.time - startTime) / duration;
        float y = Mathf.Lerp(startY, endY, percent);
        if (percent < .5f) {
            color.a = 1;
        } else {
            color.a = -2 * percent + 2f;
        }
        style.normal.textColor = color;
        Rect bonusRect = new Rect(0, y, 300, 75);
        //GUI.Box(bonusRect, "");
        GUI.Label(bonusRect, message, style);
    }
}
