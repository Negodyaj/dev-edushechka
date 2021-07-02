CREATE PROCEDURE [dbo].[Tag_SelectAll]
AS
	SELECT Id, Name 
	from [dbo].[Tag]
	WHERE IsDeleted = 0
