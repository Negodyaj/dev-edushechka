CREATE PROCEDURE [dbo].[Payment_SelectAllByUserId]
@UserId int
AS
BEGIN
	SELECT 
		p.Id, 
		p.Date, 
		p.Sum, 
		p.UserId, 
		p.IsPaid,
		p.IsDeleted,
		u.Id,
		u.FisrtName,
		u.LastName,
		u.GitHubAccount,
		u.Photo
	FROM dbo.Payment p
		inner join [User] u on u.Id=p.UserId
	WHERE (p.UserId=@UserId AND p.IsDeleted=0)
END