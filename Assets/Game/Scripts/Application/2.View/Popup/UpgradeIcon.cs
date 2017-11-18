using HoloToolkit.Unity.InputModule;
using System;
using UnityEngine;

public class UpgradeIcon : MonoBehaviour,IInputClickHandler
{
    SpriteRenderer m_Render;
    Tower m_Tower;
    BoxCollider selfbox;
    void Awake()
    {
        m_Render = GetComponent<SpriteRenderer>();
        selfbox = GetComponent<BoxCollider>();
    }

    public void Load(GameModel gm, Tower tower)
    {
        m_Render.enabled = true;
        selfbox.enabled = true;
        m_Tower = tower;

        //图标
        TowerInfo info = Game.Instance._StaticDate.GetTowerInfo(tower.ID);
        string path = "Res/Roles/" + (tower.IsTopLevel ? info.DisabledIcon : info.NormalIcon);
        m_Render.sprite = Resources.Load<Sprite>(path);
    }

    public void OnMouseDown()
    {
 #if !NETFX_CORE

        if (m_Tower.IsTopLevel)
            return;

        UpgradeTowerArgs e = new UpgradeTowerArgs()
        {
            tower = m_Tower
        };
        SendMessageUpwards("UpgradeTower", e, SendMessageOptions.DontRequireReceiver);
#endif
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
#if NETFX_CORE

        if (m_Tower.IsTopLevel)
            return;

        UpgradeTowerArgs e = new UpgradeTowerArgs()
        {
            tower = m_Tower
        };
        SendMessageUpwards("UpgradeTower", e, SendMessageOptions.DontRequireReceiver);
#endif
    }

}
