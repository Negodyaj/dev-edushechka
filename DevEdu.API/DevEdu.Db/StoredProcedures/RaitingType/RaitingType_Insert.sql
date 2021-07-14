CREATE PROCEDURE dbo.RaitingType_Insert
	@Name nvarchar(255),
	@Weight int
AS
BEGIN
	INSERT INTO dbo.RaitingType (Name, Weight)
	VALUES (@Name, @Weight)
	SELECT @@IDENTITY
END
