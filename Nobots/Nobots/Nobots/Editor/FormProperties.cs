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
                showElementInForm();
            }
        }

        private void showElementInForm()
        {
            textBoxId.Text = selectedElement.Id;
            numericUpDownPosition_x.Value = (decimal)selectedElement.Position.X;
            numericUpDownPosition_y.Value = (decimal)selectedElement.Position.Y;
            numericUpDownWidth.Value = (decimal)selectedElement.Width;
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
