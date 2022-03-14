CREATE PROCEDURE dbo.Material_SelectAll
AS
BEGIN
    SELECT m.Id,
           m.Content,
           m.Link
    FROM dbo.Material m
    WHERE m.IsDeleted = 0
END

