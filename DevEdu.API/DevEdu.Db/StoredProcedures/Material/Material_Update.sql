CREATE PROCEDURE [dbo].[Material_Update]
@Id int,
@Content nvarchar(max)
AS
UPDATE [Material]
SET
[Content] = @Content
WHERE [Id] = @Id
