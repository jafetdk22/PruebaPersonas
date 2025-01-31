using PruebaApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace PruebaApi.Services
{
    public class PersonaServices: IPersonaServices
    {
        private readonly PruebaContext _dbContext;
        private readonly string connectionString;
        public PersonaServices(PruebaContext dbContext)
        {
            _dbContext = dbContext;
            connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
        }
        public async Task<InsertarPersonaResponse> InsertarPersona(InsertarPersonaRequest request)
        {
            var response = new InsertarPersonaResponse();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_InsertarPersona", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros al procedimiento almacenado
                        cmd.Parameters.AddWithValue("@Nombres", request.Nombres);
                        cmd.Parameters.AddWithValue("@ApellidoPaterno", request.ApellidoPaterno);
                        cmd.Parameters.AddWithValue("@ApellidoMaterno", request.ApellidoMaterno);
                        cmd.Parameters.AddWithValue("@FechaNacimiento", request.FechaNacimiento);
                        cmd.Parameters.AddWithValue("@NivelEducativo", request.NivelEducativo);
                        cmd.Parameters.AddWithValue("@NumeroCelular", request.NumeroCelular);
                        cmd.Parameters.AddWithValue("@Estatus", request.Estatus);

                        conn.Open();

                        // Ejecutar el procedimiento almacenado y leer el resultado
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                response.Tipo = reader["Tipo"].ToString();
                                response.Mensaje = reader["Mensaje"].ToString();

                                // Si se insertó correctamente, recuperar el ID
                                if (response.Tipo == "Éxito")
                                {
                                    response.Id = Convert.ToInt32(reader["Id"]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Tipo = "Error";
                response.Mensaje = $"Error al insertar persona: {ex.Message}";
            }

            return response;
        }

        public async Task<List<PersonaResponse>> ListarPersonasPorFecha(PersonaRequest request)
        {
            var response = new List<PersonaResponse>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ListarPersonasPorFecha", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros solo si las fechas son proporcionadas
                        if (request.FechaInicio.HasValue)
                            cmd.Parameters.AddWithValue("@FechaInicio", request.FechaInicio.Value);
                        else
                            cmd.Parameters.AddWithValue("@FechaInicio", DBNull.Value);

                        if (request.FechaFin.HasValue)
                            cmd.Parameters.AddWithValue("@FechaFin", request.FechaFin.Value);
                        else
                            cmd.Parameters.AddWithValue("@FechaFin", DBNull.Value);

                        conn.Open();

                        // Ejecutar el procedimiento almacenado y leer los resultados
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var persona = new PersonaResponse
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Nombres = reader.IsDBNull(reader.GetOrdinal("Nombres")) ? null : reader.GetString(reader.GetOrdinal("Nombres")),
                                    ApellidoPaterno = reader.IsDBNull(reader.GetOrdinal("ApellidoPaterno")) ? null : reader.GetString(reader.GetOrdinal("ApellidoPaterno")),
                                    ApellidoMaterno = reader.IsDBNull(reader.GetOrdinal("ApellidoMaterno")) ? null : reader.GetString(reader.GetOrdinal("ApellidoMaterno")),
                                    FechaNacimiento = reader.IsDBNull(reader.GetOrdinal("FechaNacimiento")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaNacimiento")),
                                    NivelEducativo = reader.IsDBNull(reader.GetOrdinal("NivelEducativo")) ? null : reader.GetString(reader.GetOrdinal("NivelEducativo")),
                                    NumeroCelular = reader.IsDBNull(reader.GetOrdinal("NumeroCelular")) ? null : reader.GetString(reader.GetOrdinal("NumeroCelular")),
                                    Estatus = reader.GetBoolean(reader.GetOrdinal("Estatus")),
                                    FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro"))
                                };

                                response.Add(persona);
                            }

                            // Verificar si no se obtuvieron registros
                            if (response.Count == 0)
                            {
                                response.Add(new PersonaResponse
                                {
                                    Tipo = "Error",
                                    Mensaje = "No se encontraron registros."
                                });
                            }
                            else
                            {
                                response.Insert(0, new PersonaResponse
                                {
                                    Tipo = "Éxito",
                                    Mensaje = "Registros encontrados"
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Add(new PersonaResponse
                {
                    Tipo = "Error",
                    Mensaje = $"Error al consultar personas: {ex.Message}"
                });
            }

            return response;
        }

        public async Task<ActualizarPersonaResponse> ActualizarPersona(ActualizarPersonaRequest request)
        {
            var response = new ActualizarPersonaResponse();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ActualizarPersona", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros al procedimiento almacenado
                        cmd.Parameters.AddWithValue("@Id", request.Id);
                        cmd.Parameters.AddWithValue("@Nombres", request.Nombres);
                        cmd.Parameters.AddWithValue("@ApellidoPaterno", request.ApellidoPaterno);
                        cmd.Parameters.AddWithValue("@ApellidoMaterno", request.ApellidoMaterno);
                        cmd.Parameters.AddWithValue("@FechaNacimiento", request.FechaNacimiento);
                        cmd.Parameters.AddWithValue("@NivelEducativo", request.NivelEducativo);
                        cmd.Parameters.AddWithValue("@NumeroCelular", request.NumeroCelular);
                        cmd.Parameters.AddWithValue("@Estatus", request.Estatus);

                        conn.Open();

                        // Ejecutar el procedimiento almacenado y leer el resultado
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                response.Tipo = reader["Tipo"].ToString();
                                response.Mensaje = reader["Mensaje"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Tipo = "Error";
                response.Mensaje = $"Error al actualizar persona: {ex.Message}";
            }


            return response;
        }
        public async Task<EliminarPersonaResponse> EliminarPersona(int id)
        {
            var response = new EliminarPersonaResponse();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_EliminarPersonaLogica", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Solo pasamos el Id como parámetro
                        cmd.Parameters.AddWithValue("@Id", id);

                        conn.Open();

                        // Ejecutar el procedimiento almacenado y leer el resultado
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                response.Tipo = reader["Tipo"].ToString();
                                response.Mensaje = reader["Mensaje"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Tipo = "Error";
                response.Mensaje = $"Error al eliminar persona: {ex.Message}";
            }

            return response;
        }
    }
}
