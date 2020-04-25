namespace AlcancesMesaConversion
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmdConvert = new System.Windows.Forms.Button();
            this.cmdInput = new System.Windows.Forms.Button();
            this.txtRotation = new System.Windows.Forms.TextBox();
            this.lblRotation = new System.Windows.Forms.Label();
            this.cmdOutput = new System.Windows.Forms.Button();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.lblRotationLateral = new System.Windows.Forms.Label();
            this.txtRotationLateral = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmdConvert
            // 
            this.cmdConvert.Location = new System.Drawing.Point(426, 226);
            this.cmdConvert.Margin = new System.Windows.Forms.Padding(4);
            this.cmdConvert.Name = "cmdConvert";
            this.cmdConvert.Size = new System.Drawing.Size(98, 32);
            this.cmdConvert.TabIndex = 0;
            this.cmdConvert.Text = "Convert";
            this.cmdConvert.UseVisualStyleBackColor = true;
            this.cmdConvert.Click += new System.EventHandler(this.cmdConvert_Click);
            // 
            // cmdInput
            // 
            this.cmdInput.Location = new System.Drawing.Point(15, 119);
            this.cmdInput.Name = "cmdInput";
            this.cmdInput.Size = new System.Drawing.Size(98, 32);
            this.cmdInput.TabIndex = 1;
            this.cmdInput.Text = "Input file";
            this.cmdInput.UseVisualStyleBackColor = true;
            this.cmdInput.Click += new System.EventHandler(this.cmdInput_Click);
            // 
            // txtRotation
            // 
            this.txtRotation.Location = new System.Drawing.Point(143, 37);
            this.txtRotation.Name = "txtRotation";
            this.txtRotation.Size = new System.Drawing.Size(86, 23);
            this.txtRotation.TabIndex = 2;
            // 
            // lblRotation
            // 
            this.lblRotation.AutoSize = true;
            this.lblRotation.Location = new System.Drawing.Point(12, 40);
            this.lblRotation.Name = "lblRotation";
            this.lblRotation.Size = new System.Drawing.Size(129, 17);
            this.lblRotation.TabIndex = 3;
            this.lblRotation.Text = "Frontal tilt angle (°)";
            // 
            // cmdOutput
            // 
            this.cmdOutput.Location = new System.Drawing.Point(15, 166);
            this.cmdOutput.Name = "cmdOutput";
            this.cmdOutput.Size = new System.Drawing.Size(97, 32);
            this.cmdOutput.TabIndex = 4;
            this.cmdOutput.Text = "Output file";
            this.cmdOutput.UseVisualStyleBackColor = true;
            this.cmdOutput.Click += new System.EventHandler(this.cmdOutput_Click);
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(137, 124);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(370, 23);
            this.txtInput.TabIndex = 7;
            this.txtInput.Leave += new System.EventHandler(this.txtInput_Leave);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(137, 171);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(369, 23);
            this.txtOutput.TabIndex = 8;
            this.txtOutput.Leave += new System.EventHandler(this.txtOutput_Leave);
            // 
            // lblRotationLateral
            // 
            this.lblRotationLateral.AutoSize = true;
            this.lblRotationLateral.Location = new System.Drawing.Point(12, 82);
            this.lblRotationLateral.Name = "lblRotationLateral";
            this.lblRotationLateral.Size = new System.Drawing.Size(129, 17);
            this.lblRotationLateral.TabIndex = 9;
            this.lblRotationLateral.Text = "Lateral tilt angle (°)";
            // 
            // txtRotationLateral
            // 
            this.txtRotationLateral.Location = new System.Drawing.Point(142, 79);
            this.txtRotationLateral.Name = "txtRotationLateral";
            this.txtRotationLateral.Size = new System.Drawing.Size(86, 23);
            this.txtRotationLateral.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(241, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "(around the Z axis)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 17);
            this.label2.TabIndex = 12;
            this.label2.Text = "(around the X axis)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 272);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRotationLateral);
            this.Controls.Add(this.lblRotationLateral);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.cmdOutput);
            this.Controls.Add(this.lblRotation);
            this.Controls.Add(this.txtRotation);
            this.Controls.Add(this.cmdInput);
            this.Controls.Add(this.cmdConvert);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Coordinates converter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdConvert;
        private System.Windows.Forms.Button cmdInput;
        private System.Windows.Forms.TextBox txtRotation;
        private System.Windows.Forms.Label lblRotation;
        private System.Windows.Forms.Button cmdOutput;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label lblRotationLateral;
        private System.Windows.Forms.TextBox txtRotationLateral;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

