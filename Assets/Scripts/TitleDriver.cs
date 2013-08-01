using UnityEngine;
using System.Collections;

public class TitleDriver : MonoBehaviour {
    public GUISkin skin;

    Vector3 titleStart = new Vector3(0, 200, 0);
    Vector3 titleDelta = new Vector3(0, 50, 0);
    Interpolate.Function ease = Interpolate.Ease(Interpolate.EaseType.Linear);
    private float titleDuration = 2f;

    public void OnGUI() {
        GUI.skin = skin;
        Vector3 pos = Interpolate.Ease(ease, titleStart, titleDelta,
            Time.time, titleDuration);
        Rect rect = new Rect(pos.x, pos.y, Screen.width, 300);
        GUI.Label(rect, "Overthrow");
        if(Time.time > titleDuration) {
            Application.LoadLevel("intro");
        }
    }
}
