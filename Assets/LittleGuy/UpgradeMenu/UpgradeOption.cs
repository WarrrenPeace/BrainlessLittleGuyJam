using UnityEngine;

public class UpgradeOption : MonoBehaviour
{
    [SerializeField] int upgradeID;
    public void OnSelectUpgrade()
    {
        UpgradeManager.instance.PlayerSelectedOptionFromUpgradeOption();
    }
}
