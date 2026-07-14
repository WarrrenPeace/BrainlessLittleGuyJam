using UnityEngine;

public class LittleGuyHealth : Health
{
    void Start()
    {
        HealthSetup();
        HP = MaxHP;
    }
    public override void OnDeath()
    {
        if(GetComponent<LittleGuyMovement>().IsFollower())
        {
            //Tell commander to remove empty follow object
            Commander.instance.ThisGnomeDied(transform);
        }
    }
}
