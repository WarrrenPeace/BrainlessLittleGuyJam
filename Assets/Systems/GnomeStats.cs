using TMPro;
using UnityEngine;

public class GnomeStats : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject learningIcon;
    [SerializeField] TextMeshProUGUI learningCount;
    void Start()
    {
        transform.SetParent(GameObject.FindGameObjectWithTag("WorldCanvas").transform);
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
    public void UpdateLearningTimeLeft(int amount)
    {
        if(learningCount)
        {
            learningCount.text = amount.ToString();
        }
        
    }
}
