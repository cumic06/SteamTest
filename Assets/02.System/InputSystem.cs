using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyType
{
    ChatKey,
    GrapKey,
}

public class InputSystem : Singleton<InputSystem>
{
    #region Values
    public Dictionary<KeyType, KeyCode> keysDic = new();

    public KeyCode openChatKey = KeyCode.Return;
    public KeyCode grapKey = KeyCode.E;
    #endregion

    private void Start()
    {
        SetChangeKey(KeyType.ChatKey, openChatKey);
        SetChangeKey(KeyType.GrapKey, grapKey);
    }

    public void SetChangeKey(KeyType keyType, KeyCode keyCode)
    {
        if (keysDic.ContainsKey(keyType))
        {
            if (keysDic[keyType] == keyCode)
            {
                Debug.LogError("이미 등록된 키입니다.");
                return;
            }
            else
            {
                if (keysDic.ContainsValue(keyCode))
                {
                    //if (keysDic.TryGetValue(keyType, out KeyCode changeKey))
                    //{
                    //    keysDic[keyType] = changeKey;
                    //}
                }
                else
                {
                    keysDic[keyType] = keyCode;
                }
            }
        }
        else
        {
            keysDic.Add(keyType, keyCode);
        }
    }

    public float GetInputHorizontal()
    {
        return Input.GetAxis("Horizontal");
    }

    public float GetInputVertical()
    {
        return Input.GetAxis("Vertical");
    }
}