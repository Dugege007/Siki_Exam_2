using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/*
 * 创建人：杜
 * 功能说明：UI控制
 * 创建时间：2023年2月26日19:40:53
 */

public class UIManager : MonoBehaviour
{
    //玩家控制脚本
    public PlayerController player;
    //血条
    public Image playerHPBar;
    public Image playerHP;
    public Image petHPBar;
    //退出按钮
    public Button quitBtn;

    //储存UI位置
    private Vector2 lastPos1;
    private Vector2 lastPos2;
    private Vector2 lastPos3;
    private Vector2 targetPos1;
    private Vector2 targetPos2;
    private Vector2 targetPos3;

    //交换冷却时间
    public float changeTime;
    private float CDTime;

    private void Start()
    {
        //保存初始点位置
        targetPos1 = new Vector2(-1650, -100);
        targetPos2 = new Vector2(-1650, -160);
        targetPos3 = new Vector2(1750, -120);
        CDTime = Time.time;
    }

    private void Update()
    {
        //更新血量
        UpdateHP();
        //改变UI位置
        ChangeUIPos();
    }

    /// <summary>
    /// 更新血量
    /// </summary>
    private void UpdateHP()
    {
        playerHP.fillAmount = (float)player.currentHP / player.maxHP;
    }

    /// <summary>
    /// 交换UI位置
    /// </summary>
    private void ChangeUIPos()
    {
        if (Input.GetKeyDown(KeyCode.A) && Time.time - CDTime > changeTime)
        {
            //保存当前位置
            lastPos1 = playerHPBar.rectTransform.anchoredPosition;
            lastPos2 = petHPBar.rectTransform.anchoredPosition;
            lastPos3 = quitBtn.GetComponent<RectTransform>().anchoredPosition;

            //Dotween动画
            playerHPBar.rectTransform.DOAnchorPos(targetPos1, changeTime, true);
            petHPBar.rectTransform.DOAnchorPos(targetPos2, changeTime, true);
            quitBtn.GetComponent<RectTransform>().DOAnchorPos(targetPos3, changeTime, true);

            //将之前的位置赋值给目标位置
            targetPos1 = lastPos1;
            targetPos2 = lastPos2;
            targetPos3 = lastPos3;

            //重置冷却
            CDTime = Time.time;
        }
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
        //保存数据
        SaveManager.instance.Save();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
