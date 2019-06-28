//using UnityEngine;

//public class World : MonoBehaviour
//{

//}

using UnityEngine;
using System.Collections.Generic;
using System;

public class GameState
{
    public Dictionary<Vector2Int, Hex> hexByPosition = new Dictionary<Vector2Int, Hex>();
    public Dictionary<string, List<Vector2Int>> hexPositionsByNationName = new Dictionary<string, List<Vector2Int>>();
}

public class GameManager : MonoBehaviour
{
    public static NetworkListener networkListener;
    public static Commands commands;

    public static GameState gameState;

    public static event Action onRebuildWorld;

    void Awake()
    {
        networkListener = new NetworkListener();
        commands = new Commands();

        Commands.onFullState += command =>
        {
            bool initialLoad = gameState == null;

            if (initialLoad)
            {
                gameState = new GameState();
            }

            bool setHexByPosition = Utils.ListToDictionary(command.hexByPositionKeys, command.hexByPositionValues, ref gameState.hexByPosition);
            bool setHexPositionsByNationName = Utils.ListToDictionaryList(command.hexPositionsByNationNameKeys, command.hexPositionsByNationNameValues, ref gameState.hexPositionsByNationName);

            if (setHexByPosition && setHexPositionsByNationName)
            {
                Debug.Log("[GAME MANAGER] State update successful");
            }
            else
            {
                Debug.LogError("[GAME MANAGER] State update failed!");
            }

            if (initialLoad)
            {
                onRebuildWorld?.Invoke();
            }

            Debug.Log(gameState.hexByPosition[new Vector2Int(1, 1)].weather.temperature);
        };

        Commands.onInvalidPlayerInfo += command =>
        {
            Debug.LogWarning($"[GAME MANAGER] Invalid player info sent to server as {command.error}");
        };

        Commands.onUnknown += command =>
        {
            Debug.LogWarning($"[GAME MANAGER] Unknown Server Command of type {command.type} and json {command.json}");
        };
    }

    void Update()
    {
        while (Commands.serverCommands.TryDequeue(out IServerCommand serverCommand))
        {
            Commands.ExecuteCommand(serverCommand);
        }
    }

    private void OnDestroy()
    {
        networkListener.Close();
    }
}
