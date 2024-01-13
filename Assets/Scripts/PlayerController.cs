using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：玩家控制
 * 创建时间：2023年2月26日10:58:05
 */

public class PlayerController : MonoBehaviour
{
    //召唤物游戏物体
    public GameObject pet;
    //自身组件
    private Rigidbody rb;
    private Animator animator;
    //移动
    private float inputH;
    private int playerDir;
    public float moveSpeed;
    //跳跃
    public float jumpForce;
    public bool isOnGround;
    //血量
    public int maxHP;
    public int currentHP;
    //延长时间
    public float timeDelay;

    private void Awake()
    {
        //读取数据
        SaveManager.instance.Load();
    }

    private void Start()
    {
        //获取组件
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        //设置初值
        isOnGround = true;
        playerDir = 1;
        currentHP = 50;
        //重复调用回血方法
        InvokeRepeating("RestoreHP", 0, 1);
    }

    private void Update()
    {
        //跳跃
        Jump();
        //转向
        ChangeDir();
    }

    private void FixedUpdate()
    {
        //移动
        Move();
    }

    /// <summary>
    /// 移动
    /// </summary>
    private void Move()
    {
        //获取水平轴向输入
        inputH = Input.GetAxis("Horizontal");
        //播放动画
        animator.SetFloat("InputH", Mathf.Abs(inputH));
        //如果水平轴向输入不为零
        if (inputH != 0)
        {
            //如果召唤物没有移动
            if (pet.GetComponent<Pet>().moving == false)
            {
                //延时调用，让召唤物移动
                Invoke("ControllPetMove", timeDelay);
            }
            //自身移动
            rb.MovePosition(rb.position + new Vector3(inputH * moveSpeed * Time.deltaTime, 0, 0));
        }

        //根据水平轴向输入值，翻转方向
        if (inputH > 0)
        {
            playerDir = 1;
        }
        else if (inputH < 0)
        {
            playerDir = -1;
        }
    }

    /// <summary>
    /// 改变方向
    /// </summary>
    private void ChangeDir()
    {
        transform.localScale = new Vector3(playerDir, 1, 1);
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            //延时调用，让召唤物跳跃
            Invoke("ControllPetJump", timeDelay);

            //播放跳跃动画
            animator.CrossFade("Jumping", 0.1f);
            //施加一个竖直向上的力
            rb.AddForce(Vector3.up * jumpForce);
            isOnGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //如果碰到地面
        if (collision.collider.CompareTag("Ground"))
        {
            //设置动画
            animator.CrossFade("Move", 0.1f);
            isOnGround = true;
        }
    }

    /// <summary>
    /// 控制宠物移动
    /// </summary>
    private void ControllPetMove()
    {
        pet.GetComponent<Pet>().moving = true;
    }

    /// <summary>
    /// 控制宠物跳跃
    /// </summary>
    private void ControllPetJump()
    {
        pet.GetComponent<Pet>().startJump = true;
    }

    /// <summary>
    /// 恢复血量
    /// </summary>
    private void RestoreHP()
    {
        if (currentHP < maxHP)
            currentHP += (int)(maxHP / 10);
    }
}
