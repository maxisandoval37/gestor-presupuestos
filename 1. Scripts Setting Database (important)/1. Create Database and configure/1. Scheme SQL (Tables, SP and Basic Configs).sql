/****** Object:  Table [dbo].[categorias]    Script Date: 11/1/2023 16:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[categorias]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[categorias](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](50) NOT NULL,
	[tipo_operacion_id] [int] NOT NULL,
	[usuario_id] [int] NOT NULL,
 CONSTRAINT [PK_categorias] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[cuentas]    Script Date: 11/1/2023 16:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cuentas]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cuentas](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](50) NOT NULL,
	[tipo_cuenta_id] [int] NOT NULL,
	[balance] [decimal](18, 2) NOT NULL,
	[descripcion] [nchar](256) NULL,
 CONSTRAINT [PK_cuentas] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[tipo_cuenta]    Script Date: 11/1/2023 16:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tipo_cuenta]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tipo_cuenta](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](50) NOT NULL,
	[usuario_id] [int] NULL,
	[orden] [int] NULL,
 CONSTRAINT [PK_tipos_cuentas] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[tipos_operaciones]    Script Date: 11/1/2023 16:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tipos_operaciones]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tipos_operaciones](
	[id] [int] NOT NULL,
	[descripcion] [nvarchar](50) NULL,
 CONSTRAINT [PK_tipos_operaciones] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[transacciones]    Script Date: 11/1/2023 16:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[transacciones]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[transacciones](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[usuario_id] [int] NOT NULL,
	[fecha_transaccion] [datetime] NOT NULL,
	[monto] [decimal](18, 2) NOT NULL,
	[nota] [nchar](1000) NULL,
	[cuenta_id] [int] NOT NULL,
	[categoria_id] [int] NOT NULL,
 CONSTRAINT [PK_transacciones] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[usuarios]    Script Date: 11/1/2023 16:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usuarios]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[usuarios](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[email] [nvarchar](250) NOT NULL,
	[email_normalizado] [nchar](10) NOT NULL,
	[password_hash] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_usuarios] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_categorias_tipos_operaciones]') AND parent_object_id = OBJECT_ID(N'[dbo].[categorias]'))
ALTER TABLE [dbo].[categorias]  WITH CHECK ADD  CONSTRAINT [FK_categorias_tipos_operaciones] FOREIGN KEY([tipo_operacion_id])
REFERENCES [dbo].[tipos_operaciones] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_categorias_tipos_operaciones]') AND parent_object_id = OBJECT_ID(N'[dbo].[categorias]'))
ALTER TABLE [dbo].[categorias] CHECK CONSTRAINT [FK_categorias_tipos_operaciones]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_categorias_usuarios]') AND parent_object_id = OBJECT_ID(N'[dbo].[categorias]'))
ALTER TABLE [dbo].[categorias]  WITH CHECK ADD  CONSTRAINT [FK_categorias_usuarios] FOREIGN KEY([usuario_id])
REFERENCES [dbo].[usuarios] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_categorias_usuarios]') AND parent_object_id = OBJECT_ID(N'[dbo].[categorias]'))
ALTER TABLE [dbo].[categorias] CHECK CONSTRAINT [FK_categorias_usuarios]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cuentas_tipos_cuentas]') AND parent_object_id = OBJECT_ID(N'[dbo].[cuentas]'))
ALTER TABLE [dbo].[cuentas]  WITH CHECK ADD  CONSTRAINT [FK_cuentas_tipos_cuentas] FOREIGN KEY([tipo_cuenta_id])
REFERENCES [dbo].[tipo_cuenta] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cuentas_tipos_cuentas]') AND parent_object_id = OBJECT_ID(N'[dbo].[cuentas]'))
ALTER TABLE [dbo].[cuentas] CHECK CONSTRAINT [FK_cuentas_tipos_cuentas]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tipos_cuentas_usuarios]') AND parent_object_id = OBJECT_ID(N'[dbo].[tipo_cuenta]'))
ALTER TABLE [dbo].[tipo_cuenta]  WITH CHECK ADD  CONSTRAINT [FK_tipos_cuentas_usuarios] FOREIGN KEY([usuario_id])
REFERENCES [dbo].[usuarios] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tipos_cuentas_usuarios]') AND parent_object_id = OBJECT_ID(N'[dbo].[tipo_cuenta]'))
ALTER TABLE [dbo].[tipo_cuenta] CHECK CONSTRAINT [FK_tipos_cuentas_usuarios]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_transacciones_categorias]') AND parent_object_id = OBJECT_ID(N'[dbo].[transacciones]'))
ALTER TABLE [dbo].[transacciones]  WITH CHECK ADD  CONSTRAINT [FK_transacciones_categorias] FOREIGN KEY([categoria_id])
REFERENCES [dbo].[categorias] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_transacciones_categorias]') AND parent_object_id = OBJECT_ID(N'[dbo].[transacciones]'))
ALTER TABLE [dbo].[transacciones] CHECK CONSTRAINT [FK_transacciones_categorias]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_transacciones_cuentas]') AND parent_object_id = OBJECT_ID(N'[dbo].[transacciones]'))
ALTER TABLE [dbo].[transacciones]  WITH CHECK ADD  CONSTRAINT [FK_transacciones_cuentas] FOREIGN KEY([cuenta_id])
REFERENCES [dbo].[cuentas] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_transacciones_cuentas]') AND parent_object_id = OBJECT_ID(N'[dbo].[transacciones]'))
ALTER TABLE [dbo].[transacciones] CHECK CONSTRAINT [FK_transacciones_cuentas]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_transacciones_usuarios]') AND parent_object_id = OBJECT_ID(N'[dbo].[transacciones]'))
ALTER TABLE [dbo].[transacciones]  WITH CHECK ADD  CONSTRAINT [FK_transacciones_usuarios] FOREIGN KEY([usuario_id])
REFERENCES [dbo].[usuarios] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_transacciones_usuarios]') AND parent_object_id = OBJECT_ID(N'[dbo].[transacciones]'))
ALTER TABLE [dbo].[transacciones] CHECK CONSTRAINT [FK_transacciones_usuarios]
GO

/****** Default Insert: [dbo].[TIPOS_OPERACIONES]    Script Date: 11/1/2023 16:08:34 ******/
GO
INSERT [dbo].[tipos_operaciones] ([id], [descripcion]) VALUES (1, N'Ingreso')
GO
INSERT [dbo].[tipos_operaciones] ([id], [descripcion]) VALUES (2, N'Egreso')
GO

/****** Object:  StoredProcedure [dbo].[TIPOCUENTA_INSERTAR]    Script Date: 11/1/2023 16:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TIPOCUENTA_INSERTAR]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[TIPOCUENTA_INSERTAR] AS' 
END
GO

-- =============================================
-- Author:		MAXISANDOVAL37
-- Create date: 22-05-22
-- Description:	Asigna un n° de orden a cada 
--              tipo cuenta.
-- =============================================
ALTER PROCEDURE [dbo].[TIPOCUENTA_INSERTAR]
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
	SELECT SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[TRANSACCION_ACTUALIZAR]    Script Date: 11/1/2023 16:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TRANSACCION_ACTUALIZAR]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[TRANSACCION_ACTUALIZAR] AS' 
END
GO

-- ===============================================
-- Author:		MAXISANDOVAL37
-- Create date: 25-06-22
-- Description:	Proceso destionado a la 
-- actualización de transacciones
-- ===============================================
ALTER PROCEDURE [dbo].[TRANSACCION_ACTUALIZAR]
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
/****** Object:  StoredProcedure [dbo].[TRANSACCION_BORRAR]    Script Date: 11/1/2023 16:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TRANSACCION_BORRAR]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[TRANSACCION_BORRAR] AS' 
END
GO

-- ===============================================
-- Author:		MAXISANDOVAL37
-- Create date: 25-06-22
-- Description:	Actualiza el balance de la cuenta
-- y borra la transaccion.
-- ===============================================
ALTER PROCEDURE [dbo].[TRANSACCION_BORRAR]
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
/****** Object:  StoredProcedure [dbo].[TRANSACCION_INSERTAR]    Script Date: 11/1/2023 16:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TRANSACCION_INSERTAR]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[TRANSACCION_INSERTAR] AS' 
END
GO

-- ===============================================
-- Author:		MAXISANDOVAL37
-- Create date: 25-06-22
-- Description:	Inserta una transaccion y 
--              actualiza el balance de la cuenta.
-- ===============================================
ALTER PROCEDURE [dbo].[TRANSACCION_INSERTAR]
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
