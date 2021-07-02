CREATE PROCEDURE [dbo].[Tag_Update]
	@Id int,
	@Name nvarchar(50)
AS
	UPDATE [Tag] 
	SET 
	Name = @Name
	Where Id = @Id
