using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeManager : MonoBehaviour
{

    [SerializeField] private GameObject Sword;
    // private Component[] swordComponets;
    [SerializeField] private GameObject hitbox;
    private bool canAttack = true;
    private float attackCooldown = 1.0f;

    private bool _isAttacking = false;
    public bool IsAttacking
    {
        get{return _isAttacking;}
        set{_isAttacking = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        //swordComponets = GetComponentsInChildren<Sword>;
        hitbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(canAttack && !IsAttacking)
            {
                SwordAttack();
            }
        }
    }

    public void SwordAttack()
    {
        hitbox.SetActive(true);
        IsAttacking = true;
        canAttack = false;
        //Animator anim = Sword.Find("pivotFeo").GetComponent<Animator>();
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
        hitbox.SetActive(false);
        IsAttacking = false;
    }
}
