CREATE PROCEDURE [dbo].[Course_SelectAll]
AS
	SELECT * FROM [Course]
	WHERE ([IsDeleted]=0)