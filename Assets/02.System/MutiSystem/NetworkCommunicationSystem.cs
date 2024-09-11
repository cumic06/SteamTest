using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkCommunicationSystem : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    [ServerRpc]
    public void MovePlayerServerRPC(Vector2 movePos)
    {
        transform.Translate(movePos);
    }

    [ServerRpc]
    public void FlipMovePlayerServerRPC(bool state)
    {
        spriteRenderer.flipX = state;
    }

    [ServerRpc]
    public void AnimationMovePlayerServerRPC(AnimationType animationType, bool state)
    {
        animator.SetBool($"is{animationType}", state);
    }
}