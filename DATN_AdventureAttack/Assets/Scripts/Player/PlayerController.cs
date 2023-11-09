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
    private bool doubleJump;

    [Header("State")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float damage;
    private float currentHealth;

    [Header("Check")]
    [SerializeField] private float groundCheckDistance;//Khoảng cách đến điểm kiểm tra
    [SerializeField] private LayerMask whatIsGround;//Layer của vật thể

    [Header("Attack")]
    [SerializeField] private Transform pointAttack;
    [SerializeField] private float zoneAttack;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject lightningBallPrefab;
    [SerializeField] protected Transform pointBullet;
    [SerializeField] private float bulletSpeed;

    [SerializeField] private GameObject floatingText;

    private float xInput;
    private bool isGround;
    private bool isAttack;
    private bool attackStarted;
    private bool isDeath;
    private int comboCounter;
    [HideInInspector] public float timeCounter;

    private int facingDir = 1;
    private bool facingRight = true;

    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private Image hpImage;
    [SerializeField] private Image hpEffectImage;
    private float hurtSpeed = 0.01f;

    public void GetAnimCount(bool attack, int counter)
    {
        isAttack = attack;
        attackStarted = attack;
        comboCounter = counter;
        timeCounter = 1.5f;
    }

    public void AttackBallSpell()
    {
        GameObject ballSpell = Instantiate(lightningBallPrefab, new Vector3(pointBullet.position.x * facingDir, pointBullet.position.y, 0), Quaternion.Euler(0, 0, facingRight ? 360 : 180));
        ballSpell.GetComponent<Rigidbody2D>().velocity = Vector2.right * bulletSpeed * facingDir;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        isGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);//Kiểm tra chạm đất

        InputKey();
        
        PlayerAnimation();

        HealthBar();

        timeCounter -= Time.deltaTime;

        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void InputKey()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Dash();
        }
        if (Input.GetKeyDown(KeyCode.Q) && isGround)
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.E) && isGround)
        {
            attackStarted = true;
            anim.SetTrigger("Attack4");
        }
        if (Input.GetKeyDown(KeyCode.R) && isGround)
        {
            attackStarted = true;
            anim.SetTrigger("Attack5");
        }
        if (Input.GetKeyDown(KeyCode.T) && !isGround)
        {
            anim.SetTrigger("Attack6");
            doubleJump = false;
        }
    }

    private void Move()
    {
        if (attackStarted)
        {
            rb.velocity = Vector2.zero;
        }
        else if (dashTime > 0)
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
        if (isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            doubleJump = true;
        }
        else if (doubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            doubleJump = false;
        }
    }

    private void Dash()
    {
        if (dashCooldownTimer < 0)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }

    private void Attack()
    {
        if (timeCounter < 0)
        {
            comboCounter = 0;
        }
        isAttack = true;
        attackStarted = true;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pointAttack.position, zoneAttack, enemyLayer);
        if (colliders.Length > 0)
        {
            foreach (Collider2D enemy in colliders)
            {
                enemy.GetComponent<EnemyBase>().TakeDamage(damage);
            }
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
        anim.SetBool("Attack", isAttack);
        anim.SetInteger("ComboCounter", comboCounter);
    }

    public void TakeDamage(float dame)
    {
        GameObject point = Instantiate(floatingText, transform.position, Quaternion.identity);
        point.transform.GetChild(0).GetComponent<TextMeshPro>().text = "-" + dame;
        currentHealth -= dame;
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void HealthBar()
    {
        hpImage.fillAmount = currentHealth / maxHealth;

        if (hpEffectImage.fillAmount > hpImage.fillAmount)
        {
            hpEffectImage.fillAmount -= hurtSpeed;
        }
        else
        {
            hpEffectImage.fillAmount = hpImage.fillAmount;
        }
    }

    private void Death()
    {
        isDeath = true;
        anim.SetBool("Death", isDeath);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));//Đường chạm đất
        Gizmos.DrawWireSphere(pointAttack.position, zoneAttack);//Vòng tròn phạm vi tấn công
    }
}
