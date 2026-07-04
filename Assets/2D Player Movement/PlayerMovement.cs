using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 input;
    Rigidbody2D RB;
    SpriteRenderer SR;
    Animator AM;
    private Vector2 lastMoveDirection; 
    private bool isFacingLeft = true;

    [SerializeField] float movementSpeed = 15;
    // Update is called once per frame
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        AM = GetComponent<Animator>();
    }
    public void Move(InputAction.CallbackContext context)
    {
        //Store last direction
        lastMoveDirection = input.normalized;

        input = context.ReadValue<Vector2>();
        if(input.x < 0) {SR.flipX = true; isFacingLeft = true;}
        if(input.x > 0) {SR.flipX = false; isFacingLeft = false;}
        
        
    }
    void FixedUpdate()
    {
        RB.AddForce(input.normalized * movementSpeed);
    }
    void Update()
    {
        Animate();
    }
    void Animate()
    {
        AM.SetFloat("MoveX",input.normalized.x);
        AM.SetFloat("MoveY",input.normalized.y);
        AM.SetFloat("LastMoveX",lastMoveDirection.x);
        AM.SetFloat("LastMoveY",lastMoveDirection.y);
        AM.SetFloat("MoveMagnitude",input.normalized.magnitude);
    }
    public void StopToInteract(Vector2 targetPos)
    {
        //Very briefly stop player from moving

        //Face player towards target (pos)
        lastMoveDirection = (targetPos - (Vector2)transform.position).normalized;
        if(lastMoveDirection.x < 0) {SR.flipX = true; isFacingLeft = true;}
        if(lastMoveDirection.x > 0) {SR.flipX = false; isFacingLeft = false;}

        //Play animation
        AM.SetTrigger("Interact");
    }
}
