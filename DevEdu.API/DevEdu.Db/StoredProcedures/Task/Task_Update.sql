CREATE PROCEDURE [dbo].[Task_Update]
	@Id int,
	@Name nvarchar(255),
	@Description nvarchar(500),
	@Links nvarchar(500),
	@IsRequired bit
AS
BEGIN
	UPDATE [Task] 
	SET 
	[Name] = @Name,
	[Description] = @Description,
	[Links] = @Links,
	[IsRequired] = @IsRequired
	Where Id = @Id
END