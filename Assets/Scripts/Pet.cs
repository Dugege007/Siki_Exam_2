using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：召唤物控制
 * 创建时间：2023年2月26日15:59:04
 */

public class Pet : MonoBehaviour
{
    //跟随目标点
    public Transform targetTrans;
    //玩家位置
    public Transform playerTrans;
    //自身组件
    private Rigidbody rb;
    private Animator animator;
    //移动
    private float inputH;
    private int petDir;
    public float moveSpeed;
    public float stopSpeed;
    //跳跃
    public float jumpForce;
    //状态开关
    public bool moving;
    public bool startJump;
    public bool jumping;

    private void Start()
    {
        //获取组件
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        //设置初值
        petDir = 1;
    }

    private void Update()
    {
        //转向
        ChangeDir();
        //跳跃
        Jump();
    }

    private void FixedUpdate()
    {
        //朝玩家身后移动
        MoveToPlayer();
    }

    /// <summary>
    /// 向玩家身后移动
    /// </summary>
    private void MoveToPlayer()
    {
        //设置动画
        animator.SetFloat("InputH", inputH);

        //如果需要移动
        if (moving)
        {
            //设置移动动画输入为1
            inputH = 1;
            //靠近时缓动，避免闪烁
            float posX = Mathf.Clamp((targetTrans.position.x - transform.position.x) * stopSpeed, -1, 1);
            rb.MovePosition(rb.position + new Vector3(posX * moveSpeed * Time.deltaTime, 0, 0));
            //如果接近目标点
            if (Vector3.Distance(targetTrans.position, transform.position) < 0.05f)
            {
                //将速度设置为0
                rb.velocity = Vector3.zero;
                //关闭移动状态
                moving = false;
                //设置移动动画输入为0
                inputH = 0;
            }
        }

        //根据自身与玩家的位置翻转方向
        if ((playerTrans.position - transform.position).x > 0)
        {
            petDir = 1;
        }
        else if ((playerTrans.position - transform.position).x < 0)
        {
            petDir = -1;
        }
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    private void Jump()
    {
        if (startJump)
        {
            jumping = true;
            animator.CrossFade("Jumping", 0.1f);
            rb.AddForce(Vector3.up * jumpForce);
            startJump = false;
        }
    }

    /// <summary>
    /// 改变方向
    /// </summary>
    private void ChangeDir()
    {
        transform.localScale = new Vector3(petDir, 1, 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //如果碰到地面
        if (collision.collider.CompareTag("Ground"))
        {
            //设置移动动画
            animator.CrossFade("Move", 0.1f);
            jumping = false;
        }
    }
}
