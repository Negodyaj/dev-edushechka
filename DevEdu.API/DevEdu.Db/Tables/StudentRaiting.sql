CREATE TABLE [dbo].[StudentRaiting]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [UserID] INT NOT NULL, 
    [GroupId] INT NOT NULL, 
    [RaitingTypeID] INT NOT NULL, 
    [Raiting] INT NOT NULL,
    [ReportingPeriodNumber] INT NOT NULL,
    CONSTRAINT [PK_StudentRaiting] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF) 
     
)
GO

ALTER TABLE [dbo].[StudentRaiting] WITH CHECK ADD CONSTRAINT [StudentRaiting_fk0] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [dbo].[StudentRaiting] CHECK CONSTRAINT [StudentRaiting_fk0]
GO
ALTER TABLE [dbo].[StudentRaiting] WITH CHECK ADD CONSTRAINT [StudentRaiting_fk1] FOREIGN KEY ([RaitingTypeId]) REFERENCES [RaitingType]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [dbo].[StudentRaiting] CHECK CONSTRAINT [StudentRaiting_fk1]
GO
ALTER TABLE [dbo].[StudentRaiting] WITH CHECK ADD CONSTRAINT [StudentRaiting_fk2] FOREIGN KEY ([GroupId]) REFERENCES [Group]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [dbo].[StudentRaiting] CHECK CONSTRAINT [StudentRaiting_fk2]
GO
ALTER TABLE [dbo].[StudentRaiting] ADD CONSTRAINT raiting_check CHECK(raiting >= 1 and raiting <= 100)
GO
ALTER TABLE [dbo].[StudentRaiting]
ADD CONSTRAINT UC_UserId_GroupId_RaitingTypeId_ReportingPeriodNumber  UNIQUE(UserId, GroupId, RaitingTypeId, ReportingPeriodNumber)
GO
