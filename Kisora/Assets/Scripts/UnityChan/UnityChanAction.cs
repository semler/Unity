using System.Collections.Generic;
using UnityEngine;

public class UnityChanAction : MonoBehaviour
{
    // アニメーター各ステートへの参照
    static int idleState = Animator.StringToHash("Base Layer.idle");
    static int runState = Animator.StringToHash("Base Layer.run");
    static int punchLState = Animator.StringToHash("Base Layer.punchL");
    static int punchHState = Animator.StringToHash("Base Layer.punchH");
    static int kickLState = Animator.StringToHash("Base Layer.kickL");
    static int kickHState = Animator.StringToHash("Base Layer.kickH");
    static int damageState = Animator.StringToHash("Base Layer.damage");
    static int deadState = Animator.StringToHash("Base Layer.dead");

    public bool isDead = false;
    public bool isDamage = false;

    public AudioClip audioAttack;
    public AudioClip audioDamage;
    private AudioSource audioSource;

    protected PunchL punchL;
    protected PunchH punchH;
    protected KickL kickL;
    protected KickH kickH;

    private AnimatorStateInfo stateInfo;
    private Animator animator;
    //左手のコライダー
    private Collider leftHandCollider;
    //右手のコライダー
    private Collider rightHandCollider;
    //右足のコライダー
    private Collider rightFootCollider;


    // Use this for initialization
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        punchL = FindObjectOfType<PunchL>();
        punchH = FindObjectOfType<PunchH>();
        kickL = FindObjectOfType<KickL>();
        kickH = FindObjectOfType<KickH>();

        //左手のコライダーを取得
        leftHandCollider = GameObject.Find("Character1_LeftHand").GetComponent<SphereCollider>();
        //右手のコライダーを取得
        rightHandCollider = GameObject.Find("Character1_RightHand").GetComponent<SphereCollider>();
        //右足のコライダーを取得
        rightFootCollider = GameObject.Find("Character1_RightToeBase").GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (punchL.Pressed == true) {
            animator.SetBool("isPunchL", true);

            //左手コライダーをオンにする
            leftHandCollider.enabled = true;
            //一定時間後にコライダーの機能をオフにする
            Invoke("ColliderReset", 0.5f);
        } 
        if (punchH.Pressed == true) {
            animator.SetBool("isPunchH", true);


            //右手コライダーをオンにする
            rightHandCollider.enabled = true;
            //一定時間後にコライダーの機能をオフにする
            Invoke("ColliderReset", 0.5f);
        }
        if (kickL.Pressed == true) {
            animator.SetBool("isKickL", true);

            //右足コライダーをオンにする
            rightFootCollider.enabled = true;
            //一定時間後にコライダーの機能をオフにする
            Invoke("ColliderReset", 1.5f);
        }
        if (kickH.Pressed == true) {
            animator.SetBool("isKickH", true);

            //右足コライダーをオンにする
            rightFootCollider.enabled = true;
            //一定時間後にコライダーの機能をオフにする
            Invoke("ColliderReset", 1.5f);
        }

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // return flag to false
        if (stateInfo.fullPathHash == punchLState)
        {
            if (!animator.IsInTransition(0))
            {
                audioSource.clip = audioAttack;
                audioSource.Play();
                animator.SetBool("isPunchL", false);
            }
        }
        else if (stateInfo.fullPathHash == punchHState)
        {
            if (!animator.IsInTransition(0))
            {
                audioSource.clip = audioAttack;
                audioSource.Play();
                animator.SetBool("isPunchH", false);
            }
        }
        else if (stateInfo.fullPathHash == kickLState)
        {
            if (!animator.IsInTransition(0))
            {
                audioSource.clip = audioAttack;
                audioSource.Play();
                animator.SetBool("isKickL", false);
            }
        }
        else if (stateInfo.fullPathHash == kickHState)
        {
            if (!animator.IsInTransition(0))
            {
                audioSource.clip = audioAttack;
                audioSource.Play();
                animator.SetBool("isKickH", false);
            }
        }
        else if (stateInfo.fullPathHash == damageState)
        {
            if (!animator.IsInTransition(0))
            {
                audioSource.clip = audioDamage;
                audioSource.Play();
                animator.SetBool("isDamage", false);
                isDamage = false;
            }
        }


    }

    private void ColliderReset()
    {
        leftHandCollider.enabled = false;
        rightHandCollider.enabled = false;
        rightFootCollider.enabled = false;
    }

}