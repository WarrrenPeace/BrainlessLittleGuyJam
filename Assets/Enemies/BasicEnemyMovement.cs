using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour
{
    Vector2 moveDir;
    Rigidbody2D RB;
    SpriteRenderer SR;
    Animator AM;    
    
    [SerializeField] private Transform target;
    bool isMovingToTarget = false;
    private Vector2 lastMoveDirection; 
    private bool isFacingLeft = true;

    [SerializeField] float movementSpeed = 10;
    [SerializeField] float desiredDistanceToTarget = 0.25f;

    public void SetTargetLocation(Transform loc)
    {
        isMovingToTarget = true;
        target = loc;
    }
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        AM = GetComponent<Animator>();
        SetTargetLocation(GameObject.FindWithTag("Player").transform);
    }
    void Update()
    {
        if(target) {Move();}
        
        Animate();
    }
    void Move()
    {
        //Store last direction
        if(moveDir != Vector2.zero)
        {
            lastMoveDirection = moveDir.normalized;
        }
        
        
        if(Vector2.Distance(transform.position, target.position) > desiredDistanceToTarget)
        {
            moveDir = (Vector2)target.position - (Vector2)transform.position;
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
        AM.SetFloat("LastMoveX",lastMoveDirection.x);
        AM.SetFloat("LastMoveY",lastMoveDirection.y);
        AM.SetFloat("MoveMagnitude",moveDir.normalized.magnitude);
    }
}
