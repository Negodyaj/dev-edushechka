CREATE TABLE [Student_Homework_Status] (
	Id int NOT NULL,
	Name nvarchar(255) NOT NULL,
  CONSTRAINT [PK_TASKSTATUS] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)