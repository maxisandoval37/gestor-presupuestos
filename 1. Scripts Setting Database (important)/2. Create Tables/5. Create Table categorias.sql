USE [PresupuestosDB]
GO

/****** Object:  Table [dbo].[categorias]    Script Date: 6/6/2022 21:40:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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
GO

ALTER TABLE [dbo].[categorias]  WITH CHECK ADD  CONSTRAINT [FK_categorias_tipos_operaciones] FOREIGN KEY([tipo_operacion_id])
REFERENCES [dbo].[tipos_operaciones] ([id])
GO

ALTER TABLE [dbo].[categorias] CHECK CONSTRAINT [FK_categorias_tipos_operaciones]
GO

ALTER TABLE [dbo].[categorias]  WITH CHECK ADD  CONSTRAINT [FK_categorias_usuarios] FOREIGN KEY([usuario_id])
REFERENCES [dbo].[usuarios] ([id])
GO

ALTER TABLE [dbo].[categorias] CHECK CONSTRAINT [FK_categorias_usuarios]
GO