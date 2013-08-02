using UnityEngine;
using System.Collections;

public class TileScience : TileDetail {

    private float bonusThreshold = 1.5f;
    private int bonusAmount = 5;

    /// Our concept of "match" differs from most others.  Normally "MatchedTile"
    /// refers to the user dragging a type and that specific type creating a match.
    /// If the user drags a tile and the OTHER causes a match, MatchedTile isn't
    /// fired.
    /// That would feel a bit unfair for science, so instead we key in on times
    /// when AddStats is called with a primaryType other than None.  This means
    /// the user dragged for a match, regardless of whether their dragged tile
    /// is part of the matching tiles or not.
    private float lastMatch = Mathf.Infinity;

    public TileScience() {
        type = TileType.Science;
        bonusName = "Breakthrough";
        bonusDescription = "Bonus for fast matches";
        color = new Color(0, 0, 1);
        introDescription = "Scientific";
        LoadTexture();
    }
    public override void AddStatsAsBonus(GameDriver driver, TileType statsType, float amount, TileType primaryType) {
        if (primaryType == TileType.None) {
            return; // this was an cascading or auto-match, ignore it
        }
        float now = driver.GetTime();
        if (lastMatch != Mathf.Infinity && now - lastMatch < bonusThreshold) {
            driver.AddBonus(type, bonusAmount);
        }
        lastMatch = now;
        return;
    }
}
