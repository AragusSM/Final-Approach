using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameState state;
    public static int score; // score

    public static void UpdateGameState(GameState newState) {
        switch (newState) {
            case GameState.Menu:
                state = GameState.Menu;
                break;
            case GameState.AirTrafficControl:
                state = GameState.AirTrafficControl;
                break;
            case GameState.GameOver:
                state = GameState.GameOver;
                break;
        }
    }

}

public enum GameState {
    Menu,
    AirTrafficControl,
    GameOver
}
