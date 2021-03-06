﻿namespace AlcancesMesa
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pctVideoColor = new System.Windows.Forms.PictureBox();
            this.tspTop = new System.Windows.Forms.ToolStripPanel();
            this.tspBottom = new System.Windows.Forms.ToolStripPanel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusStripLabelID = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripLabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripLabelXtras = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripLabelVideo = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripLabelData = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripLabelMirror = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripLabelPlots = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripLabelSkeleton = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripLabelJoint = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripLabelAngle = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripMain_Exit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMain_Connect = new System.Windows.Forms.ToolStripButton();
            this.toolStripMain_Disconnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMain_Video = new System.Windows.Forms.ToolStripButton();
            this.toolStripMain_Data = new System.Windows.Forms.ToolStripButton();
            this.toolStripMain_Picture = new System.Windows.Forms.ToolStripButton();
            this.toolStripMain_Mirror = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMain_Plots = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMain_Skeleton = new System.Windows.Forms.ToolStripButton();
            this.toolStripMain_SkeletonWidth = new System.Windows.Forms.ToolStripNumericUpDown();
            this.toolStripMain_Joint = new System.Windows.Forms.ToolStripButton();
            this.toolStripMain_JointWidth = new System.Windows.Forms.ToolStripNumericUpDown();
            this.toolStripMain_Angle = new System.Windows.Forms.ToolStripButton();
            this.toolStripMain_AngleWidth = new System.Windows.Forms.ToolStripNumericUpDown();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMain_About = new System.Windows.Forms.ToolStripButton();
            this.mnuMainFrm = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectKinectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconectKinectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chartA = new LiveCharts.WinForms.CartesianChart();
            this.chartB = new LiveCharts.WinForms.CartesianChart();
            this.bckWorkerDraw = new System.ComponentModel.BackgroundWorker();
            this.chartC = new LiveCharts.WinForms.AngularGauge();
            this.txtTiltX = new System.Windows.Forms.TextBox();
            this.txtTiltZ = new System.Windows.Forms.TextBox();
            this.lblTiltX = new System.Windows.Forms.Label();
            this.lblTiltZ = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pctVideoColor)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.mnuMainFrm.SuspendLayout();
            this.SuspendLayout();
            // 
            // pctVideoColor
            // 
            this.pctVideoColor.Location = new System.Drawing.Point(13, 107);
            this.pctVideoColor.Margin = new System.Windows.Forms.Padding(4);
            this.pctVideoColor.Name = "pctVideoColor";
            this.pctVideoColor.Size = new System.Drawing.Size(850, 478);
            this.pctVideoColor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pctVideoColor.TabIndex = 0;
            this.pctVideoColor.TabStop = false;
            // 
            // tspTop
            // 
            this.tspTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.tspTop.Location = new System.Drawing.Point(0, 0);
            this.tspTop.Name = "tspTop";
            this.tspTop.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.tspTop.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.tspTop.Size = new System.Drawing.Size(1291, 0);
            // 
            // tspBottom
            // 
            this.tspBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tspBottom.Location = new System.Drawing.Point(0, 663);
            this.tspBottom.Name = "tspBottom";
            this.tspBottom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.tspBottom.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.tspBottom.Size = new System.Drawing.Size(1291, 0);
            // 
            // statusStrip
            // 
            this.statusStrip.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusStripLabelID,
            this.statusStripLabelStatus,
            this.statusStripLabelXtras,
            this.statusStripLabelVideo,
            this.statusStripLabelData,
            this.statusStripLabelMirror,
            this.statusStripLabelPlots,
            this.statusStripLabelSkeleton,
            this.statusStripLabelJoint,
            this.statusStripLabelAngle});
            this.statusStrip.Location = new System.Drawing.Point(0, 635);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip.ShowItemToolTips = true;
            this.statusStrip.Size = new System.Drawing.Size(1291, 28);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusStripLabelID
            // 
            this.statusStripLabelID.AutoSize = false;
            this.statusStripLabelID.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusStripLabelID.Name = "statusStripLabelID";
            this.statusStripLabelID.Size = new System.Drawing.Size(150, 23);
            this.statusStripLabelID.Text = "ID: 0x000000000000h";
            this.statusStripLabelID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.statusStripLabelID.ToolTipText = "Kinect ID connection";
            // 
            // statusStripLabelStatus
            // 
            this.statusStripLabelStatus.AutoSize = false;
            this.statusStripLabelStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.statusStripLabelStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusStripLabelStatus.Name = "statusStripLabelStatus";
            this.statusStripLabelStatus.Size = new System.Drawing.Size(100, 23);
            this.statusStripLabelStatus.Text = "Disconnected";
            this.statusStripLabelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.statusStripLabelStatus.ToolTipText = "Kinect status";
            // 
            // statusStripLabelXtras
            // 
            this.statusStripLabelXtras.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusStripLabelXtras.Name = "statusStripLabelXtras";
            this.statusStripLabelXtras.Size = new System.Drawing.Size(846, 23);
            this.statusStripLabelXtras.Spring = true;
            this.statusStripLabelXtras.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStripLabelVideo
            // 
            this.statusStripLabelVideo.AutoSize = false;
            this.statusStripLabelVideo.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusStripLabelVideo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusStripLabelVideo.Enabled = false;
            this.statusStripLabelVideo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.statusStripLabelVideo.Name = "statusStripLabelVideo";
            this.statusStripLabelVideo.Size = new System.Drawing.Size(22, 23);
            this.statusStripLabelVideo.Text = "V";
            this.statusStripLabelVideo.ToolTipText = "Video stream";
            // 
            // statusStripLabelData
            // 
            this.statusStripLabelData.AutoSize = false;
            this.statusStripLabelData.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusStripLabelData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusStripLabelData.Enabled = false;
            this.statusStripLabelData.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.statusStripLabelData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.statusStripLabelData.Name = "statusStripLabelData";
            this.statusStripLabelData.Size = new System.Drawing.Size(23, 23);
            this.statusStripLabelData.Text = "D";
            this.statusStripLabelData.ToolTipText = "Saving data";
            // 
            // statusStripLabelMirror
            // 
            this.statusStripLabelMirror.AutoSize = false;
            this.statusStripLabelMirror.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusStripLabelMirror.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusStripLabelMirror.Enabled = false;
            this.statusStripLabelMirror.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.statusStripLabelMirror.Name = "statusStripLabelMirror";
            this.statusStripLabelMirror.Size = new System.Drawing.Size(26, 23);
            this.statusStripLabelMirror.Text = "M";
            this.statusStripLabelMirror.ToolTipText = "Mirror Kinect video";
            // 
            // statusStripLabelPlots
            // 
            this.statusStripLabelPlots.AutoSize = false;
            this.statusStripLabelPlots.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusStripLabelPlots.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusStripLabelPlots.Enabled = false;
            this.statusStripLabelPlots.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.statusStripLabelPlots.Name = "statusStripLabelPlots";
            this.statusStripLabelPlots.Size = new System.Drawing.Size(26, 23);
            this.statusStripLabelPlots.Text = "P";
            this.statusStripLabelPlots.ToolTipText = "Plotting data";
            // 
            // statusStripLabelSkeleton
            // 
            this.statusStripLabelSkeleton.AutoSize = false;
            this.statusStripLabelSkeleton.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusStripLabelSkeleton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusStripLabelSkeleton.Enabled = false;
            this.statusStripLabelSkeleton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.statusStripLabelSkeleton.Name = "statusStripLabelSkeleton";
            this.statusStripLabelSkeleton.Size = new System.Drawing.Size(26, 23);
            this.statusStripLabelSkeleton.Text = "S";
            this.statusStripLabelSkeleton.ToolTipText = "Drawing skeleton";
            // 
            // statusStripLabelJoint
            // 
            this.statusStripLabelJoint.AutoSize = false;
            this.statusStripLabelJoint.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusStripLabelJoint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusStripLabelJoint.Enabled = false;
            this.statusStripLabelJoint.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.statusStripLabelJoint.Name = "statusStripLabelJoint";
            this.statusStripLabelJoint.Size = new System.Drawing.Size(26, 23);
            this.statusStripLabelJoint.Text = "J";
            this.statusStripLabelJoint.ToolTipText = "Drawing joints";
            // 
            // statusStripLabelAngle
            // 
            this.statusStripLabelAngle.AutoSize = false;
            this.statusStripLabelAngle.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.statusStripLabelAngle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusStripLabelAngle.Enabled = false;
            this.statusStripLabelAngle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.statusStripLabelAngle.Name = "statusStripLabelAngle";
            this.statusStripLabelAngle.Size = new System.Drawing.Size(26, 23);
            this.statusStripLabelAngle.Text = "A";
            this.statusStripLabelAngle.ToolTipText = "Drawing angles";
            // 
            // toolStripMain
            // 
            this.toolStripMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMain_Exit,
            this.toolStripSeparator2,
            this.toolStripMain_Connect,
            this.toolStripMain_Disconnect,
            this.toolStripSeparator3,
            this.toolStripMain_Video,
            this.toolStripMain_Data,
            this.toolStripMain_Picture,
            this.toolStripMain_Mirror,
            this.toolStripSeparator4,
            this.toolStripMain_Plots,
            this.toolStripSeparator5,
            this.toolStripMain_Skeleton,
            this.toolStripMain_SkeletonWidth,
            this.toolStripMain_Joint,
            this.toolStripMain_JointWidth,
            this.toolStripMain_Angle,
            this.toolStripMain_AngleWidth,
            this.toolStripSeparator6,
            this.toolStripMain_About});
            this.toolStripMain.Location = new System.Drawing.Point(0, 24);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStripMain.Size = new System.Drawing.Size(1291, 72);
            this.toolStripMain.TabIndex = 2;
            this.toolStripMain.Text = "Main toolbar";
            // 
            // toolStripMain_Exit
            // 
            this.toolStripMain_Exit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMain_Exit.Image")));
            this.toolStripMain_Exit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMain_Exit.Name = "toolStripMain_Exit";
            this.toolStripMain_Exit.Size = new System.Drawing.Size(52, 69);
            this.toolStripMain_Exit.Text = "Exit";
            this.toolStripMain_Exit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripMain_Exit.ToolTipText = "Exit application";
            this.toolStripMain_Exit.Click += new System.EventHandler(this.toolStripMain_Exit_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 72);
            // 
            // toolStripMain_Connect
            // 
            this.toolStripMain_Connect.CheckOnClick = true;
            this.toolStripMain_Connect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMain_Connect.Image")));
            this.toolStripMain_Connect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMain_Connect.Name = "toolStripMain_Connect";
            this.toolStripMain_Connect.Size = new System.Drawing.Size(59, 69);
            this.toolStripMain_Connect.Text = "Connect";
            this.toolStripMain_Connect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripMain_Connect.ToolTipText = "Connect Kinnect";
            this.toolStripMain_Connect.CheckedChanged += new System.EventHandler(this.toolStripMain_Connect_CheckedChanged);
            // 
            // toolStripMain_Disconnect
            // 
            this.toolStripMain_Disconnect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMain_Disconnect.Image")));
            this.toolStripMain_Disconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMain_Disconnect.Name = "toolStripMain_Disconnect";
            this.toolStripMain_Disconnect.Size = new System.Drawing.Size(75, 69);
            this.toolStripMain_Disconnect.Text = "Disconnect";
            this.toolStripMain_Disconnect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripMain_Disconnect.ToolTipText = "Disconnect Kinnect";
            this.toolStripMain_Disconnect.Click += new System.EventHandler(this.toolStripMain_Disconnect_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 72);
            // 
            // toolStripMain_Video
            // 
            this.toolStripMain_Video.CheckOnClick = true;
            this.toolStripMain_Video.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMain_Video.Image")));
            this.toolStripMain_Video.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMain_Video.Name = "toolStripMain_Video";
            this.toolStripMain_Video.Size = new System.Drawing.Size(52, 69);
            this.toolStripMain_Video.Text = "Video";
            this.toolStripMain_Video.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripMain_Video.ToolTipText = "Enable video stream";
            this.toolStripMain_Video.CheckStateChanged += new System.EventHandler(this.toolStripMain_Video_CheckStateChanged);
            // 
            // toolStripMain_Data
            // 
            this.toolStripMain_Data.CheckOnClick = true;
            this.toolStripMain_Data.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMain_Data.Image")));
            this.toolStripMain_Data.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMain_Data.Name = "toolStripMain_Data";
            this.toolStripMain_Data.Size = new System.Drawing.Size(52, 69);
            this.toolStripMain_Data.Text = "Data";
            this.toolStripMain_Data.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripMain_Data.ToolTipText = "Saving data";
            this.toolStripMain_Data.CheckStateChanged += new System.EventHandler(this.toolStripMain_Data_CheckStateChanged);
            this.toolStripMain_Data.Click += new System.EventHandler(this.toolStripMain_Data_Click);
            // 
            // toolStripMain_Picture
            // 
            this.toolStripMain_Picture.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMain_Picture.Image")));
            this.toolStripMain_Picture.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMain_Picture.Name = "toolStripMain_Picture";
            this.toolStripMain_Picture.Size = new System.Drawing.Size(52, 69);
            this.toolStripMain_Picture.Text = "Picture";
            this.toolStripMain_Picture.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripMain_Picture.ToolTipText = "Capture picture";
            this.toolStripMain_Picture.Click += new System.EventHandler(this.toolStripMain_Picture_Click);
            // 
            // toolStripMain_Mirror
            // 
            this.toolStripMain_Mirror.CheckOnClick = true;
            this.toolStripMain_Mirror.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMain_Mirror.Image")));
            this.toolStripMain_Mirror.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMain_Mirror.Name = "toolStripMain_Mirror";
            this.toolStripMain_Mirror.Size = new System.Drawing.Size(52, 69);
            this.toolStripMain_Mirror.Text = "Mirror";
            this.toolStripMain_Mirror.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripMain_Mirror.ToolTipText = "Mirror Kinect video";
            this.toolStripMain_Mirror.CheckStateChanged += new System.EventHandler(this.toolStripMain_Mirror_CheckStateChanged);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 72);
            // 
            // toolStripMain_Plots
            // 
            this.toolStripMain_Plots.CheckOnClick = true;
            this.toolStripMain_Plots.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMain_Plots.Image")));
            this.toolStripMain_Plots.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMain_Plots.Name = "toolStripMain_Plots";
            this.toolStripMain_Plots.Size = new System.Drawing.Size(52, 69);
            this.toolStripMain_Plots.Text = "Plots";
            this.toolStripMain_Plots.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripMain_Plots.ToolTipText = "Plotting data";
            this.toolStripMain_Plots.CheckStateChanged += new System.EventHandler(this.toolStripMain_Plots_CheckStateChanged);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 72);
            // 
            // toolStripMain_Skeleton
            // 
            this.toolStripMain_Skeleton.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripMain_Skeleton.CheckOnClick = true;
            this.toolStripMain_Skeleton.DoubleClickEnabled = true;
            this.toolStripMain_Skeleton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMain_Skeleton.Image")));
            this.toolStripMain_Skeleton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMain_Skeleton.Name = "toolStripMain_Skeleton";
            this.toolStripMain_Skeleton.Size = new System.Drawing.Size(61, 69);
            this.toolStripMain_Skeleton.Text = "Skeleton";
            this.toolStripMain_Skeleton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripMain_Skeleton.ToolTipText = "Drawing skeleton";
            this.toolStripMain_Skeleton.CheckStateChanged += new System.EventHandler(this.toolStripMain_Skeleton_CheckStateChanged);
            this.toolStripMain_Skeleton.DoubleClick += new System.EventHandler(this.toolStripMain_Skeleton_DoubleClick);
            // 
            // toolStripMain_SkeletonWidth
            // 
            this.toolStripMain_SkeletonWidth.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripMain_SkeletonWidth.Name = "toolStripMain_SkeletonWidth";
            this.toolStripMain_SkeletonWidth.Size = new System.Drawing.Size(45, 72);
            this.toolStripMain_SkeletonWidth.Text = "0";
            this.toolStripMain_SkeletonWidth.ValueChanged += new System.EventHandler(this.size_Changed);
            // 
            // toolStripMain_Joint
            // 
            this.toolStripMain_Joint.CheckOnClick = true;
            this.toolStripMain_Joint.DoubleClickEnabled = true;
            this.toolStripMain_Joint.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMain_Joint.Image")));
            this.toolStripMain_Joint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMain_Joint.Name = "toolStripMain_Joint";
            this.toolStripMain_Joint.Size = new System.Drawing.Size(52, 69);
            this.toolStripMain_Joint.Text = "Joint";
            this.toolStripMain_Joint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripMain_Joint.ToolTipText = "Drawing joints";
            this.toolStripMain_Joint.CheckStateChanged += new System.EventHandler(this.toolStripMain_Joint_CheckStateChanged);
            this.toolStripMain_Joint.DoubleClick += new System.EventHandler(this.toolStripMain_Joint_DoubleClick);
            // 
            // toolStripMain_JointWidth
            // 
            this.toolStripMain_JointWidth.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripMain_JointWidth.Name = "toolStripMain_JointWidth";
            this.toolStripMain_JointWidth.Size = new System.Drawing.Size(45, 72);
            this.toolStripMain_JointWidth.Text = "0";
            this.toolStripMain_JointWidth.ValueChanged += new System.EventHandler(this.size_Changed);
            // 
            // toolStripMain_Angle
            // 
            this.toolStripMain_Angle.CheckOnClick = true;
            this.toolStripMain_Angle.DoubleClickEnabled = true;
            this.toolStripMain_Angle.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMain_Angle.Image")));
            this.toolStripMain_Angle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMain_Angle.Name = "toolStripMain_Angle";
            this.toolStripMain_Angle.Size = new System.Drawing.Size(52, 69);
            this.toolStripMain_Angle.Text = "Angle";
            this.toolStripMain_Angle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripMain_Angle.ToolTipText = "Drawing angle";
            this.toolStripMain_Angle.CheckStateChanged += new System.EventHandler(this.toolStripMain_Angle_CheckStateChanged);
            this.toolStripMain_Angle.DoubleClick += new System.EventHandler(this.toolStripMain_Angle_DoubleClick);
            // 
            // toolStripMain_AngleWidth
            // 
            this.toolStripMain_AngleWidth.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripMain_AngleWidth.Name = "toolStripMain_AngleWidth";
            this.toolStripMain_AngleWidth.Size = new System.Drawing.Size(45, 72);
            this.toolStripMain_AngleWidth.Text = "0";
            this.toolStripMain_AngleWidth.ValueChanged += new System.EventHandler(this.size_Changed);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 72);
            // 
            // toolStripMain_About
            // 
            this.toolStripMain_About.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMain_About.Image")));
            this.toolStripMain_About.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMain_About.Name = "toolStripMain_About";
            this.toolStripMain_About.Size = new System.Drawing.Size(52, 69);
            this.toolStripMain_About.Text = "About";
            this.toolStripMain_About.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // mnuMainFrm
            // 
            this.mnuMainFrm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.mnuMainFrm.Location = new System.Drawing.Point(0, 0);
            this.mnuMainFrm.Name = "mnuMainFrm";
            this.mnuMainFrm.Size = new System.Drawing.Size(1291, 24);
            this.mnuMainFrm.TabIndex = 5;
            this.mnuMainFrm.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectKinectToolStripMenuItem,
            this.disconectKinectToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // connectKinectToolStripMenuItem
            // 
            this.connectKinectToolStripMenuItem.Name = "connectKinectToolStripMenuItem";
            this.connectKinectToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.connectKinectToolStripMenuItem.Text = "&Connect Kinect";
            // 
            // disconectKinectToolStripMenuItem
            // 
            this.disconectKinectToolStripMenuItem.Name = "disconectKinectToolStripMenuItem";
            this.disconectKinectToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.disconectKinectToolStripMenuItem.Text = "&Disconect Kinect";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(159, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // chartA
            // 
            this.chartA.Location = new System.Drawing.Point(873, 108);
            this.chartA.Name = "chartA";
            this.chartA.Padding = new System.Windows.Forms.Padding(10);
            this.chartA.Size = new System.Drawing.Size(406, 269);
            this.chartA.TabIndex = 8;
            this.chartA.Text = "Joint angles";
            // 
            // chartB
            // 
            this.chartB.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chartB.Location = new System.Drawing.Point(873, 383);
            this.chartB.Name = "chartB";
            this.chartB.Size = new System.Drawing.Size(406, 253);
            this.chartB.TabIndex = 26;
            this.chartB.Text = "Handtip coordinates";
            // 
            // bckWorkerDraw
            // 
            this.bckWorkerDraw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bckWorkerDraw_DoWork);
            // 
            // chartC
            // 
            this.chartC.BackColor = System.Drawing.Color.Transparent;
            this.chartC.BackColorTransparent = true;
            this.chartC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartC.Location = new System.Drawing.Point(873, 12);
            this.chartC.Name = "chartC";
            this.chartC.Size = new System.Drawing.Size(406, 204);
            this.chartC.TabIndex = 29;
            this.chartC.Text = "Obliquity angle";
            // 
            // txtTiltX
            // 
            this.txtTiltX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtTiltX.Location = new System.Drawing.Point(138, 603);
            this.txtTiltX.Name = "txtTiltX";
            this.txtTiltX.Size = new System.Drawing.Size(130, 22);
            this.txtTiltX.TabIndex = 32;
            // 
            // txtTiltZ
            // 
            this.txtTiltZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtTiltZ.Location = new System.Drawing.Point(433, 603);
            this.txtTiltZ.Name = "txtTiltZ";
            this.txtTiltZ.Size = new System.Drawing.Size(130, 22);
            this.txtTiltZ.TabIndex = 33;
            // 
            // lblTiltX
            // 
            this.lblTiltX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTiltX.AutoSize = true;
            this.lblTiltX.Location = new System.Drawing.Point(12, 606);
            this.lblTiltX.Name = "lblTiltX";
            this.lblTiltX.Size = new System.Drawing.Size(124, 16);
            this.lblTiltX.TabIndex = 34;
            this.lblTiltX.Text = "Tilt around X axis (°)";
            // 
            // lblTiltZ
            // 
            this.lblTiltZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTiltZ.AutoSize = true;
            this.lblTiltZ.Location = new System.Drawing.Point(307, 606);
            this.lblTiltZ.Name = "lblTiltZ";
            this.lblTiltZ.Size = new System.Drawing.Size(124, 16);
            this.lblTiltZ.TabIndex = 35;
            this.lblTiltZ.Text = "Tilt around Z axis (°)";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1291, 663);
            this.Controls.Add(this.lblTiltZ);
            this.Controls.Add(this.lblTiltX);
            this.Controls.Add(this.txtTiltZ);
            this.Controls.Add(this.txtTiltX);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.chartA);
            this.Controls.Add(this.chartC);
            this.Controls.Add(this.pctVideoColor);
            this.Controls.Add(this.chartB);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.mnuMainFrm);
            this.Controls.Add(this.tspBottom);
            this.Controls.Add(this.tspTop);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.mnuMainFrm;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alcance horizontal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pctVideoColor)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.mnuMainFrm.ResumeLayout(false);
            this.mnuMainFrm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pctVideoColor;
        private System.Windows.Forms.ToolStripPanel tspTop;
        private System.Windows.Forms.ToolStripPanel tspBottom;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton toolStripMain_Exit;
        private System.Windows.Forms.MenuStrip mnuMainFrm;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripMain_Connect;
        private System.Windows.Forms.ToolStripButton toolStripMain_Disconnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripMain_Data;
        private System.Windows.Forms.ToolStripButton toolStripMain_Picture;
        private System.Windows.Forms.ToolStripButton toolStripMain_Mirror;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectKinectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconectKinectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripMain_Skeleton;
        private System.Windows.Forms.ToolStripButton toolStripMain_Plots;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripNumericUpDown toolStripMain_SkeletonWidth;
        private LiveCharts.WinForms.CartesianChart chartA;
        private System.Windows.Forms.ToolStripButton toolStripMain_Joint;
        private System.Windows.Forms.ToolStripNumericUpDown toolStripMain_JointWidth;
        private System.Windows.Forms.ToolStripButton toolStripMain_Angle;
        private System.Windows.Forms.ToolStripNumericUpDown toolStripMain_AngleWidth;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton toolStripMain_About;
        private LiveCharts.WinForms.CartesianChart chartB;
        private System.Windows.Forms.ToolStripStatusLabel statusStripLabelID;
        private System.Windows.Forms.ToolStripStatusLabel statusStripLabelStatus;
        private System.Windows.Forms.ToolStripStatusLabel statusStripLabelData;
        private System.Windows.Forms.ToolStripStatusLabel statusStripLabelMirror;
        private System.ComponentModel.BackgroundWorker bckWorkerDraw;
        private System.Windows.Forms.ToolStripStatusLabel statusStripLabelPlots;
        private System.Windows.Forms.ToolStripStatusLabel statusStripLabelSkeleton;
        private System.Windows.Forms.ToolStripStatusLabel statusStripLabelJoint;
        private System.Windows.Forms.ToolStripStatusLabel statusStripLabelAngle;
        private LiveCharts.WinForms.AngularGauge chartC;
        private System.Windows.Forms.ToolStripStatusLabel statusStripLabelXtras;
        private System.Windows.Forms.ToolStripButton toolStripMain_Video;
        private System.Windows.Forms.ToolStripStatusLabel statusStripLabelVideo;
        private System.Windows.Forms.TextBox txtTiltX;
        private System.Windows.Forms.TextBox txtTiltZ;
        private System.Windows.Forms.Label lblTiltX;
        private System.Windows.Forms.Label lblTiltZ;
    }
}

