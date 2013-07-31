using UnityEngine;
using System.Collections;

public class TileReligion : TileDetail {

    public TileReligion() {
        type = TileType.Religion;
        bonusName = "Divine Inspiration";
        bonusDescription = "Highlights matches faster";
        color = new Color(0, 1, 1);
        introDescription = "Religious";
        LoadTexture();
    }
}
