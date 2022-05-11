CREATE PROCEDURE dbo.Material_Insert 
    @CourseId int,    
    @Content nvarchar(max),
    @Link nvarchar(200)
AS
BEGIN
    INSERT INTO dbo.Material (CourseId, Content, Link)
    VALUES (@CourseId, @Content, @Link)
    SELECT @@IDENTITY
END