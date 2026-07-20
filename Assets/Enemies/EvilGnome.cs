using UnityEngine;

public class EvilGnome : MonoBehaviour
{
    bool IsInVines = false;
    public void DruidAttackStopsMovement(float duration)
    {
        if(IsInVines) {CancelInvoke("DruidAttackCooldown");}

        GetComponent<MeleeEnemy>().EntangleInVines();
        IsInVines = true;

        Invoke("DruidAttackCooldown",duration);
    }
    void DruidAttackCooldown()
    {
        GetComponent<MeleeEnemy>().ReleaseVines();
        IsInVines = false;
    }
    public bool QueryVines()
    {
        return IsInVines;
    }
}
