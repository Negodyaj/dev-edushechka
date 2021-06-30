CREATE PROCEDURE dbo.Topic_SelectAll
AS
BEGIN
	SELECT
	Id, Name, Duration,IsDeleted 
	FROM dbo.Topic
	WHERE (IsDeleted=0)
END