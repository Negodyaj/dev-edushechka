CREATE PROCEDURE [dbo].[Topic_SelectById]
	@Id int
AS
	SELECT * FROM [Topic]
	WHERE ([Id] = @Id AND [IsDeleted]=0)
