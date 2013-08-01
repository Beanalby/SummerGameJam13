using UnityEngine;
using System.Collections;

public class TileReligion : TileDetail {

    private float hintCooldown = 1.5f;

    private Texture2D hintArrowHoly;
    
    public TileReligion() {
        type = TileType.Religion;
        bonusName = "Divine Inspiration";
        bonusDescription = "Highlights matches faster";
        color = new Color(0, 1, 1);
        introDescription = "Religious";
        LoadTexture();
        hintArrowHoly = Resources.Load("hintArrowHoly", typeof(Texture2D)) as Texture2D;
    }

    public override void UpdateAsBonus(GameDriver driver) {
        if (driver.board.HandleHint(hintCooldown, hintArrowHoly)) {
            driver.AddBonus(type);
        }
    }
}
