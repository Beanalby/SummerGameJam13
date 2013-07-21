using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dude : MonoBehaviour {

    public Texture2D primary, alt;

    public float xMin, xMax, x, y;
    public bool facingRight;

    private float currentDir, switchCooldown=1f, nextSwitch = -1f;

    public void Update() {
        x = Mathf.Max(xMin + primary.width, Mathf.Min(xMax - primary.width,
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
        if (facingRight) {
            Rect dudeRect = new Rect(x, y, primary.width, primary.height);
            GUI.DrawTexture(dudeRect, primary);
        } else {
            Rect dudeRect = new Rect(x+primary.width, y, -primary.width, primary.height);
            GUI.DrawTexture(dudeRect, primary);
            //GUI.DrawTextureWithTexCoords(dudeRect, primary, new Rect(0, 0, -1, 1));
        }
    }
}
