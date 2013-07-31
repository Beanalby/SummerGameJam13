using UnityEngine;
using System.Collections;

public class TileScore : TileDetail {

    public TileScore() {
        type = TileType.Score;
        LoadTexture();
    }
}
