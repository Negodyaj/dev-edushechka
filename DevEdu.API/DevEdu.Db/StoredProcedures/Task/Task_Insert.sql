CREATE PROCEDURE dbo.Task_Insert
	@Name nvarchar(255),
	@Description nvarchar(500),
	@Links nvarchar(500),
	@IsRequired bit,
	@GroupId int
AS
BEGIN
	INSERT INTO dbo.Task (Name, Description, Links, IsRequired, GroupId)
	VALUES (@Name, @Description, @Links, @IsRequired, case when @GroupId = -1 then null else @GroupId end)
	SELECT @@IDENTITY
END