CREATE PROCEDURE [dbo].[Tag_SelectAll]
AS
	SELECT * from [dbo].[Tag]
	WHERE IsDeleted = 0
