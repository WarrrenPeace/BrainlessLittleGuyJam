using UnityEngine;

public class DestroyInTime : MonoBehaviour
{
    [SerializeField] float timer;
    void Start()
    {
        Destroy(gameObject,timer);
    }
}
