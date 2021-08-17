CREATE PROCEDURE [dbo].[Tag_Task_Insert]
	@TagId  int,
	@TaskId int
AS
	Insert into [dbo].[Tag_Task] ([TagId],[TaskId])
	Values (@TagId, @TaskId)