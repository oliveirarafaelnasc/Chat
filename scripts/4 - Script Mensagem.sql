if object_id('dbo.Mensagem','U') is null
begin

	CREATE TABLE [dbo].[Mensagem]
	(
		[Id_Mensagem] [int] IDENTITY(1,1) NOT NULL,
		[Particular] [bit] not null,
		[Id_Grupo] [int] null,
		[Id_Usuario] [uniqueidentifier] NOT NULL,
		[Id_Usuario_Remetente] [uniqueidentifier] NULL,
		[Id_Usuario_Destino] [uniqueidentifier] NULL,
		[Texto_Mensagem] [varchar](2000) not null,
		[Pendente] [bit] not null,
		[Data_Criacao] [datetime] NOT NULL,
	
	 CONSTRAINT [PK_Mensagem] PRIMARY KEY CLUSTERED 
	(
		[Id_Mensagem] ASC
	))ON [PRIMARY]

	
	ALTER TABLE [dbo].[Mensagem]  WITH CHECK ADD  CONSTRAINT [FK_Mensagem_Grupo] FOREIGN KEY(Id_Grupo)
	REFERENCES [dbo].[Grupo] (Id_Grupo)


	ALTER TABLE [dbo].[Mensagem] CHECK CONSTRAINT [FK_Mensagem_Grupo]
	
	ALTER TABLE [dbo].[Mensagem]  WITH CHECK ADD  CONSTRAINT [FK_Mensagem_Usuario] FOREIGN KEY(Id_Usuario)
	REFERENCES [dbo].[Usuario] (Id_Usuario)


	ALTER TABLE [dbo].[Mensagem] CHECK CONSTRAINT [FK_Mensagem_Usuario]

	ALTER TABLE [dbo].[Mensagem]  WITH CHECK ADD  CONSTRAINT [FK_Mensagem_Usuario_Remetente] FOREIGN KEY(Id_Usuario_Remetente)
	REFERENCES [dbo].[Usuario] (Id_Usuario)


	ALTER TABLE [dbo].[Mensagem] CHECK CONSTRAINT [FK_Mensagem_Usuario_Remetente]

	ALTER TABLE [dbo].[Mensagem]  WITH CHECK ADD  CONSTRAINT [FK_Mensagem_Usuario_Destino] FOREIGN KEY(Id_Usuario_Destino)
	REFERENCES [dbo].[Usuario] (Id_Usuario)


	ALTER TABLE [dbo].[Mensagem] CHECK CONSTRAINT [FK_Mensagem_Usuario_Destino]
end

