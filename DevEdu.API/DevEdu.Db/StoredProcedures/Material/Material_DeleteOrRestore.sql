CREATE PROCEDURE dbo.Material_DeleteOrRestore
	@Id int,
	@IsDeleted bit
AS
BEGIN
	UPDATE dbo.Material
	SET
		IsDeleted = @IsDeleted
	WHERE Id = @Id
END