CREATE PROCEDURE [dbo].[Tag_Delete]
	@Id int
AS
BEGIN
	UPDATE [dbo].[Tag] 
	SET 
	IsDeleted = 1
	Where Id = @Id
END