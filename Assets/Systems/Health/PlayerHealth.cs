using UnityEngine;

public class PlayerHealth : Health
{

    void Start()
    {
        HealthSetup();
        HP = MaxHP;
    }
    public override void OnDeath()
    {
        //End the run
        // Prompt player with death screen
        GameManager.instance.PlayerDied();
    }
}
