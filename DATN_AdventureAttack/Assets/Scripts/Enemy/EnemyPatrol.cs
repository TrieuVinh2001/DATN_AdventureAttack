using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : Enemy
{
    [Header("Move")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float zoneAttack;
    [SerializeField] private float zoneMovePlayer;
    [SerializeField] private Transform[] point;
    private int pointIndex;
    private Transform target;

    [Header("Attack")]
    private float timeCooldownAttack;
    [SerializeField] private float timeAttack;
    private bool isAttack = true;
    private PlayerController player;
    private float distance;
    private bool attackMode;
    private bool isWalk;
    private bool isDeath;

    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private BoxCollider2D boxAttack;

    private int facingDir = 1;
    private bool facingRight = true;

    protected Canvas canvas;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canvas = GetComponentInChildren<Canvas>();
        target = point[pointIndex];
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!isDeath)
        {
            CheckPlayer();
            if (!attackMode)
            {
                if (player)
                {
                    Move();
                }
                else
                {
                    MovePoint();
                }
            }

            anim.SetBool("Walk", isWalk);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerController>();
        }
    }

    private void CheckPlayer()
    {
        timeCooldownAttack -= Time.deltaTime;
        if (player)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < zoneAttack)
            {
                attackMode = true;
                isWalk = false;
                if (timeCooldownAttack < 0 && isAttack)
                {
                    Attack();
                }
            }
            else
            {
                attackMode = false;
            }

            if (distance > zoneMovePlayer)
            {
                player = null;
            }
        }
    }

    private void MovePoint()
    {
        if (transform.position == target.position)
        {
            pointIndex++;
            if (pointIndex >= point.Length)
            {
                pointIndex = 0;
            }
            target = point[pointIndex];
        }
        isWalk = true;
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
        if ((transform.position.x - target.position.x) < 0 && facingRight)
        {
            Flip();
        }
        else if ((transform.position.x - target.transform.position.x) > 0 && !facingRight)
        {
            Flip();
        }
    }

    private void Move()
    {
        isWalk = true;
        Vector2 tranf = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(tranf.x, transform.position.y);
        if ((transform.position.x - player.transform.position.x) < 0 && facingRight)
        {

            Flip();

        }
        else if ((transform.position.x - player.transform.position.x) > 0 && !facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        canvas.transform.localScale = new Vector3(-1 * facingDir, 1, 1);
    }

    private void Attack()
    {
        boxAttack.enabled = true;
        timeCooldownAttack = timeAttack;
        anim.SetBool("Attack", isAttack);
        isAttack = false;
    }

    private void EndAttack()
    {
        boxAttack.enabled = false;
        anim.SetBool("Attack", isAttack);
        isAttack = true;

    }

    public override void Death()
    {
        isDeath = true;
        anim.SetBool("Death", true);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()//Vẽ
    {
        Gizmos.DrawWireSphere(transform.position, zoneAttack);
        Gizmos.DrawWireSphere(transform.position, zoneMovePlayer);
    }
}
