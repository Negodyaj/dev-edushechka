CREATE PROCEDURE dbo.Material_SelectAll
AS
BEGIN
	SELECT Id, Content from dbo.Material
	WHERE IsDeleted = 0
END

