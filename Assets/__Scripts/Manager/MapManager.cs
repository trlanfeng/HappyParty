using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {

    public enum MapState
    {
        IDLE,
        DICE,
        MOVE,
        CHANGE,
        GAME
    }

    public MapState mapState;
}
