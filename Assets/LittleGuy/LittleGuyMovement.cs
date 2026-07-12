using UnityEngine;

public class LittleGuyMovement : MonoBehaviour
{
    Vector2 moveDir;
    Rigidbody2D RB;
    SpriteRenderer SR;
    Animator AM;    
    
    private Vector2 target;
    bool isMovingToTarget = false;
    private Vector2 lastMoveDirection; 
    private bool isFacingLeft = true;

    [SerializeField] float movementSpeed = 10;
    [SerializeField] float desiredDistanceToTarget = 0.25f;

    public void SetTargetLocation(Vector2 loc)
    {
        isMovingToTarget = true;
        target = loc;
    }
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        AM = GetComponent<Animator>();
        target = transform.position;
    }
    void Update()
    {
        if(isMovingToTarget) {Move();}
        
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

    void FixedUpdate()
    {
        RB.AddForce(moveDir.normalized * movementSpeed);
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
        //Very briefly stop player from moving

        //Face player towards target (pos)
        lastMoveDirection = (targetPos - (Vector2)transform.position).normalized;
        if(lastMoveDirection.x < 0) {SR.flipX = true; isFacingLeft = true;}
        if(lastMoveDirection.x > 0) {SR.flipX = false; isFacingLeft = false;}

        //Play animation
        AM.SetTrigger("Attack");
    }
}
