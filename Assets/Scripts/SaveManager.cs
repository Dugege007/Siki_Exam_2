using UnityEngine;
using LitJson;
using System.IO;

/*
 * 创建人：杜
 * 功能说明：保存数据
 * 创建时间：2023年2月26日21:48:02
 */

public class SaveManager : MonoBehaviour
{
    //角色位置
    public Transform playerTrans;
    public Transform petTrans;
    //单例
    public static SaveManager instance;
    //保存文件路径
    private string filePath;

    //private Vector3 tempPos1;
    //private Vector3 tempPos2;
    //public float preTime;

    private void Awake()
    {
        instance= this;
    }

    private void Start()
    {
        //保存文件路径
        filePath = Application.dataPath + "/StreamingFile" + "/PosData.json";
    }

    /// <summary>
    /// 创建Save对象
    /// </summary>
    /// <returns>设置好属性值的Save对象</returns>
    private Save CreateSave()
    {
        //新建一个Save对象
        Save save = new Save();
        //设置属性值
        save.playerPosX=playerTrans.position.x;
        save.playerPosY=playerTrans.position.y;
        save.petPosX = petTrans.position.x;
        save.petPosY = petTrans.position.y;
        return save;
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    public void Save()
    {
        Save save = CreateSave();
        filePath = Application.dataPath + "/StreamingFile" + "/PosData.json";
        string saveJsonStr=JsonMapper.ToJson(save);
        StreamWriter sw= new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close();
    }

    /// <summary>
    /// 加载数据
    /// </summary>
    public void Load()
    {
        filePath = Application.dataPath + "/StreamingFile" + "/PosData.json";
        if (File.Exists(filePath))
        {
            StreamReader sr= new StreamReader(filePath);
            string jsonStr=sr.ReadToEnd();
            sr.Close();
            Save save=JsonMapper.ToObject<Save>(jsonStr);
            SetGame(save);
        }
    }

    /// <summary>
    /// 设置玩家和召唤物的位置
    /// </summary>
    /// <param name="save"></param>
    private void SetGame(Save save)
    {
        playerTrans.position = new Vector3((float)save.playerPosX, (float)save.playerPosY, 0);
        petTrans.position = new Vector3((float)save.petPosX, (float)save.petPosY, 0);
    }
}
