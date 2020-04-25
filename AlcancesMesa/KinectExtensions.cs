using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
//using System.Windows.Controls;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

namespace KinectStreams
{
    public static class Extensions
    {

        /// <summary>
        /// Get an image object from the Kinect Sensor Color Frame object
        /// http://pterneas.com/2014/02/20/kinect-for-windows-version-2-color-depth-and-infrared-streams/
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static Image CreateImageFromFrame(ColorFrame frame)
        {
            //byte[] pixels = new byte[displayWidth * displayHeight * kinectSensor.ColorFrameSource.FrameDescription.BytesPerPixel];
            byte[] pixels = new byte[frame.FrameDescription.LengthInPixels * frame.FrameDescription.BytesPerPixel * 2];
            frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);

            //Image cImg = Image.FromStream(new System.IO.MemoryStream(pixels));
            //Image image;
            //using (var ms = new System.IO.MemoryStream(pixels))
            //{
                //image = (Bitmap) Image.FromStream(ms,false,false);
            //}

            var bitmap = new Bitmap(frame.FrameDescription.Width, frame.ColorFrameSource.FrameDescription.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, bitmap.PixelFormat);
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, bitmapData.Scan0, pixels.Length);
            bitmap.UnlockBits(bitmapData);
            //image = bitmap;

            return (Image)bitmap;
        }

        /// <summary>
        /// Get an image object from the Kinect Sensor Color Frame object
        /// http://pterneas.com/2014/02/20/kinect-for-windows-version-2-color-depth-and-infrared-streams/
        /// http://geekswithblogs.net/freestylecoding/archive/2014/08/20/processing-kinect-v2-color-streams-in-parallel.aspx
        /// https://social.msdn.microsoft.com/Forums/en-US/94de3145-28d7-4a53-8d34-cd946d5cd51b/colorframe-and-depthframe-tobitmap-functions?forum=kinectv2sdk
        /// kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static void CreateImageFromFrame(ColorFrame frame, Bitmap bitmap)
        {
            //byte[] pixels = new byte[displayWidth * displayHeight * kinectSensor.ColorFrameSource.FrameDescription.BytesPerPixel];
            byte[] pixels = new byte[frame.FrameDescription.LengthInPixels * frame.FrameDescription.BytesPerPixel * 2]; // Yuy2 uses 4 bytes per 2 pixel, for a buffer size of 4,147,200 bytes. Bgra32 uses 4 bytes per pixel, for a buffer size of 8,294,400 bytes
            frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);

            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, bitmap.PixelFormat);
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, bitmapData.Scan0, pixels.Length);
            bitmap.UnlockBits(bitmapData);

            return;
        }

        /// <summary>
        /// Get an image object from the Kinect Sensor Color Frame object
        /// </summary>
        /// <param name="frame">The Kinect color frame object (in Yuy2 color format)</param>
        /// <param name="bitmap">The bitmap object where the frame will be written (in Bgra32 format)</param>
        /// <param name="pixels">The byte array intermediate</param>
        public static Image CreateImageFromFrame(ColorFrame frame, Bitmap bitmap, byte[] pixels)
        {
            //byte[] pixels = new byte[displayWidth * displayHeight * kinectSensor.ColorFrameSource.FrameDescription.BytesPerPixel];
            //byte[] pixels = new byte[frame.FrameDescription.LengthInPixels * frame.FrameDescription.BytesPerPixel * 2]; // Yuy2 uses 4 bytes per 2 pixel, for a buffer size of 4,147,200 bytes. Bgra32 uses 4 bytes per pixel, for a buffer size of 8,294,400 bytes
            frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);

            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, bitmap.PixelFormat);
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, bitmapData.Scan0, pixels.Length);
            bitmap.UnlockBits(bitmapData);

            return (Image)bitmap;
        }



        /// <summary>
        /// Returns the first tracked body.
        /// </summary>
        /// <param name="frame">The Body frame.</param>
        /// <returns>The first tracked body.</returns>
        /// https://github.com/LightBuzz/Kinect-Floor
        public static Body Body(this BodyFrame frame)
        {
            if (frame == null) return null;

            Body[] _bodyData = new Body[6];
            frame.GetAndRefreshBodyData(_bodyData);
            return _bodyData.Where(b => b != null && b.IsTracked).FirstOrDefault();
        }
        public static String GetString(this CameraSpacePoint p)
        {
            return "X:" + p.X.ToString() + " Y:" + p.Y.ToString() + " Z:" + p.Z.ToString();
        }
        public static String ToString(this CameraSpacePoint p)
        {
            return "X:" + p.X.ToString() + " Y:" + p.Y.ToString() + " Z:" + p.Z.ToString();
        }
        /// <summary>
        /// Method to estimate the total body height in meters
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static double BodyHeight(this Body body)
        {
            // Constant divergence
            const double HEAD_DIVERGENCE = 0.1;

            // Define joint variables
            var head = body.Joints[JointType.Head];
            var neck = body.Joints[JointType.Neck];
            var shoulders = body.Joints[JointType.SpineShoulder];
            var spine = body.Joints[JointType.SpineMid];
            var waist = body.Joints[JointType.SpineBase];
            var hipLeft = body.Joints[JointType.HipLeft];
            var hipRight = body.Joints[JointType.HipRight];
            var kneeLeft = body.Joints[JointType.KneeLeft];
            var kneeRight = body.Joints[JointType.KneeRight];
            var ankleLeft = body.Joints[JointType.AnkleLeft];
            var ankleRight = body.Joints[JointType.AnkleRight];
            var footLeft = body.Joints[JointType.FootLeft];
            var footRight = body.Joints[JointType.FootRight];

            // Find which leg is tracked more accurately.
            int legLeftTrackedJoints = NumberOfTrackedJoints(hipLeft, kneeLeft, ankleLeft, footLeft);
            int legRightTrackedJoints = NumberOfTrackedJoints(hipRight, kneeRight, ankleRight, footRight);

            double legLength = legLeftTrackedJoints > legRightTrackedJoints ?
                Length(hipLeft.Position, kneeLeft.Position, ankleLeft.Position, footLeft.Position) :
                Length(hipRight.Position, kneeRight.Position, ankleRight.Position, footRight.Position);

            return Length(head.Position, neck.Position, shoulders.Position, spine.Position, waist.Position) + legLength + HEAD_DIVERGENCE;
        }

        /// <summary>
        /// Calculates the number of the tracked joints from the current collection.
        /// </summary>
        /// <param name="joints">A collection of joints.</param>
        /// <returns>The number of the accurately tracked joints.</returns>
        static int NumberOfTrackedJoints(IEnumerable<Joint> joints)
        {
            int trackedJoints = 0;

            foreach (var joint in joints)
            {
                if (joint.TrackingState == TrackingState.Tracked)
                {
                    trackedJoints++;
                }
            }

            return trackedJoints;
        }

        /// <summary>
        /// Calculates the number of the tracked joints from the specified collection.
        /// </summary>
        /// <param name="joints">A collection of joints.</param>
        /// <returns>The number of the accurately tracked joints.</returns>
        static int NumberOfTrackedJoints(params Joint[] joints)
        {
            return NumberOfTrackedJoints(joints.ToList());
        }

        /// <summary>
        /// Calculates the length of the specified 3-D point.
        /// </summary>
        /// <param name="point">The specified 3-D point.</param>
        /// <returns>The corresponding length, in meters.</returns>
        public static double Length(this CameraSpacePoint point)
        {
            return Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Y, 2) + Math.Pow(point.Z, 2));
        }

        /// <summary>
        /// Returns the length of the segment defined by the specified points.
        /// </summary>
        /// <param name="point1">The first point (start of the segment).</param>
        /// <param name="point2">The second point (end of the segment).</param>
        /// <returns>The length of the segment (in meters).</returns>
        public static double Length(this CameraSpacePoint point1, CameraSpacePoint point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2) + Math.Pow(point1.Z - point2.Z, 2));
        }

        /// <summary>
        /// Returns the length of the segments defined by the specified points.
        /// </summary>
        /// <param name="points">A collection of two or more points.</param>
        /// <returns>The length of all the segments in meters.</returns>
        public static double Length(params CameraSpacePoint[] points)
        {
            double length = 0;
            
            for (int index = 0; index < points.Length - 1; index++)
            {
                length += Length(points[index], points[index + 1]);
            }

            return length;
        }

        /// <summary>
        ///  Calculates the angle between the specified points.
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point (vertex of the angle).</param>
        /// <param name="p3">The third point.</param>
        /// <param name="fullAngleRange">If true, returns the angle between 0 and 360.</param>
        /// <returns>The angle in degrees (range 0 - 180).</returns>
        public static double CalculateAngle(CameraSpacePoint p1, CameraSpacePoint p2, CameraSpacePoint p3, bool fullAngleRange = false)
        {
            System.Windows.Media.Media3D.Vector3D vector1 = p1.ToVector3() - p2.ToVector3();
            System.Windows.Media.Media3D.Vector3D vector2 = p3.ToVector3() - p2.ToVector3();

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

        /// <summary>
        /// Calculates the angle between the specified points around the specified axis.
        /// </summary>
        /// <param name="start">The start of the angle.</param>
        /// <param name="center">The center of the angle.</param>
        /// <param name="end">The end of the angle.</param>
        /// <param name="axis">The axis (x, y, z) around which the angle is calculated.</param>
        /// <returns>The angle in degrees (range 0 - 180).</returns>
        public static double CalculateAngle(CameraSpacePoint start, CameraSpacePoint center, CameraSpacePoint end, Axis axis)
        {

            switch (axis)
            {
                case Axis.X:
                    start.X = 0f;
                    center.X = 0f;
                    end.X = 0f;
                    break;
                case Axis.Y:
                    start.Y = 0f;
                    center.Y = 0f;
                    end.Y = 0f;
                    break;
                case Axis.Z:
                    start.Z = 0f;
                    center.Z = 0f;
                    end.Z = 0f;
                    break;
            }

            return CalculateAngle(start, center, end);

            /*
            System.Windows.Media.Media3D.Vector3D first = start.ToVector3() - center.ToVector3();
            System.Windows.Media.Media3D.Vector3D second = end.ToVector3() - center.ToVector3();

            return System.Windows.Media.Media3D.Vector3D.AngleBetween(first, second);
            */
        }

        /// <summary>
        /// Normalise a vector with the origin on the coordinate axes.
        /// </summary>
        /// <param name="p">Point in 3 dimensions</param>
        /// <returns></returns>
        public static CameraSpacePoint Normalise(CameraSpacePoint p)
        {
            CameraSpacePoint unitP = new CameraSpacePoint();

            double length = Math.Sqrt(p.X * p.X + p.Y * p.Y + p.Z * p.Z);
            float num = 0.0f;

            if (length != 0.0)
            {
                num = (float)(1.0 / length);
                unitP.X = p.X * num;
                unitP.Y = p.Y * num;
                unitP.Z = p.Z * num;
            }

            return unitP;   
        }
        /// <summary>
        /// Normalise a vector with the origin on the coordinate axes.
        /// </summary>
        /// <param name="v">Vector in 3 dimensions</param>
        /// <returns></returns>
        public static System.Windows.Media.Media3D.Vector3D Normalise(System.Windows.Media.Media3D.Vector3D v)
        {
            System.Windows.Media.Media3D.Vector3D unitV = new System.Windows.Media.Media3D.Vector3D();

            double length = Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
            float num = 0.0f;

            if (length != 0.0)
            {
                num = (float)(1.0 / length);
                unitV.X = v.X * num;
                unitV.Y = v.Y * num;
                unitV.Z = v.Z * num;
            }

            return unitV;
        }

        /// <summary>
        /// Rotation of a point in 3 dimensional space by theta about an arbitrary axes defined by a line between two points P1 = (x1,y1,z1) and P2 = (x2,y2,z2)
        /// http://paulbourke.net/geometry/rotate/
        /// http://inside.mines.edu/fs_home/gmurray/ArbitraryAxisRotation/
        /// </summary>
        /// <param name="p">Point to be rotated</param>
        /// <param name="theta">Angle in radians</param>
        /// <param name="p1">First point of rotation line</param>
        /// <param name="p2">Second point of rotation line</param>
        /// <returns></returns>
        public static CameraSpacePoint RotatePointAboutLine(CameraSpacePoint p, double theta, CameraSpacePoint p1, CameraSpacePoint p2)
        {
            CameraSpacePoint u = new CameraSpacePoint();
            CameraSpacePoint q1 = new CameraSpacePoint();
            CameraSpacePoint q2 = new CameraSpacePoint();

            double d;

            /* Step 1 */
            q1.X = p.X - p1.X;
            q1.Y = p.Y - p1.Y;
            q1.Z = p.Z - p1.Z;

            u.X = p2.X - p1.X;
            u.Y = p2.Y - p1.Y;
            u.Z = p2.Z - p1.Z;
            u = Normalise(u);
            d = Math.Sqrt(u.Y * u.Y + u.Z * u.Z);

            /* Step 2 */
            if (d != 0.0)
            {
                q2.X = q1.X;
                q2.Y = (float)(q1.Y * u.Z / d - q1.Z * u.Y / d);
                q2.Z = (float)(q1.Y * u.Y / d + q1.Z * u.Z / d);
            }
            else
            {
                q2 = q1;
            }

            /* Step 3 */
            q1.X = (float)(q2.X * d - q2.Z * u.X);
            q1.Y = q2.Y;
            q1.Z = (float)(q2.X * u.X + q2.Z * d);

            /* Step 4 */
            q2.X = (float)(q1.X * Math.Cos(theta) - q1.Y * Math.Sin(theta));
            q2.Y = (float)(q1.X * Math.Sin(theta) + q1.Y * Math.Cos(theta));
            q2.Z = q1.Z;

            /* Inverse of step 3 */
            q1.X = (float)(q2.X * d + q2.Z * u.X);
            q1.Y = q2.Y;
            q1.Z = (float)(-q2.X * u.X + q2.Z * d);

            /* Inverse of step 2 */
            if (d != 0)
            {
                q2.X = q1.X;
                q2.Y = (float)(q1.Y * u.Z / d + q1.Z * u.Y / d);
                q2.Z = (float)(-q1.Y * u.Y / d + q1.Z * u.Z / d);
            }
            else
            {
                q2 = q1;
            }

            /* Inverse of step 1 */
            q1.X = q2.X + p1.X;
            q1.Y = q2.Y + p1.Y;
            q1.Z = q2.Z + p1.Z;

            return (q1);
        }


        #region ToVector

        /// <summary>
        /// Converts the specified CameraSpacePoint into a 3-D vector structure.
        /// </summary>
        /// <param name="point">The CameraSpacePoint to convert</param>
        /// <returns>The corresponding 3-D vector.</returns>
        public static System.Windows.Media.Media3D.Vector3D ToVector3(this CameraSpacePoint point)
        {
            return new System.Windows.Media.Media3D.Vector3D
            {
                X = point.X,
                Y = point.Y,
                Z = point.Z
            };
        }
        
        /// <summary>
        /// Converts the specified ColorSpacePoint into a 3-D vector structure.
        /// </summary>
        /// <param name="point">The ColorSpacePoint to convert</param>
        /// <returns>The corresponding 3-D vector.</returns>
        public static System.Windows.Media.Media3D.Vector3D ToVector3(this ColorSpacePoint point)
        {
            return new System.Windows.Media.Media3D.Vector3D
            {
                X = point.X,
                Y = point.Y,
                Z = 0.0
            };
        }
        
        /// <summary>
        /// Converts the specified DepthSpacePoint into a 3-D vector structure.
        /// </summary>
        /// <param name="point">The DepthSpacePoint to convert</param>
        /// <returns>The corresponding 3-D vector.</returns>
        public static System.Windows.Media.Media3D.Vector3D ToVector3(this DepthSpacePoint point)
        {
            return new System.Windows.Media.Media3D.Vector3D
            {
                X = point.X,
                Y = point.Y,
                Z = 0.0
            };
        }
        
        /// <summary>
        /// Converts the specified 2-D point into a 3-D vector structure.
        /// </summary>
        /// <param name="point">The point to convert</param>
        /// <returns>The corresponding 3-D vector.</returns>
        public static System.Windows.Media.Media3D.Vector3D ToVector3(this Point point)
        {
            return new System.Windows.Media.Media3D.Vector3D
            {
                X = point.X,
                Y = point.Y,
                Z = 0.0
            };
        }

        #endregion

        /// <summary>
        /// Converts a Vector3D struct into a CameraSpacePoint struct
        /// </summary>
        /// <param name="vector">Vector3D struct</param>
        /// <returns>Returns a CameraSpacePointStruct</returns>
        public static CameraSpacePoint ToCameraSpace(System.Windows.Media.Media3D.Vector3D vector)
        {
            return new CameraSpacePoint { X = (float)vector.X, Y = (float)vector.Y, Z = (float)vector.Z };
        }

        /// <summary>
        /// Returns the body that is currently closest to the sensor.
        /// </summary>
        /// <param name="bodies">A list of bodies to look at</param>
        /// <returns>The closest to the camera tracked body</returns>
        public static Body FirstOrDefault(this IEnumerable<Body> bodies)
        {
            Body result = null;

            if (bodies == null) return null;

            result = bodies.Where(b => b != null && b.IsTracked).FirstOrDefault();
            
            /*
            foreach (var body in bodies)
            {
                if (body.IsTracked)
                {
                        result = body;
                        break;
                }
            }
            */

            return result;
        }

        /// <summary>
        /// Returns the body that is currently closest to the sensor.
        /// </summary>
        /// <param name="bodies">A list of bodies to look at</param>
        /// <returns>The closest to the camera tracked body</returns>
        public static Body First(this IEnumerable<Body> bodies)
        {
            Body result = null;

            if (bodies == null) return null;            

            foreach (var body in bodies)
            {
                if (body.IsTracked)
                {
                        result = body;
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the body that is currently closest to the sensor.
        /// </summary>
        /// <param name="bodies">A list of bodies to look at</param>
        /// <returns>The closest to the camera tracked body</returns>
        public static Body Closest(this IEnumerable<Body> bodies)
        {
            Body result = null;
            double closestBodyDistance = double.MaxValue;

            //if (bodies == null) return null;

            foreach (var body in bodies)
            {
                if (body.IsTracked)
                {
                    var position = body.Joints[JointType.SpineBase].Position;
                    var distance = position.Length();

                    if (result == null || distance < closestBodyDistance)
                    {
                        result = body;
                        closestBodyDistance = distance;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Captures the specified image source and saves it to the specified location.
        /// </summary>
        /// <param name="bitmap">The ImageSouce to capture.</param>
        /// <param name="path">The desired file path, including file name and extension, for the new image. Currently, JPEG and PNG formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static bool Save(System.Windows.Media.ImageSource bitmap, string path)
        {
            if (bitmap == null || path == null || string.IsNullOrWhiteSpace(path)) return false;

            try
            {
                System.Windows.Media.Imaging.BitmapEncoder encoder;

                switch (System.IO.Path.GetExtension(path))
                {
                    case ".jpg":
                    case ".JPG":
                    case ".jpeg":
                    case ".JPEG":
                        encoder = new System.Windows.Media.Imaging.JpegBitmapEncoder();
                        break;
                    case ".png":
                    case ".PNG":
                        encoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
                        break;
                    case ".bmp":
                    case ".BMP":
                        encoder = new System.Windows.Media.Imaging.BmpBitmapEncoder();
                        break;
                    default:
                        return false;
                }

                encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bitmap as System.Windows.Media.Imaging.BitmapSource));

                using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Create))
                {
                    encoder.Save(stream);
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return false;

        }

        /// <summary>
        /// Captures the specified image source and saves it to the specified location.
        /// </summary>
        /// <param name="bitmap">The ImageSouce to capture.</param>
        /// <param name="path">The desired file path, including file name and extension, for the new image. Currently, JPEG and PNG formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static bool Save(Image bitmap, string path)
        {
            if (bitmap == null || path == null || string.IsNullOrWhiteSpace(path)) return false;

            try
            {
                System.Drawing.Imaging.ImageFormat encoder = System.Drawing.Imaging.ImageFormat.Png;

                switch (System.IO.Path.GetExtension(path))
                {
                    case ".jpg":
                    case ".JPG":
                    case ".jpeg":
                    case ".JPEG":
                        encoder = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                    case ".png":
                    case ".PNG":
                        encoder = System.Drawing.Imaging.ImageFormat.Png;
                        break;
                    case ".bmp":
                    case ".BMP":
                        encoder = System.Drawing.Imaging.ImageFormat.Bmp;
                        break;
                    case ".wmf":
                    case ".WMF":
                        encoder = System.Drawing.Imaging.ImageFormat.Wmf;
                        break;
                    case ".tiff":
                    case ".TIFF":
                        encoder = System.Drawing.Imaging.ImageFormat.Tiff;
                        break;
                    default:
                        return false;
                }

                using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Create))
                {
                    bitmap.Save(stream, encoder);
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return false;
        }


            /*
            #region Camera

            public static ImageSource ToBitmap(this ColorFrame frame)
            {
                int width = frame.FrameDescription.Width;
                int height = frame.FrameDescription.Height;
                PixelFormat format = PixelFormats.Bgr32;

                byte[] pixels = new byte[width * height * ((format.BitsPerPixel + 7) / 8)];

                if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
                {
                    frame.CopyRawFrameDataToArray(pixels);
                }
                else
                {
                    frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
                }

                int stride = width * format.BitsPerPixel / 8;

                return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
            }

            public static ImageSource ToBitmap(this DepthFrame frame)
            {
                int width = frame.FrameDescription.Width;
                int height = frame.FrameDescription.Height;
                PixelFormat format = PixelFormats.Bgr32;

                ushort minDepth = frame.DepthMinReliableDistance;
                ushort maxDepth = frame.DepthMaxReliableDistance;

                ushort[] pixelData = new ushort[width * height];
                byte[] pixels = new byte[width * height * (format.BitsPerPixel + 7) / 8];

                frame.CopyFrameDataToArray(pixelData);

                int colorIndex = 0;
                for (int depthIndex = 0; depthIndex < pixelData.Length; ++depthIndex)
                {
                    ushort depth = pixelData[depthIndex];

                    byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                    pixels[colorIndex++] = intensity; // Blue
                    pixels[colorIndex++] = intensity; // Green
                    pixels[colorIndex++] = intensity; // Red

                    ++colorIndex;
                }

                int stride = width * format.BitsPerPixel / 8;

                return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
            }

            public static ImageSource ToBitmap(this InfraredFrame frame)
            {
                int width = frame.FrameDescription.Width;
                int height = frame.FrameDescription.Height;
                PixelFormat format = PixelFormats.Bgr32;

                ushort[] frameData = new ushort[width * height];
                byte[] pixels = new byte[width * height * (format.BitsPerPixel + 7) / 8];

                frame.CopyFrameDataToArray(frameData);

                int colorIndex = 0;
                for (int infraredIndex = 0; infraredIndex < frameData.Length; infraredIndex++)
                {
                    ushort ir = frameData[infraredIndex];

                    byte intensity = (byte)(ir >> 7);

                    pixels[colorIndex++] = (byte)(intensity / 1); // Blue
                    pixels[colorIndex++] = (byte)(intensity / 1); // Green   
                    pixels[colorIndex++] = (byte)(intensity / 0.4); // Red

                    colorIndex++;
                }

                int stride = width * format.BitsPerPixel / 8;

                return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
            }

            #endregion

            #region Body

            public static Joint ScaleTo(this Joint joint, double width, double height, float skeletonMaxX, float skeletonMaxY)
            {
                joint.Position = new CameraSpacePoint
                {
                    X = Scale(width, skeletonMaxX, joint.Position.X),
                    Y = Scale(height, skeletonMaxY, -joint.Position.Y),
                    Z = joint.Position.Z
                };

                return joint;
            }

            public static Joint ScaleTo(this Joint joint, double width, double height)
            {
                return ScaleTo(joint, width, height, 1.0f, 1.0f);
            }

            private static float Scale(double maxPixel, double maxSkeleton, float position)
            {
                float value = (float)((((maxPixel / maxSkeleton) / 2) * position) + (maxPixel / 2));

                if (value > maxPixel)
                {
                    return (float)maxPixel;
                }

                if (value < 0)
                {
                    return 0;
                }

                return value;
            }

            #endregion

            #region Drawing

            public static void DrawSkeleton(this Canvas canvas, Body body)
            {
                if (body == null) return;

                foreach (Joint joint in body.Joints.Values)
                {
                    canvas.DrawPoint(joint);
                }

                canvas.DrawLine(body.Joints[JointType.Head], body.Joints[JointType.Neck]);
                canvas.DrawLine(body.Joints[JointType.Neck], body.Joints[JointType.SpineShoulder]);
                canvas.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderLeft]);
                canvas.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderRight]);
                canvas.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.SpineMid]);
                canvas.DrawLine(body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft]);
                canvas.DrawLine(body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight]);
                canvas.DrawLine(body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft]);
                canvas.DrawLine(body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight]);
                canvas.DrawLine(body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft]);
                canvas.DrawLine(body.Joints[JointType.WristRight], body.Joints[JointType.HandRight]);
                canvas.DrawLine(body.Joints[JointType.HandLeft], body.Joints[JointType.HandTipLeft]);
                canvas.DrawLine(body.Joints[JointType.HandRight], body.Joints[JointType.HandTipRight]);
                canvas.DrawLine(body.Joints[JointType.HandTipLeft], body.Joints[JointType.ThumbLeft]);
                canvas.DrawLine(body.Joints[JointType.HandTipRight], body.Joints[JointType.ThumbRight]);
                canvas.DrawLine(body.Joints[JointType.SpineMid], body.Joints[JointType.SpineBase]);
                canvas.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipLeft]);
                canvas.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipRight]);
                canvas.DrawLine(body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft]);
                canvas.DrawLine(body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight]);
                canvas.DrawLine(body.Joints[JointType.KneeLeft], body.Joints[JointType.AnkleLeft]);
                canvas.DrawLine(body.Joints[JointType.KneeRight], body.Joints[JointType.AnkleRight]);
                canvas.DrawLine(body.Joints[JointType.AnkleLeft], body.Joints[JointType.FootLeft]);
                canvas.DrawLine(body.Joints[JointType.AnkleRight], body.Joints[JointType.FootRight]);
            }

            public static void DrawPoint(this Canvas canvas, Joint joint)
            {
                if (joint.TrackingState == TrackingState.NotTracked) return;

                joint = joint.ScaleTo(canvas.ActualWidth, canvas.ActualHeight);

                Ellipse ellipse = new Ellipse
                {
                    Width = 20,
                    Height = 20,
                    Fill = new SolidColorBrush(Colors.LightBlue)
                };

                Canvas.SetLeft(ellipse, joint.Position.X - ellipse.Width / 2);
                Canvas.SetTop(ellipse, joint.Position.Y - ellipse.Height / 2);

                canvas.Children.Add(ellipse);
            }

            public static void DrawLine(this Canvas canvas, Joint first, Joint second)
            {
                if (first.TrackingState == TrackingState.NotTracked || second.TrackingState == TrackingState.NotTracked) return;

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
                };

                canvas.Children.Add(line);
            }

            #endregion
    */
    }

    /// <summary>
    /// Represents an axis from the 3D space.
    /// </summary>
    public enum Axis
    {
        /// <summary>
        /// The X axis.
        /// </summary>
        X = 0,
        
        /// <summary>
        /// The Y axis.
        /// </summary>
        Y = 1,
        
        /// <summary>
        /// The Z axis.
        /// </summary>
        Z = 2
    }

    /// <summary>
    /// Wrapper Stream Class to Support 32->16bit conversion and support Speech call to Seek
    /// https://github.com/zubairdotnet/KinectSpeechColorWPF
    /// </summary>
    internal class KinectAudioStream : Stream
    {
        /// <summary>
        /// Holds the kinect audio stream, in 32-bit IEEE float format
        /// </summary>
        private Stream kinect32BitStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="KinectAudioStream" /> class.
        /// </summary>
        /// <param name="input">Kinect audio stream</param>
        public KinectAudioStream(Stream input)
        {
            this.kinect32BitStream = input;
        }

        /// <summary>
        /// Gets or sets a value indicating whether speech recognition is active
        /// </summary>
        public bool SpeechActive { get; set; }

        /// <summary>
        /// CanRead property
        /// </summary>
        public override bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        /// CanWrite property
        /// </summary>
        public override bool CanWrite
        {
            get { return false; }
        }

        /// <summary>
        /// CanSeek property
        /// </summary>
        public override bool CanSeek
        {
            // Speech does not call - but set value correctly
            get { return false; }
        }

        /// <summary>
        /// Position Property
        /// </summary>
        public override long Position
        {
            // Speech gets the position
            get { return 0; }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the length of the stream. Not implemented.
        /// </summary>
        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Flush the stream. Not implemented.
        /// </summary>
        public override void Flush()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stream Seek. Not implemented and always returns 0.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter</param>
        /// <param name="origin">A value of type SeekOrigin indicating the reference point used to obtain the new position</param>
        /// <returns>Always returns 0</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            // Even though CanSeek == false, Speech still calls seek. Return 0 to make Speech happy instead of NotImplementedException()
            return 0;
        }

        /// <summary>
        /// Set the length of the stream. Not implemented.
        /// </summary>
        /// <param name="value">Length of the stream</param>
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Write into the stream. Not implemented.
        /// </summary>
        /// <param name="buffer">Buffer to write</param>
        /// <param name="offset">Offset into the buffer</param>
        /// <param name="count">Number of bytes to write</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Read from the stream and convert from 32 bit IEEE float to 16 bit signed integer
        /// </summary>
        /// <param name="buffer">Input buffer</param>
        /// <param name="offset">Offset into buffer</param>
        /// <param name="count">Number of bytes to read</param>
        /// <returns>bytes read</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            // Kinect gives 32-bit float samples. Speech asks for 16-bit integer samples.
            const int SampleSizeRatio = sizeof(float) / sizeof(short); // = 2. 

            // Speech reads at high frequency - allow some wait period between reads (in msec)
            const int SleepDuration = 50;

            // Allocate buffer for receiving 32-bit float from Kinect
            int readcount = count * SampleSizeRatio;
            byte[] kinectBuffer = new byte[readcount];

            int bytesremaining = readcount;

            // Speech expects all requested bytes to be returned
            while (bytesremaining > 0)
            {
                // If we are no longer processing speech commands, exit
                if (!this.SpeechActive)
                {
                    return 0;
                }

                int result = this.kinect32BitStream.Read(kinectBuffer, readcount - bytesremaining, bytesremaining);
                bytesremaining -= result;

                // Speech will read faster than realtime - wait for more data to arrive
                if (bytesremaining > 0)
                {
                    System.Threading.Thread.Sleep(SleepDuration);
                }
            }

            // Convert each float audio sample to short
            for (int i = 0; i < count / sizeof(short); i++)
            {
                // Extract a single 32-bit IEEE value from the byte array
                float sample = BitConverter.ToSingle(kinectBuffer, i * sizeof(float));

                // Make sure it is in the range [-1, +1]
                if (sample > 1.0f)
                {
                    sample = 1.0f;
                }
                else if (sample < -1.0f)
                {
                    sample = -1.0f;
                }

                // Scale float to the range (short.MinValue, short.MaxValue] and then 
                // convert to 16-bit signed with proper rounding
                short convertedSample = Convert.ToInt16(sample * short.MaxValue);

                // Place the resulting 16-bit sample in the output byte array
                byte[] local = BitConverter.GetBytes(convertedSample);
                System.Buffer.BlockCopy(local, 0, buffer, offset + (i * sizeof(short)), sizeof(short));
            }

            return count;
        }
    }

    public struct Vector3
    {
        /// <summary>A vector with the minimum double values.</summary>
        public static readonly Vector3 MinValue = new Vector3(double.MinValue, double.MinValue, double.MinValue);
        /// <summary>A vector with the maximum double values.</summary>
        public static readonly Vector3 MaxValue = new Vector3(double.MaxValue, double.MaxValue, double.MaxValue);
        /// <summary>A vector with Epsilon values.</summary>
        public static readonly Vector3 Epsilon = new Vector3(double.Epsilon, double.Epsilon, double.Epsilon);
        /// <summary>A vector with zero values.</summary>
        public static readonly Vector3 Zero = new Vector3(0.0, 0.0, 0.0);
        /// <summary>Gets or sets the X component of this vector.</summary>
        public double X;
        /// <summary>Gets or sets the Y component of this vector.</summary>
        public double Y;
        /// <summary>Gets or sets the Z component of this vector.</summary>
        public double Z;

        /// <summary>Gets the length (or magnitude) of this vector.</summary>
        public double Length
        {
            get
            {
                return Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
            }
        }

        /// <summary>Gets the square of the length of this vector.</summary>
        public double LengthSquared
        {
            get
            {
                return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
            }
        }

        /// <summary>
        /// Creates a new instance of Vector3 with the specified initial values.
        /// </summary>
        /// <param name="x">Value of the X coordinate of the new vector.</param>
        /// <param name="y">Value of the Y coordinate of the new vector.</param>
        /// <param name="z">Value of the Z coordinate of the new vector.</param>
        public Vector3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>Adds two vectors and returns the result as a vector.</summary>
        /// <param name="vector1">The first vector to add.</param>
        /// <param name="vector2">The second vector to add.</param>
        /// <returns>The sum of vector1 and vector2.</returns>
        public static Vector3 operator +(Vector3 vector1, Vector3 vector2)
        {
            return Vector3.Add(vector1, vector2);
        }

        /// <summary>Subtracts one specified vector from another.</summary>
        /// <param name="vector1">The vector from which vector2 is subtracted.</param>
        /// <param name="vector2">The vector to subtract from vector1.</param>
        /// <returns>The difference between vector1 and vector2.</returns>
        public static Vector3 operator -(Vector3 vector1, Vector3 vector2)
        {
            return Vector3.Subtract(vector1, vector2);
        }

        /// <summary>Operator -Vector (unary negation).</summary>
        /// <param name="vector">Vector being negated.</param>
        /// <returns>Negation of the given vector.</returns>
        public static Vector3 operator -(Vector3 vector)
        {
            return new Vector3(-vector.X, -vector.Y, -vector.Z);
        }

        /// <summary>
        /// Multiplies the specified scalar by the specified vector and returns the resulting vector.
        /// </summary>
        /// <param name="scalar">The scalar to multiply.</param>
        /// <param name="vector">The vector to multiply.</param>
        /// <returns>The result of multiplying scalar and vector.</returns>
        public static Vector3 operator *(double scalar, Vector3 vector)
        {
            return Vector3.Multiply(scalar, vector);
        }

        /// <summary>
        /// Divides the specified vector by the specified scalar and returns the resulting vector.
        /// </summary>
        /// <param name="vector">The vector to divide.</param>
        /// <param name="scalar">The scalar by which vector will be divided.</param>
        /// <returns>The result of dividing vector by scalar.</returns>
        public static Vector3 operator /(Vector3 vector, double scalar)
        {
            return Vector3.Divide(vector, scalar);
        }

        /// <summary>Compares two vectors for equality.</summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>True if the X and Y components of vector1 and vector2 are equal; otherwise, false.</returns>
        public static bool operator ==(Vector3 vector1, Vector3 vector2)
        {
            return vector1.Equals(vector2);
        }

        /// <summary>Compares two vectors for inequality.</summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>True if the X and Y components of vector1 and vector2 are different; otherwise, false.</returns>
        public static bool operator !=(Vector3 vector1, Vector3 vector2)
        {
            return !vector1.Equals(vector2);
        }

        /// <summary>Compares the lengths of two vectors.</summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>True if the length of vector1 is smaller than the length of vector2; otherwise, false.</returns>
        public static bool operator <(Vector3 vector1, Vector3 vector2)
        {
            return vector1.Length < vector2.Length;
        }

        /// <summary>Compares the lengths of two vectors.</summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>True if the length of vector1 is smaller or equal than the length of vector2; otherwise, false.</returns>
        public static bool operator <=(Vector3 vector1, Vector3 vector2)
        {
            return vector1.Length <= vector2.Length;
        }

        /// <summary>Compares the lengths of two vectors.</summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>True if the length of vector1 is greater than the length of vector2; otherwise, false</returns>
        public static bool operator >(Vector3 vector1, Vector3 vector2)
        {
            return vector1.Length > vector2.Length;
        }

        /// <summary>Compares the lengths of two vectors.</summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>True if the length of vector1 is greater or equal than the length of vector2; otherwise, false</returns>
        public static bool operator >=(Vector3 vector1, Vector3 vector2)
        {
            return vector1.Length >= vector2.Length;
        }

        /// <summary>
        /// Adds two vectors and returns the result as a Vector structure.
        /// </summary>
        /// <param name="vector1">The first vector to add.</param>
        /// <param name="vector2">The second vector to add.</param>
        /// <returns>The sum of vector1 and vector2.</returns>
        public static Vector3 Add(Vector3 vector1, Vector3 vector2)
        {
            vector1.X += vector2.X;
            vector1.Y += vector2.Y;
            vector1.Z += vector2.Z;
            return vector1;
        }

        /// <summary>
        /// Subtracts the specified vector from another specified vector.
        /// </summary>
        /// <param name="vector1">The vector from which vector2 is subtracted.</param>
        /// <param name="vector2">The vector to subtract from vector1.</param>
        /// <returns>The difference between vector1 and vector2.</returns>
        public static Vector3 Subtract(Vector3 vector1, Vector3 vector2)
        {
            Vector3 vector3;
            vector3.X = vector1.X - vector2.X;
            vector3.Y = vector1.Y - vector2.Y;
            vector3.Z = vector1.Z - vector2.Z;
            return vector3;
        }

        /// <summary>
        /// Multiplies the specified scalar by the specified vector and returns the resulting vector.
        /// </summary>
        /// <param name="scalar">The scalar to multiply.</param>
        /// <param name="vector">The vector to multiply.</param>
        /// <returns>The result of multiplying scalar and vector.</returns>
        public static Vector3 Multiply(double scalar, Vector3 vector)
        {
            vector.X *= scalar;
            vector.Y *= scalar;
            vector.Z *= scalar;
            return vector;
        }

        /// <summary>
        /// Divides the specified vector by the specified scalar and returns the result as a Vector.
        /// </summary>
        /// <param name="vector">The vector structure to divide.</param>
        /// <param name="scalar">The amount by which vector is divided.</param>
        /// <returns>The result of dividing vector by scalar.</returns>
        public static Vector3 Divide(Vector3 vector, double scalar)
        {
            vector.X /= scalar;
            vector.Y /= scalar;
            vector.Z /= scalar;
            return vector;
        }

        /// <summary>Negates the values of X, Y, and Z on this vector.</summary>
        public void Negate()
        {
            this.X = -this.X;
            this.Y = -this.Y;
            this.Z = -this.Z;
        }

        /// <summary>Compares two vectors for equality.</summary>
        /// <param name="value">The vector to compare with this vector.</param>
        /// <returns>True if value has the same X and Y values as this vector; otherwise, false.</returns>
        public bool Equals(Vector3 value)
        {
            if (this.X == value.X && this.Y == value.Y)
                return this.Z == value.Z;
            return false;
        }

        /// <summary>Compares the two specified vectors for equality.</summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>True if t he X and Y components of vector1 and vector2 are equal; otherwise, false.</returns>
        public static bool Equals(Vector3 vector1, Vector3 vector2)
        {
            return vector1.Equals(vector2);
        }

        /// <summary>Calculates the Dot Product of the specified vectors.</summary>
        /// <param name="vector1">The first vector to evaluate.</param>
        /// <param name="vector2">The second vector to evaluate.</param>
        /// <returns>The calculated Dot Product.</returns>
        public static double DotProduct(Vector3 vector1, Vector3 vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        /// <summary>Calculates the Cross Product of the specified vectors</summary>
        /// <param name="vector1">The first vector to evaluate.</param>
        /// <param name="vector2">The second vector to evaluate.</param>
        /// <returns>The calculated Cross Product.</returns>
        public static Vector3 CrossProduct(Vector3 vector1, Vector3 vector2)
        {
            Vector3 vector3;
            vector3.X = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
            vector3.Y = vector1.Z * vector2.X - vector1.X * vector2.Z;
            vector3.Z = vector1.X * vector2.Y - vector1.Y * vector2.X;
            return vector3;
        }

        /// <summary>
        /// Calculates the distance of the specified vectors in 3D space.
        /// </summary>
        /// <param name="vector1">The first vector to evaluate.</param>
        /// <param name="vector2">The second vector to evaluate.</param>
        /// <returns>The distance between vector1 and vector2.</returns>
        public static double Distance(Vector3 vector1, Vector3 vector2)
        {
            return Math.Sqrt((vector1.X - vector2.X) * (vector1.X - vector2.X) + (vector1.Y - vector2.Y) * (vector1.Y - vector2.Y) + (vector1.Z - vector2.Z) * (vector1.Z - vector2.Z));
        }

        /// <summary>
        /// Calculates the distance btween the current and the specified vector in the 3D space.
        /// </summary>
        /// <param name="other">The vector to evaluate.</param>
        /// <returns>The distance between the vectors.</returns>
        public double Distance(Vector3 other)
        {
            return Vector3.Distance(this, other);
        }

        /// <summary>
        /// Calculates the angle, expressed in degrees, between the two specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector to evaluate.</param>
        /// <param name="vector2">The second vector to evaluate.</param>
        /// <returns>The angle, in degrees, between vector1 and vector2.</returns>
        public static double Angle(Vector3 vector1, Vector3 vector2)
        {
            vector1.Normalize();
            vector2.Normalize();
            double d = Math.Acos(Vector3.DotProduct(vector1, vector2)) * 57.2957795130823;
            if (double.IsNaN(d) || double.IsInfinity(d))
                return 0.0;
            if (Vector3.CrossProduct(vector1, vector2).Z < 0.0)
                d = 360.0 - d;
            return d;
        }

        /// <summary>
        /// Normalizes this vector (a normalized vector maintains its direction but its Length becomes 1).
        /// </summary>
        public void Normalize()
        {
            double length = this.Length;
            if (length == 0.0)
                return;
            double num = 1.0 / length;
            this.X = this.X * num;
            this.Y = this.Y * num;
            this.Z = this.Z * num;
        }

        /// <summary>
        /// Calculates the interpolated vector of the specified vectors and the specified fraction.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="fraction">The control fraction (a number between 0 and 1).</param>
        /// <returns>The interpolation vector.</returns>
        public static Vector3 Interpolate(Vector3 vector1, Vector3 vector2, double fraction)
        {
            if (fraction <= 0.0 || fraction >= 1.0)
                return Vector3.Zero;
            Vector3 vector3;
            vector3.X = vector1.X * (1.0 - fraction) + vector2.X * fraction;
            vector3.Y = vector1.Y * (1.0 - fraction) + vector2.Y * fraction;
            vector3.Z = vector1.Z * (1.0 - fraction) + vector2.Z * fraction;
            return vector3;
        }

        /// <summary>
        /// Calculates the interpolated vector of the current vector, the specified vector and the specified fraction.
        /// </summary>
        /// <param name="vector">The specified vector.</param>
        /// <param name="fraction">The control fraction (a number between 0 and 1).</param>
        /// <returns>The interpolation vector.</returns>
        public Vector3 Interpolate(Vector3 vector, double fraction)
        {
            return Vector3.Interpolate(this, vector, fraction);
        }

        /// <summary>
        /// Compares two vectors and returns the one with the maximum length.
        /// </summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>The vector with the maximum length.</returns>
        public static Vector3 Max(Vector3 vector1, Vector3 vector2)
        {
            if (vector1 >= vector2)
                return vector1;
            return vector2;
        }

        /// <summary>
        /// Compares this vector with the the specified one and returns the one with the maximum length.
        /// </summary>
        /// <param name="value">The vector to compare.</param>
        /// <returns>The vector with the maximum length.</returns>
        public Vector3 Max(Vector3 value)
        {
            return Vector3.Max(this, value);
        }

        /// <summary>
        /// Compares two vectors and returns the one with the minimum length.
        /// </summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>The vector with the minimum length.</returns>
        public static Vector3 Min(Vector3 vector1, Vector3 vector2)
        {
            if (vector1 <= vector2)
                return vector1;
            return vector2;
        }

        /// <summary>
        /// Compares this vector with the the specified one and returns the one with the minimum length.
        /// </summary>
        /// <param name="value">The vector to compare.</param>
        /// <returns>The vector with the minimum length.</returns>
        public Vector3 Min(Vector3 value)
        {
            return Vector3.Min(this, value);
        }

        /// <summary>
        /// Rotates the specified vector around the X axis by the given degrees (Euler rotation around X).
        /// </summary>
        /// <param name="value">The vector to rotate.</param>
        /// <param name="degree">The number of the degrees to rotate.</param>
        /// <returns>The rotated vector.</returns>
        public static Vector3 Pitch(Vector3 value, double degree)
        {
            Vector3 vector3;
            vector3.X = value.X;
            vector3.Y = value.Y * Math.Cos(degree) - value.Z * Math.Sin(degree);
            vector3.Z = value.Y * Math.Sin(degree) + value.Z * Math.Cos(degree);
            return vector3;
        }

        /// <summary>
        /// Rotates this vector around the X axis by the given degrees (Euler rotation around X).
        /// </summary>
        /// <param name="degree">The number of the degrees to rotate.</param>
        public void Pitch(double degree)
        {
            this = Vector3.Pitch(this, degree);
        }

        /// <summary>
        /// Rotates the specified vector around the Y axis by the given degrees (Euler rotation around Y).
        /// </summary>
        /// <param name="value">The vector to rotate.</param>
        /// <param name="degree">The number of the degrees to rotate.</param>
        /// <returns>The rotated vector.</returns>
        public static Vector3 Yaw(Vector3 value, double degree)
        {
            Vector3 vector3;
            vector3.X = value.Z * Math.Sin(degree) + value.X * Math.Cos(degree);
            vector3.Y = value.Y;
            vector3.Z = value.Z * Math.Cos(degree) - value.X * Math.Sin(degree);
            return vector3;
        }

        /// <summary>
        /// Rotates this vector around the Y axis by the given degrees (Euler rotation around Y).
        /// </summary>
        /// <param name="degree">The number of the degrees to rotate.</param>
        public void Yaw(double degree)
        {
            this = Vector3.Yaw(this, degree);
        }

        /// <summary>
        /// Rotates the specified vector around the Z axis by the given degrees (Euler rotation around Z).
        /// </summary>
        /// <param name="value">The vector to rotate.</param>
        /// <param name="degree">The number of the degrees to rotate.</param>
        /// <returns>The rotated vector.</returns>
        public static Vector3 Roll(Vector3 value, double degree)
        {
            Vector3 vector3;
            vector3.X = value.X * Math.Cos(degree) - value.Y * Math.Sin(degree);
            vector3.Y = value.X * Math.Sin(degree) + value.Y * Math.Cos(degree);
            vector3.Z = value.Z;
            return vector3;
        }

        /// <summary>
        /// Rotates this vector around the Z axis by the given degrees (Euler rotation around Z).
        /// </summary>
        /// <param name="degree">The number of the degrees to rotate.</param>
        public void Roll(double degree)
        {
            this = Vector3.Roll(this, degree);
        }

        /// <summary>
        /// Compares the length of this vector with the length of the specified object.
        /// </summary>
        /// <param name="other">The object to compare.</param>
        /// <returns>A positive number if the length of this vector is greater than the other's. A negative number if it's smaller. Zero otherwise.</returns>
        public int CompareTo(object other)
        {
            return this.CompareTo((Vector3)other);
        }

        /// <summary>
        /// Compares the length of this vector with the length of the specified one.
        /// </summary>
        /// <param name="other">The vector to compare.</param>
        /// <returns>A positive number if the length of this vector is greater than the other's. A negative number if it's smaller. Zero otherwise.</returns>
        public int CompareTo(Vector3 other)
        {
            if (this < other)
                return -1;
            return this > other ? 1 : 0;
        }

        /// <summary>Compares two vectors for equality.</summary>
        /// <param name="obj">The cast vector to compare with this vector.</param>
        /// <returns>True if value has the same X and Y values as this vector; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector3)
                return this.Equals((Vector3)obj);
            return false;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
        }

        /// <summary>
        /// Creates a string representation of this vector based on the current culture.
        /// </summary>
        /// <returns>A string representation of this vector.</returns>
        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}, Z: {2}", (object)this.X, (object)this.Y, (object)this.Z);
        }
    }

    /// <summary>
    /// Represents a floor plane.
    /// https://github.com/LightBuzz/kinect-floor
    /// https://pterneas.com/2017/09/10/floor-kinect/
    /// https://www.codeproject.com/Articles/1275569/Floor-Detection-using-Kinect-2
    /// </summary>
    public class Floor
    {
        /// <summary>
        /// The X, Y, Z, and W values of the FloorClipPlane quaternion.
        /// </summary>
        public float X { get; internal set; }
        public float Y { get; internal set; }
        public float Z { get; internal set; }
        public float W { get; internal set; }

        /// <summary>
        /// Creates a new instance of <see cref="Floor"/>.
        /// </summary>
        /// <param name="floorClipPlane">The FloorClipPlane quaternion that describes the floor.</param>
        public Floor(Vector4 floorClipPlane)
        {
            X = floorClipPlane.X;
            Y = floorClipPlane.Y;
            Z = floorClipPlane.Z;
            W = floorClipPlane.W;
        }


        /// <summary>
        /// Returns the height of the Kinect sensor.
        /// </summary>
        public float Height
        {
            get { return W; }
        }


        /// <summary>
        /// Returns the sensor's tilt angle (in degrees).
        /// </summary>
        public double SensorTilt
        {
            get { return Math.Atan(Z / Y) * (180.0 / Math.PI); }
        }

        /// <summary>
        /// Returns the sensor's tilt angle (in degrees).
        /// </summary>
        public double SensorLateralTilt
        {
            get { return Math.Atan(X / Y) * (180.0 / Math.PI); }
        }

        /// <summary>
        /// Calculates the distance between the specified joint point and the floor.
        /// </summary>
        /// <param name="point">The point to measure the distance from.</param>
        /// <returns>The distance between the floor and the point (in meters).</returns>
        public double DistanceFromFloor(CameraSpacePoint point)
        {
            double numerator = X * point.X + Y * point.Y + Z * point.Z + W;
            double denominator = Math.Sqrt(X * X + Y * Y + Z * Z);

            return numerator / denominator;
        }
    }

}
