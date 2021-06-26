CREATE PROCEDURE [dbo].[Comment_Update]
    @Id int,
	@Text nvarchar(max)
AS
    UPDATE [Comment]
    SET
    [Text] = @Text
    WHERE [Id] = @Id