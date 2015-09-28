namespace BaseDeDatos
{
    partial class ClaveForanea
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbDB = new System.Windows.Forms.ComboBox();
            this.cbEnt = new System.Windows.Forms.ComboBox();
            this.cbAtr = new System.Windows.Forms.ComboBox();
            this.lDB = new System.Windows.Forms.Label();
            this.lEnt = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbDB
            // 
            this.cbDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDB.FormattingEnabled = true;
            this.cbDB.Location = new System.Drawing.Point(126, 60);
            this.cbDB.Name = "cbDB";
            this.cbDB.Size = new System.Drawing.Size(121, 21);
            this.cbDB.TabIndex = 0;
            this.cbDB.SelectedIndexChanged += new System.EventHandler(this.cbDB_SelectedIndexChanged);
            // 
            // cbEnt
            // 
            this.cbEnt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEnt.FormattingEnabled = true;
            this.cbEnt.Location = new System.Drawing.Point(126, 119);
            this.cbEnt.Name = "cbEnt";
            this.cbEnt.Size = new System.Drawing.Size(121, 21);
            this.cbEnt.TabIndex = 1;
            this.cbEnt.SelectedIndexChanged += new System.EventHandler(this.cbEnt_SelectedIndexChanged);
            // 
            // cbAtr
            // 
            this.cbAtr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAtr.FormattingEnabled = true;
            this.cbAtr.Location = new System.Drawing.Point(126, 191);
            this.cbAtr.Name = "cbAtr";
            this.cbAtr.Size = new System.Drawing.Size(121, 21);
            this.cbAtr.TabIndex = 2;
            // 
            // lDB
            // 
            this.lDB.AutoSize = true;
            this.lDB.Location = new System.Drawing.Point(123, 44);
            this.lDB.Name = "lDB";
            this.lDB.Size = new System.Drawing.Size(144, 13);
            this.lDB.TabIndex = 3;
            this.lDB.Text = "Selecciona la Base de Datos";
            // 
            // lEnt
            // 
            this.lEnt.AutoSize = true;
            this.lEnt.Location = new System.Drawing.Point(123, 103);
            this.lEnt.Name = "lEnt";
            this.lEnt.Size = new System.Drawing.Size(110, 13);
            this.lEnt.TabIndex = 4;
            this.lEnt.Text = "Selecciona la Entidad";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(123, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Selecciona el Atributo";
            // 
            // btnAceptar
            // 
            this.btnAceptar.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAceptar.Location = new System.Drawing.Point(285, 256);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 6;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(12, 256);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // ClaveForanea
            // 
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(372, 291);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lEnt);
            this.Controls.Add(this.lDB);
            this.Controls.Add(this.cbAtr);
            this.Controls.Add(this.cbEnt);
            this.Controls.Add(this.cbDB);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(388, 330);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(388, 330);
            this.Name = "ClaveForanea";
            this.Text = "ClaveForanea";
            this.Load += new System.EventHandler(this.ClaveForanea_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbDB;
        private System.Windows.Forms.ComboBox cbEnt;
        private System.Windows.Forms.ComboBox cbAtr;
        private System.Windows.Forms.Label lDB;
        private System.Windows.Forms.Label lEnt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
    }
}