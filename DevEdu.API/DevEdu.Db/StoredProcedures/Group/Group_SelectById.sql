CREATE PROCEDURE dbo.Group_SelectById
	@Id int
AS
BEGIN
	SELECT 
		G.Id,
		G.CourseId,
		G.GroupStatusId,
		G.StartDate,
		G.Timetable,
		G.PaymentPerMonth
	FROM dbo.[Group] G
	--	inner join [User_Group] UG on G.Id=UG.UserId
	--	inner join [User_Role] UR on G.Id=UR.Id
	--	inner join [User] U on G.Id=U.Id
	WHERE (G.Id = @Id AND c.IsDeleted=0)
END