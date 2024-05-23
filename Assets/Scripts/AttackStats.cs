using UnityEngine;

public class AttackStats : MonoBehaviour
{
    public float damage;
    public float knockback;
    public bool crouchAttack;

    public AttackStats(float damage, float knockback, bool crouchAttack)
    {
        this.damage = damage;
        this.knockback = knockback;
        this.crouchAttack = crouchAttack;
    }
}
