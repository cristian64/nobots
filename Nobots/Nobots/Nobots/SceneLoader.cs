using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.XPath;
using System.Xml;

namespace Nobots
{
    public class SceneLoader
    {
        public SceneLoader()
        {
        }

        public String SceneToXml(Scene scene)
        {
            String xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n";
            xml += "<Elements>\n";
            foreach (Element i in scene.Elements)
            {
                if (i as Platform != null)
                    xml += "    " + ElementToXml((Platform)i) + "\n";
                else if (i as Box != null)
                    xml += "    " + ElementToXml((Box)i) + "\n";
                else if (i as Ladder != null)
                    xml += "    " + ElementToXml((Ladder)i) + "\n";
                else if (i as LaserBarrier != null)
                    xml += "    " + ElementToXml((LaserBarrier)i) + "\n";
                else if (i as PressurePlate != null)
                    xml += "    " + ElementToXml((PressurePlate)i) + "\n";
                else if (i as Socket != null)
                    xml += "    " + ElementToXml((Socket)i) + "\n";
                else if (i as Elevator != null)
                    xml += "    " + ElementToXml((Elevator)i) + "\n";
                else if (i as ElectricityBox != null)
                    xml += "    " + ElementToXml((ElectricityBox)i) + "\n";
                else if (i as Character != null)
                    xml += "    " + ElementToXml((Character)i) + "\n";
                else if (i as Forklift != null)
                    xml += "    " + ElementToXml((Forklift)i) + "\n";
                else
                    throw new NotImplementedException(i.GetType().Name + " is still pendent to be converted into XML");
            }
            xml += "</Elements>\n";

            Console.WriteLine(xml);
            return xml;
        }

        public String ElementToXml(Platform platform)
        {           
            String xml = "<Platform Id=\"" + platform.Id + "\" Position=\"" + platform.Position.X + "," + platform.Position.Y + "\" Width=\"" + platform.Width + "\" Height=\"" + platform.Height + "\" Rotation=\"" + platform.Rotation + "\" />";
            return xml;
        }

        public String ElementToXml(Box box)
        {
            String xml = "<Box Id=\"" + box.Id + "\" Position=\"" + box.Position.X + "," + box.Position.Y + "\" Rotation=\"" + box.Rotation + "\" />";
            return xml;
        }

        public String ElementToXml(Character character)
        {
            String xml = "<Character Id=\"" + character.Id + "\" Position=\"" + character.Position.X + "," + character.Position.Y + "\" />";
            return xml;
        }

        public String ElementToXml(ElectricityBox eBox)
        {
            Element e = eBox.activableElement as Element;
            String xml = "<ElectricityBox Id=\"" + eBox.Id + "\" Position=\"" + eBox.Position.X + "," + eBox.Position.Y + "\" ActivableElement=\"" + (e != null ? e.Id : "") + "\" />";
            return xml;
        }

        public String ElementToXml(Elevator elevator)
        {
            String xml = "<Elevator Id=\"" + elevator.Id + "\" Position=\"" + elevator.Position.X + "," + elevator.Position.Y + "\" FinalPosition=\"" + elevator.FinalPosition + "\" Active=\"" + elevator.Active + "\" />";
            return xml;
        }

        public String ElementToXml(Forklift forklift)
        {
            String xml = "<Forklift Id=\"" + forklift.Id + "\" Position=\"" + forklift.Position.X + "," + forklift.Position.Y + "\" Active=\"" + forklift.Active + "\" />";
            return xml;
        }

        public String ElementToXml(Ladder ladder)
        {
            String xml = "<Ladder Id=\"" + ladder.Id + "\" Position=\"" + ladder.Position.X + "," + ladder.Position.Y + "\" StepsNumber=\"" + ladder.StepsNumber + "\" />";
            return xml;
        }

        public String ElementToXml(LaserBarrier laserBarrier)
        {
            String xml = "<LaserBarrier Id=\"" + laserBarrier.Id + "\" Position=\"" + laserBarrier.Position.X + "," + laserBarrier.Position.Y + "\" Rotation=\"" + laserBarrier.Rotation + "\" Width=\"" + laserBarrier.Width + "\" Height=\"" + laserBarrier.Height + "\" Active=\"" + laserBarrier.Active + "\" />";
            return xml;
        }

        public String ElementToXml(PressurePlate pressurePlate)
        {
            Element e = pressurePlate.activableElement as Element;
            String xml = "<PressurePlate Id=\"" + pressurePlate.Id + "\" Position=\"" + pressurePlate.Position.X + "," + pressurePlate.Position.Y + "\" ActivableElement=\"" + (e != null ? e.Id : "") + "\" />";
            return xml;
        }

        public String ElementToXml(Socket socket)
        {
            String xml = "<Socket Id=\"" + socket.Id + "\" Position=\"" + socket.Position.X + "," + socket.Position.Y + "\" OtherSocket=\"" + (socket.OtherSocket != null ? socket.OtherSocket.Id : "") + "\" />";
            return xml;
        }
    }
}
