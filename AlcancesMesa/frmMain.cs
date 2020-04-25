using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D; // Vector3D

using LiveCharts; //Core of the library
using LiveCharts.Wpf; //The WPF controls
using LiveCharts.WinForms; //The WinForm wrappers
using LiveCharts.Defaults;

using Microsoft.Kinect;
using KinectStreams;

namespace AlcancesMesa
{
    public partial class frmMain : Form
    {
        // Program settings
        private ProgramSettings<string, string> _programSettings;
        private static readonly string _programSettingsFileName = @"Configuration.xml";

        // Tasks
        private Task _taskCompute;
        private Task _taskDraw;

        // Kinect variables
        private KinectSensor _kinectSensor = null;
        private MultiSourceFrameReader _reader = null;
        private bool _frameReceived = false;
        private IList<Body> _bodies;

        // Kinect frames
        byte[] colorBuffer;
        Image colorImage;

        // Skeleton drawing variables
        private Int32 _jointSize; private Color _jointColor; private bool _jointDraw;
        private Int32 _boneThickness; private Color _boneColor; private bool _boneDraw;
        private Int32 _angleSize; private Color _angleColor; Color _angleFill; private bool _angleDraw;

        // Kinect frame rate
        private DateTime now = DateTime.Now;
        private DateTime lastFrame = DateTime.Now;
        private TimeSpan result;
        private double fps = 0.0;
        private StringBuilder strInfo = new StringBuilder(85);

        // Saving data
        private string _path = "";
        private List<string> _listText;
        private bool _saveData = false; // Indicates that we are no saving data to a file
        System.IO.StreamWriter file = null;
        System.DateTime _startingTime;

        /*
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SendMessage(IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern bool SetWindowDisplayAffinity(IntPtr hwnd, uint affinity);
        */

        public frmMain()
        {
            InitializeComponent();

            // Custom initialization routines
            InitializeToolStripPanel();
            InitializeToolStrip();
            InitializeStatusStrip();
            InitializeMenuStrip();
            InitializeCharts();

            //Graphics.FromImage(this.pctVideoColor.Image).Clear(Color.Black); // Exception if Image is null
            //SetDoubleBuffering(pctVideoColor, true);
            Bitmap bmp = new Bitmap(pctVideoColor.Width, pctVideoColor.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);
            }
            pctVideoColor.Image = bmp;

            colorBuffer = new byte[1920 * 1080 * 2 * 2];
            colorImage = (Image)new Bitmap(1920, 1080, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            /*
            // Command line arguments
            string[] args = Environment.GetCommandLineArgs();
            MessageBox.Show(args.Length.ToString(), "Args length");
            MessageBox.Show(String.Join("\n", args), "Message");
            if (args.Length > 1)
            {
                Int32 WM_CLOSE = 0x0010;
                Int32 handle = Convert.ToInt32(args[1]);
                IntPtr managedHWND = new IntPtr(handle);

                MessageBox.Show(args[1] + "\n" + handle.ToString() + "\n" + managedHWND.ToString(), "Message");

                Message msgSuspendUpdate = Message.Create(managedHWND, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                NativeWindow window = NativeWindow.FromHandle(managedHWND);

                SendMessage(managedHWND, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

                MessageBox.Show(String.Join("\n", args), "Message");
                managedHWND = FindWindow(String.Empty, "Calculator ASM");
                SendMessage(managedHWND, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
            */

            using (var closeSplashEvent = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, "CloseSplashScreenEvent"))
            {
                closeSplashEvent.Set();
            }

            //SetWindowDisplayAffinity(this.Handle, 1); // 0 None, 1 Monitor

        }


        #region Initialization routines

        /// <summary>
        /// Initialize the ToolStripPanel component: add the child components to it
        /// </summary>
        private void InitializeToolStripPanel()
        {
            //tspTop = new ToolStripPanel();
            //tspBottom = new ToolStripPanel();
            tspTop.Join(toolStripMain);
            tspTop.Join(mnuMainFrm);
            tspBottom.Join(this.statusStrip);

            // Exit the method
            return;
        }

        /// <summary>
        /// Initialize the ToolStrip component
        /// </summary>
        private void InitializeToolStrip()
        {
            
            //ToolStripNumericUpDown c = new ToolStripNumericUpDown();
            //this.toolStripMain.Items.Add((ToolStripItem)c);

            toolStripMain.Renderer = new customRenderer(Brushes.SteelBlue, Brushes.LightSkyBlue);

            var path = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (File.Exists(path + @"\images\exit.ico")) this.toolStripMain_Exit.Image = new Icon(path + @"\images\exit.ico", 48, 48).ToBitmap();
            if (File.Exists(path + @"\images\connect.ico")) this.toolStripMain_Connect.Image = new Icon(path + @"\images\connect.ico", 48, 48).ToBitmap();
            if (File.Exists(path + @"\images\disconnect.ico")) this.toolStripMain_Disconnect.Image = new Icon(path + @"\images\disconnect.ico", 48, 48).ToBitmap();
            if (File.Exists(path + @"\images\cinema.ico")) this.toolStripMain_Video.Image = new Icon(path + @"\images\cinema.ico", 48, 48).ToBitmap();
            if (File.Exists(path + @"\images\save.ico")) this.toolStripMain_Data.Image = new Icon(path + @"\images\save.ico", 48, 48).ToBitmap();
            if (File.Exists(path + @"\images\picture.ico")) this.toolStripMain_Picture.Image = new Icon(path + @"\images\picture.ico", 48, 48).ToBitmap();
            if (File.Exists(path + @"\images\reflect-horizontal.ico")) this.toolStripMain_Mirror.Image = new Icon(path + @"\images\reflect-horizontal.ico", 48, 48).ToBitmap();
            if (File.Exists(path + @"\images\plot.ico")) this.toolStripMain_Plots.Image = new Icon(path + @"\images\plot.ico", 48, 48).ToBitmap();
            if (File.Exists(path + @"\images\about.ico")) this.toolStripMain_About.Image = new Icon(path + @"\images\about.ico", 48, 48).ToBitmap();

            /*
            using (Graphics g = Graphics.FromImage(this.toolStripMain_Skeleton.Image))
            {
                g.Clear(Color.PowderBlue);
            }
            */

            this.toolStripMain_Disconnect.Enabled = false;
            this.toolStripMain_SkeletonWidth.NumericUpDownControl.Maximum = 20;
            this.toolStripMain_SkeletonWidth.NumericUpDownControl.Minimum = 1;

            // Exit the method
            return;
        }

        /// <summary>
        /// Initialize the MenuStrip component
        /// </summary>
        private void InitializeMenuStrip()
        {
            return;
        }

        /// <summary>
        /// Initialize the StatusStrip component
        /// </summary>
        private void InitializeStatusStrip()
        {
            return;
        }

        /// <summary>
        /// Initialize the charts components
        /// </summary>
        private void InitializeCharts()
        {
            // Chart A initialization
            this.chartA.LegendLocation = LegendLocation.Top;
            this.chartA.ForeColor = System.Drawing.Color.Black;
            this.chartA.BackColor = System.Drawing.Color.White;
            this.chartA.Base.BorderBrush = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF000000");
            this.chartA.Base.BorderThickness = new System.Windows.Thickness(1.0);
            this.chartA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartA.Hoverable = false;
            this.chartA.DisableAnimations = true;
            this.chartA.DataTooltip = null;
            this.chartA.Padding = new Padding(0, 0, -20, 0);
            this.chartA.Base.Padding = new System.Windows.Thickness(0.0, 0.0, 20.0, 0.0);
            this.chartA.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Seconds",
                FontSize = 18,
                Foreground = System.Windows.Media.Brushes.Black,
                LabelFormatter = value => new System.DateTime((long)value).ToString("m:ss"),
                DisableAnimations = true,
                //Separator = new Separator { Step = TimeSpan.FromSeconds(1).Ticks }
                //Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" }
            });
            this.chartA.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Angle",
                FontSize = 18,
                Foreground = System.Windows.Media.Brushes.Black,
                DisableAnimations = true,
            });
            this.chartA.AxisX[0].Width = 12.0;
            //this.chartA.AxisY[0].MinValue = 0;
            this.chartA.AxisX[0].MaxValue = 10;
            this.chartA.AxisX[0].MinValue = 0;
            var mapper = LiveCharts.Configurations.Mappers.Xy<TimePoint>()
                        .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
                        .Y(model => model.Value);           //use the value property as Y
            /*var mapperTime = new LiveCharts.Configurations.CartesianMapper<double>()
                        .X((value, index) => index/30.0)
                        .Y((value, index) => value);*/
            this.chartA.Series = new SeriesCollection(mapper)
            {
                new LineSeries
                {
                    Title = "Shoulder",
                    Values = new ChartValues<TimePoint> { },
                    PointGeometry = null,
                    LineSmoothness = 0,
                    //Stroke = System.Windows.Media.Brushes.Aqua,
                    Fill = System.Windows.Media.Brushes.Transparent

                },
                new LineSeries
                {
                    Title = "Elbow",
                    Values = new ChartValues<TimePoint> { },
                    PointGeometry = null,
                    LineSmoothness = 0,
                    Fill = System.Windows.Media.Brushes.Transparent
                },
                new LineSeries
                {
                    Title = "Wrist",
                    Values = new ChartValues<TimePoint> { },
                    PointGeometry = null,
                    LineSmoothness = 0,
                    Fill = System.Windows.Media.Brushes.Transparent
                }
            };


            // Chart B initialization
            this.chartB.LegendLocation = LegendLocation.Top;
            this.chartB.BackColor = System.Drawing.Color.White;
            this.chartB.Base.BorderBrush = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF000000");
            this.chartB.Base.BorderThickness = new System.Windows.Thickness(1.0);
            this.chartB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartB.Hoverable = false;
            this.chartB.DisableAnimations = true;
            this.chartB.DataTooltip = null;
            this.chartB.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "X / cm",
                FontSize = 18,
                Foreground = System.Windows.Media.Brushes.Black,
                Width = 12,
                DisableAnimations = true
            });
            this.chartB.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Z / cm",
                FontSize = 18,
                Foreground = System.Windows.Media.Brushes.Black,
                DisableAnimations = true
            });
            //this.chartB.AxisY[0].MinValue = 0;
            this.chartB.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "HandTip",
                    Values = new ChartValues<ObservablePoint> (),
                    PointGeometry = null,
                    LineSmoothness = 0,
                    //Stroke = System.Windows.Media.Brushes.Aqua,
                    Fill = System.Windows.Media.Brushes.Transparent

                },
                new LineSeries
                {
                    Title = "Elbow",
                    Values = new ChartValues<ObservablePoint> (),
                    PointGeometry = null,
                    LineSmoothness = 0,
                    Fill = System.Windows.Media.Brushes.Transparent
                },
                new LineSeries
                {
                    Title = "S-Left",
                    Values = new ChartValues<ObservablePoint> (),
                    PointGeometry = null,
                    LineSmoothness = 0.2,
                    Fill = System.Windows.Media.Brushes.Transparent
                }
            };


            // 
            // Chart C initialization
            // 
            //this.chartC.BackColor = System.Drawing.Color.Transparent;
            //this.chartC.BackColorTransparent = true;
            //this.chartC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartC.DisableAnimations = true;
            this.chartC.FromValue = -40;
            this.chartC.ToValue = 40;
            this.chartC.Wedge = 140;
            this.chartC.LabelsStep = 20;
            this.chartC.TickStep = 5;
            this.chartC.TicksStrokeThickness = 2.5d;
            this.chartC.SectionsInnerRadius = 0.75;
            this.chartC.TicksForeground = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF3A3A3A");
            this.chartC.Base.Background = System.Windows.Media.Brushes.White;
            this.chartC.Base.Padding = new System.Windows.Thickness(0.0, 4.0, 0.0, 0.0);
            this.chartC.Base.FontSize = 16;
            this.chartC.Base.BorderBrush = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF000000");
            this.chartC.Base.BorderThickness = new System.Windows.Thickness(1.0);
            this.chartC.Base.LabelsEffect = new System.Windows.Media.Effects.DropShadowEffect()
            {
                Opacity = 0.0
            };
            this.chartC.Sections.Add(new AngularSection
            {
                Name="Negative",
                FromValue = -40,
                ToValue = -20,
                Fill = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFF9696")
            });
            this.chartC.Sections.Add(new AngularSection
            {
                Name="Neutral",
                FromValue = -20,
                ToValue = 20,
                Fill = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFADEABF")
            });
            this.chartC.Sections.Add(new AngularSection
            {
                Name = "Positive",
                FromValue = 20,
                ToValue = 40,
                Fill = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFF9696")
            });

            // Exit the method
            return;
        }

        #endregion Initialization routines

        #region Form
        private void frmMain_Load(object sender, EventArgs e)
        {
            // Load and apply program settings.
            _programSettings = new ProgramSettings<string, string>();
            //LoadDefaultSettings();
            LoadProgramSettings();
            ApplySettings();

            // Creates the fade in animation of the form
            Win32.Win32API.AnimateWindow(this.Handle, 200, Win32.Win32API.AnimateWindowFlags.AW_BLEND | Win32.Win32API.AnimateWindowFlags.AW_CENTER);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (new CenterWinDialog(this))
            {
                if (DialogResult.No == MessageBox.Show(this,
                                                        "Are you sure you want to exit\nthe application?",
                                                        "Exit?",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question,
                                                        MessageBoxDefaultButton.Button2))
                {
                    // Cancelar el cierre de la ventana
                    e.Cancel = true;
                }
                else
                {
                    // Cancel any data recording and close streams
                    toolStripMain_Data.Checked = false;

                    // Close the file in case it is accidentally left open
                    if (file != null)
                        file.Dispose();

                    // Guardar los datos de configuración
                    SaveProgramSettings();

                    // Fade out animation
                    Win32.Win32API.AnimateWindow(this.Handle, 200, Win32.Win32API.AnimateWindowFlags.AW_BLEND | Win32.Win32API.AnimateWindowFlags.AW_HIDE);
                }
            }

        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            pctVideoColor.Height = this.Height - pctVideoColor.Location.Y - 100;
            pctVideoColor.Width = pctVideoColor.Height * 1920 / 1080;

            if (pctVideoColor.Height >= 300)
            {
                chartA.Location = new Point(13 + pctVideoColor.Width + 18, chartA.Location.Y);
                chartA.Width = (this.Width - 29) - chartA.Location.X;
                chartB.Location = new Point(13 + pctVideoColor.Width + 18, chartB.Location.Y);
                chartB.Width = (this.Width - 29) - chartB.Location.X;
                chartC.Location = new Point(13 + pctVideoColor.Width + 18, chartC.Location.Y);
                chartC.Width = (this.Width - 29) - chartC.Location.X;
            }
        }

        #endregion Form

        #region toolStripMain

        private void toolStripMain_Connect_CheckedChanged(object sender, EventArgs e)
        {
            //_startingTime = System.DateTime.Now;

            if (toolStripMain_Connect.Checked == true)
            {
                toolStripMain_Disconnect.Enabled = true;
                ConnectKinect();
            }
            else
            {
                toolStripMain_Disconnect.Enabled = false;
                toolStripMain_Data.Checked = false;
                DisconnectKinect();
            }
                
        }

        private void toolStripMain_Disconnect_Click(object sender, EventArgs e)
        {
            toolStripMain_Disconnect.Enabled = false;
            toolStripMain_Connect.Checked = false;
            toolStripMain_Data.Checked = false;
        }

        private void toolStripMain_Exit_Click(object sender, EventArgs e)
        {
            // Cerrar llamando al evento frmMain_FormClosing
            this.Close();
        }

        private void toolStripMain_Skeleton_DoubleClick(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.FullOpen = true;
            colorDlg.AnyColor = true;
            //colorDlg.Color = ((Bitmap)this.toolStripMain_Skeleton.Image).GetPixel(1, 1);
            colorDlg.Color = Color.FromArgb(Convert.ToInt32(_programSettings["SkeletonColor"]));

            using (new CenterWinDialog(this))
            {
                if (colorDlg.ShowDialog() == DialogResult.OK)
                {
                    Graphics.FromImage(this.toolStripMain_Skeleton.Image).Clear(colorDlg.Color);
                    if (!this.toolStripMain_Skeleton.Checked) this.toolStripMain_Skeleton.Checked = true;
                    this.toolStripMain_Skeleton.Invalidate();
                    _boneColor = colorDlg.Color;
                    _programSettings["SkeletonColor"] = _boneColor.ToArgb().ToString();
                }
            }
        }

        private void toolStripMain_Joint_DoubleClick(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.FullOpen = true;
            colorDlg.AnyColor = true;
            //colorDlg.Color = ((Bitmap)this.toolStripMain_Skeleton.Image).GetPixel(1, 1);
            colorDlg.Color = Color.FromArgb(Convert.ToInt32(_programSettings["JointColor"]));

            using (new CenterWinDialog(this))
            {
                if (colorDlg.ShowDialog() == DialogResult.OK)
                {
                    Graphics.FromImage(this.toolStripMain_Skeleton.Image).Clear(colorDlg.Color);
                    if (!this.toolStripMain_Skeleton.Checked) this.toolStripMain_Skeleton.Checked = true;
                    this.toolStripMain_Joint.Invalidate();
                    _jointColor = colorDlg.Color;
                    _programSettings["JointColor"] = _jointColor.ToArgb().ToString();
                }
            }
        }

        private void toolStripMain_Angle_DoubleClick(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.FullOpen = true;
            colorDlg.AnyColor = true;
            //colorDlg.Color = ((Bitmap)this.toolStripMain_Skeleton.Image).GetPixel(1, 1);
            colorDlg.Color = Color.FromArgb(Convert.ToInt32(_programSettings["AngleColor"]));

            using (new CenterWinDialog(this))
            {
                if (colorDlg.ShowDialog() == DialogResult.OK)
                {
                    Graphics.FromImage(this.toolStripMain_Skeleton.Image).Clear(colorDlg.Color);
                    if (!this.toolStripMain_Skeleton.Checked) this.toolStripMain_Skeleton.Checked = true;
                    this.toolStripMain_Angle.Invalidate();

                    _angleColor = colorDlg.Color;
                    _angleFill = Color.FromArgb(colorDlg.Color.A >> 1, colorDlg.Color); // Half the transparency
                    _programSettings["AngleColor"] = _angleColor.ToArgb().ToString();
                    _programSettings["AngleFill"] = _angleFill.ToArgb().ToString();
                }
            }
        }

        private void toolStripMain_Data_Click(object sender, EventArgs e)
        {
        }

        private void toolStripMain_Data_CheckStateChanged(object sender, EventArgs e)
        {
            // http://stackoverflow.com/questions/9907682/create-a-txt-file-if-doesnt-exist-and-if-it-does-append-a-new-line

            // When the user or the program unchecks the button
            if (this.toolStripMain_Data.CheckState == CheckState.Unchecked)
            {
                // If we have been saving data, then write the ending time
                if (_saveData==true)
                {
                    if (file != null)
                    {
                        //file.Dispose();
                        file.Close();
                        file = null;
                    }

                    DateTime EndTime = DateTime.Now;

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(_path, true))
                    {
                        foreach(string str in _listText)
                        {
                            file.WriteLine(str);
                        }
                        file.WriteLine("Ending time:\t{0}:{1}:{2}:{3}", EndTime.Hour, EndTime.Minute, EndTime.Second, EndTime.Millisecond);
                    }

                    /*
                    if (file != null)
                    {
                        file.WriteLine("Ending time:\t{0}:{1}:{2}:{3}", EndTime.Hour, EndTime.Minute, EndTime.Second, EndTime.Millisecond);
                        //file.Dispose();
                        file.Close();
                        file = null;

                    }*/

                    _saveData = false;  // We are no longer saving data
                    _listText.Clear();  // Empty the text string to be stored
                    this.statusStripLabelXtras.Text = "";   // Clear the status bar text
                }
                statusStripLabelData.Enabled = false;
                return;
            }
            // Displays a SaveFileDialog so the user can save the data
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveDialog.Title = "Save data to disk file";
            saveDialog.CreatePrompt = true;
            saveDialog.OverwritePrompt = true;
            
            // Show the FileDialog. Exit in case the user cancels
            using (new CenterWinDialog(this))
            {
                if (saveDialog.ShowDialog() == DialogResult.Cancel)
                {
                    this.toolStripMain_Data.Checked = false;
                    return;
                }
                else
                {
                    _path = saveDialog.FileName;
                    this.statusStripLabelXtras.Text = "Saving data to " + _path;
                }
            }

            // Create the file if it doesn't exist
            if (!File.Exists(_path))
                File.Create(_path).Dispose();

            DateTime tiempo = DateTime.Now;

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(_path, false))
            {
                file.WriteLine("Stream saved from Kinect");
                file.WriteLine("Date:\t{0}-{1}-{2}", tiempo.Day, tiempo.Month, tiempo.Year);
                file.WriteLine("Starting time:\t{0}:{1}:{2}:{3}", tiempo.Hour, tiempo.Minute, tiempo.Second, tiempo.Millisecond);
                file.WriteLine("Seconds\tShoulder angle\tElbow angle\tWrist angle\tFloor height\tLeftShoulder X\tLeftShoulder Y\tLeftShoulder Z\tShoulder X\tShoulder Y\tShoulder Z\tElbow X\tElbow Y\tElbow Z\tWrist X\tWrist Y\tWrist Z\tHandTip X\tHandTip Y\tHandTip Z\txShouL\tzShouL\txElbow\tzElbow\txHand\tzHand\tObliquity\t\t");
            }

            // http://www.jeremyshanks.com/fastest-way-to-write-text-files-to-disk-in-c/
            if (file == null)
                file = new System.IO.StreamWriter(_path, true, Encoding.UTF8, 32768);   // 2^15

            _saveData = true;   // We are saving data
            _listText = new List<string>();
            statusStripLabelData.Enabled = true;
        }

        private void toolStripMain_Video_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.toolStripMain_Video.CheckState == CheckState.Checked)
            {
                this._programSettings["DisplayFrame"] = "1";
                statusStripLabelVideo.Enabled = true;
            }
            else
            {
                this._programSettings["DisplayFrame"] = "0";
                statusStripLabelVideo.Enabled = false;
            }
        }

        private void toolStripMain_Mirror_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.toolStripMain_Mirror.CheckState == CheckState.Checked)
            {
                this._programSettings["MirrorChecked"] = "1";
                statusStripLabelMirror.Enabled = true;
            }
            else
            {
                this._programSettings["MirrorChecked"] = "0";
                statusStripLabelMirror.Enabled = false;
            }
        }

        private void toolStripMain_Plots_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.toolStripMain_Plots.CheckState == CheckState.Checked)
            {
                this._programSettings["PlotsChecked"] = "1";
                statusStripLabelPlots.Enabled = true;
            }
            else
            {
                this._programSettings["PlotsChecked"] = "0";
                statusStripLabelPlots.Enabled = false;
            }
        }

        private void toolStripMain_Skeleton_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.toolStripMain_Plots.CheckState == CheckState.Checked)
            {
                this._programSettings["SkeletonChecked"] = "1";
                _boneDraw = true;
                statusStripLabelSkeleton.Enabled = true;
            }
            else
            {
                this._programSettings["SkeletonChecked"] = "0";
                _boneDraw = false;
                statusStripLabelSkeleton.Enabled = false;
            }
        }

        private void toolStripMain_Joint_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.toolStripMain_Plots.CheckState == CheckState.Checked)
            {
                this._programSettings["JointChecked"] = "1";
                _jointDraw = true;
                statusStripLabelJoint.Enabled = true;
            }
            else
            {
                this._programSettings["JointChecked"] = "0";
                _jointDraw = false;
                statusStripLabelJoint.Enabled = false;
            }
        }

        private void toolStripMain_Angle_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.toolStripMain_Plots.CheckState == CheckState.Checked)
            {
                this._programSettings["AngleChecked"] = "1";
                _angleDraw = true;
                statusStripLabelAngle.Enabled = true;
            }
            else
            {
                this._programSettings["AngleChecked"] = "0";
                _angleDraw = false;
                statusStripLabelAngle.Enabled = false;
            }
        }

        private void toolStripMain_Picture_Click(object sender, EventArgs e)
        {
            String path = Environment.SpecialFolder.DesktopDirectory.ToString();
            var img = pctVideoColor.Image;

            // Displays a SaveFileDialog so the user can save the data
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Bitmap (*.bmp)|*.bmp|Jpg (*.jpg)|*.jpg|Jpeg (*.jpeg)|*.jpeg|Portable network graphics (*.png)|*.png|Tiff (*.tiff)|*.tiff|Wmf (*.wmf)|*.wmf|All files (*.*)|*.*";
            saveDialog.Title = "Save picture to disk file";
            saveDialog.CreatePrompt = true;
            saveDialog.OverwritePrompt = true;

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                path = saveDialog.FileName;
                if (Extensions.Save(img, path) == false)
                    MessageBox.Show(this, "The image could not be saved.", "Error");
            }

            /*
            if (!File.Exists(path))
                File.Create(path).Dispose();
            */
        }

        #endregion toolStripMain

        #region mnuMainForm

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Invoke the frmMain_FormClosing event
            this.Close();
        }

        #endregion mnuMainForm

        #region Application settings

        /// <summary>
        /// Loads any saved program settings.
        /// </summary>
        private void LoadProgramSettings()
        {
            // Load the saved window settings and resize the window.
            TextReader textReader = StreamReader.Null; 
            try
            {
                textReader = new StreamReader(_programSettingsFileName);
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ProgramSettings<string, string>));
                _programSettings = (ProgramSettings<string, string>)serializer.Deserialize(textReader);
                textReader.Close();
            }
            catch (Exception ex)
            {

                if(!(ex is FileNotFoundException))
                {
                    using (new CenterWinDialog(this))
                    {
                        MessageBox.Show(this,
                                        "Unexpected error while\nloading settings data",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                }
                LoadDefaultSettings();
            }
            finally
            {
                if (textReader != null) textReader.Close();
            }
        }

        /// <summary>
        /// Saves the current program settings.
        /// </summary>
        private void SaveProgramSettings()
        {
            _programSettings["WindowLeft"] = this.DesktopLocation.X.ToString();
            _programSettings["WindowTop"] = this.DesktopLocation.Y.ToString();
            _programSettings["WindowWidth"] = this.ClientSize.Width.ToString();
            _programSettings["WindowHeight"] = this.ClientSize.Height.ToString();
            _programSettings["DisplayFrame"] = (this.toolStripMain_Video.Checked ? 1 : 0).ToString();
            _programSettings["MirrorChecked"] = (this.toolStripMain_Mirror.Checked ? 1 : 0).ToString();
            _programSettings["PlotsChecked"] = (this.toolStripMain_Plots.Checked ? 1 : 0).ToString();
            _programSettings["SkeletonChecked"] = (this.toolStripMain_Skeleton.Checked ? 1 : 0).ToString();
            //_programSettings["SkeletonColor"]
            _programSettings["SkeletonWidth"] = ((Int32)this.toolStripMain_SkeletonWidth.NumericUpDownControl.Value).ToString();
            _programSettings["JointChecked"] = (this.toolStripMain_Joint.Checked ? 1 : 0).ToString(); ;
            _programSettings["JointColor"] = this._jointColor.ToArgb().ToString();
            _programSettings["JointSize"] = this._jointSize.ToString();
            _programSettings["AngleChecked"] = (this.toolStripMain_Angle.Checked ? 1 : 0).ToString();
            _programSettings["AngleColor"] = this._angleColor.ToArgb().ToString();
            _programSettings["AngleFill"] = this._angleFill.ToArgb().ToString();
            _programSettings["AngleSize"] = this._angleSize.ToString();
            // _programSettings[""] =
            

            // Save window settings.
            TextWriter textWriter = StreamWriter.Null;
            try
            {
                textWriter = new StreamWriter(_programSettingsFileName, false);
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ProgramSettings<string, string>));
                serializer.Serialize(textWriter, _programSettings);
                textWriter.Close();
            }
            catch (Exception ex)
            {
                using (new CenterWinDialog(this))
                {
                    MessageBox.Show(this,
                                    "Unexpected error while\nsaving settings data",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            finally
            {
                if (textWriter != null) textWriter.Close();
            }

        }

        /// <summary>
        /// Update UI with settings 
        /// </summary>
        private void ApplySettings()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.DesktopLocation = new Point(Convert.ToInt32(_programSettings["WindowLeft"]), Convert.ToInt32(_programSettings["WindowTop"]));
            this.ClientSize = new Size(Convert.ToInt32(_programSettings["WindowWidth"]), Convert.ToInt32(_programSettings["WindowHeight"]));
            this.toolStripMain_Video.Checked = Convert.ToInt32(_programSettings["DisplayFrame"]) == 1 ? true : false;
            statusStripLabelVideo.Enabled = toolStripMain_Video.Checked;
            this.toolStripMain_Mirror.Checked = Convert.ToInt32(_programSettings["MirrorChecked"]) == 1 ? true : false;
            statusStripLabelMirror.Enabled = toolStripMain_Mirror.Checked;
            this.toolStripMain_Plots.Checked = Convert.ToInt32(_programSettings["PlotsChecked"]) == 1 ? true : false;
            statusStripLabelPlots.Enabled = toolStripMain_Plots.Checked;


            this._boneColor = Color.FromArgb(Convert.ToInt32(_programSettings["SkeletonColor"]));
            this._boneThickness = Convert.ToInt32(_programSettings["SkeletonWidth"]);
            this._boneDraw = Convert.ToInt32(_programSettings["SkeletonChecked"]) == 1 ? true : false;
            this.toolStripMain_Skeleton.Checked = _boneDraw;
            statusStripLabelSkeleton.Enabled = _boneDraw;
            Graphics.FromImage(this.toolStripMain_Skeleton.Image).Clear(_boneColor);
            this.toolStripMain_SkeletonWidth.NumericUpDownControl.Value = Convert.ToInt32(_boneThickness);
            this.toolStripMain_Skeleton.Invalidate();

            this._jointColor = Color.FromArgb(Convert.ToInt32(_programSettings["JointColor"]));
            this._jointSize = Convert.ToInt32(_programSettings["JointSize"]);
            this._jointDraw = Convert.ToInt32(_programSettings["JointChecked"]) == 1 ? true : false;
            this.toolStripMain_Joint.Checked = _jointDraw;
            statusStripLabelJoint.Enabled = _jointDraw;
            Graphics.FromImage(this.toolStripMain_Joint.Image).Clear(_jointColor);
            this.toolStripMain_JointWidth.NumericUpDownControl.Value = Convert.ToInt32(_jointSize);
            this.toolStripMain_Joint.Invalidate();

            this._angleFill = Color.FromArgb(Convert.ToInt32(_programSettings["AngleFill"]));
            this._angleColor = Color.FromArgb(Convert.ToInt32(_programSettings["AngleColor"]));
            this._angleSize = Convert.ToInt32(_programSettings["AngleSize"]);
            this._angleDraw= Convert.ToInt32(_programSettings["AngleChecked"]) == 1 ? true : false;
            this.toolStripMain_Angle.Checked = _angleDraw;
            statusStripLabelAngle.Enabled = _angleDraw;
            Graphics.FromImage(this.toolStripMain_Angle.Image).Clear(_angleColor);
            this.toolStripMain_AngleWidth.NumericUpDownControl.Value = Convert.ToInt32(_angleSize);
            this.toolStripMain_Angle.Invalidate();
        }

        /// <summary>
        /// Set default settings. This is called when no settings file has been found
        /// </summary>
        private void LoadDefaultSettings()
        {
            // Set default settings
            _programSettings["WindowLeft"] = this.DesktopLocation.X.ToString();    // Get current form coordinates
            _programSettings["WindowTop"] = this.DesktopLocation.Y.ToString();
            _programSettings["WindowWidth"] = this.ClientSize.Width.ToString();    // Get current form size
            _programSettings["WindowHeight"] = this.ClientSize.Height.ToString();
            _programSettings["DisplayFrame"] = "1";     // Checked
            _programSettings["MirrorChecked"] = "0";    // Unchecked
            _programSettings["PlotsChecked"] = "1";     // Checked
            _programSettings["SkeletonChecked"] = "1";  // Checked
            _programSettings["SkeletonColor"] = Color.Red.ToArgb().ToString();
            _programSettings["SkeletonWidth"] = "3";
            _programSettings["JointChecked"] = "1";     // Checked
            _programSettings["JointColor"]  = Color.Purple.ToArgb().ToString();
            _programSettings["JointSize"] = "5";
            _programSettings["AngleChecked"] = "1";     // Checked
            _programSettings["AngleColor"] = Color.Green.ToArgb().ToString();
            _programSettings["AngleFill"] = Color.FromArgb(127, Color.Green).ToArgb().ToString();
            _programSettings["AngleSize"] = "3";
        }

        #endregion Application settings

        #region Kinect

        public void InitializeKinect()
        {

            _kinectSensor = KinectSensor.GetDefault();

            if (_kinectSensor != null)
            {
                // Turn on kinect
                _kinectSensor.Open();

                //_reader = _kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Body);
                //_reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
                //_kinectSensor.IsAvailableChanged += KinectSensor_IsAvailableChanged;

            }

        }

        private void ConnectKinect()
        {

            _kinectSensor = KinectSensor.GetDefault();

            if (_kinectSensor != null)
            {
                _kinectSensor.IsAvailableChanged += KinectSensor_IsAvailableChanged;
                _kinectSensor.Open();
             
                _reader = _kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;

                foreach (Series s in chartA.Series)
                {
                    s.Values.Clear();
                }
                foreach (Series s in chartB.Series)
                {
                    s.Values.Clear();
                }
                _startingTime = System.DateTime.Now;
                this.DoubleBuffered = true;
            }

            /*
            if (_kinectSensor.IsAvailable)
            {
                
            }
            else
            {
                using (new CenterWinDialog(this))
                {
                    MessageBox.Show(this, "No Kinect sensor was found.\nPlease check Kinect is connected to the PC.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            */

        }

        private void DisconnectKinect()
        {
            if (_kinectSensor != null && _kinectSensor.IsOpen)
            {
                this.DoubleBuffered = false;
                _kinectSensor.Close();
                _reader.MultiSourceFrameArrived -= Reader_MultiSourceFrameArrived;
                _kinectSensor.IsAvailableChanged -= KinectSensor_IsAvailableChanged;

                if (_reader != null) _reader.Dispose();
                statusStripLabelID.Text = "ID: 0x000000000000h";
                statusStripLabelStatus.Text = "Disconnected";

                chartA.AxisX[0].MinValue = 0;
            }
        }

        /// <summary>
        /// Event fired when a Kinect Sensor is connected to the PC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KinectSensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {

            if (e.IsAvailable == true)
            {
                statusStripLabelStatus.Text = "Connected";
                statusStripLabelID.Text = "ID: 0x" + _kinectSensor.UniqueKinectId + "h";
                statusStrip.Refresh();
            }
            else
            {
                statusStripLabelStatus.Text = "Disconnected";
                statusStripLabelID.Text = "ID: 0x000000000000h";
                statusStrip.Refresh();
            }
            //throw new NotImplementedException();
        }

        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            _frameReceived = false;
            bool _displayImage = _programSettings["DisplayFrame"] == "1" ? true : false;
            bool _displayBody= _programSettings["SkeletonChecked"] == "1" ? true : false;
            bool _mirror = _programSettings["MirrorChecked"] == "1" ? true : false;
            bool _calculateReach = true;

            //Image img = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            // Get a reference to the multi-frame
            var reference = e.FrameReference.AcquireFrame();

            // If the frame has expired by the time we process this event, return.
            if (reference == null) return;

            // Get the colorstream and paint it to a picture box
            using (ColorFrame colorFrame = reference.ColorFrameReference.AcquireFrame())
            {
                //txtTiltX.Text = (1000.0 / colorFrame.ColorCameraSettings.FrameInterval.TotalMilliseconds).ToString("#.##");
                if (colorFrame != null)
                {
                    if (_displayImage)
                    {
                        //colorImage = KinectStreams.Extensions.CreateImageFromFrame(colorFrame);
                        //KinectStreams.Extensions.CreateImageFromFrame(colorFrame, (Bitmap)colorImage);
                        //colorImage = KinectStreams.Extensions.CreateImageFromFrame(colorFrame, (Bitmap)colorImage, colorBuffer);
                        KinectStreams.Extensions.CreateImageFromFrame(colorFrame, (Bitmap)colorImage, colorBuffer);
                        //pctVideoColor.Image = colorImage;
                    }
                    else
                    {
                        colorImage = new Bitmap(colorFrame.FrameDescription.Width, colorFrame.FrameDescription.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        using (Graphics g = Graphics.FromImage(colorImage))
                        {
                            g.Clear(Color.White);
                        }
                        //pctVideoColor.Image = img;
                    }
                }
            }

            // Get the body stream
            using (BodyFrame bodyFrame = reference.BodyFrameReference.AcquireFrame())
            {
                //g = Graphics.FromImage(img);
                _bodies= new Body[this._kinectSensor.BodyFrameSource.BodyCount];
                
                if (bodyFrame != null)
                {
                    //_bodies = new Body[bodyFrame.BodyFrameSource.BodyCount];
                    bodyFrame.GetAndRefreshBodyData(_bodies);
                    _frameReceived = true;
                    // _frameCounter ++;
                    // _floorLevel = bodyFrame.FloorClipPlane.W;

                    Floor floor = new Floor(bodyFrame.FloorClipPlane);
                    strInfo.Append("Tilt around X axis (°): ");
                    strInfo.Append(floor.SensorTilt.ToString("N2"));
                    strInfo.Append(" — ");
                    strInfo.Append("Tilt around Z axis (°): ");
                    strInfo.Append(floor.SensorLateralTilt.ToString("N2"));
                    //txtTiltX.Text = (new Floor(bodyFrame.FloorClipPlane)).SensorTilt.ToString();
                    //txtTiltZ.Text = (new Floor(bodyFrame.FloorClipPlane)).SensorLateralTilt.ToString();
                }

                //_frameReceived = true;
                // http://pterneas.com/2014/03/13/kinect-for-windows-version-2-body-tracking/
                // Body TrackedSkeleton = bodies.FirstOrDefault(s => s.IsTracked == true);
                if (_frameReceived == true)
                {
                    //Body body = _bodies.Closest();
                    //Body body = bodyFrame.Body();
                    Body body = _bodies.First();
                    
                    //foreach (Body body in _bodies)
                    //{
                    if (body != null)
                    {
                        if (body.IsTracked)
                        {
                            //lblRightHandTip.Text = body.Joints[JointType.HandRight].Position.X.ToString();
                            if (_displayBody)
                            {
                                //Task.Run(() => DrawSkeleton(Graphics.FromImage(img), body));
                                //DrawSkeleton(Graphics.FromHwnd(pctVideoColor.Handle), body);
                                DrawExtras(Graphics.FromImage(colorImage), body);
                                DrawSkeleton(Graphics.FromImage(colorImage), body);
                                //DrawSkeletonAsync(Graphics.FromImage(colorImage), body);
                                //bckWorkerDraw.RunWorkerAsync(new Tuple<Graphics, Body>(Graphics.FromImage(img), body));
                                //Task.Run(() => DrawExtras(Graphics.FromImage(img), body));
                                //Task.Run(() => DrawSkeleton(Graphics.FromImage(img), body));
                            }

                            // Calculate the body reach
                            if (_calculateReach)
                            {
                                //Task.Run(() => BodyReachSagitalPlane(body, img, bodyFrame.FloorClipPlane.W));
                                //Task.Run(() => BodyReachSagitalPlane(body, img, 50.0f));
                                //BodyReachSagitalPlane(body, bodyFrame.FloorClipPlane.W);
                                BodyReachSagitalPlane(body, 0.0f);
                            }

                        }
                    }
                    //}
                }

            }

            // Flip and draw the frame to the PictureBox control and update the carts on a different thread
            if (_mirror)
                colorImage.RotateFlip(RotateFlipType.Rotate180FlipY);

            pctVideoColor.Invoke((MethodInvoker)(() => pctVideoColor.Image = colorImage));
            //pctVideoColor.Image = colorImage;
            //this.Invalidate();

            /*
            Task.Run(() =>
            {
                pctVideoColor.Invoke((Action)(() => pctVideoColor.Image = img));
                //pctVideoColor.Invoke((Action)(() => pctVideoColor.Invalidate()));
                //chartA.Invoke((Action)(() => chartA.Invalidate()));
                //chartB.Invoke((Action)(() => chartB.Invalidate()));
                //chartC.Invoke((Action)(() => chartC.Invalidate()));
                this.Invalidate();
            });
            */

            //pctVideoColor.Invoke((Action)(() => pctVideoColor.Invalidate()));

            // Force redrawing
            //chartA.Invoke((Action)(() => chartA.Invalidate()));
            //chartB.Invoke((Action)(() => chartB.Invalidate()));
            //chartC.Invoke((Action)(() => chartC.Invalidate()));
            //bckWorkerDraw.RunWorkerAsync(null);

            // Calculate the frame rate
            now = DateTime.Now;
            result = now.Subtract(lastFrame);
            lastFrame = now;
            fps = 1000.0 / result.TotalMilliseconds;
            strInfo.Append(" — ");
            strInfo.Append("FPS: ");
            strInfo.Append(fps.ToString("#.##"));
            this.statusStripLabelXtras.Text = strInfo.ToString();
            strInfo.Clear();
            //statusStrip.Update();

        }

        #endregion Kinect

        #region Kinect drawing skeleton

        /// <summary>
        /// Draw the extras and the body skeleton with all the joints
        /// </summary>
        /// <param name="g"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private async Task DrawSkeletonAsync(Graphics g, Body body)
        {
            if (body == null) return;
            await Task.Run(() =>
            {
                DrawExtras(g, body);
                DrawSkeleton(g, body);
            });
        }

        /// <summary>
        /// Draw the body skeleton with all the joints.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="body"></param>
        private void DrawSkeleton(Graphics g, Body body)
        {
            if (body == null) return;

            // Draw all the lines connecting the joints
            DrawLine(g, body.Joints[JointType.Head], body.Joints[JointType.Neck]);
            DrawLine(g, body.Joints[JointType.Neck], body.Joints[JointType.SpineShoulder]);
            DrawLine(g, body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderLeft]);
            DrawLine(g, body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderRight]);
            DrawLine(g, body.Joints[JointType.SpineShoulder], body.Joints[JointType.SpineMid]);
            DrawLine(g, body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft]);
            DrawLine(g, body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight]);
            DrawLine(g, body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft]);
            DrawLine(g, body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight]);
            DrawLine(g, body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft]);
            DrawLine(g, body.Joints[JointType.WristRight], body.Joints[JointType.HandRight]);
            DrawLine(g, body.Joints[JointType.HandLeft], body.Joints[JointType.HandTipLeft]);
            DrawLine(g, body.Joints[JointType.HandRight], body.Joints[JointType.HandTipRight]);
            DrawLine(g, body.Joints[JointType.WristLeft], body.Joints[JointType.ThumbLeft]);
            DrawLine(g, body.Joints[JointType.WristRight], body.Joints[JointType.ThumbRight]);
            DrawLine(g, body.Joints[JointType.SpineMid], body.Joints[JointType.SpineBase]);
            DrawLine(g, body.Joints[JointType.SpineBase], body.Joints[JointType.HipLeft]);
            DrawLine(g, body.Joints[JointType.SpineBase], body.Joints[JointType.HipRight]);
            DrawLine(g, body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft]);
            DrawLine(g, body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight]);
            DrawLine(g, body.Joints[JointType.KneeLeft], body.Joints[JointType.AnkleLeft]);
            DrawLine(g, body.Joints[JointType.KneeRight], body.Joints[JointType.AnkleRight]);
            DrawLine(g, body.Joints[JointType.AnkleLeft], body.Joints[JointType.FootLeft]);
            DrawLine(g, body.Joints[JointType.AnkleRight], body.Joints[JointType.FootRight]);

            //DrawLine(g, body.Joints[JointType.ShoulderRight], body.Joints[JointType.HipRight]);
            //DrawLine(g, body.Joints[JointType.ShoulderLeft], body.Joints[JointType.HipLeft]);

            // Draw all the joint points
            foreach (Joint joint in body.Joints.Values)
            {
                DrawPoint(g, joint);
            }

        }

        /// <summary>
        /// Draw extra information
        /// </summary>
        /// <param name="g"></param>
        /// <param name="body"></param>
        private void DrawExtras(Graphics g, Body body)
        {
            if (body == null) return;


                // Shoulder projection vertical
                CameraSpacePoint shoulderV = new CameraSpacePoint();
                shoulderV.X = body.Joints[JointType.ShoulderRight].Position.X;
                shoulderV.Y = body.Joints[JointType.SpineBase].Position.Y;
                shoulderV.Z = body.Joints[JointType.ShoulderRight].Position.Z;

                DrawAngle(g, shoulderV, body.Joints[JointType.ShoulderRight].Position, body.Joints[JointType.ElbowRight].Position, 0.45f);
                DrawAngle(g, body.Joints[JointType.ShoulderRight].Position, body.Joints[JointType.ElbowRight].Position, body.Joints[JointType.WristRight].Position, 0.45f);
                DrawAngle(g, body.Joints[JointType.ElbowRight].Position, body.Joints[JointType.WristRight].Position, body.Joints[JointType.HandTipRight].Position, 0.45f);

                DrawLine(g, body.Joints[JointType.ShoulderRight].Position, shoulderV);

        }

        /// <summary>
        /// Draw point
        /// </summary>
        /// <param name="g"></param>
        /// <param name="joint"></param>
        private void DrawPoint(Graphics g, Joint joint)
        {
            if (joint.TrackingState == TrackingState.NotTracked) return;

            /*
            joint = joint.ScaleTo(canvas.ActualWidth, canvas.ActualHeight);

            Ellipse ellipse = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = new SolidColorBrush(Colors.LightBlue)
            };*/
            //CoordinateMapper.MapCameraPointToColorSpace(joint.Position);

            ColorSpacePoint pointColor = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(joint.Position);
            g.DrawEllipse(new Pen(_jointColor, _jointSize), pointColor.X - _jointSize / 2, pointColor.Y - _jointSize / 2, _jointSize, _jointSize);

        }

        private void DrawPoint(Graphics g, CameraSpacePoint p)
        {
            ColorSpacePoint pointColor = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(p);
            g.DrawEllipse(new Pen(_jointColor, _jointSize), pointColor.X - _jointSize / 2, pointColor.Y - _jointSize / 2, _jointSize, _jointSize);
        }

        private void DrawPoint(Graphics g, CameraSpacePoint p, Color c)
        {
            ColorSpacePoint pointColor = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(p);
            g.DrawEllipse(new Pen(c, _jointSize), pointColor.X - _jointSize / 2, pointColor.Y - _jointSize / 2, _jointSize, _jointSize);
        }

        /// <summary>
        /// Draws a line connecting two joints.
        /// </summary>
        /// <param name="g">Graphics device where the painting will be done.</param>
        /// <param name="first">First joint.</param>
        /// <param name="second">Ssecond joint.</param>
        private void DrawLine(Graphics g, Joint first, Joint second)
        {
            if (first.TrackingState == TrackingState.NotTracked || second.TrackingState == TrackingState.NotTracked) return;
            /*
            first = first.ScaleTo(canvas.ActualWidth, canvas.ActualHeight);
            second = second.ScaleTo(canvas.ActualWidth, canvas.ActualHeight);

            Line line = new Line
            {
                X1 = first.Position.X,
                Y1 = first.Position.Y,
                X2 = second.Position.X,
                Y2 = second.Position.Y,
                StrokeThickness = 8,
                Stroke = new SolidColorBrush(Colors.LightBlue)
            };*/
            ColorSpacePoint pointColorFirst = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(first.Position);
            ColorSpacePoint pointColorSecond = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(second.Position);
            g.DrawLine(new Pen(_boneColor, _boneThickness), pointColorFirst.X, pointColorFirst.Y, pointColorSecond.X, pointColorSecond.Y);

        }

        /// <summary>
        /// Draws a line connecting two points.
        /// </summary>
        /// <param name="g">Graphics device where the painting will be done.</param>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        private void DrawLine(Graphics g, CameraSpacePoint p1, CameraSpacePoint p2)
        {
            ColorSpacePoint pointColorFirst = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(p1);
            ColorSpacePoint pointColorSecond = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(p2);
            g.DrawLine(new Pen(_boneColor, _boneThickness), pointColorFirst.X, pointColorFirst.Y, pointColorSecond.X, pointColorSecond.Y);
        }

        /// <summary>
        /// Draws a line connecting two points.
        /// </summary>
        /// <param name="g">Graphics device where the painting will be done.</param>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <param name="c">Color of the line.</param>
        private void DrawLine(Graphics g, CameraSpacePoint p1, CameraSpacePoint p2, Color c)
        {
            ColorSpacePoint pointColorFirst = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(p1);
            ColorSpacePoint pointColorSecond = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(p2);
            g.DrawLine(new Pen(c, _boneThickness), pointColorFirst.X, pointColorFirst.Y, pointColorSecond.X, pointColorSecond.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="body"></param>
        /// <param name="dRadius"></param>
        private void DrawShoulderAngle(Graphics g, Body body, float dRadius)
        {
            // Draw the vertical line
            CameraSpacePoint shoulderP = body.Joints[JointType.SpineMid].Position;
            shoulderP.X += body.Joints[JointType.ShoulderRight].Position.X - body.Joints[JointType.SpineShoulder].Position.X;
            shoulderP.Y += body.Joints[JointType.ShoulderRight].Position.Y - body.Joints[JointType.SpineShoulder].Position.Y;
            shoulderP.Z += body.Joints[JointType.ShoulderRight].Position.Z - body.Joints[JointType.SpineShoulder].Position.Z;

            ColorSpacePoint pointColorProjection = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(shoulderP);
            ColorSpacePoint pointColorShoulder = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(body.Joints[JointType.ShoulderRight].Position);
            ColorSpacePoint pointColorElbow = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(body.Joints[JointType.ElbowRight].Position);
            //Pen pen = new Pen(Color.FromArgb(156, _boneColor), _boneThickness);
            //pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            //g.DrawLine(pen, pointColorProjection.X, pointColorProjection.Y, pointColorShoulder.X, pointColorShoulder.Y);


            // Draw the circumference
            ColorSpacePoint point1 = pointColorProjection;
            ColorSpacePoint point2 = pointColorElbow;

            point1.X = pointColorProjection.X - pointColorShoulder.X;
            point1.Y = pointColorProjection.Y - pointColorShoulder.Y;

            point2.X = pointColorElbow.X - pointColorShoulder.X;
            point2.Y = pointColorElbow.Y - pointColorShoulder.Y;

            Double length1 = point1.X * point1.X + point1.Y * point1.Y;
            Double length2 = point2.X * point2.X + point2.Y * point2.Y;

            float radius = (float)Math.Sqrt(Math.Min(length1, length2));
            radius *= dRadius;

            Double angle1 = Math.Atan(point1.Y / point1.X) * 180 / Math.PI;
            Double angle2 = Math.Atan(point2.Y / point2.X) * 180 / Math.PI;

            //label5.Text = String.Format("Ángulo 1: {0:0.0} — Ángulo 2: {1:0.0}", angle1, angle2);

            angle1 = point1.X < 0 ? 180 + angle1 : angle1;
            angle2 = point2.X < 0 ? 180 + angle2 : angle2;

            //label6.Text = String.Format("Ángulo 1: {0:0.0} — Ángulo 2: {1:0.0}", angle1, angle2);

            // Z-coordinate of the crossproduct between the two vectors
            Double crossProduct = point1.X * point2.Y - point1.Y * point2.X;

            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

            if (point1.X > point2.X)
            {
                path.AddArc(pointColorShoulder.X - radius,
                    pointColorShoulder.Y - radius,
                    2 * radius,
                    2 * radius,
                    (float)angle1,
                    (float)(angle2 - angle1));
            }
            else
            {

                path.AddArc(pointColorShoulder.X - radius,
                    pointColorShoulder.Y - radius,
                    2 * radius,
                    2 * radius,
                    (float)angle2,
                    (float)(angle1 - angle2));
            }

            path.AddLine(pointColorShoulder.X, pointColorShoulder.Y, path.PathPoints[0].X, path.PathPoints[0].Y);
            path.AddLine(pointColorShoulder.X, pointColorShoulder.Y, path.GetLastPoint().X, path.GetLastPoint().Y);

            g.FillPath(new SolidBrush(Color.FromArgb(this._angleColor.A >> 1, this._angleColor)), path);
            g.DrawPath(new Pen(this._angleColor, this._angleSize), path);

        }

        /// <summary>
        /// Draws the angle defined by three points.
        /// </summary>
        /// <param name="g">Drawing object.</param>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <param name="p3">Third point.</param>
        /// <param name="dRadius">Radius of the circle drawn, in percentaje.</param>
        private void DrawAngle(Graphics g, CameraSpacePoint p1, CameraSpacePoint p2, CameraSpacePoint p3, float dRadius)
        {
            ColorSpacePoint first = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(p1);
            ColorSpacePoint second = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(p2);
            ColorSpacePoint third = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(p3);

            ColorSpacePoint point1 = first;
            ColorSpacePoint point2 = third;

            point1.X = first.X - second.X;
            point1.Y = first.Y - second.Y;

            point2.X = third.X - second.X;
            point2.Y = third.Y - second.Y;

            Double length1 = point1.X * point1.X + point1.Y * point1.Y;
            Double length2 = point2.X * point2.X + point2.Y * point2.Y;

            float radius = (float)Math.Sqrt(Math.Min(length1, length2));
            radius *= dRadius;

            Double angle1 = Math.Atan(point1.Y / point1.X) * 180 / Math.PI;
            Double angle2 = Math.Atan(point2.Y / point2.X) * 180 / Math.PI;

            angle1 = point1.X < 0 ? 180 + angle1 : angle1;
            angle2 = point2.X < 0 ? 180 + angle2 : angle2;

            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            float sweepAngle = (float)(angle2 - angle1);
            if (sweepAngle > 180f) sweepAngle -= 360f;
            if (sweepAngle < -180f) sweepAngle += 360f;

            if (point1.X > point2.X)
            {
                path.AddArc(second.X - radius,
                    second.Y - radius,
                    2 * radius,
                    2 * radius,
                    (float)angle1,
                    sweepAngle);
            }
            else
            {
                path.AddArc(second.X - radius,
                    second.Y - radius,
                    2 * radius,
                    2 * radius,
                    (float)angle2,
                    -sweepAngle);
            }

            path.AddLine(second.X, second.Y, path.PathPoints[0].X, path.PathPoints[0].Y);
            path.AddLine(second.X, second.Y, path.GetLastPoint().X, path.GetLastPoint().Y);

            g.FillPath(new SolidBrush(Color.FromArgb(this._angleColor.A >> 1, this._angleColor)), path);
            g.DrawPath(new Pen(this._angleColor, this._angleSize), path);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="start"></param>
        /// <param name="middle"></param>
        /// <param name="end"></param>
        /// <param name="desiredRadius"></param>
        private void DrawAngleV(Graphics g, Vector3 start, Vector3 middle, Vector3 end, double desiredRadius = 0.0)
        {
            Vector3 vector1 = middle - start;
            Vector3 vector2 = middle - end;
            if (desiredRadius == 0.0)
                desiredRadius = Math.Min(vector1.Length, vector2.Length);
            vector1.Normalize();
            vector2.Normalize();
            double angle = Vector3.Angle(vector1, vector2);
            start = middle - desiredRadius * vector1;
            end = middle - desiredRadius * vector2;

            //ColorSpacePoint pointColorFirst = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(middle.Position);
            //ColorSpacePoint pointColorSecond = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(middle.Position);

            g.DrawEllipse(new Pen(Color.Green, 5), 0, 0, 0, 0);

            /*
            this.line1.put_Point(middle.ToPoint());
            this.line2.put_Point(start.ToPoint());
            this.angleFigure.put_StartPoint(end.ToPoint());
            this.arc.put_IsLargeArc(this._angle > 180.0);
            this.arc.put_Point(end.ToPoint());
            ArcSegment arc = this.arc;
            double num = desiredRadius;
            Size size = new Size(num, num);
            arc.put_Size(size);
            */
        }

        #endregion Kinect drawing skeleton

        #region Computations

        // https://stackoverflow.com/questions/52456375/example-async-task-c-sharp
        private async Task BodyReachSagitalPlane(Body body, float w)
        {

            // Variable definitions
            Double angleElbow, angleShoulder, angleWrist, angleObliquity = 0.0;
            Double xElbow, zElbow, xHand, zHand, xShoulderL, zShoulderL;
            //string strText = "";
            StringBuilder strBuilder = new StringBuilder();

            // Shoulder projection vertical
            CameraSpacePoint shoulderV = new CameraSpacePoint();
            shoulderV.X = body.Joints[JointType.ShoulderRight].Position.X;
            shoulderV.Y = body.Joints[JointType.SpineMid].Position.Y;
            shoulderV.Z = body.Joints[JointType.ShoulderRight].Position.Z;
            
            // Calculate angles
            angleShoulder = Extensions.CalculateAngle(shoulderV, body.Joints[JointType.ShoulderRight].Position, body.Joints[JointType.ElbowRight].Position, false);
            angleElbow = Extensions.CalculateAngle(body.Joints[JointType.WristRight].Position, body.Joints[JointType.ElbowRight].Position, body.Joints[JointType.ShoulderRight].Position, false);
            //angleWrist = Extensions.CalculateAngle(body.Joints[JointType.ElbowRight].Position, body.Joints[JointType.WristRight].Position, body.Joints[JointType.HandTipRight].Position, false);
            angleWrist = Extensions.CalculateAngle(body.Joints[JointType.ElbowRight].Position, body.Joints[JointType.WristRight].Position, body.Joints[JointType.HandTipRight].Position, true);
            angleWrist -= 180;

            // Calculate obliquity angle
            // Normal vector to the sagittal plane: vector shoulder left - shoulder right
            Vector3D vShoulderX = body.Joints[JointType.ShoulderRight].Position.ToVector3() - body.Joints[JointType.ShoulderLeft].Position.ToVector3();
            Vector3D vKinectX = new Vector3D(1.0, 0.0, 0.0);
            vShoulderX.Y = 0.0;
            vShoulderX.Normalize();
            angleObliquity = Vector3D.AngleBetween(vKinectX, vShoulderX);
            angleObliquity *= Math.Sign(Vector3D.CrossProduct(vShoulderX, vKinectX).Y);
            //angleObliquity = Vector3.Angle(new Vector3(1, 0, 0), new Vector3(vShoulder.X, vShoulder.Y, vShoulder.Z));

            // Calculate coordinates referred to the body frontal plane

            // Normal vector to the frontal plane: rotate 90° clockwise (in Kinect's coordinate system) multiplying by -i
            Vector3D vShoulderZ = new Vector3D(0.0, 0.0, 0.0);
            vShoulderZ.X = vShoulderX.Z;
            vShoulderZ.Z = -vShoulderX.X;

            // Decompose into basis vectors in sagitalV plane
            Vector3D vDistance;
            vDistance = body.Joints[JointType.HandTipRight].Position.ToVector3() - body.Joints[JointType.ShoulderRight].Position.ToVector3();
            vDistance.Y = 0.0;
            xHand = Vector3D.DotProduct(vDistance, vShoulderX) * 100;
            zHand = Vector3D.DotProduct(vDistance, vShoulderZ) * 100;

            vDistance = body.Joints[JointType.ElbowRight].Position.ToVector3() - body.Joints[JointType.ShoulderRight].Position.ToVector3();
            vDistance.Y = 0.0;
            xElbow = Vector3D.DotProduct(vDistance, vShoulderX) * 100;
            zElbow = Vector3D.DotProduct(vDistance, vShoulderZ) * 100;

            vDistance = body.Joints[JointType.ShoulderLeft].Position.ToVector3() - body.Joints[JointType.ShoulderRight].Position.ToVector3();
            vDistance.Y = 0.0;
            xShoulderL = Vector3D.DotProduct(vDistance, vShoulderX) * 100;
            zShoulderL = Vector3D.DotProduct(vDistance, vShoulderZ) * 100;

            //statusStripLabelXtras.Text = body.Joints[JointType.ShoulderLeft].Position.GetString();
            //statusStrip.Refresh();

            // Plot data into charts
            var now = System.DateTime.Now;
            var nowRelative = System.DateTime.Now - _startingTime;

            // Plot data
            if (_programSettings["PlotsChecked"] == "1")
            {
                await Task.Run(() =>
                {
                    chartA.Invoke((MethodInvoker)(() => chartA.Series[0].Values.Add(new TimePoint { DateTime = nowRelative, Value = angleShoulder })));
                    chartA.Invoke((MethodInvoker)(() => chartA.Series[1].Values.Add(new TimePoint { DateTime = nowRelative, Value = angleElbow })));
                    chartA.Invoke((MethodInvoker)(() => chartA.Series[2].Values.Add(new TimePoint { DateTime = nowRelative, Value = angleWrist })));

                    //chartB.Invoke((MethodInvoker)(() => chartB.Series[0].Values.Add(new ObservablePoint(xHand, zHand))));
                    //chartB.Invoke((MethodInvoker)(() => chartB.Series[1].Values.Add(new ObservablePoint(xElbow, zElbow))));
                    //chartB.Invoke((MethodInvoker)(() => chartB.Series[2].Values.Add(new ObservablePoint(xShoulderL, zShoulderL))));

                    //chartC.Invoke((MethodInvoker)(() => chartC.Value = angleObliquity));
                });

                //chartA.Series[0].Values.Add(new TimePoint { DateTime = nowRelative, Value = angleShoulder });
                //chartA.Series[1].Values.Add(new TimePoint { DateTime = nowRelative, Value = angleElbow });
                //chartA.Series[2].Values.Add(new TimePoint { DateTime = nowRelative, Value = angleWrist });
                /*chartA.Series[3].Values.Add(angleShoulder);
                chartA.Series[4].Values.Add(angleElbow);
                chartA.Series[5].Values.Add(angleWrist);
                chartA.Series[6].Values.Add(body.Joints[JointType.ShoulderRight].Position.ToVector3());*/

                //chartB.Series[0].Values.Add(new ObservablePoint(body.Joints[JointType.HandTipRight].Position.X, body.Joints[JointType.HandTipRight].Position.Z));
                //chartB.Series[1].Values.Add(new ObservablePoint(body.Joints[JointType.ElbowRight].Position.X, body.Joints[JointType.ElbowRight].Position.Z));
                chartB.Series[0].Values.Add(new ObservablePoint(xHand, zHand));
                chartB.Series[1].Values.Add(new ObservablePoint(xElbow, zElbow));
                chartB.Series[2].Values.Add(new ObservablePoint(xShoulderL, zShoulderL));

                chartC.Value = angleObliquity;

                // Force redrawing of the plots
                //chartA.Invalidate();
                //chartB.Invalidate();
                //chartC.Invalidate();

                SetAxisLimits(now);

                    /*
                    if (chartA.Series[0].Values.Count>300)
                    {
                        chartA.Series[0].Values.RemoveAt(0);
                        chartA.Series[1].Values.RemoveAt(0);
                        chartA.Series[2].Values.RemoveAt(0);
                    }*/
                
            }

            // Save data
            if (this._saveData)
            {

                //await Task.Run(() =>
                //{
                    /*strText = ((double)nowRelative.Ticks/TimeSpan.TicksPerSecond).ToString() + "\t";
                    //strText = ((double)((TimePoint)chartA.Series[0].Values[0]).DateTime.Ticks / TimeSpan.TicksPerSecond).ToString() + "\t";
                    strText += angleShoulder.ToString() + "\t";
                    strText += angleElbow.ToString() + "\t";
                    strText += angleWrist.ToString() + "\t";
                    strText += w.ToString() + "\t";
                    strText += body.Joints[JointType.ShoulderLeft].Position.X.ToString() + "\t";
                    strText += body.Joints[JointType.ShoulderLeft].Position.Y.ToString() + "\t";
                    strText += body.Joints[JointType.ShoulderLeft].Position.Z.ToString() + "\t";
                    strText += body.Joints[JointType.ShoulderRight].Position.X.ToString() + "\t";
                    strText += body.Joints[JointType.ShoulderRight].Position.Y.ToString() + "\t";
                    strText += body.Joints[JointType.ShoulderRight].Position.Z.ToString() + "\t";
                    strText += body.Joints[JointType.ElbowRight].Position.X.ToString() + "\t";
                    strText += body.Joints[JointType.ElbowRight].Position.Y.ToString() + "\t";
                    strText += body.Joints[JointType.ElbowRight].Position.Z.ToString() + "\t";
                    strText += body.Joints[JointType.WristRight].Position.X.ToString() + "\t";
                    strText += body.Joints[JointType.WristRight].Position.Y.ToString() + "\t";
                    strText += body.Joints[JointType.WristRight].Position.Z.ToString() + "\t";
                    strText += body.Joints[JointType.HandTipRight].Position.X.ToString() + "\t";
                    strText += body.Joints[JointType.HandTipRight].Position.Y.ToString() + "\t";
                    strText += body.Joints[JointType.HandTipRight].Position.Z.ToString() + "\t";
                    strText += xShoulderL.ToString() + "\t";
                    strText += zShoulderL.ToString() + "\t";
                    strText += xElbow.ToString() + "\t";
                    strText += zElbow.ToString() + "\t";
                    strText += xHand.ToString() + "\t";
                    strText += zHand.ToString() + "\t";
                    strText += angleObliquity.ToString() + "\t" + "\t";
                    
                    _listText.Add(strText);
                    */

                    strBuilder.Append(((double)nowRelative.Ticks / TimeSpan.TicksPerSecond).ToString()).Append("\t");
                    strBuilder.Append(angleShoulder.ToString()).Append("\t");
                    strBuilder.Append(angleElbow.ToString()).Append("\t");
                    strBuilder.Append(angleWrist.ToString()).Append("\t");
                    strBuilder.Append(w.ToString()).Append("\t");
                    strBuilder.Append(body.Joints[JointType.ShoulderLeft].Position.X.ToString()).Append("\t");
                    strBuilder.Append(body.Joints[JointType.ShoulderLeft].Position.Y.ToString()).Append("\t");
                    strBuilder.Append(body.Joints[JointType.ShoulderLeft].Position.Z.ToString()).Append("\t");
                    strBuilder.Append(body.Joints[JointType.ShoulderRight].Position.X.ToString()).Append("\t");
                    strBuilder.Append(body.Joints[JointType.ShoulderRight].Position.Y.ToString()).Append("\t");
                    strBuilder.Append(body.Joints[JointType.ShoulderRight].Position.Z.ToString()).Append("\t");
                    strBuilder.Append(body.Joints[JointType.ElbowRight].Position.X.ToString()).Append("\t");
                    strBuilder.Append(body.Joints[JointType.ElbowRight].Position.Y.ToString()).Append("\t");
                    strBuilder.Append(body.Joints[JointType.ElbowRight].Position.Z.ToString()).Append("\t");
                    strBuilder.Append(body.Joints[JointType.WristRight].Position.X).Append("\t");
                    strBuilder.Append(body.Joints[JointType.WristRight].Position.Y).Append("\t");
                    strBuilder.Append(body.Joints[JointType.WristRight].Position.Z).Append("\t");
                    strBuilder.Append(body.Joints[JointType.HandTipRight].Position.X.ToString()).Append("\t");
                    strBuilder.Append(body.Joints[JointType.HandTipRight].Position.Y.ToString()).Append("\t");
                    strBuilder.Append(body.Joints[JointType.HandTipRight].Position.Z.ToString()).Append("\t");
                    strBuilder.Append(xShoulderL.ToString()).Append("\t");
                    strBuilder.Append(zShoulderL.ToString()).Append("\t");
                    strBuilder.Append(xElbow.ToString()).Append("\t");
                    strBuilder.Append(zElbow.ToString()).Append("\t");
                    strBuilder.Append(xHand.ToString()).Append("\t");
                    strBuilder.Append(zHand.ToString()).Append("\t");
                    strBuilder.Append(angleObliquity.ToString()).Append("\t").Append("\t");

                    await file.WriteLineAsync(strBuilder.ToString());

                    /*
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(_path, true))
                    {
                        file.WriteLine(strText);
                    }*/
                //}).ConfigureAwait(false);

            }
          

            /* Draw parameters on the canvas
            Graphics g = Graphics.FromImage(img);
            // Right elbow angle
            DrawAngle(g, body.Joints[JointType.ShoulderRight].Position, body.Joints[JointType.ElbowRight].Position, body.Joints[JointType.WristRight].Position, 0.4f);
            // Elevation angle with spine
            DrawAngle(g, shoulderSpine, body.Joints[JointType.ShoulderRight].Position, body.Joints[JointType.ElbowRight].Position, 0.55f);
            // Elevation angle vertical
            //DrawAngle(g, shoulderV, body.Joints[JointType.ShoulderRight].Position, body.Joints[JointType.ElbowRight].Position, 0.55f);
            // Draw point in SagitalV plane
            DrawPoint(g, Extensions.ToCameraSpace(vProjectionV), Color.DarkSeaGreen);
            // Draw point in sagital plane
            DrawPoint(g, Extensions.ToCameraSpace(vProjection), Color.DarkGoldenrod);
            */

        }
        
        #endregion Computations


        private void SetAxisLimits(System.DateTime now)
        {
            if (((now - _startingTime).Ticks / TimeSpan.TicksPerSecond) >= 10.0)
            {
                chartA.AxisX[0].MaxValue = now.Ticks + TimeSpan.FromSeconds(1).Ticks - _startingTime.Ticks; // lets force the axis to be 100ms ahead
                chartA.AxisX[0].MinValue = now.Ticks - TimeSpan.FromSeconds(9).Ticks - _startingTime.Ticks; //we only care about the last 8 seconds
            }
            else
            {
                chartA.AxisX[0].MaxValue = TimeSpan.FromSeconds(10).Ticks;
            }
        }

        public class TimePoint
        {
            public System.TimeSpan DateTime { get; set; }
            public double Value { get; set; }
        }

        private void size_Changed(object sender, EventArgs e)
        {
            var numUpDown = sender as ToolStripNumericUpDown;

            if (numUpDown.Name == "toolStripMain_AngleWidth")
            {
                _angleSize = (int)numUpDown.NumericUpDownControl.Value;
            }
            else if (numUpDown.Name == "toolStripMain_JointWidth")
            {
                _jointSize = (int)numUpDown.NumericUpDownControl.Value;
            }
            else if (numUpDown.Name == "toolStripMain_SkeletonWidth")
            {
                _boneThickness = (int)numUpDown.NumericUpDownControl.Value;
            }
        }

        private void bckWorkerDraw_DoWork(object sender, DoWorkEventArgs e)
        {
            //var tupi = e.Argument as Tuple<Graphics, Body>;
            //var parameters = e.Argument as Tuple<Graphics, Body>;

            //DrawExtras(parameters.Item1, parameters.Item2);
            //DrawSkeleton(parameters.Item1, parameters.Item2);

            //
            chartA.Invalidate();
            chartB.Invalidate();
            chartC.Invalidate();
            pctVideoColor.Invalidate();
        }

        private void SetDoubleBuffering(System.Windows.Forms.Control control, bool value)
        {
            System.Reflection.PropertyInfo controlProperty = typeof(System.Windows.Forms.Control)
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            controlProperty.SetValue(control, value, null);
        }

    }
}
