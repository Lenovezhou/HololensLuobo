using System;
using System.Collections.Generic;
using UnityEngine;

class Spawner : View
{
    #region 字段
    MapController m_Map;
    private Luobo m_Luobo;
    #endregion

    public override string Name
    {
        get
        {
            return Consts.V_Spanwner;
        }
    }

    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_EnterScene);
        AttentionEvents.Add(Consts.E_SpawnMonster);
        AttentionEvents.Add(Consts.E_EndLevel);
        AttentionEvents.Add(Consts.E_SpawnTower);
        AttentionEvents.Add(Consts.E_SellTower);
    }

    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case Consts.E_EnterScene:
                SceneArgs e0 = data as SceneArgs;
                if (e0.SceneIndex == 3)
                {
                    m_Map = GetComponent<MapController>();

                    //m_Map.transform.position = Vector3.one * 2;

                    m_Map.OnTileClick += map_OnTileClick;

                    //获取数据
                    GameModel gModel = GetModel<GameModel>();
                    m_Map.LoadLevel(gModel.PlayLevel);


                    //加载萝卜
                    Vector3[] path = m_Map.Path;
                    Vector3 luoboPos = path[path.Length - 1];
                    SpawnLuobo(luoboPos);
                }
                break;
            case Consts.E_SpawnMonster:
                SpawnMonsterArgs e1 = data as SpawnMonsterArgs;
                SpawnMonster(e1.MonsterType);
                break;
            case Consts.E_SpawnTower:
                SpawnTowerArgs e2 = data as SpawnTowerArgs;
                SpawnTower(e2.Position, e2.TowerID);
                break;
            case Consts.E_EndLevel:
                Hide();
                break;
            case Consts.E_SellTower:
                SellTowerArgs t = data as SellTowerArgs;
                Game.Instance._ObjectPool.Unspawn(t.towergameobj);
                GameModel gm = GetModel<GameModel>();
                gm.Gold += t.gold;
                break;
        }
    }

    private void SpawnTower(Vector3 position, int towerID)
    {
        //找到Tile
        Tile tile = m_Map.GetTile(position);

        //创建Tower
        TowerInfo info = Game.Instance._StaticDate.GetTowerInfo(towerID);
        GameObject go = Game.Instance._ObjectPool.Spawn(info.PrefabName);
        Tower tower = go.GetComponent<Tower>();
        tower.transform.position = position;
        tower.Load(towerID, tile, m_Map.MapRect);
        //设置Tile数据
        tile.Data = tower;

    }


    #region 方法
    void SpawnMonster(int MonsterID)
    {
        string prefabName = "Monster" + MonsterID;
        GameObject go = Game.Instance._ObjectPool.Spawn(prefabName);
        Monster monster = go.GetComponent<Monster>();
        monster.Reached += monster_Reached;
        monster.HpChanged += monster_HpChanged;
        monster.Dead += monster_Dead;
        monster.Load(m_Map.Path);
    }


    //创建萝卜
    void SpawnLuobo(Vector3 position)
    {
        GameObject go = Game.Instance._ObjectPool.Spawn("Luobo");
        Luobo luobo = go.GetComponent<Luobo>();
        luobo.Position = position;
        luobo.Dead += luobo_Dead;

        m_Luobo = luobo;
    }


    void luobo_Dead(Role luobo)
    {
        //萝卜回收
        Game.Instance._ObjectPool.Unspawn(luobo.gameObject);

        //游戏结束
        GameModel gm = GetModel<GameModel>();
        SendEvent(Consts.E_EndLevel, new EndLevelArgs() { LevelID = gm.PlayLevelIndex, IsSuccess = false });
    }

    private void monster_HpChanged(int arg1, int arg2)
    {
    }

    private void monster_Reached(Monster obj)
    {
        //萝卜掉血
        m_Luobo.Damage(1);

        //怪物死亡
        obj.Hp = 0;
    }


    void monster_Dead(Role monster)
    {
        //怪物回收
        Game.Instance._ObjectPool.Unspawn(monster.gameObject);

        //胜利条件判断
        RoundModel rm = GetModel<RoundModel>();
        GameModel gm = GetModel<GameModel>();
        Monster m = monster as Monster;
        gm.Gold += m.Price;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        if (monsters.Length == 0        //场景里没有怪物了
            && !m_Luobo.IsDead          //萝卜还活着
            && rm.AllRoundsComplete)    //所有怪物都已出完
        {
            //游戏胜利
            SendEvent(Consts.E_EndLevel, new EndLevelArgs() { LevelID = gm.PlayLevelIndex, IsSuccess = true });
        }
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }



    void map_OnTileClick(object sender, TileClickEventArgs e)
    {

        GameModel gm = GetModel<GameModel>();

        //游戏还未开始，那么不操作菜单
        if (!gm.IsPlaying)
            return;

        //如果有菜单显示，那么隐藏菜单
        if (TowerPopUp.Instance.IsPopShow || (e.Tile == null))
        {
            SendEvent(Consts.E_HidePopup);
            return;
        }

        //非放塔格子，不操作菜单
        if ((null != e.Tile) && !e.Tile.CanHold)
        {
            SendEvent(Consts.E_HidePopup);
            return;
        }
        if (e.Tile.Data == null)
        {
            ShowCreateArgs arg = new ShowCreateArgs()
            {
                Position = m_Map.GetPosition(e.Tile),
                UpSide = e.Tile.Y < Map.RowCount / 2
            };
            SendEvent(Consts.E_ShowCreate, arg);
        }
        else
        {
            ShowUpgradeArgs arg = new ShowUpgradeArgs()
            {
                Tower = e.Tile.Data as Tower
            };
            SendEvent(Consts.E_ShowUpgrade, arg);
        }
    }


    #endregion
}
