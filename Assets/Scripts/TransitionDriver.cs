using UnityEngine;
using System.Collections;

public class TransitionDriver : MonoBehaviour {
    private GameState gameState;

    public void Start() {
        gameState = GameState.instance;
        Debug.Log("Transitioning...");
    }


}
