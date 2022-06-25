USE [PresupuestosDB]
GO

/****** Object:  StoredProcedure [dbo].[TIPOCUENTA_INSERTAR]    Script Date: 6/6/2022 21:52:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		MAXISANDOVAL37
-- Create date: 22-05-22
-- Description:	Asigna un n° de orden a cada 
--              tipo cuenta.
-- =============================================
CREATE PROCEDURE [dbo].[TIPOCUENTA_INSERTAR]
	--PARAMETROS DE ENTRADA:
	@nombre nvarchar(50),
	@usuario_id int
AS
BEGIN
	SET NOCOUNT ON;

	--VARIABLE INTERNA:
	DECLARE @orden int;
	--OBTENEMOS EL MAX DE orden Y LE SUMAMOS 1, EN CASO DE SER NULL HACE 0 + 1
	SELECT @orden = COALESCE(MAX(orden),0)+1 FROM tipo_cuenta WHERE usuario_id = @usuario_id

	INSERT INTO tipo_cuenta(nombre,usuario_id,orden) VALUES (@nombre,@usuario_id,@orden);
	
	--POR ULTIMO DEVOLVEMOS EL ID DEL TIPO_CUENTA CREADO
	SELECT SCOPE_IDENTITY();
END
GO