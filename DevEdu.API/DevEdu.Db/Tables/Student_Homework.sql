CREATE TABLE [Student_Homework] (
	Id int NOT NULL IDENTITY(1,1),
	HomeworkId int NOT NULL,
	StudentId int NOT NULL,
	StatusId int NOT NULL,
	Answer nvarchar(500),
	CompletedDate datetime NULL,
	Rating int NULL,
	IsDeleted bit NOT NULL DEFAULT '0',
  CONSTRAINT [PK_TASK_STUDENT] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
go

ALTER TABLE [Student_Homework] WITH CHECK ADD CONSTRAINT [Student_Homework_fk0] FOREIGN KEY ([HomeworkId]) REFERENCES [Homework]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Student_Homework] CHECK CONSTRAINT [Student_Homework_fk0]
GO
ALTER TABLE [Student_Homework] WITH CHECK ADD CONSTRAINT [Student_Homework_fk1] FOREIGN KEY ([StudentId]) REFERENCES [User]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Student_Homework] CHECK CONSTRAINT [Student_Homework_fk1]
GO
ALTER TABLE [Student_Homework] WITH CHECK ADD CONSTRAINT [Student_Homework_fk2] FOREIGN KEY ([StatusId]) REFERENCES [Student_Homework_Status]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Student_Homework] CHECK CONSTRAINT [Student_Homework_fk2]
GO
ALTER TABLE [dbo].[Student_Homework] ADD CONSTRAINT UC_TaskId_StudentId UNIQUE([HomeworkId], [StudentId])
GO