using System;
using Unity.Mathematics;
using UnityEngine;

public class HappyGnome : MonoBehaviour
{
    [SerializeField] bool LearingSpell = false;
    [SerializeField] bool hasBeenUpgraded = false;
    [SerializeField] bool hasFreeUpgrade = false;
    [SerializeField] int wavesLeftForResearch;
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

        
        if(index == 0) //Need to change to some kind of Indexer? Ill have a dictionary with all types and how many turns it takes to learn
        {
            Debug.Log("Learning Fireball");
            wavesLeftForResearch = 1;
        }
        else if(index == 1)
        {
            Debug.Log("Learning Cleric");
            wavesLeftForResearch = 3;
        }
        else if(index == 2)
        {
            Debug.Log("Learning Druid");
            wavesLeftForResearch = 2;
        }
        else
        {
            wavesLeftForResearch = 2;
        }

        LearingSpell = true;
        GS.UpdateLearningTimeLeft(wavesLeftForResearch);
    }
    public void CheckResearchProgress() //Called after a wave is cleared
    {
        
        if(LearingSpell)
        {
            Debug.Log(name + " " + wavesLeftForResearch + " left");

            wavesLeftForResearch -= 1;
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
        //Debug.Log(name + " Is upgrading to " + indexOfSpellToLearn);
    }
}
