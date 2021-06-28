CREATE PROCEDURE [dbo].[User_SelectById]
	@Id int
AS
	SELECT *
	FROM [User]
	WHERE [Id] = @Id