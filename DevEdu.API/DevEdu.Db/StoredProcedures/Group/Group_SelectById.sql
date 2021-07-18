CREATE PROCEDURE dbo.Group_SelectById
	@Id int
AS
BEGIN
	DECLARE @Teacher int = 4
	DECLARE @Tutor int = 5
	DECLARE @Student int = 6
	SELECT 
		G.Id,
		G.[Name],
		G.CourseId,
		G.GroupStatusId,
		G.StartDate,
		G.Timetable,
		G.PaymentPerMonth,
		UG.RoleId as Id,
		U.Id,
		U.FirstName,
		U.LastName,
		U.Email,
		U.Photo
	FROM dbo.[Group] G
		inner join [User_Group] UG on UG.UserId=G.Id
		inner join [User] U on UG.UserId=U.Id
	WHERE (G.Id = @Id AND U.Id = U.Id  AND UG.RoleId IN (@Teacher, @Tutor, @Student))
END