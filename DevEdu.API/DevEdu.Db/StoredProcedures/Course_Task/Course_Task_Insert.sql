CREATE PROCEDURE  dbo.Course_Task_Insert
	@CourseId int,
	@TaskId int
AS	
BEGIN
	INSERT INTO dbo.Course_Task (CorseId, TaskId)
	Values (@TagId, @TaskId)  
END