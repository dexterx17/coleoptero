using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Kinect;



namespace ARDrone.UI
{
    class ControlPosiciones
    {

        #region Variables de clase

        Joint head;
        Joint rHand;
        Joint lHand;

        Joint rCintura;
        Joint lCintura;

        Joint rHombro;
        Joint lHombro;


        float centroXR = 0;
        float centroYR = 0;
        float centroZR = 0;

        float centroXL = 0;
        float centroYL = 0;
        float centroZL = 0;

        #endregion

        #region Propiedades

        public Joint Head
        {
            set
            {
                head = value;
            }
        }

        public Joint RHand
        {
            set
            {
                rHand = value;
            }
        }
        public Joint LHand
        {
            set
            {
                lHand = value;
            }
        }

        public Joint RCintura
        {
            set
            {
                rCintura = value;
            }
        }
        public Joint LCintura
        {
            set
            {
                lCintura = value;
            }
        }

        public Joint RHombro
        {
            set
            {
                rHombro = value;
            }
        }
        public Joint LHombro
        {
            set
            {
                lHombro = value;
            }
        }

        public float CentroXR
        {
            set
            {
                centroXR = value;
            }
            get
            {
                return centroXR;
            }
        }

        public float CentroYR
        {
            set
            {
                centroYR = value;
            }
            get
            {
                return centroYR;
            }
        }

        public float CentroZR
        {
            set
            {
                centroZR = value;
            }
            get
            {
                return centroZR;
            }
        }

        public float CentroXL
        {
            set
            {
                centroXL = value;
            }
            get
            {
                return centroXL;
            }
        }

        public float CentroYL
        {
            set
            {
                centroYL = value;
            }
            get
            {
                return centroYL;
            }
        }

        public float CentroZL
        {
            set
            {
                centroZL = value;
            }
            get
            {
                return centroZL;
            }
        }
        #endregion

        #region Constructores

        //public ControlPosiciones(float X, float Y, float Z)
        //{
        //    centroX = X;
        //    centroY = Y;
        //    centroZ = Z;
        //}

        #endregion

        public void setearCentroDerecho()
        {
            float medioCuerpo = (Math.Abs(rHombro.Position.Y) + Math.Abs(rCintura.Position.Y)) / 2;

            centroXR = rHombro.Position.X + 0.1f;
            centroYR = rHombro.Position.Y - (float)0.2;
            centroZR = rHombro.Position.Z - (float)0.25;
        }

        public void setearCentroIzquierdo()
        {
            float medioCuerpo = (Math.Abs(lHombro.Position.Y) + Math.Abs(lCintura.Position.Y)) / 2;

            centroXL = lHombro.Position.X - 0.1f;
            centroYL = lHombro.Position.Y - (float)0.2;
            centroZL = lHombro.Position.Z - (float)0.25;
        }

        /// <summary>
        /// Determina si la mano esta en el cubo de estabilidad
        /// Que esta definido esta definidio con las siguientes dimensiones:
        /// en X :  20 centimetros
        /// en Y :  20 centimetros
        /// en Z :  20 centimetros 
        /// </summary>
        /// <returns></returns>
        public bool EstabilidadDerecha()
        {
            float size = (float)0.175;

            float XmanoD = rHand.Position.X;
            float YmanoD = rHand.Position.Y;
            float ZmanoD = rHand.Position.Z;

            //Control en el eje X
            //Si la mano derecha esta a la derecha del centro MENOS 20 centimentros Y
            //Si la mano derecha esta a la izquierda del centro MAS 20 centimientros
            //SI existe equilibro en el ejex X
            if (XmanoD > centroXR - size && XmanoD < centroXR + size)
            {
                //Control en el eje Y
                //Si la mano derecha esta arriba del centro MENOS 20 centimetros Y
                //Si la mano derecha esta abajo del centro MAS 20 centimetros
                //Si existe equilibrio en el eje Y
                if (YmanoD > centroYR - size && YmanoD < centroYR + size)
                {
                    //Control en el eje Z
                    //Si la mano derecha esta mas atras del centro MENOS 20 centimetros
                    //Si la mano derecha esta mas adelante del centro MAS 20 centimetros
                    //Si existe equilibro en el eje Z
                    if (ZmanoD > centroZR - size && ZmanoD < centroZR + size)
                    {
                        return true;
                    }
                    //Si no existe equilibro en el eje Z
                    else
                    {
                        // MessageBox.Show("Equilibrio");
                        return false;
                    }
                }
                //Si no existe equilibrio en el eje Y
                else
                {
                    return false;
                }
            }
            //Si no existe equilibrio en el eje X
            else
            {
                return false;
            }

        }


        /// <summary>
        /// Determina si la mano esta en el cubo de estabilidad
        /// Que esta definido esta definidio con las siguientes dimensiones:
        /// en X :  20 centimetros
        /// en Y :  20 centimetros
        /// en Z :  20 centimetros 
        /// </summary>
        /// <returns></returns>
        public bool EstabilidadIzquierda()
        {
            float size = (float)0.2;

            float XmanoL = lHand.Position.X;
            float YmanoL = lHand.Position.Y;
            float ZmanoL = lHand.Position.Z;

            //Control en el eje X
            //Si la mano derecha esta a la derecha del centro MENOS 20 centimentros Y
            //Si la mano derecha esta a la izquierda del centro MAS 20 centimientros
            //SI existe equilibro en el ejex X
            if (XmanoL > centroXL - size && XmanoL < centroXL + size)
            {
                //Control en el eje Y
                //Si la mano derecha esta arriba del centro MENOS 20 centimetros Y
                //Si la mano derecha esta abajo del centro MAS 20 centimetros
                //Si existe equilibrio en el eje Y
                if (YmanoL > centroYL - size && YmanoL < centroYL + size)
                {
                    //Control en el eje Z
                    //Si la mano derecha esta mas atras del centro MENOS 20 centimetros
                    //Si la mano derecha esta mas adelante del centro MAS 20 centimetros
                    //Si existe equilibro en el eje Z
                    if (ZmanoL > centroZL - size && ZmanoL < centroZL + size)
                    {
                        return true;
                    }
                    //Si no existe equilibro en el eje Z
                    else
                    {
                        // MessageBox.Show("Equilibrio");
                        return false;
                    }
                }
                //Si no existe equilibrio en el eje Y
                else
                {
                    return false;
                }
            }
            //Si no existe equilibrio en el eje X
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Funcion que controla que la mano este dentro dentro de una posicion
        /// en relacion al centro
        /// </summary>
        /// <param name="maxX">Distancia maxima desde el centro hacia la derecha</param>
        /// <param name="minX">Distancia minima desde el centro hacia la derecha</param>
        /// <param name="maxY">Distancia maxima desde el centro hacia abajo</param>
        /// <param name="minY">Distancia minima desde el centro hacia abajo</param>
        /// <param name="maxZ">Profundidad maxima desde el centro </param>
        /// <param name="minZ">Profundidad mimina desde el centro</param>
        /// <returns>Retorna verdadero si la mano derecha esta dentro de los intervalos enviados</returns>
        public bool controlDerecha(double maxX, double minX, double maxY, double minY, double maxZ, double minZ)
        {
            float posX = rHand.Position.X;
            float posY = rHand.Position.Y;
            float posZ = rHand.Position.Z;

            if (posZ < centroZR + maxZ && posZ > centroZR + minZ)
                if (posX < centroXR + maxX && posX > centroXR + minX)
                    if (posY < centroYR + maxY && posY > centroYR + minY)
                        return true;
                    else
                        return false;
            return false;
        }

        /// <summary>
        /// Funcion que controla que la mano este dentro dentro de una posicion
        /// en relacion al centro
        /// </summary>
        /// <param name="maxX">Distancia maxima desde el centro hacia la derecha</param>
        /// <param name="minX">Distancia minima desde el centro hacia la derecha</param>
        /// <param name="maxY">Distancia maxima desde el centro hacia abajo</param>
        /// <param name="minY">Distancia minima desde el centro hacia abajo</param>
        /// <param name="maxZ">Profundidad maxima desde el centro </param>
        /// <param name="minZ">Profundidad mimina desde el centro</param>
        /// <returns>Retorna verdadero si la mano derecha esta dentro de los intervalos enviados</returns>
        public bool controlIzquierda(double maxX, double minX, double maxY, double minY, double maxZ, double minZ)
        {
            float posX = lHand.Position.X;
            float posY = lHand.Position.Y;
            float posZ = lHand.Position.Z;

            if (posZ < centroZL + maxZ && posZ > centroZL + minZ)
                if (posX < centroXL + maxX && posX > centroXL + minX)
                    if (posY < centroYL + maxY && posY > centroYL + minY)
                        return true;
                    else
                        return false;
            return false;
        }

    }
}