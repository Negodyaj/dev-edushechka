CREATE PROCEDURE dbo.Material_SelectAll
AS
BEGIN
	SELECT 
		m.Id, 
		m.Content
	FROM dbo.Material m 
	WHERE m.IsDeleted = 0 
END

