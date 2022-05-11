CREATE PROCEDURE [dbo].[Lesson_Insert]
    @Date datetime,
    @AdditionalMaterials nvarchar(500),
    @TeacherId int,
    @LinkToRecord nvarchar(150),
    @Name nvarchar(150)
AS
BEGIN
    INSERT INTO dbo.Lesson (Date, AdditionalMaterials, TeacherId, LinkToRecord, Name)
    VALUES (@Date, @AdditionalMaterials, @TeacherId, @LinkToRecord, @Name)
    SELECT @@IDENTITY
END