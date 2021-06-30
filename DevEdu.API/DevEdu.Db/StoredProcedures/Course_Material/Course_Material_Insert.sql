CREATE PROCEDURE  dbo.Course_Material_Insert
	@CourseId int,
	@MaterialId int
AS	
BEGIN
	INSERT INTO dbo.Course_Material (CorseId, MaterialId)
	VALUES (@CorseId,  @MaterialId)  
END