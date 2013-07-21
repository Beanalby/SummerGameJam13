using UnityEngine;
using System.Collections;

public class GameDriver : MonoBehaviour {

    public GUISkin skin;

    public Texture2D LevelRobotTex, LevelReligionTex, LevelLawTex;
    public Texture2D LevelMarker;

    const int levelMin = 0, levelMax = 100;

    int score;
    float levelLaw;
    float levelRobot;
    float levelReligion;

    GUIStyle styleHuman, styleRobot, styleScience, styleReligion, styleFreedom, styleLaw;

    private int boardWidth = 600;
    int dudeHeight = 130;
    int levelHeight = 100;
    int levelWidth;
    Rect dudeRect, levelRobotRect, levelReligionRect, levelLawRect, scoreRect;

    public void Start() {
        score = 0;
        levelLaw = levelRobot = levelReligion = 50;
        dudeRect = new Rect(Screen.width - boardWidth, 0,
            boardWidth, dudeHeight);

        styleHuman = new GUIStyle(skin.label);
        styleHuman.normal.textColor = new Color(1, 1, 0);
        styleRobot = new GUIStyle(skin.label);
        styleRobot.normal.textColor = new Color(1, 0, 0);
        styleRobot.alignment = TextAnchor.UpperRight;
        styleScience = new GUIStyle(skin.label);
        styleScience.normal.textColor = new Color(0, 0, 1);
        styleReligion = new GUIStyle(skin.label);
        styleReligion.normal.textColor = new Color(0, 1, 1);
        styleReligion.alignment = TextAnchor.UpperRight;
        styleFreedom = new GUIStyle(skin.label);
        styleFreedom.normal.textColor = new Color(1, 1, 1);
        styleLaw = new GUIStyle(skin.label);
        styleLaw.normal.textColor = new Color(0, 1, 0);
        styleLaw.alignment = TextAnchor.UpperRight;

        levelWidth = (Screen.width - boardWidth);
        levelRobotRect = new Rect(0, Screen.height - (levelHeight * 3),
            levelWidth, levelHeight);
        levelReligionRect = new Rect(0, Screen.height - (levelHeight * 2),
            Screen.width - boardWidth, levelHeight);
        levelLawRect = new Rect(0, Screen.height - levelHeight,
            Screen.width - boardWidth, levelHeight);

        scoreRect = new Rect(0, 0, Screen.width - boardWidth,
            Screen.height - levelHeight * 3);
    }
    public void OnGUI() {
        GUI.skin = skin;
        GUI.Box(dudeRect, "Dude");
        DrawLevels();
        DrawScore();
    }

    public void DrawLevels() {
        DrawLevel(levelRobotRect, LevelRobotTex, levelRobot,
            "Human", "Human Creativity", "Randomly Doubles matches", styleHuman,
            "Robot", "Automation", "Automatically matches tiles for you", styleRobot);
        DrawLevel(levelReligionRect, LevelReligionTex, levelReligion,
            "Science", "Breakthrough", "Bonus for fast matches", styleScience,
            "Religion", "Redemption", "Highlights matches faster", styleReligion);
        DrawLevel(levelLawRect, LevelLawTex, levelLaw,
            "Freedom", "Freedom of Choice", "Bonus for matching different tiles", styleFreedom,
            "Law", "Uphold Justice", "Bonus for matching same tile repeatedly", styleLaw);
    }

    private void DrawLevel(Rect rect, Texture meter, float level,
            string labelLeft, string powerLeft, string descLeft, GUIStyle styleLeft,
            string labelRight, string powerRight, string descRight, GUIStyle styleRight) {

        float pad = .04f;
        GUI.Box(new Rect(0, rect.yMin, rect.width, rect.height), "");
        GUI.BeginGroup(rect);
        Rect powerRect = new Rect(rect.width*pad, 0, rect.width * (1-2*pad), rect.height / 2);
        if(level < 40) {
            GUI.Label(powerRect, "BONUS! " + powerLeft + ": " + descLeft, styleLeft);
        } else if(level > 60) {
            GUI.Label(powerRect, "BONUS! " + powerRight + ": " + descRight, styleRight);
        }
        Rect labelRect = new Rect(rect.width*pad, rect.height / 2,
            rect.width * (1-2*pad),  rect.height / 4);
        GUI.Label(labelRect, labelLeft, styleLeft);
        GUI.Label(labelRect, labelRight, styleRight);

        Rect meterRect = new Rect(rect.width*pad, rect.height * .75f,
            rect.width * (1-2*pad), meter.height);
        GUI.DrawTexture(meterRect, meter);

        float markerPos = (levelWidth - LevelMarker.width) * level/100;
        Rect markerRect = new Rect(markerPos, rect.height * .75f + meter.height,
            LevelMarker.width, LevelMarker.height);

        GUI.DrawTexture(markerRect, LevelMarker);

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

    public void Update() {
        if(Input.GetKeyDown(KeyCode.S)) {
            levelReligion -= 10;
            levelLaw -= 10;
            levelRobot -= 10;
        } else if(Input.GetKeyDown(KeyCode.F)) {
            levelReligion += 10;
            levelLaw += 10;
            levelRobot += 10;
        }
    }
}
