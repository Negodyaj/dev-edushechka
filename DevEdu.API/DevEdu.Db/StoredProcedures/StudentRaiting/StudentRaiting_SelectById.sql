CREATE PROCEDURE dbo.StudentRaiting_SelectById
	@Id int
	AS
BEGIN
	SELECT 
	sr.Id,
	sr.Raiting,
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
	u.Patronymic,
	u.Email,
	u.Username,
	u.RegistrationDate,
	u.ContractNumber,
	u.BirthDate, 
	u.GitHubAccount,
	u.Photo,
	u.PhoneNumber, 
	u.ExileDate, 
	u.CityId,
	ug.RoleId as Id
	from dbo.StudentRaiting sr
	left join [dbo].[Group] g on sr.GroupId = g.Id
	left join dbo.RaitingType rt on sr.RaitingTypeID = rt.Id
	left join [dbo].[User] u on sr.UserID = u.Id
	left join dbo.User_Group ug on ug.UserId = u.Id and ug.GroupId = g.Id
	WHERE sr.Id = @Id
	AND u.IsDeleted = 0
	AND g.IsDeleted = 0
END
