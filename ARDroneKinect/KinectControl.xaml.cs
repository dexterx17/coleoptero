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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            controlKinect.ImagenDepth = imagen;

            controlKinect.inicializarKinect();
        }

        private void btnDetener_Click(object sender, RoutedEventArgs e)
        {
            controlKinect.detenerKinect();
        }
    }
}
