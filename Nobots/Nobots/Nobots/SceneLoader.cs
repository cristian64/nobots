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
        public string ElementToXML(Platform platform)
        {
            navigator.MoveToFirstChild();
            navigator.MoveToFirstChild();
            navigator.MoveToNext();
            navigator.MoveToFirstAttribute();
           
            Console.WriteLine("platform " + navigator.Value);
           
            return "<platform id=\"" + platform.Id + "\" x=\"" + platform.Position.X + "\" y=\"" + platform.Position.Y + "\" width=\"" + platform.Width + "\" height=\"" + platform.Height + "\" rotation=\"" + platform.Rotation + "\"/>"; 
        }
/*
        public string ElementToXML(Box box)
        {
        }

        public string ElementToXML(ElectricityBox eBox)
        {
        }

        public string ElementToXML(Elevator elevator)
        {
        }

        public string ElementToXML(Forklift forklift)
        {
        }

        public string ElementToXML(Ladder ladder)
        {
        }

        public string ElementToXML(LaserBarrier laserBarrier)
        {
        }

        public string ElementToXML(PressurePlate pressurePlate)
        {
        }

        public string ElementToXML(Socket socket)
        {
        }
 **/
    }
}
