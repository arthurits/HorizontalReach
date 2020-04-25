using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LiveCharts; //Core of the library
using LiveCharts.Wpf; //The WPF controls
using LiveCharts.WinForms; //The WinForm wrappers
using LiveCharts.Defaults;

namespace LiveChartsTest
{
    public partial class frmMain : Form
    {

        private Timer _timer = new Timer();
        private Random _r;
        private System.DateTime _startingTime;
        private Int32 _axisScale = 0;

        public frmMain()
        {
            InitializeComponent();

            // Initialize the Live Charts
            InitializeCharts();

            // Initialize timer
            _timer.Tick += new EventHandler(OnTimerTick);
        }

        private void OnTimerTick(object sender, EventArgs eventArgs)
        {
            // Time variables
            var now = DateTime.Now;
            var nowRelative = now - _startingTime;

            // Data points
            var angleShoulder = _r.Next(60, 90);
            var angleElbow = _r.Next(155, 175);
            var angleWrist = _r.Next(0, 10);

            // Add the points
            chartA.Series[0].Values.Add(new TimeP { DateTime = nowRelative, Value = angleShoulder });
            chartA.Series[1].Values.Add(new TimeP { DateTime = nowRelative, Value = angleElbow });
            chartA.Series[2].Values.Add(new TimeP { DateTime = nowRelative, Value = angleWrist });

            SetAxisLimits(now, 20);

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
                DisableAnimations = true
                //Separator = new Separator { Step = TimeSpan.FromSeconds(1).Ticks }
                //Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" }
            });
            this.chartA.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Angle",
                FontSize = 18,
                Foreground = System.Windows.Media.Brushes.Black,
                DisableAnimations = true
            });
            this.chartA.AxisX[0].Width = 12.0;
            //this.chartA.AxisY[0].MinValue = 0;
            this.chartA.AxisX[0].MaxValue = 10;
            this.chartA.AxisX[0].MinValue = 0;
            var mapper = LiveCharts.Configurations.Mappers.Xy<TimeP>()
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
                    Values = new ChartValues<TimeP> { },
                    PointGeometry = null,
                    LineSmoothness = 0,
                    //Stroke = System.Windows.Media.Brushes.Aqua,
                    Fill = System.Windows.Media.Brushes.Transparent

                },
                new LineSeries
                {
                    Title = "Elbow",
                    Values = new ChartValues<TimeP> { },
                    PointGeometry = null,
                    LineSmoothness = 0,
                    Fill = System.Windows.Media.Brushes.Transparent
                },
                new LineSeries
                {
                    Title = "Wrist",
                    Values = new ChartValues<TimeP> { },
                    PointGeometry = null,
                    LineSmoothness = 0,
                    Fill = System.Windows.Media.Brushes.Transparent
                }
            };
            SetAxisLimits(_startingTime, 20);
        }

        private void cmdTimer_Click(object sender, EventArgs e)
        {
            if (cmdTimer.Text == "&Start")
            {
                cmdTimer.Text = "&Stop";
                foreach (Series s in chartA.Series) s.Values.Clear();
                _r = new Random(DateTime.Now.Second);
                _startingTime = DateTime.Now;
                _timer.Interval = Convert.ToInt32(txtTimerInterval.Text);
                chartA.AnimationsSpeed = TimeSpan.FromMilliseconds(Convert.ToDouble(txtAnimationSpeed.Text));
                _timer.Start();
            }
            else if (cmdTimer.Text == "&Stop")
            {
                cmdTimer.Text = "&Start";
                _timer.Stop();
                chartA.AxisX[0].MinValue = 0;
                _axisScale = 0;
                _startingTime = DateTime.MinValue;
            }
        }

        private void SetAxisLimits(System.DateTime now, Int32 seconds)
        {
            long ticksElapsed = (now - _startingTime).Ticks / TimeSpan.TicksPerSecond;

            if (ticksElapsed <= 1)
            {
                _axisScale = 0;
                chartA.AxisX[0].MinValue = _axisScale * TimeSpan.FromSeconds(seconds).Ticks;
                chartA.AxisX[0].MaxValue = (_axisScale + 1) * TimeSpan.FromSeconds(seconds).Ticks;
            }

            if (ticksElapsed / seconds > _axisScale)
            {
                _axisScale = (Int32)ticksElapsed / seconds;
                chartA.AxisX[0].MinValue = _axisScale * TimeSpan.FromSeconds(seconds).Ticks;
                chartA.AxisX[0].MaxValue = (_axisScale + 1) * TimeSpan.FromSeconds(seconds).Ticks;

                //chartA.Series[0].Values.RemoveAt(p => p.DataTime <= _axisScale * seconds);
                //chartA.Series[0].Values.RemoveAt(TimePoint => TimePoint.DateTime <= _axisScale * seconds);
                Int32 nPoints = chartA.Series[0].Values.Count;
                TimeP nPoint;
                for (int i=0; i< chartA.Series.Count; i++)
                {
                    nPoint = (TimeP)chartA.Series[i].Values[nPoints - 1];
                    chartA.Series[i].Values.Clear();
                    chartA.Series[i].Values.Add(nPoint);
                }

            }

            
            // Let's only use the last 30 values
            /*
            if (chartA.Series[0].Values.Count > 30)
            {
                chartA.Series[0].Values.RemoveAt(0);
                chartA.Series[1].Values.RemoveAt(0);
                chartA.Series[2].Values.RemoveAt(0);
            }*/

            /*
            if (ticksElapsed >= 60.0)
            {
                chartA.AxisX[0].MaxValue = now.Ticks + TimeSpan.FromSeconds(1).Ticks - _startingTime.Ticks; // lets force the axis to be 1 s ahead
                chartA.AxisX[0].MinValue = now.Ticks - TimeSpan.FromSeconds(59).Ticks - _startingTime.Ticks; //we only care about the last 8 seconds
            }
            else
            {
                chartA.AxisX[0].MaxValue = TimeSpan.FromSeconds(60).Ticks;
            }
            */
        }

    }

    public class TimePoint
    {
        public System.TimeSpan DateTime { get; set; }
        public double Value { get; set; }
    }

    public struct TimeP
    {
        public System.TimeSpan DateTime { get; set; }
        public double Value { get; set; }
        public TimeP(TimeSpan dt, double val)
        {
            DateTime = dt;
            Value = val;
        }
    }
}
