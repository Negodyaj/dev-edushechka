CREATE PROCEDURE dbo.Material_Tag_Insert
	@TagId int,
	@MaterialId int
AS
BEGIN
	Insert into dbo.Tag_Material (TagId,MaterialId)
	Values (@TagId, @MaterialId)
END
