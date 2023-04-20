SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		MAXISANDOVAL37
-- Create date: 12-01-22
-- Description:	Asigna los datos por defectos cuando 
--              se registra un nuevo usuario.
-- =============================================
CREATE PROCEDURE CREAR_DATOS_DEFECTO_USUARIO_NUEVO 
	@USUARIO_ID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @efectivo nvarchar(50) = 'Efectivo';
	DECLARE @cuentasDeBanco nvarchar(50) = 'Cuentas de Banco';
	DECLARE @tarjetas nvarchar(50) = 'Tarjetas';

	INSERT INTO tipo_cuenta (Nombre, usuario_id, Orden)
	VALUES (@efectivo, @usuario_id, 1),
	(@cuentasDeBanco, @usuario_id, 2),
	(@tarjetas, @usuario_id, 3);

	INSERT INTO cuentas (Nombre, Balance, tipo_cuenta_id)
	SELECT nombre, 0, id
	FROM tipo_cuenta
	WHERE usuario_id = @usuario_id;

	INSERT INTO categorias(Nombre, tipo_operacion_id, usuario_id)
	VALUES 
	('Libros', 2, @usuario_id),
	('Salario', 1,@usuario_id),
	('Mesada', 1, @usuario_id),
	('Comida', 2, @usuario_id)

END
GO
