using UnityEngine;
using System.Collections;

public class TileTime : TileDetail {

    public TileTime() {
        type = TileType.Time;
        color = Color.yellow;
        LoadTexture();
    }
}
