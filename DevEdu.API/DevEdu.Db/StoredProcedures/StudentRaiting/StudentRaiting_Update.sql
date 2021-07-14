CREATE PROCEDURE dbo.StudentRaiting_Update
	@Id int,
	@Raiting int
AS
BEGIN
	UPDATE dbo.StudentRaiting 
	SET 
	Raiting = @Raiting
	Where Id = @Id
END