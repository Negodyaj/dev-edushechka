CREATE PROCDURE  dbo.Course_Task_Delete
	@CourseId int,
	@TaskId int
AS	
BEGIN
	DELETE FROM dbo.Course_Task
	WHERE  CourseId=@CourseId AND TaskId=@TaskId
END