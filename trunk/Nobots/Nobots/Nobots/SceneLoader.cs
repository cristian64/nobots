using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.XPath;
using System.Xml;
using Microsoft.Xna.Framework;
using System.Globalization;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms;
using Nobots.Elements;

namespace Nobots
{
    public class SceneLoader : DrawableGameComponent
    {
        public SceneLoader(Game game)
            : base(game)
        {
        }

        #region FromXml

        public void SceneFromXml(String filename, Scene scene)
        {
            XmlTextReader reader = new XmlTextReader(filename);
            while (reader.Read())
            {
                // Once we find the Backgrounds tag, we start a particular loop for all of them.
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Backgrounds")
                {
                    // We'll stay in this local loop until we find the end of the Backgrounds tag.
                    while (reader.Read() && (reader.NodeType != XmlNodeType.EndElement || reader.Name != "Backgrounds"))
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                            scene.Backgrounds.Add(BackgroundFromXml(reader, scene));
                    }
                }

                // Once we find the Foregrounds tag, we start a particular loop for all of them.
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Foregrounds")
                {
                    // We'll stay in this local loop until we find the end of the Foregrouds tag.
                    while (reader.Read() && (reader.NodeType != XmlNodeType.EndElement || reader.Name != "Foregrounds"))
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                            scene.Foregrounds.Add(BackgroundFromXml(reader, scene));
                    }
                }

                // Once we find the Elements tag, we start a particular loop for all of them.
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Elements")
                {
                    // We'll stay in this local loop until we find the end of the Elements tag.
                    while (reader.Read() && (reader.NodeType != XmlNodeType.EndElement || reader.Name != "Elements"))
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            Element e = ElementFromXml(reader, scene);
                            if (e != null)
                                scene.Elements.Add(e);
                        }
                    }
                }
            }

            reader.Close();
        }

        public Vector2 PositionFromString(string xml)
        {
            return new Vector2(float.Parse(xml.Split(',')[0], CultureInfo.InvariantCulture), float.Parse(xml.Split(',')[1], CultureInfo.InvariantCulture));
        }

        public Background BackgroundFromXml(XmlTextReader reader, Scene scene)
        {
            Background e = new Background(Game, scene);
            if (reader.MoveToAttribute("Id"))
                e.Id = reader.Value;
            if (reader.MoveToAttribute("TextureName"))
                e.TextureName = reader.Value;
            if (reader.MoveToAttribute("Position"))
                e.Position = PositionFromString(reader.Value);
            if (reader.MoveToAttribute("Speed"))
                e.Speed = PositionFromString(reader.Value);
            return e;
        }

        public Element ElementFromXml(XmlTextReader reader, Scene scene)
        {
            Element e = null;
            switch (reader.Name)
            {
                case "Platform":
                    e = new Platform(Game, scene, Vector2.Zero, Vector2.One);
                    break;
                case "Box":
                    e = new Box(Game, scene, Vector2.Zero);
                    break;
                case "Closet":
                    e = new Closet(Game, scene, Vector2.Zero);
                    break;
                case "Chandelier":
                    e = new Chandelier(Game, scene, Vector2.Zero);
                    break;
                case "Forklift":
                    e = new Forklift(Game, scene, Vector2.Zero);
                    if (reader.MoveToAttribute("Active"))
                        ((IActivable)e).Active = reader.Value == "True";
                    break;
                case "Elevator":
                    e = new Elevator(Game, scene, Vector2.Zero);
                    if (reader.MoveToAttribute("InitialPosition"))
                        ((Elevator)e).InitialPosition = PositionFromString(reader.Value);
                    if (reader.MoveToAttribute("FinalPosition"))
                        ((Elevator)e).FinalPosition = PositionFromString(reader.Value);
                    if (reader.MoveToAttribute("Active"))
                        ((IActivable)e).Active = reader.Value == "True";
                    break;
                case "LaserBarrier":
                    e = new LaserBarrier(Game, scene, Vector2.Zero);
                    if (reader.MoveToAttribute("Active"))
                        ((IActivable)e).Active = reader.Value == "True";
                    break;
                case "PressurePlate":
                    e = new PressurePlate(Game, scene, Vector2.Zero);
                    if (reader.MoveToAttribute("ActivableElementId"))
                        ((Nobots.Elements.Activator)e).ActivableElementId = reader.Value;
                    break;
                case "ElectricityBox":
                    e = new ElectricityBox(Game, scene, Vector2.Zero);
                    if (reader.MoveToAttribute("ActivableElementId"))
                        ((Nobots.Elements.Activator)e).ActivableElementId = reader.Value;
                    break;
                case "Socket":
                    e = new Socket(Game, scene, Vector2.Zero);
                    if (reader.MoveToAttribute("OtherSocketId"))
                        ((Socket)e).OtherSocketId = reader.Value;
                    break;
                case "Ladder":
                    int stepsNumber = 1;
                    if (reader.MoveToAttribute("StepsNumber"))
                        stepsNumber = int.Parse(reader.Value);
                    e = new Ladder(Game, scene, stepsNumber, Vector2.Zero);
                    break;
                case "Character":
                    e = new Character(Game, scene, Vector2.Zero);
                    scene.Camera.Target = scene.InputManager.Character = (Character)e;
                    break;
                case "Stone":
                    e = new Stone(Game, scene, Vector2.Zero);
                    break;
                case "ConveyorBelt":
                    e = new ConveyorBelt(Game, scene, Vector2.Zero);
                    if (reader.MoveToAttribute("AngularSpeed"))
                        ((ConveyorBelt)e).AngularSpeed = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                    if (reader.MoveToAttribute("LinksNumber"))
                        ((ConveyorBelt)e).LinksNumber = int.Parse(reader.Value);
                    if (reader.MoveToAttribute("LinkWidth"))
                        ((ConveyorBelt)e).LinkWidth = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                    if (reader.MoveToAttribute("LinkHeight"))
                        ((ConveyorBelt)e).LinkHeight = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                    break;
                default:
                    return null;
            }

            if (reader.MoveToAttribute("Id"))
                e.Id = reader.Value;
            if (reader.MoveToAttribute("Position"))
                e.Position = PositionFromString(reader.Value);
            if (reader.MoveToAttribute("Width"))
                e.Width = float.Parse(reader.Value, CultureInfo.InvariantCulture);
            if (reader.MoveToAttribute("Height"))
                e.Height = float.Parse(reader.Value, CultureInfo.InvariantCulture);
            if (reader.MoveToAttribute("Rotation"))
                e.Rotation = float.Parse(reader.Value, CultureInfo.InvariantCulture);

            return e;
        }

        #endregion FromXml

        #region ToXml

        public String SceneToXml(Scene scene)
        {
            String xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n";
            xml += "<Scene>\n";
            xml += "    <Backgrounds>\n";
            foreach (Background i in scene.Backgrounds)
            {
                xml += "        " + ElementToXml(i) + "\n";
            }
            xml += "    </Backgrounds>\n";
            xml += "    <Elements>\n";
            foreach (Element i in scene.Elements)
            {
                if (i as Platform != null)
                    xml += "        " + ElementToXml((Platform)i) + "\n";
                else if (i as Box != null)
                    xml += "        " + ElementToXml((Box)i) + "\n";
                else if (i as Chandelier != null)
                    xml += "        " + ElementToXml((Chandelier)i) + "\n";
                else if (i as Stone != null)
                    xml += "        " + ElementToXml((Stone)i) + "\n";
                else if (i as Closet != null)
                    xml += "        " + ElementToXml((Closet)i) + "\n";
                else if (i as Ladder != null)
                    xml += "        " + ElementToXml((Ladder)i) + "\n";
                else if (i as LaserBarrier != null)
                    xml += "        " + ElementToXml((LaserBarrier)i) + "\n";
                else if (i as PressurePlate != null)
                    xml += "        " + ElementToXml((PressurePlate)i) + "\n";
                else if (i as Socket != null)
                    xml += "        " + ElementToXml((Socket)i) + "\n";
                else if (i as Elevator != null)
                    xml += "        " + ElementToXml((Elevator)i) + "\n";
                else if (i as ElectricityBox != null)
                    xml += "        " + ElementToXml((ElectricityBox)i) + "\n";
                else if (i as Character != null)
                    xml += "        " + ElementToXml((Character)i) + "\n";
                else if (i as Forklift != null)
                    xml += "        " + ElementToXml((Forklift)i) + "\n";
                else if (i as ConveyorBelt != null)
                    xml += "        " + ElementToXml((ConveyorBelt)i) + "\n";
                else
                    throw new NotImplementedException(i.GetType().Name + " is still pendent to be converted into XML");
            }
            xml += "    </Elements>\n";
            xml += "    <Foregrounds>\n";
            foreach (Background i in scene.Foregrounds)
            {
                xml += "        " + ElementToXml(i) + "\n";
            }
            xml += "    </Foregrounds>\n";
            xml += "</Scene>\n";

            Console.WriteLine(xml);
            return xml;
        }

        public String ElementToXml(Background background)
        {
            String xml = "<Background Id=\"" + background.Id + "\" Position=\"" + background.Position.X + "," + background.Position.Y + "\"  Speed=\"" + background.Speed.X + "," + background.Speed.Y + "\" TextureName=\"" + background.TextureName + "\" />";
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

        public String ElementToXml(Chandelier chandelier)
        {
            String xml = "<Chandelier Id=\"" + chandelier.Id + "\" Position=\"" + chandelier.Position.X + "," + chandelier.Position.Y + "\" Rotation=\"" + chandelier.Rotation + "\" />";
            return xml;
        }

        public String ElementToXml(Stone stone)
        {
            String xml = "<Stone Id=\"" + stone.Id + "\" Position=\"" + stone.Position.X + "," + stone.Position.Y + "\" Rotation=\"" + stone.Rotation + "\" />";
            return xml;
        }

        public String ElementToXml(Closet closet)
        {
            String xml = "<Closet Id=\"" + closet.Id + "\" Position=\"" + closet.Position.X + "," + closet.Position.Y + "\" Rotation=\"" + closet.Rotation + "\" />";
            return xml;
        }

        public String ElementToXml(Character character)
        {
            String xml = "<Character Id=\"" + character.Id + "\" Position=\"" + character.Position.X + "," + character.Position.Y + "\" />";
            return xml;
        }

        public String ElementToXml(ElectricityBox eBox)
        {
            Element e = eBox.ActivableElement as Element;
            String xml = "<ElectricityBox Id=\"" + eBox.Id + "\" Position=\"" + eBox.Position.X + "," + eBox.Position.Y + "\" ActivableElementId=\"" + (e != null ? e.Id : "") + "\" />";
            return xml;
        }

        public String ElementToXml(Elevator elevator)
        {
            String xml = "<Elevator Id=\"" + elevator.Id + "\" Position=\"" + elevator.Position.X + "," + elevator.Position.Y + "\" InitialPosition=\"" + elevator.InitialPosition.X + "," + elevator.InitialPosition.Y + "\" FinalPosition=\"" + elevator.FinalPosition.X + "," + elevator.FinalPosition.Y + "\" Active=\"" + elevator.Active + "\" />";
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

        public String ElementToXml(ConveyorBelt cBelt)
        {
            String xml = "<ConveyorBelt Id=\"" + cBelt.Id + "\" Position=\"" + cBelt.Position.X + "," + cBelt.Position.Y + "\" AngularSpeed=\"" + cBelt.AngularSpeed + "\" LinksNumber=\"" + cBelt.LinksNumber + "\" LinkWidth=\"" + cBelt.LinkWidth + "\" LinkHeight=\"" + cBelt.LinkHeight + "\" />";
            return xml;
        }

        public String ElementToXml(LaserBarrier laserBarrier)
        {
            String xml = "<LaserBarrier Id=\"" + laserBarrier.Id + "\" Position=\"" + laserBarrier.Position.X + "," + laserBarrier.Position.Y + "\" Rotation=\"" + laserBarrier.Rotation + "\" Width=\"" + laserBarrier.Width + "\" Height=\"" + laserBarrier.Height + "\" Active=\"" + laserBarrier.Active + "\" />";
            return xml;
        }

        public String ElementToXml(PressurePlate pressurePlate)
        {
            Element e = pressurePlate.ActivableElement as Element;
            String xml = "<PressurePlate Id=\"" + pressurePlate.Id + "\" Position=\"" + pressurePlate.Position.X + "," + pressurePlate.Position.Y + "\" ActivableElementId=\"" + (e != null ? e.Id : "") + "\" />";
            return xml;
        }

        public String ElementToXml(Socket socket)
        {
            String xml = "<Socket Id=\"" + socket.Id + "\" Position=\"" + socket.Position.X + "," + socket.Position.Y + "\" OtherSocketId=\"" + (socket.OtherSocket != null ? socket.OtherSocket.Id : "") + "\" />";
            return xml;
        }

        #endregion ToXml

        public void Update(GameTime gameTime, Scene scene)
        {
            KeyboardState keybState = Keyboard.GetState();

            if (Game.IsActive)
            {
                if (keybState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) && keybState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Filter = "XML file|*.xml";
                    saveFileDialog1.Title = "Save an XML File";
                    DialogResult dr = saveFileDialog1.ShowDialog();
                    if (dr == DialogResult.OK)
                        System.IO.File.WriteAllText(saveFileDialog1.FileName, SceneToXml(scene));
                }
                if (keybState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) && keybState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.O))
                {
                    OpenFileDialog openFileDialog1 = new OpenFileDialog();
                    openFileDialog1.Filter = "XML file|*.xml";
                    openFileDialog1.Title = "Open an XML File";
                    DialogResult dr = openFileDialog1.ShowDialog();
                    if (dr == DialogResult.OK)
                        SceneFromXml(openFileDialog1.FileName, scene);
                }
            }
            base.Update(gameTime);
        }
    }
}
