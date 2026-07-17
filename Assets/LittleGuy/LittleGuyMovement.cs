using UnityEngine;

public class LittleGuyMovement : MonoBehaviour
{
    Vector2 moveDir;
    Rigidbody2D RB;
    SpriteRenderer SR;
    Animator AM;    
    
    [SerializeField] private Vector2 target;
    [SerializeField] private Transform persistantTarget;
    bool isMovingToTarget = false;
    private Vector2 lastMoveDirection; 
    private bool isFacingLeft = true;

    [SerializeField] float movementSpeed = 10;
    [SerializeField] float movementSpeedFollower = 10;
    [SerializeField] float desiredDistanceToTarget = 0.25f;
    [SerializeField] LayerMask IgnoreWhileFollowing;
    bool canMove = true;
    [SerializeField] bool FindAndFollowPlayer = false;

    public void SetTargetLocation(Vector2 PosTarget)
    {
        if(PosTarget != null)
        {
            isMovingToTarget = true;
            target = PosTarget;
            GetComponent<Rigidbody2D>().excludeLayers = IgnoreWhileFollowing;
        }
        else
        {
            GetComponent<Rigidbody2D>().excludeLayers = 0;
        }
        
    }
    public void SetFollower(Transform Transtarget)
    {
        isMovingToTarget = true;
        persistantTarget = Transtarget;
    }
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        AM = GetComponent<Animator>();
        target = transform.position;

        if(FindAndFollowPlayer)
        {
            Commander.instance.AddNewGnomeToFollowers(gameObject.transform);
        }
    }
    void Update()
    {
        if(persistantTarget != null)
        {
            CheckDistanceToFollowerPos();
            if(isMovingToTarget)  {MoveAsFollower();}
        }
        if(persistantTarget == null)
        {
            if(isMovingToTarget) Move();
        }
        
        
        
        Animate();
    }
    void Move()
    {
        //Store last direction
        lastMoveDirection = moveDir.normalized;
        
        if(Vector2.Distance(transform.position, target) > desiredDistanceToTarget)
        {
            moveDir = target - (Vector2)transform.position;
            if(moveDir.x < 0) {SR.flipX = true; isFacingLeft = true;}
            if(moveDir.x > 0) {SR.flipX = false; isFacingLeft = false;}
        
            
        }
        else
        {
            isMovingToTarget = false;
            moveDir = Vector2.zero;
        }
    }
    void MoveAsFollower()
    {
        //Store last direction
        lastMoveDirection = moveDir.normalized;
        
        if(Vector2.Distance(transform.position, persistantTarget.position) > desiredDistanceToTarget)
        {
            moveDir = (Vector2)persistantTarget.position - (Vector2)transform.position;
            if(moveDir.x < 0) {SR.flipX = true; isFacingLeft = true;}
            if(moveDir.x > 0) {SR.flipX = false; isFacingLeft = false;}
        
            
        }
        else
        {
            isMovingToTarget = false;
            moveDir = Vector2.zero;
        }
    }
    void CheckDistanceToFollowerPos()
    {
        if(Vector2.Distance(transform.position, persistantTarget.position) > desiredDistanceToTarget)
        {
            isMovingToTarget = true;
        }
    }

    void FixedUpdate()
    {
        if(canMove)
        {
            if(persistantTarget)
            {
                RB.AddForce(moveDir.normalized * movementSpeedFollower);
            }
            else
            {
                RB.AddForce(moveDir.normalized * movementSpeed);
            }
        }
        
        
    }

    
    void Animate()
    {
        AM.SetFloat("MoveX",moveDir.normalized.x);
        AM.SetFloat("MoveY",moveDir.normalized.y);
        if(!isMovingToTarget)
        {
            AM.SetFloat("LastMoveX",lastMoveDirection.x);
            AM.SetFloat("LastMoveY",lastMoveDirection.y);
        }
        AM.SetFloat("MoveMagnitude",moveDir.normalized.magnitude);
    }
    public void RotateTowardsTarget(Vector2 targetPos)
    {
        //Very briefly stop Gnome from moving
        CantMoveAfterAttack();
        
        //Face player towards target (pos)
        lastMoveDirection = (targetPos - (Vector2)transform.position).normalized;
        if(lastMoveDirection.x < 0) {SR.flipX = true; isFacingLeft = true;}
        if(lastMoveDirection.x > 0) {SR.flipX = false; isFacingLeft = false;}

        //Play animation
        AM.SetTrigger("Attack");
    }
    void CantMoveAfterAttack()
    {
        canMove = false;
        Invoke("CanMoveAgain",0.15f);
    }
    void CanMoveAgain()
    {
        canMove = true;
    }
    public bool IsFollower()
    {
        if(persistantTarget) {return true;}
        else {return false;}
    }
}
