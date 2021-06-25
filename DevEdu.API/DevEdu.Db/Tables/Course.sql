CREATE TABLE [Course] (
	Id int NOT NULL Identity,
	Name nvarchar(255) NOT NULL UNIQUE,
	Description nvarchar(max) NOT NULL,
	IsDeleted bit NOT NULL DEFAULT '0',
  CONSTRAINT [PK_COURSE] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)