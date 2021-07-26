CREATE PROCEDURE dbo.StudentRating_SelectByUserId
		@UserId int
	AS
BEGIN
	SELECT 
	sr.Id,
	sr.Rating, 
	sr.ReportingPeriodNumber,
	g.Id, 
	g.CourseId,
	g.GroupStatusId,
	g.PaymentPerMonth, 
	g.StartDate, 
	g.Timetable,
	rt.Id, 
	rt.Name,
	rt.Weight,
	u.Id, 
	u.FirstName, 
	u.LastName,
	u.Email, 
	u.Photo,
	ug.RoleId as Id
	from dbo.StudentRating sr
	left join [dbo].[Group] g on sr.GroupId = g.Id
	left join dbo.RatingType rt on sr.RatingTypeID = rt.Id
	left join [dbo].[User] u on sr.UserID = u.Id
	left join dbo.User_Group ug on ug.UserId = u.Id and ug.GroupId = g.Id
	WHERE u.Id = @UserId
END