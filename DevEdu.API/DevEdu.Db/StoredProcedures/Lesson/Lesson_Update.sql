CREATE PROCEDURE [dbo].[Lesson_Update]
    @Id int,
    @LinkToRecord nvarchar(150),
    @Date datetime
AS
BEGIN
    UPDATE dbo.Lesson
    SET
    LinkToRecord = @LinkToRecord, Date = @Date
    WHERE Id = @Id
END