CREATE TABLE [dbo].[RaitingType]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [Name] NVARCHAR(255) NOT NULL, 
    [Weight] INT NOT NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0
    CONSTRAINT [PK_RaitingType] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)    
)
