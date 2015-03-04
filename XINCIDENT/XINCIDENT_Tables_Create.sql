/****** 
Copyright (C) 2014-2015 Jerome Athias
Incidents related tables for XORCISM database
This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
******/

USE [XINCIDENT]
GO

/****** Object:  Table [dbo].[INCIDENT]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENT](
	[IncidentID] [int] IDENTITY(1,1) NOT NULL,
	[source_id] [nvarchar](100) NULL,
	[IncidentCategoryID] [int] NULL,
	[publication_status] [nvarchar](50) NULL,
	[datetime_reported] [datetimeoffset](7) NULL,
	[start_datetime] [datetimeoffset](7) NULL,
	[end_datetime] [datetimeoffset](7) NULL,
	[detect_datetime] [datetimeoffset](7) NULL,
	[confirmed] [bit] NULL,
	[security_compromise] [nvarchar](100) NULL,
	[exercise] [bit] NULL,
	[ProjectID] [int] NULL,
	[exercise_name] [nvarchar](50) NULL,
	[import_datetime] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[IncidentStatusID] [int] NULL,
	[status] [nvarchar](50) NULL,
	[status_description] [nvarchar](max) NULL,
	[synopsis] [nvarchar](max) NULL,
	[summary] [nvarchar](max) NULL,
	[impact] [nvarchar](250) NULL,
	[confidence] [nvarchar](50) NULL,
	[notes] [nvarchar](max) NULL,
	[locations_affected] [int] NULL,
	[IncidentDiscoveryMethodID] [int] NULL,
	[control_failure] [nvarchar](max) NULL,
	[corrective_action] [nvarchar](max) NULL,
	[AlternativeID] [nvarchar](100) NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTCATEGORY]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTCATEGORY](
	[IncidentCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentCategoryGUID] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[IncidentCategoryName] [nvarchar](150) NOT NULL,
	[IncidentCategoryDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTCATEGORYDESCRIPTION]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTCATEGORYDESCRIPTION](
	[IncidentCategoryDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentCategoryID] [int] NOT NULL,
	[IncidentCategoryGUID] [nvarchar](500) NULL,
	[DescriptionID] [int] NOT NULL,
	[DescriptionGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTCATEGORYRACIMATRIX]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTCATEGORYRACIMATRIX](
	[IncidentCategoryRACIMatrixID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTCOMPROMISE]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTCOMPROMISE](
	[IncidentCompromiseID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentCompromiseGUID] [nvarchar](500) NULL,
	[SecurityCompromise] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL,
	[documentation] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTDISCOVERYMETHOD]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTDISCOVERYMETHOD](
	[IncidentDiscoveryMethodID] [int] IDENTITY(1,1) NOT NULL,
	[DiscoveryMethodID] [int] NULL,
	[IncidentDiscoveryMethodName] [nvarchar](100) NOT NULL,
	[IncidentDiscoveryMethodDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTEFFECT]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTEFFECT](
	[IncidentEffectID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentEffectGUID] [nvarchar](500) NULL,
	[PossibleEffect] [nvarchar](150) NOT NULL,
	[IncidentEffectDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTFORASSET]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTFORASSET](
	[AssetIncidentID] [int] IDENTITY(1,1) NOT NULL,
	[AssetIncidentGUID] [nvarchar](500) NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[AssetIncidentRelationship] [nvarchar](50) NULL,
	[AssetIncidentDescription] [nvarchar](max) NULL,
	[IncidentID] [int] NOT NULL,
	[IncidentGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[notes] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTFORINCIDENT]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTFORINCIDENT](
	[IncidentRefID] [int] NOT NULL,
	[relationshiptype] [nvarchar](50) NULL,
	[relationshipscope] [nvarchar](50) NULL,
	[IncidentSubjectID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTFORPERSON]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTFORPERSON](
	[IncidentID] [int] NOT NULL,
	[PersonID] [int] NOT NULL,
	[relationshiptype] [nvarchar](50) NULL,
	[notes] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTFORTHREATCAMPAIGN]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTFORTHREATCAMPAIGN](
	[IncidentID] [int] NOT NULL,
	[CampaignID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTID]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTID](
	[IncidentIDID] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[instance] [nvarchar](50) NULL,
	[restriction] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTIMPACT]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTIMPACT](
	[IncidentImpactID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentID] [int] NOT NULL,
	[IncidentImpactRatingID] [int] NULL,
	[IncidentImpactLossVarietyID] [int] NULL,
	[IncidentImpactLossRatingID] [int] NULL,
	[overall_amount] [numeric](18, 0) NULL,
	[overall_min_amount] [numeric](18, 0) NULL,
	[overall_max_amount] [numeric](18, 0) NULL,
	[iso_currency_code] [nvarchar](3) NULL,
	[notes] [nvarchar](max) NULL,
	[DateCreated] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[IncidentImpactAvailabilityVarietyID] [int] NULL,
	[IncidentImpactAvailabilityDurationLossID] [int] NULL,
	[IncidentImpactIntegrityVarietyID] [int] NULL,
	[IncidentImpactConfidentialityStateID] [int] NULL,
	[IncidentImpactConfidentialityVarietyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTIMPACTAVAILABILITYLOSSDURATION]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTIMPACTAVAILABILITYLOSSDURATION](
	[IncidentImpactAvailabilityLossDurationID] [int] IDENTITY(1,1) NOT NULL,
	[LossDuration] [nvarchar](50) NOT NULL,
	[LossDurationDescription] [nvarchar](100) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTIMPACTAVAILABILITYVARIETY]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTIMPACTAVAILABILITYVARIETY](
	[IncidentImpactAvailabilityVarietyID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentImpactAvailabilityVarietyName] [nvarchar](50) NULL,
	[IncidentImpactAvailabilityVarietyDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTIMPACTCONFIDENTIALITYSTATE]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTIMPACTCONFIDENTIALITYSTATE](
	[IncidentImpactConfidentialityStateID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentImpactConfidentialityStateName] [nvarchar](50) NOT NULL,
	[IncidentImpactConfidentialityStateDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTIMPACTCONFIDENTIALITYVARIETY]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTIMPACTCONFIDENTIALITYVARIETY](
	[IncidentImpactConfidentialityVarietyID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentImpactConfidentialityVarietyName] [nvarchar](50) NOT NULL,
	[IncidentImpactConfidentialityVarietyDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTIMPACTINTEGRITYVARIETY]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTIMPACTINTEGRITYVARIETY](
	[IncidentImpactIntegrityVarietyID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentImpactIntegrityVarietyName] [nvarchar](50) NOT NULL,
	[IncidentImpactIntegrityVarietyDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTIMPACTLOSSPROPERTY]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTIMPACTLOSSPROPERTY](
	[IncidentImpactLossPropertyID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentImpactLossPropertyName] [nvarchar](100) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTIMPACTLOSSRATING]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTIMPACTLOSSRATING](
	[IncidentImpactLossRatingID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentImpactLossRatingName] [nvarchar](50) NOT NULL,
	[IncidentImpactLossRatingDescription] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTIMPACTLOSSVARIETY]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTIMPACTLOSSVARIETY](
	[IncidentImpactLossVarietyID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentImpactLossVarietyName] [nvarchar](100) NOT NULL,
	[IncidentImpactLossVarietyDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTIMPACTRATING]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTIMPACTRATING](
	[IncidentImpactRatingID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentImpactRatingName] [nvarchar](50) NOT NULL,
	[IncidentImpactRatingDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTINQUIRY]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTINQUIRY](
	[IncidentIQID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentInquiryIntentID] [int] NULL,
	[purpose] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[format] [nvarchar](50) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[lang] [nvarchar](50) NULL,
	[restriction] [nvarchar](50) NULL,
	[IODEFversion] [nvarchar](50) NULL,
	[formatid] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTINQUIRYINTENT]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTINQUIRYINTENT](
	[IncidentInquiryIntentID] [int] IDENTITY(1,1) NOT NULL,
	[PackageIntent] [nvarchar](100) NOT NULL,
	[PackageIntentDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTIOC]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTIOC](
	[IncidentIOCID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentID] [int] NOT NULL,
	[comment] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[IncidentIOCTypeID] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTIOCFORTHREATCAMPAIGN]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTIOCFORTHREATCAMPAIGN](
	[IncidentIOCID] [int] NOT NULL,
	[ThreatCampaignID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTIOCTYPE]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTIOCTYPE](
	[IncidentIOCTypeID] [int] IDENTITY(1,1) NOT NULL,
	[IndicatorTypeName] [nvarchar](50) NOT NULL,
	[IndicatorTypeDocumentaion] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTIOCTYPEFORINDICATOR]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTIOCTYPEFORINDICATOR](
	[IncidentIOCTypeID] [int] NOT NULL,
	[IndicatorID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTREGISTRYHANDLE]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTREGISTRYHANDLE](
	[IncidentRegistryHandleID] [int] IDENTITY(1,1) NOT NULL,
	[registry] [nvarchar](200) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTSTATUS]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTSTATUS](
	[IncidentStatusID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentStatusGUID] [nvarchar](500) NULL,
	[IncidentStatusName] [nvarchar](50) NOT NULL,
	[IncidentStatusDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTTIMELINE]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTTIMELINE](
	[IncidentTimelineID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentID] [int] NOT NULL,
	[investigationDate] [datetimeoffset](7) NULL,
	[incidentDate] [datetimeoffset](7) NULL,
	[TimetoCompromiseValue] [int] NULL,
	[TimetoCompromiseUnit] [nvarchar](10) NULL,
	[TimetoExfiltrationValue] [int] NULL,
	[TimetoExfiltrationUnit] [nvarchar](10) NULL,
	[TimetoDiscoveryValue] [int] NULL,
	[TimetoDiscoveryUnit] [nvarchar](10) NULL,
	[TimetoContainmentValue] [int] NULL,
	[TimetoContainmentUnit] [nvarchar](10) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTTIMELINEUNIT]    Script Date: 04/03/2015 19:13:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTTIMELINEUNIT](
	[IncidentTimelineUnitID] [int] IDENTITY(1,1) NOT NULL,
	[TimeUnit] [nvarchar](10) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO


