using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroDriver : MonoBehaviour {

    public GUISkin skin;
    public GUISkin hudSkin;

    public Tile tilePrefab;

    public DudeFactory dudeFactory;

    private GameState gameState;
    GUIStyle styleHuman, styleRobot, styleScience, styleReligion, styleFreedom, styleLaw, styleScore, styleTime;
    List<Dude> dudes;

    public void Start() {
        gameState = GameState.instance;
        Debug.Log(gameState.isRobotEnemy + "/" + gameState.isReligionEnemy + "/" + gameState.isLawEnemy + " gives " + gameState.GetEnemyName());
        dudes = dudeFactory.MakeDudes(0, Screen.width, 25, 8,
            gameState.isRobotEnemy, gameState.isReligionEnemy, gameState.isLawEnemy);
        styleHuman = new GUIStyle(hudSkin.label);
        styleHuman.normal.textColor = new Color(1, 1, 0);
        styleRobot = new GUIStyle(hudSkin.label);
        styleRobot.normal.textColor = new Color(1, 0, 0);
        styleRobot.alignment = TextAnchor.UpperRight;
        styleScience = new GUIStyle(hudSkin.label);
        styleScience.normal.textColor = new Color(0, 0, 1);
        styleReligion = new GUIStyle(hudSkin.label);
        styleReligion.normal.textColor = new Color(0, 1, 1);
        styleReligion.alignment = TextAnchor.UpperRight;
        styleFreedom = new GUIStyle(hudSkin.label);
        styleFreedom.normal.textColor = new Color(1, 1, 1);
        styleLaw = new GUIStyle(hudSkin.label);
        styleLaw.normal.textColor = new Color(0, 1, 0);
        styleLaw.alignment = TextAnchor.UpperRight;

        styleScore = new GUIStyle(hudSkin.label);
        styleTime = new GUIStyle(hudSkin.label);
        styleTime.alignment = TextAnchor.UpperRight;

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
        DrawComparison(compareRect, tilePrefab.textures[1], tilePrefab.textures[4],
            styleHuman, styleRobot, "Human", "Robot");
        compareRect.yMin += Screen.height * .15f;
        compareRect.yMax += Screen.height * .15f;
        //compareRect = new Rect(Screen.width * .1f, Screen.height * .5f,
        //    Screen.width * .9f, Screen.height * .2f);
        DrawComparison(compareRect, tilePrefab.textures[5], tilePrefab.textures[3],
            styleScience, styleReligion, "Scientific", "Religious");
        compareRect.yMin += Screen.height * .15f;
        compareRect.yMax += Screen.height * .15f;
        DrawComparison(compareRect, tilePrefab.textures[0], tilePrefab.textures[2],
            styleFreedom, styleLaw, "Fight for Freedom", "Fight for Justice");

        Rect extraRect = new Rect(0, Screen.height * .8f,
            Screen.height * .15f, Screen.height * .15f);
        Rect extraLabel = new Rect(0, Screen.height * .95f, 300, 50);
        GUI.DrawTexture(extraRect, tilePrefab.textures[6]);
        GUI.Label(extraLabel, "+Score", styleScore);

        extraRect = new Rect(Screen.width - Screen.height * .15f, Screen.height * .8f,
            Screen.height * .15f, Screen.height * .15f);
        extraLabel = new Rect(Screen.width - 300, Screen.height * .95f, 300, 50);
        GUI.DrawTexture(extraRect, tilePrefab.textures[7]);
        GUI.Label(extraLabel, "+Time", styleTime);

        if (GUI.Button(buttonRect, "Fight the " + gameState.GetEnemyName() + " government!")) {
            Application.LoadLevel("game");
        }
    }

    private void DrawComparison(Rect rect, Texture texLeft, Texture texRight,
            GUIStyle styleLeft, GUIStyle styleRight,
            string textLeft, string textRight) {
        GUI.BeginGroup(rect);
        GUI.DrawTexture(new Rect(0, 0, rect.height, rect.height), texLeft);
        GUI.Label(new Rect(rect.height, 0, 200, 50), textLeft + "?", styleLeft);
        GUI.Label(new Rect(rect.width - rect.height - 200, 0, 200, 50), textRight + "?", styleRight);
        GUI.DrawTexture(new Rect(rect.width - rect.height, 0, rect.height, rect.height), texRight);
        GUI.EndGroup();
    }
}
