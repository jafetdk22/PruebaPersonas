using Microsoft.AspNetCore.Mvc;
using PruebaApi.Models;
using PruebaApi.Services;
using System.Net;

namespace PruebaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : Controller
    {
        private readonly IPersonaServices _personaService;
        public PersonaController(IPersonaServices personaService)
        {
            _personaService = personaService;
        }

        [HttpPost("Insertar")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(InsertarPersonaResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> InsertarPersona([FromBody] InsertarPersonaRequest request)
        {
            if (request == null)
            {
                return BadRequest("Los datos de la persona no son válidos.");
            }

            var response = await Task.Run(() => _personaService.InsertarPersona(request));

            if (response.Tipo == "Éxito")
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPut("Actualizar")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ActualizarPersonaResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ActualizarPersona([FromBody] ActualizarPersonaRequest request)
        {
            if (request == null || request.Id <= 0)
            {
                return BadRequest("Los datos de la persona no son válidos.");
            }

            var response = await Task.Run(() => _personaService.ActualizarPersona(request));

            if (response.Tipo == "Éxito")
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpDelete("Eliminar")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(EliminarPersonaResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EliminarPersona(int id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest("Los datos de la persona no son válidos.");
            }

            var response = await Task.Run(() => _personaService.EliminarPersona(id));

            if (response.Tipo == "Éxito")
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("ListarPorFecha")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(List<PersonaResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ListarPersonasPorFecha([FromQuery] PersonaRequest request)
        {
            try
            {
                // Llamar al servicio para obtener las personas por fecha
                var result = await _personaService.ListarPersonasPorFecha(request);

                // Filtrar los registros donde el Id es 0
                var filteredResult = result.Where(persona => persona.Id != 0).ToList();

                // Retornar la respuesta del servicio
                if (filteredResult.Count == 0 || filteredResult[0].Tipo == "Error")
                {
                    return NotFound(result); // No se encontraron registros
                }

                return Ok(filteredResult); // Retorna los registros encontrados
            }
            catch (Exception ex)
            {
                // Manejar errores en caso de que falle la consulta
                var errorResponse = new List<PersonaResponse>{
            new PersonaResponse
            {
                Tipo = "Error",
                Mensaje = $"Ocurrió un error al listar las personas: {ex.Message}"
            }
        };

                return StatusCode(500, errorResponse); // Error interno del servidor
            }
        }

    }
}
