using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Character>().OnHit(30f);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.GetComponent<Enemy>().IsDead)
            {
                return;
            }
            else
            {
                other.GetComponent<Character>().OnHit(30f);
            }
        }
    }
}
