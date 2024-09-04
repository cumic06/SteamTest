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
        if (messageInputField.gameObject.activeSelf)
        {
            if (!String.IsNullOrEmpty(messageInputField.text))
            {
                LobbySaveSystem.Instance.CurrentLobby?.SendChatString(messageInputField.text);
                messageInputField.text = "";
            }
            messageInputField.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            messageInputField.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(messageInputField.gameObject);
        }
    }

    private void ChatSent(Lobby lobby, Friend friend, string msg)
    {
        AddMessageToBox(msg);
    }

    private void AddMessageToBox(string msg)
    {
        GameObject message = Instantiate(messageTemplate.gameObject, messageContainer.transform);
        message.GetComponent<TextMeshProUGUI>().text = msg;
    }
}