USE [PresupuestosDB]
GO

INSERT INTO [dbo].[tipos_operaciones]
           ([id]
           ,[descripcion])
     VALUES
           (1,'Ingreso'),
		   (2,'Egreso')
GO