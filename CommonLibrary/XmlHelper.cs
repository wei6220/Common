using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CommonLibrary
{
    /// <summary>
    /// Xml讀寫元件5
    /// </summary>
    public class XmlHelper
    {
        private XmlDocument xmlDoc;

        /// <summary>
        /// 讀取xml檔案，並轉出xml string
        /// </summary>
        /// <param name="Path">檔案路徑</param>
        /// <returns>xml string</returns>
        public string Load(string Path)
        {
            xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(Path);
            }
            catch (Exception)
            {
                LogHelper.Write("[XmlHelper]Xml file load error!!");
            }
            return xmlDoc.InnerXml;
        }
        /// <summary>
        /// 讀取xml檔案，並轉出list (根結點)
        /// </summary>
        /// <param name="Path">檔案路徑</param>
        /// <returns>list</returns>
        public IList Read(string Path)
        {
            return Read(Path, null);
        }
        /// <summary>
        /// 讀取xml檔案，並轉出list
        /// </summary>
        /// <param name="Path">檔案路徑</param>
        /// <param name="xPath">自訂節點</param>
        /// <returns>list</returns>
        public IList Read(string Path, string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                xPath = "/";
            xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(Path);
            }
            catch (Exception ex)
            {
                LogHelper.Write("[XmlHelper] Read error!!");
                LogHelper.Write(ex.Message);
                throw new Exception(ex.Message);
            }
            XmlNodeList nodes = xmlDoc.SelectNodes(xPath);
            return (IList)ResolveNode(nodes);
        }
        /// <summary>
        /// 讀取xml字串，並轉出list
        /// </summary>
        /// <param name="XmlStr">xml字串</param>
        /// <param name="XPath">自訂節點</param>
        /// <returns>list</returns>
        public IList ReadXml(string XmlStr, string XPath)
        {
            if (string.IsNullOrEmpty(XPath))
                XPath = "/";
            xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(XmlStr);
            }
            catch (Exception ex)
            {
                LogHelper.Write("[XmlHelper] ReadXml error!!");
                throw new Exception(ex.Message);
            }
            XmlNodeList nodes = xmlDoc.SelectNodes(XPath);
            return (IList)ResolveNode(nodes);
        }
        /// <summary>
        /// 讀取xml字串，並轉出xml字串
        /// </summary>
        /// <param name="XmlStr">xml字串</param>
        /// <param name="XPath">自訂節點</param>
        /// <returns>string</returns>
        public string ExtractXml(string XmlStr, string XPath)
        {
            if (string.IsNullOrEmpty(XPath))
                return XmlStr;

            xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(XmlStr);
            }
            catch (Exception)
            {
                LogHelper.Write("[XmlHelper]ExtractXml error!!");
            }
            XmlNodeList nodes = xmlDoc.SelectNodes(XPath);
            string xml = "";
            foreach (XmlNode node in nodes)
                xml += node.InnerXml;
            return xml;
        }

        private object ResolveNode(XmlNodeList nodes)
        {
            List<object> NodeList = new List<object>();

            foreach (XmlNode node in nodes)
            {
                //TODO:處理屬性
                //XmlElement element = (XmlElement)node; 
                string NodeName = node.Name;
                object NodeValue;
                if (node.HasChildNodes)
                    NodeValue = ResolveNode(node.ChildNodes);
                else
                    return node.Value;
                NodeList.Add(new KeyValuePair<string, object>(NodeName, NodeValue));
            }
            return NodeList;
        }
        
        /// <summary>
        /// 找尋列表中的節點 
        /// </summary>
        /// <param name="TargetList"></param>
        /// <param name="TargetValue"></param>
        /// <returns></returns>
        public string Find(IList TargetList, string TargetValue)
        {
            foreach (KeyValuePair<string, object> item in TargetList)
            {
                if (item.Value.GetType() == typeof(string))
                {
                    if (string.Compare(item.Key.ToString(), TargetValue, true) == 0)
                        return item.Value.ToString();
                }
                else
                {
                    return Find((IList)item.Value, TargetValue);
                }
            }
            return "";
        }

    }
}
