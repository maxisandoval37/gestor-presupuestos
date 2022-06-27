USE [PresupuestosDB]
GO

/****** Object:  StoredProcedure [dbo].[TRANSACCION_INSERTAR]    Script Date: 25/6/2022 13:11:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ===============================================
-- Author:		MAXISANDOVAL37
-- Create date: 25-06-22
-- Description:	Inserta una transaccion y 
--              actualiza el balance de la cuenta.
-- ===============================================
CREATE PROCEDURE [dbo].[TRANSACCION_INSERTAR]
	--PARAMETROS DE ENTRADA:
	@usuario_id int,
	@fecha_transaccion date,
	@monto decimal (18,2),
	@nota nvarchar (1000) = NULL,
	@cuenta_id int,
	@categoria_id int
	
AS
BEGIN
	SET NOCOUNT ON;

	--Insertarmos el monto como un valor positivo (absoluto)
	INSERT INTO transacciones(usuario_id,fecha_transaccion,monto,nota,cuenta_id,categoria_id) 
	VALUES (@usuario_id, @fecha_transaccion, ABS(@monto), @nota, @cuenta_id, @categoria_id);
	
	UPDATE cuentas set balance += @monto where id = @cuenta_id

	--POR ULTIMO DEVOLVEMOS EL ID DE LA TRANSACCION CREADA
	SELECT SCOPE_IDENTITY();
END
GO