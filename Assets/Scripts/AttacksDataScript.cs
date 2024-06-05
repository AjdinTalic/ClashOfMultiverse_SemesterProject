using System.Collections.Generic;
using UnityEngine;

public class AttacksDataScript : MonoBehaviour
{
    public Dictionary<string,  AttackStats> attacks;

    public AttacksDataScript()
    {
        attacks = new Dictionary<string, AttackStats>()
        {
            {"KayoLightAttack", new AttackStats(300, 0, false, "lightAttackHit", "lightBlockHit") },
            {"KayoMediumAttack", new AttackStats(600, 0, false, "mediumAttackHit", "mediumBlockHit")},
            {"KayoHeavyAttack", new AttackStats(800, 0, false, "heavyAttackHit", "heavyBlockHit")},
            {"KayoCrouchLightAttack", new AttackStats(300, 0, true, "lightAttackHit", "lightBlockHit")},
            {"KayoCrouchMediumAttack", new AttackStats(600, 0, true, "mediumAttackHit", "mediumBlockHit")},
            {"KayoCrouchHeavyAttack", new AttackStats(800, 0, true, "heavyAttackHit", "heavyBlockHit")},
            {"KayoPerfectParryAttack", new AttackStats(1000, 0, false, "ppAttackHit", "ppBlockHit")}
            
        };
    }
}
