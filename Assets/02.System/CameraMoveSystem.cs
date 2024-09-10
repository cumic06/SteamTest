using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveSystem : MonoBehaviour
{
    public void Move(Vector3 movePos)
    {
        transform.position = new Vector3(movePos.x, movePos.y, -10);
    }
}