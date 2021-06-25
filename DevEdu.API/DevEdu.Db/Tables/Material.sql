CREATE TABLE [Material] (
	Id int NOT NULL Identity,
	Content nvarchar(max) NOT NULL,
	IsDeleted bit NOT NULL DEFAULT '0',
  CONSTRAINT [PK_MATERIAL] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)