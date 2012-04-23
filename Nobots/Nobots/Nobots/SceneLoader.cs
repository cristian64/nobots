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
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US"); 
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
                            scene.Foregrounds.Add(ForegroundFromXml(reader, scene));
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
            Background e = new Background(Game, scene, Vector2.Zero);
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

        public Foreground ForegroundFromXml(XmlTextReader reader, Scene scene)
        {
            Foreground e = new Foreground(Game, scene, Vector2.Zero);
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
                case "Stomper":
                    e = new Stomper(Game, scene, Vector2.Zero);
                    break;
                case "Crate":
                    e = new Crate(Game, scene, Vector2.Zero);
                    if (reader.MoveToAttribute("Color"))
                        ((Crate)e).Color = reader.Value;
                    break;
                case "Computer":
                    e = new Computer(Game, scene, Vector2.Zero);
                    break;
                case "Lamp":
                    e = new Lamp(Game, scene, Vector2.Zero);
                    break;
                case "Alarm":
                    e = new Alarm(Game, scene, Vector2.Zero);
                    break;
                case "Closet":
                    e = new Closet(Game, scene, Vector2.Zero);
                    break;
                case "Chandelier":
                    e = new Chandelier(Game, scene, Vector2.Zero);
                    break;
                case "ExperimentalTube":
                    e = new ExperimentalTube(Game, scene, Vector2.Zero);
                    break;
                case "Forklift":
                    e = new Forklift(Game, scene, Vector2.Zero);
                    break;
                case "Elevator":
                    e = new Elevator(Game, scene, Vector2.Zero);
                    if (reader.MoveToAttribute("InitialPosition"))
                        ((Elevator)e).InitialPosition = PositionFromString(reader.Value);
                    if (reader.MoveToAttribute("FinalPosition"))
                        ((Elevator)e).FinalPosition = PositionFromString(reader.Value);
                    break;
                case "MovingPlatform":
                    e = new MovingPlatform(Game, scene, Vector2.Zero);
                    if (reader.MoveToAttribute("InitialPosition"))
                        ((MovingPlatform)e).InitialPosition = PositionFromString(reader.Value);
                    if (reader.MoveToAttribute("FinalPosition"))
                        ((MovingPlatform)e).FinalPosition = PositionFromString(reader.Value);
                    break;
                case "LaserBarrier":
                    e = new LaserBarrier(Game, scene, Vector2.Zero);
                    break;
                case "Checkpoint":
                    e = new Checkpoint(Game, scene, Vector2.Zero);
                    break;
                case "Endpoint":
                    e = new Endpoint(Game, scene, Vector2.Zero);
                    if (reader.MoveToAttribute("NextLevel"))
                        ((Endpoint)e).NextLevel = reader.Value;
                    break;
                case "PressurePlate":
                    e = new PressurePlate(Game, scene, Vector2.Zero);
                    break;
                case "ElectricityBox":
                    e = new ElectricityBox(Game, scene, Vector2.Zero);
                    break;
                case "Lever":
                    e = new Lever(Game, scene, Vector2.Zero);
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
                    if (reader.MoveToAttribute("RotorsNumber"))
                        ((ConveyorBelt)e).RotorsNumber = int.Parse(reader.Value);
                    if (reader.MoveToAttribute("LinkWidth"))
                        ((ConveyorBelt)e).LinkWidth = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                    if (reader.MoveToAttribute("LinkHeight"))
                        ((ConveyorBelt)e).LinkHeight = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                    break;
                case "ImpulsePlatform":
                    e = new ImpulsePlatform(Game, scene, Vector2.Zero);
                    if (reader.MoveToAttribute("Acceleration"))
                        ((ImpulsePlatform)e).Acceleration = float.Parse(reader.Value, CultureInfo.InvariantCulture);
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

            if (e is IActivable && reader.MoveToAttribute("Active"))
                ((IActivable)e).Active = reader.Value == "True";

            if (e is Nobots.Elements.Activator && reader.MoveToAttribute("ActivableElementId"))
                ((Nobots.Elements.Activator)e).ActivableElementId = reader.Value;

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
                else if (i as Stomper != null)
                    xml += "        " + ElementToXml((Stomper)i) + "\n";
                else if (i as Crate != null)
                    xml += "        " + ElementToXml((Crate)i) + "\n";
                else if (i as Computer != null)
                    xml += "        " + ElementToXml((Computer)i) + "\n";
                else if (i as Lamp != null)
                    xml += "        " + ElementToXml((Lamp)i) + "\n";
                else if (i as Alarm != null)
                    xml += "        " + ElementToXml((Alarm)i) + "\n";
                else if (i as Chandelier != null)
                    xml += "        " + ElementToXml((Chandelier)i) + "\n";
                else if (i as ExperimentalTube != null)
                    xml += "        " + ElementToXml((ExperimentalTube)i) + "\n";
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
                else if (i as MovingPlatform != null)
                    xml += "        " + ElementToXml((MovingPlatform)i) + "\n";
                else if (i as ElectricityBox != null)
                    xml += "        " + ElementToXml((ElectricityBox)i) + "\n";
                else if (i as Lever != null)
                    xml += "        " + ElementToXml((Lever)i) + "\n";
                else if (i as Checkpoint != null)
                    xml += "        " + ElementToXml((Checkpoint)i) + "\n";
                else if (i as Character != null)
                    xml += "        " + ElementToXml((Character)i) + "\n";
                else if (i as Forklift != null)
                    xml += "        " + ElementToXml((Forklift)i) + "\n";
                else if (i as ConveyorBelt != null)
                    xml += "        " + ElementToXml((ConveyorBelt)i) + "\n";
                else if (i as ImpulsePlatform != null)
                    xml += "        " + ElementToXml((ImpulsePlatform)i) + "\n";
                else
                    throw new NotImplementedException(i.GetType().Name + " is still pendent to be converted into XML");
            }
            xml += "    </Elements>\n";
            xml += "    <Foregrounds>\n";
            foreach (Foreground i in scene.Foregrounds)
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
            String xml = "<Background Id=\"" + background.Id + "\" Position=\"" + background.Position.X + "," + background.Position.Y + "\" Rotation=\"" + background.Rotation + "\" Speed=\"" + background.Speed.X + "," + background.Speed.Y + "\" TextureName=\"" + background.TextureName + "\" />";
            return xml;
        }

        public String ElementToXml(Foreground foreground)
        {
            String xml = "<Foreground Id=\"" + foreground.Id + "\" Position=\"" + foreground.Position.X + "," + foreground.Position.Y + "\" Rotation=\"" + foreground.Rotation + "\" Speed=\"" + foreground.Speed.X + "," + foreground.Speed.Y + "\" TextureName=\"" + foreground.TextureName + "\" />";
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

        public String ElementToXml(Stomper stomper)
        {
            String xml = "<Stomper Id=\"" + stomper.Id + "\" Position=\"" + stomper.Position.X + "," + stomper.Position.Y + "\" Rotation=\"" + stomper.Rotation + "\" Active=\"" + stomper.Active + "\" />"; ;
            return xml;
        }

        public String ElementToXml(Crate crate)
        {
            String xml = "<Crate Id=\"" + crate.Id + "\" Position=\"" + crate.Position.X + "," + crate.Position.Y + "\" Rotation=\"" + crate.Rotation + "\" Color=\"" + crate.Color + "\" />";
            return xml;
        }

        public String ElementToXml(Computer computer)
        {
            String xml = "<Computer Id=\"" + computer.Id + "\" Position=\"" + computer.Position.X + "," + computer.Position.Y + "\" Rotation=\"" + computer.Rotation + "\" />";
            return xml;
        }

        public String ElementToXml(Lamp lamp)
        {
            String xml = "<Lamp Id=\"" + lamp.Id + "\" Position=\"" + lamp.Position.X + "," + lamp.Position.Y + "\" Rotation=\"" + lamp.Rotation + "\" Active=\"" + lamp.Active + "\" />";
            return xml;
        }

        public String ElementToXml(Alarm alarm)
        {
            String xml = "<Alarm Id=\"" + alarm.Id + "\" Position=\"" + alarm.Position.X + "," + alarm.Position.Y + "\" Rotation=\"" + alarm.Rotation + "\" Active=\"" + alarm.Active + "\" />";
            return xml;
        }

        public String ElementToXml(Checkpoint checkpoint)
        {
            String xml = "<Checkpoint Id=\"" + checkpoint.Id + "\" Position=\"" + checkpoint.Position.X + "," + checkpoint.Position.Y + "\" Rotation=\"" + checkpoint.Rotation + "\" Active=\"" + checkpoint.Active + "\" />";
            return xml;
        }

        public String ElementToXml(Chandelier chandelier)
        {
            String xml = "<Chandelier Id=\"" + chandelier.Id + "\" Position=\"" + chandelier.Position.X + "," + chandelier.Position.Y + "\" Rotation=\"" + chandelier.Rotation + "\" />";
            return xml;
        }

        public String ElementToXml(ExperimentalTube eTube)
        {
            String xml = "<ExperimentalTube Id=\"" + eTube.Id + "\" Position=\"" + eTube.Position.X + "," + eTube.Position.Y + "\" Rotation=\"" + eTube.Rotation + "\" />";
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

        public String ElementToXml(Lever lever)
        {
            Element e = lever.ActivableElement as Element;
            String xml = "<Lever Id=\"" + lever.Id + "\" Position=\"" + lever.Position.X + "," + lever.Position.Y + "\" Rotation=\"" + lever.Rotation + "\" ActivableElementId=\"" + (e != null ? e.Id : "") + "\" />";
            return xml;
        }

        public String ElementToXml(Elevator elevator)
        {
            String xml = "<Elevator Id=\"" + elevator.Id + "\" Position=\"" + elevator.Position.X + "," + elevator.Position.Y + "\" InitialPosition=\"" + elevator.InitialPosition.X + "," + elevator.InitialPosition.Y + "\" FinalPosition=\"" + elevator.FinalPosition.X + "," + elevator.FinalPosition.Y + "\" Active=\"" + elevator.Active + "\" />";
            return xml;
        }

        public String ElementToXml(MovingPlatform movingPlatform)
        {
            String xml = "<MovingPlatform Id=\"" + movingPlatform.Id + "\" Position=\"" + movingPlatform.Position.X + "," + movingPlatform.Position.Y + "\" InitialPosition=\"" + movingPlatform.InitialPosition.X + "," + movingPlatform.InitialPosition.Y + "\" FinalPosition=\"" + movingPlatform.FinalPosition.X + "," + movingPlatform.FinalPosition.Y + "\" Active=\"" + movingPlatform.Active + "\" />";
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
            String xml = "<ConveyorBelt Id=\"" + cBelt.Id + "\" Position=\"" + cBelt.Position.X + "," + cBelt.Position.Y + "\" Width=\"" + cBelt.Width + "\" Height=\"" + cBelt.Height + "\" AngularSpeed=\"" + cBelt.AngularSpeed + "\" RotorsNumber=\"" + cBelt.RotorsNumber + "\" LinksNumber=\"" + cBelt.LinksNumber + "\" LinkWidth=\"" + cBelt.LinkWidth + "\" LinkHeight=\"" + cBelt.LinkHeight + "\" />";
            return xml;
        }

        public String ElementToXml(ImpulsePlatform ip)
        {
            String xml = "<ImpulsePlatform Id=\"" + ip.Id + "\" Position=\"" + ip.Position.X + "," + ip.Position.Y + "\" Width=\"" + ip.Width + "\" Height=\"" + ip.Height + "\" Acceleration=\"" + ip.Acceleration + "\" Active=\"" + ip.Active + "\" />";
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
                    {
                        scene.Backgrounds.Clear();
                        scene.Elements.Clear();
                        scene.Foregrounds.Clear();
                        scene.World.Clear();
                        scene.Camera.Target = null;
                        scene.InputManager.Character = null;
                        //TODO those Clear() are bullshit. it won't free any memory since there is no Dispose in DrawableElements...
                        SceneFromXml(openFileDialog1.FileName, scene);
                    }
                }
            }
            base.Update(gameTime);
        }
    }
}
