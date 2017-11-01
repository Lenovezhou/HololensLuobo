using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//鼠标点击参数类
public class TileClickEventArgs : EventArgs
{
    public int MouseButton; //0左键，1右键
    public Tile Tile;

    public TileClickEventArgs(int mouseButton, Tile tile)
    {
        this.MouseButton = mouseButton;
        this.Tile = tile;
    }
}

public class MapController : MonoBehaviour {


    #region 常量
    public const int RowCount = 8;  //行数
    public const int ColumnCount = 12; //列数
    #endregion

    #region 字段
    public bool DrawGizmos = true; //是否绘制网格


    [SerializeField]
    SpriteRenderer background;
    [SerializeField]
    SpriteRenderer road;

    List<Tile> m_grid = new List<Tile>(); //格子集合
    List<Tile> m_road = new List<Tile>(); //路径集合

    Level m_level; //关卡数据



    float MapWidth;//地图宽
    float MapHeight;//地图高

    float TileWidth;//格子宽
    float TileHeight;//格子高

    #endregion


    #region 属性
    //怪物的寻路路径
    public Vector3[] Path
    {
        get
        {
            List<Vector3> m_path = new List<Vector3>();
            for (int i = 0; i < m_road.Count; i++)
            {
                Tile t = m_road[i];
                Vector3 point = GetPosition(t);
                m_path.Add(point);
            }
            return m_path.ToArray();
        }
    }


    public Level Level
    {
        get { return m_level; }
    }

    #endregion

    #region 方法


    public void LoadLevel(Level level)
    {


        Clear();


        RefreshSprite(level.Background, background);
        RefreshSprite(level.Road, road);

        //寻路点
        for (int i = 0; i < level.Path.Count; i++)
        {
            Point p = level.Path[i];
            Tile t = GetTile(p.X, p.Y);
            m_road.Add(t);
        }
        //炮塔点
        for (int i = 0; i < level.Holder.Count; i++)
        {
            Point p = level.Holder[i];
            Tile t = GetTile(p.X, p.Y);
            t.CanHold = true;
        }
    }


    void RefreshSprite(string _path, SpriteRenderer root)
    {
        string path = "Res/Maps/" + _path;
        string path1 = path.Split('.')[0];
        Sprite s1 = Resources.Load<Sprite>(path1);
        root.sprite = Instantiate(s1);
    }

    //清除所有信息
    public void Clear()
    {
        m_level = null;
        ClearHolder();
        ClearRoad();
    }


    //清除塔位信息
    public void ClearHolder()
    {
        foreach (Tile t in m_grid)
        {
            if (t.CanHold)
                t.CanHold = false;
        }
    }

    //清除寻路格子集合
    public void ClearRoad()
    {
        m_road.Clear();
    }



    //根据格子索引号获得格子
    public Tile GetTile(int tileX, int tileY)
    {
        int index = tileX + tileY * ColumnCount;
        if (index < 0 || index >= m_grid.Count)
            throw new IndexOutOfRangeException("格子索引越界");
        return m_grid[index];
    }

    //获取所在位置获得格子
    public Tile GetTile(Vector3 position)
    {
        int tileX = (int)((position.x + MapWidth / 2) / TileWidth);
        int tileY = (int)((position.y + MapHeight / 2) / TileHeight);
        return GetTile(tileX, tileY);
    }


    void CalculateSize()
    {
        //Vector3 leftDown = new Vector3(0, 0);
        //Vector3 rightUp = new Vector3(1, 1);

        //Vector3 p1 = Camera.main.ViewportToWorldPoint(leftDown);
        //Vector3 p2 = Camera.main.ViewportToWorldPoint(rightUp);

        //MapWidth = Math.Abs(p2.x - p1.x);
        //MapHeight = Math.Abs(p2.y - p1.y);
        Vector2 size = background.GetComponent<Renderer>().bounds.size;

        MapWidth = size.x;
        MapHeight = size.y;

        TileWidth = MapWidth / ColumnCount;
        TileHeight = MapHeight / RowCount;
    }


    //获取格子中心点所在的世界坐标
    public Vector3 GetPosition(Tile t)
    {
        Vector3 pos = background.transform.position;
        //return pos;
        return new Vector3(
                -MapWidth / 2 + (t.X + 0.5f) * TileWidth,
                -MapHeight / 2 + (t.Y + 0.5f) * TileHeight,
                background.transform.position.z
            );
    }

    void Map_OnTileClick(object sender, TileClickEventArgs e)
    {
        //当前场景不是LevelBuilder不能编辑
        if (gameObject.scene.name != "LevelBuilder")
            return;

        if (Level == null)
            return;

        //处理放塔操作
        if (e.MouseButton == 0 && !m_road.Contains(e.Tile))
        {
            e.Tile.CanHold = !e.Tile.CanHold;
        }

        //处理寻路点操作
        if (e.MouseButton == 1 && !e.Tile.CanHold)
        {
            if (m_road.Contains(e.Tile))
                m_road.Remove(e.Tile);
            else
                m_road.Add(e.Tile);
        }
    }


    //获取鼠标下面的格子
    Tile GetTileUnderMouse()
    {
        Vector2 wordPos = GetWorldPosition();
        return GetTile(wordPos);
    }

    //获取鼠标所在位置的世界坐标
    Vector3 GetWorldPosition()
    {
        Vector3 viewPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        return worldPos;
    }

    #endregion

    #region 事件
    public event EventHandler<TileClickEventArgs> OnTileClick;

    #endregion



    #region Unity回调


    void Awake()
    {
        //计算地图和格子大小
        CalculateSize();

        //创建所有的格子
        for (int i = 0; i < RowCount; i++)
            for (int j = 0; j < ColumnCount; j++)
                m_grid.Add(new Tile(j, i));

        //监听鼠标点击事件
        OnTileClick += Map_OnTileClick;
    }



    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TileClickEventArgs args = new TileClickEventArgs(0, GetTileUnderMouse());
            OnTileClick(this, args);
        }

        if (Input.GetMouseButtonDown(1))
        {
            TileClickEventArgs args = new TileClickEventArgs(1, GetTileUnderMouse());
            OnTileClick(this, args);

        }

    }



    //只在编辑器里起作用
    void OnDrawGizmos()
    {
        if (!DrawGizmos)
            return;

        //计算地图和格子大小
        CalculateSize();

        //格子颜色
        Gizmos.color = Color.green;

        //绘制行
        for (int row = 0; row <= RowCount; row++)
        {
            Vector3 from = new Vector3(-MapWidth / 2, -MapHeight / 2 + row * TileHeight,background.transform.position.z);
            Vector3 to = new Vector3(-MapWidth / 2 + MapWidth, -MapHeight / 2 + row * TileHeight,background.transform.position.z);
            Gizmos.DrawLine(from, to);
        }

        //绘制列
        for (int col = 0; col <= ColumnCount; col++)
        {
            Vector3 from = new Vector3(-MapWidth / 2 + col * TileWidth, MapHeight / 2, background.transform.position.z);
            Vector3 to = new Vector3(-MapWidth / 2 + col * TileWidth, -MapHeight / 2, background.transform.position.z);
            Gizmos.DrawLine(from, to);
        }

        //绘制格子
        foreach (Tile t in m_grid)
        {
            if (t.CanHold)
            {
                Vector3 pos = GetPosition(t);
                Gizmos.DrawIcon(pos, "holder.png", true);
            }
        }



        Gizmos.color = Color.red;
        for (int i = 0; i < m_road.Count; i++)
        {
            //起点
            if (i == 0)
            {
                Gizmos.DrawIcon(GetPosition(m_road[i]), "start.png", true);
            }

            //终点
            if (m_road.Count > 1 && i == m_road.Count - 1)
            {
                Gizmos.DrawIcon(GetPosition(m_road[i]), "end.png", true);
            }

            //红色的连线
            if (m_road.Count > 1 && i != 0)
            {
                Vector3 from = GetPosition(m_road[i - 1]);
                Vector3 to = GetPosition(m_road[i]);
                Gizmos.DrawLine(from, to);
            }
        }

    }

        #endregion
    }
