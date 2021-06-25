CREATE TABLE [Student_Lesson] (
	Id int NOT NULL Identity,
	UserId int NOT NULL,
	LessonId int NOT NULL,
	Feedback nvarchar(500),
	IsPresent bit NOT NULL DEFAULT '0',
	AbsenceReason nvarchar(500),
  CONSTRAINT [PK_STUDENT_LESSON] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
go

ALTER TABLE [Student_Lesson] WITH CHECK ADD CONSTRAINT [Student_Lesson_fk0] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Student_Lesson] CHECK CONSTRAINT [Student_Lesson_fk0]
GO
ALTER TABLE [Student_Lesson] WITH CHECK ADD CONSTRAINT [Student_Lesson_fk1] FOREIGN KEY ([LessonId]) REFERENCES [Lesson]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Student_Lesson] CHECK CONSTRAINT [Student_Lesson_fk1]
GO