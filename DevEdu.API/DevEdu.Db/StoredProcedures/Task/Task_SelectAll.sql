CREATE PROCEDURE dbo.Task_SelectAll
AS
	SELECT Name, StartDate, EndDate, Description, Links, IsRequired from dbo.Task
	WHERE IsDeleted = 0