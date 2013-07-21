using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDriver : MonoBehaviour {

    public GUISkin skin;

    public Texture2D LevelRobotTex, LevelReligionTex, LevelLawTex;
    public Texture2D LevelMarker;

    const int levelMin = 0, levelMax = 100;

    int score;
    float gameStart;
    private float timeElapsed, timeLeft = 60;

    public bool hideEndgame = false;
    float levelLaw;
    float levelRobot;
    float levelReligion;

    GUIStyle targetStyle;
    GUIStyle styleHuman, styleRobot, styleScience, styleReligion, styleFreedom, styleLaw;

    private int boardWidth = 600;
    int levelHeight = 100;
    int levelWidth;
    private int powerThreshold = 35;
    Rect levelRobotRect, levelReligionRect, levelLawRect, scoreRect;

    private GameState gameState;
    private MatchBoard board;

    public DudeFactory dudeFactory;
    private List<Dude> dudes;

    public void Start() {
        gameState = GameState.instance;
        score = 0;
        levelLaw = Random.Range(48,53);
        levelRobot = Random.Range(48,53);
        levelReligion = Random.Range(48,53);
        dudes = dudeFactory.MakeDudes(Screen.width - boardWidth, Screen.width, 0, 6,
            gameState.isRobotEnemy, gameState.isReligionEnemy, gameState.isLawEnemy);
        targetStyle = new GUIStyle(skin.label);
        targetStyle.alignment = TextAnchor.UpperRight;

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

        board = GameObject.Find("Board").GetComponent<MatchBoard>();
        StartGame();
    }
    public void OnGUI() {
        GUI.skin = skin;
        DrawLevels();
        DrawScore();
        DrawEndGame();
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
        if(level < powerThreshold) {
            GUI.Label(powerRect, "BONUS! " + powerLeft + ": " + descLeft, styleLeft);
        } else if(level > (100 - powerThreshold)) {
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
        Rect scoreLabel = new Rect(0, 0, scoreRect.width, 25);
        GUI.Label(scoreLabel, "Score: " + score);
        GUI.Label(scoreLabel, "Target Score: " + gameState.targetScore, targetStyle);
        Rect timeLabel = new Rect(0, 25, scoreRect.width, 25);
        GUI.Label(timeLabel, "Time left: " + timeLeft.ToString(".0"));
        GUI.EndGroup();
    }

    public void DrawEndGame() {
        if(timeLeft > 0 || hideEndgame) {
            return;
        }
        //Debug.Log("endGame: timeLeft=" + timeLeft + ", score=" + score + ", target=" + gameState.targetScore);
        int buttonWidth = 400, buttonHeight = 50;
        Rect endGameRect = new Rect(Screen.width * .1f, Screen.height * .25f,
            Screen.width * .8f, Screen.height * .5f);
        GUI.Box(endGameRect, "");
        GUI.Box(endGameRect, "");
        GUI.Box(endGameRect, "");
        GUI.Box(endGameRect, "");
        GUI.BeginGroup(endGameRect);

        if(score >= gameState.targetScore) {
            GUI.Label(new Rect(0, 0, endGameRect.width, endGameRect.height),
                "You win!",
                skin.customStyles[0]);
            if(GUI.Button(new Rect(endGameRect.width / 2 - buttonWidth / 2,
                    endGameRect.height * .75f, buttonWidth, buttonHeight),
                    "Overthrow Government")) {
                gameState.Transition(levelRobot > 50, levelReligion > 50,
                    levelLaw > 50);
                hideEndgame = true;
            }
        } else {
            GUI.Label(new Rect(0, 0, endGameRect.width, endGameRect.height),
                "Overthrow failed.\nBUT their grip is weakening...",
                skin.customStyles[0]);
            if(GUI.Button(new Rect(endGameRect.width / 2 - buttonWidth / 2,
                    endGameRect.height * .75f, buttonWidth, buttonHeight),
                    "Fight On!")) {
                gameState.RestartEasier();
                hideEndgame = true;
            }
        }
        GUI.EndGroup();
    }
    public void AddStats(TileType type, float amount) {
        if(!board.isPlaying) {
            return;
        }
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
                score += 4;
                break;
            case TileType.Time:
                gameStart += 5; // HACK!
                break;
        }
    }

    void StartGame() {
        gameStart = Time.time;
    }

    public void Update() {
        UpdateTime();
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

    void UpdateTime() {
        timeElapsed = Time.time - gameStart;
        timeLeft = Mathf.Max(0, gameState.gameDuration - timeElapsed);
        if(timeLeft <= 0 && board.isPlaying) {
            board.isPlaying = false;
        }
    }
}
