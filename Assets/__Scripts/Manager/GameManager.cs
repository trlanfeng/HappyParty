using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public enum GameState
    {
        SPLASH,
        LOADING,
        HALL,
        MAP,
        END
    }

    public GameState gameState;
    
    void Start()
    {
        gameState = GameState.SPLASH;
    }

    void Update()
    {
        switch (gameState)
        {
            case GameState.SPLASH:
                break;
            case GameState.LOADING:
                gameState = GameState.HALL;
                break;
            case GameState.HALL:
                gameState = GameState.MAP;
                break;
            case GameState.MAP:
                break;
            case GameState.END:
                break;
        }
    }

}
