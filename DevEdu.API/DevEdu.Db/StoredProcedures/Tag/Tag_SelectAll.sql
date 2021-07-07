CREATE PROCEDURE [dbo].[Tag_SelectAll]
AS
BEGIN
	SELECT Id, Name 
	from [dbo].[Tag]
	WHERE IsDeleted = 0
END