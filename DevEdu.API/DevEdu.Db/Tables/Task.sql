CREATE TABLE [Task] (
	Id int NOT NULL Identity,
	Name nvarchar(255) NOT NULL,
	StartDate datetime NOT NULL,
	EndDate datetime NOT NULL,
	Description nvarchar(500) NOT NULL,
	Links nvarchar(500),
	IsRequired bit NOT NULL DEFAULT '0',
	IsDeleted bit NOT NULL DEFAULT '0',
  CONSTRAINT [PK_TASK] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)