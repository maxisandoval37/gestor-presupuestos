USE [PresupuestosDB]
GO

/****** Object:  Table [dbo].[transacciones]    Script Date: 6/6/2022 21:50:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[transacciones](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[usuario_id] [int] NOT NULL,
	[fecha_transaccion] [datetime] NOT NULL,
	[monto] [decimal](18, 2) NOT NULL,
	[tipo_operacion_id] [int] NOT NULL,
	[nota] [nchar](1000) NULL,
	[cuenta_id] [int] NOT NULL,
	[categoria_id] [int] NOT NULL,
 CONSTRAINT [PK_transacciones] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[transacciones]  WITH CHECK ADD  CONSTRAINT [FK_transacciones_categorias] FOREIGN KEY([categoria_id])
REFERENCES [dbo].[categorias] ([id])
GO

ALTER TABLE [dbo].[transacciones] CHECK CONSTRAINT [FK_transacciones_categorias]
GO

ALTER TABLE [dbo].[transacciones]  WITH CHECK ADD  CONSTRAINT [FK_transacciones_cuentas] FOREIGN KEY([cuenta_id])
REFERENCES [dbo].[cuentas] ([id])
GO

ALTER TABLE [dbo].[transacciones] CHECK CONSTRAINT [FK_transacciones_cuentas]
GO

ALTER TABLE [dbo].[transacciones]  WITH CHECK ADD  CONSTRAINT [FK_transacciones_tipos_operaciones] FOREIGN KEY([tipo_operacion_id])
REFERENCES [dbo].[tipos_operaciones] ([id])
GO

ALTER TABLE [dbo].[transacciones] CHECK CONSTRAINT [FK_transacciones_tipos_operaciones]
GO

ALTER TABLE [dbo].[transacciones]  WITH CHECK ADD  CONSTRAINT [FK_transacciones_usuarios] FOREIGN KEY([usuario_id])
REFERENCES [dbo].[usuarios] ([id])
GO

ALTER TABLE [dbo].[transacciones] CHECK CONSTRAINT [FK_transacciones_usuarios]
GO
