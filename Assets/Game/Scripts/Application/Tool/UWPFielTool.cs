
using UnityEngine;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using UnityEngine.UI;
using System;


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
    List<Level> levels = new List<Level>();

    public override void FillLevel(Action<Level> callback)
    {
#if NETFX_CORE
        for (int i = 0; i < 5; i++)
        {
            ReadData("level" + i + ".xml", callback);
        }
#endif
    }

#if NETFX_CORE

    private async void ReadData(string fielname, Action<Level> callback)
    {
        StorageFolder docLib = ApplicationData.Current.LocalFolder;
        Stream stream = await docLib.OpenStreamForReadAsync("\\" + fielname);
        // 获取指定的文件的文本内容
        byte[] content = new byte[stream.Length];
        await stream.ReadAsync(content, 0, (int)stream.Length);
        stream.Dispose();
        string result = Encoding.UTF8.GetString(content, 0, content.Length);

        ReadXML(result, callback);
    }

    void ReadXML(string _xml, Action<Level> callback)
    {
        XmlDocument doc = new XmlDocument();
        string str = _xml;
        doc.LoadXml(str);

        Level level = new Level
        {
            Name = doc.SelectSingleNode("/Level/Name").InnerText,
            CardImage = doc.SelectSingleNode("/Level/CardImage").InnerText,
            Background = doc.SelectSingleNode("/Level/Background").InnerText,
            Road = doc.SelectSingleNode("/Level/Road").InnerText,
            InitScore = int.Parse(doc.SelectSingleNode("/Level/InitScore").InnerText)
        };
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
                else
                {
                    y = int.Parse(attribute.InnerText);
                }
            }
            Point p = new Point(x, y);
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
                else
                {
                    y = int.Parse(attribute.InnerText);
                }
            }
            Point p = new Point(x, y);
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
                else
                {
                    y = int.Parse(attribute.InnerText);
                }
            }
            Round r = new Round(x, y);



            level.Rounds.Add(r);

        }

        callback?.Invoke(level);

    }
#endif



    public override void LoadImage(string url, Action<Sprite> callback)
    {

        string path = url.Split('.')[0];

        Sprite sp = Resources.Load<Sprite>(path);

        callback(sp);
    }

}
