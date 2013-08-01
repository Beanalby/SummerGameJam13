using UnityEngine;
using System.Collections;

public class TileLaw : TileDetail {

    private int bonusAmount = 5;

    public TileLaw() {
        type = TileType.Law;
        bonusName = "Uphold Justice";
        bonusDescription = "Bonus for matching same tile repeatedly";
        color = new Color(0, 1, 0);
        introDescription = "Fight for Justice";
        LoadTexture();
    }

    public override void MatchedTilesAsBonus(GameDriver driver, TileType matchedType) {
        if (driver.GetCurrentStreak() >= 2) {
            driver.AddBonus(type, bonusAmount);
        }
    }
}