using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace ARDrone.UI
{
    public class MovimientosCuboDAL
    {
        #region Metodos de recuperación

        public DataTable cargarMovimientosManoIzquierda()
        {
            NpgsqlConnection connection = new NpgsqlConnection();

            try
            {
                connection.ConnectionString = "Server=127.0.0.1; Port= 5432;" +
                                             "Database=testingkinect;" +
                                             "User Id=kinect;" +
                                             "Password=kinect;";

                string select = "SELECT Nombre , MinimoX,	MaximoX	, MinimoY , MaximoY	,MinimoZ , MaximoZ FROM Movimientos_Cubos ";

                NpgsqlCommand command = new NpgsqlCommand(select, connection);

                connection.Open();

                NpgsqlDataReader dataReader = command.ExecuteReader();

                DataTable cotizacionesTable = new DataTable();

                cotizacionesTable.Load(dataReader);

                return cotizacionesTable;


            }
            catch (NpgsqlException excepcion)
            {
                MessageBox.Show(excepcion.Message);
                return null;
            }
            catch (Exception excepcion)
            {
                MessageBox.Show(excepcion.Message);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public MovimientoCubo selectMovimientosManoIzquierda(String nombre)
        {
            NpgsqlConnection connection = new NpgsqlConnection();

            try
            {
                connection.ConnectionString = "Server=127.0.0.1; Port= 5432;" +
                                             "Database=testingkinect;" +
                                             "User Id=kinect;" +
                                             "Password=kinect;"; ;

                //string select = "SELECT Nombre, Mano, MinimoX,	MaximoX	, MinimoY , MaximoY	,MinimoZ , MaximoZ FROM Movimientos_Cubos WHERE Nombre = @nombre AND Mano like @mano ";
                string select = "SELECT * FROM Movimientos_Cubos_Izquierda WHERE Nombre = @nombre ";

                NpgsqlCommand command = new NpgsqlCommand(select, connection);

                command.Parameters.Add("@nombre", NpgsqlDbType.Varchar, 25).Value = nombre;

                connection.Open();

                NpgsqlDataReader dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    MovimientoCubo movimiento = new MovimientoCubo();

                    movimiento.Nombre = dataReader["Nombre"].ToString();
                    movimiento.MaximoX = Convert.ToDouble(dataReader["MaximoX"]);
                    movimiento.MaximoY = Convert.ToDouble(dataReader["MaximoY"]);
                    movimiento.MaximoZ = Convert.ToDouble(dataReader["MaximoZ"]);
                    movimiento.MinimoX = Convert.ToDouble(dataReader["MinimoX"]);
                    movimiento.MinimoY = Convert.ToDouble(dataReader["MinimoY"]);
                    movimiento.MinimoZ = Convert.ToDouble(dataReader["MinimoZ"]);

                    return movimiento;
                }
                else
                {
                    return null;
                }

            }
            catch (NpgsqlException excepcion)
            {
                MessageBox.Show(excepcion.Message);
                return null;
            }
            catch (Exception excepcion)
            {
                MessageBox.Show(excepcion.Message);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public MovimientoCubo selectMovimientosManoDerecha(String nombre)
        {
            NpgsqlConnection connection = new NpgsqlConnection();

            try
            {
                connection.ConnectionString = "Server=127.0.0.1; Port= 5432;" +
                                             "Database=testingkinect;" +
                                             "User Id=kinect;" +
                                             "Password=kinect;";

                string select = "SELECT * FROM Movimientos_Cubos_Derecha WHERE Nombre = @nombre  ";

                NpgsqlCommand command = new NpgsqlCommand(select, connection);

                command.Parameters.Add("@nombre", NpgsqlDbType.Varchar, 25).Value = nombre;
                connection.Open();

                NpgsqlDataReader dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    MovimientoCubo movimiento = new MovimientoCubo();

                    movimiento.Nombre = dataReader["Nombre"].ToString();
                    movimiento.MaximoX = Convert.ToDouble(dataReader["MaximoX"]);
                    movimiento.MaximoY = Convert.ToDouble(dataReader["MaximoY"]);
                    movimiento.MaximoZ = Convert.ToDouble(dataReader["MaximoZ"]);
                    movimiento.MinimoX = Convert.ToDouble(dataReader["MinimoX"]);
                    movimiento.MinimoY = Convert.ToDouble(dataReader["MinimoY"]);
                    movimiento.MinimoZ = Convert.ToDouble(dataReader["MinimoZ"]);

                    return movimiento;
                }
                else
                {
                    return null;
                }

            }
            catch (NpgsqlException excepcion)
            {
                MessageBox.Show(excepcion.Message);
                return null;
            }
            catch (Exception excepcion)
            {
                MessageBox.Show(excepcion.Message);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion


    }
}
