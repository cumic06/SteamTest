using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : UnitController
{
    protected Camera myCamera;
    CameraMoveSystem camMoveSystem;

    protected override void Start()
    {
        myCamera = Camera.main;
        camMoveSystem = myCamera.GetComponent<CameraMoveSystem>();
    }

    protected virtual void FixedUpdate()
    {
        Move();
        camMoveSystem.Move(transform.position);
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    protected override void Move()
    {
        Vector3 moveVec = new(InputSystem.Instance.GetInputHorizontal(), 0, 0);
        transform.Translate(moveSpeed.currentMoveSpeed * Time.deltaTime * moveVec);
    }

    protected override void Jump()
    {
        rigid.AddForce(Vector2.up * moveSpeed.jumpPower, ForceMode2D.Impulse);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner)
        {
            rigid.gravityScale = 0;
            enabled = false;
            Debug.Log("NotOwner");
        }
        else
        {
            rigid.gravityScale = 0;
            transform.position = new Vector3(2, 0);
            rigid.gravityScale = 1;
            Debug.Log("Owner");
        }
    }
}