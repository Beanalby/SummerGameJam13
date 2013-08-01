using UnityEngine;
using System.Collections;

public class TileHuman : TileDetail {

    private int bonusChance = 10;

    public TileHuman() {
        type = TileType.Human;
        bonusName = "Human Creativity";
        bonusDescription = "Randomly doubles matches";
        color = new Color(1, 1, 0);
        introDescription = "Human";
        LoadTexture();
    }
    public override void MatchedTilesAsBonus(GameDriver driver, TileType matchedType) {
        if (Random.Range(0, 100) <= bonusChance) {
            driver.AddStats(matchedType, 3, true);
            driver.AddBonus(type);
        }
    }
}
