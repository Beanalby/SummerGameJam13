﻿using UnityEngine;
using System.Collections;

public class HintArrow : MonoBehaviour {
    public Vector3 hintPosition;
    public Position hintDirection;
    public float moveDuration;
    public Texture2D tex;

    private GameObject mesh;
    private float startTime;
    private Vector3 moveStart;
    private Vector3 moveDelta;
    private Interpolate.Function ease = Interpolate.Ease(Interpolate.EaseType.Linear);
    MeshRenderer mr;
    Color color;

    public void Start() {
        mesh = transform.Find("Mesh").gameObject;
        if (hintDirection.y == 1) {
            mesh.transform.localEulerAngles = new Vector3(0, 0, 0);
        } else if (hintDirection.x == -1) {
            mesh.transform.localEulerAngles = new Vector3(0, 0, 90);
        } else if(hintDirection.y == -1) {
            mesh.transform.localEulerAngles = new Vector3(0, 0, 180);
        } else if (hintDirection.x == 1) {
            mesh.transform.localEulerAngles = new Vector3(0, 0, 270);
        }
        moveStart = hintPosition;
        moveDelta = new Vector3(hintDirection.x, hintDirection.y, 0);
        startTime = Time.time;
        mr = GetComponentInChildren<MeshRenderer>();
        if (tex != null) {
            mr.material.mainTexture = tex;
        }
        color = new Color(1, 1, 1, 1);
    }

    public void Update() {
        float percent = ((Time.time - startTime) % moveDuration) / moveDuration;
        transform.position = Interpolate.Ease(ease, moveStart, moveDelta, percent, 1);
        if (percent <= .5f) {
            color.a = 1;
        } else {
            color.a = -2f * percent + 2f;
        }
        mr.material.color = color;
    }
}
