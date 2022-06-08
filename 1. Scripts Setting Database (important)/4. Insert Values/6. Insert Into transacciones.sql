USE [PresupuestosDB]
GO

--TODO Complete with data
INSERT INTO [dbo].[transacciones]
           ([usuario_id]
           ,[fecha_transaccion]
           ,[monto]
           ,[tipo_operacion_id]
           ,[nota]
           ,[cuenta_id]
           ,[categoria_id])
     VALUES
           (<usuario_id, int,>
           ,<fecha_transaccion, datetime,>
           ,<monto, decimal(18,2),>
           ,<tipo_operacion_id, int,>
           ,<nota, nchar(1000),>
           ,<cuenta_id, int,>
           ,<categoria_id, int,>)
GO