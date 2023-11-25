using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private EnemyPatrol enemyPatrol;
    private float zoneShoot;
    private bool canShoot;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float timeShoot;
    private float timeCooldown;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        anim = GetComponent<Animator>();
        timeCooldown = timeShoot;
    }

    // Update is called once per frame
    void Update()
    {
        timeCooldown -= Time.deltaTime;
        canShoot = enemyPatrol.isAttack && timeCooldown < 0;

        if (!enemyPatrol.melee)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (canShoot)
        {
            anim.SetTrigger("Shoot");
            timeCooldown = timeShoot;
        }
    }

    private void ShootAnimEvent()
    {
        GameObject bulletPlant = Instantiate(bullet, transform.position, Quaternion.identity);
        bulletPlant.GetComponent<Transform>().Rotate(0, transform.eulerAngles.y, 0);
        bulletPlant.GetComponent<Rigidbody2D>().velocity = bulletSpeed * Vector2.left * (transform.eulerAngles.y == 180 ? -1 : 1);
    }
}
