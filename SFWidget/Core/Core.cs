using System.Xml;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFEditor
{
    class Core
    {
        private bool _projectOpened;
        private XmlDocument _xmlDocument;
        public string FileName;

        public bool ProjectOpened
        {
            get
            {
                return _projectOpened;
            }
        }

        public string Font;
        public float Size, Spacing;
        public bool Bold, Italic, Kerning, LocalFont, HasDefChar;
        public char DefaultCharacter;
        public List<CharacterRegion> CharacterRegions;

        public Core()
        {
            CharacterRegions = new List<CharacterRegion>();
            SetDefaults();
        }

        public void SetDefaults()
        {
            using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Resources.font.spritefont"))
            using (var reader = new StreamReader(stream))
            {
                LoadFromText(reader.ReadToEnd());
            }
        }

        public bool Open(string fileName)
        {
            try
            {
                Close();
                SetDefaults();

                this.FileName = fileName;
                _projectOpened = true;

                _xmlDocument = new XmlDocument();
                _xmlDocument.Load(fileName);

                return Open();
            }
            catch
            {
                return false;
            }
        }

        private void RemoveFromXml(string path)
        {
            XmlNode root = _xmlDocument.DocumentElement;
            RemoveFromXml(root, path);
        }

        private void RemoveFromXml(XmlNode rootNode, string path)
        {
            if (path.Contains("/"))
            {
                string[] split_path = path.Split('/');
                string new_path = split_path[1];
                for (int i = 2; i < split_path.Length; i++)
                    new_path += "/" + split_path[i];

                foreach (XmlNode n in rootNode.ChildNodes)
                {
                    if (n.Name == split_path[0])
                    {
                        RemoveFromXml(n, new_path);
                        return;
                    }
                }

                return;
            }
            else
            {
                var ntr = new List<XmlNode>();

                foreach (XmlNode n in rootNode.ChildNodes)
                    if (n.Name == path)
                        ntr.Add(n);

                foreach (XmlNode n in ntr)
                    rootNode.RemoveChild(n);
            }
        }

        private void WriteToXml(string path, string innerText)
        {
            XmlNode root = _xmlDocument.DocumentElement;
            WriteToXml(root, path, innerText);
        }

        private void WriteToXml(XmlNode rootNode, string path, string innerText)
        {
            if (path.Contains("/"))
            {
                string[] split_path = path.Split('/');
                string new_path = split_path[1];
                for (int i = 2; i < split_path.Length; i++)
                    new_path += "/" + split_path[i];

                foreach (XmlNode n in rootNode.ChildNodes)
                {
                    if (n.Name == split_path[0])
                    {
                        WriteToXml(n, new_path, innerText);
                        return;
                    }
                }

                var tn = _xmlDocument.CreateElement(split_path[0]);
                rootNode.AppendChild(tn);
                WriteToXml(tn, new_path, innerText);
            }
            else
            {
                foreach (XmlNode n in rootNode.ChildNodes)
                {
                    if (n.Name == path)
                    {
                        n.InnerText = innerText;
                        return;
                    }
                }

                var tn = _xmlDocument.CreateElement(path);
                rootNode.AppendChild(tn);
                tn.InnerText = innerText;
            }
        }

        private char GetChar(XmlNode node, string path)
        {
            XmlNode n = node.SelectSingleNode(path);
            return GetChar(n.InnerText);
        }

        private char GetChar(string text)
        {
            if (text.Length == 1)
                return text[0];
            else if (text.Length > 1)
                throw new Exception("Not a char");

            return (char)32;
        }

        private string GetStyle()
        {
            string s = "Regular";

            if (Bold)
                s = "Bold";

            if (Italic)
            {
                if (s == "Bold")
                    s += " Italic";
                else
                    s = "Italic";
            }

            return s;
        }

        public void SaveToXml()
        {
            WriteToXml("Asset/FontName", Font);
            WriteToXml("Asset/Size", Size.ToString());
            WriteToXml("Asset/Style", GetStyle());
            WriteToXml("Asset/Spacing", Spacing.ToString());
            WriteToXml("Asset/UseKerning", Kerning.ToString().ToLower());

            if (HasDefChar)
                WriteToXml("Asset/DefaultCharacter", DefaultCharacter.ToString());
            else
                RemoveFromXml("Asset/DefaultCharacter");

            XmlNode root = _xmlDocument.DocumentElement;
            WriteToXml("Asset/CharacterRegions", Kerning.ToString());
            XmlNode crs = root.SelectSingleNode("Asset/CharacterRegions");
            crs.RemoveAll();

            foreach (var n in CharacterRegions)
                n.Save(_xmlDocument, crs);
        }

        public SaveError Save()
        {
            return this.FileName != null ? Save(this.FileName) : SaveError.NoFileNameSpecified;
        }

        public SaveError Save(string fileName)
        {
            try
            {
                SaveToXml();
                _xmlDocument.Save(fileName);

                return SaveError.Nothing;
            }
            catch
            {
                return SaveError.UnknownError;
            }
        }

        public string GetText()
        {
            SaveToXml();
            return XmlToString(_xmlDocument);
        }

        private string XmlToString(XmlDocument doc)
        {
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace
                };
            using (var writer = XmlWriter.Create(sb, settings)) {
                doc.Save(writer);
            }
            return sb.ToString();
        }

        public bool LoadFromText(string text)
        {
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(text);
                this._xmlDocument = xmlDocument;

                return Open();
            }
            catch
            {
                return false;
            }
        }

        public bool Open()
        {
            try
            {
                XmlNode root = _xmlDocument.DocumentElement;

                string tmpFont = root.SelectSingleNode("Asset/FontName").InnerText;
                bool tmpLocalFont = File.Exists(Path.GetDirectoryName(FileName) + Font) || File.Exists(Font);
                if(!tmpLocalFont)
                {
                    var fonts = new List<string>();
                    foreach (System.Drawing.FontFamily font in System.Drawing.FontFamily.Families)
                        fonts.Add(font.Name);

                    if(!fonts.Contains(tmpFont))
                        tmpLocalFont = true;
                }

                float tmpSize = float.Parse(root.SelectSingleNode("Asset/Size").InnerText);
                float tmpSpacing = float.Parse(root.SelectSingleNode("Asset/Spacing").InnerText);
                bool tmpKerning = Convert.ToBoolean(root.SelectSingleNode("Asset/UseKerning").InnerText);

                var _style =  root.SelectSingleNode("Asset/Style").InnerText;
                bool tmpBold = _style.Contains("Bold");
                bool tmpItalic = _style.Contains("Italic");

                var tmpHasDefChar = false;
                var tmpDefString = "";

                var tmpCharRegion = new List<CharacterRegion>();
                var hasCharRegion = false;
                XmlNode crs = null;

                try
                {
                    crs = root.SelectSingleNode("Asset/CharacterRegions");
                    hasCharRegion = true;
                }
                catch 
                {
                }

                if(hasCharRegion)
                {
                    foreach(XmlNode child in crs.ChildNodes)
                    {
                        char start = GetChar(child, "Start");
                        char end = GetChar(child, "End");

                        tmpCharRegion.Add(new CharacterRegion((int)start, (int)end));
                    }
                }

                try
                {
                    tmpDefString = root.SelectSingleNode("Asset/DefaultCharacter").InnerText;
                    tmpHasDefChar = true;
                }
                catch
                {
                }

                if(tmpHasDefChar)
                    DefaultCharacter = GetChar(tmpDefString);
                HasDefChar = tmpHasDefChar;

                Font = tmpFont;
                LocalFont = tmpLocalFont;

                Size = tmpSize;
                Spacing = tmpSpacing;
                Kerning = tmpKerning;

                Bold = tmpBold;
                Italic = tmpItalic;

                CharacterRegions = tmpCharRegion;
            }
            catch 
            {
                return false;
            }

            return true;
        }

        public void Close()
        {
            _projectOpened = false;
            _xmlDocument = null;
        }
    }
}
