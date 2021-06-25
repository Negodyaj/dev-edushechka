CREATE TABLE [Course_Material] (
	Id int NOT NULL Identity,
	MaterialId int NOT NULL,
	CourseId int NOT NULL,
  CONSTRAINT [PK_COURSE_MATERIAL] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
go

ALTER TABLE [Course_Material] WITH CHECK ADD CONSTRAINT [Course_Material_fk0] FOREIGN KEY ([MaterialId]) REFERENCES [Material]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Course_Material] CHECK CONSTRAINT [Course_Material_fk0]
GO
ALTER TABLE [Course_Material] WITH CHECK ADD CONSTRAINT [Course_Material_fk1] FOREIGN KEY ([CourseId]) REFERENCES [Course]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Course_Material] CHECK CONSTRAINT [Course_Material_fk1]
GO