using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameModel : Model
{
    private int m_PlayLevelIndex;
    private bool m_isPlaying;
    private List<Level> m_Levels = new List<Level>();
    private int m_GameProgress;
    private bool m_IsPlaying;
    private int m_gold;

    #region 属性
    public override string Name
    {
        get
        {
            return Consts.M_GameModel;
        }
    }

    public int GameProgress
    {
        get { return m_GameProgress; }
    }

    public bool IsPlaying1
    {
        get
        {
            return m_IsPlaying;
        }

        set
        {
            m_IsPlaying = value;
        }
    }


    public int PlayLevelIndex
    {
        get { return m_PlayLevelIndex; }
    }

    public void EndLevel(bool isSuccess)
    {
        if (isSuccess && PlayLevelIndex > GameProgress)
        {
            //重新获取
            m_GameProgress = PlayLevelIndex;

            //保存
            //Saver.SetProgress(PlayLevelIndex);
        }

        //游戏停止状态
        IsPlaying1 = false;
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

    public int Gold
    {
        get { return m_gold; }
        set
        {
            m_gold = value;
            SendEvent(Consts.E_Gold, value);
        }
    }

    public int LevelCount { get { return m_Levels.Count; } }



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

        aft.FillLevel((level)=> { m_Levels.Add(level); });

        //List<FileInfo> files = Tools.GetLevelFiles();
        //for (int i = 0; i < files.Count; i++)
        //{
        //    Level level = new Level();
        //    Tools.FillLevel(files[i].FullName, ref level);
        //    levels.Add(level);
        //}

        //m_Levels = levels;

        //读取游戏进度
        //m_GameProgress = Saver.GetProgress();
        //m_GameProgress = 0;
    }
#endregion
}
