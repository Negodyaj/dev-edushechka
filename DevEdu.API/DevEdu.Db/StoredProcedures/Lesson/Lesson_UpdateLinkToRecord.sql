CREATE PROCEDURE [dbo].[Lesson_UpdateLinkToRecord]
    @Id int,
    @LinkToRecord nvarchar(159),
    @Date datetime
AS
BEGIN
    UPDATE dbo.Lesson
    SET
    LinkToRecord = @LinkToRecord
    WHERE (Id = @Id and Date = @Date)
END