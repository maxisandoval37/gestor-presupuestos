USE [PresupuestosDB]
GO

/****** Object:  Table [dbo].[cuentas]    Script Date: 6/6/2022 21:43:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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
GO

ALTER TABLE [dbo].[cuentas]  WITH CHECK ADD  CONSTRAINT [FK_cuentas_tipos_cuentas] FOREIGN KEY([tipo_cuenta_id])
REFERENCES [dbo].[tipo_cuenta] ([id])
GO

ALTER TABLE [dbo].[cuentas] CHECK CONSTRAINT [FK_cuentas_tipos_cuentas]
GO