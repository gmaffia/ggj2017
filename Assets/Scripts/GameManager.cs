using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class GameManager : NetworkBehaviour {

    public GameObject Alice;
    public GameObject Bob;
    public GameObject Slasher;
    public SyncListString players = new SyncListString();
    public SyncListInt spawnIndexes = new SyncListInt();
    public string[] availablePlayers = new string[3] { "Alice", "Bob", "Slasher" };
    public string currentPlayer = null;
<<<<<<< HEAD
	public string HeroScene;
	public string SlasherScene;
=======

    void Start()
    {        
        GameObject soundBank = GameObject.Find("WwiseGlobal").GetComponent<AkBank>().gameObject;
        AkSoundEngine.PostEvent("play_music", soundBank);
    }

    public void printPlayers()
    {
        for(int i = 0;  i < players.Count; i++)
        {
            Debug.Log("Player " + i + " -> " + players[i]);
        }
    }

>>>>>>> 47f71919bc1892c3bfe90eace3108712c512d9dc
    public string registerPlayer()
    {
        string playerId = availablePlayers[players.Count];
        players.Add(playerId);
        return playerId;
    }

    public GameObject findSpawnPointByPlayerId(string playerId)
    {
        GameObject toReturn = null;
        if(playerId != "Slasher")
        {
            Debug.Log("I'm not a slasher, I'm " + playerId);
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("HeroSpawnPoint");
            bool foundSpawnPoint = false;
            while(!foundSpawnPoint)
            {
                // Get random spawn point index
                int index = (int)Random.Range(0, spawnPoints.Length);
                Debug.Log("Gonna try " + index.ToString());
                if( !spawnIndexes.Contains(index) )
                {
                    Debug.Log("Gonna add new spawn point to list");
                    spawnIndexes.Add(index);
                    foundSpawnPoint = true;
                }
            }
        }
        else
        {
            GameObject spawnPoint = GameObject.FindGameObjectWithTag("SlasherSpawnPoint");
            toReturn = spawnPoint;
        }
        return toReturn;
    }

	public void RpcHeroesWin(){
		FindObjectOfType<CustomNetworkManager> ().ServerChangeScene (HeroScene);
	}
	public void RpcSlasherWin(){
		FindObjectOfType<CustomNetworkManager> ().ServerChangeScene (SlasherScene);
	}
}