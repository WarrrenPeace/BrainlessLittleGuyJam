using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    public enum State {Idle, March, Run, WindUp, Attack}
    public State state;
    Vector2 moveDir;
    Vector2 lastMoveDirection; 
    Rigidbody2D RB;
    SpriteRenderer SR;
    Animator AM;    
    
    [Header("Stats")]
    [SerializeField] float movementSpeed = 10;
    [SerializeField] float desiredDistanceToTarget = 0.25f;
    [SerializeField] float attackCooldown = 1.5f;
    [SerializeField] int attackDamage = 5;
    [SerializeField] float animationAttackDelay = 0.6f;

    [Header("Targeting")]
    [SerializeField] private Transform target;    
    private bool targetInRange = false;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float detectionRadius = 1;
    public bool targetPlayerInstantly = false;
    public bool isEvil = true;
    private float boredomTimer = 1.5f;
    [SerializeField] private float boredom = 0;


    
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        AM = GetComponent<Animator>();
        if(targetPlayerInstantly) {SetTarget(GameObject.FindGameObjectWithTag("Player").transform);} //Bug if player is dead
        InvokeRepeating("AutoFindTarget",0.5f,0.2f);
        ResumeTravelToGoal();
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
        if(targetTransform != null)
        {
            if(!targetInRange) {ChangeState(State.Run);}
            target = targetTransform;
        }
    }
    void ChangeState(State stateToChangeTo)
    {
        boredom = boredomTimer;
        state = stateToChangeTo;
        Debug.Log(state);
    }
    void Update()
    {
        DistanceToTargetCheck();
        switch (state)
        {
            case State.Idle: //Standing still
                Idle();
                break;
                case State.March: //Running to opposite side
                March();
                break;
            case State.Run: //Enemy running towards enemy
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
                Debug.Log(tempTarget.gameObject.layer + " Targeted by " + gameObject.layer);
                
            }
        }

        
        return tempTarget;
    }
    void Idle()
    {
        //Timer here to start marching again
        if(boredom > 0)
        {
            boredom -= 1 * Time.deltaTime;
        }
        else
        {
            Debug.Log("Start March!");
            ResumeTravelToGoal();
        }
    }
    void March()
    {
        if(moveDir.x < 0) {SR.flipX = true;}
        if(moveDir.x > 0) {SR.flipX = false;}
    }
    void DistanceToTargetCheck()
    {
        if(!target)
        {
            if(state != State.Idle && state != State.March) {StopMovement();}
            
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
            if(moveDir.x < 0) {SR.flipX = true;}
            if(moveDir.x > 0) {SR.flipX = false;}
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
            attackCooldown = .75f;
            AM.SetTrigger("Attack");
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
            case State.March: //When enemy spawns in
                RB.AddForce(moveDir.normalized * movementSpeed);
                break;
            case State.Run: //Enemy dciding what state to enter
                RB.AddForce(moveDir.normalized * movementSpeed * 2);
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
        Debug.Log("Stop");
        ChangeState(State.Idle);
        moveDir = Vector2.zero;
    }
    void ResumeTravelToGoal() //After break of having no target continue traveling to respective side of screen
    {
        ChangeState(State.March);
        if(isEvil)
        {
            moveDir = -Vector2.right;
        }
        else
        {
            moveDir = Vector2.right;
        }
        
    }
}
