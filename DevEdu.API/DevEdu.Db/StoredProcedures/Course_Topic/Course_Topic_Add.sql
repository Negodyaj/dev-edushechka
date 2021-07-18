Create Procedure [dbo].[Course_Topic_Add]
	@tblCourseTopic Course_TopicType readonly
As
Begin
	set nocount on;
	
	insert into Course_Topic(CourseId,TopicId,Position)
	Select CourseId,TopicId,Position From @tblCourseTopic

	End  