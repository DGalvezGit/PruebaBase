using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseDeDatos
{
    public partial class DModificaDato : Form
    {
        public DModificaDato()
        {
            InitializeComponent();
        }

        public DModificaDato(string nomAtr)
        {
            InitializeComponent();
            this.lAtr.Text = nomAtr;
        }

        private void DModificaDato_Load(object sender, EventArgs e)
        {
            this.lAtr.Size = TextRenderer.MeasureText(this.lAtr.Text, this.lAtr.Font);
            this.lAtr.Location = new Point(this.textBox1.Location.X - this.lAtr.Width - 3, this.lAtr.Location.Y);
        }

        public string dato()
        {
            return this.textBox1.Text;
        }
    }
}
