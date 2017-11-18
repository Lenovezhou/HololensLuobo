using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelect : View {
    public override string Name
    {
        get
        {
            return Consts.V_Select;
        }
    }

    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_EnterScene);
    }


    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case Consts.E_EnterScene:
                SceneArgs e = data as SceneArgs;
                if (e.SceneIndex == 2)
                {
                    GetComponent<Canvas>().worldCamera = Camera.main;
                    GetComponent<Canvas>().enabled = true;


                    m_GameModel = GetModel<GameModel>();

                    LoadCards();
                }
                break;
            default:
                break;
        }
    }




    #region 字段
    int m_SelectedIndex = -1;
    GameModel m_GameModel;
    List<Card> m_Cards = new List<Card>();
    public Button btnStart;

    #endregion

    #region 方法
    void LoadCards()
    {
        List<Level> levels = m_GameModel.AllLevels;

        List<Card> cards = new List<Card>();

        for (int i = 0; i < levels.Count; i++)
        {
            Card card = new Card()
            {
                LevelID = i,
                CardImage = levels[i].CardImage,
                IsLocked = !(i <= m_GameModel.GameProgress + 1)
            };
            cards.Add(card);
        }
        this.m_Cards = cards;

        //监听卡片点击事件
        UICard[] uiCards = this.transform.Find("Bg/UICards").GetComponentsInChildren<UICard>();
        foreach (UICard uiCard in uiCards)
        {
            uiCard.OnClick += (card) =>
            {
                SelectCard(card.LevelID);
            };
        }

        //默认选中第1个卡片
        SelectCard(0);
    }




    //选择卡牌
    void SelectCard(int index)
    {
        if (m_SelectedIndex == index)
            return;

        m_SelectedIndex = index;

        //计算索引号
        int left = m_SelectedIndex - 1;
        int current = m_SelectedIndex;
        int right = m_SelectedIndex + 1;

        //绑定数据
        Transform container = this.transform.Find("Bg/UICards");

        //左边
        if (left < 0)
        {
            container.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            container.GetChild(0).gameObject.SetActive(true);
            container.GetChild(0).GetComponent<UICard>().IsTransparent = true;
            container.GetChild(0).GetComponent<UICard>().DataBind(m_Cards[left]);
        }

        //当前
        container.GetChild(1).GetComponent<UICard>().IsTransparent = false;
        container.GetChild(1).GetComponent<UICard>().DataBind(m_Cards[current]);

        //当前开始按钮
        btnStart.gameObject.SetActive(!m_Cards[current].IsLocked);

        //右边
        if (right >= m_Cards.Count)
        {
            container.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            container.GetChild(2).gameObject.SetActive(true);
            container.GetChild(2).GetComponent<UICard>().IsTransparent = true;
            container.GetChild(2).GetComponent<UICard>().DataBind(m_Cards[right]);
        }
    }

    #endregion

    #region Button回调
    public void StartBut()
    {
        //测试
        //m_SelectedIndex = m_SelectedIndex ;
        StartLevelArgs e = new StartLevelArgs()
        {
            LevelIndex = m_SelectedIndex
        };

        SendEvent(Consts.E_StartLevel, e);
    }
#endregion

}
