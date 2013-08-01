using UnityEngine;
using System.Collections;

public enum TileType { None, Freedom, Human, Law, Religion, Robot, Science, Score, Time };


public abstract class TileDetail {

    private static TileDetail _Freedom;
    public static TileDetail Freedom {
        get {
            if (_Freedom == null)
                _Freedom = new TileFreedom();
            return _Freedom;
        }
    }
    private static TileDetail _Human;
    public static TileDetail Human {
        get {
            if (_Human == null)
                _Human = new TileHuman();
            return _Human;
        }
    }
    private static TileDetail _Law;
    public static TileDetail Law {
        get {
            if (_Law == null)
                _Law = new TileLaw();
            return _Law;
        }
    }
    private static TileDetail _Religion;
    public static TileDetail Religion {
        get {
            if (_Religion == null)
                _Religion = new TileReligion();
            return _Religion;
        }
    }
    private static TileDetail _Robot;
    public static TileDetail Robot {
        get {
            if (_Robot == null)
                _Robot = new TileRobot();
            return _Robot;
        }
    }
    private static TileDetail _Science;
    public static TileDetail Science {
        get {
            if (_Science == null)
                _Science = new TileScience();
            return _Science;
        }
    }
    private static TileDetail _Score;
    public static TileDetail Score {
        get {
            if (_Score == null)
                _Score = new TileScore();
            return _Score;
        }
    }
    private static TileDetail _Time;
    public static TileDetail Time {
        get {
            if (_Time == null)
                _Time = new TileTime();
            return _Time;
        }
    }

    public static TileDetail Get(TileType type) {
        switch (type) {
            case TileType.Freedom: return Freedom;
            case TileType.Human: return Human;
            case TileType.Law: return Law;
            case TileType.Religion: return Religion;
            case TileType.Robot: return Robot;
            case TileType.Science: return Science;
            case TileType.Score: return Score;
            case TileType.Time: return Time;
            default: throw new System.ArgumentException(type.ToString());
        }
    }
    public static TileDetail GetOpposite(TileType type) {
        switch (type) {
            case TileType.Freedom: return Law;
            case TileType.Human: return Robot;
            case TileType.Law: return Freedom;
            case TileType.Religion: return Science;
            case TileType.Robot: return Human;
            case TileType.Science: return Freedom;
            case TileType.Score: return null;
            case TileType.Time: return null;
            default: throw new System.ArgumentException(type.ToString());
        }
    }

    public TileType type;
    public string introDescription;
    public string bonusName;
    public string bonusDescription;
    public Color color;
    public Texture2D texture;

    protected void LoadTexture() {
        texture = Resources.Load("Icons/Icon_" + type.ToString(), typeof(Texture2D)) as Texture2D;
    }
    public virtual void MatchedTiles(GameDriver driver, TileType matchedType) { }
}
