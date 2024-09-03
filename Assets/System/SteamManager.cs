using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;
using Steamworks.Data;
using System;

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

    private async void GameLobbyJoinRequseted(Lobby lobby, SteamId id)
    {
        await lobby.Join();
    }

    private void LobbyEntered(Lobby lobby)
    {
        LobbySaveSystem.Instance.CurrentLobby = lobby;
        lobbyID.text = lobby.Id.ToString();
        mainMenu.SetActive(false);
        inLobbyMenu.SetActive(true);
    }

    private void LobbyCreated(Result result, Lobby lobby)
    {
        if (result == Result.OK)
        {
            lobby.SetPublic();
            lobby.SetJoinable(true);
        }
    }

    public async void CreateHostLoby()
    {
        await SteamMatchmaking.CreateLobbyAsync(maxMember);
    }

    public async void JoinLobyWithID()
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
}