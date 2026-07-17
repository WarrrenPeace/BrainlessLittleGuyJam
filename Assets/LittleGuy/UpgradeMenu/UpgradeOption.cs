using UnityEngine;

public class UpgradeOption : MonoBehaviour
{
    public void OnSelectUpgrade(int type)
    {
        UpgradeManager.instance.PlayerSelectedOptionFromUpgradeOption(type);
    }
}
