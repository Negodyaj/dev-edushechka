CREATE PROCEDURE [dbo].[Course_SelectById]
	@Id int
AS
	SELECT * FROM [Course]
	WHERE ([Id] = @Id)