CREATE PROCEDURE [dbo].[Material_SelectAll]
AS
SELECT [Id], [Content] from [Material]
WHERE [IsDeleted]=0

