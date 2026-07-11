using UnityEngine;

public class EnemyHealth : Health
{
    void Start()
    {
        HealthSetup();
        HP = MaxHP;
    }
}
