CREATE PROCEDURE [dbo].[Tag_Material_Insert]
	@TagID int ,
	@MaterialID int
AS
	Insert into [dbo].[Tag_Material] ([TagId],[MaterialID])
	Values (@TagID, @MaterialID )
	Select @@IDENTITY