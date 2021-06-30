CREATE PROCEDURE dbo.Material_Update
	@Id int,
	@Content nvarchar(max)
AS
BEGIN
	UPDATE dbo.Material
	SET
		Content = @Content
	WHERE Id = @Id
END