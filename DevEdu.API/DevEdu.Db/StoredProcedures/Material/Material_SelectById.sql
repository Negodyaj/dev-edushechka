CREATE PROCEDURE dbo.Material_SelectById
	@MaterialId int
AS
BEGIN
	SELECT Id, Content, IsDeleted from dbo.Material
	WHERE Id = @MaterialId
END