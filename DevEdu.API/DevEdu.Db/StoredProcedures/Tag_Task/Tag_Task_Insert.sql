CREATE PROCEDURE [dbo].[Tag_Task_Insert]
	@TagID int ,
	@TaskID int
AS
	Insert into [dbo].[Tag_Task] ([TagId],[TaskId])
	Values (@TagID, @TaskID)
	Select @@IDENTITY