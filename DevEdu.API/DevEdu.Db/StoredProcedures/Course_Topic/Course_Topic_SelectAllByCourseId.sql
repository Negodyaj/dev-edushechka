CREATE PROCEDURE [dbo].[Course_Topic_SelectAllByCourseId]
	@CourseId int
As
Begin
	Select 
		ct.Id,
		ct.Position,
		t.Id,
		t.Name,
		t.Duration
	From dbo.Course_Topic ct
		inner join dbo.Topic t on t.Id = ct.TopicId 
	Where (CourseId = @CourseId)
End
