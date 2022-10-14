using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeManager : MonoBehaviour
{

    public GameObject Sword;
//    private Component[] swordComponets;
    public GameObject hitbox;
    public bool canAttack = true;
    public float attackCooldown = 1.0f;
    public bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
 //       swordComponets = GetComponentsInChildren<Sword>;
        hitbox.active = false;
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
        hitbox.active = true;
        isAttacking = true;
        canAttack = false;
//        Animator anim = Sword.Find("pivotFeo").GetComponent<Animator>();
        Animator anim = Sword.GetComponent<Animator>();
        anim.SetTrigger("Attack");
        Invoke("ResetAttackCooldown", attackCooldown);
        Invoke("ResetIsAttacking", attackCooldown-.05f);
    }

    void ResetAttackCooldown()
    {
        canAttack = true;
    }

    void ResetIsAttacking()
    {
        hitbox.active = false;
        isAttacking = false;
    }
}
