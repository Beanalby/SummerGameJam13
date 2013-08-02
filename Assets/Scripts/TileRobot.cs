using UnityEngine;
using System.Collections;

public class TileRobot : TileDetail {

    public float bonusChance = 20;

    private Texture2D hintArrowRobot;

    public TileRobot() {
        type = TileType.Robot;
        bonusName = "Automation";
        bonusDescription = "Tiles occasionally auto-match";
        color = new Color(1, 0, 0);
        introDescription = "Robot";
        LoadTexture();
        hintArrowRobot = Resources.Load("hintArrowRobot", typeof(Texture2D)) as Texture2D;
    }

    public override void UpdateAsBonus(GameDriver driver) {
        // each second after a match, has bonusChance to automatch
        if (driver.board.IsNewIdleSecond
            && Random.Range(0, 100) < bonusChance) {
            PairInt position, direction;
            if (driver.board.FindMatch(out position, out direction)) {
                driver.board.SwapTile(position, direction, false);
                driver.board.CreateHintOneShot(position, direction, hintArrowRobot);
                driver.AddBonus(type);
            }
        }
    }


}
