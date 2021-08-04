CREATE PROCEDURE [dbo].[Lesson_Update]
    @Id int,
    @TeacherComment nvarchar(255),
    @LinkToRecord nvarchar(150)
AS
BEGIN
    UPDATE dbo.Lesson
    SET
    TeacherComment = @TeacherComment, LinkToRecord = @LinkToRecord
    WHERE Id = @Id
END