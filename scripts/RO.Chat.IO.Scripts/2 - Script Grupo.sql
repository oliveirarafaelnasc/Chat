if object_id('dbo.Mensagem_Grupo','U') is null
begin
	CREATE TABLE [dbo].[Grupo](
	[Id_Grupo] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	 CONSTRAINT [PK_Grupo] PRIMARY KEY CLUSTERED 
	(
		[Id_Grupo] ASC
	)) ON [PRIMARY]
end
