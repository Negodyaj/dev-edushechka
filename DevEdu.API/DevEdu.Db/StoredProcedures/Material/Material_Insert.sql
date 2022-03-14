CREATE PROCEDURE dbo.Material_Insert 
    @Content nvarchar(max),
    @Link nvarchar(200)
AS
BEGIN
    INSERT INTO dbo.Material (Content, Link)
    VALUES (@Content, @Link)
    SELECT @@IDENTITY
END