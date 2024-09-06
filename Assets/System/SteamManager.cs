using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;
using Steamworks.Data;
using System;
using Unity.Netcode;
using Netcode.Transports.Facepunch;
using UnityEngine.SceneManagement;

public class SteamManager : MonoBehaviour
{
    private Lobby currentLobby;
    public int maxMember = 4;//테스트라 일단 4

    public GameObject mainMenu;
    public GameObject inLobbyMenu;

    public TMP_InputField lobyIDInputField;
    public TextMeshProUGUI lobbyID;

    private void OnEnable()
    {
        SteamMatchmaking.OnLobbyCreated += LobbyCreated;
        SteamMatchmaking.OnLobbyEntered += LobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested += GameLobbyJoinRequseted;
    }


    private void OnDisable()
    {
        SteamMatchmaking.OnLobbyCreated -= LobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= LobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested -= GameLobbyJoinRequseted;
    }

    #region Lobby
    private async void GameLobbyJoinRequseted(Lobby lobby, SteamId id)
    {
        await lobby.Join();
    }

    private void LobbyCreated(Result result, Lobby lobby)
    {
        if (result == Result.OK)
        {
            lobby.SetPublic();
            lobby.SetJoinable(true);
            NetworkManager.Singleton.StartHost();
        }
    }

    private void LobbyEntered(Lobby lobby)
    {
        LobbySaveSystem.Instance.CurrentLobby = lobby;
        lobbyID.text = $"{lobby.Id}";
        CheckUI();

        if (NetworkManager.Singleton.IsHost) return;
        NetworkManager.Singleton.gameObject.GetComponent<FacepunchTransport>().targetSteamId = lobby.Owner.Id;
        NetworkManager.Singleton.StartClient();
    }

    public async void CreateHostLoby()
    {
        await SteamMatchmaking.CreateLobbyAsync(maxMember);
    }

    public async void JoinLobbyWithID()
    {
        ulong id;

        if (!ulong.TryParse(lobyIDInputField.text, out id))
            return;

        Lobby[] lobbies = await SteamMatchmaking.LobbyList.WithSlotsAvailable(1).RequestAsync();

        foreach (var lobby in lobbies)
        {
            if (lobby.Id == id)
            {
                await lobby.Join();
                return;
            }
        }
    }

    public void LeaveLobby()
    {
        LobbySaveSystem.Instance.CurrentLobby?.Leave();
        LobbySaveSystem.Instance.CurrentLobby = null;
        NetworkManager.Singleton.Shutdown();
        CheckUI();
    }
    #endregion


    public void CopyID()
    {
        TextEditor textEditor = new();
        textEditor.text = lobbyID.text;
        textEditor.SelectAll();
        textEditor.Copy();
    }


    private void CheckUI()
    {
        if (LobbySaveSystem.Instance.CurrentLobby == null)
        {
            mainMenu.SetActive(true);
            inLobbyMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(false);
            inLobbyMenu.SetActive(true);
        }
    }

    public void StartGameSever()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            Debug.Log(NetworkManager.Singleton.ConnectedHostname);
            NetworkManager.Singleton.SceneManager.LoadScene(SceneType.GameScene.ToString(), LoadSceneMode.Single);
        }
    }
}