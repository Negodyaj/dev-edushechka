CREATE PROCEDURE [dbo].[Task_SelectAll]
AS
	SELECT * from [dbo].[Task]
	WHERE IsDeleted = 0