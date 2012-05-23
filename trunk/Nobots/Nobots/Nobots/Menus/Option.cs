using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nobots.Menus
{
    public class Option
    {
        protected Scene scene;
        public String Text = "";

        public Option(String text, Scene scene)
        {
            Text = text;
            this.scene = scene;
        }

        public virtual void Refresh(bool selected) { }

        public virtual void AActionStop() { }
        public virtual void RightActionStop() { }
        public virtual void LeftActionStop() { }
    }

    public class ResumeOption : Option
    {
        public ResumeOption(Scene scene) :
            base("Resume", scene)
        {
        }

        public override void AActionStop()
        {
            scene.Menu.Enabled = false;
            scene.Transitioner.AlphaTarget = 0;
        }
    }

    public class ExitOption : Option
    {
        public ExitOption(Scene scene) :
            base("Exit", scene)
        {
        }

        public override void AActionStop()
        {
            scene.Game.Exit();
        }
    }

    public class RestartLevelOption : Option
    {
        public RestartLevelOption(Scene scene) :
            base("Restart Level", scene)
        {
        }

        public override void AActionStop()
        {
            scene.CleanAndLoad(scene.SceneLoader.LastLevel);
            scene.Menu.Enabled = false;
        }
    }

    public class LoadLevelOption : Option
    {
        int selectedIndex = 0;

        public LoadLevelOption(Scene scene) :
            base("Load Level", scene)
        {
        }

        public override void Refresh(bool selected)
        {
            Text = "Load Level";
            if (selected)
            {
                Text += "     ";
                for (int i = 0; i < scene.SceneLoader.Levels.Count; i++)
                {
                    if (selectedIndex == i)
                    {
                        Text += (i + 1).ToString() + " ";
                    }
                    else
                    {
                        Text += ". ";
                    }
                }
            }
        }

        public override void AActionStop()
        {
            scene.CleanAndLoad(scene.SceneLoader.Levels[selectedIndex]);
            scene.Menu.Enabled = false;
        }

        public override void RightActionStop()
        {
            selectedIndex = (selectedIndex + 1) % scene.SceneLoader.Levels.Count;
            Refresh(true);
        }

        public override void LeftActionStop()
        {
            selectedIndex = (selectedIndex + scene.SceneLoader.Levels.Count - 1) % scene.SceneLoader.Levels.Count;
            Refresh(true);
        }
    }
}
