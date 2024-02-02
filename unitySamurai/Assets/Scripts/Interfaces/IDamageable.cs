using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
   public void TakeDamage(DamageInfo damageInfo);
}

public class DamageInfo
{
    public GameObject initiator;
    public float damage;

    public DamageInfo(GameObject initiator, float damage)
    {
        this.initiator = initiator;
        this.damage = damage;
    }
}