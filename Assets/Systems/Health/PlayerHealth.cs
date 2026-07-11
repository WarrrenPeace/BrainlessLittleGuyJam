using UnityEngine;

public class PlayerHealth : Health
{

    void Start()
    {
        HealthSetup();
        HP = MaxHP;
    }
    void OnPlayerDeath()
    {
        //End the run
        // Prompt player with death screen
    }
}
