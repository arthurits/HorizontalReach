namespace LiveChartsTest
{
    partial class frmMain
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

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.chartA = new LiveCharts.WinForms.CartesianChart();
            this.lblAnimationSpeed = new System.Windows.Forms.Label();
            this.txtAnimationSpeed = new System.Windows.Forms.TextBox();
            this.cmdTimer = new System.Windows.Forms.Button();
            this.lblTimerInterval = new System.Windows.Forms.Label();
            this.txtTimerInterval = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // chartA
            // 
            this.chartA.Location = new System.Drawing.Point(16, 15);
            this.chartA.Margin = new System.Windows.Forms.Padding(4);
            this.chartA.Name = "chartA";
            this.chartA.Size = new System.Drawing.Size(819, 462);
            this.chartA.TabIndex = 0;
            this.chartA.Text = "cartesianChart1";
            // 
            // lblAnimationSpeed
            // 
            this.lblAnimationSpeed.AutoSize = true;
            this.lblAnimationSpeed.Location = new System.Drawing.Point(13, 488);
            this.lblAnimationSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationSpeed.Name = "lblAnimationSpeed";
            this.lblAnimationSpeed.Size = new System.Drawing.Size(145, 17);
            this.lblAnimationSpeed.TabIndex = 1;
            this.lblAnimationSpeed.Text = "Animation speed (ms)";
            // 
            // txtAnimationSpeed
            // 
            this.txtAnimationSpeed.Location = new System.Drawing.Point(166, 485);
            this.txtAnimationSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.txtAnimationSpeed.Name = "txtAnimationSpeed";
            this.txtAnimationSpeed.Size = new System.Drawing.Size(80, 23);
            this.txtAnimationSpeed.TabIndex = 2;
            this.txtAnimationSpeed.Text = "10";
            // 
            // cmdTimer
            // 
            this.cmdTimer.Location = new System.Drawing.Point(721, 488);
            this.cmdTimer.Margin = new System.Windows.Forms.Padding(4);
            this.cmdTimer.Name = "cmdTimer";
            this.cmdTimer.Size = new System.Drawing.Size(114, 36);
            this.cmdTimer.TabIndex = 3;
            this.cmdTimer.Text = "&Start";
            this.cmdTimer.UseVisualStyleBackColor = true;
            this.cmdTimer.Click += new System.EventHandler(this.cmdTimer_Click);
            // 
            // lblTimerInterval
            // 
            this.lblTimerInterval.AutoSize = true;
            this.lblTimerInterval.Location = new System.Drawing.Point(12, 518);
            this.lblTimerInterval.Name = "lblTimerInterval";
            this.lblTimerInterval.Size = new System.Drawing.Size(126, 17);
            this.lblTimerInterval.TabIndex = 4;
            this.lblTimerInterval.Text = "Timer interval (ms)";
            // 
            // txtTimerInterval
            // 
            this.txtTimerInterval.Location = new System.Drawing.Point(166, 515);
            this.txtTimerInterval.Name = "txtTimerInterval";
            this.txtTimerInterval.Size = new System.Drawing.Size(80, 23);
            this.txtTimerInterval.TabIndex = 5;
            this.txtTimerInterval.Text = "1000";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 554);
            this.Controls.Add(this.txtTimerInterval);
            this.Controls.Add(this.lblTimerInterval);
            this.Controls.Add(this.cmdTimer);
            this.Controls.Add(this.txtAnimationSpeed);
            this.Controls.Add(this.lblAnimationSpeed);
            this.Controls.Add(this.chartA);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LiveCharts.WinForms.CartesianChart chartA;
        private System.Windows.Forms.Label lblAnimationSpeed;
        private System.Windows.Forms.TextBox txtAnimationSpeed;
        private System.Windows.Forms.Button cmdTimer;
        private System.Windows.Forms.Label lblTimerInterval;
        private System.Windows.Forms.TextBox txtTimerInterval;
    }
}