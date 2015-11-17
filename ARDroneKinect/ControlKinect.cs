using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;


using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf;
using System.Windows.Documents;

using ARDrone.Control;
using ARDrone.Control.Commands;
using ARDrone.Control.Data;
using ARDrone.Control.Events;

namespace ARDrone.UI
{
    public class ControlKinect
    {

        const int KEYEVENTF_KEYDOWN = 0x0;
        const int KEYEVENTF_KEYUP = 0x2;

        //Clase con operaciones de posiciones X Y Z
        ControlPosiciones controlPosiciones = new ControlPosiciones();

        public DroneControl droneXontrol = null;

        public System.Windows.Controls.RichTextBox TEXTOLEFT = new System.Windows.Controls.RichTextBox();
        public System.Windows.Controls.RichTextBox TEXTORIGHT = new System.Windows.Controls.RichTextBox();


        int cHD = 0;
        int cHI = 0;
        int cHAdel = 0;
        int cHAtras = 0;
        int cHAbajo = 0;
        int cHArriba = 0;


        int frecuencia = 5;

        #region Variables de clase

        KinectSensor sensor = null;

        Image imagenDepth = null;

        Skeleton[] skeletons;

        float vRoll = 0;

        float vPitch = 0;

        float vYaw = 0;

        float vGaz = 0;

        byte[] colorBytes;

        #endregion

        #region Propiedades

        public float VGaz
        {
            get { return vGaz; }
            set { vGaz = value; }
        }

        public float VYaw
        {
            get { return vYaw; }
            set { vYaw = value; }
        }


        public float VPitch
        {
            get { return vPitch; }
            set { vPitch = value; }
        }

        public float VRoll
        {
            get { return vRoll; }
            set { vRoll = value; }
        }


        public Image ImagenDepth
        {
            set
            {
                imagenDepth = value;
            }
        }


        #endregion


        public void inicializarKinect()
        {
            try
            {

                sensor = KinectSensor.KinectSensors.FirstOrDefault();

                //Inicializa y configura de traqueo de profundidad
                configTraqueoDepth();

                //Inicializa y configura traqueo de esqueleto
                configTraqueoSkeleton();

                //Inicializa y configura camara RGB
                // configTraqueoColor();

                //   sensor.ElevationAngle = 0;

                sensor.Start();

            }
            catch (Exception)
            {

                //  System.Windows.MessageBox.Show("Kinect no Conectado!");
            }

        }

        public void detenerKinect()
        {
            if (sensor != null)
            {
                sensor.AudioSource.Stop();
                sensor.Stop();
                sensor.Dispose();
                sensor = null;
            }
        }

        #region Traking de Color RGB

        public void configTraqueoColor()
        {
            sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            sensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(sensor_ColorFrameReady);
        }

        void sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (var image = e.OpenColorImageFrame())
            {
                if (image == null)
                    return;

                if (colorBytes == null ||
                    colorBytes.Length != image.PixelDataLength)
                {
                    colorBytes = new byte[image.PixelDataLength];
                }

                image.CopyPixelDataTo(colorBytes);

                //You could use PixelFormats.Bgr32 below to ignore the alpha,
                //or if you need to set the alpha you would loop through the bytes 
                //as in this loop below
                int length = colorBytes.Length;
                for (int i = 0; i < length; i += 4)
                {
                    colorBytes[i + 3] = 255;
                }

                BitmapSource source = BitmapSource.Create(image.Width,
                    image.Height,
                    96,
                    96,
                    PixelFormats.Bgra32,
                    null,
                    colorBytes,
                    image.Width * image.BytesPerPixel);
                //  imagenDepth.Source = source;
            }
        }

        #endregion

        #region Traking de Profundidad

        private void configTraqueoDepth()
        {
            sensor.DepthFrameReady += new EventHandler<DepthImageFrameReadyEventArgs>(sensor_DepthFrameReady);

            sensor.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
        }

        void sensor_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            imagenDepth.Source = e.OpenDepthImageFrame().ToBitmapSource();
        }

        #endregion

        private void configTraqueoSkeleton()
        {
            sensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(sensor_SkeletonFrameReady);

            sensor.SkeletonStream.Enable();

            //  sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        void sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (var skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null)
                    return;

                if (skeletons == null ||
                    skeletons.Length != skeletonFrame.SkeletonArrayLength)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                }

                skeletonFrame.CopySkeletonDataTo(skeletons);

                foreach (var skeleton in skeletons)
                {
                    if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {

                        var head = skeleton.Joints[JointType.Head];
                        var rightHand = skeleton.Joints[JointType.HandRight];
                        var leftHand = skeleton.Joints[JointType.HandLeft];
                        var rightHombro = skeleton.Joints[JointType.ShoulderRight];
                        var leftHombro = skeleton.Joints[JointType.ShoulderLeft];
                        var rightCintura = skeleton.Joints[JointType.HipRight];
                        var leftCintura = skeleton.Joints[JointType.HipLeft];

                        //   SetEllipsePosition(circuloMano, rightHand);
                        //   SetEllipsePosition(circuloCintura, cintura);

                        //   dibujarCentro(centro,head,cintura);

                        controlPosiciones.Head = head;

                        controlPosiciones.RHand = rightHand;
                        controlPosiciones.LHand = leftHand;

                        controlPosiciones.RHombro = rightHombro;
                        controlPosiciones.LHombro = leftHombro;

                        controlPosiciones.RCintura = rightCintura;
                        controlPosiciones.LCintura = leftCintura;

                        controlPosiciones.setearCentroDerecho();
                        controlPosiciones.setearCentroIzquierdo();



                        float velocidad = 0.5f;

                        #region Mano derecha


                        // Create a FlowDocument to contain content for the RichTextBox.
                        FlowDocument myFlowDoc = new FlowDocument();

                        // Create a paragraph and add the Run and Bold to it.
                        Paragraph myParagraph = new Paragraph();


                        if (controlPosiciones.EstabilidadDerecha())
                        {
                            myParagraph.Inlines.Add("R EQUILIBRADO ");
                            Navigate((float)0, (float)0, (float)0, (float)0);
                        }
                        else
                        {
                            if (controlPosiciones.controlDerecha(0.4, -0.4, 0.50, 0.20, 0.3, -0.3))
                            {
                                myParagraph.Inlines.Add("R ARRIBA  ");


                                vGaz = (rightHand.Position.Y - controlPosiciones.CentroYR) / 0.3f;

                                if (cHArriba <= frecuencia)
                                {
                                    cHArriba++;
                                }
                                else
                                {
                                    myParagraph.Inlines.Add("\n" + (controlPosiciones.CentroYR - rightHand.Position.Y) / 0.3f);
                                    Navigate(0f, 0f, 0f, vGaz);
                                    cHArriba = 0;
                                }


                            }
                            if (controlPosiciones.controlDerecha(0.4, -0.4, -0.2, -0.50, 0.3, -0.3))
                            {
                                myParagraph.Inlines.Add("R ABAJO ");


                                vGaz = (rightHand.Position.Y - controlPosiciones.CentroYR) / velocidad;

                                if (cHAbajo <= frecuencia)
                                {
                                    cHAbajo++;
                                }
                                else
                                {
                                    myParagraph.Inlines.Add("" + (controlPosiciones.CentroYR - rightHand.Position.Y) / velocidad);
                                    Navigate(0f, 0f, 0f, vGaz);
                                    cHAbajo = 0;
                                }
                            }

                            if (controlPosiciones.controlDerecha(-0.20, -0.5, 0.4, -0.4, 0.3, -0.3))
                            {
                                myParagraph.Inlines.Add("R IZQUIERDA ");


                                vRoll = (rightHand.Position.X - controlPosiciones.CentroXR) / velocidad;

                                if (cHI <= frecuencia)
                                {
                                    cHI++;
                                }
                                else
                                {
                                    myParagraph.Inlines.Add("\n" + (controlPosiciones.CentroXR - rightHand.Position.X) / velocidad);
                                    Navigate(vRoll, 0f, 0f, 0f);
                                    cHI = 0;
                                }
                            }

                            if (controlPosiciones.controlDerecha(0.5, 0.2, 0.4, -0.4, 0.3, -0.3))
                            {
                                myParagraph.Inlines.Add("R DERECHA ");


                                vRoll = (rightHand.Position.X - controlPosiciones.CentroXR) / velocidad;

                                if (cHD <= frecuencia)
                                {
                                    cHD++;
                                }
                                else
                                {
                                    myParagraph.Inlines.Add("\n" + (controlPosiciones.CentroXR - rightHand.Position.X) / velocidad);
                                    Navigate(vRoll, 0f, 0f, 0f);
                                    cHD = 0;
                                }
                            }

                            if (controlPosiciones.controlDerecha(0.25, -0.25, 0.25, -0.25, -0.2, -0.5))
                            {
                                myParagraph.Inlines.Add("R ADELANTE ");


                                vPitch = (rightHand.Position.Z - controlPosiciones.CentroZR) / velocidad;
                                if (cHAdel <= frecuencia)
                                {
                                    cHAdel++;
                                }
                                else
                                {
                                    myParagraph.Inlines.Add("\n" + (controlPosiciones.CentroZR - rightHand.Position.Z) / velocidad);
                                    Navigate(0f, vPitch, 0f, 0f);
                                    cHAdel = 0;
                                }
                            }

                            if (controlPosiciones.controlDerecha(0.15, -0.15, 0.15, -0.15, 0.4, -0.1))
                            {
                                myParagraph.Inlines.Add("R ATRAS ");


                                vPitch = (rightHand.Position.Z - controlPosiciones.CentroZR) / velocidad;
                                if (cHAtras <= frecuencia)
                                {
                                    cHAtras++;
                                }
                                else
                                {
                                    myParagraph.Inlines.Add("" + (controlPosiciones.CentroZR - rightHand.Position.Z) / velocidad);
                                    Navigate(0f, vPitch, 0f, 0f);
                                    cHAtras = 0;
                                }
                            }
                        }

                        myFlowDoc.Blocks.Add(myParagraph);

                        TEXTORIGHT.Document = myFlowDoc;

                        #endregion


                        // Create a FlowDocument to contain content for the RichTextBox.
                        FlowDocument myFlowDoc2 = new FlowDocument();

                        // Create a paragraph and add the Run and Bold to it.
                        Paragraph myParagraph2 = new Paragraph();


                        #region Mano izquierda
                        if (controlPosiciones.EstabilidadIzquierda())
                        {
                            myParagraph2.Inlines.Add("L EQUILIBRADO ");

                        }
                        else
                        {
                            if (controlPosiciones.controlIzquierda(0.15, -0.15, 0.60, 0.40, 0.30, -0.15))
                            {
                                myParagraph2.Inlines.Add("L DESPEGAR ");

                                Takeoff();

                            }
                            if (controlPosiciones.controlIzquierda(0.15, -0.15, -0.3, -0.50, 0.15, -0.15))
                            {
                                myParagraph2.Inlines.Add("L ATERRIZAR ");
                                Land();
                            }

                            if (controlPosiciones.controlIzquierda(-0.20, -0.4, 0.15, -0.15, 0.15, -0.15))
                            {
                                myParagraph2.Inlines.Add("L IZQUIERDA ");
                                myParagraph2.Inlines.Add("" + (controlPosiciones.CentroXL - leftHand.Position.X) / velocidad);

                                vYaw = (controlPosiciones.CentroXL - leftHand.Position.X) / velocidad;

                                Navigate(0, 0f, vYaw, 0f);

                            }

                            if (controlPosiciones.controlIzquierda(0.4, 0.2, 0.15, -0.15, 0.15, -0.15))
                            {
                                myParagraph2.Inlines.Add("L DERECHA ");
                                myParagraph2.Inlines.Add("" + (controlPosiciones.CentroXL - leftHand.Position.X) / velocidad);

                                vYaw = (controlPosiciones.CentroXL - leftHand.Position.X) / velocidad;

                                Navigate(0, 0f, vYaw, 0f);
                            }

                            if (controlPosiciones.controlIzquierda(0.15, -0.15, 0.30, -0.3, -0.3, -0.5))
                            {
                                myParagraph2.Inlines.Add("L Emergencia ");

                                Emergency();
                            }

                            if (controlPosiciones.controlIzquierda(0.15, -0.15, 0.15, -0.15, 0.3, 0.1))
                            {
                                myParagraph2.Inlines.Add("L FlatTrim ");

                                FlatTrim();
                            }

                        }

                        #endregion

                        myFlowDoc2.Blocks.Add(myParagraph2);


                        TEXTOLEFT.Document = myFlowDoc2;


                    }
                }

            }
        }


        //#region Movimiento de helicoptero


        //private void Takeoff()
        //{
        //    Command takeOffCommand = new FlightModeCommand(DroneFlightMode.TakeOff);

        //    if (!droneXontrol.IsCommandPossible(takeOffCommand))
        //        return;

        //    droneXontrol.SendCommand(takeOffCommand);
        //}

        //private void Land()
        //{
        //    Command landCommand = new FlightModeCommand(DroneFlightMode.Land);

        //    if (!droneXontrol.IsCommandPossible(landCommand))
        //        return;

        //    droneXontrol.SendCommand(landCommand);
        //}

        //private void Navigate(float roll, float pitch, float yaw, float gaz)
        //{
        //    FlightMoveCommand flightMoveCommand = new FlightMoveCommand(roll, pitch, yaw, gaz);

        //    if (droneXontrol.IsCommandPossible(flightMoveCommand))
        //        droneXontrol.SendCommand(flightMoveCommand);
        //}

        //private void EnterHoverMode()
        //{
        //    Command enterHoverModeCommand = new HoverModeCommand(DroneHoverMode.Hover);

        //    if (!droneXontrol.IsCommandPossible(enterHoverModeCommand))
        //        return;

        //    droneXontrol.SendCommand(enterHoverModeCommand);
        //}

        //private void LeaveHoverMode()
        //{
        //    Command leaveHoverModeCommand = new HoverModeCommand(DroneHoverMode.StopHovering);

        //    if (!droneXontrol.IsCommandPossible(leaveHoverModeCommand))
        //        return;

        //    droneXontrol.SendCommand(leaveHoverModeCommand);

        //}

        //private void Emergency()
        //{
        //    Command emergencyCommand = new FlightModeCommand(DroneFlightMode.Emergency);

        //    if (!droneXontrol.IsCommandPossible(emergencyCommand))
        //        return;

        //    droneXontrol.SendCommand(emergencyCommand);

        //}
        //private void FlatTrim()
        //{
        //    Command resetCommand = new FlightModeCommand(DroneFlightMode.Reset);
        //    Command flatTrimCommand = new FlatTrimCommand();

        //    if (!droneXontrol.IsCommandPossible(resetCommand) || !droneXontrol.IsCommandPossible(flatTrimCommand))
        //        return;

        //    droneXontrol.SendCommand(resetCommand);
        //    droneXontrol.SendCommand(flatTrimCommand);

        //}

        //#endregion


        #region dibujar en interfaz
        //This method is used to position the ellipses on the canvas
        //according to correct movements of the tracked joints.
        /*      private void SetEllipsePosition(Ellipse ellipse, Joint joint)
              {
                  var point = sensor.MapSkeletonPointToColor(joint.Position, sensor.ColorStream.Format);    
                  ellipse.Width = 20;
                  ellipse.Height = 20;
                  ellipse.Fill = new SolidColorBrush(Colors.Green);

                  Canvas.SetLeft(ellipse, (point.X - ellipse.ActualWidth / 2));
                  Canvas.SetTop(ellipse, (point.Y - ellipse.ActualHeight / 2));
              }

              private void dibujarCentro(Canvas forma, Joint head, Joint cint)
              {
                  var point = sensor.MapSkeletonPointToColor(head.Position, sensor.ColorStream.Format);
               //   forma.Fill = new SolidColorBrush(Colors.Green);

                  Canvas.SetLeft(forma, (point.X - forma.ActualWidth / 2));
                  Canvas.SetTop(forma, (point.Y - forma.ActualHeight / 2));
              }
               */


        #endregion

    }
}
