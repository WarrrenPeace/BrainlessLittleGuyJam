using Unity.Mathematics;
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
    void Start()
    {
        //Instantiate(GnomeTypes[indes]);
    }
    public void ToggleUpgradeMenu(bool toggle)
    {
        Menu.SetActive(toggle);
    }
    public void PopulateUpgradeOptionsForGnome(HappyGnome HG)
    {
        target = HG;
    }
    public void PlayerSelectedOptionFromUpgradeOption(int type)
    {
        //Turn into this type of tower
        target.StartToResearch(type);
        ToggleUpgradeMenu(false);
    }
    public void UpgradeGnome(int index, HappyGnome gnome)
    {
        Instantiate(GnomeTypes[index],gnome.transform.position,quaternion.identity,null);
    }
}
