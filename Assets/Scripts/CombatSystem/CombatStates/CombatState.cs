using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.XR;

public abstract class CombatState
{
    protected CombatScript origin;
    protected List<AttackNode.FollowUpAttack> possibleAttacks;
    public abstract void EnterState(CombatScript player, List<AttackNode.FollowUpAttack> attackList);
    public abstract void UpdateState();
    public abstract void ExitState();
    public void ReturnToIdle()
    {
        origin.ResetSprite();
        origin.UnShake();
        origin.GetComponent<Rigidbody2D>().isKinematic = false;
        origin.TransitionToCombatState(new CombatIdle(), null);
    }
}

public class CombatIdle : CombatState
{
    public override void EnterState(CombatScript player, List<AttackNode.FollowUpAttack> attackList)
    {
        origin = player;
        possibleAttacks = attackList;
    }
    public override void UpdateState()
    {


        foreach(AttackNode.FollowUpAttack followUp in possibleAttacks)
        {
            if (followUp.input.secondary != CombatInput.SecondaryInput.None)
            {
                if (Input.GetKeyDown(Constants.attackInput) && Input.GetKey(CombatInput.ConvertToKeyCode(followUp.input.secondary, origin.GetFacingRight())))
                {
                    //origin.TransitionToCombatState(new CombatInAttack(), followUp.attack);
                }
            }
            else if(Input.GetKeyDown(Constants.attackInput))
            {
                //origin.TransitionToCombatState(new CombatInAttack(), followUp.attack);
            }
        }
    }
    public override void ExitState()
    {

    }
}

public class CombatInAttack : CombatState
{
    private AttackNode curNode;
    private AttackState curState;

    public override void EnterState(CombatScript player, List<AttackNode.FollowUpAttack> attackList)
    {
        player.GetComponent<Rigidbody2D>().isKinematic = true;

        origin = player;
        possibleAttacks = attackList;

        //if(curNode.type == AttackNode.AttackType.Instant) TransitionToAttackState(new AttackWindup());
        //if(curNode.type == AttackNode.AttackType.Charge) TransitionToAttackState(new AttackCharge());
    }
    public override void UpdateState()
    {
        curState.UpdateState();
    }
    public override void ExitState()
    {

    }
    public void TransitionToAttackState(AttackState newState)
    {
        curState?.ExitState();
        curState = newState;

        curState.EnterState(this, origin, curNode);
    }

    public void FinishAttack(AttackNode followUp)
    {
        if (followUp != null) origin.TransitionToCombatState(new CombatInAttack(), followUp);

        ReturnToIdle();
    }

    // This is for Follow Up attacks, if you transition to CombatState Attack and give an AttackNode as the second argument, it will automatically play the Attack without Input needed
    public void ExecuteAttackNode(AttackNode attackNode)
    {
        curNode = attackNode;
    }
}

public class CombatRecovering : CombatState
{
    public override void EnterState(CombatScript player, List<AttackNode.FollowUpAttack> attackList)
    {
        origin = player;
        possibleAttacks = attackList;
    }
    public override void UpdateState()
    {

    }
    public override void ExitState()
    {

    }
}