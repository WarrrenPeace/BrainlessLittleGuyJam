using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    public enum State {Idle, Run, WindUp, Attack}
    public State state;
    Vector2 moveDir;
    Vector2 lastMoveDirection; 
    Rigidbody2D RB;
    SpriteRenderer SR;
    Animator AM;    
    
    [Header("Stats")]
    [SerializeField] float movementSpeed = 10;
    [SerializeField] float desiredDistanceToTarget = 0.25f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] int attackDamage = 5;
    [SerializeField] float animationAttackDelay = 0.6f;

    [Header("Targeting")]
    [SerializeField] private Transform target;    
    private bool targetInRange = false;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float detectionRadius = 1;
    public bool targetPlayerInstantly = false;
    bool canMove = true;


    
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        AM = GetComponent<Animator>();
        if(targetPlayerInstantly) {SetTarget(GameObject.FindGameObjectWithTag("Player").transform);} //Bug if player is dead
        InvokeRepeating("AutoFindTarget",0.5f,0.4f);
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
        if(!targetInRange) {ChangeState(State.Run);}
        target = targetTransform;
    }
    void ChangeState(State stateToChangeTo)
    {
        state = stateToChangeTo;
    }
    void Update()
    {
        DistanceToTargetCheck();
        switch (state)
        {
            case State.Idle: //When enemy spawns in
                Idle();
                break;
            case State.Run: //Enemy running
                Move();
                break;
            case State.WindUp: //Enemy Winding up attack, lead to attack
                WindUp();
                break;
            case State.Attack: //Enemy begins to attack
                Attack();
                break;
        }
        CooldownTimer();
        Animate();
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
    void Idle()
    {
        
    }
    void DistanceToTargetCheck()
    {
        if(!target)
        {
            if(state != State.Idle) {StopMovement();}
            
            return;
        }
        
        if(Vector2.Distance(transform.position, target.position) > desiredDistanceToTarget)
        {
            //Enenmy not in range for attack
            if(targetInRange)
            {
                targetInRange = false;
                ChangeState(State.Run);
            }
        }
        else 
        {
            //Enemy is close to target
            if(!targetInRange)
            {
                moveDir = Vector2.zero;
                targetInRange = true;
                ChangeState(State.WindUp);
            }
        }
    }
    
    void Move()
    {
        if(!target)
        {
            Debug.Log(name + "Has no target");
            moveDir = Vector2.zero;
            return;
        }

        //Store last direction
        if(moveDir != Vector2.zero)
        {
            lastMoveDirection = moveDir.normalized;
        }
        
        if(!targetInRange)
        {
            moveDir = (Vector2)target.position - (Vector2)transform.position;
            if(moveDir.x < 0) {SR.flipX = true;}
            if(moveDir.x > 0) {SR.flipX = false;}
        }
        else //Enemy is close to target
        {
            moveDir = Vector2.zero;
        }
    }
    void WindUp()
    {
        ChangeState(State.Attack);
    }
    void Attack()
    {
        
        if(attackCooldown > 0)
        {
            
        }
        else
        {
            attackCooldown = 1f;
            AM.SetTrigger("Attack");
            canMove = false;
            Invoke("CanMoveAgain",0.5f);
            Invoke("AnimationAttackDelay",animationAttackDelay);
            
        }
    }
    void AnimationAttackDelay()
    {
        if(target)
            target.GetComponent<Health>()?.ApplyDamage(attackDamage);
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Idle: //When enemy spawns in
                //Idle();
                break;
            case State.Run: //Enemy dciding what state to enter
                if(canMove){RB.AddForce(moveDir.normalized * movementSpeed);}
                break;
            case State.WindUp: //Enemy winding up attack, lead to attack
                //WindUp();
                break;
            case State.Attack: //Enemy begins to attack
                //Attack();
                break;
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
    void Animate()
    {
        AM.SetFloat("MoveX",moveDir.normalized.x);
        AM.SetFloat("MoveY",moveDir.normalized.y);
        AM.SetFloat("LastMoveX",lastMoveDirection.x);
        AM.SetFloat("LastMoveY",lastMoveDirection.y);
        AM.SetFloat("MoveMagnitude",moveDir.normalized.magnitude);
    }
    void StopMovement()
    {
        ChangeState(State.Idle);
        moveDir = Vector2.zero;
    }
    void CanMoveAgain()
    {
        canMove = true;
    }
}
