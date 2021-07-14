CREATE TABLE [dbo].[StudentRaiting]
(
	[Id] INT NOT NULL, 
    [UserID] INT NOT NULL, 
    [RaitingTypeID] INT NOT NULL, 
    [Raiting] INT NOT NULL
    CONSTRAINT [PK_StudentRaiting] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)
)
GO

ALTER TABLE [StudentRaiting] WITH CHECK ADD CONSTRAINT [StudentRaiting_fk0] FOREIGN KEY ([UserID]) REFERENCES [User]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [StudentRaiting] CHECK CONSTRAINT [StudentRaiting_fk0]
GO
ALTER TABLE [StudentRaiting] WITH CHECK ADD CONSTRAINT [StudentRaiting_fk1] FOREIGN KEY ([RaitingTypeID]) REFERENCES [RaitingType]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [StudentRaiting] CHECK CONSTRAINT [StudentRaiting_fk1]
GO
ALTER TABLE StudentRaiting WITH CHECK ADD CONSTRAINT raiting_check CHECK(raiting >= 0 and raiting <= 100)
GO
