CREATE PROCEDURE dbo.Group_SelectById
	@Id int,
	@Role int
AS
BEGIN
	SELECT 
		G.Id,
		G.Name,
		G.GroupStatusId,
		G.StartDate,
		G.Timetable,
		G.PaymentPerMonth,
		C.Id,
		C.Name,
		C.[Description],
		UG.RoleId as Id,
		U.Id,
		U.FirstName,
		U.LastName,
		U.Email,
		U.Photo
	FROM dbo.[Group] G
		inner join [Course] C on C.Id = G.CourseId
		inner join [User_Group] UG on UG.GroupId=G.Id
		inner join [User] U on UG.UserId=U.Id
	WHERE (G.Id = @Id AND U.Id = U.Id  AND UG.RoleId = @Role)
END