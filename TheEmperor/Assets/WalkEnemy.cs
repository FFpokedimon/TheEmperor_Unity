using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkEnemy : Enemy
{
    [SerializeField] float speed;
    [SerializeField] float detectionDistance;
    float patroltimer;

    public override void Move()
    {
        if (distance < detectionDistance && distance > attackDistance)
        {
            transform.LookAt(player.transform);
            anim.SetBool("Run", true);
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        }
        else if (distance > detectionDistance)
        {
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
            patroltimer += Time.deltaTime;
            anim.SetBool("Run", true);
            if (patroltimer > 10)
            {
                transform.Rotate(new Vector3(0, 90, 0));
                patroltimer = 0;
            }
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }

    public override void Attack()
    {
        timer += Time.deltaTime;
        if (distance < attackDistance && timer > cooldown)
        {
            timer = 0;
            player.GetComponent<PlayerController>().ChangeHealth(damage);          
            anim.SetBool("Attack", true);
        }
        else
        {
            anim.SetBool("Attack", false);
        }
    }
}