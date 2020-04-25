namespace Alcances
{
    partial class frmKinectVideo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKinectVideo));
            System.Windows.Media.SolidColorBrush solidColorBrush2 = new System.Windows.Media.SolidColorBrush();
            this.pctVideoColor = new System.Windows.Forms.PictureBox();
            this.btnStream = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblConnectionID = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblRightHandTip = new System.Windows.Forms.Label();
            this.lblE1V = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblE1 = new System.Windows.Forms.Label();
            this.lblAngleV = new System.Windows.Forms.Label();
            this.lblAngle = new System.Windows.Forms.Label();
            this.numSkeleton = new System.Windows.Forms.NumericUpDown();
            this.pctSkeleton = new System.Windows.Forms.PictureBox();
            this.numAngle = new System.Windows.Forms.NumericUpDown();
            this.pctAngle = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblObliquity = new System.Windows.Forms.Label();
            this.lblE2V = new System.Windows.Forms.Label();
            this.lblE2 = new System.Windows.Forms.Label();
            this.dlgColor = new System.Windows.Forms.ColorDialog();
            this.chartA = new LiveCharts.WinForms.CartesianChart();
            this.chartB = new LiveCharts.WinForms.CartesianChart();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblSagitalVDeviation = new System.Windows.Forms.Label();
            this.lblFloor = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pctVideoColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSkeleton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctSkeleton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctAngle)).BeginInit();
            this.SuspendLayout();
            // 
            // pctVideoColor
            // 
            this.pctVideoColor.Location = new System.Drawing.Point(13, 248);
            this.pctVideoColor.Margin = new System.Windows.Forms.Padding(4);
            this.pctVideoColor.Name = "pctVideoColor";
            this.pctVideoColor.Size = new System.Drawing.Size(800, 450);
            this.pctVideoColor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pctVideoColor.TabIndex = 0;
            this.pctVideoColor.TabStop = false;
            // 
            // btnStream
            // 
            this.btnStream.Location = new System.Drawing.Point(13, 188);
            this.btnStream.Margin = new System.Windows.Forms.Padding(4);
            this.btnStream.Name = "btnStream";
            this.btnStream.Size = new System.Drawing.Size(69, 28);
            this.btnStream.TabIndex = 1;
            this.btnStream.Text = "Stream";
            this.btnStream.UseVisualStyleBackColor = true;
            this.btnStream.Visible = false;
            this.btnStream.Click += new System.EventHandler(this.btnStream_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 134);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Sensor status:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(115, 134);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(13, 17);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 164);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Connection ID:";
            // 
            // lblConnectionID
            // 
            this.lblConnectionID.AutoSize = true;
            this.lblConnectionID.Location = new System.Drawing.Point(115, 164);
            this.lblConnectionID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConnectionID.Name = "lblConnectionID";
            this.lblConnectionID.Size = new System.Drawing.Size(13, 17);
            this.lblConnectionID.TabIndex = 5;
            this.lblConnectionID.Text = "-";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(233, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Right hand tip height:";
            // 
            // lblRightHandTip
            // 
            this.lblRightHandTip.AutoSize = true;
            this.lblRightHandTip.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblRightHandTip.Location = new System.Drawing.Point(373, 130);
            this.lblRightHandTip.Name = "lblRightHandTip";
            this.lblRightHandTip.Size = new System.Drawing.Size(16, 24);
            this.lblRightHandTip.TabIndex = 7;
            this.lblRightHandTip.Text = "-";
            // 
            // lblE1V
            // 
            this.lblE1V.AutoSize = true;
            this.lblE1V.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblE1V.Location = new System.Drawing.Point(373, 184);
            this.lblE1V.Name = "lblE1V";
            this.lblE1V.Size = new System.Drawing.Size(16, 24);
            this.lblE1V.TabIndex = 8;
            this.lblE1V.Text = "-";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(233, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "SagitalV projection:";
            // 
            // lblE1
            // 
            this.lblE1.AutoSize = true;
            this.lblE1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblE1.Location = new System.Drawing.Point(373, 211);
            this.lblE1.Name = "lblE1";
            this.lblE1.Size = new System.Drawing.Size(16, 24);
            this.lblE1.TabIndex = 10;
            this.lblE1.Text = "-";
            // 
            // lblAngleV
            // 
            this.lblAngleV.AutoSize = true;
            this.lblAngleV.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAngleV.Location = new System.Drawing.Point(655, 184);
            this.lblAngleV.Name = "lblAngleV";
            this.lblAngleV.Size = new System.Drawing.Size(16, 24);
            this.lblAngleV.TabIndex = 11;
            this.lblAngleV.Text = "-";
            // 
            // lblAngle
            // 
            this.lblAngle.AutoSize = true;
            this.lblAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAngle.Location = new System.Drawing.Point(655, 211);
            this.lblAngle.Name = "lblAngle";
            this.lblAngle.Size = new System.Drawing.Size(16, 24);
            this.lblAngle.TabIndex = 12;
            this.lblAngle.Text = "-";
            // 
            // numSkeleton
            // 
            this.numSkeleton.Location = new System.Drawing.Point(629, 132);
            this.numSkeleton.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSkeleton.Name = "numSkeleton";
            this.numSkeleton.Size = new System.Drawing.Size(45, 23);
            this.numSkeleton.TabIndex = 21;
            this.numSkeleton.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numSkeleton.ValueChanged += new System.EventHandler(this.size_Changed);
            // 
            // pctSkeleton
            // 
            this.pctSkeleton.BackColor = System.Drawing.Color.Red;
            this.pctSkeleton.Location = new System.Drawing.Point(578, 132);
            this.pctSkeleton.Name = "pctSkeleton";
            this.pctSkeleton.Size = new System.Drawing.Size(45, 23);
            this.pctSkeleton.TabIndex = 20;
            this.pctSkeleton.TabStop = false;
            this.pctSkeleton.Click += new System.EventHandler(this.color_Click);
            // 
            // numAngle
            // 
            this.numAngle.Location = new System.Drawing.Point(758, 132);
            this.numAngle.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numAngle.Name = "numAngle";
            this.numAngle.Size = new System.Drawing.Size(45, 23);
            this.numAngle.TabIndex = 22;
            this.numAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numAngle.ValueChanged += new System.EventHandler(this.size_Changed);
            // 
            // pctAngle
            // 
            this.pctAngle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.pctAngle.Location = new System.Drawing.Point(707, 132);
            this.pctAngle.Name = "pctAngle";
            this.pctAngle.Size = new System.Drawing.Size(45, 23);
            this.pctAngle.TabIndex = 23;
            this.pctAngle.TabStop = false;
            this.pctAngle.Click += new System.EventHandler(this.color_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(233, 215);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Sagital projection:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(536, 215);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(116, 17);
            this.label8.TabIndex = 15;
            this.label8.Text = "Sagital elevation:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(233, 161);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 17);
            this.label9.TabIndex = 16;
            this.label9.Text = "Obliquity:";
            // 
            // lblObliquity
            // 
            this.lblObliquity.AutoSize = true;
            this.lblObliquity.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblObliquity.Location = new System.Drawing.Point(373, 157);
            this.lblObliquity.Name = "lblObliquity";
            this.lblObliquity.Size = new System.Drawing.Size(16, 24);
            this.lblObliquity.TabIndex = 17;
            this.lblObliquity.Text = "-";
            // 
            // lblE2V
            // 
            this.lblE2V.AutoSize = true;
            this.lblE2V.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblE2V.Location = new System.Drawing.Point(447, 184);
            this.lblE2V.Name = "lblE2V";
            this.lblE2V.Size = new System.Drawing.Size(16, 24);
            this.lblE2V.TabIndex = 18;
            this.lblE2V.Text = "-";
            // 
            // lblE2
            // 
            this.lblE2.AutoSize = true;
            this.lblE2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblE2.Location = new System.Drawing.Point(447, 211);
            this.lblE2.Name = "lblE2";
            this.lblE2.Size = new System.Drawing.Size(16, 24);
            this.lblE2.TabIndex = 19;
            this.lblE2.Text = "-";
            // 
            // chartA
            // 
            this.chartA.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chartA.BackColor = System.Drawing.SystemColors.Control;
            this.chartA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartA.Hoverable = false;
            this.chartA.Location = new System.Drawing.Point(831, 146);
            this.chartA.Name = "chartA";
            solidColorBrush2.Color = System.Windows.Media.Color.FromArgb(((byte)(30)), ((byte)(30)), ((byte)(30)), ((byte)(30)));
            this.chartA.ScrollBarFill = solidColorBrush2;
            this.chartA.ScrollHorizontalFrom = 0D;
            this.chartA.ScrollHorizontalTo = 0D;
            this.chartA.ScrollMode = LiveCharts.ScrollMode.None;
            this.chartA.ScrollVerticalFrom = 0D;
            this.chartA.ScrollVerticalTo = 0D;
            this.chartA.Size = new System.Drawing.Size(390, 270);
            this.chartA.TabIndex = 24;
            this.chartA.Text = "Angle";
            // 
            // chartB
            // 
            this.chartB.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chartB.Hoverable = true;
            this.chartB.Location = new System.Drawing.Point(831, 422);
            this.chartB.Name = "chartB";
            this.chartB.ScrollBarFill = solidColorBrush2;
            this.chartB.ScrollHorizontalFrom = 0D;
            this.chartB.ScrollHorizontalTo = 0D;
            this.chartB.ScrollMode = LiveCharts.ScrollMode.None;
            this.chartB.ScrollVerticalFrom = 0D;
            this.chartB.ScrollVerticalTo = 0D;
            this.chartB.Size = new System.Drawing.Size(390, 270);
            this.chartB.TabIndex = 25;
            this.chartB.Text = "Angle";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(527, 188);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(125, 17);
            this.label10.TabIndex = 26;
            this.label10.Text = "SagitalV elevation:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(436, 161);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(125, 17);
            this.label11.TabIndex = 27;
            this.label11.Text = "SagitalV deviation:";
            // 
            // lblSagitalVDeviation
            // 
            this.lblSagitalVDeviation.AutoSize = true;
            this.lblSagitalVDeviation.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblSagitalVDeviation.Location = new System.Drawing.Point(560, 157);
            this.lblSagitalVDeviation.Name = "lblSagitalVDeviation";
            this.lblSagitalVDeviation.Size = new System.Drawing.Size(16, 24);
            this.lblSagitalVDeviation.TabIndex = 28;
            this.lblSagitalVDeviation.Text = "-";
            // 
            // lblFloor
            // 
            this.lblFloor.AutoSize = true;
            this.lblFloor.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblFloor.Location = new System.Drawing.Point(158, 211);
            this.lblFloor.Name = "lblFloor";
            this.lblFloor.Size = new System.Drawing.Size(16, 24);
            this.lblFloor.TabIndex = 29;
            this.lblFloor.Text = "-";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(115, 215);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 17);
            this.label5.TabIndex = 30;
            this.label5.Text = "Floor:";
            // 
            // frmKinectVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 711);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblFloor);
            this.Controls.Add(this.lblSagitalVDeviation);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.chartB);
            this.Controls.Add(this.chartA);
            this.Controls.Add(this.pctAngle);
            this.Controls.Add(this.numAngle);
            this.Controls.Add(this.numSkeleton);
            this.Controls.Add(this.pctSkeleton);
            this.Controls.Add(this.lblE2);
            this.Controls.Add(this.lblE2V);
            this.Controls.Add(this.lblObliquity);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblAngle);
            this.Controls.Add(this.lblAngleV);
            this.Controls.Add(this.lblE1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblE1V);
            this.Controls.Add(this.lblRightHandTip);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblConnectionID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStream);
            this.Controls.Add(this.pctVideoColor);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "frmKinectVideo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Visualizador Kinect";
            this.Resize += new System.EventHandler(this.frmKinectVideo_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pctVideoColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSkeleton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctSkeleton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctAngle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pctVideoColor;
        private System.Windows.Forms.Button btnStream;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblConnectionID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblRightHandTip;
        private System.Windows.Forms.Label lblE1V;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblE1;
        private System.Windows.Forms.Label lblAngleV;
        private System.Windows.Forms.Label lblAngle;

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblObliquity;
        private System.Windows.Forms.Label lblE2V;
        private System.Windows.Forms.Label lblE2;

        private System.Windows.Forms.PictureBox pctSkeleton;

        private System.Windows.Forms.NumericUpDown numSkeleton;

        private System.Windows.Forms.NumericUpDown numAngle;
        private System.Windows.Forms.PictureBox pctAngle;
        private System.Windows.Forms.ColorDialog dlgColor;
        private LiveCharts.WinForms.CartesianChart chartA;
        private LiveCharts.WinForms.CartesianChart chartB;

        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblSagitalVDeviation;

        private System.Windows.Forms.Label lblFloor;
        private System.Windows.Forms.Label label5;
    }
}

