using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    public SyncListString players = new SyncListString();
    public string[] availablePlayers = new string[3] { "Alice", "Bob", "Slasher" };
    public string currentPlayer = null;

    public string registerPlayer()
    {
        Debug.Log("Registering player");
        string playerId = availablePlayers[players.Count];
        players.Add(playerId);
        return playerId;
    }
}