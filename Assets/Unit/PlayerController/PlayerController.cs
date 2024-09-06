using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : UnitController
{
    protected override void Start()
    {

    }

    protected override void Move()
    {

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