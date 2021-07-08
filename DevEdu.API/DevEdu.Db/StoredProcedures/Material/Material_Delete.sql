CREATE PROCEDURE dbo.Material_Delete
	@Id int,
	@IsDeleted bit
AS
BEGIN
	UPDATE dbo.Material
	SET
		IsDeleted = @IsDeleted
	WHERE Id = @Id
END