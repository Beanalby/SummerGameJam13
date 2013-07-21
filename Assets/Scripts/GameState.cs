using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

    public static GameState instance;

    public float targetScore = 1;

    public bool isRobot;
    public bool isReligion;
    public bool isLaw;

    public bool isRobotPlayer;
    public bool isReligionPlayer;
    public bool isLawPlayer;

    public void Start() {
        if(instance != null) {
            Destroy(gameObject);
        }
        instance = this;

        GameObject.DontDestroyOnLoad(gameObject);
    }

    public void RestartEasier() {
        targetScore /= 2;
        Application.LoadLevel("game");
    }

    public void Transition() {
        Application.LoadLevel("transition");
    }
}
