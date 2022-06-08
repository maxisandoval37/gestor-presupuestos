USE [PresupuestosDB]
GO

/****** Object:  Table [dbo].[tipo_cuenta]    Script Date: 6/6/2022 21:45:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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
GO

ALTER TABLE [dbo].[tipo_cuenta]  WITH CHECK ADD  CONSTRAINT [FK_tipos_cuentas_usuarios] FOREIGN KEY([usuario_id])
REFERENCES [dbo].[usuarios] ([id])
GO

ALTER TABLE [dbo].[tipo_cuenta] CHECK CONSTRAINT [FK_tipos_cuentas_usuarios]
GO