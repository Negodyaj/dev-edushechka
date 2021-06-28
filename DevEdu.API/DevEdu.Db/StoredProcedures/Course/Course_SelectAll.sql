CREATE PROCEDURE dbo.Course_SelectAll
AS
BEGIN
	SELECT 
	Id, Name, Description
	FROM dbo.Course
	WHERE (IsDeleted=0)
END