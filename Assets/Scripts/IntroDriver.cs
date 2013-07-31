using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroDriver : MonoBehaviour {

    public GUISkin skin;
    public GUISkin hudSkin;

    public Tile tilePrefab;

    public DudeFactory dudeFactory;

    private GameState gameState;

    public void Start() {
        gameState = GameState.instance;
        dudeFactory.MakeDudes(0, Screen.width, 25, 8,
            gameState.isRobotEnemy, gameState.isReligionEnemy, gameState.isLawEnemy);
    }

    public void OnGUI() {
        GUI.skin = skin;
        GUI.Label(new Rect(0, 0,
                Screen.width, Screen.height * .4f),
            "The " + gameState.GetEnemyName() + " government has ruled too long. It's time for them to go.\n\n\n\n\n\n\n\nMatch 3 symbols to get support and change your affiliations.\nCollect " + gameState.targetScore + " support to overthrow the government.");
        GUI.Label(new Rect(0, Screen.height * .4f,
                Screen.width, Screen.height * .2f),
            "Which your uprising be?");
        Rect buttonRect = new Rect(Screen.width * .2f, Screen.height * .9f,
            Screen.width * .6f, 50);

        Rect compareRect = new Rect(Screen.width * .2f, Screen.height * .4f,
            Screen.width * .6f, Screen.height * .15f);
        DrawComparison(compareRect, TileDetail.Human, TileDetail.Robot);
        compareRect.yMin += Screen.height * .15f;
        compareRect.yMax += Screen.height * .15f;
        DrawComparison(compareRect, TileDetail.Science, TileDetail.Religion);
        compareRect.yMin += Screen.height * .15f;
        compareRect.yMax += Screen.height * .15f;
        DrawComparison(compareRect, TileDetail.Freedom, TileDetail.Law);

        Rect extraRect = new Rect(0, Screen.height * .8f,
            Screen.height * .15f, Screen.height * .15f);
        Rect extraLabel = new Rect(0, Screen.height * .95f, 300, 50);
        GUI.DrawTexture(extraRect, TileDetail.Score.texture);
        GUI.Label(extraLabel, "+Score");

        extraRect = new Rect(Screen.width - Screen.height * .15f, Screen.height * .8f,
            Screen.height * .15f, Screen.height * .15f);
        extraLabel = new Rect(Screen.width - 300, Screen.height * .95f, 300, 50);
        GUI.DrawTexture(extraRect, TileDetail.Time.texture);
        GUIStyle style = new GUIStyle(hudSkin.label);
        style.alignment = TextAnchor.UpperRight;
        GUI.Label(extraLabel, "+Time", style);

        if (GUI.Button(buttonRect, "Fight the " + gameState.GetEnemyName() + " government!")) {
            Application.LoadLevel("game");
        }
    }

    private void DrawComparison(Rect rect, TileDetail left, TileDetail right) {
        GUIStyle style = new GUIStyle(hudSkin.label);
        GUI.BeginGroup(rect);
        GUI.DrawTexture(new Rect(0, 0, rect.height, rect.height), left.texture);
        style.normal.textColor = left.color;
        GUI.Label(new Rect(rect.height, 0, 200, 50), left.introDescription + "?", style);
        style.normal.textColor = right.color;
        style.alignment = TextAnchor.UpperRight;
        GUI.Label(new Rect(rect.width - rect.height - 200, 0, 200, 50), right.introDescription + "?", style);
        GUI.DrawTexture(new Rect(rect.width - rect.height, 0, rect.height, rect.height), right.texture);
        GUI.EndGroup();
    }
}
