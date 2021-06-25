CREATE TABLE [Lesson] (
	Id int NOT NULL Identity,
	Date datetime NOT NULL,
	TeacherComment nvarchar(500),
	TeacherId int NOT NULL,
	IsDeleted bit NOT NULL DEFAULT '0',
  CONSTRAINT [PK_LESSON] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
go

ALTER TABLE [Lesson] WITH CHECK ADD CONSTRAINT [Lesson_fk0] FOREIGN KEY ([TeacherId]) REFERENCES [User]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Lesson] CHECK CONSTRAINT [Lesson_fk0]
GO