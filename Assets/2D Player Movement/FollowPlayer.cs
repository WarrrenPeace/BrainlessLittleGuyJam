using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    void LateUpdate()
    {
        if(player)
        {
            transform.position = player.position;
        }
        
    }
}
