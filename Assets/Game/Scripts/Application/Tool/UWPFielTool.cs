
using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.IO;


//利用宏定义区分Unity3D与UWP的引用空间
#if NETFX_CORE
using Windows.Storage;
using XmlReader = WinRTLegacy.Xml.XmlReader;
using XmlWriter = WinRTLegacy.Xml.XmlWriter;
using StreamWriter = WinRTLegacy.IO.StreamWriter;
using StreamReader = WinRTLegacy.IO.StreamReader;
#else
using XmlReader = System.Xml.XmlReader;
using XmlWriter = System.Xml.XmlWriter;
using StreamWriter = System.IO.StreamWriter;
using StreamReader = System.IO.StreamReader;
#endif


public class UWPFielTool:AbstractFielTool
{

    private List<Level> levels = new List<Level>();

//#if NETFX_CORE
//    public async void GetAsyncFiels()
//    {
//        StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
//        StringBuilder outputText = new StringBuilder();

//        IReadOnlyList<IStorageItem> itemsList = await picturesFolder.GetItemsAsync();
//        for (int i = 0; i<itemsList.Length; i++)
//		{
//            Level level = new Level();
//            FillLevel(files[i].FullName, ref level);
//            levels.add(level);
//		}
//    }
//#endif


    public override void FillLevel(ref List<Level> _levels)
    {

#if NETFX_CORE
        awit GetAsyncFiels();
        _levels = level;
#else
        List<FileInfo> files = GetLevelFiles();
        for (int i = 0; i < files.Count; i++)
        {
            Level level = new Level();
            FillLevel(files[i].FullName, ref level);
            _levels.Add(level);
        }
#endif
    }


#if UNITY_EDITOR
    //填充Level类数据
    public override void FillLevel(string fileName, ref Level level)
    {
        //FileInfo file = new FileInfo(fileName);
        //StreamReader sr = new StreamReader(file.OpenRead(), Encoding.UTF8);

        XmlDocument doc = new XmlDocument();
        doc.Load(fileName);
        //doc.Load(sr);

        level.Name = doc.SelectSingleNode("/Level/Name").InnerText;
        level.CardImage = doc.SelectSingleNode("/Level/CardImage").InnerText;
        level.Background = doc.SelectSingleNode("/Level/Background").InnerText;
        level.Road = doc.SelectSingleNode("/Level/Road").InnerText;
        level.InitScore = int.Parse(doc.SelectSingleNode("/Level/InitScore").InnerText);

        XmlNodeList nodes;

        nodes = doc.SelectNodes("/Level/Holder/Point");
        for (int i = 0; i < nodes.Count; i++)
        {
            XmlNode node = nodes[i];
            Point p = new Point(
                int.Parse(node.Attributes["X"].Value),
                int.Parse(node.Attributes["Y"].Value));

            level.Holder.Add(p);
        }

        nodes = doc.SelectNodes("/Level/Path/Point");
        for (int i = 0; i < nodes.Count; i++)
        {
            XmlNode node = nodes[i];

            Point p = new Point(
                int.Parse(node.Attributes["X"].Value),
                int.Parse(node.Attributes["Y"].Value));

            level.Path.Add(p);
        }

        nodes = doc.SelectNodes("/Level/Rounds/Round");
        for (int i = 0; i < nodes.Count; i++)
        {
            XmlNode node = nodes[i];

            Round r = new Round(
                    int.Parse(node.Attributes["Monster"].Value),
                    int.Parse(node.Attributes["Count"].Value)
                );

            level.Rounds.Add(r);
        }

        //sr.Close();
        //sr.Dispose();
    }




    //读取关卡列表
    public override List<FileInfo> GetLevelFiles()
    {

        string[] files = Directory.GetFiles(Consts.LevelDir, "*.xml");

        List<FileInfo> list = new List<FileInfo>();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = new FileInfo(files[i]);
            list.Add(file);
        }
        return list;
    }

#endif

}
