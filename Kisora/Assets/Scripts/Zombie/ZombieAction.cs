using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAction : MonoBehaviour
{
    static int deadState = Animator.StringToHash("Base Layer.dead");

    private CharacterController controller;
    private Animator animator;
    private Vector3 unityChanDestination;
    private Vector3 randDestination;
    private Vector3 velocity;
    private Vector3 direction;
    private float waitTime;
    private float currentTime;
    private GameObject unityChan;
    private UnityChanAction unityChanAction;

    private AnimatorStateInfo stateInfo;

    //左手のコライダー
    private Collider leftHandCollider;
    //右手のコライダー
    private Collider rightHandCollider;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        velocity = Vector3.zero;
        unityChan = GameObject.Find("UnityChan");
        unityChanAction = unityChan.GetComponent<UnityChanAction>();
        waitTime = 2.0f;
        currentTime = 0.0f;
        randDestination = RandDestination();

        foreach (Transform child in transform)
        {
            if (child.gameObject.name.CompareTo("L Hand") == 0)
            {
                //左手のコライダーを取得
                leftHandCollider = child.gameObject.GetComponent<SphereCollider>();
            }

            if (child.gameObject.name.CompareTo("R Hand") == 0)
            {
                //右手のコライダーを取得
                rightHandCollider = child.gameObject.GetComponent<SphereCollider>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == deadState) {
            return;
        }
            
        unityChanDestination = new Vector3(unityChan.transform.position.x, unityChan.transform.position.y, unityChan.transform.position.z);

        if (unityChanAction.isDead || Vector3.Distance(unityChanDestination, transform.position) > 10) {
            animator.SetBool("isAttack", false);

            if (Vector3.Distance(randDestination, transform.position) < 0.5) {
                currentTime += Time.deltaTime;
                if (currentTime > waitTime) {
                    animator.SetBool("isWalk", true);
                    randDestination = RandDestination();
                    currentTime = 0.0f;

                    Walk(randDestination, 0.5f);
                } else {
                    animator.SetBool("isWalk", false);
                }
            } else {
                animator.SetBool("isWalk", true);

                Walk(randDestination, 0.5f);
            }
        } else if (Vector3.Distance(unityChanDestination, transform.position) < 1) {
            animator.SetBool("isWalk", false);
            Attack();

            direction = (unityChanDestination - transform.position).normalized;
            transform.LookAt(new Vector3(unityChanDestination.x, transform.position.y, unityChanDestination.z));
        } else {
            animator.SetBool("isWalk", true);
            animator.SetBool("isAttack", false);

            Walk(unityChanDestination, 1.0f);
        }


    }

    private void Walk(Vector3 destination, float speed)
    {
        if (controller.isGrounded) {
            velocity = Vector3.zero;
            direction = (destination - transform.position).normalized;
            transform.LookAt(new Vector3(destination.x, transform.position.y, destination.z));
            velocity = direction * speed;
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private Vector3 RandDestination()
    {
        Vector3 random = Random.insideUnitCircle * 10;
        return transform.position + new Vector3(random.x, 0, random.y);
    }



    private void Attack()
    {
        //手コライダーをオンにする
        leftHandCollider.enabled = true;
        rightHandCollider.enabled = true;
        //一定時間後にコライダーの機能をオフにする
        Invoke("ColliderReset", 0.3f);

        animator.SetBool("isAttack", true);
    }


    private void ColliderReset()
    {
        leftHandCollider.enabled = false;
        rightHandCollider.enabled = false;
    }
}
