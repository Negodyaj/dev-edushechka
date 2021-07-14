CREATE PROCEDURE dbo.RaitingType_Delete
	@Id int
AS
BEGIN
	UPDATE dbo.RaitingType 
	SET 
	IsDeleted = 1
	Where Id = @Id
END