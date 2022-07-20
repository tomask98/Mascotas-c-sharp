using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient; 
namespace ABMMascotas
{
    class AccesoDatos
    {
        SqlConnection conexion;
        SqlDataReader Lector;
        SqlCommand commando;

        string CadenaConexion;

        public AccesoDatos()
        {
            CadenaConexion = @"Data Source=Tomas;Initial Catalog=Veterinaria;Integrated Security=True";
            conexion = new SqlConnection(CadenaConexion);
            commando = new SqlCommand();
        }

        private void conectar()
        {
            conexion.Open();
            commando.Connection = conexion;
            commando.CommandType= CommandType.Text;

        }
        private void desconectar()
        {
            conexion.Close();
        }
        public DataTable consultaBD(string ConsultaSQL)
        {
            DataTable tabla = new DataTable();
            conectar();
            commando.CommandText = ConsultaSQL;
            tabla.Load(commando.ExecuteReader());
            desconectar();
            return tabla;
        }
        
        public int actualizarBD(string ConsultaSQL, List<Parametro>lparametros)
        {
            int filasafectada;
            conectar();
            commando.CommandText = ConsultaSQL;
            commando.Parameters.Clear();
            foreach (Parametro P in lparametros)
            {
                commando.Parameters.AddWithValue(P.Nombre, P.Valor);
            }
            filasafectada= commando.ExecuteNonQuery();
            desconectar();

            return filasafectada;

        }



    }
}
