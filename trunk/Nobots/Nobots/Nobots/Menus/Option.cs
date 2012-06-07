using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Nobots.Elements;
using System.Diagnostics;
using System.Windows.Forms;

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

    public class ControlsOption : Option
    {
        public ControlsOption(Scene scene) :
            base("Controls", scene)
        {
        }
    }

    public class ResumeOption : Option
    {
        public ResumeOption(Scene scene) :
            base("Resume", scene)
        {
            bool resuming = false;
            try
            {
                string text = System.IO.File.ReadAllText(@"Content\levels\lastlevel");
                if (text != "" || scene.SceneLoader.LastLevel != "")
                    resuming = true;
            }
            catch (Exception) { }

            Text = resuming ? "Resume" : "Start New Game";
        }

        public override void Refresh(bool selected)
        {
            bool resuming = false;
            try
            {
                string text = System.IO.File.ReadAllText(@"Content\levels\lastlevel");
                if (text != "" || scene.SceneLoader.LastLevel != "")
                    resuming = true;
            }
            catch (Exception) { }

            Text = resuming ? "Resume" : "Start New Game";
        }

        public override void AActionStop()
        {
            scene.Menu.Enabled = false;
            scene.Transitioner.AlphaTarget = 0;

            if (scene.Elements.Count + scene.Backgrounds.Count + scene.Backgrounds.Count == 0)
            {
                try
                {
                    string text = System.IO.File.ReadAllText(@"Content\levels\lastlevel");
                    scene.CleanAndLoad(text);
                }
                catch (Exception)
                {
                    scene.CleanAndLoad("level1");
                }
            }
        }
    }

    public class EditorOption : Option
    {
        public EditorOption(Scene scene) :
            base("Open Editor", scene)
        {
        }

        public override void AActionStop()
        {
            try
            {
                Process.Start("Synergy (editor).exe");
                scene.Game.Exit();
            }
            catch (Exception)
            {
                Text = "Open Editor  (ERROR: \"Synergy (editor).exe\" not found)";
            }
        }

        public override void Refresh(bool selected)
        {
            if (!selected)
                Text = "Open Editor";
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

    public class LastCheckpointOption : Option
    {
        public LastCheckpointOption(Scene scene) :
            base("Last Checkpoint", scene)
        {
        }

        public override void AActionStop()
        {
            scene.Menu.Enabled = false;

#if !FINAL_RELEASE
            if (MessageBox.Show("This is the Editor mode. Last checkpoint may be not available if the current level was loaded from a external source by Ctrl+O. Are you sure you want to continue and lose every not saved change?", "Warning!", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
#endif
            Vector2? checkpointPosition = null;
            bool? characterActive = null;
            foreach (Element i in scene.Elements)
            {
                if (i is Checkpoint && ((Checkpoint)i).Active)
                {
                    checkpointPosition = i.Position;
                }

                if (i is Character)
                {
                    characterActive = ((Character)i).Active;
                }
            }
            if (checkpointPosition != null)
                scene.CleanAndLoad(scene.SceneLoader.LastLevel, checkpointPosition, characterActive);
            else
                scene.CleanAndLoad(scene.SceneLoader.LastLevel);
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
            scene.Menu.Enabled = false;
#if !FINAL_RELEASE
            if (MessageBox.Show("This is the Editor mode. Restarting the level may not work if the current level was loaded from a external source by Ctrl+O. Are you sure you want to continue and lose every not saved change?", "Warning!", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
#endif
            scene.CleanAndLoad(scene.SceneLoader.LastLevel);
        }
    }

    public class LoadLevelOption : Option
    {
        int selectedIndex = 0;

        public LoadLevelOption(Scene scene) :
            base("Load Level", scene)
        {
        }

        bool firstTime = true;
        public override void Refresh(bool selected)
        {
            if (!selected)
            {
                if (scene.SceneLoader.LastLevel != "")
                {
                    selectedIndex = scene.SceneLoader.Levels.IndexOf(scene.SceneLoader.LastLevel);
                    if (selectedIndex < 0)
                        selectedIndex = 0;
                }
            }

            if (firstTime)
            {
                try
                {
                    string text = System.IO.File.ReadAllText(@"Content\levels\lastlevel");
                    selectedIndex = scene.SceneLoader.Levels.IndexOf(text);
                }
                catch (Exception) { }
                if (selectedIndex < 0)
                    selectedIndex = 0;
                firstTime = false;
            }

            Text = "Load Level";
            if (selected)
            {
                Text += "       ";
                for (int i = 0; i < scene.SceneLoader.Levels.Count; i++)
                {
                    if (selectedIndex == i)
                    {
                        Text += (i + 1).ToString() + "  ";
                    }
                    else
                    {
                        Text += "•  ";
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
            scene.SoundManager.ISoundEngine.Play2D(scene.SoundManager.Nav, false, false, false);
        }

        public override void LeftActionStop()
        {
            selectedIndex = (selectedIndex + scene.SceneLoader.Levels.Count - 1) % scene.SceneLoader.Levels.Count;
            Refresh(true);
            scene.SoundManager.ISoundEngine.Play2D(scene.SoundManager.Nav, false, false, false);
        }
    }
}
