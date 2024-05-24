using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.IO;

namespace pryPedidosPrograma2
{
    internal class clsBaseDatos
    {
        OleDbConnection conexion;
        OleDbCommand comando;
        OleDbDataReader lectorBD;

        OleDbDataAdapter adaptador;
        DataSet objDS;

        string rutaArchivo;
        public string estadoConexion;

        public clsBaseDatos()
        {
            try
            {
                rutaArchivo = @"\Downloads\pryPedidosPrograma2\bin\Debug";

                conexion = new OleDbConnection();
                conexion.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + rutaArchivo;
                conexion.Open();

                objDS = new DataSet();

                estadoConexion = "Conectado";
            }
            catch (Exception error)
            {
                estadoConexion = error.Message;
            }
        }

        public void RegistroLogInicioSesion()
        {
            try
            {
                comando = new OleDbCommand();

                comando.Connection = conexion;
                comando.CommandType = System.Data.CommandType.TableDirect;
                comando.CommandText = "Logs";

                adaptador = new OleDbDataAdapter(comando);

                adaptador.Fill(objDS, "Logs");

                DataTable objTabla = objDS.Tables["Logs"];
                DataRow nuevoRegistro = objTabla.NewRow();

                nuevoRegistro["Categoria"] = "Inicio Sesión";
                nuevoRegistro["FechaHora"] = DateTime.Now;
                nuevoRegistro["Descripcion"] = "Inicio exitoso";

                objTabla.Rows.Add(nuevoRegistro);

                OleDbCommandBuilder constructor = new OleDbCommandBuilder(adaptador);
                adaptador.Update(objDS, "Logs");

                estadoConexion = "Registro exitoso de log";
            }
            catch (Exception error)
            {

                estadoConexion = error.Message;
            }

        }

        public void ValidarUsuario(string nombreUser, string passUser)
        {
            try
            {
                // Open the connection explicitly before using the reader
                conexion.Open();

                comando = new OleDbCommand();
                comando.Connection = conexion;
                comando.CommandType = System.Data.CommandType.TableDirect;
                comando.CommandText = "Usuario";

                lectorBD = comando.ExecuteReader();

                if (lectorBD.HasRows)
                {
                    while (lectorBD.Read())
                    {
                        if (lectorBD[1].ToString() == nombreUser && lectorBD[2].ToString() == passUser)
                        {
                            estadoConexion = "Usuario EXISTE";
                        }
                    }
                }

                // Close the reader and connection after use (important!)
            }
            catch (Exception error)
            {
                estadoConexion = error.Message;
            }
            finally
            {
                if (lectorBD != null)
                    lectorBD.Close();
                if (conexion != null && conexion.State == ConnectionState.Open)
                    conexion.Close();
            }
        }
        public void RegistrarLog(string categoria, string descripcion)
        {
            try
            {
                conexion.Open();
                string query = "INSERT INTO log (categoria, fecha_hora, descripcion) VALUES (?, ?, ?)";
                comando = new OleDbCommand(query, conexion);
                comando.Parameters.AddWithValue("@categoria", categoria);
                comando.Parameters.AddWithValue("@fecha_hora", DateTime.Now);
                comando.Parameters.AddWithValue("@descripcion", descripcion);

                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conexion.Close();
            }
        }
        public DataTable ObtenerLogs()
        {
            DataTable logs = new DataTable();
            try
            {
                conexion.Open();
                string query = "SELECT * FROM log";
                adaptador = new OleDbDataAdapter(query, conexion);
                adaptador.Fill(logs);
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conexion.Close();
            }
            return logs;
        }
    }



}
