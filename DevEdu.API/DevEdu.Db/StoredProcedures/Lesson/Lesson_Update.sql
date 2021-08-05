CREATE PROCEDURE [dbo].[Lesson_Update]
    @Id int,
    @TeacherComment nvarchar(255),
    @LinkToRecord nvarchar(150),
    @Date datetime
AS
BEGIN
    UPDATE dbo.Lesson
    SET
    TeacherComment = @TeacherComment, LinkToRecord = @LinkToRecord, Date = @Date
    WHERE Id = @Id
END