CREATE PROCEDURE [dbo].[Comment_SelectById]
	@Id int
AS
	SELECT * FROM [Comment]
	WHERE ([Id] = @Id)