/****** 
Copyright (C) 2014-2015 Jerome Athias
Attacks related tables for XORCISM database
This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
******/

USE [XATTACK]
GO

/****** Object:  Table [dbo].[ATTACKCATEGORY]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKCATEGORY](
	[AttackCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[AttackCategoryGUID] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[CategoryGUID] [nvarchar](500) NULL,
	[AttackCategoryName] [nvarchar](250) NULL,
	[AttackCategoryDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[VocabularyGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[CreationObjectGUID] [nvarchar](500) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKCATEGORYREFERENCE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKCATEGORYREFERENCE](
	[AttackCategoryReferenceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKCONSEQUENCE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKCONSEQUENCE](
	[AttackConsequenceID] [int] IDENTITY(1,1) NOT NULL,
	[Consequence] [nvarchar](max) NULL,
	[ConsequenceNote] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKCONSEQUENCETAG]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKCONSEQUENCETAG](
	[AttackConsequenceTagID] [int] IDENTITY(1,1) NOT NULL,
	[AttackConsequenceID] [int] NULL,
	[TagID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKEXAMPLE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKEXAMPLE](
	[AttackExampleID] [int] IDENTITY(1,1) NOT NULL,
	[AttackExampleGUID] [nvarchar](500) NULL,
	[AttackExampleName] [nvarchar](250) NULL,
	[AttackExampleDescription] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL,
	[AttackExampleVocabularyID] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKEXAMPLEFORATTACKPATTERN]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKEXAMPLEFORATTACKPATTERN](
	[AttackExampleForAttackPatternID] [int] IDENTITY(1,1) NOT NULL,
	[AttackExampleID] [int] NOT NULL,
	[AttackExampleGUID] [nvarchar](500) NULL,
	[AttackPatternID] [int] NOT NULL,
	[AttackPatternGUID] [nvarchar](500) NULL,
	[capec_id] [nvarchar](20) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKEXECUTIONFLOW]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKEXECUTIONFLOW](
	[AttackExecutionFlowID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternID] [int] NULL,
	[capec_id] [nvarchar](20) NULL,
	[AttackExecutionFlowGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKEXECUTIONFLOWPHASE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKEXECUTIONFLOWPHASE](
	[AttackExecutionFlowPhaseID] [int] IDENTITY(1,1) NOT NULL,
	[AttackExecutionFlowID] [int] NULL,
	[AttackExecutionFlowGUID] [nvarchar](500) NULL,
	[AttackPhaseID] [int] NULL,
	[AttackPhaseGUID] [nvarchar](500) NULL,
	[AttackPhaseOrder] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKMETHOD]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKMETHOD](
	[AttackMethodID] [int] IDENTITY(1,1) NOT NULL,
	[AttackMethodGUID] [nvarchar](500) NULL,
	[AttackMethodTitle] [nvarchar](100) NOT NULL,
	[AttackMethodDescription] [nvarchar](max) NULL,
	[SourceID] [int] NULL,
	[SourceGUID] [nvarchar](500) NULL,
	[VocabularyID] [int] NULL,
	[VocabularyGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[CreationObjectGUID] [nvarchar](500) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKMETHODDESCRIPTION]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKMETHODDESCRIPTION](
	[AttackMethodDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKMETHODFORATTACKPATTERN]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKMETHODFORATTACKPATTERN](
	[AttackPatternMethodID] [int] IDENTITY(1,1) NOT NULL,
	[AttackMethodID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKMETHODREFERENCE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKMETHODREFERENCE](
	[AttackMethodReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[AttackMethodID] [int] NULL,
	[ReferenceID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKMETHODTAG]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKMETHODTAG](
	[AttackMethodTagID] [int] IDENTITY(1,1) NOT NULL,
	[AttackMethodID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERN]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERN](
	[AttackPatternID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternGUID] [nvarchar](500) NULL,
	[capec_id] [nvarchar](20) NULL,
	[category] [bit] NULL,
	[AttackPatternName] [nvarchar](250) NULL,
	[AttackPatternDescription] [nvarchar](max) NULL,
	[PatternAbstraction] [nvarchar](50) NULL,
	[PatternCompleteness] [nvarchar](50) NULL,
	[PatternStatus] [nvarchar](50) NULL,
	[TypicalSeverity] [nvarchar](50) NULL,
	[Payload_Activation_Impact] [nvarchar](250) NULL,
	[SourceID] [int] NULL,
	[SourceGUID] [nvarchar](500) NULL,
	[RepositoryID] [int] NULL,
	[RepositoryGUID] [nvarchar](500) NULL,
	[VocabularyID] [int] NULL,
	[VocabularyGUID] [nvarchar](500) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[CreationObjectGUID] [nvarchar](500) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNATTACKCONSEQUENCE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNATTACKCONSEQUENCE](
	[AttackPatternAttackConsequenceID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternID] [int] NULL,
	[AttackPatternGUID] [nvarchar](500) NULL,
	[CAPECAttackConsequenceOrder] [int] NULL,
	[AttackConsequenceID] [int] NULL,
	[Consequence_Note] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNATTACKCONSEQUENCESCOPE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNATTACKCONSEQUENCESCOPE](
	[AttackPatternAttackConsequenceScopeID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternAttackConsequenceID] [int] NULL,
	[AttackScopeID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNATTACKTECHNICALIMPACT]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNATTACKTECHNICALIMPACT](
	[AttackPatternAttackTechnicalImpactID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternAttackConsequenceID] [int] NULL,
	[AttackTechnicalImpactID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNCWE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNCWE](
	[AttackPatternCWEID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternID] [int] NULL,
	[WeaknessRelationship] [nvarchar](50) NULL,
	[CWEID] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNFORTHREATACTORTTP]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNFORTHREATACTORTTP](
	[AttackPatternForThreatActorTTPID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNINDICATORWARNING]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNINDICATORWARNING](
	[AttackPatternIndicatorWarningID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternID] [int] NULL,
	[AttackPatternIndicatorWarningOrder] [int] NULL,
	[IndicatorWarningAttack] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNMITIGATION]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNMITIGATION](
	[AttackPatternMitigationID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternMitigationGUID] [nvarchar](500) NULL,
	[AttackPatternID] [int] NULL,
	[AttackPatternGUID] [nvarchar](500) NULL,
	[MitigationID] [int] NULL,
	[MitigationGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNNOTE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNNOTE](
	[AttackPatternNoteID] [int] IDENTITY(1,1) NOT NULL,
	[NoteText] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNNOTES]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNNOTES](
	[AttackPatternNotesID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternID] [int] NULL,
	[NoteOrder] [int] NULL,
	[AttackPatternNoteID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNOBFUSCATIONTECHNIQUE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNOBFUSCATIONTECHNIQUE](
	[AttackPatternObfuscationTechniqueID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[AttackPatternGUID] [nvarchar](500) NULL,
	[ObfuscationTechniqueID] [int] NOT NULL,
	[ObfuscationTechniqueGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNPROBINGTECHNIQUE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNPROBINGTECHNIQUE](
	[AttackPatternProbingTechniqueID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternID] [int] NULL,
	[AttackPatternGUID] [nvarchar](500) NULL,
	[AttackTechniqueID] [int] NULL,
	[AttackTechniqueGUID] [nvarchar](500) NULL,
	[TechniqueID] [int] NULL,
	[TechniqueGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNREFERENCE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNREFERENCE](
	[AttackPatternReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternReferenceGUID] [nvarchar](500) NULL,
	[AttackPatternID] [int] NULL,
	[AttackPatternGUID] [nvarchar](500) NULL,
	[ReferenceID] [int] NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[Reference_ID] [nvarchar](50) NULL,
	[Local_Reference_ID] [nvarchar](50) NULL,
	[Reference_Section] [nvarchar](100) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNRELATIONSHIP]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNRELATIONSHIP](
	[AttackPatternRelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternRefID] [int] NULL,
	[AttackPatternRefGUID] [nvarchar](500) NULL,
	[RelationshipName] [nvarchar](50) NULL,
	[Relationship_Description] [nvarchar](max) NULL,
	[AttackPatternSubjectID] [int] NULL,
	[AttackPatternSubjectGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNSECURITYCONTROL]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNSECURITYCONTROL](
	[AttackPatternSecurityControlID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternID] [int] NULL,
	[SecurityControlID] [int] NULL,
	[AttackPatternSecurityControlVocabularyID] [int] NULL,
	[AttackPatternSecurityControlOrder] [int] NULL,
	[SecurityControlTypeID] [int] NULL,
	[SecurityControlType] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNTAG]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNTAG](
	[AttackPatternTagID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternTagGUID] [nvarchar](500) NULL,
	[AttackPatternID] [int] NULL,
	[AttackPatternGUID] [nvarchar](500) NULL,
	[TagID] [int] NULL,
	[TagGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNVIEW]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNVIEW](
	[AttackPatternViewID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternViewGUID] [nvarchar](500) NULL,
	[ViewVocabularyID] [int] NULL,
	[AttackPatternViewName] [nvarchar](250) NULL,
	[View_Structure] [nvarchar](50) NULL,
	[AttackPatternViewDescription] [nvarchar](max) NULL,
	[View_Filter] [nvarchar](50) NULL,
	[ViewStatus] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNVIEWRELATIONSHIP]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNVIEWRELATIONSHIP](
	[AttackPatternViewRelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternViewID] [int] NULL,
	[Ordinal] [nvarchar](50) NULL,
	[Relationship_Target_Form] [nvarchar](50) NULL,
	[Relationship_Nature] [nvarchar](50) NULL,
	[Relationship_Description] [nvarchar](max) NULL,
	[AttackPatternID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPAYLOAD]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPAYLOAD](
	[AttackPayloadID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPayloadGUID] [nvarchar](500) NULL,
	[PayloadText] [nvarchar](max) NOT NULL,
	[Payload_Activation_Impact] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromdate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPAYLOADENCODER]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPAYLOADENCODER](
	[AttackPayloadEncoderID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPayloadEncoderName] [nvarchar](100) NULL,
	[AttackPayloadEncoderDescription] [nvarchar](max) NULL,
	[AttackPayloadEncoderVersion] [nvarchar](50) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPAYLOADFORATTACKPATTERN]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPAYLOADFORATTACKPATTERN](
	[AttackPatternPayloadID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternPayloadGUID] [nvarchar](500) NULL,
	[AttackPayloadID] [int] NOT NULL,
	[AttackPayloadGUID] [nvarchar](500) NULL,
	[AttackPayloadImpactID] [int] NULL,
	[AttackPatternID] [int] NOT NULL,
	[AttackPatternGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPAYLOADIMPACT]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPAYLOADIMPACT](
	[AttackPayloadImpactID] [int] IDENTITY(1,1) NOT NULL,
	[PayloadActivationImpactDescription] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPAYLOADIMPACTFORATTACKPATTERN]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPAYLOADIMPACTFORATTACKPATTERN](
	[AttackPatternPayloadImpactID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPayloadImpactID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPHASE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPHASE](
	[AttackPhaseID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPhaseGUID] [nvarchar](500) NULL,
	[PhaseID] [int] NULL,
	[AttackPhaseName] [nvarchar](50) NULL,
	[AttackPhaseDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPHASEFORATTACKPATTERN]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPHASEFORATTACKPATTERN](
	[AttackPatternAttackPhaseID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternID] [int] NULL,
	[AttackPatternGUID] [nvarchar](500) NULL,
	[AttackPhaseGUID] [nvarchar](500) NULL,
	[AttackPhaseID] [int] NULL,
	[AttackPhaseVocabularyID] [int] NULL,
	[AttackPhaseOrder] [int] NULL,
	[AttackPhaseDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPREREQUISITE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPREREQUISITE](
	[AttackPrerequisiteID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPrerequisiteGUID] [nvarchar](500) NULL,
	[PrerequisiteText] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPREREQUISITEFORATTACKPATTERN]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPREREQUISITEFORATTACKPATTERN](
	[AttackPatternAttackPrerequisiteID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPrerequisiteID] [int] NOT NULL,
	[AttackPatternID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPURPOSE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPURPOSE](
	[AttackPurposeID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPurposeGUID] [nchar](10) NULL,
	[AttackPurposeName] [nvarchar](100) NOT NULL,
	[AttackPurposeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPURPOSEFORATTACKPATTERN]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPURPOSEFORATTACKPATTERN](
	[AttackPatternPurposeID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPurposeID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKRESOURCE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKRESOURCE](
	[AttackResourceID] [int] IDENTITY(1,1) NOT NULL,
	[AttackResourceText] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKRESOURCEFORATTACKPATTERN]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKRESOURCEFORATTACKPATTERN](
	[AttackPatternAttackResourceRequiredID] [int] IDENTITY(1,1) NOT NULL,
	[AttackResourceID] [int] NOT NULL,
	[AttackPatternID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKRESOURCETAG]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKRESOURCETAG](
	[AttackResourceTagID] [int] IDENTITY(1,1) NOT NULL,
	[AttackResourceID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSCENARIO]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSCENARIO](
	[AttackScenarioID] [int] IDENTITY(1,1) NOT NULL,
	[ScenarioID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSCOPE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSCOPE](
	[AttackScopeID] [int] IDENTITY(1,1) NOT NULL,
	[AttackScopeGUID] [nvarchar](500) NULL,
	[ConsequenceScope] [nvarchar](100) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSTEP]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSTEP](
	[AttackStepID] [int] IDENTITY(1,1) NOT NULL,
	[AttackStepGUID] [nvarchar](500) NULL,
	[AttackPatternAttackPhaseID] [int] NULL,
	[AttackStepVocabularyID] [int] NULL,
	[AttackStepOrder] [int] NULL,
	[Attack_Step_Title] [nvarchar](500) NULL,
	[Attack_Step_Description] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSTEPINDICATOR]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSTEPINDICATOR](
	[AttackStepIndicatorID] [int] IDENTITY(1,1) NOT NULL,
	[AttackStepIndicatorGUID] [nvarchar](500) NULL,
	[AttackStepID] [int] NULL,
	[AttackStepGUID] [nvarchar](500) NULL,
	[IndicatorID] [int] NULL,
	[IndicatorGUID] [nvarchar](500) NULL,
	[AttackStepIndicatorVocabularyID] [nvarchar](50) NULL,
	[AttackStepIndicatorType] [nvarchar](50) NULL,
	[AttackStepIndicatorDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSTEPINDICATORENVIRONMENT]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSTEPINDICATORENVIRONMENT](
	[AttackStepIndicatorEnvironmentID] [int] IDENTITY(1,1) NOT NULL,
	[AttackStepIndicatorID] [int] NOT NULL,
	[EnvironmentID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSTEPINDICATORTAG]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSTEPINDICATORTAG](
	[AttackStepIndicatorTagID] [int] IDENTITY(1,1) NOT NULL,
	[AttackStepIndicatorID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSTEPOUTCOME]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSTEPOUTCOME](
	[AttackStepOutcomeID] [int] IDENTITY(1,1) NOT NULL,
	[AttackStepID] [int] NULL,
	[OutcomeVocabularyID] [nvarchar](50) NULL,
	[OutcomeType] [nvarchar](50) NULL,
	[OutcomeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSTEPOUTCOMETAG]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSTEPOUTCOMETAG](
	[AttackStepOutcomeTagID] [int] IDENTITY(1,1) NOT NULL,
	[AttackStepOutcomeID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSTEPSECURITYCONTROL]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSTEPSECURITYCONTROL](
	[AttackStepSecurityControlID] [int] IDENTITY(1,1) NOT NULL,
	[AttackStepID] [int] NULL,
	[AttackStepGUID] [nvarchar](500) NULL,
	[SecurityControlID] [int] NULL,
	[SecurityControlGUID] [nvarchar](500) NULL,
	[AttackStepSecurityControlVocabularyID] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSTEPTAG]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSTEPTAG](
	[AttackStepTagID] [int] IDENTITY(1,1) NOT NULL,
	[AttackStepID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSTEPTECHNIQUE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSTEPTECHNIQUE](
	[AttackStepTechniqueID] [int] IDENTITY(1,1) NOT NULL,
	[AttackStepTechniqueGUID] [nvarchar](500) NULL,
	[AttackStepTechniqueVocabularyID] [nvarchar](50) NULL,
	[AttackStepID] [int] NULL,
	[AttackTechniqueID] [int] NULL,
	[AttackStepTechniqueOrder] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSTEPTECHNIQUEENVIRONMENT]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSTEPTECHNIQUEENVIRONMENT](
	[AttackStepTechniqueEnvironmentID] [int] IDENTITY(1,1) NOT NULL,
	[AttackStepTechniqueID] [int] NULL,
	[EnvironmentID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSTEPTECHNIQUELEVERAGEDPATTERN]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSTEPTECHNIQUELEVERAGEDPATTERN](
	[AttackStepTechniqueLeveragedPatternID] [int] IDENTITY(1,1) NOT NULL,
	[AttackStepTechniqueID] [int] NULL,
	[AttackStepTechniqueGUID] [nvarchar](500) NULL,
	[AttackPatternID] [int] NULL,
	[AttackPatternGUID] [nvarchar](500) NULL,
	[LeveragedAttackPatternOrder] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSURFACE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSURFACE](
	[AttackSurfaceID] [int] IDENTITY(1,1) NOT NULL,
	[AttackSurfaceGUID] [nvarchar](500) NULL,
	[AttackSurfaceName] [nvarchar](250) NULL,
	[AttackSurfaceDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSURFACECHANGERECORD]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSURFACECHANGERECORD](
	[AttackSurfaceChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSURFACEFORATTACKPATTERN]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSURFACEFORATTACKPATTERN](
	[AttackPatternSurfaceID] [int] IDENTITY(1,1) NOT NULL,
	[AttackSurfaceID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSURFACEINTERACTIONPOINTS]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSURFACEINTERACTIONPOINTS](
	[AttackSurfaceInteractionPointsID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSURFACELOCALITY]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSURFACELOCALITY](
	[AttackSurfaceLocalityID] [int] IDENTITY(1,1) NOT NULL,
	[AttackSurfaceLocalityName] [nvarchar](100) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSURFACELOCALITYFORATTACKSURFACE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSURFACELOCALITYFORATTACKSURFACE](
	[AttackSurfaceLocalitiesID] [int] IDENTITY(1,1) NOT NULL,
	[AttackSurfaceLocalityID] [int] NOT NULL,
	[AttackSurfaceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSURFACESERVICE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSURFACESERVICE](
	[AttackSurfaceServiceID] [int] IDENTITY(1,1) NOT NULL,
	[AttackSurfaceID] [int] NOT NULL,
	[EndPointID] [int] NULL,
	[TargetFunctionalServiceID] [int] NULL,
	[TargetFunctionalServiceName] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSURFACESERVICEPROTOCOL]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSURFACESERVICEPROTOCOL](
	[AttackSurfaceServiceProtocolID] [int] IDENTITY(1,1) NOT NULL,
	[AttackSurfaceServiceID] [int] NOT NULL,
	[TargetFunctionalServiceProtocolID] [int] NULL,
	[ProtocolID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSURFACETYPE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSURFACETYPE](
	[AttackSurfaceTypeID] [int] IDENTITY(1,1) NOT NULL,
	[AttackSurfaceTypeName] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSURFACETYPEFORATTACKSURFACE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSURFACETYPEFORATTACKSURFACE](
	[AttackSurfaceTypesID] [int] IDENTITY(1,1) NOT NULL,
	[AttackSurfaceTypeID] [int] NOT NULL,
	[AttackSurfaceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTECHNICALIMPACT]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTECHNICALIMPACT](
	[AttackTechnicalImpactID] [int] IDENTITY(1,1) NOT NULL,
	[AttackTechnicalImpactGUID] [nvarchar](500) NULL,
	[ImpactID] [int] NULL,
	[ConsequenceTechnicalImpact] [nvarchar](250) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTECHNIQUE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTECHNIQUE](
	[AttackTechniqueID] [int] IDENTITY(1,1) NOT NULL,
	[AttackTechniqueGUID] [nvarchar](500) NULL,
	[TechniqueID] [int] NULL,
	[AttackTechniqueName] [nvarchar](500) NULL,
	[AttackTechniqueDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTECHNIQUEINDICATOR]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTECHNIQUEINDICATOR](
	[AttackTechniqueIndicatorID] [int] IDENTITY(1,1) NOT NULL,
	[AttackTechniqueID] [int] NULL,
	[IndicatorID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTECHNIQUEREFERENCE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTECHNIQUEREFERENCE](
	[AttackTechniqueReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[AttackTechniqueID] [int] NULL,
	[ReferenceID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTECHNIQUETAG]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTECHNIQUETAG](
	[AttackTechniqueTagID] [int] IDENTITY(1,1) NOT NULL,
	[AttackTechniqueID] [int] NULL,
	[AttackTechniqueGUID] [nvarchar](500) NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTECHNIQUETOOL]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTECHNIQUETOOL](
	[AttackTechniqueToolID] [int] IDENTITY(1,1) NOT NULL,
	[AttackTechniqueID] [int] NULL,
	[AttackTechniqueGUID] [nvarchar](500) NULL,
	[AttackToolID] [int] NULL,
	[AttackToolGUID] [nvarchar](500) NULL,
	[ToolID] [int] NULL,
	[ToolGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTOOL]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTOOL](
	[AttackToolID] [int] IDENTITY(1,1) NOT NULL,
	[AttackTooldGUID] [nvarchar](500) NULL,
	[TooldID] [int] NULL,
	[AttackToolTypeID] [int] NULL,
	[AttackToolName] [nvarchar](100) NOT NULL,
	[AttackToolVersion] [nvarchar](50) NULL,
	[VersionID] [int] NULL,
	[AttackToolDescription] [nvarchar](max) NULL,
	[AttackToolAuthor] [nvarchar](100) NULL,
	[AttackToolLink] [nvarchar](250) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTOOLAUTHENTICATIONTYPE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTOOLAUTHENTICATIONTYPE](
	[AttackToolAuthenticationTypeID] [int] IDENTITY(1,1) NOT NULL,
	[AttackToolID] [int] NULL,
	[AuthenticationTypeID] [int] NULL,
	[AttackToolAuthenticationTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTOOLDESCRIPTION]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTOOLDESCRIPTION](
	[AttackToolDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTOOLFORTHREATACTORTTP]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTOOLFORTHREATACTORTTP](
	[ThreatActorTTPAttackToolID] [int] NOT NULL,
	[AttackToolID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[notes] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTOOLMODULE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTOOLMODULE](
	[AttackToolModuleID] [int] IDENTITY(1,1) NOT NULL,
	[AttackToolModuleName] [nvarchar](250) NULL,
	[AttackToolModuleDescription] [nvarchar](max) NULL,
	[AttackToolModuleVersion] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTOOLMODULEAUTHENTICATIONTYPE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTOOLMODULEAUTHENTICATIONTYPE](
	[AttackToolModuleAuthenticationTypeID] [int] IDENTITY(1,1) NOT NULL,
	[AttackToolModuleID] [int] NULL,
	[AuthenticationTypeID] [int] NULL,
	[AttackToolModuleAuthenticationTypeDescription] [nvarchar](max) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTOOLTAG]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTOOLTAG](
	[AttackToolTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTOOLTYPE]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTOOLTYPE](
	[AttackToolTypeID] [int] IDENTITY(1,1) NOT NULL,
	[AttackToolTypeGUID] [nvarchar](500) NULL,
	[AttackToolTypeName] [nvarchar](100) NOT NULL,
	[AttackToolTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKVECTOR]    Script Date: 04/03/2015 19:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKVECTOR](
	[AttackVectorID] [int] IDENTITY(1,1) NOT NULL,
	[AttackVectorName] [nvarchar](150) NULL,
	[AttackvectorDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


