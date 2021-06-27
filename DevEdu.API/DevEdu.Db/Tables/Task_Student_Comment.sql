CREATE TABLE [Task_Student_Comment] (
	Id int NOT NULL IDENTITY(1,1),
	TaskStudentId int NOT NULL,
	CommentId int NOT NULL,
  CONSTRAINT [PK_TASK_STUDENT_COMMENT] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
go

ALTER TABLE [Task_Student_Comment] WITH CHECK ADD CONSTRAINT [Task_Student_Comment_fk0] FOREIGN KEY ([TaskStudentId]) REFERENCES [Task_Student]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Task_Student_Comment] CHECK CONSTRAINT [Task_Student_Comment_fk0]
GO
ALTER TABLE [Task_Student_Comment] WITH CHECK ADD CONSTRAINT [Task_Student_Comment_fk1] FOREIGN KEY ([CommentId]) REFERENCES [Comment]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Task_Student_Comment] CHECK CONSTRAINT [Task_Student_Comment_fk1]
GO