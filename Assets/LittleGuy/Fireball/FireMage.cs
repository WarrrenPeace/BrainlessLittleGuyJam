using Unity.VisualScripting;
using UnityEngine;

public class FireMage : MonoBehaviour
{
    public enum State {Idle, Attack}
    public State state;
    SpriteRenderer SR;
    Animator AM;   

    [SerializeField] float attackCooldown = 1.5f;
    [SerializeField] int attackDamage = 5;
    [SerializeField] float animationAttackDelay = 0.6f;

    [Header("Targeting")]
    [SerializeField] private Transform target;    
    [SerializeField] private bool targetInRange = false;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float detectionRadius = 1;
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        AM = GetComponent<Animator>();
        InvokeRepeating("AutoFindTarget",0.5f,1);
    }
    void ChangeState(State stateToChangeTo)
    {
        state = stateToChangeTo;
    }
    void AutoFindTarget()
    {
        if(!target)
        {
            SetTarget(FindTarget());
        }
        
    }
    public void SetTarget(Transform targetTransform)
    {
        if(!targetInRange) {ChangeState(State.Idle);}
        target = targetTransform;
    }
    Transform FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position,detectionRadius,targetLayer);
        float closestDistance = detectionRadius;
        Transform tempTarget = null;

        foreach (Collider2D hit in hits)
        {
            if(Vector2.Distance(transform.position,hit.transform.position) < closestDistance)
            {
                //New closer Target
                closestDistance = Vector2.Distance(transform.position,hit.transform.position);
                tempTarget = hit.transform;
            }
        }

        
        return tempTarget;
    }

    void Update()
    {
        DistanceToTargetCheck();
        switch (state)
        {
            case State.Idle: //When enemy spawns in
                //Idle();
                break;
            case State.Attack: //Enemy running
                Attack();
                break;
        }
        CooldownTimer();
        //Animate();
    }
    void DistanceToTargetCheck()
    {
        if(!target)
        {
            if(state != State.Idle) {ChangeState(State.Idle);}
            if(targetInRange) {targetInRange = false;}
            return;
        }
        
        if(Vector2.Distance(transform.position, target.position) > detectionRadius)
        {
            //Enenmy not in range for attack
            if(targetInRange)
            {
                targetInRange = false;
                ChangeState(State.Idle);
            }
        }
        else 
        {
            //Enemy is close to target
            if(!targetInRange)
            {
                targetInRange = true;
                ChangeState(State.Attack);
            }
        }
    }
    void Attack()
    {
        
        if(attackCooldown > 0)
        {
            
        }
        else
        {
            attackCooldown = 1.5f;
            AM.SetTrigger("Attack");
            Invoke("AnimationAttackDelay",animationAttackDelay);
            
        }
    }
    void AnimationAttackDelay()
    {
        if(target)
            target.GetComponent<Health>()?.ApplyDamage(attackDamage);
    }
    void CooldownTimer()
    {
        if(attackCooldown > 0)
        {
            attackCooldown -= 1 * Time.deltaTime;
        }
        else
        {
            
        }
    }
}
