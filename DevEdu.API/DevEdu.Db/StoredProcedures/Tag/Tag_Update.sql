CREATE PROCEDURE [dbo].[Tag_Update]
	@Id int,
	@Name nvarchar(50)
AS
BEGIN
	UPDATE [dbo].[Tag] 
	SET 
	Name = @Name
	Where Id = @Id
END
