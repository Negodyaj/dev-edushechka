CREATE PROCEDURE [dbo].[Tag_Delete]
	@Id int
AS
	UPDATE [Tag] 
	SET 
	IsDeleted = 1
	Where Id = @Id