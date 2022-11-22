using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private bool IsShooting;

    // Start is called before the first frame update
    void Start()
    {
        //swordComponets = GetComponentsInChildren<Sword>;
        hitbox.SetActive(false);
    }

    
    public void onFire(InputAction.CallbackContext context){
        IsShooting  =  context.action.triggered;//yeah, im that lazy
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(IsShooting && canAttack && !IsAttacking)
            {
                SwordAttack();
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
        Invoke("ResetIsAttacking", attackCooldown-.5f);
        Invoke("ResetAttackCooldown", attackCooldown);
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
