using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dude : MonoBehaviour {

    public Texture2D texStatic;
    public Texture2D[] texAnim;

    public float xMin, xMax, x, y;
    public bool facingRight;

    private float currentDir, switchCooldown=3f, nextSwitch = -1f;

    int currentFrame;
    float nextFrameSwitch;

    public void Update() {
        if (Time.time > nextFrameSwitch) {
            currentFrame = (currentFrame + 1) % 4;
            nextFrameSwitch = Time.time + .25f;
        }
        x = Mathf.Max(xMin + texStatic.width, Mathf.Min(xMax - texStatic.width,
            x + currentDir * Time.deltaTime));
        if (Time.time > nextSwitch) {
            if (currentDir == 0) {
                currentDir = Random.Range(15f, 35f);
                if (Random.Range(0f, 1f) > .5f) {
                    currentDir = -currentDir;
                }
                facingRight = currentDir > 0;
            } else {
                currentDir = 0;
            }
            nextSwitch = Time.time + Random.Range(switchCooldown * .5f,
                switchCooldown * 1.5f);
        }
    }

    public void OnGUI() {
        Rect dudeRect;
        if (facingRight) {
            dudeRect = new Rect(x, y, texStatic.width, texStatic.height);
        } else {
            dudeRect = new Rect(x+texStatic.width, y, -texStatic.width, texStatic.height);
        }
        if (currentDir != 0 && texAnim != null && texAnim.Length == 4) {
            GUI.DrawTexture(dudeRect, texAnim[currentFrame]);
        } else {
            GUI.DrawTexture(dudeRect, texStatic);
        }
    }
}
