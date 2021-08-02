CREATE TABLE [Notification] (
	Id int NOT NULL IDENTITY(1,1),
	Date datetime NOT NULL,
	Text nvarchar(max) NOT NULL,
	RoleId int,
	UserId int,
	GroupId int,
	IsDeleted bit NOT NULL DEFAULT '0',
  CONSTRAINT [PK_NOTIFICATION] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
go

ALTER TABLE [Notification] WITH CHECK ADD CONSTRAINT [Notification_fk0] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Notification] CHECK CONSTRAINT [Notification_fk0]
GO
ALTER TABLE [Notification] WITH CHECK ADD CONSTRAINT [Notification_fk1] FOREIGN KEY ([RoleId]) REFERENCES [Role]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Notification] CHECK CONSTRAINT [Notification_fk1]
GO