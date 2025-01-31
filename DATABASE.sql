CREATE DATABASE Prueba;

-- =============================================
-- Tabla: Personas
-- =============================================
CREATE TABLE Personas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombres NVARCHAR(100) NOT NULL,
    ApellidoPaterno NVARCHAR(100) NOT NULL,
    ApellidoMaterno NVARCHAR(100) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    NivelEducativo NVARCHAR(50) NOT NULL,
    NumeroCelular VARCHAR(15) NOT NULL CHECK (NumeroCelular LIKE '[0-9]%' AND LEN(NumeroCelular) BETWEEN 10 AND 15),
    Estatus BIT NOT NULL DEFAULT 1, -- 1: Activo, 0: Eliminado
    FechaRegistro DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- Procedimiento: sp_InsertarPersona
-- =============================================
CREATE PROCEDURE [dbo].[sp_InsertarPersona]
    @Nombres NVARCHAR(100),
    @ApellidoPaterno NVARCHAR(100),
    @ApellidoMaterno NVARCHAR(100),
    @FechaNacimiento DATE,
    @NivelEducativo NVARCHAR(50),
    @NumeroCelular VARCHAR(15),
    @Estatus BIT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Validaciones
        IF LTRIM(RTRIM(@Nombres)) = '' OR LTRIM(RTRIM(@ApellidoPaterno)) = '' OR 
           LTRIM(RTRIM(@ApellidoMaterno)) = '' OR LTRIM(RTRIM(@NumeroCelular)) = ''
        BEGIN
            SELECT 'Error' AS Tipo, 'Todos los campos son obligatorios' AS Mensaje;
            RETURN;
        END

        IF @NumeroCelular NOT LIKE '[0-9]%' OR LEN(@NumeroCelular) < 10 OR LEN(@NumeroCelular) > 15
        BEGIN
            SELECT 'Error' AS Tipo, 'Número de celular inválido' AS Mensaje;
            RETURN;
        END

        IF @FechaNacimiento > GETDATE()
        BEGIN
            SELECT 'Error' AS Tipo, 'Fecha de nacimiento inválida' AS Mensaje;
            RETURN;
        END

        -- Insertar persona
        INSERT INTO Personas (Nombres, ApellidoPaterno, ApellidoMaterno, FechaNacimiento, NivelEducativo, NumeroCelular, Estatus)
        VALUES (@Nombres, @ApellidoPaterno, @ApellidoMaterno, @FechaNacimiento, @NivelEducativo, @NumeroCelular, @Estatus);

        SELECT 'Éxito' AS Tipo, 'Persona insertada correctamente' AS Mensaje, SCOPE_IDENTITY() AS Id;
    END TRY
    BEGIN CATCH
        SELECT 'Error' AS Tipo, ERROR_MESSAGE() AS Mensaje;
    END CATCH
END;
GO

-- =============================================
-- Procedimiento: sp_ActualizarPersona
-- =============================================
CREATE PROCEDURE [dbo].[sp_ActualizarPersona]
    @Id INT,
    @Nombres NVARCHAR(100),
    @ApellidoPaterno NVARCHAR(100),
    @ApellidoMaterno NVARCHAR(100),
    @FechaNacimiento DATE,
    @NivelEducativo NVARCHAR(50),
    @NumeroCelular VARCHAR(15),
    @Estatus BIT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Validaciones
        IF LTRIM(RTRIM(@Nombres)) = '' OR LTRIM(RTRIM(@ApellidoPaterno)) = '' OR 
           LTRIM(RTRIM(@ApellidoMaterno)) = '' OR LTRIM(RTRIM(@NumeroCelular)) = ''
        BEGIN
            SELECT 'Error' AS Tipo, 'Todos los campos son obligatorios' AS Mensaje;
            RETURN;
        END

        IF @NumeroCelular NOT LIKE '[0-9]%' OR LEN(@NumeroCelular) < 10 OR LEN(@NumeroCelular) > 15
        BEGIN
            SELECT 'Error' AS Tipo, 'Número de celular inválido' AS Mensaje;
            RETURN;
        END

        IF @FechaNacimiento > GETDATE()
        BEGIN
            SELECT 'Error' AS Tipo, 'Fecha de nacimiento inválida' AS Mensaje;
            RETURN;
        END

        -- Actualizar persona
        UPDATE Personas
        SET Nombres = @Nombres,
            ApellidoPaterno = @ApellidoPaterno,
            ApellidoMaterno = @ApellidoMaterno,
            FechaNacimiento = @FechaNacimiento,
            NivelEducativo = @NivelEducativo,
            NumeroCelular = @NumeroCelular,
            Estatus = @Estatus
        WHERE Id = @Id;

        IF @@ROWCOUNT = 0
            SELECT 'Error' AS Tipo, 'No se encontró la persona con el ID proporcionado' AS Mensaje;
        ELSE
            SELECT 'Éxito' AS Tipo, 'Persona actualizada correctamente' AS Mensaje;
    END TRY
    BEGIN CATCH
        SELECT 'Error' AS Tipo, ERROR_MESSAGE() AS Mensaje;
    END CATCH
END;
GO

-- =============================================
-- Procedimiento: sp_EliminarPersonaLogica
-- =============================================
CREATE PROCEDURE [dbo].[sp_EliminarPersonaLogica]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Personas WHERE Id = @Id)
        BEGIN
            SELECT 'Error' AS Tipo, 'No se encontró la persona con el ID proporcionado' AS Mensaje;
            RETURN;
        END

        UPDATE Personas SET Estatus = 0 WHERE Id = @Id;

        IF @@ROWCOUNT = 0
            SELECT 'Error' AS Tipo, 'No se pudo realizar la baja lógica' AS Mensaje;
        ELSE
            SELECT 'Éxito' AS Tipo, 'Persona eliminada correctamente (baja lógica)' AS Mensaje;
    END TRY
    BEGIN CATCH
        SELECT 'Error' AS Tipo, ERROR_MESSAGE() AS Mensaje;
    END CATCH
END;
GO

-- =============================================
-- Procedimiento: sp_ListarPersonasPorFecha
-- =============================================
CREATE PROCEDURE [dbo].[sp_ListarPersonasPorFecha]
    @FechaInicio DATE = NULL,
    @FechaFin DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @FechaInicio IS NULL AND @FechaFin IS NULL
    BEGIN
        SELECT * FROM Personas WHERE Estatus = 1;
        RETURN;
    END

    IF @FechaInicio > @FechaFin
    BEGIN
        SELECT 'Error' AS Tipo, 'La fecha de inicio no puede ser mayor que la de fin' AS Mensaje;
        RETURN;
    END

    -- Utilizamos >= para incluir la fecha de inicio y <= para incluir la fecha de fin
    SELECT * 
    FROM Personas 
    WHERE Estatus = 1 
      AND FechaRegistro >= @FechaInicio 
      AND FechaRegistro <= @FechaFin;
END;
GO
