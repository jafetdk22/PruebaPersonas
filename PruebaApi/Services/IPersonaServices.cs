using PruebaApi.Models;

namespace PruebaApi.Services
{
    public interface IPersonaServices
    {
        Task<InsertarPersonaResponse> InsertarPersona(InsertarPersonaRequest request);
        Task<List<PersonaResponse>> ListarPersonasPorFecha(PersonaRequest request);
        Task<ActualizarPersonaResponse> ActualizarPersona(ActualizarPersonaRequest request);
        Task<EliminarPersonaResponse> EliminarPersona(int id);
    }
}
