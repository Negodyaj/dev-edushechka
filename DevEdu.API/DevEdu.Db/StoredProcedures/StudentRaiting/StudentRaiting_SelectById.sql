CREATE PROCEDURE dbo.StudentRaiting_SelectById
	@Id int
	AS
BEGIN
	SELECT sr.Id, sr.Raiting, rt.Id, rt.Name, rt.Weight, u.Id, u.FirstName, u.LastName, u.Patronymic, u.Email, u.Username, u.RegistrationDate,
		   u.ContractNumber, u.BirthDate, u.GitHubAccount, u.Photo, u.PhoneNumber, u.ExileDate, u.CityId, ur.RoleId
	from dbo.StudentRaiting sr
	left join dbo.RaitingType rt on sr.RaitingTypeID = rt.Id
	left join [dbo].[User] u on sr.UserID = u.Id
	left join dbo.User_Role ur on u.Id = ur.UserId
	WHERE sr.Id = @Id
	AND u.IsDeleted = 0
END
