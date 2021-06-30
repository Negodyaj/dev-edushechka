CREATE PROCEDURE [dbo].[Lesson_Update]
    @Id int,
    @TeacherComment nvarchar(255),
    @Date datetime
AS
BEGIN
    UPDATE dbo.Lesson
    SET
    TeacherComment = @TeacherComment
    WHERE (Id = @Id and Date = @Date)
END