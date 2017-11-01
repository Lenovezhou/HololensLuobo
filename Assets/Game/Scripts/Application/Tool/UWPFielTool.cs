
using UnityEngine;
using System.Text;
using System.Collections.Generic;
using System.IO;


//利用宏定义区分Unity3D与UWP的引用空间
#if NETFX_CORE
using Windows.Data.Xml.Dom;
using Windows.Storage;
using XmlReader = WinRTLegacy.Xml.XmlReader;
using XmlWriter = WinRTLegacy.Xml.XmlWriter;
using StreamWriter = WinRTLegacy.IO.StreamWriter;
using StreamReader = WinRTLegacy.IO.StreamReader;
#else
using System.Xml;
using XmlReader = System.Xml.XmlReader;
using XmlWriter = System.Xml.XmlWriter;
using StreamWriter = System.IO.StreamWriter;
using StreamReader = System.IO.StreamReader;
#endif


public class UWPFielTool : AbstractFielTool
{
   
    public override void FillLevel(ref List<Level> levels)
    {
        for (int i = 0; i < 5; i++)
        {
            string path ="level" + i + ".xml";//"file://"+ Consts.LevelDir + @"\level" + i + ".xml";
            Level l = new Level();
#if NETFX_CORE
            ReadData(path,l);
#endif
            levels.Add(l);
        }
    }

#if NETFX_CORE

    //private async void LoadLoalXML(string filename, Level l)
    //{
    //    StorageFolder docLib = await KnownFolders.DocumentsLibrary.GetFolderAsync("Data");
    //    StorageFile docFile = await docLib.GetFileAsync(filename);
    //    string s;
    //    using (Stream fs = await docFile.OpenStreamForReadAsync())
    //    {
    //        byte[] byData = new byte[fs.Length];
    //        fs.Read(byData, 0, (int)fs.Length);
    //        s = System.Text.Encoding.UTF8.GetString(byData);
    //    }
    //    Read(s,ref l);
    //}



    /// <summary>
    ///Hololens读取浏览器上的文件
    /// </summary>
    private async void ReadData(string fielname,Level l)
    {
        StorageFolder docLib = ApplicationData.Current.LocalFolder;
        // var docFile = docLib.OpenStreamForReadAsync("\\Test20170815.txt");
        // 获取应用程序数据存储文件夹

        Stream stream = await docLib.OpenStreamForReadAsync("\\" + fielname);

        // 获取指定的文件的文本内容
        byte[] content = new byte[stream.Length];
        await stream.ReadAsync(content, 0, (int)stream.Length);

        string result = Encoding.UTF8.GetString(content, 0, content.Length);

        Read(result,ref l);

    }



    void Read(string _xml ,ref Level level)
    {

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(_xml);

        level.Name = doc.SelectSingleNode("/Level/Name").InnerText;
        level.CardImage = doc.SelectSingleNode("/Level/CardImage").InnerText;
        level.Background = doc.SelectSingleNode("/Level/Background").InnerText;
        level.Road = doc.SelectSingleNode("/Level/Road").InnerText;
        level.InitScore = int.Parse(doc.SelectSingleNode("/Level/InitScore").InnerText);

        XmlNodeList nodes;

        nodes = doc.SelectNodes("/Level/Holder/Point");
        for (int i = 0; i < nodes.Count; i++)
        {
            IXmlNode node = nodes[i];
            int x = 0, y = 0;
            foreach (var attribute in node.Attributes)
            {
                if (attribute.NodeName == "X")
                {
                    x = int.Parse(attribute.InnerText);
                }

                if (attribute.NodeName == "Y")
                {
                    y = int.Parse(attribute.InnerText);
                }
            }

            Point p = new Point(x,y);

            level.Holder.Add(p);
        }

        nodes = doc.SelectNodes("/Level/Path/Point");
        for (int i = 0; i < nodes.Count; i++)
        {
            IXmlNode node = nodes[i];
            int x = 0, y = 0;
            foreach (var attribute in node.Attributes)
            {
                if (attribute.NodeName == "X")
                {
                    x = int.Parse(attribute.InnerText);
                }

                if (attribute.NodeName == "Y")
                {
                    y = int.Parse(attribute.InnerText);
                }
            }

            Point p = new Point(x,y);

            level.Path.Add(p);
        }

        nodes = doc.SelectNodes("/Level/Rounds/Round");
        for (int i = 0; i < nodes.Count; i++)
        {
            IXmlNode node = nodes[i];
            int x = 0, y = 0;
            foreach (var attribute in node.Attributes)
            {
                if (attribute.NodeName == "Monster")
                {
                    x = int.Parse(attribute.InnerText);
                }

                if (attribute.NodeName == "Count")
                {
                    y = int.Parse(attribute.InnerText);
                }
            }

            Round r = new Round(x,y);

            level.Rounds.Add(r);
        }
    }



#endif


}
