CREATE PROCEDURE dbo.Comment_Update
    @Id int,
	@Text nvarchar(max)
AS
BEGIN
    UPDATE dbo.Comment
    SET
        [Text] = @Text
    WHERE Id = @Id
END