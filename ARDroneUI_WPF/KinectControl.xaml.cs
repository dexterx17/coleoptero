using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using ARDrone.Control;

namespace ARDrone.UI
{
    /// <summary>
    /// Lógica de interacción para KinectControl.xaml
    /// </summary>
    public partial class KinectControl : Window
    {

        ControlKinect controlKinect = new ControlKinect();

        private void aplicarMedidas()
        {
            
        }

        public KinectControl()
        {
            InitializeComponent();
        }

        public KinectControl(DroneControl droneControl)
        {
            InitializeComponent();

            controlKinect.ImagenDepth = imagen;

            controlKinect.TextoLeft = txtLeft;
            controlKinect.TextoRight = txtRight;
            controlKinect.TextoVelocidad = txtVelocidades;
            controlKinect.droneXontrol = droneControl;
            
            controlKinect.inicializarKinect();

        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            controlKinect.ImagenDepth = imagen;
            controlKinect.TextoLeft = txtLeft;
            controlKinect.TextoRight = txtRight;
            controlKinect.TextoVelocidad = txtVelocidades;
            controlKinect.inicializarKinect();
        }

        private void btnDetener_Click(object sender, RoutedEventArgs e)
        {
            controlKinect.detenerKinect();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controlKinect.detenerKinect();
        }

        private void btnIniciar_Click(object sender, RoutedEventArgs e)
        {
            controlKinect.crearRuta1();
            controlKinect.crearRuta2();
        }
    }
}
