CREATE PROCEDURE dbo.Material_Insert
	@Content nvarchar(max)
AS
BEGIN
	INSERT INTO dbo.Material (Content)
	VALUES (@Content)
	SELECT @@IDENTITY
END