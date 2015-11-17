using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;

using System.Windows.Threading;
using System.Windows.Input;

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


        MovimientosCuboDAL movesDAL = new MovimientosCuboDAL();

        public DroneControl droneXontrol = null;

        public System.Windows.Controls.RichTextBox TextoLeft;
        public System.Windows.Controls.RichTextBox TextoRight;

        public System.Windows.Controls.RichTextBox TextoVelocidad;


        int cHD = 0;
        int cHI = 0;
        int cHAdel = 0;
        int cHAtras = 0;
        int cHAbajo = 0;
        int cHArriba = 0;


        int frecuencia = 5;


        #region Movimientos

        private MovimientoCubo mArriba;
        private MovimientoCubo mAbajo;
        private MovimientoCubo mIzquierda;
        private MovimientoCubo mDerecha;
        private MovimientoCubo mAdelante;
        private MovimientoCubo mAtras;
        private MovimientoCubo mGiroDerecha;
        private MovimientoCubo mGiroIzquierda;
        private MovimientoCubo mDespegar;
        private MovimientoCubo mAterrizar;
        private MovimientoCubo mEmergencia;
        private MovimientoCubo mFlatTrim;

        #endregion

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

        private void inicializarRangos()
        {
            mArriba = movesDAL.selectMovimientosManoDerecha("ARRIBA");
            mAbajo = movesDAL.selectMovimientosManoDerecha("ABAJO");
            mIzquierda = movesDAL.selectMovimientosManoDerecha("IZQUIERDA");
            mDerecha = movesDAL.selectMovimientosManoDerecha("DERECHA");
            mAdelante = movesDAL.selectMovimientosManoDerecha("ADELANTE");
            mAtras = movesDAL.selectMovimientosManoDerecha("ATRAS");
            mGiroDerecha = movesDAL.selectMovimientosManoIzquierda("DERECHA");
            mGiroIzquierda = movesDAL.selectMovimientosManoIzquierda("IZQUIERDA");
            mDespegar = movesDAL.selectMovimientosManoIzquierda("DESPEGAR");
            mAterrizar = movesDAL.selectMovimientosManoIzquierda("ATERRIZAR");
            mEmergencia = movesDAL.selectMovimientosManoIzquierda("EMERGENCIA");
            mFlatTrim = movesDAL.selectMovimientosManoIzquierda("FLAT TRIM");

        }

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

                //ESTABLECER LOS CUBOS QUE ESTABLECEN LOS MOVIMIENTOS DE CADA MANO
                inicializarRangos();

                sensor.Start();

            }
            catch (Exception)
            {

                //  System.Windows.MessageBox.Show("Kinect no Conectado!");
                MessageBox.Show("Error en inicio de Kinect");
            }

        }

        public void detenerKinect()
        {
            if (sensor != null)
            {
            
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
                            //if (controlPosiciones.controlDerecha(0.4, -0.4, 0.50, 0.20, 0.3, -0.3))
                            if (controlPosiciones.controlDerecha(mArriba.MaximoX, mArriba.MinimoX, mArriba.MaximoY,mArriba.MinimoY, mArriba.MaximoZ,mArriba.MinimoZ))
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
                            //if (controlPosiciones.controlDerecha(0.4, -0.4, -0.2, -0.50, 0.3, -0.3))
                            if (controlPosiciones.controlDerecha(mAbajo.MaximoX, mAbajo.MinimoX, mAbajo.MaximoY, mAbajo.MinimoY, mAbajo.MaximoZ, mAbajo.MinimoZ))
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

                           // if (controlPosiciones.controlDerecha(-0.20, -0.5, 0.4, -0.4, 0.3, -0.3))
                            if (controlPosiciones.controlDerecha(mIzquierda.MaximoX, mIzquierda.MinimoX, mIzquierda.MaximoY, mIzquierda.MinimoY, mIzquierda.MaximoZ, mIzquierda.MinimoZ))
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

                            //if (controlPosiciones.controlDerecha(0.5, 0.2, 0.4, -0.4, 0.3, -0.3))
                            if (controlPosiciones.controlDerecha(mDerecha.MaximoX, mDerecha.MinimoX, mDerecha.MaximoY, mDerecha.MinimoY, mDerecha.MaximoZ, mDerecha.MinimoZ))
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

//                            if (controlPosiciones.controlDerecha(0.25, -0.25, 0.25, -0.25, -0.2, -0.5))
                            if (controlPosiciones.controlDerecha(mAdelante.MaximoX, mAdelante.MinimoX, mAdelante.MaximoY, mAdelante.MinimoY, mAdelante.MaximoZ, mAdelante.MinimoZ))
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

                          //  if (controlPosiciones.controlDerecha(0.15, -0.15, 0.15, -0.15, 0.4, -0.1))
                            if (controlPosiciones.controlDerecha(mAtras.MaximoX, mAtras.MinimoX, mAtras.MaximoY, mAtras.MinimoY, mAtras.MaximoZ, mAtras.MinimoZ))
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

                        TextoRight.Document = myFlowDoc;

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
                            //if (controlPosiciones.controlIzquierda(0.15, -0.15, 0.60, 0.40, 0.30, -0.15))
                            if (controlPosiciones.controlIzquierda(mDespegar.MaximoX, mDespegar.MinimoX, mDespegar.MaximoY, mDespegar.MinimoY, mDespegar.MaximoZ, mDespegar.MinimoZ))
                            {
                                myParagraph2.Inlines.Add("L DESPEGAR ");

                                Takeoff();

                            }
                          //  if (controlPosiciones.controlIzquierda(0.15, -0.15, -0.3, -0.50, 0.15, -0.15))
                            if (controlPosiciones.controlIzquierda(mAterrizar.MaximoX, mAterrizar.MinimoX, mAterrizar.MaximoY, mAterrizar.MinimoY, mAterrizar.MaximoZ, mAterrizar.MinimoZ))
                            {
                                myParagraph2.Inlines.Add("L ATERRIZAR ");
                                Land();
                            }

                           // if (controlPosiciones.controlIzquierda(-0.20, -0.4, 0.15, -0.15, 0.15, -0.15))
                            if (controlPosiciones.controlIzquierda(mGiroIzquierda.MaximoX, mGiroIzquierda.MinimoX, mGiroIzquierda.MaximoY, mGiroIzquierda.MinimoY, mGiroIzquierda.MaximoZ, mGiroIzquierda.MinimoZ))
                            {
                                myParagraph2.Inlines.Add("L IZQUIERDA ");
                                myParagraph2.Inlines.Add("" + (controlPosiciones.CentroXL - leftHand.Position.X) / velocidad);

                                vYaw = (controlPosiciones.CentroXL - leftHand.Position.X) / velocidad;

                                Navigate(0, 0f, vYaw, 0f);

                            }

                            //if (controlPosiciones.controlIzquierda(0.4, 0.2, 0.15, -0.15, 0.15, -0.15))
                            if (controlPosiciones.controlIzquierda(mGiroDerecha.MaximoX, mGiroDerecha.MinimoX, mGiroDerecha.MaximoY, mGiroDerecha.MinimoY, mGiroDerecha.MaximoZ, mGiroDerecha.MinimoZ))
                            {
                                myParagraph2.Inlines.Add("L DERECHA ");
                                myParagraph2.Inlines.Add("" + (controlPosiciones.CentroXL - leftHand.Position.X) / velocidad);

                                vYaw = (controlPosiciones.CentroXL - leftHand.Position.X) / velocidad;

                                Navigate(0, 0f, vYaw, 0f);
                            }

                         //   if (controlPosiciones.controlIzquierda(0.15, -0.15, 0.30, -0.3, -0.3, -0.5))
                            if (controlPosiciones.controlIzquierda(mEmergencia.MaximoX, mEmergencia.MinimoX, mEmergencia.MaximoY, mEmergencia.MinimoY, mEmergencia.MaximoZ, mEmergencia.MinimoZ))
                            {
                                myParagraph2.Inlines.Add("L Emergencia ");

                                Emergency();
                            }

                           // if (controlPosiciones.controlIzquierda(0.15, -0.15, 0.15, -0.15, 0.3, 0.1))
                            if (controlPosiciones.controlIzquierda(mFlatTrim.MaximoX, mFlatTrim.MinimoX, mFlatTrim.MaximoY, mFlatTrim.MinimoY, mFlatTrim.MaximoZ, mFlatTrim.MinimoZ))
                            {
                                myParagraph2.Inlines.Add("L FlatTrim ");

                                FlatTrim();
                            }

                        }

                        #endregion

                        myFlowDoc2.Blocks.Add(myParagraph2);


                        TextoLeft.Document = myFlowDoc2;


                    }
                }

            }
        }


        #region Movimiento de helicoptero
       
        /// <summary>
        /// Aceleracion por 10 segundos a la derecha 
        ///    crearRuta(10, "X", -0.5f);
        /// Aceleracion por 10 segundos a la izquierda
        ///    crearRuta(10, "X", 0.5f);
        /// Aceleracion por 10 segundos hacia Arriba
        ///     crearRuta(10, "Y", -0.5f);
        /// Aceleracion por 10 segundos hacia Abajo
        ///    crearRuta(10, "Y", 0.5f);
        /// Aceleracion por 10 segundos hacia Adelante
        ///    crearRuta(10, "Z", 0.5f);
        /// Aceleracion por 10 segundos hacia Atras
        ///    crearRuta(10, "Z", -0.5f);
        /// Girar por 10 segundos hacia la derecha
        ///    crearRuta(10, "Z", -0.5f);
        /// Girar por 10 segundos hacia la izquierda
        ///    crearRuta(10, "Z", 0.5f);
        /// </summary>
        public void crearRuta1()
        {

            //Despegar en 3 segundos
            crearRuta(3, "D", 0f);

            ////Desplazamiento de 4 segundos a la derecha
            //crearRuta(4, "X", -0.5f);
            ////Desplazamiento de 4 segundos hacia adelante
            //crearRuta(4, "Z", 0.5f);
            ////Desplazamiento de 4 segundos a la izquierda
            //crearRuta(4, "X", 0.5f);
            ////Desplazamiento de 4 segundos hacia atras
            //crearRuta(4, "Z", -0.5f);

       //     Land();
         
        }
        public void crearRuta2()
        {

          //  Takeoff();

            //Despegar en 3 segundos
            //crearRuta(3, "D", 0f);

            //Desplazamiento de 4 segundos hacia adelante
            crearRuta(5, "Z", 0.5f);

          //  Desplazamiento de 4 segundos hacia adelante
            crearRuta(6, "Z", 0.5f);

            //Desplazamiento de 4 segundos hacia arriba
          //  crearRuta(3, "Y", -0.5f);

          //  Land();

        }

        string _direccion;
        float _acelerator;
        int _tiempo;

        public void crearRuta(int segundos, string direccion,float aceleracion)
        {
            _direccion = direccion;
            _acelerator = aceleracion;
            _tiempo = segundos;

            DispatcherTimer tiempo = new DispatcherTimer();

            tiempo.Tick += new EventHandler(tiempo_Tick);

            tiempo.Interval = new TimeSpan(0, 0, 1);

            tiempo.Start();

        }


        FlowDocument myFlowDoc3 = new FlowDocument();
        Paragraph myParagraph3 = new Paragraph();

        //// Create a FlowDocument to contain content for the RichTextBox.
        //FlowDocument myFlowDoc3;
        //// Create a paragraph and add the Run and Bold to it.
        //Paragraph myParagraph3 ;

        int contador = 0;

        void tiempo_Tick(object sender, EventArgs e)
        {
            //myparagraph3 = new paragraph();
            //myflowdoc3 = new flowdocument();

            contador++;
            switch (_direccion)
            {
                case "X":
                    {
                        Navigate(_acelerator, 0f, 0f, 0f);
                        if (_acelerator > 0)
                        {
                            myParagraph3.Inlines.Add("Movimiendose hacia la izquierda a " + _acelerator.ToString()+"\n en "+contador+" de "+_tiempo);
                        }
                        else
                        {
                            myParagraph3.Inlines.Add("Movimiendose hacia la derecha a " + _acelerator.ToString() + "\n en " + contador + " de " + _tiempo);
                        }
                    }break;
                case "Y":
                    {
                        Navigate(0f, 0f, 0f, _acelerator);
                        if (_acelerator > 0)
                        {
                            myParagraph3.Inlines.Add("Descendiendo a " + _acelerator.ToString() + "\n en " + contador + " de " + _tiempo);
                        }
                        else
                        {
                            myParagraph3.Inlines.Add("Ascendiendo a " + _acelerator.ToString() + "\n en " + contador + " de " + _tiempo);
                        }
                    } break;
                case "Z":
                    {
                        Navigate(0f, _acelerator, 0f, 0f);
                        if (_acelerator > 0)
                        {
                            myParagraph3.Inlines.Add("Movimiendose hacia adelante a " + _acelerator.ToString() + "\n en " + contador + " de " + _tiempo);
                        }
                        else
                        {
                            myParagraph3.Inlines.Add("Movimiendose hacia atras a " + _acelerator.ToString() + "\n en " + contador + " de " + _tiempo);
                        }
                        MessageBox.Show("Direccion Z:" + _direccion);
                    } break;
                case "A":
                    {
                        Navigate(0, 0f, _acelerator, 0f);
                        if (_acelerator > 0)
                        {
                            myParagraph3.Inlines.Add("Girando hacia la izquierda a " + _acelerator.ToString() + "\n en " + contador + " de " + _tiempo);
                        }
                        else
                        {
                            myParagraph3.Inlines.Add("Girando hacia la derecha a " + _acelerator.ToString() + "\n en " + contador + " de " + _tiempo);
                        }
                    } break;
                case "D":
                    {
                        Takeoff();
                        myParagraph3.Inlines.Add("Despegando en "+contador+" de "+_tiempo);
                    }break;

            }
            MessageBox.Show("Direccion:"+_direccion);

            //Instanciar timer para deternerlo
            if (contador == _tiempo)
            {
                DispatcherTimer tiempo = (DispatcherTimer)sender;
                tiempo.Stop();
                contador = 0;
            }


            //Imprimo el texto que describe el movimiento actual de cuadrirotor
            //En el txtVelocidad del Formulario
            myFlowDoc3.Blocks.Add(myParagraph3);
            TextoVelocidad.Document = myFlowDoc3;
        }

        private void Takeoff()
        {
            Command takeOffCommand = new FlightModeCommand(DroneFlightMode.TakeOff);

            if (!droneXontrol.IsCommandPossible(takeOffCommand))
                return;

            droneXontrol.SendCommand(takeOffCommand);
        }

        private void Land()
        {
            Command landCommand = new FlightModeCommand(DroneFlightMode.Land);

            if (!droneXontrol.IsCommandPossible(landCommand))
                return;

            droneXontrol.SendCommand(landCommand);
        }

        private void Navigate(float roll, float pitch, float yaw, float gaz)
        {
            FlightMoveCommand flightMoveCommand = new FlightMoveCommand(roll, pitch, yaw, gaz);

            if (droneXontrol.IsCommandPossible(flightMoveCommand))
                droneXontrol.SendCommand(flightMoveCommand);
        }

        private void EnterHoverMode()
        {
            Command enterHoverModeCommand = new HoverModeCommand(DroneHoverMode.Hover);

            if (!droneXontrol.IsCommandPossible(enterHoverModeCommand))
                return;

            droneXontrol.SendCommand(enterHoverModeCommand);
        }

        private void LeaveHoverMode()
        {
            Command leaveHoverModeCommand = new HoverModeCommand(DroneHoverMode.StopHovering);

            if (!droneXontrol.IsCommandPossible(leaveHoverModeCommand))
                return;

            droneXontrol.SendCommand(leaveHoverModeCommand);

        }

        private void Emergency()
        {
            Command emergencyCommand = new FlightModeCommand(DroneFlightMode.Emergency);

            if (!droneXontrol.IsCommandPossible(emergencyCommand))
                return;

            droneXontrol.SendCommand(emergencyCommand);

        }
        private void FlatTrim()
        {
            Command resetCommand = new FlightModeCommand(DroneFlightMode.Reset);
            Command flatTrimCommand = new FlatTrimCommand();

            if (!droneXontrol.IsCommandPossible(resetCommand) || !droneXontrol.IsCommandPossible(flatTrimCommand))
                return;

            droneXontrol.SendCommand(resetCommand);
            droneXontrol.SendCommand(flatTrimCommand);

        }

        #endregion


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
