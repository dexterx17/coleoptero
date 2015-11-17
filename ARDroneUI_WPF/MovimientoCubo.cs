using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARDrone.UI
{
    /// <summary>
    /// Objeto que representa un Cubo en 3 dimension
    /// Eje X =>ancho
    /// Eje Y =>alto
    /// Eje Z =>profundidad
    /// </summary>
    public class MovimientoCubo
    {
        #region Datos

        private string _nombre;
        private double _minimoX;
        private double _maximoX;
        private double _minimoY;
        private double _maximoY;
        private double _minimoZ;
        private double _maximoZ;

        #endregion

        #region Propiedades

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }


        public double MinimoX
        {
            get { return _minimoX; }
            set { _minimoX = value; }
        }

        public double MaximoX
        {
            get { return _maximoX; }
            set { _maximoX = value; }
        }

        public double MinimoY
        {
            get { return _minimoY; }
            set { _minimoY = value; }
        }

        public double MaximoY
        {
            get { return _maximoY; }
            set { _maximoY = value; }
        }

        public double MinimoZ
        {
            get { return _minimoZ; }
            set { _minimoZ = value; }
        }

        public double MaximoZ
        {
            get { return _maximoZ; }
            set { _maximoZ = value; }
        }
        #endregion

    }
}
