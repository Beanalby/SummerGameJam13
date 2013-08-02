using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

    private static GameState _instance;
    public static GameState instance {
        get {
            if(_instance == null) {
                GameObject obj = new GameObject();
                obj.name = "[GameState]";
                GameState state = obj.AddComponent<GameState>();
                _instance = state;
                _instance.isRobotEnemy = Random.Range(0f, 1f) < .5f;
                _instance.isReligionEnemy = Random.Range(0f, 1f) < .5f;
                _instance.isLawEnemy = Random.Range(0f, 1f) < .5f;
            }
            return _instance;
        }
    }

    public float gameDuration = 60;
    public int targetScore = 100;

    public bool isRobotEnemy = false;
    public bool isReligionEnemy = false;
    public bool isLawEnemy = false;

    public bool isRobotPlayer = false;
    public bool isReligionPlayer = false;
    public bool isLawPlayer = false;

    public void Start() {
        if(_instance != null && _instance != this) {
            Destroy(gameObject);
        }

        GameObject.DontDestroyOnLoad(gameObject);
    }
    public void RestartEasier() {
        targetScore /= 2;
        Application.LoadLevel("intro");
    }

    public void Restart() {
        isRobotEnemy = isRobotPlayer;
        isReligionEnemy = isReligionPlayer;
        isLawEnemy = isLawPlayer;
        targetScore = (int)(targetScore * 1.5);
        Application.LoadLevel("intro");
    }
    public void Transition(bool isRobotNew, bool isReligionNew, bool isLawNew) {
        isRobotPlayer = isRobotNew;
        isReligionPlayer = isReligionNew;
        isLawPlayer = isLawNew;
        Application.LoadLevel("transition");
    }

    public string Flags2name(bool isRobot, bool isReligion, bool isLaw) {
        string name = "";
        if (isRobot) {
            name = "Robotic " + name;
        } else {
            name = "Pro-human " + name;
        }
        if (isReligion) {
            name = name + "Religious ";
        } else {
            name = name + "Scientific ";
        }
        if (isLaw) {
            name = name + "Police State";
        } else {
            name = name + "Anarchist";
        }
        return name;
    }
    public string GetPlayerName() {
        return Flags2name(isRobotPlayer, isReligionPlayer, isLawPlayer);
    }
    public string GetEnemyName() {
        return Flags2name(isRobotEnemy, isReligionEnemy, isLawEnemy);
    }
}
