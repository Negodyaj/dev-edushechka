CREATE PROCEDURE  dbo.Course_Task_Insert
	@CourseId int,
	@TaskId int
AS	
BEGIN
	INSERT INTO dbo.Course_Task (CourseId, TaskId)
	Values (@CourseId, @TaskId)  
END