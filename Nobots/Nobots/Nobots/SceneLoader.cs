using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.XPath;
using System.Xml;

namespace Nobots
{
    class SceneLoader
    {
        FileStream stream;
        XPathDocument document;
        XPathNavigator navigator;
        //XPathNodeIterator node;

        public SceneLoader()
        {
            stream = new FileStream("Content/levels/level1.xml", FileMode.Open);
            document = new XPathDocument(stream);
            navigator = document.CreateNavigator();
            //node = navigator.Select("xml/entry");
        }

        public String SceneToXml(Scene scene)
        {
            String xml = "<Scene>\n";
            foreach (Element i in scene.Elements)
            {
                if (i as Platform != null)
                    xml += "    " + ElementToXml((Platform)i) + "\n";
            }
            xml += "</Scene>\n";

            Console.WriteLine(xml);
            return xml;
        }

        public String ElementToXml(Platform platform)
        {           
            String xml = "<Platform Id=\"" + platform.Id + "\" Position=\"" + platform.Position.X + "," + platform.Position.X + "\" Width=\"" + platform.Width + "\" Height=\"" + platform.Height + "\" Rotation=\"" + platform.Rotation + " \"/>";
            return xml;
        }

        public String ElementToXml(Box box)
        {
            String xml = "";
            return xml;
        }

        public String ElementToXml(ElectricityBox eBox)
        {
            String xml = "";
            return xml;
        }

        public String ElementToXml(Elevator elevator)
        {
            String xml = "";
            return xml;
        }

        public String ElementToXml(Forklift forklift)
        {
            String xml = "";
            return xml;
        }

        public String ElementToXml(Ladder ladder)
        {
            String xml = "";
            return xml;
        }

        public String ElementToXml(LaserBarrier laserBarrier)
        {
            String xml = "";
            return xml;
        }

        public String ElementToXml(PressurePlate pressurePlate)
        {
            String xml = "";
            return xml;
        }

        public String ElementToXml(Socket socket)
        {
            String xml = "";
            return xml;
        }
    }
}
