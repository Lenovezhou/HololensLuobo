using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBoard : View
{
    #region 字段
    public Text txtScore;
    public Image imgRoundInfo;
    public Text txtCurrent;
    public Text txtTotal;
    public Image imgPauseInfo;
    public Button btnSpeed1;
    public Button btnSpeed2;
    public Button btnResume;
    public Toggle togglePause;
    public Button btnSystem;

    bool m_IsPlaying = false;
    GameSpeed m_Speed = GameSpeed.One;
    int m_Score = 0;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Board; }
    }

    public int Score
    {
        get { return m_Score; }
        set
        {
            m_Score = value;
            txtScore.text = value.ToString();
        }
    }

    public bool IsPlaying
    {
        get { return m_IsPlaying; }
        set
        {
            m_IsPlaying = value;

            imgRoundInfo.gameObject.SetActive(value);
            imgPauseInfo.gameObject.SetActive(!value);
        }
    }

    public GameSpeed Speed
    {
        get { return m_Speed; }
        set
        {
            m_Speed = value;

            btnSpeed1.gameObject.SetActive(m_Speed == GameSpeed.One);
            btnSpeed2.gameObject.SetActive(m_Speed == GameSpeed.Two);
        }
    }
    #endregion

    #region 方法
    public void UpdateRoundInfo(int currentRound, int totalRound)
    {
        txtCurrent.text = currentRound.ToString("D2");//始终保留2位整数
        txtTotal.text = totalRound.ToString();
    }
    #endregion

    #region Unity回调
    void Awake()
    {
        this.Score = 0;
        this.IsPlaying = true;
        this.Speed = GameSpeed.One;
    }
    #endregion

    #region 事件回调
    public void OnSpeed1Click()
    {
        Speed = GameSpeed.Two;
    }

    public void OnSpeed2Click()
    {
        Speed = GameSpeed.One;
    }

    public void OnPauseClick(bool ison)
    {
        IsPlaying = ison;
        PauseResumGameArgs pr = new PauseResumGameArgs { show = false, pausegame = !ison };
        SendEvent(Consts.E_UISystem, pr);
    }

    public void OnResumeClick()
    {
        IsPlaying = true;
        PauseResumGameArgs pr = new PauseResumGameArgs { show = false, pausegame = false };
        SendEvent(Consts.E_UISystem,pr);
    }

    public void OnSystemClick()
    {
        IsPlaying = false;
        PauseResumGameArgs pr = new PauseResumGameArgs { show = true, pausegame = true };
        SendEvent(Consts.E_UISystem,pr);
    }

    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_UISystem);
        AttentionEvents.Add(Consts.E_Gold);
    }

    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case Consts.E_UISystem:
                PauseResumGameArgs pr = data as PauseResumGameArgs;
                togglePause.isOn = !pr.pausegame;
                break;
            case Consts.E_Gold:
                int gold = (int)data;
                Score = gold;
                break;
            default:
                break;
        }
    }
    #endregion
}