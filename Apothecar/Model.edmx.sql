
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/11/2017 11:18:48
-- Generated from EDMX file: C:\Users\BD Production\Documents\Visual Studio 2013\Projects\Apothecary\Apothecar\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ApoDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_EssentialOilCombo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comboes] DROP CONSTRAINT [FK_EssentialOilCombo];
GO
IF OBJECT_ID(N'[dbo].[FK_EssentialOilCombo1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comboes] DROP CONSTRAINT [FK_EssentialOilCombo1];
GO
IF OBJECT_ID(N'[dbo].[FK_DescriptorEssentialOil]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Descriptors] DROP CONSTRAINT [FK_DescriptorEssentialOil];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[EssentialOils]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EssentialOils];
GO
IF OBJECT_ID(N'[dbo].[Descriptors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Descriptors];
GO
IF OBJECT_ID(N'[dbo].[Comboes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comboes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'EssentialOils'
CREATE TABLE [dbo].[EssentialOils] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Descriptors'
CREATE TABLE [dbo].[Descriptors] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [EssentialOilId] int  NOT NULL
);
GO

-- Creating table 'Comboes'
CREATE TABLE [dbo].[Comboes] (
    [EssentialOilId1] int  NOT NULL,
    [EssentialOilId2] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'EssentialOils'
ALTER TABLE [dbo].[EssentialOils]
ADD CONSTRAINT [PK_EssentialOils]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Descriptors'
ALTER TABLE [dbo].[Descriptors]
ADD CONSTRAINT [PK_Descriptors]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [EssentialOilId1], [EssentialOilId2] in table 'Comboes'
ALTER TABLE [dbo].[Comboes]
ADD CONSTRAINT [PK_Comboes]
    PRIMARY KEY CLUSTERED ([EssentialOilId1], [EssentialOilId2] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [EssentialOilId1] in table 'Comboes'
ALTER TABLE [dbo].[Comboes]
ADD CONSTRAINT [FK_EssentialOilCombo]
    FOREIGN KEY ([EssentialOilId1])
    REFERENCES [dbo].[EssentialOils]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [EssentialOilId2] in table 'Comboes'
ALTER TABLE [dbo].[Comboes]
ADD CONSTRAINT [FK_EssentialOilCombo1]
    FOREIGN KEY ([EssentialOilId2])
    REFERENCES [dbo].[EssentialOils]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EssentialOilCombo1'
CREATE INDEX [IX_FK_EssentialOilCombo1]
ON [dbo].[Comboes]
    ([EssentialOilId2]);
GO

-- Creating foreign key on [EssentialOilId] in table 'Descriptors'
ALTER TABLE [dbo].[Descriptors]
ADD CONSTRAINT [FK_EssentialOilDescriptor]
    FOREIGN KEY ([EssentialOilId])
    REFERENCES [dbo].[EssentialOils]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EssentialOilDescriptor'
CREATE INDEX [IX_FK_EssentialOilDescriptor]
ON [dbo].[Descriptors]
    ([EssentialOilId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------