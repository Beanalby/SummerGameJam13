using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransitionDriver : MonoBehaviour {
    public GUISkin skin;

    private GameState gameState;
    string enemyDisplay, playerDisplay;

    public DudeFactory dudeFactory;

    public void Start() {
        gameState = GameState.instance;
        enemyDisplay = gameState.GetEnemyName();
        playerDisplay = gameState.GetPlayerName();
        dudeFactory.MakeDudes(0, Screen.width, 200, 8,
            gameState.isRobotPlayer, gameState.isReligionPlayer, gameState.isLawPlayer);
    }

    public void OnGUI() {
        GUI.skin = skin;

        GUI.Label(new Rect(Screen.width * .25f, 0,
            Screen.width * .5f, Screen.height * .25f),
            "Congradulations, you've toppled the " + enemyDisplay
            + " that ran the government.");
        GUI.Label(new Rect(Screen.width * .25f, Screen.height * .25f,
            Screen.width * .5f, Screen.height * .25f),
            "Your " + playerDisplay + " will surely have a long, peaceful rule.");
        Rect buttonRect = new Rect(Screen.width * .4f, Screen.height * .75f,
            Screen.width * .2f, 100);
        if (GUI.Button(buttonRect, "230 years later...")) {
            gameState.Restart();
        }
    }


}
