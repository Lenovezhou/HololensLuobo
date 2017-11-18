using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPopUp : View {
    #region 字段
    public SpawnPanel CreatePanel;
    public UpgradePanel UpgradePanel;
    #endregion

    #region 属性
    public override string Name
    {
        get
        {
            return Consts.V_TowerCreator;
        }
    }


    private static TowerPopUp m_Instance = null;

    public static TowerPopUp Instance {
        get
        {
            return m_Instance;
        }
    }

    public bool IsPopShow
    {
        get
        {
            SpriteRenderer[] sprs = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < sprs.Length; i++)
            {
                if (sprs[i].enabled)
                {
                    return true;
                }
            }
            return false;
        }
    }


    #endregion


    #region Unity回调
    private void Awake()
    {
        m_Instance = this;
    }
    #endregion

    #region 方法
    public void ShowCreatePanel(Vector3 position, bool upSide)
    {
        HideAllPanels();

        GameModel gm = GetModel<GameModel>();
        CreatePanel.Show(gm, position, upSide);
    }

    public void ShowUpgradePanel(Tower tower)
    {
        HideAllPanels();

        GameModel gm = GetModel<GameModel>();
        UpgradePanel.Show(gm, tower);
    }

    public void HideAllPanels()
    {
        CreatePanel.Hide();
        UpgradePanel.Hide();
    }

    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_EnterScene);
        AttentionEvents.Add(Consts.E_ShowCreate);
        AttentionEvents.Add(Consts.E_ShowUpgrade);
        AttentionEvents.Add(Consts.E_HidePopup);
    }

    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case Consts.E_ShowCreate:
                ShowCreateArgs e1 = data as ShowCreateArgs;
                ShowCreatePanel(e1.Position, e1.UpSide);
                break;
            case Consts.E_ShowUpgrade:
                ShowUpgradeArgs e2 = data as ShowUpgradeArgs;
                ShowUpgradePanel(e2.Tower);
                break;
            case Consts.E_HidePopup:
                HideAllPanels();
                break;
            case Consts.E_EnterScene:
                SceneArgs sa = data as SceneArgs;
                if (sa.SceneIndex == 3)
                {
                    Canvas[] cans = GetComponentsInChildren<Canvas>();
                    for (int i = 0; i < cans.Length; i++)
                    {
                        cans[i].worldCamera = Camera.main;
                    }
                }
                break;
        }

    }

     
    void SpawnTower(SpawnTowerArgs e)
    {
        //HideAllPanels();
        SendEvent(Consts.E_SpawnTower, e);
    }

    void UpgradeTower(UpgradeTowerArgs e)
    {
        e.tower.Level++;
        //HideAllPanels();
        //SendEvent(Consts.E_UpgradeTower, e);
    }

    void SellTower(SellTowerArgs e)
    {
        //HideAllPanels();
        SendEvent(Consts.E_SellTower, e);
    }
    #endregion

}
