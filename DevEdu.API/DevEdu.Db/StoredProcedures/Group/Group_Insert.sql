CREATE PROCEDURE dbo.Group_Insert
    @CourseId int,
    @StartDate date,
	@Timetable nvarchar(500),
    @PaymentPerMonth decimal(6,2) 
AS
BEGIN
	declare @groupStatusRecruting int = 1
	INSERT INTO dbo.[Group] (CourseId, GroupStatusId, StartDate, PaymentPerMonth, Timetable)
	VALUES (@CourseId, @groupStatusRecruting, @StartDate, @PaymentPerMonth, @Timetable)
	SELECT @@IDENTITY
END