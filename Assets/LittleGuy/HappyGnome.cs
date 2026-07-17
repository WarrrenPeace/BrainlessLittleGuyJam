using System;
using Unity.Mathematics;
using UnityEngine;

public class HappyGnome : MonoBehaviour
{
    [SerializeField] bool hasBeenUpgraded = false;
    [SerializeField] bool hasFreeUpgrade = false;
    int wavesLeftForResearch;
    [SerializeField] private int indexOfSpellToLearn;
    [SerializeField] GameObject GnomeStatsPrefab;
    [SerializeField] GnomeStats GS;
    void OnEnable()
    {
        WaveManager.waveEnded  += CheckResearchProgress;
    }
    void OnDisable()
    {
        WaveManager.waveEnded  -= CheckResearchProgress;
    }
    void Start()
    {
        GS = Instantiate(GnomeStatsPrefab,transform.position,quaternion.identity,null).GetComponent<GnomeStats>();
        GS.SetTarget(transform);
    }
    public bool HasThisGnomeLearnedASpell() //?
    {
        return hasBeenUpgraded;
    }
    public void StartToResearch(int index) //Called on Gnome when clicked
    {
        //Player picked an upgrade, this gnome will convert into new gnome after set amount of turns
        GS.ShowLearningIcon();
        indexOfSpellToLearn = index;
        if(hasFreeUpgrade)
        {
            //Skip progress and instantly upgrade
            UpgradeGnome();
        }

        
        if(index == 0)
        {
            Debug.Log("Learing Fireball");
            wavesLeftForResearch = 1;
        }
        if(index == 1)
        {
            Debug.Log("Learing Heal");

            wavesLeftForResearch = 3;
        }
        else
        {
            wavesLeftForResearch = 2;
        }
    }
    public void CheckResearchProgress() //Called after a wave is cleared
    {
        if(!hasBeenUpgraded)
        {
            wavesLeftForResearch --;
            if(wavesLeftForResearch <= 0)
            {
                UpgradeGnome();
            }
        }
        
    }
    void UpgradeGnome()
    {
        //Gnome has learned the spell!
        //Instatiate effect
        //Destroy tower
        Destroy(gameObject);
        //Tell upgrademanager to spawn new tower at this location with index
        UpgradeManager.instance.UpgradeGnome(indexOfSpellToLearn,this);
        Debug.Log(name + " Is upgrading to " + indexOfSpellToLearn);
    }
}
