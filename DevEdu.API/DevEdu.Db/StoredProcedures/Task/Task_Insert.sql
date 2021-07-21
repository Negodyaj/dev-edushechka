CREATE PROCEDURE dbo.Task_Insert
	@Name nvarchar(255),
	@Description nvarchar(500),
	@Links nvarchar(500),
	@IsRequired bit
AS
BEGIN
	INSERT INTO dbo.Task (Name, Description, Links, IsRequired)
	VALUES (@Name, @Description, @Links, @IsRequired)
	SELECT @@IDENTITY
END