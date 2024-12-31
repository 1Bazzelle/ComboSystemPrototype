using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.Intrinsics;
using UnityEngine;

using static UnityEngine.Rendering.DebugUI;

public class CombatScript : MonoBehaviour
{
    // Later make this just a weapon, the weapon will contain its list of possible AttackNode trees
    [SerializeField] private List<AttackNode.FollowUpAttack> comboStarters;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite playerSprite;

    [SerializeField] private AttackEvent playerAttack;

    private SpriteFlash spriteFlash;

    readonly float gizmoPointRadiusStandard = 0.1f;

    private CombatState pState;

    private bool facingRight;

    private void OnEnable()
    {
        ResetSprite();

        spriteFlash = GetComponent<SpriteFlash>();

        TransitionToCombatState(new CombatIdle(), null);

        facingRight = true;
    }
    private void Update()
    {
        pState.UpdateState();
    }

    public void TransitionToCombatState(CombatState newState, AttackNode attack)
    {
        pState = newState;

        if (pState is CombatInAttack && attack != null)
        {
            CombatInAttack state = pState as CombatInAttack;
            state.ExecuteAttackNode(attack);
        }

        pState.EnterState(this, comboStarters);
    }

    public bool GetFacingRight()
    {
        return facingRight;
    }

    public void SetSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }
    public void ResetSprite()
    {
        spriteRenderer.sprite = playerSprite;
    }

    public void PlayerAttacked(AttackNode attack)
    {
        playerAttack.Occured(attack);
    }

    public void Shake(float intensity)
    {
        Vector2 offset = new(transform.position.x + Random.Range(0, intensity), transform.position.y + Random.Range(0, intensity)/2);

        spriteRenderer.transform.position = offset;
    }
    public void UnShake()
    {
        spriteRenderer.transform.position = transform.position;
    }

    public void QueueFlash(float dur, Color flashCol)
    {
        spriteFlash.QueueFlash(dur, flashCol);
    }

    public void UnFlash()
    {
        spriteFlash.UnFlash();
    }

    private void OnDrawGizmos()
    {
        if (comboStarters == null) return;

        Vector2 playerPos = new(transform.position.x, transform.position.y);

        foreach (AttackNode.FollowUpAttack followUp in comboStarters)
        {
            if (followUp.showGizmos == false) return;

            Attack attack = followUp.attack;

            // Visualization Logic for an Attack here from here

            foreach(AttackNode node in attack.attackNodes)
            {
                switch (node.type)
                {
                    case AttackNode.Type.SpriteChange:
                        break;
                    case AttackNode.Type.Wait:
                        break;
                    case AttackNode.Type.Charge:
                        break;
                    case AttackNode.Type.Attack:

                        Gizmos.color = Color.red;
                        foreach (Hitbox hitbox in node.hitboxes)
                        {
                            Vector2 boxCenter = (Vector2)transform.position + hitbox.offset;
                            Gizmos.DrawWireCube(boxCenter, hitbox.boxSize);
                        }

                        Gizmos.color = Color.cyan;
                        Gizmos.DrawWireSphere(playerPos + node.strikeSpritePos, gizmoPointRadiusStandard);

                        break;
                    case AttackNode.Type.ApplyKnockback:

                        Gizmos.color = Color.magenta;
                        Vector2 knockbackVector = node.forceDir.normalized;
                        Gizmos.DrawWireSphere(playerPos + new Vector2(2, 1), gizmoPointRadiusStandard);
                        Gizmos.DrawLine(playerPos + new Vector2(2, 1), playerPos + new Vector2(2, 1) + knockbackVector * node.forceIntensity);

                        break;
                    case AttackNode.Type.MoveSelf:

                        Gizmos.color = Color.blue;

                        Gizmos.DrawWireSphere(playerPos + node.moveOffset, gizmoPointRadiusStandard);
                        Gizmos.DrawLine(playerPos, playerPos + node.moveOffset);

                        break;
                    case AttackNode.Type.MoveEnemies:

                        Gizmos.color = Color.green;

                        foreach (Hitbox hitbox in node.enemyMoveHitboxes)
                        {
                            Vector2 boxCenter = (Vector2)transform.position + hitbox.offset;
                            Gizmos.DrawWireCube(boxCenter, hitbox.boxSize);
                        }

                        Gizmos.DrawWireSphere(playerPos + node.enemyMoveOffset, gizmoPointRadiusStandard);

                        break;
                    case AttackNode.Type.EndAttack:

                        break;
                }
            }
            // to here
        }
    }
}
