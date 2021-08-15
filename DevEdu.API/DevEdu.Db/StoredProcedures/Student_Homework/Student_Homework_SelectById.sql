CREATE PROCEDURE dbo.Student_Homework_SelectById
	@Id int
AS
BEGIN
	SELECT
		sh.Id,
		sh.HomeworkId,	
		sh.Answer,
		sh.CompletedDate,
		u.Id,
		u.Username,
		u.FirstName,
		u.LastName,
		u.Email,
		u.GitHubAccount,
		u.Photo,
		u.IsDeleted,
		t.Id,
		t.[Name],
		t.[Description],
		t.Links,
		t.IsRequired,
		t.IsDeleted,
		sh.StatusId as Id
	FROM Student_Homework sh
		inner join [User] u on u.Id = sh.StudentId
		inner join Homework h on h.Id = sh.HomeworkId
		inner join Task t on t.Id = h.TaskId
	WHERE sh.Id =  @Id
END