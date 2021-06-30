CREATE PROCEDURE dbo.Group_Insert
    @CourseId int,
	@GroupStatusId int,
    @StartDate date,
    @PaymentPerMonth decimal(6,2) 
AS
BEGIN
	INSERT INTO dbo.[Group] (CourseId, GroupStatusId, StartDate, PaymentPerMonth)
	VALUES (@CourseId, @GroupStatusId, @StartDate, @PaymentPerMonth)
	SELECT @@IDENTITY
END