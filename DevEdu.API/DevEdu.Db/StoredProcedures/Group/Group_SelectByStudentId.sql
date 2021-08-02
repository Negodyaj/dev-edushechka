CREATE PROCEDURE dbo.Group_SelectByStudentId
	@StudentId int
AS
BEGIN
	SELECT 
		ug.GroupId
	FROM dbo.User_Group ug
		inner join dbo.Student_Lesson sl on sl.Id = @StudentId
	WHERE ug.UserId = sl.UserId
END
