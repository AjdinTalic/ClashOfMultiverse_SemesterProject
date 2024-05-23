using System.Collections.Generic;
using UnityEngine;

public class AttacksDataScript : MonoBehaviour
{
    public Dictionary<string,  AttackStats> attacks;

    public AttacksDataScript()
    {
        attacks = new Dictionary<string, AttackStats>()
        {
            {"KayoLightAttack", new AttackStats(300, 0, false) },
            {"KayoMediumAttack", new AttackStats(600, 0, false)},
            {"KayoHeavyAttack", new AttackStats(800, 0, false)},
            {"KayoCrouchLightAttack", new AttackStats(300, 0, true)},
            {"KayoCrouchMediumAttack", new AttackStats(600, 0, true)},
            {"KayoCrouchHeavyAttack", new AttackStats(800, 0, true)}
            
        };
    }
}
