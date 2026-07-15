using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    [SerializeField] GameObject Menu;
    [SerializeField] GameObject UpgradeOptionButtonPrefab;
    [SerializeField] HappyGnome target;
    [SerializeField] GameObject[] GnomeTypes;
    void Awake()
    {
        instance = this;
    }
    public void ToggleUpgradeMenu(bool toggle)
    {
        Menu.SetActive(toggle);
    }
    public void PopulateUpgradeOptionsForGnome(HappyGnome HG)
    {
        target = HG;
        Debug.Log(HG.name);
    }
    public void PlayerSelectedOptionFromUpgradeOption(int type)
    {
        //Turn into this type of tower
        Debug.Log(type);
    }
}
