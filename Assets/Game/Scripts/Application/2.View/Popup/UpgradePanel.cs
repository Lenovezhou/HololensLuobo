using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour {
    #region 字段
    UpgradeIcon m_UpgradeIcon;
    SellIcon m_SellIcon;
    #endregion


    void Awake()
    {
        m_UpgradeIcon = GetComponentInChildren<UpgradeIcon>();
        m_SellIcon = GetComponentInChildren<SellIcon>();
    }


    #region 方法
    public void Show(GameModel gm, Tower tower)
    {
        gameObject.SetActive(true);

        transform.position = tower.transform.position;

        m_UpgradeIcon.Load(gm, tower);
        m_SellIcon.Load(tower);

    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    #endregion
}
