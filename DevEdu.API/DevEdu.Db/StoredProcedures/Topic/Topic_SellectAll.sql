CREATE PROCEDURE [dbo].[Topic_SellectAll]
AS
	SELECT * FROM [Topic]
	WHERE ([IsDeleted]=0)