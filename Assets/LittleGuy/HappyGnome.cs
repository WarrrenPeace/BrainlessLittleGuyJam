using UnityEngine;

public class HappyGnome : MonoBehaviour
{
    [SerializeField] bool hasBeenUpgraded = false;
    int wavesLeftForResearch;

    public bool HasThisGnomeLearnedASpell() //?
    {
        return hasBeenUpgraded;
    }
    public void StartToResearch()
    {
        //Called on Gnome when clicked
        //Player picked an upgrade, this gnome will convert into new gnome after set amount of turns
        //wavesLeftForResearch = amount
    }
    public void CheckResearchProgress()
    {
        
    }
}
