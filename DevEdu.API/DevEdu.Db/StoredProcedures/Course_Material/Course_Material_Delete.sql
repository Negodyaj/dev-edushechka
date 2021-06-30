CREATE PROCEDURE  dbo.Course_Material_Delete
	@CourseId int,
	@MaterialId int
AS	
BEGIN
	DELETE FROM dbo.Course_Material
	WHERE  CourseId=@CourseId AND MaterialId=@MaterialId
END