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

    public class EditorOption : Option
    {
        public EditorOption(Scene scene) :
            base("Open Editor", scene)
        {
        }

        public override void AActionStop()
        {
            Process.Start("Synergy (editor).exe");
            scene.Game.Exit();
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
                        Text += "· ";
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
