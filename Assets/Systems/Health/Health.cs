using System.Security.Principal;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Damage_VFX))]
public class Health : MonoBehaviour
{
    public SpriteRenderer SR;
    public Damage_VFX VFX;
    public int MaxHP, HP;
    public GameObject deathFX;
    public void HealthSetup()
    {
        SR = GetComponent<SpriteRenderer>();
        VFX = GetComponent<Damage_VFX>();
    }
    public void ApplyDamage(int amount)
    {
        if(HP - amount > 0)
        {
            HP -= amount;
        }
        else
        {
            HP -= amount;
            Death();
            return;
        }
        VFX.PlayOnDamageVFX();
        OnApplyDamage();
    }
    public virtual void OnApplyDamage()
    {
        
    }
    public void HealDamage(int amount)
    {
        if(HP + amount <= MaxHP)
        {
            HP += amount;
        }
        else
        {
            HP = MaxHP;
        }
        OnHealDamage();
    }
    public virtual void OnHealDamage()
    {
        
    }
    public virtual void Death()
    {
        //Spawn effect on death
        Instantiate(deathFX,transform.position,quaternion.identity,null);
        OnDeath();
        Destroy(gameObject);
    }
    public virtual void OnDeath()
    {
        
    }
}
