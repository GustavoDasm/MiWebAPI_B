
using MyWebAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace MyWebAPI.Data
{
    public class EmpleadoData
    {
        //La palabra clave readonly indica que esta variable solo puede ser asignada una vez (generalmente en el constructor)
        private readonly string conexion;

        public EmpleadoData(IConfiguration configuration)
        {
            //obtiene la cadena de conexión con nombre "CadenaSQL" de la configuración de la aplicación utilizando
            //el método GetConnectionString
            conexion = configuration.GetConnectionString("CadenaSQL");
        }

        //Metodo asincrono que devuelve una tarea
        public async Task<List<Empleado>> Lista()   
        {
            //Se inicializa una nueva lista vacía llamada lista, que almacenará objetos de tipo Empleado.
            List<Empleado> lista = new List<Empleado>();
            //Se crea una nueva instacia SQLConnection llamada con
            using (var con = new SqlConnection(conexion))
            {
                //Se abre la conexión a la base de datos de forma asincrónica utilizando el método OpenAsync()
                await con.OpenAsync(); //OpenAsync espera hasta que la conexión se abra antes de continuar con el siguiente código.

                //Se crea un nuevo comando SQL (SqlCommand) llamado cmd que ejecutará el procedimiento "sp_listaEmpleados" en la conexión con.
                SqlCommand cmd = new SqlCommand("sp_listaEmpleados", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Empleado
                        {
                            IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                            NombreCompleto = reader["NombreCompleto"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            Sueldo = Convert.ToDecimal(reader["Sueldo"]),
                            FechaContrato = reader["FechaContrato"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<Empleado> Obtener(int Id)
        {
            Empleado objeto = new Empleado();
            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerEmpleado", con);
                cmd.Parameters.AddWithValue("@IdEmpleado", Id);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        objeto = new Empleado
                        {
                            IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                            NombreCompleto = reader["NombreCompleto"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            Sueldo = Convert.ToDecimal(reader["Sueldo"]),
                            FechaContrato = reader["FechaContrato"].ToString()
                        };
                    }
                }
            }
            return objeto;
        }

        public async Task<bool> Crear(Empleado objeto)
        {
            bool respuesta = true;
            using (var con = new SqlConnection(conexion))
            {

                SqlCommand cmd = new SqlCommand("sp_crearEmpleado", con);
                cmd.Parameters.AddWithValue("@NombreCompleto", objeto.NombreCompleto);
                cmd.Parameters.AddWithValue("@Correo", objeto.Correo);
                cmd.Parameters.AddWithValue("@Sueldo", objeto.Sueldo);
                cmd.Parameters.AddWithValue("@FechaContrato", objeto.FechaContrato);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> Editar(Empleado objeto)
        {
            bool respuesta = true;
            using (var con = new SqlConnection(conexion))
            {

                SqlCommand cmd = new SqlCommand("sp_editarEmpleado", con);
                cmd.Parameters.AddWithValue("@IdEmpleado", objeto.IdEmpleado);
                cmd.Parameters.AddWithValue("@NombreCompleto", objeto.NombreCompleto);
                cmd.Parameters.AddWithValue("@Correo", objeto.Correo);
                cmd.Parameters.AddWithValue("@Sueldo", objeto.Sueldo);
                cmd.Parameters.AddWithValue("@FechaContrato", objeto.FechaContrato);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> Eliminar(int id)
        {
            bool respuesta = true;
            using (var con = new SqlConnection(conexion))
            {

                SqlCommand cmd = new SqlCommand("sp_eliminarEmpleado", con);
                cmd.Parameters.AddWithValue("@IdEmpleado", id);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
    }
}
