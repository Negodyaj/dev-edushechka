CREATE PROCEDURE [dbo].[Course_Topic_SelectAllByCourseId]
	@CourseId int
As
Begin
	Select 
		ct.TopicId,
		ct.Position,
		t.Name as TopicName,
		t.Duration
	From dbo.Course_Topic ct
		inner join dbo.Topic t on t.Id = ct.TopicId 
	Where (CourseId = @CourseId)
End
