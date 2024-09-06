using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Steamworks;
using Steamworks.Data;

public class ChatSystem : MonoBehaviour
{
    public TMP_InputField messageInputField;

    public TextMeshProUGUI messageTemplate;

    public GameObject messageContainer;

    private void OnEnable()
    {
        SteamMatchmaking.OnChatMessage += ChatSent;
        SteamMatchmaking.OnLobbyEntered += LobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined += LobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave += LobbyMemberLeave;
    }


    private void Start()
    {
        messageTemplate.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(InputSystem.Instance.keysDic[KeyType.ChatKey]))
        {
            ToggleChatBox();
        }
    }

    private void ToggleChatBox()
    {
        if (!String.IsNullOrEmpty(messageInputField.text))
        {
            LobbySaveSystem.Instance.CurrentLobby?.SendChatString(messageInputField.text);
            messageInputField.text = "";
        }
        EventSystem.current.SetSelectedGameObject(messageInputField.gameObject);
    }

    #region LobbyStateMsg
    private void LobbyEntered(Lobby lobby)
    {
        AddMessageToBox("You Entered the Lobby");
    }

    private void LobbyMemberLeave(Lobby lobby, Friend friend)
    {
        AddMessageToBox($"{friend.Name} Left the Lobby");
    }

    private void LobbyMemberJoined(Lobby lobby, Friend friend)
    {
        AddMessageToBox($"{friend.Name} Join the Lobby");
    }
    #endregion

    #region Chat
    private void ChatSent(Lobby lobby, Friend friend, string msg)
    {
        AddMessageToBox(msg);
    }

    private void AddMessageToBox(string msg)
    {
        GameObject message = Instantiate(messageTemplate.gameObject, messageContainer.transform);
        message.GetComponent<TextMeshProUGUI>().text = msg;
    }
    #endregion
}