using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewAttack", menuName = "Attack")]
public class Attack : ScriptableObject
{
    public List<AttackNode> attackNodes;
}