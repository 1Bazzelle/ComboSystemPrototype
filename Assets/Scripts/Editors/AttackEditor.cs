using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Windows;
using UnityEngine.U2D;

[CustomPropertyDrawer(typeof(AttackNode))]
public class AttackNodeDrawer : PropertyDrawer
{
    private const float lineHeight = 18f;
    private const float spacing = 4f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var typeProperty = property.FindPropertyRelative("type");

        // Draw the foldout
        property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, lineHeight), property.isExpanded, label, true);

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            float yOffset = position.y + lineHeight + spacing;

            // Node Type Field
            EditorGUI.PropertyField(new Rect(position.x, yOffset, position.width, lineHeight), typeProperty, new GUIContent("Node Type"));
            yOffset += lineHeight + spacing;

            // Draw different fields based on the Node Type
            switch ((AttackNode.Type)typeProperty.enumValueIndex)
            {
                case AttackNode.Type.SpriteChange:
                    DrawField(ref yOffset, position, property, "sprite", "Sprite");
                    break;

                case AttackNode.Type.Wait:
                    DrawField(ref yOffset, position, property, "waitTime", "Wait Time");
                    break;

                case AttackNode.Type.Charge:
                    DrawField(ref yOffset, position, property, "chargeTime", "Charge Time");
                    DrawField(ref yOffset, position, property, "shakeWhileCharge", "Shake during Charge?");
                    DrawField(ref yOffset, position, property, "shakeIntensity", "Shake Intensity");
                    break;

                case AttackNode.Type.Attack:
                    DrawField(ref yOffset, position, property, "hitboxes", "Hitboxes");
                    DrawField(ref yOffset, position, property, "damage", "Damage");
                    DrawField(ref yOffset, position, property, "element", "Element");
                    DrawField(ref yOffset, position, property, "freezeFrameTime", "Freeze Frame Time");
                    DrawField(ref yOffset, position, property, "freezeFrameSprite", "Freeze Frame Sprite");
                    DrawField(ref yOffset, position, property, "strikeSprite", "Strike Sprite");
                    DrawField(ref yOffset, position, property, "strikeSpritePos", "Strike Sprite Position");
                    DrawField(ref yOffset, position, property, "strikeSpriteLifetime", "Strike Sprite Lifetime");
                    break;

                case AttackNode.Type.ApplyKnockback:
                    DrawField(ref yOffset, position, property, "forceDir", "Force Direction");
                    DrawField(ref yOffset, position, property, "forceIntensity", "Knockback Intensity");
                    break;

                case AttackNode.Type.MoveSelf:
                    DrawField(ref yOffset, position, property, "moveInterpolation", "Move Interpolation");
                    DrawField(ref yOffset, position, property, "moveInterpolationAmplifier", "Move Interpolation Amplifier");
                    DrawField(ref yOffset, position, property, "moveOffset", "Move Offset");
                    break;

                case AttackNode.Type.MoveEnemies:
                    DrawField(ref yOffset, position, property, "enemyMoveHitboxes", "Enemy Move Hitboxes");
                    DrawField(ref yOffset, position, property, "enemyMoveInterpolation", "Enemy Move Interpolation");
                    DrawField(ref yOffset, position, property, "enemyMoveInterpolationAmplifier", "Enemy Move Interpolation Amplifier");
                    DrawField(ref yOffset, position, property, "enemyMoveOffset", "Enemy Move Offset");
                    break;

                case AttackNode.Type.EndAttack:
                    DrawField(ref yOffset, position, property, "followUpAttacks", "Follow Up Attacks");
                    break;
            }

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
        {
            return lineHeight + spacing;
        }

        float totalHeight = lineHeight + spacing;

        var typeProperty = property.FindPropertyRelative("type");

        switch ((AttackNode.Type)typeProperty.enumValueIndex)
        {
            case AttackNode.Type.SpriteChange:
                totalHeight += lineHeight + spacing;  // for sprite field
                break;

            case AttackNode.Type.Wait:
                totalHeight += lineHeight + spacing;  // for waitTime field
                break;

            case AttackNode.Type.Charge:
                totalHeight += lineHeight * 3 + spacing * 2;  // chargeTime, shakeWhileCharge, shakeIntensity
                break;

            case AttackNode.Type.Attack:
                totalHeight += lineHeight * 8 + spacing * 7;  // all fields related to Attack

                var hitboxesProperty = property.FindPropertyRelative("hitboxes");
                if (hitboxesProperty.isExpanded)
                {
                    totalHeight += EditorGUI.GetPropertyHeight(hitboxesProperty, true) + spacing;
                }
                break;

            case AttackNode.Type.ApplyKnockback:
                totalHeight += lineHeight * 2 + spacing;  // forceDir, forceIntensity
                break;

            case AttackNode.Type.MoveSelf:
                totalHeight += lineHeight * 3 + spacing * 2;  // moveInterpolation, moveInterpolationAmplifier, moveOffset
                break;

            case AttackNode.Type.MoveEnemies:
                totalHeight += lineHeight * 4 + spacing * 3;  // moveAttackBehaviour, enemyMoveInterpolation, etc.

                var enemyMoveHitboxesProperty = property.FindPropertyRelative("enemyMoveHitboxes");
                if (enemyMoveHitboxesProperty.isExpanded)
                {
                    totalHeight += EditorGUI.GetPropertyHeight(enemyMoveHitboxesProperty, true) + spacing;
                }
                break;

            case AttackNode.Type.EndAttack:
                totalHeight += lineHeight + spacing;  // for followUpAttacks field

                // Adding the height for the FollowUpAttacks list
                var followUpAttacksProperty = property.FindPropertyRelative("followUpAttacks");
                if (followUpAttacksProperty.isExpanded)
                {
                    totalHeight += EditorGUI.GetPropertyHeight(followUpAttacksProperty, true) + spacing;
                }
                break;
        }

        return totalHeight + lineHeight + spacing; // Add extra space for the last field
    }

    private void DrawField(ref float yOffset, Rect position, SerializedProperty property, string fieldName, string label)
    {
        var fieldProperty = property.FindPropertyRelative(fieldName);
        var fieldHeight = EditorGUI.GetPropertyHeight(fieldProperty);

        EditorGUI.PropertyField(new Rect(position.x, yOffset, position.width, fieldHeight), fieldProperty, new GUIContent(label));
        yOffset += fieldHeight + spacing;
    }
}