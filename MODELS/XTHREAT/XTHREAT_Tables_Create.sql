/****** 
Copyright (C) 2014-2015 Jerome Athias
Threats related tables for XORCISM database
This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
******/

USE [XTHREAT]
GO

/****** Object:  Table [dbo].[THREATACTION]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTION](
	[ThreatActionID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActionCategoryID] [int] NOT NULL,
	[ThreatActionCategoryName] [nvarchar](50) NULL,
	[ThreatActionName] [nvarchar](100) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[notes] [nvarchar](max) NULL,
	[target] [nvarchar](50) NULL,
	[AssetID] [int] NULL,
	[PersonID] [int] NULL,
	[PhysicalLocationID] [int] NULL,
	[ThreatActionTargetID] [int] NULL,
	[ThreatActionLocationID] [int] NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTIONCATEGORY]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTIONCATEGORY](
	[ThreatActionCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActionCategoryName] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTIONFORINCIDENT]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTIONFORINCIDENT](
	[IncidentThreatActionID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActionID] [int] NOT NULL,
	[ThreatActorID] [int] NOT NULL,
	[IncidentID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ThreatIntendedEffectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTIONLOCATION]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTIONLOCATION](
	[ThreatActionLocationID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActionLocationName] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NOT NULL,
	[PhysicalLocationID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTIONLOCATIONFORTHREATACTIONCATEGORY]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTIONLOCATIONFORTHREATACTIONCATEGORY](
	[ThreatActionCategoryLocationID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActionCategoryID] [int] NOT NULL,
	[ThreatActionLocationID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTIONTARGET]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTIONTARGET](
	[ThreatActionTargetID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActionCategoryID] [int] NOT NULL,
	[ThreatActionTargetName] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTIONVARIETY]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTIONVARIETY](
	[ThreatActionVarietyID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActionCategoryID] [int] NOT NULL,
	[ThreatActionVarietyName] [nvarchar](100) NOT NULL,
	[ThreatActionCategoryDescription] [nvarchar](max) NULL,
	[WASCID] [nvarchar](10) NULL,
	[note] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTIONVARIETYFORTHREATACTORTTP]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTIONVARIETYFORTHREATACTORTTP](
	[ThreatActorTTPActionVarietyID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[ThreatActionVarietyID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTIONVECTOR]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTIONVECTOR](
	[ThreatActionVectorID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActionCategoryID] [int] NOT NULL,
	[ThreatActionVectorName] [nvarchar](250) NOT NULL,
	[VocabularyID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTOR]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTOR](
	[ThreatActorID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActorGUID] [nvarchar](500) NULL,
	[ThreatActorName] [nvarchar](250) NULL,
	[ThreatActorDescription] [nvarchar](max) NULL,
	[ActorExternal] [bit] NULL,
	[ActorInternal] [bit] NULL,
	[role] [nvarchar](50) NULL,
	[country] [nvarchar](50) NULL,
	[notes] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidentialityLevelID] [int] NULL,
	[SourceID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ImportanceID] [int] NULL,
	[CriticalityLevelID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORADDRESS]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORADDRESS](
	[ThreatActorAddressID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActorID] [int] NULL,
	[ThreatActorGUID] [nvarchar](500) NULL,
	[AddressID] [int] NULL,
	[AddressGUID] [nvarchar](500) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[ConfidentialityLevelID] [int] NULL,
	[SourceID] [int] NULL,
	[SourceGUID] [nvarchar](500) NULL,
	[CollectionMethodID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORCHANGERECORD]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORCHANGERECORD](
	[ThreatActorChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTOREMAILADDRESS]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTOREMAILADDRESS](
	[ThreatActorEmailAddressID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORFORINCIDENT]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORFORINCIDENT](
	[IncidentThreatActorID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentID] [int] NOT NULL,
	[IncidentGUID] [nvarchar](500) NULL,
	[ThreatActorID] [int] NOT NULL,
	[ThreatActorGUID] [nvarchar](500) NULL,
	[ThreatMotiveID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[ThreatActorRoleID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORFORTHREATCAMPAIGN]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORFORTHREATCAMPAIGN](
	[ThreatCampaignActorID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatCampaignID] [int] NOT NULL,
	[ThreatCampaignGUID] [datetimeoffset](7) NULL,
	[ThreatActorID] [int] NOT NULL,
	[ThreatActorGUID] [nvarchar](500) NULL,
	[ConfidenceLevel] [nvarchar](50) NULL,
	[ConfidentialityLevelID] [int] NULL,
	[notes] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORGROUP]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORGROUP](
	[ThreatActorGroupID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActorGroupGUID] [nvarchar](500) NULL,
	[GroupID] [int] NULL,
	[ThreatActorGroupName] [nvarchar](50) NULL,
	[ThreatActorGroupDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORGROUPTACTIC]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORGROUPTACTIC](
	[ThreatActorGroupTacticID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActorGroupID] [int] NOT NULL,
	[ThreatActorGroupGUID] [nvarchar](500) NULL,
	[TacticID] [int] NOT NULL,
	[TacticGUID] [nvarchar](500) NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORINFRASTRUCTURE]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORINFRASTRUCTURE](
	[ThreatActorInfrastructureID] [int] IDENTITY(1,1) NOT NULL,
	[AttackerInfrastructureGUID] [nvarchar](500) NULL,
	[AttackerInfrastructureName] [nvarchar](150) NOT NULL,
	[AttackerInfrastructureDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORINFRASTRUCTUREFORTHREATACTOR]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORINFRASTRUCTUREFORTHREATACTOR](
	[ThreatActorThreatActorInfrastructureID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActorID] [int] NOT NULL,
	[ThreatActorInfrastructureID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[notes] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORINFRASTRUCTUREFORTHREATACTORTTP]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORINFRASTRUCTUREFORTHREATACTORTTP](
	[ThreatActorTTPInfrastructureID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActorInfrastructureID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[Information_Source] [nvarchar](250) NULL,
	[ConfidenceLevel] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORPAOS]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORPAOS](
	[ThreatActorPAOSID] [int] IDENTITY(1,1) NOT NULL,
	[PlanningAndOperationalSupport] [nvarchar](200) NOT NULL,
	[PAOSDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORROLE]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORROLE](
	[ThreatActorRoleID] [int] IDENTITY(1,1) NOT NULL,
	[role] [nvarchar](50) NOT NULL,
	[roleDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORSKILLFORATTACKPATTERN]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORSKILLFORATTACKPATTERN](
	[AttackPatternRequiredSkillID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[AttackPatternRequiredSkillOrder] [int] NULL,
	[Skill_or_Knowledge_Level] [nvarchar](50) NULL,
	[SkillLevelID] [int] NULL,
	[Skill_or_Knowledge_Type] [nvarchar](max) NULL,
	[SkillID] [int] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORSOPHISTICATION]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORSOPHISTICATION](
	[ThreatActorSophisticationID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActorSophisticationGUID] [nvarchar](500) NULL,
	[ThreatActorSophisticationName] [nvarchar](50) NULL,
	[ThreatActorSophisticationDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORTACTIC]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORTACTIC](
	[ThreatActorTacticID] [int] IDENTITY(1,1) NOT NULL,
	[TacticID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORTAG]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORTAG](
	[ThreatActorTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORTTP]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORTTP](
	[ThreatActorTTPID] [int] IDENTITY(1,1) NOT NULL,
	[TTPTitle] [nvarchar](100) NOT NULL,
	[TTPDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[Information_Source] [nvarchar](250) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORTTPFORINCIDENT]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORTTPFORINCIDENT](
	[ThreatActorTTPIncidentID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[ThreatActorGUID] [nvarchar](500) NULL,
	[IncidentID] [int] NOT NULL,
	[IncidentGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORTTPFORINDICATOR]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORTTPFORINDICATOR](
	[ThreatActorTTPIndicatorID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[ThreatActorTTPGUID] [nvarchar](500) NULL,
	[IndicatorID] [int] NOT NULL,
	[IndicatorGUID] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORTTPFORTHREATACTORTTP]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORTTPFORTHREATACTORTTP](
	[ThreatActorTTPMappingID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActorTTPRefID] [int] NOT NULL,
	[Relationship] [nvarchar](50) NULL,
	[ThreatActorTTPSubjectID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORVARIETY]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORVARIETY](
	[ThreatActorVarietyID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActorTypeGUID] [nvarchar](500) NULL,
	[ExternalVariety] [bit] NULL,
	[InternalVariety] [bit] NULL,
	[ActorVariety] [nvarchar](50) NOT NULL,
	[ActorVarietyDescription] [nvarchar](200) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATAGENT]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATAGENT](
	[ThreatAgentID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatAgentGUID] [nvarchar](500) NULL,
	[ThreatAgentName] [nvarchar](100) NOT NULL,
	[ThreatAgentDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATAGENTCATEGORY]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATAGENTCATEGORY](
	[ThreatAgentCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATAGENTCHANGERECORD]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATAGENTCHANGERECORD](
	[ThreatAgentChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATAGENTFOROWASPTOP10]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATAGENTFOROWASPTOP10](
	[OWASPTOP10ThreatAgentID] [int] IDENTITY(1,1) NOT NULL,
	[OWASPTOP10ID] [int] NOT NULL,
	[ThreatAgentID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATAGENTTAG]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATAGENTTAG](
	[ThreatAgentTagID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatAgentID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGN]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGN](
	[ThreatCampaignID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatCampaignGUID] [nvarchar](500) NULL,
	[ThreatCampaignTitle] [nvarchar](150) NULL,
	[ThreatCampaignStatus] [nvarchar](50) NULL,
	[ThreatCampaignDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[SourceID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[ImportanceID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNCHANGERECORD]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNCHANGERECORD](
	[ThreatCampaignChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNDESCRIPTION]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNDESCRIPTION](
	[ThreatCampaignDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatCampaignID] [int] NOT NULL,
	[ThreatCampaignGUID] [nvarchar](500) NULL,
	[DescriptionID] [int] NOT NULL,
	[DescriptionGUID] [nvarchar](500) NULL,
	[ConfidentialityLevelID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[SourceID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNFORTHREATCAMPAIGN]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNFORTHREATCAMPAIGN](
	[ThreatCampaignMappingID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatCampaignRefID] [int] NOT NULL,
	[ThreatCampaignRefGUID] [nvarchar](500) NULL,
	[Relationship] [nvarchar](50) NULL,
	[ThreatCampaignSubjectID] [int] NOT NULL,
	[ThreatCampaignSubjectGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[ConfidenceLevel] [nvarchar](50) NULL,
	[ConfidenceLevelID] [int] NULL,
	[notes] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNMETHODOLOGY]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNMETHODOLOGY](
	[ThreatCampaignMethodologyID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNNAME]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNNAME](
	[ThreatCampaignNameID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatCampaignGUID] [nvarchar](500) NULL,
	[ThreatCampaignName] [nvarchar](150) NOT NULL,
	[internalname] [bit] NULL,
	[externalname] [bit] NULL,
	[Information_Source] [nvarchar](250) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[ValidFromDate] [datetimeoffset](7) NOT NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNNAMEFORTHREATCAMPAIGN]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNNAMEFORTHREATCAMPAIGN](
	[ThreatCampaignThreatCampaignNameID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatCampaignID] [int] NOT NULL,
	[ThreatCampaignNameID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[ValidFromDate] [datetimeoffset](7) NOT NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNREFERENCE]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNREFERENCE](
	[ThreatCampaignReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatCampaignID] [int] NULL,
	[ThreatCampaignGUID] [nvarchar](500) NULL,
	[ReferenceID] [int] NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNSOURCE]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNSOURCE](
	[ThreatCampaignSourceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNSTATUS]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNSTATUS](
	[ThreatCampaignStatusID] [int] IDENTITY(1,1) NOT NULL,
	[CampaignStatus] [nvarchar](50) NOT NULL,
	[CampaignStatusDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNTAG]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNTAG](
	[ThreatCampaignTagID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatCampaignID] [int] NOT NULL,
	[ThreatCampaignGUID] [nvarchar](500) NULL,
	[TagID] [int] NOT NULL,
	[TagGUID] [nvarchar](500) NULL,
	[ConfidentialityLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNTECHNIQUE]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNTECHNIQUE](
	[ThreatCampaignTechniqueID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNTOOL]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNTOOL](
	[ThreatCampaignToolID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNTYPE]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNTYPE](
	[ThreatCampaignTypeID] [int] IDENTITY(1,1) NOT NULL,
	[CampaignTypeTitle] [nvarchar](100) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCATEGORY]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCATEGORY](
	[ThreatCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatCategoryGUID] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[ThreatCategoryName] [nvarchar](250) NULL,
	[ThreatCategoryDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ImportanceID] [int] NULL,
	[ValidityID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCATEGORYDESCRIPTION]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCATEGORYDESCRIPTION](
	[ThreatCategoryDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatCategoryID] [int] NOT NULL,
	[DescriptionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCATEGORYREFERENCE]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCATEGORYREFERENCE](
	[ThreatCategoryReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatCategoryID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[CreationObjectID] [int] NULL,
	[ConfidentialityLevelID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCATEGORYTAG]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCATEGORYTAG](
	[ThreatCategoryTagID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatCategoryID] [int] NOT NULL,
	[TagID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[VocbularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATINTENDEDEFFECT]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATINTENDEDEFFECT](
	[ThreatIntendedEffectID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatIntendedEffectGUID] [nvarchar](500) NULL,
	[IntendedEffectName] [nvarchar](200) NOT NULL,
	[IntendedEffectDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATINTENDEDEFFECTFORINCIDENT]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATINTENDEDEFFECTFORINCIDENT](
	[IncidentThreatIntendedEffectID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatIntendedEffectID] [int] NOT NULL,
	[ThreatIntendedEffectGUID] [nvarchar](500) NULL,
	[IncidentID] [int] NOT NULL,
	[IncidentGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[ValidFromDate] [datetimeoffset](7) NOT NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATINTENDEDEFFECTFORTHREATACTORTTP]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATINTENDEDEFFECTFORTHREATACTORTTP](
	[ThreatActorTTPIntendedEffectID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatIntendedEffectID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[notes] [nvarchar](max) NULL,
	[ConfidenceLevel] [nvarchar](50) NULL,
	[Information_Source] [nvarchar](250) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NOT NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATINTENDEDEFFECTFORTHREATCAMPAIGN]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATINTENDEDEFFECTFORTHREATCAMPAIGN](
	[ThreatCampaignIntendedEffectID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatIntendedEffectID] [int] NOT NULL,
	[ThreatCampaignID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[ValidFromDate] [datetimeoffset](7) NOT NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATMOTIVE]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATMOTIVE](
	[ThreatMotiveID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatMotiveGUID] [nvarchar](500) NULL,
	[motive] [nvarchar](50) NOT NULL,
	[motiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntildate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATMOTIVEDESCRIPTION]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATMOTIVEDESCRIPTION](
	[ThreatMotiveDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATMOTIVEFORTHREATACTOR]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATMOTIVEFORTHREATACTOR](
	[ThreatActorMotiveID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatMotiveID] [int] NOT NULL,
	[ThreatActorID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATMOTIVETAG]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATMOTIVETAG](
	[ThreatMotiveTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATTYPE]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATTYPE](
	[ThreatTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatTypeGUID] [nvarchar](500) NULL,
	[ThreatTypeName] [nvarchar](50) NOT NULL,
	[ThreatTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATTYPEDESCRIPTION]    Script Date: 04/03/2015 18:51:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATTYPEDESCRIPTION](
	[ThreatTypeDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatTypeID] [int] NOT NULL,
	[DescriptionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO


