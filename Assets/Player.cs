using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    public Collider2D [] enemyColliders;
    [Header("Attack details")]
    [SerializeField]private float attackRadius;
    [SerializeField]private Transform attackPoint;
    [SerializeField]private LayerMask whatIsEnemy;

    [Header("Movement details")]

    [SerializeField]private float moveSpeed = 3.5f;
    [SerializeField]private float jumpForce = 8;
    private bool isGround;
    private float xInput;
    private bool facingRight = true;

    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whayIsGround;


    


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovment();
        HandleAnimtions();
        HandleFlip();
        
        
    }

    public void DamageEnemies()
    {
        enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsEnemy);
    }
    private void HandleCollision()
    {
        isGround = Physics2D.Raycast(transform.position,Vector2.down, groundCheckDistance,whayIsGround);
    }
    private void HandleInput()
    {  
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if(Input.GetKeyDown(KeyCode.Mouse0))
            TryToAttack();

    }

    private void TryToAttack()
    {
        if (isGround)
        anim.SetTrigger("Attack");
    }


    private void HandleAnimtions()
    {
        bool isMoving = rb.linearVelocity.x != 0;
        anim.SetBool("isMoving",isMoving);
        anim.SetFloat("yVelocity",rb.linearVelocity.y);
    }

    private void HandleMovment()
    {
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);

    }

private void Jump()
    {
       if (isGround)
       rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
    private void HandleFlip()
    {
        if (rb.linearVelocity.x > 0 && facingRight==false)
        Flip();
        else if (rb.linearVelocity.x < 0 && facingRight == true)
         Flip();
    

    }
    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight =!facingRight;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0,-groundCheckDistance)); 
        Gizmos.DrawWireSphere(attackPoint.position,attackRadius);
    }
}