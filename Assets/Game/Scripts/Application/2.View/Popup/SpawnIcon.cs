using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using HoloToolkit.Unity.InputModule;

public class SpawnIcon : MonoBehaviour, IInputClickHandler
{
    [SerializeField]
    SpriteRenderer m_Render;
    TowerInfo m_Info;
    Vector3 m_CreatePosition;
    bool m_Enough = false;
    private Action callback;
    private BoxCollider selfcollider;

    void Awake()
    {
        m_Render = GetComponent<SpriteRenderer>();
        selfcollider = GetComponent<BoxCollider>();
    }


    public void Load(GameModel gm, TowerInfo info, Vector3 createPosition, bool upSide,Action call = null)
    {

        m_Info = info;

        m_CreatePosition = createPosition;

        //是否足够
        m_Enough = gm.Gold > info.BasePrice;
        m_Enough = true;
        //图标
        string path = "Res/Roles/" + (m_Enough ? info.NormalIcon : info.DisabledIcon);
        m_Render.sprite = Resources.Load<Sprite>(path);

        //本地位置
        Vector3 pos = transform.localPosition;
        pos.y = upSide ? Mathf.Abs(pos.y) : -Mathf.Abs(pos.y);
        transform.localPosition = pos;

        this.callback = call;
        Show();
        
    }

    public void Hide()
    {
        m_Render.enabled = false;
        selfcollider.enabled = false;
    }

    private void Show()
    {
        m_Render.enabled = true;
        selfcollider.enabled = true;
    }



    public void OnMouseDown()
    {
#if !NETFX_CORE

        SpawnTowerArgs e = new SpawnTowerArgs()
        {
            Position = m_CreatePosition,
            TowerID = m_Info.ID
        };
        //太消耗
        SendMessageUpwards("SpawnTower", e, SendMessageOptions.DontRequireReceiver);
#endif  
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
#if NETFX_CORE

        SpawnTowerArgs e = new SpawnTowerArgs()
        {
            Position = m_CreatePosition,
            TowerID = m_Info.ID
        };
        //太消耗
        SendMessageUpwards("SpawnTower", e, SendMessageOptions.DontRequireReceiver);
        if (null != callback)
            callback();
#endif
    }
}
