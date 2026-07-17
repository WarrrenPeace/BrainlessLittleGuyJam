using UnityEngine;

public class GnomeStats : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject learningIcon;
    void Start()
    {
        transform.parent = GameObject.FindGameObjectWithTag("WorldCanvas").transform;
    }
    public void SetTarget(Transform t)
    {
        target = t;
    }
    void LateUpdate()
    {
        if(target)
        {
            transform.position = target.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ShowLearningIcon()
    {
        learningIcon.SetActive(true);
    }
}
