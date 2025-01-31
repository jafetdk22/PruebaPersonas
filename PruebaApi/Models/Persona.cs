using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaApi.Models;

public partial class Persona
{
    public int Id { get; set; }

    public string Nombres { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string ApellidoMaterno { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string NivelEducativo { get; set; } = null!;

    public string NumeroCelular { get; set; } = null!;

    public bool Estatus { get; set; }

    public DateTime? FechaRegistro { get; set; }
}
public class InsertarPersonaRequest
{
    [Required(ErrorMessage = "El campo Nombres es obligatorio.")]
    [StringLength(100, ErrorMessage = "El campo Nombres no puede exceder los 100 caracteres.")]
    public string Nombres { get; set; }

    [Required(ErrorMessage = "El campo Apellido Paterno es obligatorio.")]
    [StringLength(100, ErrorMessage = "El campo Apellido Paterno no puede exceder los 100 caracteres.")]
    public string ApellidoPaterno { get; set; }

    [Required(ErrorMessage = "El campo Apellido Materno es obligatorio.")]
    [StringLength(100, ErrorMessage = "El campo Apellido Materno no puede exceder los 100 caracteres.")]
    public string ApellidoMaterno { get; set; }

    [Required(ErrorMessage = "El campo Fecha de Nacimiento es obligatorio.")]
    [DataType(DataType.Date, ErrorMessage = "El campo Fecha de Nacimiento debe tener un formato de fecha válido.")]
    [PastDate(ErrorMessage = "La fecha de nacimiento no puede ser futura.")]
    public DateTime FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El campo Nivel Educativo es obligatorio.")]
    [StringLength(50, ErrorMessage = "El campo Nivel Educativo no puede exceder los 50 caracteres.")]
    public string NivelEducativo { get; set; }

    [Required(ErrorMessage = "El campo Número Celular es obligatorio.")]
    [Phone(ErrorMessage = "El campo Número Celular debe tener un formato de número válido.")]
    [StringLength(15, ErrorMessage = "El campo Número Celular no puede exceder los 15 caracteres.")]
    public string NumeroCelular { get; set; }

    [Required(ErrorMessage = "El campo Estatus es obligatorio.")]
    public bool Estatus { get; set; }
}
public class PastDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime dateTime)
        {
            return dateTime <= DateTime.Now;
        }
        return false;
    }
}
public class InsertarPersonaResponse
{
    public string Tipo { get; set; } 
    public string Mensaje { get; set; }
    public int? Id { get; set; }  
}

public class PersonaResponse
{
    public int Id { get; set; }
    public string Nombres { get; set; }
    public string ApellidoPaterno { get; set; }
    public string ApellidoMaterno { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string NivelEducativo { get; set; }
    public string NumeroCelular { get; set; }
    public bool Estatus { get; set; }
    public DateTime FechaRegistro { get; set; }

    // Campos adicionales para los mensajes
    public string Tipo { get; set; }
    public string Mensaje { get; set; }
}
public class PersonaRequest
{
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
}
public class ListarPersonasPorFechaResponse
{
    public string Tipo { get; set; }  
    public string Mensaje { get; set; }
    public List<Persona> Personas { get; set; }  

    public class Persona
    {
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string NivelEducativo { get; set; }
        public string NumeroCelular { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}


public class ActualizarPersonaRequest
{
    public int Id { get; set; }
    public string Nombres { get; set; }
    public string ApellidoPaterno { get; set; }
    public string ApellidoMaterno { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string NivelEducativo { get; set; }
    public string NumeroCelular { get; set; }
    public bool Estatus { get; set; }
}
public class ActualizarPersonaResponse
{
    public string Tipo { get; set; } 
    public string Mensaje { get; set; }
}
public class EliminarPersonaRequest
{
    public int Id { get; set; }
}

public class EliminarPersonaResponse
{
    public string Tipo { get; set; } 
    public string Mensaje { get; set; }
}
