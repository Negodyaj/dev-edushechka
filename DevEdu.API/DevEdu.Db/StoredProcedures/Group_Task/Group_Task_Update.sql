CREATE PROCEDURE dbo.Group_Task_Update
	@GroupId int,
	@TaskId int,
    @StartDate datetime,
	@EndDate datetime
AS
BEGIN
    UPDATE dbo.Group_Task
    SET
    StartDate = @StartDate,
    EndDate = @EndDate
    WHERE GroupId = @GroupId AND TaskId = @TaskId
END