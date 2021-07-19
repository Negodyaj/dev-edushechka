CREATE PROCEDURE dbo.Group_SelectAll
	@Role int
AS
BEGIN
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
		inner join [User_Group] UG on UG.GroupId=G.Id
		inner join [User] U on UG.UserId=U.Id
	WHERE (U.Id = U.Id  AND UG.RoleId = @Role)
END