using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ClericMage : MonoBehaviour
{
    public enum State {Idle, Attack}
    public State state;
    SpriteRenderer SR;
    Animator AM;   
    LittleGuyMovement LGM;

    [SerializeField] float setAttackCooldown = 1.5f;
    [SerializeField] float attackCooldown = 1.5f;
    [SerializeField] int attackDamage = 5;
    [SerializeField] float animationAttackDelay = 0.6f;

    [Header("Targeting")]
    [SerializeField] private Transform target;    
    [SerializeField] private bool targetInRange = false;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float detectionRadius = 1;

    [Header("Effects")]
    [SerializeField] GameObject FX;
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        AM = GetComponent<Animator>();
        LGM = GetComponent<LittleGuyMovement>();
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
        else
        {
            CheckTargetIsntFarAway();
        }
        
    }
    void CheckTargetIsntFarAway() //Because of course i have to
    {
        if(Vector2.Distance(transform.position,target.position) >= detectionRadius)
        {
            target = null;
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
        int lowestHealth = 1000;
        Transform tempTarget = null;

        foreach (Collider2D hit in hits)
        {
            if(hit.gameObject == gameObject)
            {
                continue;
            }
            if(hit.GetComponent<Health>().QueryFullHealth()) {continue;}
            else
            {
                if(hit.GetComponent<Health>()?.QueryHealthValue() < lowestHealth)
                {
                    lowestHealth = hit.GetComponent<Health>().QueryHealthValue();
                    tempTarget = hit.transform;
                }
            }
            //if(Vector2.Distance(transform.position,hit.transform.position) < closestDistance)
            //{
            //    //New closer Target
            //    closestDistance = Vector2.Distance(transform.position,hit.transform.position);
            //    tempTarget = hit.transform;
            //}
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
            if(!target)
            {
                SetTarget(FindTarget()); //quickly find another target if current target dies
            }
            attackCooldown = setAttackCooldown;
            LGM?.RotateTowardsTarget(target.position);
            Invoke("AnimationAttackDelay",animationAttackDelay);
        }
    }
    void AnimationAttackDelay()
    {
        if(target)
        {
            target.GetComponent<Health>()?.HealDamage(attackDamage);
            if(FX) {Instantiate(FX,target.position,quaternion.identity,null);}

            if(target.GetComponent<Health>().QueryFullHealth()) { target = null;}
        }   
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
