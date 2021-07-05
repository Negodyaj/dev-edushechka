CREATE PROCEDURE dbo.Tag_Material_Insert
	@TagId int ,
	@MaterialId int
AS
	Insert into dbo.Tag_Material (TagId,MaterialId)
	Values (@TagId, @MaterialId )