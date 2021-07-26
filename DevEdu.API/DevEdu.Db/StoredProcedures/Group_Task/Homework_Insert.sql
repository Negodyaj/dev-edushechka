CREATE PROCEDURE dbo.Group_Task_Insert
	@GroupId int,
	@TaskId int,
	@StartDate datetime,
	@EndDate datetime
AS
BEGIN
	INSERT INTO dbo.Homework (GroupId,TaskId,StartDate,EndDate)
	VALUES (@GroupId, @TaskId, @StartDate, @EndDate)
	SELECT @@IDENTITY
END