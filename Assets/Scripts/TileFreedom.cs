using UnityEngine;
using System.Collections;

public class TileFreedom : TileDetail {

    public TileFreedom() {
        type = TileType.Freedom;
        bonusName = "Freedom of Choice";
        bonusDescription = "Bonus for matching different tiles";
        color = new Color(1, 1, 1);
        introDescription = "Fight for Freedom";
        LoadTexture();
    }
}
