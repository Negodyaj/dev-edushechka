CREATE PROCEDURE dbo.Course_Insert
	@Name nvarchar(255),
	@Description nvarchar(max)
AS
BEGIN
	INSERT INTO dbo.Course (Name,Description)
	VALUES (@Name, @Description)
	SELECT @@IDENTITY
END