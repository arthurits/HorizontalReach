using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using Microsoft.Kinect;

using KinectStreams;

//using Microsoft.Speech.AudioFormat;
//using Microsoft.Speech.Recognition;

using LiveCharts; //Core of the library
using LiveCharts.Wpf; //The WPF controls
using LiveCharts.WinForms; //The WinForm wrappers
using LiveCharts.Defaults;

namespace Alcances
{
    // Video encoding: https://code.msdn.microsoft.com/windowsapps/How-to-encode-several-to-a-053953d1
    public partial class frmKinectVideo : Form
    {
        #region Members

        /// <summary>
        /// Radius of drawn joint circles
        /// </summary>
        private Int32 _jointSize;

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private Int32 _boneThickness;

        /// <summary>
        /// Color of the drawn joint circles
        /// </summary>
        private Color _jointColor;

        /// <summary>
        /// Color of the drawn joint lines
        /// </summary>
        private Color _boneColor;

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        private const double ClipBoundsThickness = 10;

        /// <summary>
        /// Constant for clamping Z values of camera space points from being negative
        /// </summary>
        private const float InferredZPositionClamp = 0.1f;

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as closed
        /// </summary>
        private readonly Brush handClosedBrush = new SolidBrush(Color.FromArgb(128, 255, 0, 0));
        
        /// <summary>
        /// Brush used for drawing hands that are currently tracked as opened
        /// </summary>
        private readonly Brush handOpenBrush = new SolidBrush(Color.FromArgb(128, 0, 255, 0));

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as in lasso (pointer) position
        /// </summary>
        private readonly Brush handLassoBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 255));

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        private readonly Brush trackedJointBrush = new SolidBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred
        /// </summary>        
        private readonly Brush inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Pen used for drawing bones that are currently inferred
        /// </summary>        
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        /// <summary>
        /// Drawing group for body rendering output
        /// </summary>
        //private DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        //private DrawingImage imageSource;

        /// <summary>
        /// Bitmap to display
        /// </summary>
        private Bitmap colorBitmap = null;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor _kinectSensor = null;

        /// <summary>
        /// Stream for 32b-16b conversion.
        /// </summary>
        private KinectAudioStream convertStream = null;

        /// <summary>
        /// Speech recognition engine using audio data from Kinect.
        /// </summary>
        //private SpeechRecognitionEngine speechEngine = null;

        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper coordinateMapper = null;

        /// <summary>
        /// Reader for frames
        /// </summary>
        private MultiSourceFrameReader _reader = null;

        /// <summary>
        /// Array for the bodies
        /// </summary>
        //private Body[] _bodies = null;
        private IList<Body> _bodies;

        /// <summary>
        /// Monitors whether the frama has been received or not
        /// </summary>
        private bool _frameReceived;

        private bool _displayBody;

        private bool _displayAngle;

        private bool _displayImage;

        private bool _calculateReach;

        /// <summary>
        /// definition of bones
        /// </summary>
        private List<Tuple<JointType, JointType>> bones;

        /// <summary>
        /// Width of display (depth space)
        /// </summary>
        private int displayWidth;

        /// <summary>
        /// Height of display (depth space)
        /// </summary>
        private int displayHeight;

        /// <summary>
        /// List of colors for each body tracked
        /// </summary>
        private List<Pen> bodyColors;

        /// <summary>
        /// Current status text to display
        /// </summary>
        private string statusText = null;

        #endregion Members


        System.DateTime _startingTime;
        System.Collections.ArrayList _floorLevel;
        System.Collections.ArrayList _elbowFlexion;
        bool _mirror;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public frmKinectVideo()
        {
            InitializeComponent();

            //this.ribbonButton2.Image = (new Bitmap("Resources/Exit.bmp"));
            //new Bitmap(Resources.myImage);
            //this.ribbonButton5.Image = Image.FromFile(@"..\Icons\Save picture.ico");
            //this.ribbonButton4.Image = Image.FromFile(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Icons\Save 2.ico");
            var path = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            
            
            

            //Start Kinect initialization
            //InitializeKinect();

            _displayBody = true;
            _displayAngle = false;
            
            _displayImage = true;
            _calculateReach = true;
            _mirror = true;
            
            numSkeleton.Value = 10;
            numAngle.Value = 20;
            _boneColor = Color.Red;
            pctSkeleton.BackColor = Color.Red;
            _jointColor = Color.Purple;
            pctAngle.BackColor = Color.Purple;

            Bitmap bmp = new Bitmap(pctVideoColor.Width, pctVideoColor.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);
            }
            pctVideoColor.Image = bmp;



            InitializeCharts();

        }

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

        /// <summary>
        /// Initializes the chart objets
        /// </summary>
        private void InitializeCharts()
        {
            // Chart A initialization
            this.chartA.LegendLocation = LegendLocation.Top;
            this.chartA.BackColor = System.Drawing.Color.White;
            this.chartA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartA.Hoverable = false;
            this.chartA.DisableAnimations = true;
            this.chartA.DataTooltip = null;
            this.chartA.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Seconds",
                FontSize = 16,
                LabelFormatter = value => new System.DateTime((long)value).ToString("m:ss"),
                //Separator = new Separator { Step = TimeSpan.FromSeconds(1).Ticks }
                //Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" }
            });
            this.chartA.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Angle",
                FontSize = 16
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
            this.chartA.Series = new SeriesCollection (mapper)
            {
                new LineSeries
                {
                    Title = "Sagital V",
                    Values = new ChartValues<TimePoint> { },
                    PointGeometry = null,
                    LineSmoothness = 1,
                    //Stroke = System.Windows.Media.Brushes.Aqua,
                    Fill = System.Windows.Media.Brushes.Transparent

                },
                new LineSeries
                {
                    Title = "Sagital",
                    Values = new ChartValues<TimePoint> { },
                    PointGeometry = null,
                    LineSmoothness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent
                },
                new LineSeries
                {
                    Title = "Elbow flexion",
                    Values = new ChartValues<TimePoint> { },
                    PointGeometry = null,
                    LineSmoothness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Visibility = System.Windows.Visibility.Hidden
                },
                new LineSeries
                {
                    Title = "Floor plane",
                    Values = new ChartValues<float> { },
                    PointGeometry = null,
                    LineSmoothness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Visibility = System.Windows.Visibility.Collapsed
                },
                new LineSeries
                {
                    Title = "Obliquity",
                    Values = new ChartValues<double> { },
                    PointGeometry = null,
                    LineSmoothness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Visibility = System.Windows.Visibility.Collapsed
                },
                new LineSeries
                {
                    Title = "SagitalV deviation",
                    Values = new ChartValues<double> { },
                    PointGeometry = null,
                    LineSmoothness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Visibility = System.Windows.Visibility.Collapsed
                },
                new LineSeries
                {
                    Title = "Shoulder",
                    Values = new ChartValues<Vector3D> { },
                    PointGeometry = null,
                    LineSmoothness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Visibility = System.Windows.Visibility.Collapsed
                }
            };
            
            // Chart B initialization
            this.chartB.LegendLocation = LegendLocation.Top;
            this.chartB.BackColor = System.Drawing.Color.White;
            this.chartB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartB.Hoverable = false;
            this.chartB.DisableAnimations = true;
            this.chartB.DataTooltip = null;
            this.chartB.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Z / cm",
                FontSize = 16,
                Width = 12
            });
            this.chartB.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Y / cm",
                FontSize = 16
            });
            //this.chartB.AxisY[0].MinValue = 0;
            this.chartB.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Sagital V",
                    Values = new ChartValues<ObservablePoint> (),
                    PointGeometry = null,
                    LineSmoothness = 1,
                    //Stroke = System.Windows.Media.Brushes.Aqua,
                    Fill = System.Windows.Media.Brushes.Transparent

                },
                new LineSeries
                {
                    Title = "Sagital",
                    Values = new ChartValues<ObservablePoint> (),
                    PointGeometry = null,
                    LineSmoothness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent
                }
            };
        }

        private void Form_Closed(object sender, EventArgs e)
        { 
            if (_reader != null) 
            { 
                _reader.Dispose(); 
            } 

            if (_kinectSensor != null) 
            { 
                _kinectSensor.Close(); 
            } 
        }

        public void InitializeKinect()
        {
            _kinectSensor = KinectSensor.GetDefault();

            if (_kinectSensor != null)
            {
                // Turn on kinect
                _kinectSensor.Open();
                displayHeight = _kinectSensor.ColorFrameSource.FrameDescription.Height;
                displayWidth = _kinectSensor.ColorFrameSource.FrameDescription.Width;
                //displayWidth = pctVideoColor.Width;
                //displayHeight = pctVideoColor.Height;
            

                //var reader = kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.BodyIndex | FrameSourceTypes.Body);
                //_reader = _kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
                //_reader = _kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);
                _reader = _kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;

                /*bodyFrameReader = kinectSensor.BodyFrameSource.OpenReader();

                if (bodyFrameReader != null)
                {
                    //bodyFrameReader.FrameArrived += Reader_FrameArrived;
                }*/

                //kinectSensor.ColorFrameSource.OpenReader() += Color_FrameArrived;
                // create the colorFrameDescription from the ColorFrameSource using Bgra format
                FrameDescription colorFrameDescription = this._kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);
                this.colorBitmap = new Bitmap(displayWidth, displayHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            }

        }

        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            _frameReceived = false;
            Image img = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppRgb); ;

            // Get a reference to the multi-frame
            var reference = e.FrameReference.AcquireFrame();
            
            // Get the colorstream and paint it to a picture box
            using (ColorFrame colorFrame = reference.ColorFrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    //byte[] pixels = new byte[displayWidth * displayHeight * kinectSensor.ColorFrameSource.FrameDescription.BytesPerPixel];
                    //byte[] pixels = new byte[kinectSensor.ColorFrameSource.FrameDescription.LengthInPixels * kinectSensor.ColorFrameSource.FrameDescription.BytesPerPixel];
                    //colorFrame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
                    //Image cImg = Image.FromStream(new System.IO.MemoryStream(pixels));
                    //pctVideoColor.Image = cImg;
                    //pctVideoColor.Image = CreateImageFromFrame(colorFrame);
                    if (_displayImage)
                    { 
                        img = KinectStreams.Extensions.CreateImageFromFrame(colorFrame);
                        //pctVideoColor.Image = img;
                        //Graphics g = Graphics.FromImage(img);
                        //g.FillRectangle(Brushes.Red, 0, 20, 200, 10);
                        //https://social.msdn.microsoft.com/Forums/windows/en-US/989fdf86-5bcc-4b68-bbd3-a850d43de6f6/is-picturebox-the-best-way-to-draw-a-line
                    }
                    else
                    {
                        img = new Bitmap(colorFrame.FrameDescription.Width, colorFrame.FrameDescription.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        using (Graphics g = Graphics.FromImage(img))
                        {
                            g.Clear(Color.White);
                        }
                        //pctVideoColor.Image = img;
                    }
                }

                //Image<ColorImageFormat.Bgra, byte> colorImage = new Image<ColorImageFormat.Bgra, byte>(displayWidth, displayHeight);          
                //colorFrame.CopyConvertedFrameDataToIntPtr(this.colorBitmap.BackBuffer, (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4), ColorImageFormat.Bgra);
                //pctVideoColor.Image = colorFrame;
            }
            
            // Get the body stream
            using (BodyFrame bodyFrame = reference.BodyFrameReference.AcquireFrame())
            {
                _frameReceived = false;
                if (bodyFrame != null)
                {
                    _bodies = new Body[bodyFrame.BodyFrameSource.BodyCount];
                    bodyFrame.GetAndRefreshBodyData(_bodies);
                    _frameReceived = true;
                    // _frameCounter ++;
                    // _floorLevel = bodyFrame.FloorClipPlane.W;
                }
            

            // http://pterneas.com/2014/03/13/kinect-for-windows-version-2-body-tracking/
            // Body TrackedSkeleton = bodies.FirstOrDefault(s => s.IsTracked == true);
            if (_frameReceived == true)
            {
                Body body = _bodies.Closest();
                //foreach (Body body in _bodies)
                //{
                    if (body != null)
                    {
                        if (body.IsTracked)
                        {
                            //IReadOnlyDictionary<JointType, Joint> joints = body.Joints;
                            //IDictionary<JointType,Joint> joints = body.Joints.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                            //Dictionary<JointType, Point> jointPoints = new Dictionary<JointType, Point>();

                            // http://pterneas.com/2014/05/06/understanding-kinect-coordinate-mapping/
                            
                            //Joint midSpine = joints[JointType.SpineMid];
                            //float ms_distance_x = midSpine.Position.X;
                            //CameraSpacePoint pointCamera = body.Joints[JointType.HandTipRight].Position;
                            //ColorSpacePoint pointColor = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(pointCamera);
                            //lblRightHandTip.Text = _pointColor.Y.ToString("#.##");
                            /*lblRightHandTip.Text = string.Format("X-value: {0:0.00} — Y-value: {1:0.00} — Z-value: {2:0.00}",
                                body.Joints[JointType.HandRight].Position.X,
                                body.Joints[JointType.HandRight].Position.Y,
                                body.Joints[JointType.HandRight].Position.Z);
                            */
                            lblRightHandTip.Text = string.Format("X-value: {0:0.00} m — Y-value: {1:0.00} m — Z-value: {2:0.00} m",
                                body.Joints[JointType.AnkleRight].Position.X,
                                body.Joints[JointType.AnkleRight].Position.Y,
                                body.Joints[JointType.AnkleRight].Position.Z);

                            // Calculate the body reach
                            if (_calculateReach)
                            {
                                BodyReachSagitalPlane(body, img, bodyFrame.FloorClipPlane.W);
                            }

                            //lblRightHandTip.Text = body.Joints[JointType.HandRight].Position.X.ToString();
                            if (_displayBody)
                            {
                                DrawSkeleton(Graphics.FromImage(img), body);
                                //DrawSkeleton(Graphics.FromHwnd(pctVideoColor.Handle), body);
                            }

                        }
                    }
                //}
            }
                
            }
            if (!_mirror)
                img.RotateFlip(RotateFlipType.Rotate180FlipY);
            pctVideoColor.Image = img;
        }

        /// <summary>
        /// Compute the body reach in the sagital plane
        /// </summary>
        /// <param name="body">A Microsoft.Kinect class that represents a single body</param>
        private void BodyReachSagitalPlane(Body body, Image img, float floorLevel)
        {

            // Shoulder projection paralell to the body spine
            CameraSpacePoint shoulderSpine = body.Joints[JointType.SpineMid].Position;
                shoulderSpine.X += body.Joints[JointType.ShoulderRight].Position.X - body.Joints[JointType.SpineShoulder].Position.X;
                shoulderSpine.Y += body.Joints[JointType.ShoulderRight].Position.Y - body.Joints[JointType.SpineShoulder].Position.Y;
                shoulderSpine.Z += body.Joints[JointType.ShoulderRight].Position.Z - body.Joints[JointType.SpineShoulder].Position.Z;

            // Shoulder projection vertical
            CameraSpacePoint shoulderV = new CameraSpacePoint();
                shoulderV.X = body.Joints[JointType.ShoulderRight].Position.X;
                shoulderV.Y = body.Joints[JointType.SpineMid].Position.Y;
                shoulderV.Z = body.Joints[JointType.ShoulderRight].Position.Z;

            // Variable definitions
            Double angleElbow, angleObliquity, angleSagitalElevation, angleSagitalElevationV, angleSagitalDeviation;
            Double coordE1, coordE2, coordE1V, coordE2V;
            //angleRaw = Extensions.CalculateAngle(shoulderSpine, body.Joints[JointType.ShoulderRight].Position, body.Joints[JointType.HandTipRight].Position);
            //angleRawV = Extensions.CalculateAngle(shoulderV, body.Joints[JointType.ShoulderRight].Position, body.Joints[JointType.HandTipRight].Position);
            //Graphics g = Graphics.FromImage(pctVideoColor.Image);

            // Normal vector to the sagittal plane: vector shoulder left - shoulder right
            Vector3D vectorSagital = body.Joints[JointType.ShoulderRight].Position.ToVector3() - body.Joints[JointType.ShoulderLeft].Position.ToVector3();
            Vector3D vectorSagitalV = vectorSagital;
            vectorSagitalV.Y = 0; // It goes along the X axis, paralell to the floor
            vectorSagital.Normalize();
            vectorSagitalV.Normalize();

            // Calculate the base of the sagitalV plane
            Vector3D e1V = shoulderV.ToVector3() - body.Joints[JointType.ShoulderRight].Position.ToVector3();
            Vector3D e2V = Vector3D.CrossProduct(vectorSagitalV, e1V);
            e1V.Normalize();
            e2V.Normalize();

            // Calculate the base of the sagital plane
            Vector3D e1 = body.Joints[JointType.AnkleRight].Position.ToVector3() - (Vector3D.DotProduct(body.Joints[JointType.AnkleRight].Position.ToVector3() - body.Joints[JointType.ShoulderRight].Position.ToVector3(), vectorSagital) * vectorSagital) - body.Joints[JointType.ShoulderRight].Position.ToVector3();
            Vector3D e2 = Vector3D.CrossProduct(vectorSagital, e1);
            e1.Normalize();
            e2.Normalize();

            //Project right hand tip onto sagitalV plane
            Vector3D vHandTipRight = body.Joints[JointType.HandTipRight].Position.ToVector3();
            Vector3D vShoulderRight = body.Joints[JointType.ShoulderRight].Position.ToVector3();
            Vector3D vProjectionV = new Vector3D();
            vProjectionV = vHandTipRight - Vector3D.DotProduct(vHandTipRight - vShoulderRight, vectorSagitalV) * vectorSagitalV;

            if (Vector3D.CrossProduct(vectorSagitalV, new Vector3D(1.0, 0.0, 0.0)).Y >= 0)
                angleSagitalElevationV = Extensions.CalculateAngle(shoulderV, body.Joints[JointType.ShoulderRight].Position, Extensions.ToCameraSpace(vProjectionV), true);
            else
                angleSagitalElevationV = Extensions.CalculateAngle(Extensions.ToCameraSpace(vProjectionV), body.Joints[JointType.ShoulderRight].Position, shoulderV, true);

            //Project right hand tip onto sagital plane
            Vector3D vProjection = new Vector3D();
            vProjection = vHandTipRight - Vector3D.DotProduct(vHandTipRight - vShoulderRight, vectorSagital) * vectorSagital;

            if (Vector3D.CrossProduct(vectorSagital, new Vector3D(1.0, 0.0, 0.0)).Y >= 0)
                angleSagitalElevation = Extensions.CalculateAngle(Extensions.ToCameraSpace(e1 + body.Joints[JointType.ShoulderRight].Position.ToVector3()), body.Joints[JointType.ShoulderRight].Position, Extensions.ToCameraSpace(vProjection), true);
            else
                angleSagitalElevation = Extensions.CalculateAngle(Extensions.ToCameraSpace(vProjection), body.Joints[JointType.ShoulderRight].Position, Extensions.ToCameraSpace(e1 + body.Joints[JointType.ShoulderRight].Position.ToVector3()), true);
            
            // Decompose into basis vectors in sagitalV plane
            coordE1V = Vector3D.DotProduct(vProjectionV - vShoulderRight, e1V)  * 100;
            coordE2V = Vector3D.DotProduct(vProjectionV - vShoulderRight, e2V) * 100;

            // Decompose into basis vector in sagital plane
            coordE1 = Vector3D.DotProduct(vProjection - vShoulderRight, e1)  * 100;
            coordE2 = Vector3D.DotProduct(vProjection - vShoulderRight, e2) * 100;

            // Calculate angles
            angleElbow = Extensions.CalculateAngle(body.Joints[JointType.WristRight].Position, body.Joints[JointType.ElbowRight].Position, body.Joints[JointType.ShoulderRight].Position, false);
            angleObliquity = Vector3D.AngleBetween((new CameraSpacePoint { X = 1f, Y = 0f, Z = 0f }).ToVector3(), vectorSagitalV);
            angleObliquity *= Math.Sign(Vector3D.CrossProduct(vectorSagitalV, new Vector3D { X = 1f, Y = 0f, Z = 0f }).Y);

            //var vectorHandXZ = new Vector3D { X = body.Joints[JointType.HandTipRight].Position.X, Y = 0f, Z = body.Joints[JointType.HandTipRight].Position.Z };
            Vector3D vectorHandXZ = vHandTipRight - body.Joints[JointType.ShoulderRight].Position.ToVector3();
            vectorHandXZ.Y = 0;
            angleSagitalDeviation = Vector3D.AngleBetween(vectorHandXZ, e2V);
            angleSagitalDeviation *= Math.Sign(Vector3D.CrossProduct(vectorHandXZ, e2V).Y);
            angleSagitalDeviation = angleSagitalDeviation <= 90 ? angleSagitalDeviation : 180 - angleSagitalDeviation;

            // Show information in labels
            lblAngleV.Text = angleSagitalElevationV.ToString("#.#") + "°";
            lblAngle.Text = angleSagitalElevation.ToString("#.#") + "°";
            lblE1V.Text = coordE1V.ToString("#.#") + " cm";
            lblE2V.Text = coordE2V.ToString("#.#") + " cm";
            lblE1.Text = coordE1.ToString("#.#") + " cm";
            lblE2.Text = coordE2.ToString("#.#") + " cm";

            lblObliquity.Text = angleObliquity.ToString("#.#") + "°";
            lblSagitalVDeviation.Text = angleSagitalDeviation.ToString("#.#") + "°";
            lblFloor.Text = floorLevel.ToString("#.##") + " m";
            if (_displayAngle)
            {
                // Plot data into charts
                var now = System.DateTime.Now;
                var nowRelative = System.DateTime.Now - _startingTime;

                chartA.Series[0].Values.Add(new TimePoint { DateTime = nowRelative, Value = angleSagitalElevationV });
                chartA.Series[1].Values.Add(new TimePoint { DateTime = nowRelative, Value = angleSagitalElevation });
                chartA.Series[2].Values.Add(new TimePoint { DateTime = nowRelative, Value = angleElbow });
                chartA.Series[3].Values.Add(floorLevel);
                chartA.Series[4].Values.Add(angleObliquity);
                chartA.Series[5].Values.Add(angleSagitalDeviation);
                chartA.Series[6].Values.Add(body.Joints[JointType.ShoulderRight].Position.ToVector3());

                chartB.Series[0].Values.Add(new ObservablePoint(coordE2V, -coordE1V));
                chartB.Series[1].Values.Add(new ObservablePoint(coordE2, -coordE1));

                SetAxisLimits(now);
            }

            // Draw parameters on the canvas
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
            //
            
            /*
            // Plot the axes of the sagitalV plane
            Vector3D prod1 = e1V + body.Joints[JointType.ShoulderRight].Position.ToVector3();
            Vector3D prod2 = e2V + body.Joints[JointType.ShoulderRight].Position.ToVector3();
            Vector3D prod3 = vectorSagitalV + body.Joints[JointType.ShoulderRight].Position.ToVector3();
            DrawLine(g, Extensions.ToCameraSpace(prod1), body.Joints[JointType.ShoulderRight].Position, Color.DarkSeaGreen);
            DrawLine(g, Extensions.ToCameraSpace(prod2), body.Joints[JointType.ShoulderRight].Position, Color.DarkSeaGreen);
            DrawLine(g, Extensions.ToCameraSpace(prod3), body.Joints[JointType.ShoulderRight].Position, Color.DarkSeaGreen);
            */

            // Documentation
            //
            // http://stackoverflow.com/questions/23472048/projecting-3d-points-to-2d-plane
            // http://stackoverflow.com/questions/8942950/how-do-i-find-the-orthogonal-projection-of-a-point-onto-a-plane
            // http://stackoverflow.com/questions/9605556/how-to-project-a-3d-point-to-a-3d-plane
            //
            // http://blog.hackandi.com/inst/blog/2014/03/18/convert-kinect-cameraspace-to-worldspace-relative-to-floor/
            // http://gamedev.stackexchange.com/questions/80310/transform-world-space-using-kinect-floorclipplane-to-move-origin-to-floor-while
        }

        /// <summary>
        /// Compute the REBA method
        /// </summary>
        /// <param name="body">A Microsoft.Kinect class that represents a single body</param>
        private void posturalREBA(Body body)
        {

        }

        /// <summary>
        /// Compute the RULA method
        /// </summary>
        /// <param name="body">A Microsoft.Kinect class that represents a single body</param>
        private void posturalRULA(Body body)
        {

        }

        /// <summary>
        /// Compute the OWAS method
        /// </summary>
        /// <param name="body">A Microsoft.Kinect class that represents a single body</param>
        private void posturalOWAS(Body body)
        {

        }


        #region UI events

        private void btnStream_Click(object sender, EventArgs e)
        {
            if (btnStream.Text=="Stream")
            {
                _kinectSensor = KinectSensor.GetDefault();

                if (_kinectSensor != null)
                {
                    this.DoubleBuffered = true;
                    _kinectSensor.IsAvailableChanged += KinectSensor_IsAvailableChanged;
                    _kinectSensor.Open();

                    lblConnectionID.Text = _kinectSensor.UniqueKinectId.ToString();
                    btnStream.Text = "Stop";
                    //this.ribbonButton1.Text = "Stop";
                    //this.ribbonButton1.Image = global::Alcances.Properties.Resources.iconConnect.ToBitmap();
                    //this.ribbonButton1.Image = new Icon(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Icons\Connect 2.ico", 48,48).ToBitmap();

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
                }

            }
            else
            {
                if (_kinectSensor != null && _kinectSensor.IsOpen)
                {
                    this.DoubleBuffered = false;
                    //_reader = _kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Body);
                    _kinectSensor.Close();
                    _reader.MultiSourceFrameArrived -= Reader_MultiSourceFrameArrived;
                    _kinectSensor.IsAvailableChanged -= KinectSensor_IsAvailableChanged;

                    if (_reader != null) _reader.Dispose();
                    btnStream.Text = "Stream";
                    //this.ribbonButton1.Text = "Stream";
                    //this.ribbonButton1.Image = global::Alcances.Properties.Resources.iconDisconnect.ToBitmap();
                    //this.ribbonButton1.Image = new Icon(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Icons\disconnect.ico", 48, 48).ToBitmap();
                    //pctVideoColor.Image = null;
                    lblStatus.Text = "Disconnected";
                    lblConnectionID.Text = "";

                    chartA.AxisX[0].MinValue = 0;
                }
            }
        }
        /// <summary>
        /// Event fired when a Kinect Sensor is connected to the PC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KinectSensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            
            if (e.IsAvailable==true)
            {
                lblStatus.Text = e.IsAvailable.ToString();
                lblConnectionID.Text = _kinectSensor.UniqueKinectId;
            }
            else
            {
                lblStatus.Text = "Disconnected";
                lblConnectionID.Text = "";
            }
            //throw new NotImplementedException();
        }

        private void skeleton_Changed(object sender, EventArgs e)
        {
            //_displayBody = ribbonCheckBox1.Checked;
        }
        private void angle_Changed(object sender, EventArgs e)
        {
            //_displayAngle = ribbonCheckBox2.Checked;
        }
        private void picture_Changed(object sender, EventArgs e)
        {
            //_displayImage = ribbonCheckBox3.Checked;
        }
        private void plot_angle_Changed(object sender, EventArgs e)
        {
            //chartA.UpdaterState = ribbonCheckBox4.Checked ? UpdaterState.Running : UpdaterState.Paused;
        }
        private void plot_reach_Changed(object sender, EventArgs e)
        {
            //chartB.UpdaterState = ribbonCheckBox5.Checked ? UpdaterState.Running : UpdaterState.Paused;
        }
        private void mirror_Changed(object sender, EventArgs e)
        {
            //_mirror = rCheckMirror.Checked;
        }
        private void exit_Click(object sender, EventArgs e)
        {
            //this.btnStream_Click;
            this.Close();
            //Application.Exit();
        }

        private void color_Click(object sender, EventArgs e)
        {
            if (dlgColor.ShowDialog() == DialogResult.OK)
            {
                var pictureBox = sender as PictureBox;
                if (pictureBox.Name == "pctSkeleton")
                {
                    pctSkeleton.BackColor = dlgColor.Color;
                    _boneColor = dlgColor.Color;
                }
                else if (pictureBox.Name == "pctAngle")
                {
                    pctAngle.BackColor = dlgColor.Color;
                    _jointColor = dlgColor.Color;
                }
                
            }

        }

        private void size_Changed(object sender, EventArgs e)
        {
            var numUpDown = sender as NumericUpDown;
            if (numUpDown.Name == "numSkeleton")
                _boneThickness = (int)numUpDown.Value;
            else if (numUpDown.Name == "numAngle")
                _jointSize = (int)numUpDown.Value;
        }

        private void save_Click(object sender, EventArgs e)
        {
            // http://stackoverflow.com/questions/9907682/create-a-txt-file-if-doesnt-exist-and-if-it-does-append-a-new-line

            String path = @"E:\Angle.txt";

            // Displays a SaveFileDialog so the user can save the data
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveDialog.Title = "Save data to disk file";
            saveDialog.CreatePrompt = true;
            saveDialog.OverwritePrompt = true;
            if (saveDialog.ShowDialog()==DialogResult.Cancel) return;

            path = saveDialog.FileName;

            if (!File.Exists(path))
                File.Create(path).Dispose();

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
            {
                file.WriteLine("Seconds\tElbow angle\tFloor height\tObliquity\tSagital deviation\tOrthogonal angle\tOrthogonal X\tOrthogonal Y\tAngle\tX\tY\tShoulder X\tShoulder Y\tShoulder Z\t\t");
                for (int i = 0; i < chartB.Series[0].Values.Count; i++)
                {
                    path = ((double)((TimePoint)chartA.Series[0].Values[i]).DateTime.Ticks / TimeSpan.TicksPerSecond).ToString() + "\t";
                    path += ((TimePoint)chartA.Series[2].Values[i]).Value.ToString() + "\t";
                    path += (chartA.Series[3].Values[i]).ToString() + "\t";
                    path += chartA.Series[4].Values[i].ToString() + "\t";
                    path += chartA.Series[5].Values[i].ToString() + "\t";
                    for (int j = 0; j < chartB.Series.Count; j++)
                    {
                        path += ((TimePoint)chartA.Series[j].Values[i]).Value.ToString() + "\t";
                        path += ((ObservablePoint)chartB.Series[j].Values[i]).X.ToString() + "\t";
                        path += ((ObservablePoint)chartB.Series[j].Values[i]).Y.ToString() + "\t";
                    }
                    path += ((Vector3D)chartA.Series[6].Values[i]).X.ToString() + "\t";
                    path += ((Vector3D)chartA.Series[6].Values[i]).Y.ToString() + "\t";
                    path += ((Vector3D)chartA.Series[6].Values[i]).Z.ToString() + "\t" + "\t";
                    file.WriteLine(path);
                }
            }

            MessageBox.Show("Data has been successfully\rsaved to disk", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }

        private void picture_Save(object sender, EventArgs e)
        {
            String path = Environment.SpecialFolder.DesktopDirectory.ToString();
            var img = pctVideoColor.Image;

            // Displays a SaveFileDialog so the user can save the data
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Bitmap (*.bmp)|*.bmp|Jpg (*.jpg)|*.jpg|Jpeg (*.jpeg)|*.jpeg|Portable network graphics (*.png)|*.png|Tiff (*.tiff)|*.tiff|Wmf (*.wmf)|*.wmf|All files (*.*)|*.*";
            saveDialog.Title = "Save picture to disk file";
            saveDialog.CreatePrompt = true;
            saveDialog.OverwritePrompt = true;
            saveDialog.ShowDialog();

            path = saveDialog.FileName;

            Extensions.Save(img, path);

            /*
            if (!File.Exists(path))
                File.Create(path).Dispose();
            */
        }
        #endregion UI events


        #region Drawing functions

        /// <summary>
        /// Draw the body skeleton.
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
        /// 
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
        private void DrawLine (Graphics g, CameraSpacePoint p1, CameraSpacePoint p2)
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

            if (point1.X>point2.X)
            {
                path.AddArc(pointColorShoulder.X - radius,
                    pointColorShoulder.Y - radius,
                    2 * radius,
                    2 * radius,
                    (float)angle1,
                    (float)(angle2-angle1));
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
                        
            g.FillPath(handOpenBrush, path);
            g.DrawPath(new Pen(Color.Green, 5), path);
            
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

            g.FillPath(handOpenBrush, path);
            g.DrawPath(new Pen(Color.Green, 5), path);

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



        #endregion Drawing functions

        
        private void frmKinectVideo_Resize(object sender, EventArgs e)
        {
            pctVideoColor.Height = this.Height - pctVideoColor.Location.Y - 52;
            pctVideoColor.Width = pctVideoColor.Height * 1920 / 1080;

            if (pctVideoColor.Height >= 300)
            {
                chartA.Location = new Point(13 + pctVideoColor.Width + 18, chartA.Location.Y);
                chartA.Width = (this.Width - 29) - chartA.Location.X;
                chartB.Location = new Point(13 + pctVideoColor.Width + 18, chartB.Location.Y);
                chartB.Width = (this.Width - 29) - chartB.Location.X;
            }
        }

        
    }

    public class TimePoint
    {
        public System.TimeSpan DateTime { get; set; }
        public double Value { get; set; }
    }

}
