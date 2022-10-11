using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeManager : MonoBehaviour
{

    public GameObject Sword;
    public bool canAttack = true;
    public float attackCooldown = 1.0f;
    public bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(canAttack && !isAttacking)
            {
                SwordAttack();
            }
        }
    }

    public void SwordAttack()
    {
        isAttacking = true; 
        canAttack = false;
        Animator anim = Sword.GetComponent<Animator>();
        anim.SetTrigger("Attack");
        Invoke("ResetAttackCooldown", attackCooldown);
        Invoke("ResetIsAttacking", attackCooldown);
    }

    void ResetAttackCooldown()
    {
        canAttack = true;
    }

    void ResetIsAttacking()
    {
        isAttacking = false;
    }
}
