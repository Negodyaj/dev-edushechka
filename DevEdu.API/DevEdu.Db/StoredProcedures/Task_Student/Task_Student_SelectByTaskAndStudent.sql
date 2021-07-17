CREATE PROCEDURE dbo.Task_Student_SelectByTaskAndStudent
	@TaskId int,
	@StudentId int
AS
BEGIN
	SELECT
		tstud.Id,
		tstud.TaskId,
		tstud.StudentId,
		tstud.Answer,
		com.Id,
		com.UserID,
		com.Text,
		com.Date,
		tstud.StatusId as Id
	FROM dbo.Task_Student tstud
		LEFT JOIN dbo.Task_Student_Comment tc on tc.TaskStudentID = tstud.Id
		LEFT JOIN dbo.Comment com on com.Id = tc.CommentId
	WHERE tstud.TaskId =  @TaskId AND tstud.StudentId =  @StudentId
END