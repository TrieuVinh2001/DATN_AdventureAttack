using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private bool attack;
    private int counter;
    private PlayerController player;

    [SerializeField] private BoxCollider2D boxAttack4;

    private void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    public void EndAttack()
    {
        attack = false;
        counter++;

        if (counter > 2)
        { 
            counter = 0;
        }

        player.GetAnimCount(attack, counter);
    }

    private void Attack5()
    {
        player.AttackBallSpell();
    }

    public void Attack4()
    {
        boxAttack4.enabled = true;
    }

    public void EndAttack4()
    {
        boxAttack4.enabled = false;
    }

}
