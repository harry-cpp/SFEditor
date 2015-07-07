using System.Xml;

namespace SFEditor
{
    class CharacterRegion
    {
        public int Start, End;

        public CharacterRegion(int start, int end)
        {
            this.Start = start;
            this.End = end;
        }

        public void Save(XmlDocument doc, XmlNode root)
        {
            var node = doc.CreateElement("CharacterRegion");

            var startnode = doc.CreateElement("Start");
            startnode.InnerText = ((char)Start).ToString();
            node.AppendChild(startnode);

            var endnode = doc.CreateElement("End");
            endnode.InnerText = ((char)End).ToString();
            node.AppendChild(endnode);

            root.AppendChild(node);
        }
    }
}
