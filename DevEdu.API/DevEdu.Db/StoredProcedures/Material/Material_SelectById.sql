CREATE PROCEDURE [dbo].[Material_SelectById]
@MaterialId int
AS
SELECT [Id], [Content] from [Material]
WHERE [IsDeleted] = 0 AND [Id] = @MaterialId
