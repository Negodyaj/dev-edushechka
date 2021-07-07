CREATE PROCEDURE dbo.Task_SelectAll
AS
BEGIN
	SELECT 
		Id,
		Name,
		StartDate,
		EndDate,
		Description, 
		Links,
		IsRequired 
	From dbo.Task
	WHERE IsDeleted = 0
END