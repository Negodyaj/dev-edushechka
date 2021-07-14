CREATE PROCEDURE dbo.RaitingType_SelectById
	@Id int
AS
	BEGIN
	SELECT Id, Name, Weight 
	from dbo.RaitingType
	WHERE Id = @Id
	AND IsDeleted = 0
END
