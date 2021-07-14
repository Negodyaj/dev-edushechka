CREATE TABLE [dbo].[RaitingType]
(
	[Id] INT NOT NULL, 
    [Name] NVARCHAR(255) NOT NULL, 
    [Weight] INT NOT NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0
    CONSTRAINT [PK_RaitingType] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)    
)
