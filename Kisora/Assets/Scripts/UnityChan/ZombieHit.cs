using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHit : MonoBehaviour
{

    //オブジェクトと接触した瞬間に呼び出される
    void OnTriggerEnter(Collider other)
    {
        if (other.tag.StartsWith("Zombie")) {

            GameObject zombie = other.gameObject;
            Animator animator = zombie.GetComponent<Animator>();

            animator.SetBool("isDead", true);
        }
    }
}