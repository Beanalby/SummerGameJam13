using UnityEngine;
using System.Collections;

public class GameDriver : MonoBehaviour {

    const int levelMin = 0, levelMax = 100;

    int score;
    float levelLaw;
    float levelRobot;
    float levelReligion;

    private int boardWidth = 600;
    int dudeHeight = 130;
    int levelHeight = 400;
    Rect dudeRect, levelRect, scoreRect;

    public void Start() {
        score = 0;
        levelLaw = levelRobot = levelReligion = 50;

        dudeRect = new Rect(Screen.width - boardWidth, 0,
            boardWidth, dudeHeight);
        levelRect = new Rect(0, Screen.height - levelHeight,
            Screen.width - boardWidth, levelHeight);
        scoreRect = new Rect(0, 0, Screen.width - boardWidth,
            Screen.height - levelHeight);
    }
    public void OnGUI() {
        GUI.Box(dudeRect, "Dude");
        DrawLevels();
        DrawScore();
    }

    public void DrawLevels() {
        GUI.Box(levelRect, "");
        GUI.BeginGroup(levelRect);
        GUILayout.BeginVertical();

        GUILayout.Label("LevelRobot: " + levelRobot);
        GUILayout.Label("LevelReligion: " + levelReligion);
        GUILayout.Label("LevelLaw: " + levelLaw);
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUI.EndGroup();
    }
    public void DrawScore() {
        GUI.Box(scoreRect, "");
        GUI.BeginGroup(scoreRect);
        GUILayout.BeginVertical();
        GUILayout.Label("Score: " + score);
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUI.EndGroup();
    }
    public void AddStats(TileType type, float amount) {
        //Debug.Log(name + " increasing " + type + " by " + amount);
        score += (int)amount;
        switch (type) {
            case TileType.Freedom:
                levelLaw = Mathf.Max(levelMin, Mathf.Min(levelMax, levelLaw - amount)); break;
            case TileType.Human:
                levelRobot = Mathf.Max(levelMin, Mathf.Min(levelMax, levelRobot - amount)); break;
            case TileType.Law:
                levelLaw = Mathf.Max(levelMin, Mathf.Min(levelMax, levelLaw + amount)); break;
            case TileType.Religious:
                levelReligion = Mathf.Max(levelMin, Mathf.Min(levelMax, levelReligion + amount)); break;
            case TileType.Robot:
                levelRobot = Mathf.Max(levelMin, Mathf.Min(levelMax, levelRobot + amount)); break;
            case TileType.Science:
                levelReligion = Mathf.Max(levelMin, Mathf.Min(levelMax, levelReligion - amount)); break;
            case TileType.Score:
                // TODO
                break;
            case TileType.Time:
                // TODO
                break;
        }
    }
}
