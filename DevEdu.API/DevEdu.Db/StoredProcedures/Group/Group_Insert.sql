CREATE PROCEDURE dbo.Group_Insert
	@Name nvarchar(50),
    @CourseId int,
    @StartDate date,
    @EndDate date,
	@Timetable nvarchar(500),
    @PaymentPerMonth decimal(6,2) 
AS
BEGIN
	declare @groupStatusRecruting int = 1
	INSERT INTO dbo.[Group] ([Name], CourseId, GroupStatusId, StartDate, EndDate, PaymentPerMonth, Timetable)
	VALUES (@Name, @CourseId, @groupStatusRecruting, @EndDate, @StartDate, @PaymentPerMonth, @Timetable)
	SELECT @@IDENTITY
END