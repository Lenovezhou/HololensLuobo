using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameModel : Model
{
    private int m_PlayLevelIndex;
    private bool m_isPlaying;
    private List<Level> m_Levels = new List<Level>();


    #region 属性
    public override string Name
    {
        get
        {
            return Consts.M_GameModel;
        }
    }


    public int PlayLevelIndex
    {
        get { return m_PlayLevelIndex; }
    }

    public void EndLevel(bool isSuccess)
    {
        //if (isSuccess && PlayLevelIndex > GameProgress)
        //{
        //    //重新获取
        //    m_GameProgress = PlayLevelIndex;

        //    //保存
        //    Saver.SetProgress(PlayLevelIndex);
        //}

        ////游戏停止状态
        //m_IsPlaying = false;
    }

    public bool IsPlaying
    {
        get { return m_isPlaying; }
        set { m_isPlaying = value; }
    }



    public List<Level> AllLevels
    {
        get { return m_Levels; }
    }

    public Level PlayLevel
    {
        get
        {
            if (m_PlayLevelIndex < 0 || m_PlayLevelIndex > m_Levels.Count - 1)
                throw new IndexOutOfRangeException("关卡不存在");

            return m_Levels[m_PlayLevelIndex];
        }
    }



    #endregion


#region 帮助方法
    public void StartLevel(int levelIndex)
    {
        m_PlayLevelIndex = levelIndex;
        m_isPlaying = true;
    }


    public void Initialize()
    {
        //构建Level集合
        List<Level> levels = new List<Level>();

        AbstractFielTool aft = FielsFactory.CreatFielTool();

        aft.FillLevel(ref levels);

        //List<FileInfo> files = Tools.GetLevelFiles();
        //for (int i = 0; i < files.Count; i++)
        //{
        //    Level level = new Level();
        //    Tools.FillLevel(files[i].FullName, ref level);
        //    levels.Add(level);
        //}
        m_Levels = levels;

        //读取游戏进度
        //m_GameProgress = Saver.GetProgress();
        //m_GameProgress = 0;
    }
#endregion
}
