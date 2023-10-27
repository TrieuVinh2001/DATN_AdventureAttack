using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;//Thời gian tốc biến cố định
    [SerializeField] private float dashCooldown;//Thời gian hồi tốc biến
    private float dashTime;//Thời gian dùng tốc biến
    private float dashCooldownTimer;//Biến trung gian giảm dần thời gian hồi tốc biến

    //[Header("State")]
    //[SerializeField] private float maxHealth;
    //[SerializeField] private float damage;
    //private float currentHealth;

    [Header("Check")]
    [SerializeField] private float groundCheckDistance;//Khoảng cách đến điểm kiểm tra
    [SerializeField] private LayerMask whatIsGround;//Layer của vật thể

    //[Header("Attack")]
    //[SerializeField] private Transform pointAttack;
    //[SerializeField] private float zoneAttack;
    //[SerializeField] private LayerMask enemyLayer;
    //[SerializeField] private GameObject lightningBallPrefab;
    //[SerializeField] private float bulletSpeed;

    //[SerializeField] private GameObject floatingText;

    private float xInput;
    private bool isGround;
    //private bool isAttack;
    //private bool attackStarted;
    //private bool isDeath;
    //private int comboCounter;

    private int facingDir = 1;
    private bool facingRight = true;

    private Rigidbody2D rb;
    private Animator anim;

    //[SerializeField] private Image hpImage;
    //[SerializeField] private Image hpEffectImage;
    //private float hurtSpeed = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        isGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Dash();
        }
        PlayerAnimation();
        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {

        if (dashTime > 0)
        {
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }

        if (rb.velocity.x < 0 && facingRight)
        {
            Flip();
        }
        else if (rb.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void Dash()
    {
        if (dashCooldownTimer < 0)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        //transform.Rotate(0, 180, 0);
        transform.localScale = new Vector3(1 * facingDir, 1, 1);
    }

    private void PlayerAnimation()
    {
        anim.SetFloat("xInput", Mathf.Abs(xInput));
        anim.SetBool("Jump", isGround);
        anim.SetBool("Dash", dashTime > 0);
        //anim.SetBool("Attack", isAttack);
        //anim.SetInteger("ComboCounter", comboCounter);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
