CREATE PROCEDURE dbo.Course_Update
    @Id int,
    @Name nvarchar(255),
	@Description nvarchar(max)
AS
BEGIN
    UPDATE dbo.Course
    SET
        [Name] = @Name,
        Description = @Description
    WHERE Id = @Id
END