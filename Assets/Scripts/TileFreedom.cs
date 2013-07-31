using UnityEngine;
using System.Collections;

public class TileFreedom : TileDetail {

    private int bonusAmount = 3;

    public TileFreedom() {
        type = TileType.Freedom;
        bonusName = "Freedom of Choice";
        bonusDescription = "Bonus for matching different tiles";
        color = new Color(1, 1, 1);
        introDescription = "Fight for Freedom";
        LoadTexture();
    }

    public override void MatchedTiles(GameDriver driver) {
        if (driver.GetCurrentAntiStreak() >= 4) {
            driver.AddBonus(type, bonusAmount);
        }
    }
}
