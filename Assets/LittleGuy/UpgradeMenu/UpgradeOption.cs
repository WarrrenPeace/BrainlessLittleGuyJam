using UnityEngine;

public class UpgradeOption : MonoBehaviour
{
    [SerializeField] int upgradeID;
    public void OnSelectUpgrade(int type)
    {
        UpgradeManager.instance.PlayerSelectedOptionFromUpgradeOption(type);
    }
}
