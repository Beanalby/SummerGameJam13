using UnityEngine;
using System.Collections;

public class TileHuman : TileDetail {

    public TileHuman() {
        type = TileType.Human;
        bonusName = "Human Creativity";
        bonusDescription = "Randomly doubles matches";
        color = new Color(1, 1, 0);
        introDescription = "Human";
        LoadTexture();
    }
}
