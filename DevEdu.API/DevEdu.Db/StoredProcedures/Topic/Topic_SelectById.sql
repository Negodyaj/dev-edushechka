CREATE PROCEDURE dbo.Topic_SelectById
	@Id int
AS
BEGIN
	SELECT 
	Id, Name, Duration,IsDeleted 
	FROM dbo.Topic
	WHERE (Id = @Id AND IsDeleted=0)
END
