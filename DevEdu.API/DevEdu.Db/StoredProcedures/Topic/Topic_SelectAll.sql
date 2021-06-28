CREATE PROCEDURE [dbo].[Topic_SelectAll]
AS
	SELECT * FROM [Topic]
	WHERE ([IsDeleted]=0)