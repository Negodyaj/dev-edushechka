CREATE PROCEDURE dbo.RaitingType_SelectAll
AS
	BEGIN
	SELECT Id, Name, Weight 
	from dbo.RaitingType
	WHERE IsDeleted = 0
END