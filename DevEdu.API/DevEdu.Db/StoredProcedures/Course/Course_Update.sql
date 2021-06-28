CREATE PROCEDURE [dbo].[Course_Update]
    @Id int,
    @Name nvarchar(255),
	@Description nvarchar(max)
AS
    UPDATE [Course]
    SET
        [Name] = @Name,
        [Description] = @Description
    WHERE [Id] = @Id

