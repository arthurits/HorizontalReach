using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.IO;
using System.Runtime.InteropServices;

namespace AlcancesMesaConversion
{
    public partial class Form1 : Form
    {
        // Variable definition
        //string fileInputPath = string.Empty;
        string filePathOpen = "c:\\";
        string filePathSave = "c:\\";
        string fileFilter = "txt files(*.txt)|*.txt|All files(*.*)|*.*";

        //StreamReader fileReader;
        //StreamWriter fileWriter;

        public Form1()
        {
            InitializeComponent();
        }

        private void cmdInput_Click(object sender, EventArgs e)
        {
            using (new CenterWinDialog(this))
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = filePathOpen;
                    openFileDialog.Filter = fileFilter;
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.Title = "Open file";
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        filePathOpen = openFileDialog.FileName;
                        txtInput.Text = filePathOpen;

                        //Read the contents of the file into a stream
                        //var file = openFileDialog.OpenFile();
                    }
                }
            }
        }

        private void cmdOutput_Click(object sender, EventArgs e)
        {
            using (new CenterWinDialog(this))
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.InitialDirectory = filePathSave;
                    saveFileDialog.Filter = fileFilter;
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.Title = "Save data to disk file";
                    saveFileDialog.CreatePrompt = false;
                    saveFileDialog.OverwritePrompt = true;
                    saveFileDialog.RestoreDirectory = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        filePathSave = saveFileDialog.FileName;
                        txtOutput.Text = filePathSave;
                    }
                }
            }
        }

        private void txtInput_Leave(object sender, EventArgs e)
        {
            filePathOpen = txtInput.Text;
        }

        private void txtOutput_Leave(object sender, EventArgs e)
        {
            filePathSave = txtOutput.Text;
        }

        private void cmdConvert_Click(object sender, EventArgs e)
        {
            // Local variables
            var conversion = true;
            var line = string.Empty;
            string[] dataSplit;
            Double number = 0.0;
            Double[] data = new Double[27];
            Double[] dataNew = new Double[27];


            // Create the file if it doesn't exist
            if (!File.Exists(filePathOpen))
            {
                MessageBox.Show(this, "Please select the input text file", "Select", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // Create the file if it doesn't exist    
            if (!File.Exists(filePathSave))
                File.Create(filePathSave).Dispose();

            // Get frontal tilt angle (around X axis)
            double radFrontal = 0.0;
            conversion = Double.TryParse(txtRotation.Text, out radFrontal);
            if (conversion) radFrontal *= Math.PI / 180.0;

            // Get lateral tilt angle (around Z axis)
            double radLateral = 0.0;
            conversion = Double.TryParse(this.txtRotationLateral.Text, out radLateral);
            if (conversion) radLateral *= Math.PI / 180.0;


            using (StreamReader fileReader = new StreamReader(filePathOpen))
            {
                using (StreamWriter fileWriter = new StreamWriter(filePathSave, false, Encoding.UTF8, 32768))
                {
                    while ((line = fileReader.ReadLine()) != null)
                    {
                        conversion = true;

                        line = line.TrimEnd('\t');
                        dataSplit = line.Split('\t');
                        for (Int32 i = 0; i < dataSplit.Length; i++)
                        {
                            conversion = Double.TryParse(dataSplit[i], out number);
                            data[i] = number;
                        }

                        if (conversion)
                        {
                            Calculate(data, dataNew, radFrontal, radLateral);
                            line = string.Join("\t", dataNew);
                        }

                        fileWriter.WriteLine(line);
                    }
                }
            }

            using (new CenterWinDialog(this))
            {
                MessageBox.Show(this,
                "The file\n" + filePathOpen + "\n" + "was correctly converted into file\n" + filePathSave, "Success!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            }
        }

        private void Calculate(Double[] data, Double[] dataNew, Double radFrontal, Double radLateral)
        {
            if (data.Length != 27) return;
            if (radFrontal == 0.0 && radLateral == 0.0) return;

            // Vector definitions
            Vector3D axisX;
            Vector3D axisY;
            Vector3D axisZ;
            Vector3D vector = new Vector3D(0.0, 0.0, 0.0);  // Intermediate vector

            // Do not modify the first 5 values
            //dataNew[0] = data[0];
            //dataNew[1] = data[1];
            //dataNew[2] = data[2];
            //dataNew[3] = data[3];
            //dataNew[4] = data[4];

            data.CopyTo(dataNew, 0);

            // Then do the rotation around the Z axis (the lateral tilt)
            if (radLateral != 0.0)
            {
                // Rotate coordinate axes around Z axis
                axisX = new Vector3D(Math.Cos(radLateral), Math.Sin(radLateral), 0.0);
                axisY = new Vector3D(-Math.Sin(radLateral), Math.Cos(radLateral), 0.0);
                axisZ = new Vector3D(0.0, 0.0, 1.0);

                //Project LeftShoulder coordinates into rotated axes
                vector = new Vector3D(dataNew[5], dataNew[6], dataNew[7]);
                dataNew[5] = Vector3D.DotProduct(vector, axisX);
                dataNew[6] = Vector3D.DotProduct(vector, axisY);
                dataNew[7] = Vector3D.DotProduct(vector, axisZ);

                //Project Shoulder coordinates into rotated axes
                vector = new Vector3D(dataNew[8], dataNew[9], dataNew[10]);
                dataNew[8] = Vector3D.DotProduct(vector, axisX);
                dataNew[9] = Vector3D.DotProduct(vector, axisY);
                dataNew[10] = Vector3D.DotProduct(vector, axisZ);

                //Project Elbow coordinates into rotated axes
                vector = new Vector3D(dataNew[11], dataNew[12], dataNew[13]);
                dataNew[11] = Vector3D.DotProduct(vector, axisX);
                dataNew[12] = Vector3D.DotProduct(vector, axisY);
                dataNew[13] = Vector3D.DotProduct(vector, axisZ);

                //Project Wrist coordinates into rotated axes
                vector = new Vector3D(dataNew[14], dataNew[15], dataNew[16]);
                dataNew[14] = Vector3D.DotProduct(vector, axisX);
                dataNew[15] = Vector3D.DotProduct(vector, axisY);
                dataNew[16] = Vector3D.DotProduct(vector, axisZ);

                //Project HandTip coordinates into rotated axes
                vector = new Vector3D(dataNew[17], dataNew[18], dataNew[19]);
                dataNew[17] = Vector3D.DotProduct(vector, axisX);
                dataNew[18] = Vector3D.DotProduct(vector, axisY);
                dataNew[19] = Vector3D.DotProduct(vector, axisZ);
            }

            // First do the rotation around the X axis (the frontal tilt)
            if (radFrontal != 0.0)
            {
                // Rotate coordinate axes around X axis
                axisX = new Vector3D(1.0, 0.0, 0.0);
                axisY = new Vector3D(0.0, Math.Cos(radFrontal), Math.Sin(radFrontal));
                axisZ = new Vector3D(0.0, -Math.Sin(radFrontal), Math.Cos(radFrontal));

                //Project LeftShoulder coordinates into rotated axes
                vector = new Vector3D(dataNew[5], dataNew[6], dataNew[7]);
                dataNew[5] = Vector3D.DotProduct(vector, axisX);
                dataNew[6] = Vector3D.DotProduct(vector, axisY);
                dataNew[7] = Vector3D.DotProduct(vector, axisZ);

                //Project Shoulder coordinates into rotated axes
                vector = new Vector3D(dataNew[8], dataNew[9], dataNew[10]);
                dataNew[8] = Vector3D.DotProduct(vector, axisX);
                dataNew[9] = Vector3D.DotProduct(vector, axisY);
                dataNew[10] = Vector3D.DotProduct(vector, axisZ);

                //Project Elbow coordinates into rotated axes
                vector = new Vector3D(dataNew[11], dataNew[12], dataNew[13]);
                dataNew[11] = Vector3D.DotProduct(vector, axisX);
                dataNew[12] = Vector3D.DotProduct(vector, axisY);
                dataNew[13] = Vector3D.DotProduct(vector, axisZ);

                //Project Wrist coordinates into rotated axes
                vector = new Vector3D(dataNew[14], dataNew[15], dataNew[16]);
                dataNew[14] = Vector3D.DotProduct(vector, axisX);
                dataNew[15] = Vector3D.DotProduct(vector, axisY);
                dataNew[16] = Vector3D.DotProduct(vector, axisZ);

                //Project HandTip coordinates into rotated axes
                vector = new Vector3D(dataNew[17], dataNew[18], dataNew[19]);
                dataNew[17] = Vector3D.DotProduct(vector, axisX);
                dataNew[18] = Vector3D.DotProduct(vector, axisY);
                dataNew[19] = Vector3D.DotProduct(vector, axisZ);
            }

            // Calculate a new coordinate base at the ShoulderRight joint
            Vector3D vShoulderX = new Vector3D(dataNew[8] - dataNew[5], dataNew[9] - dataNew[6], dataNew[10] - dataNew[7]);
            vShoulderX.Y = 0.0;
            vShoulderX.Normalize();

            Vector3D vShoulderZ = new Vector3D(0.0, 0.0, 0.0);
            vShoulderZ.X = vShoulderX.Z;
            vShoulderZ.Z = -vShoulderX.X;

            // Proyect ShoulderLeft into this new base
            vector = new Vector3D(dataNew[5] - dataNew[8], dataNew[6] - dataNew[9], dataNew[7] - dataNew[10]);
            dataNew[20] = Vector3D.DotProduct(vector, vShoulderX) * 100;
            dataNew[21] = Vector3D.DotProduct(vector, vShoulderZ) * 100;

            // Proyect Elbow into this new base
            vector = new Vector3D(dataNew[11] - dataNew[8], dataNew[12] - dataNew[9], dataNew[13] - dataNew[10]);
            dataNew[22] = Vector3D.DotProduct(vector, vShoulderX) * 100;
            dataNew[23] = Vector3D.DotProduct(vector, vShoulderZ) * 100;

            // Proyect HandTip into this new base
            vector = new Vector3D(dataNew[17] - dataNew[8], dataNew[18] - dataNew[9], dataNew[19] - dataNew[10]);
            dataNew[24] = Vector3D.DotProduct(vector, vShoulderX) * 100;
            dataNew[25] = Vector3D.DotProduct(vector, vShoulderZ) * 100;

            // Recalculate the shoulder elevation angle
            dataNew[1] = CalculateAngle(new Vector3D(dataNew[8], dataNew[9]-0.5, dataNew[10]), new Vector3D(dataNew[8], dataNew[9], dataNew[10]), new Vector3D(dataNew[11], dataNew[12], dataNew[13]));
            
            // Do not modify the last value (Obliquity angle)
            //dataNew[26] = data[26];
        }

        /// <summary>
        /// Calculates the angle of defined by three points. The angle is located at "point2"
        /// </summary>
        /// <param name="point1">Vector defining the first point</param>
        /// <param name="point2">Vector definig the point where the angle is located at</param>
        /// <param name="point3">Vector defining the end point</param>
        /// <param name="fullAngleRange">True if the returned angle range is 0-360</param>
        /// <returns>The angle defined by the points</returns>
        private double CalculateAngle(System.Windows.Media.Media3D.Vector3D point1, System.Windows.Media.Media3D.Vector3D point2, System.Windows.Media.Media3D.Vector3D point3, bool fullAngleRange = false)
        {
            System.Windows.Media.Media3D.Vector3D vector1 = point1 - point2;
            System.Windows.Media.Media3D.Vector3D vector2 = point3 - point2;

            double angle = System.Windows.Media.Media3D.Vector3D.AngleBetween(vector1, vector2);

            if (double.IsNaN(angle) || double.IsInfinity(angle))
                return 0.0;

            if (fullAngleRange == true)
            {
                if (System.Windows.Media.Media3D.Vector3D.CrossProduct(vector1, vector2).Z < 0.0)
                    angle = angle > 90 ? 360.0 - angle : -angle;
                //angle = 360.0 - angle;
            }

            return angle;

        }
    }

    /// <summary>
    /// Centers a dialog into its parent window
    /// </summary>
    /// https://stackoverflow.com/questions/2576156/winforms-how-can-i-make-messagebox-appear-centered-on-mainform
    /// https://stackoverflow.com/questions/1732443/center-messagebox-in-parent-form
    public class CenterWinDialog : IDisposable
    {
        private int mTries = 0;
        private Form mOwner;
        private Rectangle clientRect;

        public CenterWinDialog(Form owner)
        {
            mOwner = owner;
            clientRect = Screen.FromControl(owner).WorkingArea;

            if (owner.WindowState != FormWindowState.Minimized)
                owner.BeginInvoke(new MethodInvoker(findDialog));
        }

        private void findDialog()
        {
            // Enumerate windows to find the message box
            if (mTries < 0) return;
            EnumThreadWndProc callback = new EnumThreadWndProc(checkWindow);
            if (EnumThreadWindows(GetCurrentThreadId(), callback, IntPtr.Zero))
            {
                if (++mTries < 10) mOwner.BeginInvoke(new MethodInvoker(findDialog));
            }
        }
        private bool checkWindow(IntPtr hWnd, IntPtr lp)
        {
            // Checks if <hWnd> is a dialog
            StringBuilder sb = new StringBuilder(260);
            GetClassName(hWnd, sb, sb.Capacity);
            if (sb.ToString() != "#32770") return true;

            // Got it
            Rectangle frmRect = new Rectangle(mOwner.Location, mOwner.Size);
            RECT dlgRect;
            GetWindowRect(hWnd, out dlgRect);

            int x = frmRect.Left + (frmRect.Width - dlgRect.Right + dlgRect.Left) / 2;
            int y = frmRect.Top + (frmRect.Height - dlgRect.Bottom + dlgRect.Top) / 2;

            clientRect.Width -= (dlgRect.Right - dlgRect.Left);
            clientRect.Height -= (dlgRect.Bottom - dlgRect.Top);
            clientRect.X = x < clientRect.X ? clientRect.X : (x > clientRect.Right ? clientRect.Right : x);
            clientRect.Y = y < clientRect.Y ? clientRect.Y : (y > clientRect.Bottom ? clientRect.Bottom : y);

            MoveWindow(hWnd, clientRect.X, clientRect.Y, dlgRect.Right - dlgRect.Left, dlgRect.Bottom - dlgRect.Top, true);

            return false;
        }
        public void Dispose()
        {
            mTries = -1;
        }

        // P/Invoke declarations
        private delegate bool EnumThreadWndProc(IntPtr hWnd, IntPtr lp);
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern bool EnumThreadWindows(int tid, EnumThreadWndProc callback, IntPtr lp);
        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern int GetCurrentThreadId();
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder buffer, int buflen);
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT rc);
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool repaint);
        private struct RECT { public int Left; public int Top; public int Right; public int Bottom; }
    }

}
