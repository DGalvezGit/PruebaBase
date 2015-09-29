namespace BaseDeDatos
{
    partial class dUser
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
            this.lNombre = new System.Windows.Forms.Label();
            this.lcontra = new System.Windows.Forms.Label();
            this.tbNombre = new System.Windows.Forms.TextBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.tbContra = new System.Windows.Forms.TextBox();
            this.lPrivilegios = new System.Windows.Forms.Label();
            this.chbAlta = new System.Windows.Forms.CheckBox();
            this.chbBaja = new System.Windows.Forms.CheckBox();
            this.chbConsulta = new System.Windows.Forms.CheckBox();
            this.chbMod = new System.Windows.Forms.CheckBox();
            this.dtpVigIni = new System.Windows.Forms.DateTimePicker();
            this.lVigIni = new System.Windows.Forms.Label();
            this.lVigFinal = new System.Windows.Forms.Label();
            this.dtpVigFin = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // lNombre
            // 
            this.lNombre.AutoSize = true;
            this.lNombre.Location = new System.Drawing.Point(40, 34);
            this.lNombre.Name = "lNombre";
            this.lNombre.Size = new System.Drawing.Size(46, 13);
            this.lNombre.TabIndex = 0;
            this.lNombre.Text = "Usuario:";
            // 
            // lcontra
            // 
            this.lcontra.AutoSize = true;
            this.lcontra.Location = new System.Drawing.Point(22, 60);
            this.lcontra.Name = "lcontra";
            this.lcontra.Size = new System.Drawing.Size(64, 13);
            this.lcontra.TabIndex = 1;
            this.lcontra.Text = "Contraseña:";
            // 
            // tbNombre
            // 
            this.tbNombre.Location = new System.Drawing.Point(92, 31);
            this.tbNombre.Name = "tbNombre";
            this.tbNombre.Size = new System.Drawing.Size(100, 20);
            this.tbNombre.TabIndex = 0;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAceptar.Location = new System.Drawing.Point(279, 155);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 7;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(15, 155);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 8;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // tbContra
            // 
            this.tbContra.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.tbContra.Location = new System.Drawing.Point(92, 57);
            this.tbContra.MaxLength = 10;
            this.tbContra.Name = "tbContra";
            this.tbContra.PasswordChar = '*';
            this.tbContra.Size = new System.Drawing.Size(100, 20);
            this.tbContra.TabIndex = 1;
            // 
            // lPrivilegios
            // 
            this.lPrivilegios.AutoSize = true;
            this.lPrivilegios.Location = new System.Drawing.Point(32, 85);
            this.lPrivilegios.Name = "lPrivilegios";
            this.lPrivilegios.Size = new System.Drawing.Size(57, 13);
            this.lPrivilegios.TabIndex = 7;
            this.lPrivilegios.Text = "Privilegios:";
            // 
            // chbAlta
            // 
            this.chbAlta.AutoSize = true;
            this.chbAlta.Location = new System.Drawing.Point(119, 111);
            this.chbAlta.Name = "chbAlta";
            this.chbAlta.Size = new System.Drawing.Size(44, 17);
            this.chbAlta.TabIndex = 4;
            this.chbAlta.Text = "Alta";
            this.chbAlta.UseVisualStyleBackColor = true;
            // 
            // chbBaja
            // 
            this.chbBaja.AutoSize = true;
            this.chbBaja.Location = new System.Drawing.Point(185, 111);
            this.chbBaja.Name = "chbBaja";
            this.chbBaja.Size = new System.Drawing.Size(47, 17);
            this.chbBaja.TabIndex = 5;
            this.chbBaja.Text = "Baja";
            this.chbBaja.UseVisualStyleBackColor = true;
            // 
            // chbConsulta
            // 
            this.chbConsulta.AutoSize = true;
            this.chbConsulta.Location = new System.Drawing.Point(35, 111);
            this.chbConsulta.Name = "chbConsulta";
            this.chbConsulta.Size = new System.Drawing.Size(67, 17);
            this.chbConsulta.TabIndex = 3;
            this.chbConsulta.Text = "Consulta";
            this.chbConsulta.UseVisualStyleBackColor = true;
            // 
            // chbMod
            // 
            this.chbMod.AutoSize = true;
            this.chbMod.Location = new System.Drawing.Point(247, 111);
            this.chbMod.Name = "chbMod";
            this.chbMod.Size = new System.Drawing.Size(86, 17);
            this.chbMod.TabIndex = 6;
            this.chbMod.Text = "Modificación";
            this.chbMod.UseVisualStyleBackColor = true;
            // 
            // dtpVigIni
            // 
            this.dtpVigIni.Location = new System.Drawing.Point(218, 34);
            this.dtpVigIni.Name = "dtpVigIni";
            this.dtpVigIni.Size = new System.Drawing.Size(136, 20);
            this.dtpVigIni.TabIndex = 2;
            // 
            // lVigIni
            // 
            this.lVigIni.AutoSize = true;
            this.lVigIni.Location = new System.Drawing.Point(218, 15);
            this.lVigIni.Name = "lVigIni";
            this.lVigIni.Size = new System.Drawing.Size(81, 13);
            this.lVigIni.TabIndex = 13;
            this.lVigIni.Text = "Vigencia Inicial:";
            // 
            // lVigFinal
            // 
            this.lVigFinal.AutoSize = true;
            this.lVigFinal.Location = new System.Drawing.Point(218, 60);
            this.lVigFinal.Name = "lVigFinal";
            this.lVigFinal.Size = new System.Drawing.Size(76, 13);
            this.lVigFinal.TabIndex = 15;
            this.lVigFinal.Text = "Vigencia Final:";
            // 
            // dtpVigFin
            // 
            this.dtpVigFin.Location = new System.Drawing.Point(218, 79);
            this.dtpVigFin.Name = "dtpVigFin";
            this.dtpVigFin.Size = new System.Drawing.Size(136, 20);
            this.dtpVigFin.TabIndex = 14;
            // 
            // dUser
            // 
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(369, 190);
            this.Controls.Add(this.lVigFinal);
            this.Controls.Add(this.dtpVigFin);
            this.Controls.Add(this.lVigIni);
            this.Controls.Add(this.dtpVigIni);
            this.Controls.Add(this.chbMod);
            this.Controls.Add(this.chbConsulta);
            this.Controls.Add(this.chbBaja);
            this.Controls.Add(this.chbAlta);
            this.Controls.Add(this.lPrivilegios);
            this.Controls.Add(this.tbContra);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.tbNombre);
            this.Controls.Add(this.lcontra);
            this.Controls.Add(this.lNombre);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dUser";
            this.Text = "Agrega Usuario";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lNombre;
        private System.Windows.Forms.Label lcontra;
        private System.Windows.Forms.TextBox tbNombre;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.TextBox tbContra;
        private System.Windows.Forms.Label lPrivilegios;
        private System.Windows.Forms.CheckBox chbAlta;
        private System.Windows.Forms.CheckBox chbBaja;
        private System.Windows.Forms.CheckBox chbConsulta;
        private System.Windows.Forms.CheckBox chbMod;
        private System.Windows.Forms.DateTimePicker dtpVigIni;
        private System.Windows.Forms.Label lVigIni;
        private System.Windows.Forms.Label lVigFinal;
        private System.Windows.Forms.DateTimePicker dtpVigFin;
    }

}