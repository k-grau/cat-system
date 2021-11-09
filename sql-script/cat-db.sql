﻿CREATE TABLE [dbo].[Levnadsform] (
    [Id]  INT           IDENTITY (1, 1) NOT NULL,
    [Liv] NVARCHAR (20) NOT NULL,
    CONSTRAINT [PK_Tbl_Levnadsform] PRIMARY KEY CLUSTERED ([Id] ASC)
);

﻿
INSERT INTO [dbo].[Levnadsform] ([Liv])
VALUES 
('Innekatt'),
('Utekatt'),
('Forvildad')


CREATE TABLE [dbo].[Katt] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [Namn]   VARCHAR (30) NOT NULL,
    [Fodd]   INT NOT NULL,
    [Farg]   VARCHAR (30) NOT NULL,
    [Sort]   VARCHAR (30) NULL,
    [Lever_som]       INT          NOT NULL,
    CONSTRAINT [PK_Tbl_Katt] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Tbl_Katt_Tbl_Levnadsform] FOREIGN KEY ([Lever_som]) REFERENCES [dbo].[Levnadsform] ([Id]),
);


﻿CREATE TABLE [dbo].[HarOvana] (
    [Ovana]  INT NOT NULL,
    [Katt] INT NOT NULL,
    CONSTRAINT [FK1_Tbl_HarOvana] FOREIGN KEY ([Ovana]) REFERENCES [dbo].[Ovana] ([Id]),
    CONSTRAINT [FK2_Tbl_HarOvana] FOREIGN KEY ([Katt]) REFERENCES [dbo].[Katt] ([Id])
);


﻿INSERT INTO [Ovana] 
([Beteende])
VALUES ('Klöser'), ('Stökar'),('Protestbajsar');






