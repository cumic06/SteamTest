using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;

public class PlayerSpawnerSystem : NetworkBehaviour
{
    public GameObject Player;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneLoaded;
    }

    private void SceneLoaded(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (IsHost && sceneName == SceneType.GameScene.ToString())
        {
            foreach (ulong id in clientsCompleted)
            {
                GameObject player = Instantiate(Player);
                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);
            }
        }
    }
}