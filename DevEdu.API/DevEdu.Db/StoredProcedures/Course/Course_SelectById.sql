CREATE PROCEDURE dbo.Course_SelectById
	@Id int
AS
BEGIN
	SELECT 
	Id, Name, Description, IsDeleted
	FROM dbo.Course
	WHERE (Id = @Id)
END