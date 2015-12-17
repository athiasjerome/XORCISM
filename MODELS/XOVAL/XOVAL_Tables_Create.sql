/****** 
Copyright (C) 2014-2015 Jerome Athias
MITRE OVAL related tables for XORCISM database
This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
******/


USE [XOVAL]
GO

/****** Object:  Table [dbo].[OPERATORENUMERATION]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OPERATORENUMERATION](
	[OperatorEnumerationID] [int] IDENTITY(1,1) NOT NULL,
	[OperatorValue] [nvarchar](50) NOT NULL,
	[OperatorDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALBEHAVIOR]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALBEHAVIOR](
	[OVALBehaviorID] [int] IDENTITY(1,1) NOT NULL,
	[BehaviorKey] [nvarchar](50) NULL,
	[BehaviorValue] [nvarchar](50) NULL,
	[BehaviorID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALBEHAVIORFOROVALOBJECT]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALBEHAVIORFOROVALOBJECT](
	[OVALObjectBehaviorID] [int] IDENTITY(1,1) NOT NULL,
	[OVALObjectID] [int] NOT NULL,
	[OVALBehaviorID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCLASSDIRECTIVES]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCLASSDIRECTIVES](
	[OVALClassDirectivesID] [int] IDENTITY(1,1) NOT NULL,
	[OVALClassEnumerationID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCLASSDIRECTIVESFOROVALDIRECTIVES]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCLASSDIRECTIVESFOROVALDIRECTIVES](
	[OVALDirectivesID] [int] NOT NULL,
	[OVALClassDirectivesID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCLASSDIRECTIVESFOROVALRESULTS]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCLASSDIRECTIVESFOROVALRESULTS](
	[OVALResultsID] [int] NOT NULL,
	[OVALClassDirectivesID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCLASSENUMERATION]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCLASSENUMERATION](
	[OVALClassEnumerationID] [int] IDENTITY(1,1) NOT NULL,
	[OVALClassEnumerationGUID] [nvarchar](500) NULL,
	[ClassValue] [nvarchar](50) NOT NULL,
	[ClassDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCOMPONENTGROUP]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCOMPONENTGROUP](
	[OVALComponentGroupID] [int] IDENTITY(1,1) NOT NULL,
	[OVALVariableID] [int] NULL,
	[OVALFunctionID] [int] NULL,
	[FunctionName] [nvarchar](50) NULL,
	[FunctionOperation] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCRITERIA]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCRITERIA](
	[OVALCriteriaID] [int] IDENTITY(1,1) NOT NULL,
	[OperatorEnumerationID] [int] NULL,
	[negate] [bit] NULL,
	[comment] [nvarchar](max) NULL,
	[applicabilitycheck] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCRITERIACRITERION]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCRITERIACRITERION](
	[OVALCriteriaCriterionID] [int] IDENTITY(1,1) NOT NULL,
	[OVALCriteriaID] [int] NOT NULL,
	[negate] [bit] NULL,
	[OVALTestID] [int] NULL,
	[comment] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[RepositoryID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCRITERIAEXTENDDEFINITION]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCRITERIAEXTENDDEFINITION](
	[OVALCriteriaExtendDefinitionID] [int] IDENTITY(1,1) NOT NULL,
	[OVALCriteriaID] [int] NOT NULL,
	[negate] [bit] NULL,
	[OVALDefinitionID] [int] NULL,
	[comment] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCRITERIAFOROVALCRITERIA]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCRITERIAFOROVALCRITERIA](
	[OVALCriteriaRelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[OVALCriteriaRefID] [int] NOT NULL,
	[RelationshipName] [nvarchar](50) NULL,
	[OVALCriteriaSubjectID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CriteriaRank] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCRITERIATYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCRITERIATYPE](
	[OVALCriteriaTypeID] [int] IDENTITY(1,1) NOT NULL,
	[OperatorEnumerationID] [int] NOT NULL,
	[OperatorValue] [nvarchar](50) NOT NULL,
	[negate] [bit] NULL,
	[ResultEnumerationID] [int] NOT NULL,
	[applicability_check] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCRITERIATYPEFOROVALDEFINITIONTYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCRITERIATYPEFOROVALDEFINITIONTYPE](
	[OVALDefinitionTypeID] [int] NOT NULL,
	[OVALCriteriaTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCRITERION]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCRITERION](
	[OVALCriterionID] [int] IDENTITY(1,1) NOT NULL,
	[OVALTestIDPattern] [nvarchar](500) NOT NULL,
	[negate] [bit] NULL,
	[comment] [nvarchar](max) NULL,
	[applicabilitycheck] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[RepositoryID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCRITERIONTYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCRITERIONTYPE](
	[OVALCriterionTypeID] [int] IDENTITY(1,1) NOT NULL,
	[OVALTestID] [int] NULL,
	[OVALTestIDPattern] [nvarchar](500) NOT NULL,
	[OVALTestVersion] [int] NOT NULL,
	[variable_instance] [int] NULL,
	[negate] [bit] NULL,
	[ResultEnumerationID] [int] NOT NULL,
	[applicability_check] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCRITERIONTYPEFOROVALCRITERIATYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCRITERIONTYPEFOROVALCRITERIATYPE](
	[OVALCriteriaTypeID] [int] NOT NULL,
	[OVALCriterionTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFAULTDIRECTIVES]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFAULTDIRECTIVES](
	[OVALDefaultDirectivesID] [int] IDENTITY(1,1) NOT NULL,
	[include_source_definitions] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITION]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITION](
	[OVALDefinitionID] [int] IDENTITY(1,1) NOT NULL,
	[OVALDefinitionIDPattern] [nvarchar](500) NOT NULL,
	[OVALDefinitionVersion] [int] NOT NULL,
	[OVALClassEnumerationID] [int] NULL,
	[deprecated] [bit] NULL,
	[OVALDefinitionTitle] [nvarchar](max) NULL,
	[OVALDefinitionDescription] [nvarchar](max) NULL,
	[notes] [nvarchar](max) NULL,
	[OVALCriteriaID] [int] NULL,
	[signature] [nvarchar](max) NULL,
	[StatusName] [nvarchar](150) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[RepositoryID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONCCE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONCCE](
	[OVALDefinitionCCEID] [int] IDENTITY(1,1) NOT NULL,
	[OVALDefinitionID] [int] NOT NULL,
	[CCEID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONCHANGE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONCHANGE](
	[OVALDefinitionChangeID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ChangeDate] [datetimeoffset](7) NULL,
	[ChangeTypeName] [nvarchar](50) NULL,
	[ChangeValue] [nvarchar](100) NULL,
	[ChangeComment] [nvarchar](max) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONCHANGES]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONCHANGES](
	[OVALDefinitionChangesID] [int] IDENTITY(1,1) NOT NULL,
	[OVALDefinitionID] [int] NULL,
	[OVALDefinitionChangeID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[OrganisationID] [int] NULL,
	[PersonID] [int] NULL,
	[AuthorID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONCPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONCPE](
	[OVALDefinitionCPEID] [int] IDENTITY(1,1) NOT NULL,
	[OVALDefinitionID] [int] NULL,
	[OVALDefinitionCPERelationship] [nvarchar](50) NULL,
	[CPEID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONFAMILY]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONFAMILY](
	[OVALDefinitionFamilyID] [int] IDENTITY(1,1) NOT NULL,
	[OVALDefinitionID] [int] NULL,
	[OSFamilyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONORGANISATION]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONORGANISATION](
	[OrganisationOVALDefinitionID] [int] IDENTITY(1,1) NOT NULL,
	[OVALDefinitionID] [int] NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[OrganisationRole] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONPLATFORM]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONPLATFORM](
	[OVALDefinitionPlatformID] [int] IDENTITY(1,1) NOT NULL,
	[OVALDefinitionID] [int] NULL,
	[PlatformID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONPRODUCT]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONPRODUCT](
	[OVALDefinitionProductID] [int] IDENTITY(1,1) NOT NULL,
	[OVALDefinitionID] [int] NULL,
	[ProductID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONREFERENCE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONREFERENCE](
	[OVALDefinitionReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[OVALDefinitionID] [int] NULL,
	[ReferenceID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONS]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONS](
	[OVALDefinitionsID] [int] IDENTITY(1,1) NOT NULL,
	[GeneratorTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONSTATUS]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONSTATUS](
	[OVALDefinitionID] [int] NOT NULL,
	[StatusID] [int] NOT NULL,
	[StatusDate] [nvarchar](50) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONTAG]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONTAG](
	[OVALDefinitionTagID] [int] IDENTITY(1,1) NOT NULL,
	[OVALDefinitionID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONTYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONTYPE](
	[OVALDefinitionTypeID] [int] IDENTITY(1,1) NOT NULL,
	[OVALDefinitionID] [int] NULL,
	[OVALDefinitionIDPattern] [nvarchar](500) NOT NULL,
	[OVALDefinitionVersion] [int] NOT NULL,
	[variable_instance] [int] NULL,
	[OVALClassEnumerationID] [int] NULL,
	[ClassValue] [nvarchar](50) NULL,
	[ResultEnumerationID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONTYPEFOROVALSYSTEMTYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONTYPEFOROVALSYSTEMTYPE](
	[OVALSystemTypeID] [int] NOT NULL,
	[OVALDefinitionTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONVULNERABILITY]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONVULNERABILITY](
	[OVALDefinitionVulnerabilityID] [int] IDENTITY(1,1) NOT NULL,
	[OVALDefinitionID] [int] NULL,
	[OVALDefinitionVulnerabilityRelationship] [nvarchar](50) NULL,
	[VulnerabilityID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDIRECTIVE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDIRECTIVE](
	[OVALDirectiveID] [int] IDENTITY(1,1) NOT NULL,
	[reported] [bit] NOT NULL,
	[ContentEnumerationValue] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDIRECTIVES]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDIRECTIVES](
	[OVALDirectivesID] [int] IDENTITY(1,1) NOT NULL,
	[GeneratorTypeID] [int] NOT NULL,
	[OVALDefaultDirectivesID] [int] NOT NULL,
	[signature] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDIRECTIVESTYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDIRECTIVESTYPE](
	[OVALDirectivesTypeID] [int] IDENTITY(1,1) NOT NULL,
	[definition_trueOVALDirectiveID] [int] NOT NULL,
	[definition_falseOVALDirectiveID] [int] NOT NULL,
	[definition_unknownOVALDirectiveID] [int] NOT NULL,
	[definition_errorDirectiveID] [int] NOT NULL,
	[definition_not_evaluatedDirectiveID] [int] NOT NULL,
	[definition_not_applicableDirectiveID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALENTITYATTRIBUTEGROUP]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALENTITYATTRIBUTEGROUP](
	[OVALEntityAttributeGroupID] [int] IDENTITY(1,1) NOT NULL,
	[SimpleDataTypeID] [int] NULL,
	[DataTypeName] [nvarchar](50) NULL,
	[OperationEnumerationID] [int] NULL,
	[OperationValue] [nvarchar](50) NULL,
	[mask] [bit] NULL,
	[OVALVariableID] [int] NULL,
	[OVALVariableIDPattern] [nvarchar](500) NULL,
	[CheckEnumerationID] [int] NULL,
	[EnumerationValue] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALENTITYCOMPLEXBASE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALENTITYCOMPLEXBASE](
	[OVALEntityComplexBaseID] [int] IDENTITY(1,1) NOT NULL,
	[OVALEntityAttributeGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALENTITYSIMPLEBASE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALENTITYSIMPLEBASE](
	[OVALEntitySimpleBaseID] [int] IDENTITY(1,1) NOT NULL,
	[OVALEntityAttributeGroupID] [int] NOT NULL,
	[SimpleBaseValue] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALEXTENSIONPOINT]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALEXTENSIONPOINT](
	[ExtensionPointID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALEXTENSIONPOINTFOROVALGENERATORTYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALEXTENSIONPOINTFOROVALGENERATORTYPE](
	[GeneratorTypeID] [int] NOT NULL,
	[ExtensionPointID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALEXTENSIONPOINTFORSYSTEMINFO]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALEXTENSIONPOINTFORSYSTEMINFO](
	[SystemInfoID] [int] NOT NULL,
	[OVALExtensionPointID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALFILTER]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALFILTER](
	[OVALFilterID] [int] IDENTITY(1,1) NOT NULL,
	[OVALStateID] [int] NOT NULL,
	[FilterActionValue] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALFILTERFOROVALSET]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALFILTERFOROVALSET](
	[OVALSetID] [int] NOT NULL,
	[OVALFilterID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALGENERATORTYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALGENERATORTYPE](
	[GeneratorTypeID] [int] IDENTITY(1,1) NOT NULL,
	[productname] [nvarchar](150) NULL,
	[productversion] [nvarchar](50) NULL,
	[schemaversion] [float] NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALITEM]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALITEM](
	[OVALItemID] [int] IDENTITY(1,1) NOT NULL,
	[OVALItemIDPattern] [nvarchar](500) NOT NULL,
	[StatusID] [int] NULL,
	[StatusName] [nvarchar](150) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALITEMATTRIBUTEGROUP]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALITEMATTRIBUTEGROUP](
	[OVALItemAttributeGroupID] [int] IDENTITY(1,1) NOT NULL,
	[DataTypeName] [nvarchar](50) NULL,
	[mask] [bit] NULL,
	[StatusName] [nvarchar](150) NULL,
	[OVALItemIDPattern] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALITEMCOMPLEXBASE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALITEMCOMPLEXBASE](
	[OVALItemComplexBaseID] [int] IDENTITY(1,1) NOT NULL,
	[OVALItemAttributeGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALITEMFOROVALSYSTEMOBJECT]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALITEMFOROVALSYSTEMOBJECT](
	[OVALSystemObjectID] [int] NOT NULL,
	[OVALItemID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALITEMSIMPLEBASE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALITEMSIMPLEBASE](
	[OVALItemSimpleBaseID] [int] IDENTITY(1,1) NOT NULL,
	[OVALItemAttributeGroupID] [int] NOT NULL,
	[EntityValue] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALLITERALCOMPONENT]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALLITERALCOMPONENT](
	[OVALLiteralComponentID] [int] IDENTITY(1,1) NOT NULL,
	[SimpleDataTypeID] [int] NULL,
	[LiteralComponentValue] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALLITERALCOMPONENTFOROVALCOMPONENTGROUP]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALLITERALCOMPONENTFOROVALCOMPONENTGROUP](
	[OVALComponentGroupLiteralComponentID] [int] IDENTITY(1,1) NOT NULL,
	[OVALComponentGroupID] [int] NOT NULL,
	[OVALLiteralComponentID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALMESSAGETYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALMESSAGETYPE](
	[MessageTypeID] [int] IDENTITY(1,1) NOT NULL,
	[MessageLevelValue] [nvarchar](50) NULL,
	[MessageLevelID] [int] NULL,
	[MessageText] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALMESSAGETYPEFOROVALDEFINITIONTYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALMESSAGETYPEFOROVALDEFINITIONTYPE](
	[OVALDefinitionTypeID] [int] NOT NULL,
	[OVALMessageTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALMESSAGETYPEFOROVALITEM]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALMESSAGETYPEFOROVALITEM](
	[OVALItemID] [int] NOT NULL,
	[MessageTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALMESSAGETYPEFOROVALSYSTEMOBJECT]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALMESSAGETYPEFOROVALSYSTEMOBJECT](
	[OVALSystemObjectID] [int] NOT NULL,
	[MessageTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALMESSAGETYPEFOROVALTESTEDITEM]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALMESSAGETYPEFOROVALTESTEDITEM](
	[OVALTestedItemID] [int] NOT NULL,
	[MessageTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALMESSAGETYPEFOROVALTESTTYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALMESSAGETYPEFOROVALTESTTYPE](
	[OVALTestTypeID] [int] NOT NULL,
	[OVALMessageTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALNAMESPACE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALNAMESPACE](
	[OVALNamespaceID] [int] IDENTITY(1,1) NOT NULL,
	[OVALNamespaceName] [nvarchar](50) NULL,
	[OVALNamespaceDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECT]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECT](
	[OVALObjectID] [int] IDENTITY(1,1) NOT NULL,
	[OVALObjectIDPattern] [nvarchar](500) NOT NULL,
	[OVALObjectVersion] [int] NOT NULL,
	[OVALObjectGUID] [nvarchar](500) NULL,
	[comment] [nvarchar](max) NOT NULL,
	[deprecated] [bit] NULL,
	[notes] [nvarchar](max) NULL,
	[signature] [nvarchar](max) NULL,
	[OVALObjectDataTypeID] [int] NULL,
	[OVALNamespaceID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[RepositoryID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTCOMPONENT]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTCOMPONENT](
	[OVALObjectComponentID] [int] IDENTITY(1,1) NOT NULL,
	[OVALObjectID] [int] NOT NULL,
	[OVALItemEntityName] [nvarchar](500) NOT NULL,
	[OVALItemEntityRecord] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTCOMPONENTFOROVALCOMPONENTGROUP]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTCOMPONENTFOROVALCOMPONENTGROUP](
	[OVALComponentGroupObjectComponentID] [int] IDENTITY(1,1) NOT NULL,
	[OVALComponentGroupID] [int] NOT NULL,
	[OVALObjectComponentID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTDATATYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTDATATYPE](
	[OVALObjectDataTypeID] [int] IDENTITY(1,1) NOT NULL,
	[OVALObjectDataTypeName] [nvarchar](50) NULL,
	[OVALObjectDataTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTFIELD]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTFIELD](
	[OVALObjectFieldID] [int] IDENTITY(1,1) NOT NULL,
	[OVALEntityAttributeGroupID] [int] NULL,
	[FieldName] [nvarchar](500) NOT NULL,
	[OperationEnumerationID] [int] NULL,
	[FieldValue] [nvarchar](500) NULL,
	[DataTypeName] [nvarchar](50) NULL,
	[OVALNamespaceID] [int] NULL,
	[Namespace] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CheckEnumerationID] [int] NULL,
	[OVALVariableID] [int] NULL,
	[VarRef] [nvarchar](500) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTFIELDFOROVALOBJECTRECORD]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTFIELDFOROVALOBJECTRECORD](
	[OVALObjectRecordFieldID] [int] IDENTITY(1,1) NOT NULL,
	[OVALObjectRecordID] [int] NOT NULL,
	[OVALObjectFieldID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTFILE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTFILE](
	[OVALObjectFileID] [int] IDENTITY(1,1) NOT NULL,
	[OVALObjectID] [int] NULL,
	[FileID] [int] NULL,
	[OVALVariableID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTFOROVALSET]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTFOROVALSET](
	[OVALSetObjectID] [int] IDENTITY(1,1) NOT NULL,
	[OVALSetID] [int] NOT NULL,
	[OVALObjectID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTFOROVALTEST]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTFOROVALTEST](
	[OVALTestObjectID] [int] IDENTITY(1,1) NOT NULL,
	[OVALTestID] [int] NOT NULL,
	[OVALObjectID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTRECORD]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTRECORD](
	[OVALObjectRecordID] [int] IDENTITY(1,1) NOT NULL,
	[OVALObjectDataTypeID] [int] NULL,
	[OperationValue] [nvarchar](50) NULL,
	[mask] [bit] NULL,
	[OVALVariableIDPattern] [nvarchar](500) NULL,
	[EnumerationValue] [nvarchar](50) NULL,
	[OVALNamespaceID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTRECORDFOROVALOBJECT]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTRECORDFOROVALOBJECT](
	[OVALObjectObjectRecordID] [int] IDENTITY(1,1) NOT NULL,
	[OVALObjectID] [int] NOT NULL,
	[OVALObjectRecordID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTTAG]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTTAG](
	[OVALObjectTagID] [int] IDENTITY(1,1) NOT NULL,
	[OVALObjectID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTWINDOWSREGISTRYKEY]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTWINDOWSREGISTRYKEY](
	[OVALObjectWindowsRegistryKeyID] [int] IDENTITY(1,1) NOT NULL,
	[OVALObjectID] [int] NOT NULL,
	[OVALObjectGUID] [nvarchar](500) NULL,
	[operation] [nvarchar](50) NULL,
	[WindowsRegistryKeyObjectID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALRESULTS]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALRESULTS](
	[OVALResultsID] [int] IDENTITY(1,1) NOT NULL,
	[GeneratorTypeID] [int] NOT NULL,
	[OVALDefaultDirectivesID] [int] NOT NULL,
	[OVALDefinitionsID] [int] NULL,
	[OVALResultsTypeID] [int] NOT NULL,
	[signature] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALRESULTSTYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALRESULTSTYPE](
	[OVALResultsTypeId] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSET]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSET](
	[OVALSetID] [int] IDENTITY(1,1) NOT NULL,
	[SetOperatorValue] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSETFOROVALSET]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSETFOROVALSET](
	[OVALSetRefID] [int] NOT NULL,
	[OVALSetSubjectID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSTATE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSTATE](
	[OVALStateID] [int] IDENTITY(1,1) NOT NULL,
	[OVALStateIDPattern] [nvarchar](500) NOT NULL,
	[OVALStateVersion] [int] NULL,
	[OVALStateSimpleBaseID] [int] NULL,
	[OVALStateComplexBaseID] [int] NULL,
	[OVALStateTypeID] [int] NULL,
	[DataTypeName] [nvarchar](50) NULL,
	[OperatorEnumerationID] [int] NULL,
	[comment] [nvarchar](max) NOT NULL,
	[deprecated] [bit] NULL,
	[notes] [nvarchar](max) NULL,
	[signature] [nvarchar](max) NULL,
	[OVALNamespaceID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[RepositoryID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSTATECOMPLEXBASE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSTATECOMPLEXBASE](
	[OVALStateComplexBaseID] [int] IDENTITY(1,1) NOT NULL,
	[CheckEnumerationID] [int] NULL,
	[EnumerationValue] [nvarchar](50) NULL,
	[DataTypeName] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSTATEFIELD]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSTATEFIELD](
	[OVALStateFieldID] [int] IDENTITY(1,1) NOT NULL,
	[OVALEntityAttributeGroupID] [int] NULL,
	[FieldName] [nvarchar](500) NULL,
	[DataTypeName] [nvarchar](50) NULL,
	[OperationEnumerationID] [int] NULL,
	[CheckEnumerationID] [int] NULL,
	[FieldValue] [nvarchar](max) NULL,
	[OVALNamespaceID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[OVALVariableID] [int] NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSTATEFIELDFOROVALSTATERECORD]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSTATEFIELDFOROVALSTATERECORD](
	[OVALStateRecordStateFieldID] [int] IDENTITY(1,1) NOT NULL,
	[OVALStateRecordID] [int] NOT NULL,
	[OVALStateFieldID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSTATEFOROVALTEST]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSTATEFOROVALTEST](
	[OVALTestStateID] [int] IDENTITY(1,1) NOT NULL,
	[OVALTestID] [int] NOT NULL,
	[OVALStateID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSTATERECORD]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSTATERECORD](
	[OVALStateRecordID] [int] IDENTITY(1,1) NOT NULL,
	[OVALStateComplexBaseID] [int] NULL,
	[OVALStateTypeID] [int] NULL,
	[DataTypeName] [nvarchar](50) NOT NULL,
	[OperationEnumerationID] [int] NULL,
	[mask] [bit] NULL,
	[OVALVariableID] [int] NULL,
	[CheckEnumerationID] [int] NULL,
	[OVALNamespaceID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[RepositoryID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSTATERECORDFOROVALSTATE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSTATERECORDFOROVALSTATE](
	[OVALStateStateRecordID] [int] IDENTITY(1,1) NOT NULL,
	[OVALStateID] [int] NOT NULL,
	[OVALStateRecordID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[RepositoryID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSTATESIMPLEBASE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSTATESIMPLEBASE](
	[OVALStateSimpleBaseID] [int] IDENTITY(1,1) NOT NULL,
	[CheckEnumerationID] [int] NULL,
	[EnumerationValue] [nvarchar](50) NULL,
	[EntityValue] [nvarchar](500) NULL,
	[DataTypeName] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSTATETYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSTATETYPE](
	[OVALStateTypeID] [int] IDENTITY(1,1) NOT NULL,
	[OVALStateTypeName] [nvarchar](100) NULL,
	[OVALStateTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSYSTEMCHARACTERISTICS]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSYSTEMCHARACTERISTICS](
	[OVALSystemCharacteristicsID] [int] IDENTITY(1,1) NOT NULL,
	[GeneratorTypeID] [int] NOT NULL,
	[signature] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSYSTEMOBJECT]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSYSTEMOBJECT](
	[OVALSystemObjectID] [int] IDENTITY(1,1) NOT NULL,
	[OVALObjectID] [int] NOT NULL,
	[OVALObjectIDPattern] [nvarchar](500) NOT NULL,
	[OVALObjectVersion] [int] NOT NULL,
	[VariableInstance] [int] NULL,
	[comment] [nvarchar](max) NULL,
	[FlagID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSYSTEMTYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSYSTEMTYPE](
	[OVALSystemTypeID] [int] IDENTITY(1,1) NOT NULL,
	[OVALSystemCharacteristicsID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSYSTEMTYPEFOROVALRESULTSTYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSYSTEMTYPEFOROVALRESULTSTYPE](
	[OVALResultsTypeID] [int] NOT NULL,
	[OVALSystemTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALTEST]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALTEST](
	[OVALTestID] [int] IDENTITY(1,1) NOT NULL,
	[OVALTestIDPattern] [nvarchar](500) NOT NULL,
	[OVALTestVersion] [int] NOT NULL,
	[ExistenceEnumerationID] [int] NULL,
	[CheckEnumerationID] [int] NULL,
	[OperatorEnumerationID] [int] NULL,
	[comment] [nvarchar](max) NOT NULL,
	[deprecated] [bit] NULL,
	[notes] [nvarchar](max) NULL,
	[signature] [nvarchar](max) NULL,
	[OVALTestDataTypeID] [int] NULL,
	[OVALNamespaceID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[RepositoryID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALTESTDATATYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALTESTDATATYPE](
	[OVALTestDataTypeID] [int] IDENTITY(1,1) NOT NULL,
	[OVALTestDataTypeName] [nvarchar](100) NULL,
	[OVALTestDataTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALTESTEDITEM]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALTESTEDITEM](
	[OVALTestedItemID] [int] IDENTITY(1,1) NOT NULL,
	[OVALItemID] [int] NULL,
	[OVALItemIDPattern] [nvarchar](500) NOT NULL,
	[ResultEnumerationID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALTESTEDITEMFOROVALTESTTYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALTESTEDITEMFOROVALTESTTYPE](
	[OVALTestTypeID] [int] NOT NULL,
	[OVALTestedItemID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALTESTEDVARIABLE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALTESTEDVARIABLE](
	[OVALTestedVariableID] [int] IDENTITY(1,1) NOT NULL,
	[OVALVariableID] [int] NULL,
	[OVALVariableIDPattern] [nvarchar](500) NOT NULL,
	[OVALVariableValue] [nvarchar](500) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALTESTEDVARIABLEFOROVALTESTTYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALTESTEDVARIABLEFOROVALTESTTYPE](
	[OVALTestTypeID] [int] NOT NULL,
	[OVALTestedVariableId] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALTESTFOROVALTESTS]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALTESTFOROVALTESTS](
	[OVALTestsTestID] [int] IDENTITY(1,1) NOT NULL,
	[OVALTestsID] [int] NOT NULL,
	[OVALTestID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALTESTS]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALTESTS](
	[OVALTestsID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALTESTTAG]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALTESTTAG](
	[OVALTestTagID] [int] IDENTITY(1,1) NOT NULL,
	[OVALTestID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALTESTTYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALTESTTYPE](
	[OVALTestTypeID] [int] IDENTITY(1,1) NOT NULL,
	[OVALTestID] [int] NULL,
	[OVALTestIDPattern] [nvarchar](500) NOT NULL,
	[OVALTestVersion] [int] NOT NULL,
	[variable_instance] [int] NULL,
	[ExistenceEnumerationID] [int] NULL,
	[ExistenceValue] [nvarchar](50) NULL,
	[CheckEnumerationID] [int] NULL,
	[EnumerationValue] [nvarchar](50) NOT NULL,
	[OperatorEnumerationID] [int] NULL,
	[OperatorValue] [nvarchar](50) NULL,
	[ResultEnumerationID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALTESTTYPEFOROVALSYSTEMTYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALTESTTYPEFOROVALSYSTEMTYPE](
	[OVALSystemTypeTestTypeID] [int] IDENTITY(1,1) NOT NULL,
	[OVALSystemTypeID] [int] NOT NULL,
	[OVALTestTypeID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALVARIABLE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALVARIABLE](
	[OVALVariableID] [int] IDENTITY(1,1) NOT NULL,
	[OVALVariableIDPattern] [nvarchar](500) NOT NULL,
	[OVALVariableVersion] [int] NOT NULL,
	[OVALVariableDataTypeID] [int] NULL,
	[comment] [nvarchar](max) NOT NULL,
	[deprecated] [bit] NULL,
	[signature] [nvarchar](max) NULL,
	[OVALNamespaceID] [int] NULL,
	[OVALVariableTypeID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALVARIABLECOMPONENT]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALVARIABLECOMPONENT](
	[OVALVariableComponentID] [int] IDENTITY(1,1) NOT NULL,
	[OVALVariableID] [int] NOT NULL,
	[OVALItemFieldName] [nvarchar](50) NULL,
	[OVALObjectRefID] [int] NULL,
	[OVALVariableRefID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALVARIABLECOMPONENTFOROVALCOMPONENTGROUP]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALVARIABLECOMPONENTFOROVALCOMPONENTGROUP](
	[OVALComponentGroupVariableComponentID] [int] IDENTITY(1,1) NOT NULL,
	[OVALComponentGroupID] [int] NOT NULL,
	[OVALVariableComponentID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALVARIABLEDATATYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALVARIABLEDATATYPE](
	[OVALVariableDataTypeID] [int] IDENTITY(1,1) NOT NULL,
	[OVALVariableDataTypeName] [nvarchar](50) NULL,
	[OVALVariableDataTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALVARIABLEFOROVALVARIABLES]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALVARIABLEFOROVALVARIABLES](
	[OVALVariablesID] [int] NOT NULL,
	[OVALVariableID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALVARIABLES]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALVARIABLES](
	[OVALVariablesID] [int] IDENTITY(1,1) NOT NULL,
	[OVALGeneratorTypeID] [int] NOT NULL,
	[signature] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALVARIABLETAG]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALVARIABLETAG](
	[OVALVariableTagID] [int] IDENTITY(1,1) NOT NULL,
	[OVALVariableID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALVARIABLETYPE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALVARIABLETYPE](
	[OVALVariableTypeID] [int] IDENTITY(1,1) NOT NULL,
	[OVALVariableTypeName] [nvarchar](50) NULL,
	[OVALVariableTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALVARIABLEVALUE]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALVARIABLEVALUE](
	[OVALVariableValueID] [int] IDENTITY(1,1) NOT NULL,
	[OVALVariableID] [int] NOT NULL,
	[OVALVariableGUID] [nvarchar](500) NULL,
	[ValueID] [int] NOT NULL,
	[ValueValue] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALVARIABLEVALUEFOROVALSYSTEMOBJECT]    Script Date: 04/03/2015 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALVARIABLEVALUEFOROVALSYSTEMOBJECT](
	[OVALSystemObjectVariableValueID] [int] IDENTITY(1,1) NOT NULL,
	[OVALSystemObjectID] [int] NOT NULL,
	[OVALVariableValueID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO


