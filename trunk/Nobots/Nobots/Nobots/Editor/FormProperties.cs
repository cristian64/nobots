using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nobots.Editor
{
    public partial class FormProperties : Form
    {
        private Element selectedElement;
        public Element SelectedElement
        {
            get { return selectedElement; }
            set 
            {
                selectedElement = value;
                reset();
                if(selectedElement != null)
                    showElementInForm();
            }
        }

        private void showElementInForm()
        {
            setInitialValues();
            
            if (selectedElement is Background)
            {
                numericUpDownWidth.Enabled = false;
                numericUpDownHeight.Enabled = false;
                numericUpDownRotation.Enabled = false;
                flowLayoutPanelTextureName.Visible = true;
                textBoxTextureName.Text = ((Background)selectedElement).TextureName;
                flowLayoutPanelSpeed.Visible = true;
                numericUpDownSpeedX.Value = (decimal)((Background)selectedElement).Speed.X;
                numericUpDownSpeedY.Value = (decimal)((Background)selectedElement).Speed.Y;
            }

            if (selectedElement is IActivable)
            {
                flowLayoutPanelActive.Visible = true;
                checkBoxActive.Checked = ((IActivable)selectedElement).Active;
            }

            if (selectedElement is Activator)
            {
                flowLayoutPanelActivableElementId.Visible = true;
                textBoxActivableElementId.Text = ((Activator)selectedElement).ActivableElementId;
            }

            if (selectedElement is Elevator)
            {
                flowLayoutPanelInitialPosition.Visible = true;
                flowLayoutPanelFinalPosition.Visible = true;
                numericUpDownInitialPositionX.Value = (decimal)((Elevator)selectedElement).InitialPosition.X;
                numericUpDownInitialPositionY.Value = (decimal)((Elevator)selectedElement).InitialPosition.Y;
                numericUpDownFinalPositionX.Value = (decimal)((Elevator)selectedElement).FinalPosition.X;
                numericUpDownFinalPositionY.Value = (decimal)((Elevator)selectedElement).FinalPosition.Y;
            }

            if (selectedElement is Socket)
            {
                flowLayoutPanelOtherSocketId.Visible = true;
                textBoxOtherSocketId.Text = ((Socket)selectedElement).OtherSocketId;
            }
            
            if (selectedElement is Ladder)
            {
                flowLayoutPanelStepsNumber.Visible = true;
                numericUpDownStepsNumber.Value = (decimal)((Ladder)selectedElement).StepsNumber;
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
            labelElementType.Text = selectedElement.GetType().Name;
            textBoxId.Text = selectedElement.Id;
            numericUpDownPositionX.Value = (decimal)selectedElement.Position.X;
            numericUpDownPositionY.Value = (decimal)selectedElement.Position.Y;
            numericUpDownWidth.Value = (decimal)selectedElement.Width;
            numericUpDownHeight.Value = (decimal)selectedElement.Height;
            numericUpDownRotation.Value = (decimal)selectedElement.Rotation;
        }

        public FormProperties()
        {
            InitializeComponent();
        }

        private void FormProperties_Load(object sender, EventArgs e)
        {

        }
    }
}
