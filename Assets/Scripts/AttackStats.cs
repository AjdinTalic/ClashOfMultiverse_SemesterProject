using UnityEngine;

public class AttackStats : MonoBehaviour
{
    public float damage;
    public float knockback;
    public bool crouchAttack;
    public string attackRecovery;
    public string blockRecovery;

    public AttackStats(float damage, float knockback, bool crouchAttack, string attackRecovery, string blockRecovery)
    {
        this.damage = damage;
        this.knockback = knockback;
        this.crouchAttack = crouchAttack;
        this.attackRecovery = attackRecovery;
        this.blockRecovery = blockRecovery;
    }
}
