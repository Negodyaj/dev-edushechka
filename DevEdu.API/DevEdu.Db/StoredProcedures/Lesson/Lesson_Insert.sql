CREATE PROCEDURE [dbo].[Lesson_Insert]
    @Date datetime,
    @AdditionalMaterials nvarchar(500),
    @TeacherId int,
    @LinkToRecord nvarchar(150)
AS
BEGIN
    INSERT INTO dbo.Lesson (Date, AdditionalMaterials, TeacherId, LinkToRecord)
    VALUES (@Date, @AdditionalMaterials, @TeacherId, @LinkToRecord)
    SELECT @@IDENTITY
END