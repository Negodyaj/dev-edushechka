﻿CREATE TABLE [City] (
	Id		int				NOT NULL,
	Name	nvarchar(100)	NOT NULL,
  CONSTRAINT [PK_CITY] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)
)