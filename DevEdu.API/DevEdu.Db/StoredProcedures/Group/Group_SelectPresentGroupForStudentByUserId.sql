CREATE PROCEDURE dbo.Group_SelectPresentGroupForStudentByUserId
	@Id int
AS
BEGIN
     SELECT
		gr.Id
     FROM dbo.[Group] gr 
		LEFT JOIN dbo.User_Group ug on ug.GroupId = gr.Id
		LEFT JOIN dbo.[User] u on u.Id = ug.UserId
     WHERE u.Id = @Id 
		AND gr.StartDate = (SELECT max(gr.StartDate)  FROM  dbo.[Group] gr)
END
