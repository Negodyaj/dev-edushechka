CREATE PROCEDURE dbo.Topic_Update
    @Id int,
    @Name nvarchar(255),
	@Duration int
AS
BEGIN
    UPDATE dbo.Topic
    SET
    Name = @Name,
    Duration = @Duration
    WHERE Id = @Id
END