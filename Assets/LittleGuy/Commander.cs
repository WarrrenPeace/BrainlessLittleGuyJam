using UnityEngine;
using UnityEngine.InputSystem;

public class Commander : MonoBehaviour
{
    private Camera cam;
    public LayerMask commandLayerMask;

    Collider2D target;

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
                target.GetComponent<LittleGuyMovement>()?.SetTargetLocation(transform.position);
                AS.PlayOneShot(orderLittleGuy,PlayerPrefs.GetFloat("FXVolume",1));
                GetComponent<PlayerMovement>()?.StopToInteract(target.transform.position);
            }
			return;
		}
	}
}
