CREATE TABLE [Group_Material] (
	Id int NOT NULL Identity,
	MaterialId int NOT NULL,
	GroupId int NOT NULL,
  CONSTRAINT [PK_GROUP_MATERIAL] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
go

ALTER TABLE [Group_Material] WITH CHECK ADD CONSTRAINT [Group_Material_fk0] FOREIGN KEY ([MaterialId]) REFERENCES [Material]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Group_Material] CHECK CONSTRAINT [Group_Material_fk0]
GO
ALTER TABLE [Group_Material] WITH CHECK ADD CONSTRAINT [Group_Material_fk1] FOREIGN KEY ([GroupId]) REFERENCES [Group]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Group_Material] CHECK CONSTRAINT [Group_Material_fk1]
GO