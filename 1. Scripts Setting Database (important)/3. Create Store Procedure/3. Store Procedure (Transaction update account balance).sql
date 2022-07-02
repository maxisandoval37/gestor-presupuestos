USE [PresupuestosDB]
GO

/****** Object:  StoredProcedure [dbo].[TRANSACCION_ACTUALIZAR]    Script Date: 02/07/2022 18:20:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ===============================================
-- Author:		MAXISANDOVAL37
-- Create date: 25-06-22
-- Description:	Proceso destionado a la 
-- actualización de transacciones
-- ===============================================
CREATE PROCEDURE [dbo].[TRANSACCION_ACTUALIZAR]
	--PARAMETROS DE ENTRADA:
	@id int,
	@fecha_transaccion datetime,
	@monto decimal(18,2),
	@monto_anterior decimal(18,2),
	@cuenta_id int,
	@cuenta_id_anterior int,
	@categoria_id int,
	@nota nvarchar (1000) = NULL
	
AS
BEGIN
	SET NOCOUNT ON;

	--REVERTIMOS LA TRANSACCION ANTERIOR
	UPDATE cuentas SET balance -= @monto_anterior WHERE id = @cuenta_id_anterior;

	--REALIZAR NUEVA TRANSACCION
	UPDATE cuentas SET balance += @monto WHERE id = @cuenta_id;

	UPDATE transacciones SET monto = ABS(@monto), fecha_transaccion = @fecha_transaccion, categoria_id = @categoria_id, cuenta_id = @cuenta_id 
	WHERE id = @id;

END
GO