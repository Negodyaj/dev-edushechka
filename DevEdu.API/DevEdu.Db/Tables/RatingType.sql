CREATE TABLE [dbo].[RatingType]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [Name] NVARCHAR(255) NOT NULL, 
    [Weight] INT NOT NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0
    CONSTRAINT [PK_RatingType] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)    
)
