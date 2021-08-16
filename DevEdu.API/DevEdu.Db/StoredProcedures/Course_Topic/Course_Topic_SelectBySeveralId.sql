CREATE PROCEDURE [dbo].[Course_Topic_SelectBySeveralId]
	@tblIds dbo.IdType readonly
as
Begin
	select 
		ct.Id,
		ct.CourseId,
		ct.TopicId,
		ct.Position,
		t.Id,
		t.Name,
		t.Duration
From dbo.Course_Topic ct
	inner join @tblIds ids
	on ct.Id = ids.Id
	inner join [dbo].[Topic] t 
	on ct.TopicId = t.Id
End 
