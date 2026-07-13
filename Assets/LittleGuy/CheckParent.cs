using UnityEngine;

public class CheckParent : MonoBehaviour
{
    void Update()
    {
        if(transform.parent == null)
        {
            Destroy(gameObject);
        }
    }
}
