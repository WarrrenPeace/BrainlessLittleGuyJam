using UnityEngine;

public class LittleGuyHealth : Health
{
    void Start()
    {
        HealthSetup();
        HP = MaxHP;
    }
}
