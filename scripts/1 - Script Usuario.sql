if object_id('dbo.Usuario','U') is null
begin

	CREATE TABLE [dbo].[Usuario]
	(
		[Id_Usuario] [uniqueidentifier] NOT NULL,
		[Email] [varchar](500),
		[NomeUsuario] [varchar](150) NOT NULL,
		[Nome] [varchar](150) NOT NULL,
		[Senha] [varchar](150) NOT NULL
	
	 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
	(
		[Id_Usuario] 
	))ON [PRIMARY]
end
