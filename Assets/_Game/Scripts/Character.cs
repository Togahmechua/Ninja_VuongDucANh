using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected HealthBar healthBar;
    // [SerializeField] private CombatText combatTextPrefab;
    private string currentAnimName;
    private float hp;
    public bool IsDead => hp <= 0;

    protected virtual void Start()
    {
        OnInit();
    }

    public virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(100,transform);
    }

    public virtual void OnDespawn()
    {

    }

    protected virtual void OnDeath()
    {
        ChangeAnim("Die");
        
        Invoke(nameof(OnDespawn), 2f);
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    public void OnHit(float damge)
    {
        Debug.Log("Hit");
        if (!IsDead)
        {
            hp -= damge;

            if (IsDead)
            {
                hp = 0;
                OnDeath();
            }

            healthBar.SetNewHp(hp);
            // Instantiate(combatTextPrefab,transform.position, Quaternion.identity).OnInit(damge);
        }
    }

}
