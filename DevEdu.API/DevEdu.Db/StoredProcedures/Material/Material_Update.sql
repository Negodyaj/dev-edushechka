CREATE PROCEDURE dbo.Material_Update 
    @Id int,
    @Content nvarchar(max),
    @Link nvarchar(200)
AS
BEGIN
    UPDATE dbo.Material
    SET Content = @Content,
        Link    = @Link
    WHERE Id = @Id
END