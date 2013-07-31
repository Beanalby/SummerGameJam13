using UnityEngine;
using System.Collections;

public class TileScience : TileDetail {

    public TileScience() {
        type = TileType.Science;
        bonusName = "Breakthrough";
        bonusDescription = "Bonus for fast matches";
        color = new Color(0, 0, 1);
        introDescription = "Scientific";
        LoadTexture();
    }
}
