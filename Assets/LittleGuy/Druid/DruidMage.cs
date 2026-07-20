using Unity.VisualScripting;
using UnityEngine;

public class DruidMage : MonoBehaviour
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

    [SerializeField] float vineEffectCooldown = 3.1f;
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
        float closestDistance = detectionRadius;
        Transform tempTarget = null;

        foreach (Collider2D hit in hits)
        {
            if(Vector2.Distance(transform.position,hit.transform.position) < closestDistance)
            {
                if(hit.GetComponent<EvilGnome>().QueryVines())
                {
                    continue; //Skip because gnome is already entangled
                }
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
            target.GetComponent<Health>()?.ApplyDamage(attackDamage);
            if(FX) {Instantiate(FX,target.position,Quaternion.identity,target);} //Vines are chilren of target!

            //Druids apply vines to their target, stopping their movement for a duration of seconds
            target.GetComponent<EvilGnome>().DruidAttackStopsMovement(vineEffectCooldown);
            target = null;
            //All gnomes have movement and attack logic in one script, when i add new gnome types ill need to fix this logic for druid
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
