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

    private int boardWidth = 600;
    int levelHeight = 100;
    int levelWidth;
    private int bonusThreshold = 35;
    Rect levelRobotRect, levelReligionRect, levelLawRect, scoreRect;

    private GameState gameState;
    private MatchBoard board;

    public DudeFactory dudeFactory;

    private Queue<TileType> lastMatches;

    public void Start() {
        lastMatches = new Queue<TileType>(3);
        gameState = GameState.instance;
        score = 0;
        levelLaw = Random.Range(48,53);
        levelRobot = Random.Range(48,53);
        levelReligion = Random.Range(48,53);
        dudeFactory.MakeDudes(Screen.width - boardWidth, Screen.width, 0, 6,
            gameState.isRobotEnemy, gameState.isReligionEnemy, gameState.isLawEnemy);
        targetStyle = new GUIStyle(skin.label);
        targetStyle.alignment = TextAnchor.UpperRight;

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
            TileDetail.Human, TileDetail.Robot);
        DrawLevel(levelReligionRect, LevelReligionTex, levelReligion,
            TileDetail.Science, TileDetail.Religion);
        DrawLevel(levelLawRect, LevelLawTex, levelLaw,
            TileDetail.Freedom, TileDetail.Law);

    }

    private void DrawLevel(Rect rect, Texture meter, float level,
            TileDetail left, TileDetail right) {

        float pad = .04f;
        GUI.Box(new Rect(0, rect.yMin, rect.width, rect.height), "");
        GUI.BeginGroup(rect);
        Rect powerRect = new Rect(rect.width*pad, 0, rect.width * (1-2*pad), rect.height / 2);
        GUIStyle style = new GUIStyle(skin.label);
        if(level < bonusThreshold) {
            style.normal.textColor = left.color;
            GUI.Label(powerRect, "BONUS! " + left.bonusName + ": " + left.bonusDescription, style);
        } else if(level > (100 - bonusThreshold)) {
            style.normal.textColor = right.color;
            GUI.Label(powerRect, "BONUS! " + right.bonusName + ": " + right.bonusDescription, style);
        }
        Rect labelRect = new Rect(rect.width*pad, rect.height / 2,
            rect.width * (1-2*pad),  rect.height / 4);
        style.normal.textColor = left.color;
        GUI.Label(labelRect, left.type.ToString(), style);
        style.normal.textColor = right.color;
        style.alignment = TextAnchor.UpperRight;
        GUI.Label(labelRect, right.type.ToString(), style);

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
        GUI.Label(scoreLabel, "Support: " + score);
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

    private bool IsBonusActive(TileType type) {
        switch (type) {
            case TileType.Freedom:
                return levelLaw < bonusThreshold;
            case TileType.Human:
                return levelRobot < bonusThreshold;
            case TileType.Law:
                return levelLaw > (100 - bonusThreshold);
            case TileType.Religion:
                return levelReligion > (100 - bonusThreshold);
            case TileType.Robot:
                return levelRobot > (100 - bonusThreshold);
            case TileType.Science:
                return levelReligion < bonusThreshold;
            default:
                throw new System.ArgumentException(type.ToString());
        }
    }

    public void AddBonus(TileType type, float amount) {
        // indicate that the user just got a bonus
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
            case TileType.Religion:
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

    // Indicates the user actively matched a type of tiles, might
    // trigger bonuses
    public void MatchedTiles(TileType type) {
        lastMatches.Enqueue(type);
        while (lastMatches.Count > 3) {
            lastMatches.Dequeue();
        }

        if (IsBonusActive(TileType.Freedom)) {
            // add a bonus if the three most recent matches were all different
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
