using UnityEngine;
using System.Collections;

public class HallManager : MonoBehaviour {

    public enum HallState
    {
        READY
    }

    public enum GameMode
    {
        One,
        Two,
        Three,
        Four
    }

    public GameMode gameMode = GameMode.One;

    public void modeSelect(int i)
    {
        gameMode = (GameMode)i;
        switch (gameMode)
        {
            case GameMode.One:
                Application.LoadLevel("town1");
                break;
            case GameMode.Two:
                Application.LoadLevel("town1");
                break;
            case GameMode.Three:
                Application.LoadLevel("town1");
                break;
            case GameMode.Four:
                Application.LoadLevel("town1");
                break;
        }
    }
}
