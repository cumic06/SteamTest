using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public abstract class UnitController : NetworkBehaviour, IDamageable, IHealable
{
    #region Value
    public Health health;
    public MoveSpeed moveSpeed;

    protected bool isGround;

    protected Rigidbody2D rigid;
    protected CapsuleCollider2D capsuleCollider;

    public Action onDamageAction;
    public Action onHealAction;
    #endregion

    protected virtual void Awake()
    {
        rigid = GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        capsuleCollider = GetComponent(typeof(CapsuleCollider2D)) as CapsuleCollider2D;
    }

    protected virtual void Start()
    {

    }

    #region HP
    public void TakeDamage(int damage)
    {
        health.currentHP -= damage;
        onDamageAction?.Invoke();
        ClampHP();
    }

    public void TakeHeal(int heal)
    {
        health.currentHP += heal;
        onHealAction?.Invoke();
        ClampHP();
    }

    protected void ClampHP()
    {
        health.currentHP = Mathf.Clamp(health.currentHP, 0, health.maxHP);
    }

    #endregion

    protected abstract void Move();
}