CREATE PROCEDURE dbo.Homework_Update
	@Id int,
    @StartDate datetime,
	@EndDate datetime
AS
BEGIN
    UPDATE dbo.Homework
    SET
    StartDate = @StartDate,
    EndDate = @EndDate
    WHERE Id = @Id
END