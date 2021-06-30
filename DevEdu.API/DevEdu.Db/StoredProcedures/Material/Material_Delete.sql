CREATE PROCEDURE dbo.Material_Delete
	@Id int
AS
BEGIN
	UPDATE dbo.Material
	SET
		IsDeleted = 1
	WHERE Id = @Id
END