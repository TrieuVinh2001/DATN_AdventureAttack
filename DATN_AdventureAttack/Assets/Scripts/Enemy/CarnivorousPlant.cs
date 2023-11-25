using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivorousPlant : EnemyBase
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float damage;

    [SerializeField] private float zoneAttack;
    [SerializeField] private float zoneShoot;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject bullet;

    private Transform target;

    private float timeCooldownShoot;
    [SerializeField] private float timeShoot;

    private float timeCooldownAttack;
    [SerializeField] private float timeAttack;

    private bool canShoot = false;
    private bool canAttack = false;

    private Animator anim;
    private Canvas canvas;

    protected override void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();
        canvas = GetComponentInChildren<Canvas>();
    }

    protected override void Update()
    {
        base.Update();

        timeCooldownAttack -= Time.deltaTime;
        timeCooldownShoot -= Time.deltaTime;

        Attack();       
    }

    private void Attack()
    {
        canShoot = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, zoneShoot, playerLayer);
        canAttack = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, zoneAttack, playerLayer);

        if (canShoot && timeCooldownShoot < 0 && !canAttack)
        {
            RaycastHit2D col = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, zoneShoot, playerLayer);
            target = col.collider.GetComponent<PlayerController>().transform;
            anim.SetTrigger("Shoot");
            timeCooldownShoot = timeShoot;
            GameObject bulletPlant = Instantiate(bullet, transform.position, Quaternion.identity);
            bulletPlant.GetComponent<Transform>().localScale = new Vector3(transform.localScale.x, 1, 1);
            bulletPlant.GetComponent<Rigidbody2D>().velocity = bulletSpeed * Vector2.left * transform.localScale.x;


        }
        else if (canAttack && timeCooldownAttack < 0)
        {
            RaycastHit2D col = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, zoneShoot, playerLayer);
            target = col.collider.GetComponent<PlayerController>().transform;
            timeCooldownAttack = timeAttack;
            anim.SetTrigger("Attack"); 
        }
    }

    private void AttackAnimationEvent()
    {
        if (target != null)
        {
            target.GetComponent<PlayerStats>().TakeDamage(damage);
        }
    }

    public override void Death()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x - zoneShoot * transform.localScale.x , transform.position.y));
    }
}
