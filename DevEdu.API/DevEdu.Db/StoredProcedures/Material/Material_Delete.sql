CREATE PROCEDURE [dbo].[Material_Delete]
@Id int
AS
UPDATE [Material]
SET
[IsDeleted] = 1
WHERE [Id] = @Id
