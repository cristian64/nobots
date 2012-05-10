using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Nobots.Elements;

namespace Nobots.Editor
{
    public partial class FormProperties : Form
    {
        public String NewElementType;
        public Game Game;
        private Scene scene;

        // This variable is used for disable events while reseting and initializing controls.
        private Element selectionEvents;

        private Element selection;
        public Element Selection
        {
            get { return selection; }
            set 
            {
                selection = value;
                selectionEvents = null;
                flowLayoutPanel1.Visible = (selection != null);

                if (flowLayoutPanel1.Visible)
                    listBoxAvailableElements.Size = initialSize;
                else
                    listBoxAvailableElements.Size = new Size(initialSize.Width, initialSize.Height + flowLayoutPanel1.Size.Height + 30);

                reset();
                if(selection != null)
                    showElementInForm();

                selectionEvents = selection;
            }
        }

        private void showElementInForm()
        {
            setInitialValues();
            
            if (selection is Background)
            {
                flowLayoutPanelTextureName.Visible = true;
                textBoxTextureName.Text = ((Background)selection).TextureName;
                flowLayoutPanelScale.Visible = true;
                numericUpDownScale.Value = (decimal)((Background)selection).Scale;
                flowLayoutPanelSpeed.Visible = true;
                numericUpDownSpeedX.Value = (decimal)((Background)selection).Speed.X;
                numericUpDownSpeedY.Value = (decimal)((Background)selection).Speed.Y;
            }

            if (selection is Foreground)
            {
                flowLayoutPanelTextureName.Visible = true;
                textBoxTextureName.Text = ((Foreground)selection).TextureName;
                flowLayoutPanelScale.Visible = true;
                numericUpDownScale.Value = (decimal)((Foreground)selection).Scale;
                flowLayoutPanelSpeed.Visible = true;
                numericUpDownSpeedX.Value = (decimal)((Foreground)selection).Speed.X;
                numericUpDownSpeedY.Value = (decimal)((Foreground)selection).Speed.Y;
            }

            if (selection is IActivable)
            {
                flowLayoutPanelActive.Visible = true;
                checkBoxActive.Checked = ((IActivable)selection).Active;
            }

            if (selection is Nobots.Elements.Activator)
            {
                flowLayoutPanelActivableElementId.Visible = true;
                textBoxActivableElementId.Text = ((Nobots.Elements.Activator)selection).ActivableElementId;
            }

            if (selection is Elevator)
            {
                flowLayoutPanelInitialPosition.Visible = true;
                flowLayoutPanelFinalPosition.Visible = true;
                numericUpDownInitialPositionX.Value = (decimal)((Elevator)selection).InitialPosition.X;
                numericUpDownInitialPositionY.Value = (decimal)((Elevator)selection).InitialPosition.Y;
                numericUpDownFinalPositionX.Value = (decimal)((Elevator)selection).FinalPosition.X;
                numericUpDownFinalPositionY.Value = (decimal)((Elevator)selection).FinalPosition.Y;
            }

            if (selection is MovingPlatform)
            {
                flowLayoutPanelInitialPosition.Visible = true;
                flowLayoutPanelFinalPosition.Visible = true;
                numericUpDownInitialPositionX.Value = (decimal)((MovingPlatform)selection).InitialPosition.X;
                numericUpDownInitialPositionY.Value = (decimal)((MovingPlatform)selection).InitialPosition.Y;
                numericUpDownFinalPositionX.Value = (decimal)((MovingPlatform)selection).FinalPosition.X;
                numericUpDownFinalPositionY.Value = (decimal)((MovingPlatform)selection).FinalPosition.Y;
            }

            if (selection is Socket)
            {
                flowLayoutPanelOtherSocketId.Visible = true;
                textBoxOtherSocketId.Text = ((Socket)selection).OtherSocketId;
            }

            if (selection is ExperimentalTube)
            {
                flowLayoutPanelOtherTubeId.Visible = true;
                textBoxOtherTubeId.Text = ((ExperimentalTube)selection).OtherTubeId;
            }
            
            if (selection is Ladder)
            {
                flowLayoutPanelStepsNumber.Visible = true;
                numericUpDownStepsNumber.Value = (decimal)((Ladder)selection).StepsNumber;
            }

            if (selection is CameraScale)
            {
                flowLayoutPanelScaleTarget.Visible = true;
                numericUpDownScaleTarget.Value = (decimal)((CameraScale)selection).ScaleTarget;
            }

            if (selection is GlidePlatform)
            {
                flowLayoutPanelStepsNumber.Visible = true;
                numericUpDownStepsNumber.Value = (decimal)((GlidePlatform)selection).StepsNumber;
                flowLayoutPanelVelocity.Visible = true;
                numericUpDownVelocity.Value = (decimal)((GlidePlatform)selection).Velocity;
            }

            if (selection is TrainTrack)
            {
                flowLayoutPanelStepsNumber.Visible = true;
                numericUpDownStepsNumber.Value = (decimal)((TrainTrack)selection).StepsNumber;
            }

            if (selection is ConveyorBelt)
            {
                flowLayoutPanelAngularSpeed.Visible = true;
                numericUpDownAngularSpeed.Value = (decimal)((ConveyorBelt)selection).AngularSpeed;
                flowLayoutPanelRotorsNumber.Visible = true;
                numericUpDownRotorsNumber.Value = (decimal)((ConveyorBelt)selection).RotorsNumber;
                flowLayoutPanelLinksNumber.Visible = true;
                numericUpDownLinksNumber.Value = (decimal)((ConveyorBelt)selection).LinksNumber;
                flowLayoutPanelLinkWidth.Visible = true;
                numericUpDownLinkWidth.Value = (decimal)((ConveyorBelt)selection).LinkWidth;
                flowLayoutPanelLinkHeight.Visible = true;
                numericUpDownLinkHeight.Value = (decimal)((ConveyorBelt)selection).LinkHeight;
            }

            if (selection is ImpulsePlatform)
            {
                flowLayoutPanelAcceleration.Visible = true;
                numericUpDownAcceleration.Value = (decimal)((ImpulsePlatform)selection).Acceleration;
            }

            if (selection is Endpoint)
            {
                flowLayoutPanelNextLevel.Visible = true;
                textBoxNextLevel.Text = ((Endpoint)selection).NextLevel;
            }

            if (selection is Hint)
            {
                flowLayoutPanelText.Visible = true;
                textBoxText.Text = ((Hint)selection).Text;
            }

            if (selection is Crate)
            {
                flowLayoutPanelColor.Visible = true;
                comboBoxColor.Text = ((Crate)selection).Color;
            }

            if (selection is Sound)
            {
                flowLayoutPanelSoundName.Visible = true;
                textBoxSoundName.Text = ((Sound)selection).SoundName;
                flowLayoutPanelVolume.Visible = true;
                numericUpDownVolume.Value = (decimal)((Sound)selection).Volume;
            }

            if (selection is Crane)
            {
                flowLayoutPanelLeftShift.Visible = true;
                numericUpDownLeftShift.Value = (decimal)((Crane)selection).LeftShift;
                flowLayoutPanelRightShift.Visible = true;
                numericUpDownRightShift.Value = (decimal)((Crane)selection).RightShift;
                flowLayoutPanelUpShift.Visible = true;
                numericUpDownUpShift.Value = (decimal)((Crane)selection).UpShift;
                flowLayoutPanelDownShift.Visible = true;
                numericUpDownDownShift.Value = (decimal)((Crane)selection).DownShift;
            }
        }

        private void reset()
        {
            labelElementType.Text = "(no selection)";
            textBoxId.Text = "";
            numericUpDownPositionX.Value = 0;
            numericUpDownPositionY.Value = 0;
            numericUpDownWidth.Value = numericUpDownWidth.Minimum;
            numericUpDownHeight.Value = numericUpDownHeight.Minimum;
            numericUpDownRotation.Value = 0;

            //visibility
            flowLayoutPanelActivableElementId.Visible = false;
            flowLayoutPanelActive.Visible = false;
            flowLayoutPanelFinalPosition.Visible = false;
            flowLayoutPanelInitialPosition.Visible = false;
            flowLayoutPanelOtherSocketId.Visible = false;
            flowLayoutPanelScale.Visible = false;
            flowLayoutPanelSpeed.Visible = false;
            flowLayoutPanelStepsNumber.Visible = false;
            flowLayoutPanelLinksNumber.Visible = false;
            flowLayoutPanelRotorsNumber.Visible = false;
            flowLayoutPanelLinkWidth.Visible = false;
            flowLayoutPanelLinkHeight.Visible = false;
            flowLayoutPanelAngularSpeed.Visible = false;
            flowLayoutPanelTextureName.Visible = false;
            flowLayoutPanelNextLevel.Visible = false;
            flowLayoutPanelSoundName.Visible = false;
            flowLayoutPanelOtherTubeId.Visible = false;
            flowLayoutPanelVolume.Visible = false;
            flowLayoutPanelAcceleration.Visible = false;
            flowLayoutPanelVelocity.Visible = false;
            flowLayoutPanelColor.Visible = false;
            flowLayoutPanelScaleTarget.Visible = false;
            flowLayoutPanelLeftShift.Visible = false;
            flowLayoutPanelRightShift.Visible = false;
            flowLayoutPanelUpShift.Visible = false;
            flowLayoutPanelDownShift.Visible = false;
            flowLayoutPanelText.Visible = false;
        }

        private void setInitialValues()
        {
            //values for every element
            labelElementType.Text = selection.GetType().Name;
            textBoxId.Text = selection.Id;
            numericUpDownPositionX.Value = (decimal)selection.Position.X;
            numericUpDownPositionY.Value = (decimal)selection.Position.Y;
            numericUpDownWidth.Value = (decimal)selection.Width;
            numericUpDownHeight.Value = (decimal)selection.Height;
            numericUpDownRotation.Value = (decimal)selection.Rotation;
        }

        private Size initialSize;

        public FormProperties(Game game, Scene scene)
        {
            this.Game = game;
            this.scene = scene;
            InitializeComponent();
            Top = 0;
            Left = Screen.PrimaryScreen.WorkingArea.Size.Width - Size.Width;
            Size = new Size(Size.Width, Screen.PrimaryScreen.WorkingArea.Size.Height);
            checkBoxShowDebug.Checked = scene.PhysicsDebug.Enabled;
            checkBoxPhysicsEngine.Checked = scene.World.Enabled;
            checkBoxShowEmblems.Checked = SelectionManager.ShowEmblems;
            flowLayoutPanel1.Visible = false;
            initialSize = listBoxAvailableElements.Size;
            listBoxAvailableElements.Size = new Size(initialSize.Width, initialSize.Height + flowLayoutPanel1.Size.Height + 30);
        }

        private void textBoxId_TextChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                selectionEvents.Id = textBoxId.Text;
        }

        private void numericUpDownPositionX_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                selectionEvents.Position = new Vector2((float)numericUpDownPositionX.Value, selectionEvents.Position.Y);
        }

        private void numericUpDownPositionY_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                selectionEvents.Position = new Vector2(selectionEvents.Position.X, (float)numericUpDownPositionY.Value);
        }

        private void numericUpDownWidth_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                selectionEvents.Width = (float)numericUpDownWidth.Value;
        }

        private void numericUpDownHeight_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                selectionEvents.Height = (float)numericUpDownHeight.Value;
        }

        private void numericUpDownRotation_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                selectionEvents.Rotation = (float)numericUpDownRotation.Value;
        }

        private void textBoxTextureName_TextChanged(object sender, EventArgs e)
        {
            if (selectionEvents as Background != null)
                ((Background)selectionEvents).TextureName = textBoxTextureName.Text;
            if (selectionEvents as Foreground != null)
                ((Foreground)selectionEvents).TextureName = textBoxTextureName.Text;
        }

        private void numericUpDownSpeedX_ValueChanged(object sender, EventArgs e)
        {
            Background b = selectionEvents as Background;
            if (b != null)
                b.Speed = new Vector2((float)numericUpDownSpeedX.Value, b.Speed.Y);
            Foreground f = selectionEvents as Foreground;
            if (f != null)
                f.Speed = new Vector2((float)numericUpDownSpeedX.Value, f.Speed.Y);
        }

        private void numericUpDownSpeedY_ValueChanged(object sender, EventArgs e)
        {
            Background b = selectionEvents as Background;
            if (b != null)
                b.Speed = new Vector2(b.Speed.X, (float)numericUpDownSpeedY.Value);
            Foreground f = selectionEvents as Foreground;
            if (f != null)
                f.Speed = new Vector2(f.Speed.X, (float)numericUpDownSpeedY.Value);
        }

        private void checkBoxActive_CheckedChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((IActivable)selectionEvents).Active = checkBoxActive.Checked;
        }

        private void textBoxActivableElementId_TextChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((Nobots.Elements.Activator)selectionEvents).ActivableElementId = textBoxActivableElementId.Text;
        }

        private void textBoxOtherSocketId_TextChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((Socket)selectionEvents).OtherSocketId = textBoxOtherSocketId.Text;
        }

        private void numericUpDownStepsNumber_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null && selectionEvents is Ladder)
                ((Ladder)selectionEvents).StepsNumber = (int)numericUpDownStepsNumber.Value;
            else if (selectionEvents != null && selectionEvents is GlidePlatform)
                ((GlidePlatform)selectionEvents).StepsNumber = (int)numericUpDownStepsNumber.Value;
            else if (selectionEvents != null && selectionEvents is TrainTrack)
                ((TrainTrack)selectionEvents).StepsNumber = (int)numericUpDownStepsNumber.Value;
        }

        private void numericUpDownInitialPositionX_ValueChanged(object sender, EventArgs e)
        {
            Elevator elevator = selectionEvents as Elevator;
            if (elevator != null)
                elevator.InitialPosition = new Vector2((float)numericUpDownInitialPositionX.Value, elevator.InitialPosition.Y);
            MovingPlatform movingPlatform = selectionEvents as MovingPlatform;
            if (movingPlatform != null)
                movingPlatform.InitialPosition = new Vector2((float)numericUpDownInitialPositionX.Value, movingPlatform.InitialPosition.Y);
        }

        private void numericUpDownInitialPositionY_ValueChanged(object sender, EventArgs e)
        {
            Elevator elevator = selectionEvents as Elevator;
            if (elevator != null)
                elevator.InitialPosition = new Vector2(elevator.InitialPosition.X, (float)numericUpDownInitialPositionY.Value);
            MovingPlatform movingPlatform = selectionEvents as MovingPlatform;
            if (movingPlatform != null)
                movingPlatform.InitialPosition = new Vector2(movingPlatform.InitialPosition.X, (float)numericUpDownInitialPositionY.Value);
        }

        private void numericUpDownFinalPositionX_ValueChanged(object sender, EventArgs e)
        {
            Elevator elevator = selectionEvents as Elevator;
            if (elevator != null)
                elevator.FinalPosition = new Vector2((float)numericUpDownFinalPositionX.Value, elevator.FinalPosition.Y);
            MovingPlatform movingPlatform = selectionEvents as MovingPlatform;
            if (movingPlatform != null)
                movingPlatform.FinalPosition = new Vector2((float)numericUpDownFinalPositionX.Value, movingPlatform.FinalPosition.Y);
        }

        private void numericUpDownFinalPositionY_ValueChanged(object sender, EventArgs e)
        {
            Elevator elevator = selectionEvents as Elevator;
            if (elevator != null)
                elevator.FinalPosition = new Vector2(elevator.FinalPosition.X, (float)numericUpDownFinalPositionY.Value);
            MovingPlatform movingPlatform = selectionEvents as MovingPlatform;
            if (movingPlatform != null)
                movingPlatform.FinalPosition = new Vector2(movingPlatform.FinalPosition.X, (float)numericUpDownFinalPositionY.Value);
        }

        private void checkBoxShowDebug_CheckedChanged(object sender, EventArgs e)
        {
            scene.PhysicsDebug.Enabled = checkBoxShowDebug.Checked;
        }

        private void checkBoxPhysicsEngine_CheckedChanged(object sender, EventArgs e)
        {
            scene.World.Enabled = checkBoxPhysicsEngine.Checked;
        }

        private void listBoxAvailableElements_SelectedValueChanged(object sender, EventArgs e)
        {
            NewElementType = listBoxAvailableElements.Text;
        }

        private void numericUpDownLinksNumber_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((ConveyorBelt)selectionEvents).LinksNumber = (int)numericUpDownLinksNumber.Value;
        }

        private void numericUpDownAngularSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((ConveyorBelt)selectionEvents).AngularSpeed = (float)numericUpDownAngularSpeed.Value;
        }

        private void numericUpDownLinkWidth_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((ConveyorBelt)selectionEvents).LinkWidth = (float)numericUpDownLinkWidth.Value;
        }

        private void numericUpDownLinkHeight_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((ConveyorBelt)selectionEvents).LinkHeight = (float)numericUpDownLinkHeight.Value;
        }

        private void numericUpDownRotorsNumber_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((ConveyorBelt)selectionEvents).RotorsNumber = (int)numericUpDownRotorsNumber.Value;
        }

        private void checkBoxShowEmblems_CheckedChanged(object sender, EventArgs e)
        {
            SelectionManager.ShowEmblems = checkBoxShowEmblems.Checked;
        }

        private void numericUpDownAcceleration_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((ImpulsePlatform)selectionEvents).Acceleration = (float)numericUpDownAcceleration.Value;
        }

        private void textBoxNextLevel_TextChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((Endpoint)selectionEvents).NextLevel = textBoxNextLevel.Text;
        }

        private void comboBoxColor_TextChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((Crate)selectionEvents).Color = comboBoxColor.Text;
        }

        private void textBoxSoundName_TextChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((Sound)selectionEvents).SoundName = textBoxSoundName.Text;
        }

        private void numericUpDownVolume_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((Sound)selectionEvents).Volume = (float)numericUpDownVolume.Value;
        }

        private void numericUpDownScale_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents as Background != null)
                ((Background)selectionEvents).Scale = (float)numericUpDownScale.Value;
            if (selectionEvents as Foreground != null)
                ((Foreground)selectionEvents).Scale = (float)numericUpDownScale.Value;
        }

        private void textBoxOtherTubeId_TextChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((ExperimentalTube)selectionEvents).OtherTubeId = textBoxOtherTubeId.Text;
        }

        private void numericUpDownVelocity_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((GlidePlatform)selectionEvents).Velocity = (float)numericUpDownVelocity.Value;
        }

        private void numericUpDownScaleTarget_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((CameraScale)selectionEvents).ScaleTarget = (float)numericUpDownScaleTarget.Value;
        }

        private void numericUpDownLeftShift_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((Crane)selectionEvents).LeftShift = (float)numericUpDownLeftShift.Value;
        }

        private void numericUpDownRightShift_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((Crane)selectionEvents).RightShift = (float)numericUpDownRightShift.Value;
        }

        private void numericUpDownUpShift_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((Crane)selectionEvents).UpShift = (float)numericUpDownUpShift.Value;
        }

        private void numericUpDownDownShift_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((Crane)selectionEvents).DownShift = (float)numericUpDownDownShift.Value;
        }

        private void textBoxText_TextChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((Hint)selectionEvents).Text = textBoxText.Text;
        }
    }
}
