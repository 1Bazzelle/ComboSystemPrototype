using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;
using static UnityEngine.UI.Image;

public abstract class AttackState
{
    protected CombatInAttack origin;

    protected CombatScript player;

    protected AttackNode curNode;


    private List<Collider2D> enemiesHit = new();

    public abstract void EnterState(CombatInAttack original, CombatScript cs, AttackNode node);
    public abstract void UpdateState();
    public abstract void ExitState();

    protected void DealDamage()
    {
        curNode.enemyHit = false;

        foreach (Hitbox hitbox in curNode.hitboxes)
        {
            Collider2D[] hits = hitbox.GetColliders(player.transform.position, Constants.Layer_Entity);
            foreach (Collider2D enemy in hits)
            {
                if (enemy.CompareTag(Constants.Tag_Enemy) && !enemiesHit.Contains(enemy))
                {
                    curNode.enemyHit = true;

                    enemiesHit.Add(enemy);

                    enemy.GetComponent<IDamageTaker>().TakeDamage(curNode.damage);

                    float knockbackIntensity = curNode.forceIntensity + Random.Range(-curNode.forceIntensity * Constants.chaosValue, curNode.forceIntensity * Constants.chaosValue);
                    enemy.GetComponent<Rigidbody2D>().velocity = curNode.forceDir.normalized * knockbackIntensity;
                }
            }
            List<Collider2D> fuckignArrays = new();
            for (int i = 0; i < hits.Length; i++)
            {
                fuckignArrays.Add(hits[i]);
            }

            for (int i = 0; i < enemiesHit.Count; i++)
            {
                if (!fuckignArrays.Contains(enemiesHit[i])) enemiesHit.RemoveAt(i); 
            }
        }
        
        if (curNode.enemyHit && curNode.freezeFrameTime > 0)
        {
            //if (curNode.freezeFrameSprite != null) player.SetSprite(curNode.freezeFrameSprite);
            player.PlayerAttacked(curNode);
        }
    }
}

public class AttackWindup : AttackState
{
    private float timeElapsed;
    public override void EnterState(CombatInAttack original, CombatScript cs, AttackNode node) 
    {
        origin = original;
        player = cs;
        curNode = node;

        timeElapsed = 0;

        //if (curNode.windupSprite != null) player.SetSprite(curNode.windupSprite);
    }
    public override void UpdateState()
    {/*
        if (!curNode.hasWindup && curNode.windUpTime != 0)
        {
            curNode.windUpTime = 0;
            origin.TransitionToAttackState(new AttackMoveEntities());
        }
        if (curNode.windUpTime < timeElapsed)
        {
            origin.TransitionToAttackState(new AttackMoveEntities());
        }
        */
        timeElapsed += Time.deltaTime;
    }
    public override void ExitState()
    {
        
    }
}
public class AttackCharge : AttackState
{
    private float timeElapsed;
    private bool flashed;
    public override void EnterState(CombatInAttack original, CombatScript cs, AttackNode node)
    {
        origin = original;
        player = cs;
        curNode = node;
        timeElapsed = 0;

        flashed = false;

        //if (curNode.windupSprite != null) player.SetSprite(curNode.windupSprite);
    }
    public override void UpdateState()
    {
        player.Shake(curNode.shakeIntensity);
        if (curNode.chargeTime < timeElapsed)
        {
            if (!flashed)
            {
                player.QueueFlash(0.25f, Color.white);
                flashed = true;
            }
            if(Input.GetKeyUp(Constants.attackInput))
            {
                player.UnShake();
                player.UnFlash();
                
                origin.TransitionToAttackState(new AttackMoveEntities());
            }
        }
        else if(Input.GetKeyUp(Constants.attackInput) && curNode.chargeTime > timeElapsed)
        {
            origin.ReturnToIdle();
        }

        timeElapsed += Time.deltaTime;
    }
    public override void ExitState()
    {

    }
}
public class AttackMoveEntities : AttackState
{
    private float moveProgress;

    private float timeElapsed;

    private float waitAfterMove;

    private Vector2? selfStartPos;

    private List<Collider2D> enemiesToMove = new();
    private List<Vector2> enemyStartPos;
    public override void EnterState(CombatInAttack original, CombatScript cs, AttackNode node)
    {
        /*
        origin = original;
        player = cs;
        curNode = node;

        if (!curNode.movesSelf && !curNode.movesEnemy)
        {
            DealDamage();
        }

        if (curNode.moveAttackBehaviour == AttackNode.MoveAttackBehaviour.ApplyBeforeMove) DealDamage();

        foreach(Hitbox hitbox in curNode.enemyMoveHitboxes)
        {
            foreach(Collider2D enemy in hitbox.GetColliders(player.transform.position, Constants.Layer_Entity))
            {
                if (enemy.CompareTag(Constants.Tag_Enemy)) enemiesToMove.Add(enemy);
            }
        }
        waitAfterMove = 0;
        */
    }
    public override void UpdateState()
    {
        /*
        if (curNode.moveAttackBehaviour == AttackNode.MoveAttackBehaviour.ApplyDuringMove) DealDamage();

        if (curNode.movesSelf && curNode.movesEnemy)
        {
            MovePlayer();
            MoveEnemies();
        }
        else if (curNode.movesSelf && !curNode.movesEnemy)
        {
            MovePlayer();
        }
        else if (!curNode.movesSelf && curNode.movesEnemy)
        {
            MoveEnemies();
        }

        timeElapsed += Time.deltaTime;

        
        moveProgress = timeElapsed / curNode.moveTime;

        if (moveProgress > 1)
        {
            moveProgress = 1;
            waitAfterMove += Time.deltaTime;
        }

        if (moveProgress == 1 && curNode.waitAfterMove < waitAfterMove) origin.TransitionToAttackState(new AttackCooldown());
        */
    }
    public override void ExitState()
    {
        /*
        if (curNode.moveAttackBehaviour == AttackNode.MoveAttackBehaviour.ApplyAfterMove) DealDamage();

        if (curNode.movesSelf)
            player.transform.position = (Vector2)selfStartPos + curNode.moveOffset;

        if (curNode.movesEnemy)
        {
            for(int i = 0; i < enemiesToMove.Count; i++)
            {
                enemiesToMove[i].transform.position = enemyStartPos[i] + curNode.enemyMoveOffset;
            }
        }

        selfStartPos = null;
        */
    }

    private void MovePlayer()
    {
        /*
        if (selfStartPos == null) selfStartPos = player.transform.position;

        if (curNode.moveSprite != null) player.SetSprite(curNode.moveSprite);

        switch (curNode.moveInterpolation)
        {
            case Interpolation.Linear:
                player.transform.position = Vector2.Lerp((Vector2)selfStartPos, (Vector2)selfStartPos + curNode.moveOffset, moveProgress);
                break;
            case Interpolation.EaseIn:
                player.transform.position = 
                    Utility.InterpolationEaseIn((Vector2)selfStartPos, (Vector2)selfStartPos + curNode.moveOffset, moveProgress, curNode.moveInterpolationAmplifier);
                break;
            case Interpolation.EaseOut:
                player.transform.position = 
                    Utility.InterpolationEaseOut((Vector2)selfStartPos, (Vector2)selfStartPos + curNode.moveOffset, moveProgress, curNode.moveInterpolationAmplifier);
                break;
            case Interpolation.EaseInOut:
                player.transform.position = 
                    Utility.InterpolationEaseInOut((Vector2)selfStartPos, (Vector2)selfStartPos + curNode.moveOffset, moveProgress, curNode.moveInterpolationAmplifier);
                break;
        }
        */
    }
    private void MoveEnemies()
    {
        if (enemyStartPos == null)
        {
            enemyStartPos = new();
            foreach (Collider2D enemy in enemiesToMove)
            {
                enemyStartPos.Add(enemy.transform.position);
            }
        }

        for (int i = 0; i < enemiesToMove.Count; i++)
        {
            switch (curNode.moveInterpolation)
            {
                case Interpolation.Linear:
                    enemiesToMove[i].transform.position = Vector2.Lerp(enemyStartPos[i], enemyStartPos[i] + curNode.enemyMoveOffset, moveProgress);
                    break;
                case Interpolation.EaseIn:
                    enemiesToMove[i].transform.position = 
                        Utility.InterpolationEaseIn(enemyStartPos[i], enemyStartPos[i] + curNode.enemyMoveOffset, moveProgress, curNode.enemyMoveInterpolationAmplifier);
                    break;
                case Interpolation.EaseOut:
                    enemiesToMove[i].transform.position = 
                        Utility.InterpolationEaseOut(enemyStartPos[i], enemyStartPos[i] + curNode.enemyMoveOffset, moveProgress, curNode.enemyMoveInterpolationAmplifier);
                    break;
                case Interpolation.EaseInOut:
                    enemiesToMove[i].transform.position = 
                        Utility.InterpolationEaseInOut(enemyStartPos[i], enemyStartPos[i] + curNode.enemyMoveOffset, moveProgress, curNode.enemyMoveInterpolationAmplifier);
                    break;
            }
        }
    }
}
public class AttackCooldown : AttackState
{
    private float timeElapsed;

    public override void EnterState(CombatInAttack original, CombatScript cs, AttackNode node)
    {
        origin = original;
        player = cs;
        curNode = node;

        timeElapsed = 0;

        //player.SetSprite(curNode.attackSprite);
    }
    public override void UpdateState()
    {
        //if (curNode.cooldownTime < timeElapsed) origin.TransitionToAttackState(new AttackFollowUpWindow());

        timeElapsed += Time.deltaTime;
    }
    public override void ExitState()
    {

    }
}
public class AttackFollowUpWindow : AttackState
{
    private float timeElapsed;
    public override void EnterState(CombatInAttack original, CombatScript cs, AttackNode node)
    {
        origin = original;
        player = cs;
        curNode = node;

        timeElapsed = 0;
    }
    public override void UpdateState()
    {
        int availableFollowUps = 0;
        /*
        foreach(AttackNode.FollowUpAttack followUp in curNode.followUpNodes)
        {
            if (followUp.timeWindow > timeElapsed) availableFollowUps++;
            if (followUp.timeWindow > timeElapsed && Input.GetKeyDown(Constants.attackInput) && Input.GetKey(CombatInput.ConvertToKeyCode(followUp.input.secondary, player.GetFacingRight())))
                origin.FinishAttack(followUp.attack);
        }
        */
        
        if (availableFollowUps == 0)
        {
            player.ResetSprite();
            origin.FinishAttack(null);
        }

        timeElapsed += Time.deltaTime;
    }
    public override void ExitState()
    {

    }
}