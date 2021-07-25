CREATE TABLE [Task_Student] (
	Id int NOT NULL IDENTITY(1,1),
	TaskId int NOT NULL,
	StudentId int NOT NULL,
	StatusId int NOT NULL,
	Answer nvarchar(500),
	CompletedDate date  NULL,
  CONSTRAINT [PK_TASK_STUDENT] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
go

ALTER TABLE [Task_Student] WITH CHECK ADD CONSTRAINT [Task_Student_fk0] FOREIGN KEY ([TaskId]) REFERENCES [Task]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Task_Student] CHECK CONSTRAINT [Task_Student_fk0]
GO
ALTER TABLE [Task_Student] WITH CHECK ADD CONSTRAINT [Task_Student_fk1] FOREIGN KEY ([StudentId]) REFERENCES [User]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Task_Student] CHECK CONSTRAINT [Task_Student_fk1]
GO
ALTER TABLE [Task_Student] WITH CHECK ADD CONSTRAINT [Task_Student_fk2] FOREIGN KEY ([StatusId]) REFERENCES [TaskStatus]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Task_Student] CHECK CONSTRAINT [Task_Student_fk2]
GO
ALTER TABLE [dbo].[Task_Student] ADD CONSTRAINT UC_TaskId_StudentId UNIQUE([TaskId], [StudentId])
GO