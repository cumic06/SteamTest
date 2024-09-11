using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : UnitController
{
    protected Camera myCamera;
    protected CameraMoveSystem camMoveSystem;
    protected NetworkCommunicationSystem networkCommunicationSystem;

    protected override void Awake()
    {
        base.Awake();
        networkCommunicationSystem = GetComponent<NetworkCommunicationSystem>();
        myCamera = Camera.main;
        camMoveSystem = myCamera.GetComponent<CameraMoveSystem>();
    }

    protected virtual void FixedUpdate()
    {
        if (InputSystem.Instance.GetInputHorizontal() != 0)
        {
            Move();
            Flip();
        }
        else
        {
            networkCommunicationSystem.AnimationMovePlayerServerRPC(AnimationType.Walk, false);
        }

        camMoveSystem.Move(transform.position);
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    #region Movement
    protected override void Move()
    {
        Vector3 moveVec = new(InputSystem.Instance.GetInputHorizontal(), 0, 0);
        Vector3 movePos = moveSpeed.currentMoveSpeed * Time.deltaTime * moveVec;

        networkCommunicationSystem.MovePlayerServerRPC(movePos);

        networkCommunicationSystem.AnimationMovePlayerServerRPC(AnimationType.Walk, true);
    }

    protected override void Flip()
    {
        networkCommunicationSystem.FlipMovePlayerServerRPC(InputSystem.Instance.GetInputHorizontal() < 0);
    }

    protected override void Jump()
    {
        rigid.AddForce(Vector2.up * moveSpeed.jumpPower, ForceMode2D.Impulse);
    }
    #endregion

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            rigid.gravityScale = 0;
            transform.position = new Vector3(2, 0);
            rigid.gravityScale = 1;
        }
        else
        {
            rigid.gravityScale = 0;
        }

        if (IsOwner)
        {

        }
        else
        {
            enabled = false;
        }
    }

}