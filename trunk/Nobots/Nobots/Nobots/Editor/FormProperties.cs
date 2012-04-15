using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

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
                numericUpDownWidth.Enabled = false;
                numericUpDownHeight.Enabled = false;
                numericUpDownRotation.Enabled = false;
                flowLayoutPanelTextureName.Visible = true;
                textBoxTextureName.Text = ((Background)selection).TextureName;
                flowLayoutPanelSpeed.Visible = true;
                numericUpDownSpeedX.Value = (decimal)((Background)selection).Speed.X;
                numericUpDownSpeedY.Value = (decimal)((Background)selection).Speed.Y;
            }

            if (selection is IActivable)
            {
                flowLayoutPanelActive.Visible = true;
                checkBoxActive.Checked = ((IActivable)selection).Active;
            }

            if (selection is Activator)
            {
                flowLayoutPanelActivableElementId.Visible = true;
                textBoxActivableElementId.Text = ((Activator)selection).ActivableElementId;
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

            if (selection is Socket)
            {
                flowLayoutPanelOtherSocketId.Visible = true;
                textBoxOtherSocketId.Text = ((Socket)selection).OtherSocketId;
            }
            
            if (selection is Ladder)
            {
                flowLayoutPanelStepsNumber.Visible = true;
                numericUpDownStepsNumber.Value = (decimal)((Ladder)selection).StepsNumber;
            }
        }

        private void reset()
        {
            labelElementType.Text = "(no selection)";
            textBoxId.Text = "";
            numericUpDownPositionX.Value = 0;
            numericUpDownPositionY.Value = 0;
            numericUpDownWidth.Value = 0;
            numericUpDownHeight.Value = 0;
            numericUpDownRotation.Value = 0;

            //visibility
            flowLayoutPanelActivableElementId.Visible = false;
            flowLayoutPanelActive.Visible = false;
            flowLayoutPanelFinalPosition.Visible = false;
            flowLayoutPanelInitialPosition.Visible = false;
            flowLayoutPanelOtherSocketId.Visible = false;
            flowLayoutPanelSpeed.Visible = false;
            flowLayoutPanelStepsNumber.Visible = false;
            flowLayoutPanelTextureName.Visible = false;
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
            if (selectionEvents != null)
                ((Background)selectionEvents).TextureName = textBoxTextureName.Text;
        }

        private void numericUpDownSpeedX_ValueChanged(object sender, EventArgs e)
        {
            Background b = selectionEvents as Background;
            if (b != null)
                b.Speed = new Vector2((float)numericUpDownSpeedX.Value, b.Speed.Y);
        }

        private void numericUpDownSpeedY_ValueChanged(object sender, EventArgs e)
        {
            Background b = selectionEvents as Background;
            if (b != null)
                b.Speed = new Vector2(b.Speed.X, (float)numericUpDownSpeedY.Value);
        }

        private void checkBoxActive_CheckedChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((IActivable)selectionEvents).Active = checkBoxActive.Checked;
        }

        private void textBoxActivableElementId_TextChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((Activator)selectionEvents).ActivableElementId = textBoxActivableElementId.Text;
        }

        private void textBoxOtherSocketId_TextChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((Socket)selectionEvents).OtherSocketId = textBoxOtherSocketId.Text;
        }

        private void numericUpDownStepsNumber_ValueChanged(object sender, EventArgs e)
        {
            if (selectionEvents != null)
                ((Ladder)selectionEvents).StepsNumber = (int)numericUpDownStepsNumber.Value;
        }

        private void numericUpDownInitialPositionX_ValueChanged(object sender, EventArgs e)
        {
            Elevator elevator = selectionEvents as Elevator;
            if (elevator != null)
                elevator.InitialPosition = new Vector2((float)numericUpDownInitialPositionX.Value, elevator.InitialPosition.Y);
        }

        private void numericUpDownInitialPositionY_ValueChanged(object sender, EventArgs e)
        {
            Elevator elevator = selectionEvents as Elevator;
            if (elevator != null)
                elevator.InitialPosition = new Vector2(elevator.InitialPosition.X, (float)numericUpDownInitialPositionY.Value);
        }

        private void numericUpDownFinalPositionX_ValueChanged(object sender, EventArgs e)
        {
            Elevator elevator = selectionEvents as Elevator;
            if (elevator != null)
                elevator.FinalPosition = new Vector2((float)numericUpDownFinalPositionX.Value, elevator.FinalPosition.Y);
        }

        private void numericUpDownFinalPositionY_ValueChanged(object sender, EventArgs e)
        {
            Elevator elevator = selectionEvents as Elevator;
            if (elevator != null)
                elevator.FinalPosition = new Vector2(elevator.FinalPosition.X, (float)numericUpDownFinalPositionY.Value);
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
    }
}
