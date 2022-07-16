USE [PresupuestosDB]
GO

/****** Object:  StoredProcedure [dbo].[TRANSACCION_BORRAR]    Script Date: 16/07/2022 17:56:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ===============================================
-- Author:		MAXISANDOVAL37
-- Create date: 25-06-22
-- Description:	Actualiza el balance de la cuenta
-- y borra la transaccion.
-- ===============================================
CREATE PROCEDURE [dbo].[TRANSACCION_BORRAR]
	--PARAMETROS DE ENTRADA:
	@id int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @monto decimal(18,2);
	DECLARE @cuenta_id int;
	DECLARE @tipo_operacion_id int;

	SELECT @monto = monto, @cuenta_id = cuenta_id, @tipo_operacion_id = tipo_operacion_id --llenamos los datos de las variables declaradas
	FROM transacciones INNER JOIN categorias cat ON cat.id = transacciones.categoria_id 
	WHERE transacciones.id = @id;

	IF (@tipo_operacion_id = 2)--SI ES UN GASTO
		SET @monto = @monto * -1

	--DESCONTAMOS EL MONTO A ELIMINAR DEL BALANCE
	UPDATE cuentas set balance -= @monto WHERE id = @cuenta_id;

	--FINALMENTE BORRAMOS LA TRANSACCION
	DELETE transacciones WHERE id = @id;

END
GO