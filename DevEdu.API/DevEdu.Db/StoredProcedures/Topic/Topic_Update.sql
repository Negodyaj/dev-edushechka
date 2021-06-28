CREATE PROCEDURE [dbo].[Topic_Update]
    @Id int,
    @Name nvarchar(255),
	@Duration int
AS
    UPDATE [Topic]
    SET
    [Name] = @Name,
    [Duration] = @Duration
    WHERE [Id] = @Id