using UnityEngine;
using System.Collections;

public class TileLaw : TileDetail {

    public TileLaw() {
        type = TileType.Law;
        bonusName = "Uphold Justice";
        bonusDescription = "Bonus for matching same tile repeatedly";
        color = new Color(0, 1, 0);
        introDescription = "Fight for Justice";
        LoadTexture();
    }
}
