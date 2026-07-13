using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Commander : MonoBehaviour
{
    private Camera cam;
    public LayerMask commandLayerMask;

    Collider2D target;
    [Header("Followers")]
    [SerializeField] private GameObject FollowerSlotPrefab;
    [SerializeField] int MaxFollowers = 1; 
    [SerializeField] int followers = 0;
    [SerializeField] float radius = 1f;
    [SerializeField] List<Transform> FollowPoints; //EMPTY objects that followers track
    [SerializeField] List<Transform> Followers; //Gameobject of followers for info

    [Header("AudioFX")]private AudioSource AS;
    [SerializeField] private AudioClip orderLittleGuy;
    
    void Awake()
    {
        cam = Camera.main;
        
    }
    void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    void Update ()
	{
		var currentMouse = Mouse.current;
		var worldPos = cam.ScreenToWorldPoint (currentMouse.position.ReadValue()); // I dont want to find camera every frame, so ill get ref in awake

		if (currentMouse.leftButton.wasPressedThisFrame)
		{
			var collider = Physics2D.OverlapPoint (worldPos, commandLayerMask);
            target = collider;
			if (!collider)
            {
                target = null;
                return;
            }
		}
		else if (currentMouse.leftButton.wasReleasedThisFrame)
		{
            if(target)
            {
                if(target.tag == "Player")
                {
                    
                }
                else
                DetermineEligibleTower(target.transform);
                //Might return in method above if it fails, so this below wont run
                AS.PlayOneShot(orderLittleGuy,PlayerPrefs.GetFloat("FXVolume",1));
                GetComponent<PlayerMovement>()?.StopToInteract(target.transform.position);
            }
			return;
		}
	}
    void DetermineEligibleTower(Transform targetTower)
    {
        if(targetTower.GetComponent<LittleGuyMovement>().IsFollower())
        {
            RemoveTowerAsFollowerSlot(targetTower);
        }
        else
        {
            if(Followers.Count + 1 <= MaxFollowers)
            {
                AssignTowerToFollowerSlot(targetTower);
            }
            else
            {
                //Tell tower to move to your location instead
                targetTower.GetComponent<LittleGuyMovement>()?.SetTargetLocation(transform.position);
            }
        }
        UpdateCircleFormation(); //Update circle when removing or adding followers
    }
    void AssignTowerToFollowerSlot(Transform targetTower)
    {
        //Add new transform then assign tower to it
        Transform temp = Instantiate(FollowerSlotPrefab,transform).transform;
        FollowPoints.Add(temp);
        targetTower.GetComponent<LittleGuyMovement>()?.SetFollower(temp);
        Followers.Add(targetTower);
    }
    void RemoveTowerAsFollowerSlot(Transform targetTower)
    {
        //Tower no longer follows empty
        FollowPoints[Followers.IndexOf(targetTower)].transform.parent = null;
        FollowPoints.Remove(FollowPoints[Followers.IndexOf(targetTower)]);
        Followers.Remove(targetTower);
        targetTower.GetComponent<LittleGuyMovement>()?.SetFollower(null);
        targetTower.GetComponent<LittleGuyMovement>()?.SetTargetLocation(targetTower.transform.position);
    }




    public void UpdateCircleFormation()
    {
        FollowPoints.RemoveAll(item => item == null);
        float angleStep = (2 * Mathf.PI) / FollowPoints.Count;

        for (int i = 0; i < FollowPoints.Count; i++)
        {
            float angle = i * angleStep + 55;
            
            // Calculate position in local space (relative to player)
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            FollowPoints[i].localPosition = new Vector3(x, y, 0);
        }
    }
}
