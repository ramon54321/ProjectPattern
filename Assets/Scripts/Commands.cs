using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

/*

 Adding a server command

 1. Add class to ServerCommand
 2. Add to ParseServerCommand
 3. Add to ExecuteCommand
 4. Add event in Commands
 5. Add listeners where required

*/

public interface IServerCommand { }

public sealed class ServerCommand
{
    [Serializable]
    public class FullState : IServerCommand
    {
		public int worldWidth;
		public int worldHeight;
		public List<Vector2Int> hexByPositionKeys;
        public List<Hex> hexByPositionValues;
        public List<string> hexPositionsByNationNameKeys;
        public List<ListVector2Int> hexPositionsByNationNameValues;
    }

    [Serializable]
    public class InvalidPlayerInfo : IServerCommand
    {
        public string error;
    }

    public class Unknown : IServerCommand
    {
        public string type;
        public string json;

        public Unknown(string type, string json)
        {
            this.type = type;
            this.json = json;
        }
    }
}

public class Commands
{
    public static ConcurrentQueue<IServerCommand> serverCommands = new ConcurrentQueue<IServerCommand>();

    public static event Action<ServerCommand.FullState> onFullState;
    public static event Action<ServerCommand.InvalidPlayerInfo> onInvalidPlayerInfo;
    public static event Action<ServerCommand.Unknown> onUnknown;

    private static IServerCommand ParseServerCommand(string message)
    {
        var messageParts = message.Split('|');
        if (messageParts.Length != 2)
        {
            return null;
        }
        var type = messageParts[0];
        var json = messageParts[1];

		//Debug.Log($"type: {type} | json: {json}");

		switch (type)
        {
            case "FullState":
                return JsonUtility.FromJson<ServerCommand.FullState>(json);
            case "InvalidPlayerInfo":
                return JsonUtility.FromJson<ServerCommand.InvalidPlayerInfo>(json);
            default:
                return new ServerCommand.Unknown(type, json);
        }
    }

    public static void ExecuteCommand(IServerCommand serverCommand)
    {
        switch (serverCommand)
        {
            case ServerCommand.FullState command:
                onFullState?.Invoke(command);
                break;
            case ServerCommand.InvalidPlayerInfo command:
                onInvalidPlayerInfo?.Invoke(command);
                break;
            case ServerCommand.Unknown command:
                onUnknown?.Invoke(command);
                break;
        }
    }

    public Commands()
    {
        NetworkListener.onServerMessage += (message) => {
            var serverCommand = ParseServerCommand(message);
            serverCommands.Enqueue(serverCommand);
        };
    }
}
