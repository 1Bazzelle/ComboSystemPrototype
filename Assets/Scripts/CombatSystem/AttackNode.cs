using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public struct Hitbox
{
    public Vector2 offset;
    public Vector2 boxSize;
    public readonly Collider2D[] GetColliders(Vector2 playerPos, LayerMask layerMask)
    {
        return Physics2D.OverlapBoxAll(playerPos + offset, boxSize, 0, layerMask);
    }
}

public enum Interpolation
{
    Linear,
    EaseIn,
    EaseOut,
    EaseInOut
}

[System.Serializable]
public class AttackNode
{
    public enum Type
    {
        SpriteChange,
        Wait,
        Charge,
        Attack,
        ApplyKnockback,
        MoveSelf,
        MoveEnemies,
        EndAttack,
    }

    public enum Element
    {
        None,
        Fire,
        Stun,
        Blunt
    }

    public Type type;

    public Sprite sprite;

    public float waitTime;

    public float chargeTime;
    public bool shakeWhileCharge;
    public float shakeIntensity;


    public Interpolation moveInterpolation;
    public float moveInterpolationAmplifier;
    public Vector2 moveOffset;

    public Interpolation enemyMoveInterpolation;
    public float enemyMoveInterpolationAmplifier;
    public List<Hitbox> enemyMoveHitboxes;
    public Vector2 enemyMoveOffset;

    public float moveTime;


    public List<Hitbox> hitboxes;

    public float freezeFrameTime;
    public Sprite freezeFrameSprite;

    public float damage;
    public Element element;

    public bool enemyHit;

    public Sprite strikeSprite;
    public Vector2 strikeSpritePos;
    public float strikeSpriteLifetime;

    public Vector2 forceDir;
    public float forceIntensity;

    [System.Serializable]
    public struct FollowUpAttack
    {
        public bool showGizmos;
        public float timeWindow;
        public CombatInput input;
        public Attack attack;
    }

    public List<FollowUpAttack> followUpAttacks;
}