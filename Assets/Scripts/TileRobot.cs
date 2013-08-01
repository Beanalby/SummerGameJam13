using UnityEngine;
using System.Collections;

public class TileRobot : TileDetail {

    public TileRobot() {
        type = TileType.Robot;
        bonusName = "Automation";
        bonusDescription = "Tiles occasionally auto-match";
        color = new Color(1, 0, 0);
        introDescription = "Robot";
        LoadTexture();
    }
}
