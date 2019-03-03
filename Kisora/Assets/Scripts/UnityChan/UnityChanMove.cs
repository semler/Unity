using UnityEngine;
using System.Threading;

public class UnityChanMove : MonoBehaviour
{
    
    public float gravity;
    public float speed;
    public float jumpSpeed;
    public float rotateSpeed;
    public AudioClip audioRun;

    // JoyStick
    protected Joystick joystick;

    private AudioSource audioSource;
    private CharacterController characterController;
    private Animator animator;
    private Vector3 moveDirection = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        joystick = FindObjectOfType<Joystick>();
    }

    // Update is called once per frame
    void Update()
    {
        //rayを使用した接地判定
        if (!CheckGrounded())
        {
            return;
        }

        if (joystick.Vertical > 0)
        {
            moveDirection.z = speed;
            audioSource.clip = audioRun;
            audioSource.Play();
        }
        else
        {
            moveDirection.z = 0;
        }

        if (joystick.Horizontal < -0.3) {
            transform.Rotate(0, rotateSpeed * -1, 0);
        } else if (joystick.Horizontal > 0.3) {
            transform.Rotate(0, rotateSpeed, 0);
        }

        //jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveDirection.y = jumpSpeed;
        }

        //重力を発生させる
        moveDirection.y -= gravity * Time.deltaTime;

        //移動の実行
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        characterController.Move(globalDirection * Time.deltaTime);

        //速度が０以上の時、Runを実行する
        animator.SetBool("isRun", moveDirection.z > 0.0f);

    }

    //rayを使用した接地判定メソッド
    public bool CheckGrounded()
    {
        //初期位置と向き
        var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        //rayの探索範囲
        var tolerance = 0.3f;

        //rayのHit判定
        //第一引数：飛ばすRay
        //第二引数：Rayの最大距離
        return Physics.Raycast(ray, tolerance);
    }

}