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
                numericUpDownSpeed_x.Value = (decimal)((Background)selectedElement).Speed.X;
                numericUpDownSpeed_y.Value = (decimal)((Background)selectedElement).Speed.Y;
            }
            else if (selectedElement is IActivable)
            {
                flowLayoutPanelActive.Visible = true;
                checkBoxActive.Checked = ((IActivable)selectedElement).Active;
            }
            else if (selectedElement is Activator)
            {
                flowLayoutPanelActivableElementId.Visible = true;
                textBoxActivableElementId.Text = ((Activator)selectedElement).ActivableElementId;
            }

            if (selectedElement is Elevator)
            {
                flowLayoutPanelInitialPosition.Visible = true;
                flowLayoutPanelFinalPosition.Visible = true;
                numericUpDownInitialPosition_x.Value = (decimal)((Elevator)selectedElement).InitialPosition.X;
                numericUpDownInitialPosition_y.Value = (decimal)((Elevator)selectedElement).InitialPosition.Y;
                numericUpDownFinalPosition_x.Value = (decimal)((Elevator)selectedElement).FinalPosition.X;
                numericUpDownFinalPosition_y.Value = (decimal)((Elevator)selectedElement).FinalPosition.Y;
            }
            else if (selectedElement is Socket)
            {
                flowLayoutPanelOtherSocketId.Visible = true;
                textBoxOtherSocketId.Text = ((Socket)selectedElement).OtherSocketId;
            }
            else if (selectedElement is Ladder)
            {
                flowLayoutPanelStepsNumber.Visible = true;
                numericUpDownStepsNumber.Value = (decimal)((Ladder)selectedElement).StepsNumber;
            }
        }

        private void reset()
        {
            labelElementType.Text = "";
            textBoxId.Clear();
            numericUpDownPosition_x.Value = 0;
            numericUpDownPosition_y.Value = 0;
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
            numericUpDownPosition_x.Value = (decimal)selectedElement.Position.X;
            numericUpDownPosition_y.Value = (decimal)selectedElement.Position.Y;
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
