CREATE PROCEDURE dbo.Material_SelectById 
    @Id int
AS
BEGIN
    SELECT m.Id,
           m.CourseId,
           m.Content,
           m.Link
    FROM dbo.Material m
    WHERE m.Id = @Id
END