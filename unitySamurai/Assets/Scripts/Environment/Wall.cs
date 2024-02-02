using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IDamageable
{
    public Transform temp_damagedWallPrefab;

    public void TakeDamage(DamageInfo damageInfo)
    {
        Debug.Log("DAMAGED");
        Instantiate(temp_damagedWallPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
