CREATE PROCEDURE dbo.StudentRaiting_Insert
	@UserID int,
	@RaitingTypeID int,
	@Raiting int
AS
BEGIN
	INSERT INTO dbo.StudentRaiting (UserID, RaitingTypeID, Raiting)
	VALUES (@UserID, @RaitingTypeID, @Raiting)
	SELECT @@IDENTITY
END
