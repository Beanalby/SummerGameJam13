using UnityEngine;
using System.Collections;

public class GameDriver : MonoBehaviour {
    public void OnGUI() {
        int boardWidth = 600;
        int dudeHeight = 130;
        int meterHeight = 400;
        Rect dudeRect = new Rect(Screen.width - boardWidth, 0,
            boardWidth, dudeHeight);
        Rect meterRect = new Rect(0, Screen.height - meterHeight,
            Screen.width - boardWidth, meterHeight);
        Rect logoRect = new Rect(0, 0, Screen.width - boardWidth,
            Screen.height - meterHeight);
        GUI.Box(dudeRect, "Dude");
        GUI.Box(meterRect, "Levels");
        GUI.Box(logoRect, "Logo");
    }
}
