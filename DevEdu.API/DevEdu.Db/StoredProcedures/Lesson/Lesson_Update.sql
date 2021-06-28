CREATE PROCEDURE [dbo].[Lesson_Update]
    @Id int,
    @TeacherComment nvarchar(255)
    -- + Date
AS
    UPDATE [Lesson]
    SET
    [TeacherComment] = @TeacherComment
    WHERE [Id] = @Id