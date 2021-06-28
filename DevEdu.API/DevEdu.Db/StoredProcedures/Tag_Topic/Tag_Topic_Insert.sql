CREATE PROCEDURE dbo.Tag_Topic_Insert
	@TagId int,
	@TopicId int
AS
	INSERT INTO dbo.Tag_Topic (TagId, TopicId) 
	VALUES (@TagId, @TopicId) 