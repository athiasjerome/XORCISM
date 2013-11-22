/****** XORCISM eXpandable Open Research on Cyber Information Security Models, Data Model v0.9 ******/
/****** Tables creation script  ******/
/****** Copyright (C) 2013 Jerome Athias ******/
/******
This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
******/

USE [XORCISM]
GO

/****** Object:  Table [dbo].[ACCOUNT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACCOUNT](
	[AccountID] [int] IDENTITY(1,1) NOT NULL,
	[AccountName] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[AccountDescription] [nvarchar](max) NULL,
	[AccountDomain] [nvarchar](500) NULL,
	[disabled] [bit] NULL,
	[locked_out] [bit] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[PersonID] [int] NULL,
	[OrganisationID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACRONYM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACRONYM](
	[AcronymID] [int] IDENTITY(1,1) NOT NULL,
	[AcronymAbbreviation] [nvarchar](20) NOT NULL,
	[AcronymPhrase] [nvarchar](500) NOT NULL,
	[AcronymDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTION](
	[ActionID] [int] IDENTITY(1,1) NOT NULL,
	[ActionGUID] [nvarchar](500) NULL,
	[ActionREFID] [nvarchar](500) NULL,
	[ActionStatusID] [int] NULL,
	[ActionStatusName] [nvarchar](50) NULL,
	[ordinal_position] [int] NULL,
	[ActionContextID] [int] NULL,
	[ActionContextName] [nvarchar](50) NULL,
	[ActionTimestamp] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ActionDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONACTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONACTION](
	[ActionRefID] [int] NOT NULL,
	[ActionRelationshipTypeID] [int] NOT NULL,
	[ActionRelationshipTypeName] [nvarchar](150) NOT NULL,
	[ActionSubjectID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONACTIONARGUMENTNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONACTIONARGUMENTNAME](
	[ActionID] [int] NOT NULL,
	[ActionArgumentNameID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONACTIONNAME](
	[ActionID] [int] NOT NULL,
	[ActionNameID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONACTIONTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONACTIONTYPE](
	[ActionID] [int] NOT NULL,
	[ActionTypeID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONALIAS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONALIAS](
	[ActionID] [int] NOT NULL,
	[ActionAlias] [nvarchar](250) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONARGUMENTNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONARGUMENTNAME](
	[ActionArgumentNameID] [int] IDENTITY(1,1) NOT NULL,
	[ActionArgumentNameName] [nvarchar](150) NOT NULL,
	[ActionArgumentNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONASSOCIATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONASSOCIATION](
	[ActionAssociationID] [int] IDENTITY(1,1) NOT NULL,
	[ActionObjectAssociationType] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONCOLLECTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONCOLLECTION](
	[ActionCollectionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONCONTEXT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONCONTEXT](
	[ActionContextID] [int] IDENTITY(1,1) NOT NULL,
	[ActionContextName] [nvarchar](50) NOT NULL,
	[ActionContextDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONCYBEROBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONCYBEROBJECT](
	[ActionID] [int] NOT NULL,
	[CyberObjectID] [int] NOT NULL,
	[ActionObjectAssociationTypeID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONDESCRIPTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONDESCRIPTION](
	[ActionDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[ActionID] [int] NOT NULL,
	[ActionDescription] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONDISCOVERYMETHOD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONDISCOVERYMETHOD](
	[ActionID] [int] NOT NULL,
	[DiscoveryMethodID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONIMPLEMENTATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONIMPLEMENTATION](
	[ActionImplementationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONNAME](
	[ActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[ActionNameName] [nvarchar](100) NOT NULL,
	[ActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONOBJECTASSOCIATIONTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONOBJECTASSOCIATIONTYPE](
	[ActionObjectAssociationTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ActionObjectAssociationTypeName] [nvarchar](150) NOT NULL,
	[ActionObjectAssociationTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONPERTINENTCYBEROBJECTPROPERTY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONPERTINENTCYBEROBJECTPROPERTY](
	[ActionID] [int] NOT NULL,
	[CyberObjectPropertyID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONPOOL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONPOOL](
	[ActionPoolID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONRELATIONSHIPTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONRELATIONSHIPTYPE](
	[ActionRelationshipTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ActionRelationshipTypeName] [nvarchar](150) NOT NULL,
	[ActionRelationshipTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONSTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONSTATUS](
	[ActionStatusID] [int] IDENTITY(1,1) NOT NULL,
	[ActionStatusName] [nvarchar](50) NOT NULL,
	[ActionStatusDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONTAKEN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONTAKEN](
	[ActionTakenID] [int] IDENTITY(1,1) NOT NULL,
	[ActionName] [nvarchar](100) NOT NULL,
	[ActionDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONTAKENFORINCIDENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONTAKENFORINCIDENT](
	[ActionTakenForIncidentID] [int] IDENTITY(1,1) NOT NULL,
	[ActionTakenID] [int] NOT NULL,
	[IncidentID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONTAKENFORTHREATCAMPAIGN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONTAKENFORTHREATCAMPAIGN](
	[ActionTakenForThreatCampaignID] [int] IDENTITY(1,1) NOT NULL,
	[ActionTakenID] [int] NOT NULL,
	[ThreatCampaignID] [int] NOT NULL,
	[ThreatActorID] [int] NULL,
	[ActionStartDate] [datetimeoffset](7) NULL,
	[ActionEndDate] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONTYPE](
	[ActionTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ActionTypeName] [nvarchar](150) NOT NULL,
	[ActionTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIVATIONZONE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIVATIONZONE](
	[ActivationZoneID] [int] IDENTITY(1,1) NOT NULL,
	[ActivationZoneText] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIVATIONZONEFORATTACKPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIVATIONZONEFORATTACKPATTERN](
	[ActivationZoneID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ADDRESS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ADDRESS](
	[AddressID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NULL,
	[category] [nvarchar](150) NULL,
	[Address_Value] [nvarchar](500) NULL,
	[VLAN_Name] [nvarchar](500) NULL,
	[VLAN_Num] [int] NULL,
	[is_source] [bit] NULL,
	[is_destination] [bit] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ADDRESSBLACKLIST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ADDRESSBLACKLIST](
	[AddressBlacklistID] [int] IDENTITY(1,1) NOT NULL,
	[AddressID] [int] NULL,
	[EmailID] [int] NULL,
	[emailaddress] [nvarchar](100) NULL,
	[is_source] [bit] NULL,
	[is_destination] [bit] NULL,
	[OrganisationID] [int] NULL,
	[PersonID] [int] NULL,
	[AssetID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ADDRESSWHITELIST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ADDRESSWHITELIST](
	[AddressWhitelistID] [int] IDENTITY(1,1) NOT NULL,
	[AddressID] [int] NULL,
	[EmailID] [int] NULL,
	[emailaddress] [nvarchar](100) NULL,
	[is_source] [bit] NULL,
	[is_destination] [bit] NULL,
	[OrganisationID] [int] NULL,
	[PersonID] [int] NULL,
	[AssetID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AFFECTEDRESOURCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AFFECTEDRESOURCE](
	[AffectedResourceID] [int] IDENTITY(1,1) NOT NULL,
	[AffectedResourceName] [nvarchar](100) NOT NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AGENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AGENT](
	[AgentID] [int] IDENTITY(1,1) NOT NULL,
	[ipaddressIPv4] [nvarchar](50) NULL,
	[AgentStatus] [nvarchar](50) NULL,
	[AgentLoadValue] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[SensorID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[API]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[API](
	[APIID] [int] IDENTITY(1,1) NOT NULL,
	[APIDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APICALL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APICALL](
	[APICallID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APIFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APIFUNCTION](
	[APIID] [int] NOT NULL,
	[FunctionID] [int] NOT NULL,
	[Normalized_Function_Name] [nvarchar](100) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APIMEMORYADDRESS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APIMEMORYADDRESS](
	[APIID] [int] NOT NULL,
	[MemoryAddressID] [int] NOT NULL,
	[FunctionID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APIPLATFORM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APIPLATFORM](
	[APIID] [int] NOT NULL,
	[PlatformID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATION](
	[ApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationName] [nvarchar](250) NOT NULL,
	[ApplicationDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONAUTHENTICATIONTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONAUTHENTICATIONTYPE](
	[ApplicationAuthenticationTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NULL,
	[AuthenticationTypeID] [int] NULL,
	[AuthenticationRank] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ApplicationAuthenticationTypeDescription] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONFORASSET](
	[AssetID] [int] NOT NULL,
	[ApplicationID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONFORORGANISATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONFORORGANISATION](
	[ApplicationForOrganisationID] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[ApplicationOrganisationID] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONFUNCTION](
	[ApplicationFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[FunctionID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONPERSON]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONPERSON](
	[ApplicationID] [int] NOT NULL,
	[PersonID] [int] NOT NULL,
	[Usage] [nvarchar](100) NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONVERSION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONVERSION](
	[ApplicationVersionID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NULL,
	[VersionID] [int] NULL,
	[ApplicationVersionDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARCHITECTURALPARADIGM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARCHITECTURALPARADIGM](
	[ArchitecturalParadigmID] [int] IDENTITY(1,1) NOT NULL,
	[ArchitecturalParadigmName] [nvarchar](150) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARCHITECTURALPARADIGMFORTECHNICALCONTEXT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARCHITECTURALPARADIGMFORTECHNICALCONTEXT](
	[ArchitecturalParadigmID] [int] NOT NULL,
	[TechnicalContextID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFASSET](
	[ARFAssetID] [int] IDENTITY(1,1) NOT NULL,
	[ARFAssetUID] [nvarchar](500) NOT NULL,
	[AssetID] [int] NULL,
	[ReferenceID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFASSETFORASSETS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFASSETFORASSETS](
	[AssetsID] [int] NOT NULL,
	[ARFAssetID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFEXTENDEDINFO]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFEXTENDEDINFO](
	[ARFExtendedInfoID] [int] IDENTITY(1,1) NOT NULL,
	[ExtendedInfoNCName] [nvarchar](500) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFEXTENDEDINFOFORARFEXTENDEDINFOS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFEXTENDEDINFOFORARFEXTENDEDINFOS](
	[ARFExtendedInfosID] [int] NOT NULL,
	[ARFExtendedInfoID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFEXTENDEDINFOS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFEXTENDEDINFOS](
	[ARFExtendedInfosID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFOBJECTREF]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFOBJECTREF](
	[ARFObjectRefID] [int] IDENTITY(1,1) NOT NULL,
	[ARFObjectRefUID] [nvarchar](500) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFOBJECTREFARFASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFOBJECTREFARFASSET](
	[ARFObjectRefID] [int] NOT NULL,
	[ARFAssetID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFOBJECTREFREPORT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFOBJECTREFREPORT](
	[ARFObjectRefID] [int] NOT NULL,
	[ReportID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFOBJECTREFREPORTREQUEST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFOBJECTREFREPORTREQUEST](
	[ARFObjectRefID] [int] NOT NULL,
	[ReportRequestID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFRELATIONSHIP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFRELATIONSHIP](
	[ARFRelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[RelationshipTypeQName] [nvarchar](500) NOT NULL,
	[RelationshipTypeID] [int] NULL,
	[RelationshipScope] [nvarchar](50) NULL,
	[RelationshipSubjectNCName] [nvarchar](500) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFRELATIONSHIPARFASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFRELATIONSHIPARFASSET](
	[ARFRelationshipID] [int] NOT NULL,
	[ARFAssetID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFRELATIONSHIPFORARFRELATIONSHIPS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFRELATIONSHIPFORARFRELATIONSHIPS](
	[ARFRelationshipsID] [int] NOT NULL,
	[ARFRelationshipID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFRELATIONSHIPREPORT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFRELATIONSHIPREPORT](
	[ARFRelationshipID] [int] NOT NULL,
	[ReportID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFRELATIONSHIPREPORTREQUEST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFRELATIONSHIPREPORTREQUEST](
	[ARFRelationshipID] [int] NOT NULL,
	[ReportRequestID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFRELATIONSHIPS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFRELATIONSHIPS](
	[ARFRelationshipsID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARITHMETICFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARITHMETICFUNCTION](
	[ArithmeticFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[ArithmeticOperationName] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARITHMETICOPERATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARITHMETICOPERATION](
	[ArithmeticOperationID] [int] IDENTITY(1,1) NOT NULL,
	[ArithmeticOperationName] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARTIFACT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARTIFACT](
	[ArtifactID] [int] IDENTITY(1,1) NOT NULL,
	[Raw_Artifact] [nvarchar](max) NULL,
	[Raw_Artifact_Reference] [nvarchar](250) NULL,
	[ArtifactTypeID] [int] NOT NULL,
	[content_type] [nvarchar](50) NULL,
	[content_type_version] [nvarchar](50) NULL,
	[suspected_malicious] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARTIFACTHASHVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARTIFACTHASHVALUE](
	[ArtifactID] [int] NOT NULL,
	[HashValueID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARTIFACTPACKAGING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARTIFACTPACKAGING](
	[ArtifactID] [int] NOT NULL,
	[PackagingID] [int] NOT NULL,
	[is_encrypted] [bit] NULL,
	[is_compressed] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARTIFACTTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARTIFACTTYPE](
	[ArtifactTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ArtifactTypeName] [nvarchar](50) NULL,
	[ArtifactTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[aspnet_Applications]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[aspnet_Applications](
	[ApplicationName] [nvarchar](256) NOT NULL,
	[LoweredApplicationName] [nvarchar](256) NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](256) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[aspnet_Membership]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[aspnet_Membership](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordFormat] [int] NOT NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[MobilePIN] [nvarchar](16) NULL,
	[Email] [nvarchar](256) NULL,
	[LoweredEmail] [nvarchar](256) NULL,
	[PasswordQuestion] [nvarchar](256) NULL,
	[PasswordAnswer] [nvarchar](128) NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[LastPasswordChangedDate] [datetime] NOT NULL,
	[LastLockoutDate] [datetime] NOT NULL,
	[FailedPasswordAttemptCount] [int] NOT NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NOT NULL,
	[FailedPasswordAnswerAttemptCount] [int] NOT NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NOT NULL,
	[Comment] [ntext] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[aspnet_Paths]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[aspnet_Paths](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[PathId] [uniqueidentifier] NOT NULL,
	[Path] [nvarchar](256) NOT NULL,
	[LoweredPath] [nvarchar](256) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[aspnet_PersonalizationAllUsers]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[aspnet_PersonalizationAllUsers](
	[PathId] [uniqueidentifier] NOT NULL,
	[PageSettings] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[aspnet_PersonalizationPerUser]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[aspnet_PersonalizationPerUser](
	[Id] [uniqueidentifier] NOT NULL,
	[PathId] [uniqueidentifier] NULL,
	[UserId] [uniqueidentifier] NULL,
	[PageSettings] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[aspnet_Profile]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[aspnet_Profile](
	[UserId] [uniqueidentifier] NOT NULL,
	[PropertyNames] [ntext] NOT NULL,
	[PropertyValuesString] [ntext] NOT NULL,
	[PropertyValuesBinary] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[aspnet_Roles]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[aspnet_Roles](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[LoweredRoleName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[aspnet_SchemaVersions]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[aspnet_SchemaVersions](
	[Feature] [nvarchar](128) NOT NULL,
	[CompatibleSchemaVersion] [nvarchar](128) NOT NULL,
	[IsCurrentVersion] [bit] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[aspnet_Users]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[aspnet_Users](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[LoweredUserName] [nvarchar](256) NOT NULL,
	[MobileAlias] [nvarchar](16) NULL,
	[IsAnonymous] [bit] NOT NULL,
	[LastActivityDate] [datetime] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[aspnet_UsersInRoles]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[aspnet_UsersInRoles](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[aspnet_WebEvent_Events]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[aspnet_WebEvent_Events](
	[EventId] [char](32) NOT NULL,
	[EventTimeUtc] [datetime] NOT NULL,
	[EventTime] [datetime] NOT NULL,
	[EventType] [nvarchar](256) NOT NULL,
	[EventSequence] [decimal](19, 0) NOT NULL,
	[EventOccurrence] [decimal](19, 0) NOT NULL,
	[EventCode] [int] NOT NULL,
	[EventDetailCode] [int] NOT NULL,
	[Message] [nvarchar](1024) NULL,
	[ApplicationPath] [nvarchar](256) NULL,
	[ApplicationVirtualPath] [nvarchar](256) NULL,
	[MachineName] [nvarchar](256) NOT NULL,
	[RequestUrl] [nvarchar](1024) NULL,
	[ExceptionType] [nvarchar](256) NULL,
	[Details] [ntext] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[ASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSET](
	[AssetID] [int] IDENTITY(1,1) NOT NULL,
	[AssetName] [nvarchar](150) NULL,
	[AssetDescription] [nvarchar](max) NULL,
	[AssetCriticalityLevel] [nvarchar](50) NULL,
	[TaskCriticalAsset] [bit] NULL,
	[DefenseCriticalAsset] [bit] NULL,
	[OSName] [nvarchar](250) NULL,
	[Enabled] [bit] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[X500name] [nvarchar](250) NULL,
	[fqdn] [nvarchar](250) NULL,
	[hostname] [nvarchar](100) NULL,
	[motherboardguid] [nvarchar](50) NULL,
	[instancename] [nvarchar](100) NULL,
	[networkname] [nvarchar](100) NULL,
	[ipnetrangestartIPv4] [nvarchar](50) NULL,
	[ipnetrangeendIPv4] [nvarchar](50) NULL,
	[ipnetrangestartIPv6] [nvarchar](50) NULL,
	[ipnetrangeendIPv6] [nvarchar](50) NULL,
	[cidr] [nvarchar](100) NULL,
	[websiteurl] [nvarchar](255) NULL,
	[documentroot] [nvarchar](100) NULL,
	[locale] [nvarchar](50) NULL,
	[installationid] [nvarchar](50) NULL,
	[license] [nvarchar](max) NULL,
	[systemname] [nvarchar](250) NULL,
	[version] [nvarchar](50) NULL,
	[ipaddressIPv4] [nvarchar](50) NULL,
	[ipaddressIPv6] [nvarchar](50) NULL,
	[subnetmaskIPv4] [nvarchar](50) NULL,
	[subnetmaskIPv6] [nvarchar](50) NULL,
	[defaultrouteIPv4] [nvarchar](50) NULL,
	[defaultrouteIPv6] [nvarchar](50) NULL,
	[personal] [bit] NULL,
	[managedbythirdparty] [bit] NULL,
	[hostedbythirdparty] [bit] NULL,
	[notes] [nvarchar](max) NULL,
	[cloud] [nvarchar](50) NULL,
	[AssetManagementID] [int] NULL,
	[AssetOwnershipID] [int] NULL,
	[AssetLocationID] [int] NULL,
	[virtual] [bit] NULL,
	[ADParticipation] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETADDRESS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETADDRESS](
	[AssetID] [int] NOT NULL,
	[AddressID] [int] NOT NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETBLACKLIST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETBLACKLIST](
	[AssetBlacklistID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[OrganisationID] [int] NULL,
	[PersonID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETCERTIFICATE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETCERTIFICATE](
	[AssetCertificateID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[CertificateID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[AssetCertificateDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETCERTIFICATEORGANISATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETCERTIFICATEORGANISATION](
	[AssetCertificateOrganisationID] [int] IDENTITY(1,1) NOT NULL,
	[AssetCertificateID] [int] NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[AssetCertificateOrganisationDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETCREDENTIAL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETCREDENTIAL](
	[AssetCredentialID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NULL,
	[AuthenticationTypeID] [int] NULL,
	[AuthenticationType] [nvarchar](50) NULL,
	[Username] [nvarchar](255) NULL,
	[Password] [nvarchar](255) NULL,
	[PersonID] [int] NULL,
	[OrganisationID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETCRITICALITYLEVEL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETCRITICALITYLEVEL](
	[AssetCriticalityLevelID] [int] IDENTITY(1,1) NOT NULL,
	[AssetCriticalityName] [nvarchar](50) NULL,
	[AssetCriticalityDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETCRITICALITYLEVELFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETCRITICALITYLEVELFORASSET](
	[AssetCriticalityID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetCriticalityLevelID] [int] NOT NULL,
	[DateCreated] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETDEVICE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETDEVICE](
	[AssetID] [int] NOT NULL,
	[DeviceID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETFORASSET](
	[AssetForAssetID] [int] IDENTITY(1,1) NOT NULL,
	[AssetRefID] [int] NOT NULL,
	[AssetRelationshipID] [int] NULL,
	[relationshiptype] [nvarchar](50) NULL,
	[relationshipscope] [nvarchar](50) NULL,
	[AssetSubjectID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETFORORGANISATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETFORORGANISATION](
	[AssetForOrganisationID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETFORTHREATACTORTTP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETFORTHREATACTORTTP](
	[AssetForThreatActorTTPID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[Information_Source] [nvarchar](250) NULL,
	[ConfidenceLevel] [nvarchar](50) NULL,
	[notes] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETFUNCTION](
	[AssetFunctionID] [int] NOT NULL,
	[AssetFunction] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETFUNCTIONFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETFUNCTIONFORASSET](
	[AssetFunctionForAssetID] [int] IDENTITY(1,1) NOT NULL,
	[AssetFunctionID] [int] NOT NULL,
	[AssetID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETGEOLOCATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETGEOLOCATION](
	[AssetGeoLocationID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[GeoLocationID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETGROUP](
	[AssetGroupID] [int] IDENTITY(1,1) NOT NULL,
	[AssetForAssetID] [int] NULL,
	[AssetGroupName] [nvarchar](250) NULL,
	[AssetGroupDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[OrganisationID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETINFORMATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETINFORMATION](
	[AssetInformationID] [int] IDENTITY(1,1) NOT NULL,
	[hostname] [nvarchar](max) NULL,
	[netbios] [nvarchar](max) NULL,
	[hosttype] [nvarchar](max) NULL,
	[JobID] [int] NOT NULL,
	[information] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETLOCATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETLOCATION](
	[AssetLocationID] [int] IDENTITY(1,1) NOT NULL,
	[AssetLocationType] [nvarchar](50) NOT NULL,
	[AssetLocationDescription] [nvarchar](100) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETLOCATIONFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETLOCATIONFORASSET](
	[AssetLocationTimeID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetLocationID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETMANAGEMENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETMANAGEMENT](
	[AssetManagementID] [int] IDENTITY(1,1) NOT NULL,
	[ManagementType] [nvarchar](50) NOT NULL,
	[ManagementDescription] [nvarchar](100) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETMANAGEMENTFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETMANAGEMENTFORASSET](
	[AssetManagementTimeID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetManagementID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETORGANIZATIONALUNIT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETORGANIZATIONALUNIT](
	[AssetOrganizationalUnitID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationalUnitID] [int] NOT NULL,
	[AssetID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETOWNERSHIP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETOWNERSHIP](
	[AssetOwnershipID] [int] IDENTITY(1,1) NOT NULL,
	[OwnershipName] [nvarchar](50) NOT NULL,
	[OwnershipDescription] [nvarchar](100) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETPHYSICALLOCATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETPHYSICALLOCATION](
	[AssetPhysicalLocationTimeID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[PhysicalLocationID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[InformationPersonID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETRELATIONSHIP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETRELATIONSHIP](
	[AssetRelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[relationshiptype] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETREPORTCOLLECTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETREPORTCOLLECTION](
	[AssetReportCollectionID] [int] IDENTITY(1,1) NOT NULL,
	[ARFReportCollectionID] [nvarchar](500) NULL,
	[ReportRequestsID] [int] NULL,
	[AssetsID] [int] NULL,
	[ReportsID] [int] NULL,
	[ARFRelationshipsID] [int] NULL,
	[ARFExtendedInfosID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETRISKRATING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETRISKRATING](
	[AssetRiskRatingID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[RiskRatingID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETROLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETROLE](
	[AssetRoleID] [int] NOT NULL,
	[AssetRole] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETROLEFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETROLEFORASSET](
	[AssetRoleForAssetID] [int] IDENTITY(1,1) NOT NULL,
	[AssetRoleID] [int] NOT NULL,
	[AssetID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETS](
	[AssetsID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETSENSOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETSENSOR](
	[AssetSensorID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[SensorID] [int] NOT NULL,
	[AssetSensorName] [nvarchar](50) NULL,
	[AssetSensorDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETSESSION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETSESSION](
	[AssetSessionID] [int] IDENTITY(1,1) NOT NULL,
	[SessionID] [int] NULL,
	[AssetID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETSYNTHETICID]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETSYNTHETICID](
	[AssetSyntheticID] [int] IDENTITY(1,1) NOT NULL,
	[resource] [nvarchar](250) NOT NULL,
	[id] [nvarchar](250) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETSYNTHETICIDFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETSYNTHETICIDFORASSET](
	[AssetSyntheticIDForAssetID] [int] NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetSyntheticID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETVALUE](
	[AssetValueID] [int] IDENTITY(1,1) NOT NULL,
	[AssetValueName] [nvarchar](50) NOT NULL,
	[AssetValueDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETVALUEFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETVALUEFORASSET](
	[AssetValueForAssetID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetValueID] [int] NOT NULL,
	[ValueValue] [float] NULL,
	[iso_currency_code] [nvarchar](3) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETVARIETY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETVARIETY](
	[AssetVarietyID] [int] IDENTITY(1,1) NOT NULL,
	[AssetVarietyName] [nvarchar](100) NOT NULL,
	[AssetVarietyDescription] [nvarchar](500) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETVARIETYFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETVARIETYFORASSET](
	[AssetVarietyForAssetID] [int] IDENTITY(1,1) NOT NULL,
	[AssetVarietyID] [int] NOT NULL,
	[AssetID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETWHITELIST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETWHITELIST](
	[AssetWhitelistID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[OrganisationID] [int] NULL,
	[PersonID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACHMENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACHMENT](
	[AttachmentID] [int] IDENTITY(1,1) NOT NULL,
	[AttachmentGUID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKCONSEQUENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKCONSEQUENCE](
	[AttackConsequenceID] [int] IDENTITY(1,1) NOT NULL,
	[Consequence] [nvarchar](100) NULL,
	[ConsequenceNote] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKCONSEQUENCEFORCAPEC]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKCONSEQUENCEFORCAPEC](
	[AttackConsequenceID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKCONSEQUENCEFORCWE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKCONSEQUENCEFORCWE](
	[AttackConsequenceForCWEID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[AttackConsequenceRankForCWE] [int] NOT NULL,
	[AttackConsequenceID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKCONSEQUENCENOTE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKCONSEQUENCENOTE](
	[AttackConsequenceNoteID] [int] IDENTITY(1,1) NOT NULL,
	[AttackConsequenceNoteText] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKCONSEQUENCENOTES]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKCONSEQUENCENOTES](
	[AttackConsequenceNoteForAttackConsequenceID] [int] IDENTITY(1,1) NOT NULL,
	[AttackConsequenceID] [int] NOT NULL,
	[AttackConsequenceNoteID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKCONSEQUENCESCOPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKCONSEQUENCESCOPE](
	[AttackConsequenceScopeID] [int] IDENTITY(1,1) NOT NULL,
	[ConsequenceScope] [nvarchar](100) NOT NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKCONSEQUENCESCOPEFORATTACKCONSEQUENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKCONSEQUENCESCOPEFORATTACKCONSEQUENCE](
	[AttackConsequenceScopeID] [int] NOT NULL,
	[AttackConsequenceID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKEXAMPLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKEXAMPLE](
	[AttackExampleID] [int] IDENTITY(1,1) NOT NULL,
	[AttackExampleDescription] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKEXAMPLEFORATTACKPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKEXAMPLEFORATTACKPATTERN](
	[AttackExampleForAttackPatternID] [int] IDENTITY(1,1) NOT NULL,
	[AttackExampleID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKMETHOD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKMETHOD](
	[AttackMethodID] [int] NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKMETHODFORATTACKPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKMETHODFORATTACKPATTERN](
	[AttackMethodID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERN](
	[AttackPatternID] [int] IDENTITY(1,1) NOT NULL,
	[capec_id] [nvarchar](20) NULL,
	[AttackPatternName] [nvarchar](250) NULL,
	[AttackPatternDescription] [nvarchar](max) NULL,
	[AttackPatternDescriptionRaw] [nvarchar](max) NULL,
	[PatternAbstraction] [nvarchar](50) NULL,
	[PatternCompleteness] [nvarchar](50) NULL,
	[PatternStatus] [nvarchar](50) NULL,
	[TypicalSeverity] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPATTERNFORTHREATACTORTTP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPATTERNFORTHREATACTORTTP](
	[AttackPatternForThreatActorTTPID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPAYLOAD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPAYLOAD](
	[AttackPayloadID] [int] IDENTITY(1,1) NOT NULL,
	[PayloadText] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPAYLOADENCODER]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPAYLOADENCODER](
	[AttackPayloadEncoderID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPayloadEncoderName] [nvarchar](100) NULL,
	[AttackPayloadEncoderDescription] [nvarchar](max) NULL,
	[AttackPayloadEncoderVersion] [nvarchar](50) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPAYLOADFORATTACKPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPAYLOADFORATTACKPATTERN](
	[AttackPayloadID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPAYLOADIMPACT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPAYLOADIMPACT](
	[AttackPayloadImpactID] [int] IDENTITY(1,1) NOT NULL,
	[PayloadActivationImpactDescription] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPAYLOADIMPACTFORATTACKPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPAYLOADIMPACTFORATTACKPATTERN](
	[AttackPayloadImpactID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPREREQUISITE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPREREQUISITE](
	[AttackPrerequisiteID] [int] IDENTITY(1,1) NOT NULL,
	[PrerequisiteText] [nvarchar](max) NOT NULL,
	[PrerequisiteTextRaw] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPREREQUISITEFORCAPEC]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPREREQUISITEFORCAPEC](
	[AttackPrerequisiteID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPURPOSE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPURPOSE](
	[AttackPurposeID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPurposeName] [nvarchar](100) NOT NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKPURPOSEFORATTACKPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKPURPOSEFORATTACKPATTERN](
	[AttackPurposeID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKRESOURCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKRESOURCE](
	[AttackResourceID] [int] IDENTITY(1,1) NOT NULL,
	[AttackResourceText] [nvarchar](max) NOT NULL,
	[AttackResourceTextRaw] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKRESOURCEFORCAPECS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKRESOURCEFORCAPECS](
	[AttackResourceID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSCENARIO]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[ATTACKSURFACE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSURFACE](
	[AttackSurfaceID] [int] IDENTITY(1,1) NOT NULL,
	[AttackSurfaceName] [nvarchar](250) NULL,
	[AttackSurfaceDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSURFACEFORATTACKPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSURFACEFORATTACKPATTERN](
	[AttackSurfaceID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSURFACELOCALITY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSURFACELOCALITY](
	[AttackSurfaceLocalityID] [int] IDENTITY(1,1) NOT NULL,
	[AttackSurfaceLocalityName] [nvarchar](100) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSURFACELOCALITYFORATTACKSURFACE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSURFACELOCALITYFORATTACKSURFACE](
	[AttackSurfaceLocalityID] [int] NOT NULL,
	[AttackSurfaceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSURFACESERVICE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSURFACESERVICE](
	[AttackSurfaceID] [int] NOT NULL,
	[TargetFunctionalServiceID] [int] NOT NULL,
	[TragerFunctionalServiceName] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSURFACETYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSURFACETYPE](
	[AttackSurfaceTypeID] [int] IDENTITY(1,1) NOT NULL,
	[AttackSurfaceTypeName] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKSURFACETYPEFORATTACKSURFACE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKSURFACETYPEFORATTACKSURFACE](
	[AttackSurfaceTypeID] [int] NOT NULL,
	[AttackSurfaceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTECHNICALIMPACT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTECHNICALIMPACT](
	[AttackTechnicalImpactID] [int] IDENTITY(1,1) NOT NULL,
	[ConsequenceTechnicalImpact] [nvarchar](100) NOT NULL,
	[ConsequenceTechnicalImpactRaw] [nvarchar](100) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTECHNICALIMPACTFORATTACKCONSEQUENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTECHNICALIMPACTFORATTACKCONSEQUENCE](
	[AttackTechnicalImpactID] [int] NOT NULL,
	[AttackConsequenceID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTOOL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTOOL](
	[AttackToolID] [int] IDENTITY(1,1) NOT NULL,
	[AttackToolTypeID] [int] NULL,
	[AttackToolName] [nvarchar](100) NOT NULL,
	[AttackToolVersion] [nvarchar](50) NULL,
	[AttackToolDescription] [nvarchar](max) NULL,
	[AttackToolAuthor] [nvarchar](100) NULL,
	[AttackToolLink] [nvarchar](250) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTOOLAUTHENTICATIONTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
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
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTOOLFORTHREATACTORTTP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTOOLFORTHREATACTORTTP](
	[AttackToolID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[notes] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTOOLMODULE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTOOLMODULE](
	[AttackToolModuleID] [int] IDENTITY(1,1) NOT NULL,
	[AttackToolModuleName] [nvarchar](250) NULL,
	[AttackToolModuleDescription] [nvarchar](max) NULL,
	[AttackToolModuleVersion] [nvarchar](50) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKTOOLMODULEAUTHENTICATIONTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[ATTACKTOOLTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKTOOLTYPE](
	[AttackToolTypeID] [int] IDENTITY(1,1) NOT NULL,
	[AttackToolTypeName] [nvarchar](100) NOT NULL,
	[AttackToolTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACKVECTOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACKVECTOR](
	[AttackVectorID] [int] IDENTITY(1,1) NOT NULL,
	[AttackVectorName] [nvarchar](150) NULL,
	[AttackvectorDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTRIBUTE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTRIBUTE](
	[AttributeID] [int] IDENTITY(1,1) NOT NULL,
	[AttributeName] [nvarchar](250) NULL,
	[AttributeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTRIBUTEVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTRIBUTEVALUE](
	[AttributeValueID] [int] IDENTITY(1,1) NOT NULL,
	[AttributeID] [int] NOT NULL,
	[AttributeValueName] [nvarchar](250) NULL,
	[AttributeValueDescription] [nvarchar](max) NULL,
	[AttributeValueType] [nvarchar](max) NULL,
	[AttributeValue] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AUDIT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AUDIT](
	[AuditID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AUDITFINDING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AUDITFINDING](
	[AuditFindingID] [int] IDENTITY(1,1) NOT NULL,
	[AuditID] [int] NOT NULL,
	[FindingID] [int] NOT NULL,
	[AuditProcedureID] [int] NULL,
	[AuditFindingName] [nvarchar](250) NULL,
	[AuditFindingDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AUDITPROCEDURE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AUDITPROCEDURE](
	[AuditProcedureID] [int] IDENTITY(1,1) NOT NULL,
	[AuditProcedureName] [nvarchar](250) NULL,
	[AuditProcedureDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AUTHENTICATIONTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AUTHENTICATIONTYPE](
	[AuthenticationTypeID] [int] IDENTITY(1,1) NOT NULL,
	[AuthenticationTypeName] [nvarchar](50) NULL,
	[AuthenticationTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AUTHOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AUTHOR](
	[AuthorID] [int] IDENTITY(1,1) NOT NULL,
	[AuthorName] [nvarchar](250) NOT NULL,
	[PersonID] [int] NULL,
	[OrganisationID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEGINFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[BEGINFUNCTION](
	[BeginFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[StartsWithCharacters] [varchar](500) NOT NULL,
	[OVALComponentGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[BEHAVIOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIOR](
	[BehaviorID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORACTIONCOMPOSITION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORACTIONCOMPOSITION](
	[BehaviorActionCompositionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORASSOCIATEDCODE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORASSOCIATEDCODE](
	[BehaviorAssociatedCodeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORCOLLECTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORCOLLECTION](
	[BehaviorCollectionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORDESCRIPTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORDESCRIPTION](
	[BehaviorDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORDISCOVERYMETHOD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORDISCOVERYMETHOD](
	[BehaviorDiscoveryMethodID] [int] IDENTITY(1,1) NOT NULL,
	[BehaviorID] [int] NULL,
	[DiscoveryMethodID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORIDMATCHINGPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORIDMATCHINGPATTERN](
	[BehaviorIDPatternID] [int] NOT NULL,
	[BehaviorID] [int] NOT NULL,
	[BehaviorIDMatchingPattern] [nvarchar](500) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORIDPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORIDPATTERN](
	[BehaviorIDPatternID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORPURPOSE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORPURPOSE](
	[BehaviorPurposeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORRELATIONSHIPS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORRELATIONSHIPS](
	[BehaviorRelationShipsID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARK](
	[BenchmarkID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkXCCDFID] [nvarchar](250) NULL,
	[lang] [nvarchar](10) NULL,
	[BenchmarkTitle] [nvarchar](250) NOT NULL,
	[BenchmarkDescription] [nvarchar](max) NULL,
	[BenchmarkVersion] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[signature] [nvarchar](max) NULL,
	[resolved] [bit] NULL,
	[style] [nvarchar](50) NULL,
	[stylehref] [nvarchar](250) NULL,
	[cpeplatform] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKCHECK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKCHECK](
	[BenchmarkCheckID] [int] IDENTITY(1,1) NOT NULL,
	[CheckContent] [nvarchar](max) NULL,
	[CheckingSystemURI] [nvarchar](250) NOT NULL,
	[negate] [bit] NULL,
	[id] [nvarchar](500) NULL,
	[selector] [nvarchar](50) NULL,
	[multicheck] [bit] NULL,
	[base] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKCHECKCONTENTREF]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKCHECKCONTENTREF](
	[BenchmarkCheckContentRefID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkCheckID] [int] NOT NULL,
	[CheckContentRefhref] [nvarchar](500) NOT NULL,
	[CheckContentName] [nvarchar](250) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKCHECKEXPORT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKCHECKEXPORT](
	[BenchmarkCheckExport] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkCheckID] [int] NOT NULL,
	[BenchmarkValueID] [int] NOT NULL,
	[CheckingSystemVariable] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKCHECKFORBENCHMARKRULE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKCHECKFORBENCHMARKRULE](
	[BenchmarkRuleID] [int] NOT NULL,
	[BenchmarkCheckID] [int] NOT NULL,
	[ValueSelectorID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKCHECKFORBENCHMARKRULERESULT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKCHECKFORBENCHMARKRULERESULT](
	[BenchmarkRuleResultID] [int] NOT NULL,
	[BenchmarkCheckID] [int] NOT NULL,
	[OutputData] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKCHECKIMPORT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKCHECKIMPORT](
	[BenchmarkCheckImportID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkCheckID] [int] NOT NULL,
	[BenchmarkTestResultID] [int] NOT NULL,
	[CheckImportValue] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKCHECKLIST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKCHECKLIST](
	[BenchmarkID] [int] NOT NULL,
	[ChecklistID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKCOMPLEXCHECK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKCOMPLEXCHECK](
	[BenchmarkComplexCheckID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkCheckRefID] [int] NULL,
	[BenchmarkCheckSubjectID] [int] NULL,
	[BenchmarkComplexCheckRefID] [int] NULL,
	[BenchmarkComplexCheckSubjectID] [int] NULL,
	[Operator] [nvarchar](5) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKCOMPLEXCHECKFORBENCHMARKRULE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKCOMPLEXCHECKFORBENCHMARKRULE](
	[BenchmarkRuleID] [int] NOT NULL,
	[BenchmarkComplexCheckID] [int] NOT NULL,
	[ValueSelectorID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKCOMPLEXCHECKFORBENCHMARKRULERESULT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKCOMPLEXCHECKFORBENCHMARKRULERESULT](
	[BenchmarkRuleResultID] [int] NOT NULL,
	[BenchmarkComplexCheckID] [int] NOT NULL,
	[OutputData] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKDCSTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKDCSTATUS](
	[BenchmarkID] [int] NOT NULL,
	[DCStatusID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKDESCRIPTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKDESCRIPTION](
	[BecnhmarkDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkID] [int] NOT NULL,
	[BenchmarkDescription] [nvarchar](max) NOT NULL,
	[lang] [nvarchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKFIXINSTANCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKFIXINSTANCE](
	[BenchmarkFixInstanceID] [int] IDENTITY(1,1) NOT NULL,
	[FixActionID] [int] NOT NULL,
	[instance] [nvarchar](500) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKFIXTEXT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKFIXTEXT](
	[BenchmarkFixTextID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkFixText] [nvarchar](max) NOT NULL,
	[lang] [nvarchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKFIXTEXTFIXACTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKFIXTEXTFIXACTION](
	[BenchmarkFixTextID] [int] NOT NULL,
	[FixActionID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKFRONT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKFRONT](
	[BenchmarkFrontID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkID] [int] NOT NULL,
	[BenchmarkFrontMatter] [nvarchar](max) NOT NULL,
	[lang] [nvarchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKGROUP](
	[BenchmarkGroupID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkGroupXCCDFID] [nvarchar](250) NULL,
	[BenchmarkGroupVersion] [nvarchar](50) NULL,
	[clusterid] [nvarchar](500) NULL,
	[hidden] [bit] NULL,
	[prohibitChanges] [bit] NULL,
	[lang] [nvarchar](10) NULL,
	[base] [nvarchar](50) NULL,
	[Id] [nvarchar](150) NULL,
	[weight] [numeric](3, 0) NULL,
	[signature] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKGROUPCONFLICT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKGROUPCONFLICT](
	[BenchmarkGroupID] [int] NOT NULL,
	[BenchmarkGroupConfID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKGROUPDCSTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKGROUPDCSTATUS](
	[BenchmarkGroupID] [int] NOT NULL,
	[DCStatusID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKGROUPDESCRIPTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKGROUPDESCRIPTION](
	[BenchmarkGroupDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkGroupID] [int] NOT NULL,
	[BenchmarkDescription] [nvarchar](max) NOT NULL,
	[lang] [nvarchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKGROUPFORBENCHMARK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKGROUPFORBENCHMARK](
	[BenchmarkGroupID] [int] NOT NULL,
	[BenchmarkID] [int] NOT NULL,
	[selected] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKGROUPMETADATA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKGROUPMETADATA](
	[BenchmarkGroupID] [int] NOT NULL,
	[MetadataID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKGROUPQUESTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKGROUPQUESTION](
	[BenchmarkGroupdID] [int] NOT NULL,
	[QuestionID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKGROUPREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKGROUPREFERENCE](
	[BenchmarkGroupID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKGROUPREQUIRE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKGROUPREQUIRE](
	[BenchmarkGroupID] [int] NOT NULL,
	[BenchmarkGroupReqID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKGROUPSTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKGROUPSTATUS](
	[BenchmarkGroupID] [int] NOT NULL,
	[StatusID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKGROUPTITLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKGROUPTITLE](
	[BenchmarkGroupTitleID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkGroupID] [int] NOT NULL,
	[BenchmarkTitle] [nvarchar](500) NOT NULL,
	[lang] [nvarchar](10) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKGROUPWARNING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKGROUPWARNING](
	[BenchmarkGroupID] [int] NOT NULL,
	[WarningID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKMESSAGE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKMESSAGE](
	[BenchmarkMessageID] [int] IDENTITY(1,1) NOT NULL,
	[DiagnosticMessage] [nvarchar](max) NOT NULL,
	[severity] [nvarchar](50) NULL,
	[lang] [nvarchar](10) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKMESSAGEFORBENCHMARKRULERESULT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKMESSAGEFORBENCHMARKRULERESULT](
	[BenchmarkRuleResultID] [int] NOT NULL,
	[BenchmarkMessageID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKMETADATA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKMETADATA](
	[BenchmarkID] [int] NOT NULL,
	[MetadataID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKMODEL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKMODEL](
	[BenchmarkModelID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkID] [int] NOT NULL,
	[BenchmarkModel] [nvarchar](500) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKNOTICE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKNOTICE](
	[BenchmarkNoticeID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkID] [int] NOT NULL,
	[BenchmarkNotice] [nvarchar](max) NOT NULL,
	[lang] [nvarchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKOVERRIDE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKOVERRIDE](
	[BenchmarkOverrideID] [int] IDENTITY(1,1) NOT NULL,
	[oldresult] [nvarchar](50) NULL,
	[newresult] [nvarchar](50) NULL,
	[remark] [nvarchar](max) NULL,
	[lang] [nvarchar](10) NULL,
	[time] [datetimeoffset](7) NULL,
	[authority] [nvarchar](150) NULL,
	[PersonID] [int] NULL,
	[OrganisationID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKOVERRIDEFORBENCHMARKRULERESULT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKOVERRIDEFORBENCHMARKRULERESULT](
	[BenchmarkRuleResultID] [int] NOT NULL,
	[BenchmarkOverrideID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPLAINTEXT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPLAINTEXT](
	[BenchmarkPlaintextID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkID] [int] NOT NULL,
	[PlainTextID] [nvarchar](100) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPLATFORM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPLATFORM](
	[BenchmarkPlatformID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkPlatformText] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPLATFORMFORBENCHMARK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPLATFORMFORBENCHMARK](
	[BenchmarkID] [int] NOT NULL,
	[BenchmarkPlatformID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPLATFORMFORBENCHMARKGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPLATFORMFORBENCHMARKGROUP](
	[BenchmarkGroupID] [int] NOT NULL,
	[BenchmarkPlatformID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPLATFORMFORBENCHMARKPROFILE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPLATFORMFORBENCHMARKPROFILE](
	[BenchmarkProfileID] [int] NOT NULL,
	[BenchmarkPlatformID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPLATFORMFORBENCHMARKRULE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPLATFORMFORBENCHMARKRULE](
	[BenchmarkRuleID] [int] NOT NULL,
	[BenchmarkPlatformID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPLATFORMFORBENCHMARKTESTRESULT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPLATFORMFORBENCHMARKTESTRESULT](
	[BenchmarkTestResultID] [int] NOT NULL,
	[BenchmarkPlatformID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILE](
	[BenchmarkProfileID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkProfileXCCDFID] [nvarchar](250) NULL,
	[BenchmarkProfileVersion] [nvarchar](50) NULL,
	[signature] [nvarchar](max) NULL,
	[prohibitChanges] [bit] NULL,
	[abstract] [bit] NULL,
	[notetag] [nvarchar](250) NULL,
	[extends] [nvarchar](250) NULL,
	[base] [nvarchar](max) NULL,
	[Id] [nvarchar](150) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILEDCSTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILEDCSTATUS](
	[BenchmarkProfileID] [int] NOT NULL,
	[DCStatusID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILEDESCRIPTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILEDESCRIPTION](
	[BenchmarkProfileDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkProfileID] [int] NOT NULL,
	[BenchmarkProfileDescription] [nvarchar](max) NOT NULL,
	[lang] [nvarchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILEFORBENCHMARKTAILORING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILEFORBENCHMARKTAILORING](
	[BenchmarkTailoringID] [int] NOT NULL,
	[BenchmarkProfileID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILEMETADATA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILEMETADATA](
	[BenchmarkProfileID] [int] NOT NULL,
	[MetadataID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILEREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILEREFERENCE](
	[BenchmarkProfileID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILEREFINERULE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILEREFINERULE](
	[BenchmarkProfileRefineRuleID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkProfileID] [int] NOT NULL,
	[BenchmarkGroupID] [int] NULL,
	[BenchmarkRuleID] [int] NULL,
	[clusterid] [nvarchar](500) NULL,
	[weight] [numeric](3, 0) NULL,
	[severity] [nvarchar](20) NULL,
	[role] [nvarchar](20) NULL,
	[ValueSelectorID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILEREFINERULEREMARK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILEREFINERULEREMARK](
	[BenchmarkProfileRefineRuleID] [int] NOT NULL,
	[BenchmarkRemarkID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILEREFINEVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILEREFINEVALUE](
	[BenchmarkProfileRefineValueID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkProfileID] [int] NOT NULL,
	[BenchmarkValueID] [int] NULL,
	[clusterid] [nvarchar](500) NULL,
	[ValueSelectorID] [nvarchar](500) NULL,
	[BenchmarkValueValueID] [int] NULL,
	[BenchmarkValueLowerBoundID] [int] NULL,
	[BenchmarkValueUpperBoundID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILEREFINEVALUEREMARK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILEREFINEVALUEREMARK](
	[BenchmarkProfileRefineValueID] [int] NOT NULL,
	[BenchmarkRemarkID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILESELECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILESELECT](
	[BenchmarkProfileSelectID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkProfileID] [int] NOT NULL,
	[BenchmarkGroupID] [int] NULL,
	[BenchmarkRuleID] [int] NULL,
	[selected] [bit] NULL,
	[clusterid] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILESELECTREMARK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILESELECTREMARK](
	[BenchmarkProfileSelectID] [int] NOT NULL,
	[BenchmarkRemarkID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILESETCOMPLEXVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILESETCOMPLEXVALUE](
	[BenchmarkProfileSetComplexValueID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkProfileID] [int] NOT NULL,
	[BenchmarkValueID] [int] NOT NULL,
	[BenchmarkSetComplexValue] [nvarchar](max) NOT NULL,
	[clusterid] [nvarchar](500) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILESETCOMPLEXVALUEREMARK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILESETCOMPLEXVALUEREMARK](
	[BenchmarkProfileSetComplexValueID] [int] NOT NULL,
	[BenchmarkRemarkID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILESETVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILESETVALUE](
	[BenchmarkProfileSetValueID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkProfileID] [int] NOT NULL,
	[BenchmarkValueID] [int] NOT NULL,
	[BenchmarkValueSetValue] [nvarchar](500) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILESETVALUEREMARK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILESETVALUEREMARK](
	[BenchmarkProfileSetValueID] [int] NOT NULL,
	[BenchmarkRemarkID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILESTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILESTATUS](
	[BenchmarkProfileID] [int] NOT NULL,
	[StatusID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKPROFILETITLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKPROFILETITLE](
	[BenchmarkProfileTitleID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkProfileID] [int] NOT NULL,
	[BenchmarkProfileTitle] [nvarchar](500) NOT NULL,
	[lang] [nvarchar](10) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKREAR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKREAR](
	[BenchmarkRearID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkID] [int] NOT NULL,
	[BenchmarkRearMatter] [nvarchar](max) NOT NULL,
	[lang] [nvarchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKREFERENCE](
	[BenchmarkID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKREMARK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKREMARK](
	[BenchmarkRemarkID] [int] IDENTITY(1,1) NOT NULL,
	[Remark] [nvarchar](max) NOT NULL,
	[lang] [nvarchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRESULTINSTANCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRESULTINSTANCE](
	[BenchmarkResultInstanceID] [int] NOT NULL,
	[BenchmarkRuleResultID] [int] NOT NULL,
	[instance] [nvarchar](500) NOT NULL,
	[context] [nvarchar](max) NULL,
	[parentContext] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULE](
	[BenchmarkRuleID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkRuleXCCDFID] [nvarchar](250) NULL,
	[weight] [numeric](3, 0) NULL,
	[BenchmarkRuleVersion] [nvarchar](50) NULL,
	[abstract] [bit] NULL,
	[clusterid] [nvarchar](500) NULL,
	[extends] [nvarchar](500) NULL,
	[hidden] [bit] NULL,
	[prohibitChanges] [bit] NULL,
	[lang] [nvarchar](10) NULL,
	[base] [nvarchar](50) NULL,
	[Id] [nvarchar](150) NULL,
	[signature] [nvarchar](max) NULL,
	[role] [nvarchar](20) NULL,
	[severity] [nvarchar](20) NULL,
	[multiple] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULECONFLICT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULECONFLICT](
	[BenchmarkRuleID] [int] NOT NULL,
	[BenchmarkRuleConfID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULEDCSTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULEDCSTATUS](
	[BenchmarkRuleID] [int] NOT NULL,
	[DCStatusID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULEDESCRIPTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULEDESCRIPTION](
	[BenchmarkRuleID] [int] NOT NULL,
	[BenchmarkDescription] [nvarchar](max) NOT NULL,
	[lang] [nvarchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULEFIXACTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULEFIXACTION](
	[BenchmarkRuleID] [int] NOT NULL,
	[FixActionID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULEFIXTEXT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULEFIXTEXT](
	[BenchmarkRuleID] [int] NOT NULL,
	[BenchmarkFixTextID] [int] NOT NULL,
	[lang] [nvarchar](10) NULL,
	[override] [bit] NULL,
	[FixActionID] [int] NULL,
	[reboot] [bit] NULL,
	[strategy] [nvarchar](50) NULL,
	[disruption] [nvarchar](50) NULL,
	[complexity] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULEFORBENCHMARK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULEFORBENCHMARK](
	[BenchmarkRuleID] [int] NOT NULL,
	[BenchmarkID] [int] NOT NULL,
	[selected] [bit] NULL,
	[ValueSelectorID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULEFORBENCHMARKGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULEFORBENCHMARKGROUP](
	[BenchmarkGroupID] [int] NOT NULL,
	[BenchmarkRuleID] [int] NOT NULL,
	[ValueSelectorID] [nvarchar](500) NULL,
	[selected] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULEIDENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULEIDENT](
	[BenchmarkRuleIdentID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkRuleIdentText] [nvarchar](500) NOT NULL,
	[IdentificationSystemID] [int] NULL,
	[cce_id] [nvarchar](20) NULL,
	[CPEID] [nvarchar](255) NULL,
	[VulnerabilityID] [int] NULL,
	[ReferenceID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULEIDENTFORBENCHMARKRULE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULEIDENTFORBENCHMARKRULE](
	[BenchmarkRuleID] [int] NOT NULL,
	[BenchmarkRuleIdentID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULEIDENTFORBENCHMARKRULERESULT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULEIDENTFORBENCHMARKRULERESULT](
	[BenchmarkRuleResultID] [int] NOT NULL,
	[BenchmarkRuleIdentID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULEMETADATA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULEMETADATA](
	[BenchmarkRuleID] [int] NOT NULL,
	[MetadataID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULEPROFILENOTE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULEPROFILENOTE](
	[BenchmarkRuleProfileNoteID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkRuleID] [int] NOT NULL,
	[BenchmarkRuleProfileNoteText] [nvarchar](max) NOT NULL,
	[lang] [nvarchar](10) NULL,
	[BenchmarkProfileID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULEQUESTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULEQUESTION](
	[BenchmarkRuleID] [int] NOT NULL,
	[QuestionID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULEREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULEREFERENCE](
	[BenchmarkRuleID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULEREQUIRE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULEREQUIRE](
	[BenchmarkRuleID] [int] NOT NULL,
	[BenchmarkRuleReqID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULERESULT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULERESULT](
	[BenchmarkRuleResultID] [int] IDENTITY(1,1) NOT NULL,
	[result] [nvarchar](50) NOT NULL,
	[BenchmarkRuleID] [int] NOT NULL,
	[role] [nvarchar](20) NULL,
	[severity] [nvarchar](20) NULL,
	[time] [datetimeoffset](7) NULL,
	[BenchmarkRuleVersion] [nvarchar](50) NULL,
	[weight] [numeric](3, 0) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULERESULTFORBENCHMARKTESTRESULT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULERESULTFORBENCHMARKTESTRESULT](
	[BenchmarkTestResultID] [int] NOT NULL,
	[BenchmarkRuleResultID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULERESULTMETADATA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULERESULTMETADATA](
	[BenchmarkRuleResultID] [int] NOT NULL,
	[MetadataID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULESTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULESTATUS](
	[BenchmarkRuleID] [int] NOT NULL,
	[StatusID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULETITLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULETITLE](
	[BenchmarkRuleID] [int] NOT NULL,
	[BenchmarkTitle] [nvarchar](500) NOT NULL,
	[lang] [nvarchar](10) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKRULEWARNING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKRULEWARNING](
	[BenchmarkRuleID] [int] NOT NULL,
	[WarningID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKSCORE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKSCORE](
	[BenchmarkScoreID] [int] IDENTITY(1,1) NOT NULL,
	[ScoringModelURI] [nvarchar](250) NULL,
	[MaximumScore] [numeric](18, 0) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKSCOREFORBENCHMARKTESTRESULT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKSCOREFORBENCHMARKTESTRESULT](
	[BenchmarkTestResultID] [int] NOT NULL,
	[BenchmarkScoreID] [int] NOT NULL,
	[BenchmarkScoreValue] [numeric](18, 0) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKSTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKSTATUS](
	[BenchmarkID] [int] NOT NULL,
	[StatusID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKSUB]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKSUB](
	[BenchmarkSubID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKSUBFORBENCHMARKFIXTEXT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKSUBFORBENCHMARKFIXTEXT](
	[BenchmarkFixTextID] [int] NOT NULL,
	[BenchmarkSubID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKSUBFORFIXACTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKSUBFORFIXACTION](
	[FixActionID] [int] NOT NULL,
	[BenchmarkSubID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKSUBGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKSUBGROUP](
	[BenchmarkGroupID] [int] NOT NULL,
	[BenchmarkSubGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTAILORING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTAILORING](
	[BenchmarkTailoringID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkTailoringXCCDFID] [nvarchar](250) NULL,
	[BenchmarkID] [int] NULL,
	[BenchmarkTailoringVersion] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[signature] [nvarchar](max) NULL,
	[Id] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTAILORINGDCSTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTAILORINGDCSTATUS](
	[BenchmarkTailoringID] [int] NOT NULL,
	[DCStatusID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTAILORINGFILE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTAILORINGFILE](
	[BenchmarkTailoringFileID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkTailoringID] [int] NOT NULL,
	[hrefURI] [nvarchar](500) NOT NULL,
	[BenchmarkTailoringVersion] [nvarchar](50) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTAILORINGMETADATA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTAILORINGMETADATA](
	[BenchmarkTailoringID] [int] NOT NULL,
	[MetadataID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTAILORINGSTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTAILORINGSTATUS](
	[BenchmarkTailoringID] [int] NOT NULL,
	[StatusID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTESTRESULT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTESTRESULT](
	[BenchmarkTestResultID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkTestResultXCCDFID] [nvarchar](250) NULL,
	[BenchmarkID] [int] NULL,
	[BenchmarkTailoringFileID] [int] NULL,
	[BenchmarkProfileID] [int] NULL,
	[signature] [nvarchar](max) NULL,
	[starttime] [datetimeoffset](7) NULL,
	[endtime] [datetimeoffset](7) NOT NULL,
	[testsystem] [nvarchar](250) NULL,
	[BenchmarkVersion] [nvarchar](50) NULL,
	[Id] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTESTRESULTASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTESTRESULTASSET](
	[BenchmarkTestResultID] [int] NOT NULL,
	[AssetID] [int] NOT NULL,
	[name] [nvarchar](max) NULL,
	[ipaddressIPv4] [nvarchar](50) NULL,
	[ipaddressIPv6] [nvarchar](50) NULL,
	[MacAdress] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTESTRESULTASSETSYNTHETICID]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTESTRESULTASSETSYNTHETICID](
	[BenchmarkTestResultID] [int] NOT NULL,
	[AssetSyntheticID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTESTRESULTCREDENTIAL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTESTRESULTCREDENTIAL](
	[BenchmarkTestResultID] [int] NOT NULL,
	[AssetCredentialID] [int] NOT NULL,
	[authenticated] [bit] NULL,
	[privileged] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTESTRESULTFORBENCHMARKCHECKCONTENTREF]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTESTRESULTFORBENCHMARKCHECKCONTENTREF](
	[BenchmarkTestResultID] [int] NOT NULL,
	[BenchmarkCheckContentRefID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTESTRESULTMETADATA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTESTRESULTMETADATA](
	[BenchmarkTestResultID] [int] NOT NULL,
	[MetadataID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTESTRESULTORGANISATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTESTRESULTORGANISATION](
	[BenchmarkTestResultID] [int] NOT NULL,
	[OrganisationID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTESTRESULTREMARK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTESTRESULTREMARK](
	[BenchmarkTestResultID] [int] NOT NULL,
	[BenchmarkRemarkID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTESTRESULTSETCOMPLEXVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTESTRESULTSETCOMPLEXVALUE](
	[BenchmarkTestResultSetComplexValueID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkTestResultID] [int] NOT NULL,
	[BenchmarkValueID] [int] NOT NULL,
	[BenchmarkSetComplexValue] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTESTRESULTSETVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTESTRESULTSETVALUE](
	[BenchmarkTestResultSetValueID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkTestResultID] [int] NOT NULL,
	[BenchmarkValueID] [int] NOT NULL,
	[BenchmarkValueSetValue] [nvarchar](500) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTESTRESULTTARGETFACT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTESTRESULTTARGETFACT](
	[BenchmarkTestResultTargetFactID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkTestResultID] [int] NOT NULL,
	[AssetID] [int] NULL,
	[FactName] [nvarchar](500) NOT NULL,
	[type] [nvarchar](50) NULL,
	[FactValue] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTESTRESULTTITLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTESTRESULTTITLE](
	[BenchmarkTestResultID] [int] NOT NULL,
	[BenchmarkTestResultTitle] [nvarchar](500) NOT NULL,
	[lang] [nvarchar](10) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKTITLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKTITLE](
	[BenchmarkID] [int] NOT NULL,
	[BenchmarkTitle] [nvarchar](500) NOT NULL,
	[lang] [nvarchar](10) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUE](
	[BenchmarkValueID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkValueXCCDFID] [nvarchar](250) NULL,
	[weight] [numeric](3, 0) NULL,
	[BenchmarkValueVersion] [nvarchar](50) NULL,
	[abstract] [bit] NULL,
	[clusterid] [nvarchar](500) NULL,
	[extends] [nvarchar](500) NULL,
	[hidden] [bit] NULL,
	[prohibitChanges] [bit] NULL,
	[lang] [nvarchar](10) NULL,
	[base] [nvarchar](50) NULL,
	[Id] [nvarchar](150) NULL,
	[signature] [nvarchar](max) NULL,
	[type] [nvarchar](50) NULL,
	[operator] [nvarchar](50) NULL,
	[interactive] [bit] NULL,
	[interfaceHint] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUECHOICE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUECHOICE](
	[BenchmarkValueChoiceID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkValueChoicesID] [int] NOT NULL,
	[ChoiceValue] [nvarchar](max) NOT NULL,
	[ValueSelectorID] [nvarchar](500) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUECHOICES]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUECHOICES](
	[BenchmarkValueChoicesID] [int] IDENTITY(1,1) NOT NULL,
	[mustMatch] [bit] NULL,
	[selector] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUECHOICESFORBENCHMARKVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUECHOICESFORBENCHMARKVALUE](
	[BenchmarkValueChoicesID] [int] NOT NULL,
	[BenchmarkValueID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUECOMPLEXDEFAULT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUECOMPLEXDEFAULT](
	[BenchmarkValueID] [int] NOT NULL,
	[BenchmarkComplexDefault] [nvarchar](max) NOT NULL,
	[ValueSelectorID] [nvarchar](500) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUECOMPLEXVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUECOMPLEXVALUE](
	[BenchmarkValueID] [int] NOT NULL,
	[BenchmarkComplexValue] [nvarchar](max) NOT NULL,
	[ValueSelectorID] [nvarchar](500) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUEDCSTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUEDCSTATUS](
	[BenchmarkValueID] [int] NOT NULL,
	[DCStatusID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUEDEFAULT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUEDEFAULT](
	[BenchmarkValueID] [int] NOT NULL,
	[BenchmarkDefaultValue] [nvarchar](500) NOT NULL,
	[ValueSelectorID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUEDESCRIPTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUEDESCRIPTION](
	[BenchmarkValueID] [int] NOT NULL,
	[BenchmarkDescription] [nvarchar](max) NOT NULL,
	[lang] [nvarchar](10) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUEFORBENCHMARKGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUEFORBENCHMARKGROUP](
	[BenchmarkGroupID] [int] NOT NULL,
	[BenchmarkValueID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUELOWERBOUND]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUELOWERBOUND](
	[BenchmarkValueLowerBoundID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkValueID] [int] NOT NULL,
	[LowerBound] [decimal](18, 0) NOT NULL,
	[ValueSelectorID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUEMATCH]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUEMATCH](
	[BenchmarkValueMatchID] [int] IDENTITY(1,1) NOT NULL,
	[RegexName] [nvarchar](100) NULL,
	[Regex] [nvarchar](500) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUEMATCHFORBENCHMARKVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUEMATCHFORBENCHMARKVALUE](
	[BenchmarkValueID] [int] NOT NULL,
	[BenchmarkValueMatchID] [int] NOT NULL,
	[ValueSelectorID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUEMETADATA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUEMETADATA](
	[BenchmarkValueID] [int] NOT NULL,
	[MetadataID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUEQUESTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUEQUESTION](
	[BenchmarkValueID] [int] NOT NULL,
	[QuestionID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUEREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUEREFERENCE](
	[BenchmarkValueID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUESOURCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUESOURCE](
	[BenchmakValueID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUESTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUESTATUS](
	[BenchmarkValueID] [int] NOT NULL,
	[StatusID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUETITLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUETITLE](
	[BenchmarkValueID] [int] NOT NULL,
	[BenchmarkTitle] [nvarchar](500) NOT NULL,
	[lang] [nvarchar](10) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUEUPPERBOUND]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUEUPPERBOUND](
	[BenchmarkValueUpperBoundID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkValueID] [int] NOT NULL,
	[UpperBound] [decimal](18, 0) NOT NULL,
	[ValueSelectorID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUEVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUEVALUE](
	[BenchmarkValueValueID] [int] IDENTITY(1,1) NOT NULL,
	[BenchmarkValueID] [int] NOT NULL,
	[BenchmarkValueValue] [nvarchar](500) NOT NULL,
	[ValueSelectorID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BENCHMARKVALUEWARNING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENCHMARKVALUEWARNING](
	[BenchmarkValueID] [int] NOT NULL,
	[WarningID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BUSINESSIMPACT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUSINESSIMPACT](
	[BusinessImpactID] [int] IDENTITY(1,1) NOT NULL,
	[ImpactLevel] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BUSINESSIMPACTFORBUSINESSRISK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUSINESSIMPACTFORBUSINESSRISK](
	[BusinessImpactID] [int] NOT NULL,
	[BusinessRiskID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BUSINESSIMPACTFORREGULATORYRISK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUSINESSIMPACTFORREGULATORYRISK](
	[BusinessImpactID] [int] NOT NULL,
	[RegulatoryRiskID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BUSINESSPROCESS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUSINESSPROCESS](
	[BusinessProcessID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BUSINESSRISK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUSINESSRISK](
	[BusinessRiskID] [int] IDENTITY(1,1) NOT NULL,
	[RiskDescription] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CAPEC]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CAPEC](
	[capec_id] [nvarchar](20) NOT NULL,
	[CategoryName] [nvarchar](200) NULL,
	[DescriptionSummaryClean] [nvarchar](max) NULL,
	[DescriptionSummary] [nvarchar](max) NULL,
	[CapecStatus] [nvarchar](50) NULL,
	[TypicalSeverity] [nvarchar](50) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[AttackPatternID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CAPECEXPLOITLIKELIHOOD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CAPECEXPLOITLIKELIHOOD](
	[capec_id] [nvarchar](20) NOT NULL,
	[ExploitLikelihoodID] [int] NOT NULL,
	[Explanation] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CAPECREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CAPECREFERENCE](
	[CapecReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[Reference_ID] [nvarchar](50) NULL,
	[Local_Reference_ID] [nvarchar](50) NULL,
	[ReferenceTitle] [nvarchar](250) NULL,
	[ReferenceSection] [nvarchar](250) NULL,
	[ReferenceEdition] [nvarchar](100) NULL,
	[ReferencePublisher] [nvarchar](100) NULL,
	[ReferencePubDate] [nvarchar](50) NULL,
	[ReferenceLink] [nvarchar](250) NULL,
	[ReferenceDescription] [nvarchar](max) NULL,
	[ReferencePublication] [nvarchar](150) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CAPECREFERENCEAUTHOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CAPECREFERENCEAUTHOR](
	[CapecReferenceID] [int] NOT NULL,
	[AuthorID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CAPECREFERENCEFORCAPEC]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CAPECREFERENCEFORCAPEC](
	[CapecReferenceID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CAPECRELATIONSHIP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CAPECRELATIONSHIP](
	[capec_id] [nvarchar](20) NOT NULL,
	[RelationshipTargetForm] [nvarchar](50) NOT NULL,
	[RelationshipNature] [nvarchar](50) NOT NULL,
	[RelationshipTargetID] [nvarchar](20) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CAPECVIEW]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CAPECVIEW](
	[ViewID] [int] IDENTITY(1,1) NOT NULL,
	[CAPECViewID] [nvarchar](50) NOT NULL,
	[CAPECViewName] [nvarchar](500) NOT NULL,
	[CAPECViewStatus] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CATEGORY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CATEGORY](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](150) NOT NULL,
	[CategoryDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCE](
	[cce_id] [nvarchar](20) NOT NULL,
	[platform] [nvarchar](20) NULL,
	[modified] [datetimeoffset](7) NULL,
	[description] [nvarchar](max) NULL,
	[parameter] [nvarchar](250) NULL,
	[technical_mechanism] [nvarchar](250) NULL,
	[reference] [nvarchar](100) NULL,
	[resource_id] [nvarchar](4000) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCEFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCEFORASSET](
	[cce_id] [nvarchar](20) NOT NULL,
	[AssetID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCEFORCPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCEFORCPE](
	[cce_id] [nvarchar](20) NOT NULL,
	[CPEID] [nvarchar](255) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCEFORTHREATACTORTTP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCEFORTHREATACTORTTP](
	[cce_id] [nvarchar](20) NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCEPARAMETER]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCEPARAMETER](
	[CCEParameterID] [int] IDENTITY(1,1) NOT NULL,
	[CCEParameterText] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCEPARAMETERFORCCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCEPARAMETERFORCCE](
	[CCEParameterID] [int] NOT NULL,
	[cce_id] [nvarchar](20) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCEREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCEREFERENCE](
	[CCEReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[resource_id] [nvarchar](4000) NOT NULL,
	[ReferenceText] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCEREFERENCEFORCCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCEREFERENCEFORCCE](
	[CCEReferenceID] [int] NOT NULL,
	[cce_id] [nvarchar](20) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCERESOURCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCERESOURCE](
	[CCEResourceID] [int] IDENTITY(1,1) NOT NULL,
	[resource_id] [nvarchar](4000) NOT NULL,
	[modified] [nchar](10) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ResourceTitle] [nvarchar](max) NULL,
	[ResourcePublisher] [nvarchar](150) NULL,
	[issued] [nchar](10) NULL,
	[ResourceVersion] [nvarchar](50) NULL,
	[ResourceFormat] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCERESOURCEAUTHOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCERESOURCEAUTHOR](
	[CCEResourceID] [int] NOT NULL,
	[AuthorID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCERESOURCEFORCCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCERESOURCEFORCCE](
	[CCEResourceID] [int] NOT NULL,
	[cce_id] [nvarchar](20) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCERESOURCEFORCCEREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCERESOURCEFORCCEREFERENCE](
	[CCEResourceID] [int] NOT NULL,
	[CCEReferenceID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCETECHNICALMECHANISM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCETECHNICALMECHANISM](
	[CCETechnicalMechanismID] [int] IDENTITY(1,1) NOT NULL,
	[TechnicalMechanismText] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCETECHNICALMECHANISMFORCCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCETECHNICALMECHANISMFORCCE](
	[CCETechnicalMechanismID] [int] NOT NULL,
	[cce_id] [nvarchar](20) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CERTIFICATE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CERTIFICATE](
	[CertificateID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CERTIFICATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CERTIFICATION](
	[CertificationID] [int] IDENTITY(1,1) NOT NULL,
	[CertificationAcronym] [nvarchar](50) NULL,
	[CertificationName] [nvarchar](500) NOT NULL,
	[lang] [nvarchar](10) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHARACTERENCODING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHARACTERENCODING](
	[CharacterEncodingID] [int] IDENTITY(1,1) NOT NULL,
	[CharacterEncodingName] [nvarchar](50) NOT NULL,
	[CharacterEncodingDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHECKENUMERATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHECKENUMERATION](
	[CheckEnumerationID] [int] IDENTITY(1,1) NOT NULL,
	[EnumerationValue] [nvarchar](50) NOT NULL,
	[EnumerationDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHECKLIST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHECKLIST](
	[ChecklistID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[AnswerSchemes] [nvarchar](50) NULL,
	[ChecklistCategID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHECKLISTANSWER]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHECKLISTANSWER](
	[AnswerID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionID] [int] NULL,
	[Answer] [nvarchar](max) NULL,
	[AnswerComments] [nvarchar](max) NULL,
	[AttachmentData] [image] NULL,
	[AttachmentMimeType] [nvarchar](50) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHECKLISTCATEG]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHECKLISTCATEG](
	[ChecklistCategID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nchar](10) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHECKLISTCHAPTER]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHECKLISTCHAPTER](
	[ChapterID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[ChecklistID] [int] NULL,
	[ParentChapterID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHECKLISTQUESTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHECKLISTQUESTION](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Target] [nvarchar](max) NULL,
	[ChapterID] [int] NULL,
	[Tags] [nvarchar](max) NULL,
	[lang] [nvarchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CIAIMPACTFORCAPEC]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CIAIMPACTFORCAPEC](
	[CIAImpactID] [int] IDENTITY(1,1) NOT NULL,
	[capec_id] [nvarchar](20) NOT NULL,
	[Confidentiality_Impact] [nvarchar](50) NULL,
	[Integrity_Impact] [nvarchar](50) NULL,
	[Availability_Impact] [nvarchar](50) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COASTAGE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COASTAGE](
	[COAStageID] [int] IDENTITY(1,1) NOT NULL,
	[COAStageName] [nvarchar](100) NOT NULL,
	[COAStageDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CODE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CODE](
	[CodeID] [int] IDENTITY(1,1) NOT NULL,
	[ScriptID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COLLECTIONMETHOD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COLLECTIONMETHOD](
	[CollectionMethodID] [int] IDENTITY(1,1) NOT NULL,
	[CollectionMethodName] [nvarchar](150) NULL,
	[MeasureSourceID] [int] NULL,
	[CollectionMethodDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMMAND]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMMAND](
	[CommandID] [int] IDENTITY(1,1) NOT NULL,
	[CommandName] [nvarchar](50) NOT NULL,
	[CommandDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMMANDS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMMANDS](
	[CommandsID] [int] IDENTITY(1,1) NOT NULL,
	[ScriptName] [nvarchar](250) NOT NULL,
	[CommandsDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMPLIANCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMPLIANCE](
	[ComplianceID] [int] IDENTITY(1,1) NOT NULL,
	[ComplianceName] [nvarchar](50) NULL,
	[ComplianceVersion] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ComplianceDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMPLIANCECATEGORY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMPLIANCECATEGORY](
	[ComplianceCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ComplianceCategoryName] [nvarchar](50) NULL,
	[ComplianceCategoryDescription] [nvarchar](max) NULL,
	[ComplianceID] [int] NULL,
	[ParentCategoryID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMPLIANCEREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMPLIANCEREFERENCE](
	[ComplianceReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[ComplianceID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[ConfidenceLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ComplianceReferenceDescription] [nvarchar](max) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMPRESSION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMPRESSION](
	[CompressionID] [int] IDENTITY(1,1) NOT NULL,
	[compression_mechanism] [nvarchar](50) NULL,
	[compression_mechanism_ref] [nvarchar](250) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONCATFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONCATFUNCTION](
	[ConcatFunctionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONDITION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONDITION](
	[ConditionID] [int] IDENTITY(1,1) NOT NULL,
	[ConditionName] [nvarchar](50) NOT NULL,
	[ConditionDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONDITIONAPPLICATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONDITIONAPPLICATION](
	[ConditionApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[ConditionApplicationName] [nvarchar](50) NOT NULL,
	[ConditionApplicationDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONFIDENCELEVEL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONFIDENCELEVEL](
	[ConfidenceLevelID] [int] IDENTITY(1,1) NOT NULL,
	[ConfidenceLevelName] [nvarchar](100) NOT NULL,
	[ConfidenceLevelDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONNECTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONNECTION](
	[ConnectionID] [int] IDENTITY(1,1) NOT NULL,
	[ipaddressIPv4] [nvarchar](50) NULL,
	[ipaddressIPv6] [nvarchar](50) NULL,
	[macaddress] [nvarchar](50) NULL,
	[subnetmaskIPv4] [nvarchar](50) NULL,
	[subnetmaskIPv6] [nvarchar](50) NULL,
	[defaultrouteIPv4] [nvarchar](50) NULL,
	[defaultrouteIPv6] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONNECTIONFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONNECTIONFORASSET](
	[ConnectionID] [int] NOT NULL,
	[AssetID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONTACT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONTACT](
	[ContactID] [int] IDENTITY(1,1) NOT NULL,
	[ContactTypeID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONTACTTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONTACTTYPE](
	[ContactTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ContactTypeName] [nvarchar](50) NOT NULL,
	[ContactTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONTENTENUMERATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONTENTENUMERATION](
	[ContentEnumerationID] [int] IDENTITY(1,1) NOT NULL,
	[ContentEnumerationValue] [nvarchar](50) NOT NULL,
	[ContentEnumerationDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONTROLSTRENGTH]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONTROLSTRENGTH](
	[ControlStrengthID] [int] IDENTITY(1,1) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COOKIE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COOKIE](
	[CookieID] [int] IDENTITY(1,1) NOT NULL,
	[CookieName] [nvarchar](250) NULL,
	[CookieValue] [nvarchar](max) NULL,
	[CookieDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COUNTFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COUNTFUNCTION](
	[CountFunctionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COUNTRY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COUNTRY](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[CountryCode] [nchar](2) NOT NULL,
	[CountryName] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPE](
	[CPEID] [nvarchar](255) NOT NULL,
	[Title] [nvarchar](255) NULL,
	[NVDID] [int] NULL,
	[ModificationDate] [datetime] NULL,
	[Status] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEBLACKLIST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEBLACKLIST](
	[CPEBlacklistID] [int] IDENTITY(1,1) NOT NULL,
	[CPEID] [nvarchar](255) NOT NULL,
	[OrganisationID] [int] NULL,
	[PersonID] [int] NULL,
	[AssetID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEFORAPPLICATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEFORAPPLICATION](
	[ApplicationID] [int] NOT NULL,
	[CPEID] [nvarchar](255) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEFORASSET](
	[AssetID] [int] NOT NULL,
	[CPEID] [nvarchar](255) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEFORBENCHMARKPLATFORM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEFORBENCHMARKPLATFORM](
	[BenchmarkPlatformID] [int] NOT NULL,
	[CPEID] [nvarchar](255) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEFORFIXACTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEFORFIXACTION](
	[CPEID] [nvarchar](255) NOT NULL,
	[FixActionID] [int] NOT NULL,
	[relationshiptype] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEFORORGANISATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEFORORGANISATION](
	[OrganisationID] [int] NOT NULL,
	[CPEID] [nvarchar](255) NOT NULL,
	[Usage] [nvarchar](100) NULL,
	[Description] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEFORPLATFORM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEFORPLATFORM](
	[PlatformID] [int] NOT NULL,
	[CPEID] [nvarchar](255) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEPORT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEPORT](
	[CPEID] [nvarchar](255) NOT NULL,
	[PortID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEWHITELIST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEWHITELIST](
	[CPEWhitelistID] [int] IDENTITY(1,1) NOT NULL,
	[CPEID] [nvarchar](255) NOT NULL,
	[OrganisationID] [int] NULL,
	[PersonID] [int] NULL,
	[AssetID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CREDENTIAL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CREDENTIAL](
	[CredentialID] [int] IDENTITY(1,1) NOT NULL,
	[AuthenticationTypeID] [int] NULL,
	[Username] [nvarchar](255) NULL,
	[Password] [nvarchar](255) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CUSTOMOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CUSTOMOBJECT](
	[CustomObjectID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWE](
	[CWEID] [nvarchar](50) NOT NULL,
	[CWEName] [nvarchar](max) NULL,
	[CWEStatus] [nvarchar](50) NULL,
	[CWEAbstraction] [nvarchar](50) NULL,
	[CWEDescriptionSummaryClean] [nvarchar](max) NULL,
	[CWEDescriptionSummary] [nvarchar](max) NULL,
	[CWEExtendedDescription] [nvarchar](max) NULL,
	[CWEExtendedDescriptionClean] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[CWECausalNature] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CWEURL] [nvarchar](500) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEAFFECTEDRESOURCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEAFFECTEDRESOURCE](
	[CWEAffectedResourceID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[AffectedResourceID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEALTERNATETERM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEALTERNATETERM](
	[CWEAlternateTermID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[AlternateTerm] [nvarchar](max) NOT NULL,
	[AlternateTermClean] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEDEMONSTRATIVEEXAMPLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEDEMONSTRATIVEEXAMPLE](
	[CWEDemonstrativeExampleID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[DemonstrativeExampleID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEDETECTIONMETHOD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEDETECTIONMETHOD](
	[CWEDetectionMethodID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[DetectionMethodID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEFORCAPEC]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEFORCAPEC](
	[CWEID] [nvarchar](50) NOT NULL,
	[capec_id] [nvarchar](20) NOT NULL,
	[WeaknessRelationship] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEFOROWASPTOP10]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEFOROWASPTOP10](
	[CWEID] [nvarchar](50) NOT NULL,
	[OWASPTOP10ID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWELANGUAGE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWELANGUAGE](
	[CWELanguageID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[LanguageID] [int] NOT NULL,
	[Prevalence] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWELANGUAGECLASS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWELANGUAGECLASS](
	[CWELanguageClassID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[LanguageClassID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEREFERENCE](
	[CWEReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[ReferenceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWERELATIONSHIPCATEGORY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWERELATIONSHIPCATEGORY](
	[CWERelationshipCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[RelationshipNature] [nvarchar](50) NOT NULL,
	[RelationshipTargetCWEID] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWETAXONOMYNODE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWETAXONOMYNODE](
	[CWETaxonomyNodeID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[TaxonomyNodeID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWETHEORETICALNOTE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWETHEORETICALNOTE](
	[CWETheoreticalNoteID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[TheoreticalNoteID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWETIMEOFINTRODUCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWETIMEOFINTRODUCTION](
	[CWETimeOfIntroductionID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[IntroductoryPhase] [nvarchar](100) NOT NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWETOP25]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWETOP25](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[YearTop25] [int] NOT NULL,
	[Rank] [int] NOT NULL,
	[Score] [float] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBJECT](
	[CyberObjectID] [int] IDENTITY(1,1) NOT NULL,
	[CyberObjectGUID] [nvarchar](500) NULL,
	[CyberObjectIDREF] [nvarchar](500) NULL,
	[has_changed] [bit] NULL,
	[ObjectStateID] [int] NULL,
	[CurrentState] [nvarchar](150) NULL,
	[CyberObjectDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBJECTCOLLECTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBJECTCOLLECTION](
	[CyberObjectCollectionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBJECTCYBEROBJECTPROPERTIES]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBJECTCYBEROBJECTPROPERTIES](
	[CyberObjectID] [int] NOT NULL,
	[CyberObjectPropertiesID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBJECTDOMAINSPECIFICPROPERTY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBJECTDOMAINSPECIFICPROPERTY](
	[CyberObjectID] [int] NOT NULL,
	[abstract] [int] NULL,
	[MetadataID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBJECTEFFECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBJECTEFFECT](
	[CyberObjectID] [int] NOT NULL,
	[EffectTypeID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBJECTFORCYBEROBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBJECTFORCYBEROBJECT](
	[CyberObjectRefID] [int] NOT NULL,
	[CyberObjectSubjectID] [int] NOT NULL,
	[ObjectRelationshipID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBJECTFORCYBEROBSERVABLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBJECTFORCYBEROBSERVABLE](
	[CyberObservableID] [int] NOT NULL,
	[CyberObjectID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBJECTPOOL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBJECTPOOL](
	[CyberObjectPoolID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBJECTPOOLCYBEROBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBJECTPOOLCYBEROBJECT](
	[CyberObjectPoolID] [int] NOT NULL,
	[CyberObjectID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBJECTPROPERTIES]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBJECTPROPERTIES](
	[CyberObjectPropertiesID] [int] IDENTITY(1,1) NOT NULL,
	[CyberObjectTypeID] [int] NULL,
	[xsitype] [nvarchar](500) NULL,
	[type] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[category] [nvarchar](150) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBJECTPROPERTIESFORCYBEROBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBJECTPROPERTIESFORCYBEROBJECT](
	[CyberObjectID] [int] NOT NULL,
	[CyberObjectPropertiesID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBJECTPROPERTY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBJECTPROPERTY](
	[CyberObjectPropertyID] [int] IDENTITY(1,1) NOT NULL,
	[CyberObjectPropertyGUID] [nvarchar](500) NULL,
	[CyberObjectPropertyIDREF] [nvarchar](500) NULL,
	[CyberObjectPropertyName] [nvarchar](150) NULL,
	[CyberObjectPropertyDescription] [nvarchar](max) NULL,
	[appears_random] [bit] NULL,
	[ConditionApplicationID] [int] NULL,
	[apply_condition] [nvarchar](50) NULL,
	[bit_mask] [nvarchar](50) NULL,
	[ConditionID] [int] NULL,
	[condition] [nvarchar](50) NULL,
	[DataTypeID] [int] NULL,
	[datatype] [nvarchar](50) NULL,
	[defanging_algorithm_ref] [nvarchar](250) NULL,
	[has_changed] [bit] NULL,
	[is_defanged] [bit] NULL,
	[is_obfuscated] [bit] NULL,
	[obfuscation_algorithm_ref] [nvarchar](250) NULL,
	[PatternTypeID] [int] NULL,
	[PatternTypeName] [nvarchar](50) NULL,
	[refanging_transform] [nvarchar](50) NULL,
	[refanging_transform_type] [nvarchar](50) NULL,
	[regex_syntax] [nvarchar](50) NULL,
	[trend] [bit] NULL,
	[CategoryID] [int] NULL,
	[category] [nvarchar](150) NULL,
	[CyberObjectPropertyValue] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBJECTPROPERTYFORCYBEROBJECTPROPERTIES]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBJECTPROPERTYFORCYBEROBJECTPROPERTIES](
	[CyberObjectPropertiesID] [int] NOT NULL,
	[CyberObjectPropertyID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBJECTPROPERTYFORCYBEROBJECTPROPERTY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBJECTPROPERTYFORCYBEROBJECTPROPERTY](
	[CyberObjectPropertyRefID] [int] NOT NULL,
	[CyberObjectPropertySubjectID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBJECTPROPERTYGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBJECTPROPERTYGROUP](
	[CyberObjectPropertyGroupID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBJECTPROPERTYPATTERNFIELDGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBJECTPROPERTYPATTERNFIELDGROUP](
	[CyberObjectPropertyID] [int] NOT NULL,
	[PatternFieldGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBJECTTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBJECTTYPE](
	[CyberObjectTypeID] [int] IDENTITY(1,1) NOT NULL,
	[CyberObjectTypeName] [nvarchar](50) NOT NULL,
	[CyberObjectTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBSERVABLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBSERVABLE](
	[CyberObservableID] [int] IDENTITY(1,1) NOT NULL,
	[CyberObservableGUID] [nvarchar](500) NULL,
	[CyberObservableIDREF] [nvarchar](500) NULL,
	[negate] [bit] NULL,
	[CyberObservableTitle] [nvarchar](500) NULL,
	[CyberObservableDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBSERVABLEFORCYBEROBSERVABLES]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBSERVABLEFORCYBEROBSERVABLES](
	[CyberObservablesID] [int] NOT NULL,
	[CyberObservableID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBSERVABLEFORINCIDENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBSERVABLEFORINCIDENT](
	[CyberObservableID] [int] NOT NULL,
	[IncidentID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBSERVABLEFORINDICATOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBSERVABLEFORINDICATOR](
	[CyberObservableID] [int] NOT NULL,
	[IndicatorID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBSERVABLEFORTHREATINFRASTUCTURE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBSERVABLEFORTHREATINFRASTUCTURE](
	[CyberObservableID] [int] NOT NULL,
	[ThreatActorInfrastructureID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBSERVABLEKEYWORD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBSERVABLEKEYWORD](
	[CyberObservableID] [int] NOT NULL,
	[KeywordID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBSERVABLES]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBSERVABLES](
	[CyberObservablesID] [int] IDENTITY(1,1) NOT NULL,
	[cybox_major_version] [nvarchar](5) NULL,
	[cybox_minor_version] [nvarchar](5) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CYBEROBSERVABLETESTMECHANISM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CYBEROBSERVABLETESTMECHANISM](
	[CyberObservableTestMechanismID] [int] IDENTITY(1,1) NOT NULL,
	[Producer] [nvarchar](100) NULL,
	[CyberObservableTestMechanismGID] [nvarchar](100) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATAFORMAT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATAFORMAT](
	[DataFormatID] [int] IDENTITY(1,1) NOT NULL,
	[DataFormatName] [nvarchar](50) NULL,
	[DataFormatDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATASEGMENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATASEGMENT](
	[DataSegmentID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATASIZEUNIT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATASIZEUNIT](
	[DataSizeUnitID] [int] IDENTITY(1,1) NOT NULL,
	[DataSizeName] [nvarchar](50) NOT NULL,
	[DataSizeDescription] [nvarchar](max) NULL,
	[lang] [nvarchar](10) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATATYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATATYPE](
	[DataTypeID] [int] IDENTITY(1,1) NOT NULL,
	[DataTypeName] [nvarchar](50) NOT NULL,
	[DataTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATETIMEFORMAT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATETIMEFORMAT](
	[DateTimeFormatID] [int] IDENTITY(1,1) NOT NULL,
	[DateTimeFormatValue] [nvarchar](50) NOT NULL,
	[DataType] [nvarchar](50) NOT NULL,
	[DateTimeFormatDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DCSTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DCSTATUS](
	[DCStatusID] [int] NOT NULL,
	[DCStatusContent] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEBUGGINGACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DEBUGGINGACTIONNAME](
	[DebuggingActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[DebuggingActionNameName] [nvarchar](150) NOT NULL,
	[DebuggingActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEFENSETOOLTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DEFENSETOOLTYPE](
	[DefenseToolTypeID] [int] IDENTITY(1,1) NOT NULL,
	[DefenseToolTypeName] [nvarchar](100) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEMONSTRATIVEEXAMPLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DEMONSTRATIVEEXAMPLE](
	[DemonstrativeExampleID] [int] IDENTITY(1,1) NOT NULL,
	[DemonstrativeExampleGUID] [nvarchar](500) NULL,
	[DemonstrativeExampleVocabularyID] [nvarchar](50) NULL,
	[DemonstrativeExampleIntroText] [nvarchar](max) NULL,
	[DemonstrativeExampleBody] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[LanguageID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DETECTABILITY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DETECTABILITY](
	[DetectabilityID] [int] IDENTITY(1,1) NOT NULL,
	[DetectabilityName] [nvarchar](50) NOT NULL,
	[DetectabilityDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DETECTIONMETHOD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DETECTIONMETHOD](
	[DetectionMethodID] [int] IDENTITY(1,1) NOT NULL,
	[DetectionMethodVocabularyID] [nvarchar](50) NULL,
	[DetectionMethodName] [nvarchar](100) NOT NULL,
	[DetectionMethodDescription] [nvarchar](max) NULL,
	[DetectionMethodEffectiveness] [nvarchar](50) NULL,
	[DetectionMethodEffectivenessNotes] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEVICE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DEVICE](
	[DeviceID] [int] IDENTITY(1,1) NOT NULL,
	[Device_Type] [nvarchar](250) NOT NULL,
	[Manufacturer] [nvarchar](250) NULL,
	[Model] [nvarchar](250) NULL,
	[Serial_Number] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEVICEBLACKLIST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DEVICEBLACKLIST](
	[DeviceBlacklistID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceID] [int] NOT NULL,
	[OrganisationID] [int] NULL,
	[PersonID] [int] NULL,
	[AssetID] [int] NULL,
	[CreatedDate] [int] NULL,
	[ValidFromDate] [int] NULL,
	[ValidUntilDate] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEVICEDRIVERACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DEVICEDRIVERACTIONNAME](
	[DeviceDriverActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceDriverActionNameName] [nvarchar](150) NOT NULL,
	[DeviceDriverActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEVICEWHITELIST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DEVICEWHITELIST](
	[DeviceWhitelistID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceID] [int] NOT NULL,
	[OrganisationID] [int] NULL,
	[PersonID] [int] NULL,
	[AssetID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DIRECTORYACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DIRECTORYACTIONNAME](
	[DirectoryActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[DirectoryActionNameName] [nvarchar](150) NOT NULL,
	[DirectoryActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DISCOVERYMETHOD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DISCOVERYMETHOD](
	[DiscoveryMethodID] [int] IDENTITY(1,1) NOT NULL,
	[DiscoveryMethodName] [nvarchar](150) NULL,
	[MeasureSourceID] [int] NULL,
	[DiscoveryMethodDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DISK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DISK](
	[DiskID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DISKACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DISKACTIONNAME](
	[DiskActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[DiskActionNameName] [nvarchar](150) NULL,
	[DiskActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DISKPARTITION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DISKPARTITION](
	[DiskPartitionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DNSACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DNSACTIONNAME](
	[DNSActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[DNSActionNameName] [nvarchar](150) NOT NULL,
	[DNSActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DNSCACHE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DNSCACHE](
	[DNSCacheID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DNSQUERY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DNSQUERY](
	[DNSQueryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DNSRECORD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DNSRECORD](
	[DNSRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOWNTIME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOWNTIME](
	[DowntimeID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NULL,
	[DownFromDate] [datetimeoffset](7) NULL,
	[DownToDate] [datetimeoffset](7) NOT NULL,
	[DowntimeDuration] [int] NULL,
	[DowntimePlanned] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DPE](
	[DPEID] [int] IDENTITY(1,1) NOT NULL,
	[CPEID] [nvarchar](255) NULL,
	[CredentialID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[PortID] [int] NULL,
	[ProtocolID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EFFECTIVENESS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EFFECTIVENESS](
	[EffectivenessID] [int] IDENTITY(1,1) NOT NULL,
	[EffectivenessName] [nvarchar](100) NOT NULL,
	[EffectivenessDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EFFECTTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EFFECTTYPE](
	[EffectTypeID] [int] IDENTITY(1,1) NOT NULL,
	[EffectTypeName] [nvarchar](150) NOT NULL,
	[EffectTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAIL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAIL](
	[EmailID] [int] IDENTITY(1,1) NOT NULL,
	[emailaddress] [nvarchar](100) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILADDRESS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILADDRESS](
	[AddressID] [int] NOT NULL,
	[EmailID] [int] NOT NULL,
	[emailaddress] [nvarchar](100) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILFORORGANISATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILFORORGANISATION](
	[emailaddress] [nvarchar](100) NOT NULL,
	[OrganisationID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILFORPERSON]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILFORPERSON](
	[emailaddress] [nvarchar](100) NOT NULL,
	[PersonID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILMESSAGE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILMESSAGE](
	[EmailMessageID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENCODING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENCODING](
	[EncodingID] [int] IDENTITY(1,1) NOT NULL,
	[algorithm] [nvarchar](50) NOT NULL,
	[EncodingDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENCRYPTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENCRYPTION](
	[EncryptionID] [int] IDENTITY(1,1) NOT NULL,
	[encryption_mechanism] [nvarchar](50) NOT NULL,
	[encryption_mechanism_ref] [nvarchar](250) NULL,
	[EncryptionDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENDFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENDFUNCTION](
	[EndFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[EndsWithCharacters] [nvarchar](500) NOT NULL,
	[OVALComponentGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENDPOINT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENDPOINT](
	[EndPointID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NULL,
	[DeviceID] [int] NULL,
	[AddressID] [int] NULL,
	[ProtocolID] [int] NULL,
	[ProtocolName] [nvarchar](100) NULL,
	[PortID] [int] NULL,
	[PortNumber] [int] NULL,
	[Service] [nvarchar](50) NULL,
	[Version] [nvarchar](max) NULL,
	[CPEID] [nvarchar](255) NULL,
	[SessionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENGINE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENGINE](
	[EngineID] [int] IDENTITY(1,1) NOT NULL,
	[EngineName] [nvarchar](250) NULL,
	[EngineDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENVIRONMENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENVIRONMENT](
	[EnvironmentID] [int] IDENTITY(1,1) NOT NULL,
	[CapecEnvironmentID] [nvarchar](50) NULL,
	[EnvironmentTitle] [nvarchar](150) NOT NULL,
	[EnvironmentDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ESCAPEREGEXFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ESCAPEREGEXFUNCTION](
	[EscapeRegexFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[OVALComponentGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENT](
	[EventID] [int] IDENTITY(1,1) NOT NULL,
	[EventName] [nvarchar](50) NULL,
	[EventTypeID] [int] NULL,
	[start_datetime] [datetimeoffset](7) NULL,
	[stop_datetime] [datetimeoffset](7) NULL,
	[AnomalyEvent] [bit] NULL,
	[AnomalyDescription] [nvarchar](max) NULL,
	[AuditRecordEvent] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTCOLLECTIONMETHOD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTCOLLECTIONMETHOD](
	[EventID] [int] NOT NULL,
	[CollectionMethodID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[AssetID] [int] NULL,
	[DeviceID] [int] NULL,
	[CPEID] [nvarchar](255) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTCOMMENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTCOMMENT](
	[EventCommentID] [int] IDENTITY(1,1) NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTCOMMENTFOREVENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTCOMMENTFOREVENT](
	[EventID] [int] NOT NULL,
	[EventCommentID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTENDPOINT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTENDPOINT](
	[EventID] [int] NOT NULL,
	[EndPointID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTFORASSET](
	[EventID] [int] NOT NULL,
	[AssetID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTFOREVENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTFOREVENT](
	[EventRefID] [int] NOT NULL,
	[relationshiptype] [nvarchar](50) NULL,
	[relationshipscope] [nvarchar](50) NULL,
	[EventSubjectID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTFORINCIDENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTFORINCIDENT](
	[EventID] [int] NOT NULL,
	[IncidentID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTPROPERTY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTPROPERTY](
	[EventPropertyID] [int] IDENTITY(1,1) NOT NULL,
	[EventPropertyGUID] [nvarchar](500) NULL,
	[EventPropertyIDREF] [nvarchar](500) NULL,
	[EventPropertyName] [nvarchar](150) NULL,
	[EventPropertyDescription] [nvarchar](max) NULL,
	[appears_random] [bit] NULL,
	[datatype] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTPROPERTYADDRESS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTPROPERTYADDRESS](
	[EventPropertyID] [int] NOT NULL,
	[AddressID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTPROPERTYFOREVENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTPROPERTYFOREVENT](
	[EventID] [int] NOT NULL,
	[EventPropertyID] [int] NOT NULL,
	[EventPropertyValue] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTSIGNATURE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTSIGNATURE](
	[EventID] [int] NOT NULL,
	[SignatureID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[total_packets_collected] [int] NULL,
	[total_bytes_collected] [int] NULL,
	[data_flow_direction] [nvarchar](50) NULL,
	[connection_start_datetime] [datetimeoffset](7) NULL,
	[connection_end_datetime] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTTYPE](
	[EventTypeID] [int] IDENTITY(1,1) NOT NULL,
	[EventTypeName] [nvarchar](150) NOT NULL,
	[EventTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXISTENCEENUMERATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXISTENCEENUMERATION](
	[ExistenceEnumerationID] [int] IDENTITY(1,1) NOT NULL,
	[ExistenceValue] [nvarchar](50) NOT NULL,
	[ExistenceDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOIT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOIT](
	[ExploitID] [int] IDENTITY(1,1) NOT NULL,
	[Referential] [nvarchar](50) NULL,
	[RefID] [nvarchar](250) NULL,
	[Name] [nvarchar](250) NULL,
	[Location] [nvarchar](max) NULL,
	[Date] [date] NULL,
	[Verification] [bit] NULL,
	[Platform] [nvarchar](50) NULL,
	[Author] [nvarchar](250) NULL,
	[RPORT] [int] NULL,
	[saint_id] [nchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[Type] [nchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITABILITY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITABILITY](
	[ExploitabilityID] [int] IDENTITY(1,1) NOT NULL,
	[ExploitabilityLevel] [nvarchar](50) NOT NULL,
	[ExploitabilityDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITAUTHOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITAUTHOR](
	[ExploitID] [int] NOT NULL,
	[AuthorID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITFORCPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITFORCPE](
	[CPEID] [nvarchar](255) NOT NULL,
	[ExploitID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ExploitCPEName] [nvarchar](250) NULL,
	[ExploitCPEDescription] [nvarchar](max) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITFORREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITFORREFERENCE](
	[ReferenceID] [int] NOT NULL,
	[ExploitID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITFORTHREATACTORTTP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITFORTHREATACTORTTP](
	[ExploitID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITFORVULNERABILITY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITFORVULNERABILITY](
	[ExploitID] [int] NOT NULL,
	[VulnerabilityID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITLIKELIHOOD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITLIKELIHOOD](
	[ExploitLikelihoodID] [int] IDENTITY(1,1) NOT NULL,
	[Likelihood] [nvarchar](50) NOT NULL,
	[LikelihoodDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITLIKELIHOODFORATTACKPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITLIKELIHOODFORATTACKPATTERN](
	[ExploitLikelihoodID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[Explanation] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITLIKELIHOODFORCWE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITLIKELIHOODFORCWE](
	[ExploitLikelihoodForCWEID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[ExploitLikelihoodID] [int] NOT NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITOSINSTRUCTIONMEMORYADDRESS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITOSINSTRUCTIONMEMORYADDRESS](
	[ExploitID] [int] NOT NULL,
	[OSInstructionMemoryAddressID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITPARAMETER]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITPARAMETER](
	[ExploitParameterID] [int] IDENTITY(1,1) NOT NULL,
	[ExploitParameterName] [nvarchar](100) NOT NULL,
	[DefaultValue] [nvarchar](500) NULL,
	[ExploitParameterDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITPARAMETERFOREXPLOIT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITPARAMETERFOREXPLOIT](
	[ExploitID] [int] NOT NULL,
	[ExploitParameterID] [int] NOT NULL,
	[OrderRank] [int] NULL,
	[DefaultValue] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPOSURELEVEL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPOSURELEVEL](
	[ExposureLevelID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILE](
	[FileID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILEACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILEACTIONNAME](
	[FileActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[FileActionNameName] [nvarchar](150) NOT NULL,
	[FileActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILTERACTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILTERACTION](
	[FilterActionID] [int] IDENTITY(1,1) NOT NULL,
	[FilterActionValue] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDING](
	[FindingID] [int] IDENTITY(1,1) NOT NULL,
	[FindingName] [nvarchar](250) NULL,
	[FindingDescription] [nvarchar](max) NULL,
	[EndPointID] [int] NULL,
	[FindingStatus] [nvarchar](50) NULL,
	[SecurityLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ReportedDate] [datetimeoffset](7) NULL,
	[FindingDecision] [nvarchar](50) NULL,
	[MitigationDate] [datetimeoffset](7) NULL,
	[RemediationDate] [datetimeoffset](7) NULL,
	[FindingResult] [nvarchar](max) NULL,
	[FindingURL] [nvarchar](500) NULL,
	[VulnerableParameterType] [nvarchar](50) NULL,
	[VulnerableParameter] [nvarchar](250) NULL,
	[VulnerableParameterValue] [nvarchar](max) NULL,
	[FindingRequest] [nvarchar](max) NULL,
	[RequestType] [nvarchar](50) NULL,
	[FindingResponse] [nvarchar](max) NULL,
	[IsFalsePositive] [bit] NULL,
	[VulnerabilityID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGASSET](
	[FindingAssetID] [int] IDENTITY(1,1) NOT NULL,
	[FindingID] [int] NULL,
	[AssetID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[FindingAssetName] [nvarchar](250) NULL,
	[FindingAssetDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGPERSON]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGPERSON](
	[FindingPersonID] [int] IDENTITY(1,1) NOT NULL,
	[FindingID] [int] NULL,
	[PersonID] [int] NULL,
	[FindingPersonName] [nvarchar](50) NULL,
	[FindingPersonDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGRECOMMENDATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGRECOMMENDATION](
	[FindingRecommendationID] [int] IDENTITY(1,1) NOT NULL,
	[FindingID] [int] NOT NULL,
	[RecommendationID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[FindingRecommendationName] [nvarchar](250) NULL,
	[FindingRecommendationDescription] [nvarchar](max) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGVULNERABILITY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGVULNERABILITY](
	[FindingVulnerabilityID] [int] IDENTITY(1,1) NOT NULL,
	[FindingID] [int] NULL,
	[VulnerabilityID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[FindingVulnerabilityName] [nvarchar](250) NULL,
	[FindingVulnerabilityDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIXACTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIXACTION](
	[FixActionID] [int] IDENTITY(1,1) NOT NULL,
	[description] [nvarchar](max) NULL,
	[type] [nvarchar](100) NULL,
	[source] [nvarchar](250) NULL,
	[VocabularyID] [int] NULL,
	[lang] [nvarchar](10) NULL,
	[id] [nvarchar](250) NULL,
	[reboot] [bit] NULL,
	[strategy] [nvarchar](50) NULL,
	[disruption] [nvarchar](50) NULL,
	[complexity] [nvarchar](50) NULL,
	[systemURI] [nvarchar](250) NULL,
	[platformURI] [nvarchar](500) NULL,
	[XCCDFContent] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIXACTIONCOST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIXACTIONCOST](
	[FixActionCostID] [int] IDENTITY(1,1) NOT NULL,
	[cost_corrective_action] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIXACTIONFORBENCHMARKRULERESULT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIXACTIONFORBENCHMARKRULERESULT](
	[BenchmarkRuleResultID] [int] NOT NULL,
	[FixActionID] [int] NOT NULL,
	[XCCDFContent] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIXACTIONFORFIXACTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIXACTIONFORFIXACTION](
	[FixActionRefID] [int] NOT NULL,
	[relationshiptype] [nvarchar](50) NULL,
	[FixActionSubjectID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIXACTIONFORINCIDENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIXACTIONFORINCIDENT](
	[FixActionForIncidentID] [int] IDENTITY(1,1) NOT NULL,
	[FixActionID] [int] NOT NULL,
	[IncidentID] [int] NOT NULL,
	[FixActionCostID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIXACTIONFORVULNERABILITY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIXACTIONFORVULNERABILITY](
	[FixActionID] [int] NOT NULL,
	[VulnerabilityID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIXSYSTEM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIXSYSTEM](
	[FixSystemID] [int] IDENTITY(1,1) NOT NULL,
	[systemURI] [nvarchar](250) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FLAG]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FLAG](
	[FlagID] [int] IDENTITY(1,1) NOT NULL,
	[FlagValue] [nvarchar](50) NOT NULL,
	[FlagDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FRAMEWORK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FRAMEWORK](
	[FrameworkID] [int] IDENTITY(1,1) NOT NULL,
	[FrameworkName] [nvarchar](250) NOT NULL,
	[FrameworkVersion] [nvarchar](50) NULL,
	[FrameworkDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FRAMEWORKFORTECHNICALCONTEXT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FRAMEWORKFORTECHNICALCONTEXT](
	[FrameworkID] [int] NOT NULL,
	[TechnicalContextID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FRAMEWORKREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FRAMEWORKREFERENCE](
	[FrameworkReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[FrameworkReferenceDescription] [datetimeoffset](7) NULL,
	[FrameworkID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FREQUENCY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FREQUENCY](
	[FrequencyID] [int] IDENTITY(1,1) NOT NULL,
	[rate] [float] NOT NULL,
	[scale] [nvarchar](100) NOT NULL,
	[TrendID] [int] NULL,
	[TrendName] [nvarchar](50) NULL,
	[TimeUnitID] [int] NULL,
	[units] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FTPACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FTPACTIONNAME](
	[FTPActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[FTPActionNameName] [nvarchar](150) NOT NULL,
	[FTPActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FUNCTION](
	[FunctionID] [int] IDENTITY(1,1) NOT NULL,
	[FunctionName] [nvarchar](100) NOT NULL,
	[VocabularyID] [int] NULL,
	[KnownVulnerable] [bit] NULL,
	[deprecated] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FUNCTIONARGUMENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FUNCTIONARGUMENT](
	[FunctionArgumentID] [int] IDENTITY(1,1) NOT NULL,
	[FunctionID] [int] NULL,
	[FunctionArgumentName] [nvarchar](150) NOT NULL,
	[FunctionArgumentDescription] [nvarchar](max) NULL,
	[FunctionArgumentType] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GEOLOCATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GEOLOCATION](
	[GeoLocationID] [int] IDENTITY(1,1) NOT NULL,
	[room_identifier] [nvarchar](50) NULL,
	[building_number] [nvarchar](50) NULL,
	[street_address] [nvarchar](250) NULL,
	[city] [nvarchar](100) NULL,
	[state] [nvarchar](100) NULL,
	[postal_code] [nvarchar](10) NULL,
	[country] [nvarchar](100) NULL,
	[latitude] [int] NULL,
	[longitude] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GROUPINGRELATIONSHIP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GROUPINGRELATIONSHIP](
	[GroupingRelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[GroupingRelationshipName] [nvarchar](150) NOT NULL,
	[GroupingRelationshipDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GUIACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GUIACTIONNAME](
	[GUIActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[GUIActionNameName] [nvarchar](150) NOT NULL,
	[GUIActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GUIDELINE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GUIDELINE](
	[GuidelineID] [int] IDENTITY(1,1) NOT NULL,
	[GuidelineText] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GUIDELINEFORATTACKPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GUIDELINEFORATTACKPATTERN](
	[GuidelineID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GUIDIALOGBOX]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GUIDIALOGBOX](
	[GUIDialogboxID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GUIOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GUIOBJECT](
	[GUIObjectID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GUIWINDOW]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GUIWINDOW](
	[GUIWindowID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HASHNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HASHNAME](
	[HashNameID] [int] IDENTITY(1,1) NOT NULL,
	[HashingAlgorithmName] [nvarchar](50) NOT NULL,
	[HashingAlgorithmDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HASHVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HASHVALUE](
	[HashValueID] [int] IDENTITY(1,1) NOT NULL,
	[HashNameID] [int] NULL,
	[HashValueValue] [nvarchar](100) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HOOKINGACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HOOKINGACTIONNAME](
	[HookingActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[HookingActionNameName] [nvarchar](150) NOT NULL,
	[HookingActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HOST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HOST](
	[HostID] [int] IDENTITY(1,1) NOT NULL,
	[ipaddressIPv4] [nvarchar](50) NULL,
	[macaddress] [nvarchar](50) NULL,
	[OsName] [nvarchar](250) NULL,
	[HostService] [nvarchar](50) NULL,
	[HostVersion] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CPEID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HOSTENDPOINT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HOSTENDPOINT](
	[HostEndPointID] [int] IDENTITY(1,1) NOT NULL,
	[HostPort] [int] NULL,
	[HostProtocol] [nvarchar](50) NULL,
	[HostID] [int] NULL,
	[HostService] [nvarchar](50) NULL,
	[HostVersion] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPACTIONNAME](
	[HTTPActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[HTTPActionNameName] [nvarchar](150) NOT NULL,
	[HTTPActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPSESSION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPSESSION](
	[HTTPSessionID] [int] IDENTITY(1,1) NOT NULL,
	[SessionID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HUMANRISK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HUMANRISK](
	[HumanRiskID] [int] IDENTITY(1,1) NOT NULL,
	[HumanRiskName] [nvarchar](100) NOT NULL,
	[HumanRiskDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IDENTIFICATIONSYSTEM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IDENTIFICATIONSYSTEM](
	[IdentificationSystemID] [int] IDENTITY(1,1) NOT NULL,
	[SystemURI] [nvarchar](250) NOT NULL,
	[IdentifierValueDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IMPACT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IMPACT](
	[ImpactID] [int] IDENTITY(1,1) NOT NULL,
	[TechnicalImpact] [bit] NULL,
	[BusinessImpact] [bit] NULL,
	[ImpactName] [nvarchar](50) NULL,
	[ImpactDescription] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IMPORTANCETYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IMPORTANCETYPE](
	[ImportanceTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ImportanceTypeName] [nvarchar](50) NOT NULL,
	[ImportanceTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENT]    Script Date: 11/22/2013 11:59:50 AM ******/
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
	[AlternativeID] [nvarchar](100) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTCATEGORY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTCATEGORY](
	[IncidentCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentCategoryName] [nvarchar](150) NOT NULL,
	[IncidentCategoryDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTCOMPROMISE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTCOMPROMISE](
	[IncidentCompromiseID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityCompromise] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL,
	[documentation] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTDISCOVERYMETHOD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTDISCOVERYMETHOD](
	[IncidentDiscoveryMethodID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentDiscoveryMethodName] [nvarchar](100) NOT NULL,
	[IncidentDiscoveryMethodDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTEFFECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTEFFECT](
	[IncidentEffectID] [int] IDENTITY(1,1) NOT NULL,
	[PossibleEffect] [nvarchar](150) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTFORASSET](
	[IncidentID] [int] NOT NULL,
	[AssetID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[notes] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTFORINCIDENT]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INCIDENTFORPERSON]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INCIDENTFORTHREATCAMPAIGN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTFORTHREATCAMPAIGN](
	[IncidentID] [int] NOT NULL,
	[CampaignID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTID]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTID](
	[IncidentIDID] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[instance] [nvarchar](50) NULL,
	[restriction] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTIMPACT]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INCIDENTIMPACTAVAILABILITYLOSSDURATION]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INCIDENTIMPACTAVAILABILITYVARIETY]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INCIDENTIMPACTCONFIDENTIALITYSTATE]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INCIDENTIMPACTCONFIDENTIALITYVARIETY]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INCIDENTIMPACTINTEGRITYVARIETY]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INCIDENTIMPACTLOSSPROPERTY]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INCIDENTIMPACTLOSSRATING]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INCIDENTIMPACTLOSSVARIETY]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INCIDENTIMPACTRATING]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INCIDENTINQUIRY]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INCIDENTINQUIRYINTENT]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INCIDENTIOC]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INCIDENTIOCFORTHREATCAMPAIGN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTIOCFORTHREATCAMPAIGN](
	[IncidentIOCID] [int] NOT NULL,
	[ThreatCampaignID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTIOCTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INCIDENTIOCTYPEFORINDICATOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTIOCTYPEFORINDICATOR](
	[IncidentIOCTypeID] [int] NOT NULL,
	[IndicatorID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTREGISTRYHANDLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTREGISTRYHANDLE](
	[IncidentRegistryHandleID] [int] IDENTITY(1,1) NOT NULL,
	[registry] [nvarchar](200) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTSTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INCIDENTSTATUS](
	[IncidentStatusID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentStatusName] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INCIDENTTIMELINE]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INCIDENTTIMELINEUNIT]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[INDICATOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATOR](
	[IndicatorID] [int] IDENTITY(1,1) NOT NULL,
	[IndicatorTitle] [nvarchar](100) NOT NULL,
	[IndicatorDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevel] [nvarchar](50) NULL,
	[LikelyImpact] [nvarchar](100) NULL,
	[Producer] [nvarchar](100) NULL,
	[negate] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INDICATORFORINDICATOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATORFORINDICATOR](
	[IndicatorRefID] [int] NOT NULL,
	[IndicatorSubjectID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INDICATORID]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATORID](
	[IndicatorIDID] [int] IDENTITY(1,1) NOT NULL,
	[IndicatorAlternativeID] [nvarchar](250) NOT NULL,
	[resource] [nvarchar](250) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INDICATORIDFORINCIDENTIOC]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATORIDFORINCIDENTIOC](
	[IndicatorIDID] [int] NOT NULL,
	[IncidentIOCID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INDICATORIDFORINDICATOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATORIDFORINDICATOR](
	[IndicatorIDID] [int] NOT NULL,
	[IndicatorID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INFORMATIONSOURCETYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INFORMATIONSOURCETYPE](
	[InformationSourceTypeID] [int] IDENTITY(1,1) NOT NULL,
	[InformationSourceTypeName] [nvarchar](150) NOT NULL,
	[InformationSourceTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INFORMATIONTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INFORMATIONTYPE](
	[InformationTypeID] [int] IDENTITY(1,1) NOT NULL,
	[InformationTypeName] [nvarchar](150) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INFORMATIONTYPEFORTHREATACTORTTP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INFORMATIONTYPEFORTHREATACTORTTP](
	[InformationTypeID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INFRASTRUCTURE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INFRASTRUCTURE](
	[InfrastructureID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INJECTIONVECTOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INJECTIONVECTOR](
	[InjectionVectorID] [int] IDENTITY(1,1) NOT NULL,
	[InjectionVectorText] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INJECTIONVECTORFORATTACKPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INJECTIONVECTORFORATTACKPATTERN](
	[InjectionVectorID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INSTANCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INSTANCE](
	[InstanceID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INSTRUCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INSTRUCTION](
	[InstructionID] [int] IDENTITY(1,1) NOT NULL,
	[OpcodeID] [int] NOT NULL,
	[Register1ID] [int] NULL,
	[Register2ID] [int] NULL,
	[InstructionOperand1Value] [nvarchar](50) NULL,
	[InstructionOperand2Value] [nvarchar](50) NULL,
	[InstructionHEXValue] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INTERACTIONLEVEL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INTERACTIONLEVEL](
	[InteractionLevelID] [int] IDENTITY(1,1) NOT NULL,
	[InteractionLevel] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INTERFACE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INTERFACE](
	[InterfaceID] [int] IDENTITY(1,1) NOT NULL,
	[InterfaceName] [nvarchar](500) NOT NULL,
	[ipaddressIPv4] [nvarchar](50) NULL,
	[ipaddressIPv6] [nvarchar](50) NULL,
	[MacAddress] [nvarchar](250) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INTERFACEFORSYSTEMINFO]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INTERFACEFORSYSTEMINFO](
	[SystemInfoID] [int] NOT NULL,
	[InterfaceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IPCACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IPCACTIONNAME](
	[IPCActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[IPCActionNameName] [nvarchar](150) NOT NULL,
	[IPCActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IRCACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IRCACTIONNAME](
	[IRCActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[IRCActionNameName] [nvarchar](150) NOT NULL,
	[IRCActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ISOCURRENCY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ISOCURRENCY](
	[iso_currency_code] [nvarchar](3) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[KEYWORD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[KEYWORD](
	[KeywordID] [int] IDENTITY(1,1) NOT NULL,
	[KeywordValue] [nvarchar](100) NOT NULL,
	[lang] [nvarchar](10) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[KILLCHAIN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[KILLCHAIN](
	[KillChainID] [int] IDENTITY(1,1) NOT NULL,
	[KillChainGID] [nvarchar](250) NULL,
	[KillChainName] [nvarchar](100) NOT NULL,
	[KillChainDefiner] [nvarchar](100) NULL,
	[KillChainReference] [nvarchar](250) NULL,
	[KillChainNumberOfPhases] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[KILLCHAINFORTHREATACTORTTP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[KILLCHAINFORTHREATACTORTTP](
	[KillChainID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[KILLCHAINPHASE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[KILLCHAINPHASE](
	[KillChainPhaseID] [int] IDENTITY(1,1) NOT NULL,
	[KillChainPhaseGID] [nvarchar](100) NULL,
	[KillChainPhaseName] [nvarchar](100) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[KILLCHAINPHASEFORKILLCHAIN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[KILLCHAINPHASEFORKILLCHAIN](
	[KillChainPhaseID] [int] NOT NULL,
	[KillChainID] [int] NOT NULL,
	[ordinality] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[KILLCHAINPHASEFORTHREATACTORTTP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[KILLCHAINPHASEFORTHREATACTORTTP](
	[KillChainPhaseID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LANGUAGE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LANGUAGE](
	[LanguageID] [int] IDENTITY(1,1) NOT NULL,
	[LanguageName] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LANGUAGECLASS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LANGUAGECLASS](
	[LanguageClassID] [int] IDENTITY(1,1) NOT NULL,
	[LanguageClassDescription] [nvarchar](500) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LANGUAGEFORAPPLICATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LANGUAGEFORAPPLICATION](
	[ApplicationID] [int] NOT NULL,
	[LanguageID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LANGUAGEFORTECHNICALCONTEXT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LANGUAGEFORTECHNICALCONTEXT](
	[LanguageID] [int] NOT NULL,
	[TechnicalContextID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LANGUAGEFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LANGUAGEFUNCTION](
	[LanguageID] [int] NOT NULL,
	[FunctionID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LIBRARY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LIBRARY](
	[LibraryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LIBRARYACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LIBRARYACTIONNAME](
	[LibraryActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[LibraryActionNameName] [nvarchar](150) NOT NULL,
	[LibraryActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LICENSE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LICENSE](
	[LicenseID] [int] IDENTITY(1,1) NOT NULL,
	[LicenseName] [nvarchar](250) NULL,
	[LicenseVersion] [nvarchar](50) NULL,
	[LicenseTypeID] [int] NULL,
	[LicenseDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LICENSETYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LICENSETYPE](
	[LicenseTypeID] [int] IDENTITY(1,1) NOT NULL,
	[LicenseTypeName] [nvarchar](250) NULL,
	[LicenseTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LINUXPACKAGE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LINUXPACKAGE](
	[LinuxPackageID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOCALE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOCALE](
	[LocaleID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOCATIONPOINT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOCATIONPOINT](
	[LocationPointID] [int] IDENTITY(1,1) NOT NULL,
	[latitude] [int] NOT NULL,
	[longitude] [int] NOT NULL,
	[elevation] [int] NULL,
	[radius] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOCATIONPOINTFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOCATIONPOINTFORASSET](
	[LocationPointID] [int] NOT NULL,
	[AssetID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[source] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOCATIONPOINTFORORGANISATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOCATIONPOINTFORORGANISATION](
	[LocationPointID] [int] NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[source] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOCATIONPOINTFORPERSON]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOCATIONPOINTFORPERSON](
	[LocationPointID] [int] NOT NULL,
	[PersonID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[source] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOCATIONREGION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOCATIONREGION](
	[LocationRegionID] [int] IDENTITY(1,1) NOT NULL,
	[regionname] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOCATIONREGIONFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOCATIONREGIONFORASSET](
	[LocationRegionID] [int] NOT NULL,
	[AssetID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[source] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOSSFACTOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOSSFACTOR](
	[LossFactorID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOSSFORM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOSSFORM](
	[LossFormID] [int] IDENTITY(1,1) NOT NULL,
	[LossFormName] [nvarchar](50) NOT NULL,
	[LossFormDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOSSPROPERTY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOSSPROPERTY](
	[LossPropertyID] [int] IDENTITY(1,1) NOT NULL,
	[LossPropertyName] [nvarchar](50) NOT NULL,
	[LossPropertyDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOSSPROPERTYFORINCIDENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOSSPROPERTYFORINCIDENT](
	[IncidentID] [int] NOT NULL,
	[LossPropertyID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MALWAREACTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MALWAREACTION](
	[MalwareActionID] [int] IDENTITY(1,1) NOT NULL,
	[ActionID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MALWAREAVCLASSIFICATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MALWAREAVCLASSIFICATION](
	[MalwareAVClassificationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MALWAREBEHAVIOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MALWAREBEHAVIOR](
	[MalwareBehaviorID] [int] IDENTITY(1,1) NOT NULL,
	[BehaviorID] [int] NULL,
	[ordinal_position] [int] NOT NULL,
	[status] [nvarchar](100) NULL,
	[ActionStatusID] [int] NULL,
	[duration] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MALWAREBUNDLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MALWAREBUNDLE](
	[MalwareBundleID] [int] IDENTITY(1,1) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MALWAREBUNDLETYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MALWAREBUNDLETYPE](
	[MalwareBundleTypeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MALWAREENTITY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MALWAREENTITY](
	[MalwareEntityID] [int] IDENTITY(1,1) NOT NULL,
	[MalwareEntityName] [nvarchar](50) NOT NULL,
	[MalwareEntityDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MALWAREINSTANCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MALWAREINSTANCE](
	[MalwareInstanceID] [int] IDENTITY(1,1) NOT NULL,
	[MalwareInstanceName] [nvarchar](100) NOT NULL,
	[MalwareInstanceDescription] [nvarchar](max) NULL,
	[MalwareHashMD5] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MALWAREMECHANISM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MALWAREMECHANISM](
	[MalwareMechanismID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MALWARENAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MALWARENAME](
	[MalwareNameID] [int] IDENTITY(1,1) NOT NULL,
	[MalwareName] [nvarchar](250) NOT NULL,
	[NameGivenBy] [nvarchar](500) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MALWARENAMEFORMALWAREINSTANCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MALWARENAMEFORMALWAREINSTANCE](
	[MalwareNameID] [int] NOT NULL,
	[MalwareInstanceID] [int] NOT NULL,
	[NameGivenBy] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MALWARESUBJECTRELATIONSHIP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MALWARESUBJECTRELATIONSHIP](
	[MalwareSubjectRelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[MalwareSubjectRelationshipName] [nvarchar](150) NOT NULL,
	[MalwareSubjectRelationshipDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MALWARETYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MALWARETYPE](
	[MalwareTypeID] [int] IDENTITY(1,1) NOT NULL,
	[MalwareTypeName] [nvarchar](100) NOT NULL,
	[MalwareTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MALWARETYPEFORMALWAREINSTANCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MALWARETYPEFORMALWAREINSTANCE](
	[MalwareTypeID] [int] NOT NULL,
	[MalwareInstanceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MATURITYRATING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MATURITYRATING](
	[MaturityRatingID] [int] IDENTITY(1,1) NOT NULL,
	[ScoringSystemID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEASURESOURCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEASURESOURCE](
	[MeasureSourceID] [int] IDENTITY(1,1) NOT NULL,
	[SourceClassID] [int] NULL,
	[SourceClassName] [nvarchar](50) NULL,
	[MeasureSourceName] [nvarchar](150) NULL,
	[SourceTypeID] [int] NULL,
	[SourceTypeName] [nvarchar](150) NULL,
	[MeasureSourceDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEASURESOURCECONTRIBUTOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEASURESOURCECONTRIBUTOR](
	[MeasureSourceID] [int] NOT NULL,
	[PersonID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEASURESOURCEINFORMATIONSOURCETYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEASURESOURCEINFORMATIONSOURCETYPE](
	[MeasureSourceID] [int] NOT NULL,
	[InformationSourceTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEASURESOURCEPLATFORM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEASURESOURCEPLATFORM](
	[MeasureSourceID] [int] NOT NULL,
	[PlatformID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEASURESOURCESYSTEM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEASURESOURCESYSTEM](
	[MeasureSourceID] [int] NOT NULL,
	[SystemID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEASURESOURCETOOL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEASURESOURCETOOL](
	[MeasureSourceID] [int] NOT NULL,
	[ToolInformationID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEASURESOURCETOOLTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEASURESOURCETOOLTYPE](
	[MeasureSourceID] [int] NOT NULL,
	[ToolTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEMORYADDRESS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEMORYADDRESS](
	[MemoryAddressID] [int] IDENTITY(1,1) NOT NULL,
	[MemoryAddressValue] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEMORYOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEMORYOBJECT](
	[MemoryObjectID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MESSAGELEVEL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MESSAGELEVEL](
	[MessageLevelID] [int] IDENTITY(1,1) NOT NULL,
	[MessageLevelValue] [nvarchar](50) NOT NULL,
	[MessageLevelDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[METADATA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[METADATA](
	[MetadataID] [int] IDENTITY(1,1) NOT NULL,
	[MetadataContent] [nvarchar](max) NOT NULL,
	[type] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[METHODOLOGY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[METHODOLOGY](
	[MethodologyID] [int] IDENTITY(1,1) NOT NULL,
	[MethodologyName] [nvarchar](100) NOT NULL,
	[lang] [nvarchar](10) NULL,
	[VocabularyID] [int] NULL,
	[MethodologyReference] [nvarchar](500) NULL,
	[MethodologyDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MIME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MIME](
	[MIMEID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATION](
	[MitigationID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationVocabularyID] [nvarchar](50) NULL,
	[SolutionMitigationText] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONFORATTACKPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONFORATTACKPATTERN](
	[MitigationID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONFORCWE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONFORCWE](
	[MitigationID] [int] NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONPHASE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONPHASE](
	[MitigationPhaseID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationPhaseName] [nvarchar](250) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONPHASEFORMITIGATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONPHASEFORMITIGATION](
	[MitigationPhaseForMitigationID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationID] [int] NOT NULL,
	[MitigationPhaseID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONREFERENCE](
	[MitigationReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[MitigationReferenceDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONSTRATEGY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONSTRATEGY](
	[MitigationStrategyID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationStrategyGUID] [nvarchar](500) NULL,
	[MitigationStrategyVocabularyID] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONSTRATEGYFORMITIGATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONSTRATEGYFORMITIGATION](
	[MitigationStrategyForMitigationID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationID] [int] NOT NULL,
	[MitigationStrategyID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MUTEX]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MUTEX](
	[MutexID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MUTEXNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MUTEXNAME](
	[MutexNameID] [int] IDENTITY(1,1) NOT NULL,
	[MutexID] [int] NULL,
	[MutexName] [nvarchar](250) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MUTEXTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MUTEXTYPE](
	[MutexTypeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NAICS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NAICS](
	[NAICSID] [int] IDENTITY(1,1) NOT NULL,
	[NAICSSector] [nvarchar](6) NOT NULL,
	[NAICSDescription] [nvarchar](200) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETROUTE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETROUTE](
	[NetRouteID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKACTIONNAME](
	[NetworkActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[NetworkActionNameName] [nvarchar](150) NOT NULL,
	[NetworkActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKCONNECTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKCONNECTION](
	[NetworkConnectionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKFLOW]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKFLOW](
	[NetworkFlowID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKPACKET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKPACKET](
	[NetworkPacketID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKROUTEENTRY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKROUTEENTRY](
	[NetworkRouteEntryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKSHAREACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKSHAREACTIONNAME](
	[NetworkShareActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[NetworkShareActionNameName] [nvarchar](150) NOT NULL,
	[NetworkShareActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKSOCKET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKSOCKET](
	[NetworkSocketID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKSUBNET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKSUBNET](
	[NetworkSubnetID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NOTIFICATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NOTIFICATION](
	[NotificationID] [int] IDENTITY(1,1) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[UserID] [uniqueidentifier] NULL,
	[NotificationMessage] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OBFUSCATIONTECHNIQUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OBFUSCATIONTECHNIQUE](
	[ObfuscationTechniqueID] [int] IDENTITY(1,1) NOT NULL,
	[ObfuscationTechniqueName] [nvarchar](150) NOT NULL,
	[ObfuscationTechniqueDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OBJECTRELATIONSHIP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OBJECTRELATIONSHIP](
	[ObjectRelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[ObjectRelationshipName] [nvarchar](150) NOT NULL,
	[ObjectRelationshipDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OBJECTSTATE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OBJECTSTATE](
	[ObjectStateID] [int] IDENTITY(1,1) NOT NULL,
	[ObjectStateName] [nvarchar](150) NOT NULL,
	[ObjectStateDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OBSERVATIONMETHOD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OBSERVATIONMETHOD](
	[ObservationMethodID] [int] IDENTITY(1,1) NOT NULL,
	[ObservationMethodName] [nvarchar](150) NOT NULL,
	[MeasureSourceID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OPCODE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OPCODE](
	[OpcodeID] [int] IDENTITY(1,1) NOT NULL,
	[OpcodeName] [nvarchar](50) NOT NULL,
	[OpcodeDescription] [nvarchar](max) NULL,
	[OpcodeHEXValue] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OPCODEFORCPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OPCODEFORCPE](
	[CPEID] [nvarchar](255) NOT NULL,
	[OpcodeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OPERATIONENUMERATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OPERATIONENUMERATION](
	[OperationEnumerationID] [int] IDENTITY(1,1) NOT NULL,
	[OperationValue] [nvarchar](50) NOT NULL,
	[OperationDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OPERATIONENUMERATIONFORSIMPLEDATATYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OPERATIONENUMERATIONFORSIMPLEDATATYPE](
	[SimpleDataTypeID] [int] NOT NULL,
	[OperationEnumerationID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OPERATORENUMERATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OPERATORENUMERATION](
	[OperatorEnumerationID] [int] IDENTITY(1,1) NOT NULL,
	[OperatorValue] [nvarchar](50) NOT NULL,
	[OperatorDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANISATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANISATION](
	[OrganisationID] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationName] [nvarchar](100) NOT NULL,
	[OrganisationType] [nvarchar](100) NULL,
	[OrganisationKnownAs] [nvarchar](100) NULL,
	[industry] [nvarchar](6) NULL,
	[CountryID] [int] NOT NULL,
	[employee_count] [nvarchar](50) NULL,
	[revenueamount] [int] NULL,
	[iso_currency_code] [nvarchar](3) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[TrustLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANISATIONFORTHREATACTORTTP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANISATIONFORTHREATACTORTTP](
	[OrganisationID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[Information_Source] [nvarchar](250) NULL,
	[ConfidenceLevel] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[notes] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANIZATIONALUNIT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANIZATIONALUNIT](
	[OrganizationalUnitID] [int] IDENTITY(1,1) NOT NULL,
	[OUName] [nvarchar](250) NOT NULL,
	[OUDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANIZATIONALUNITFORORGANISATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANIZATIONALUNITFORORGANISATION](
	[OrganisationUnitsID] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[OrganizationalUnitID] [int] NOT NULL,
	[OUChildName] [nvarchar](150) NULL,
	[OUChildDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OS](
	[IDOS] [int] NOT NULL,
	[OSname] [nchar](10) NOT NULL,
	[OSversion] [nchar](10) NULL,
	[LocaleID] [int] NULL,
	[OSlang] [nchar](10) NULL,
	[OSSP] [nchar](10) NULL,
	[Platform] [nchar](10) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OSFAMILY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSFAMILY](
	[OSFamilyID] [int] IDENTITY(1,1) NOT NULL,
	[FamilyName] [nvarchar](50) NULL,
	[FamilyDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OSFAMILYFOROS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSFAMILYFOROS](
	[IDOS] [int] NOT NULL,
	[OSFamilyID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OSILAYER]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSILAYER](
	[OSILayerID] [int] IDENTITY(1,1) NOT NULL,
	[OSILayerName] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OSILAYERFORATTACKSURFACE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSILAYERFORATTACKSURFACE](
	[OSILayerID] [int] NOT NULL,
	[AttackSurfaceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OSINSTRUCTIONMEMORYADDRESS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSINSTRUCTIONMEMORYADDRESS](
	[OSInstructionMemoryAddressID] [int] IDENTITY(1,1) NOT NULL,
	[IDOS] [int] NOT NULL,
	[InstructionID] [int] NOT NULL,
	[MemoryAddressID] [int] NOT NULL,
	[OSPatchLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OSPATCHLEVEL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSPATCHLEVEL](
	[OSPatchlevelID] [int] IDENTITY(1,1) NOT NULL,
	[IDOS] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OSPATCHLEVELPATCH]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSPATCHLEVELPATCH](
	[OSPatchLevelID] [int] NOT NULL,
	[PatchID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALAFFECTED]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALAFFECTED](
	[OVALAffectedID] [int] IDENTITY(1,1) NOT NULL,
	[OSFamilyID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALAFFECTEDFOROVALMETADATA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALAFFECTEDFOROVALMETADATA](
	[OVALMetadataID] [int] NOT NULL,
	[OVALAffectedID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALAFFECTEDPLATFORM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALAFFECTEDPLATFORM](
	[OVALAffectedID] [int] NOT NULL,
	[PlatformID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALAFFECTEDPRODUCT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALAFFECTEDPRODUCT](
	[OVALAffectedID] [int] NOT NULL,
	[ProductID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALBEHAVIOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALBEHAVIOR](
	[OVALBehaviorID] [int] IDENTITY(1,1) NOT NULL,
	[BehaviorKey] [nvarchar](50) NULL,
	[BehaviorValue] [nvarchar](50) NULL,
	[BehaviorID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALBEHAVIORFOROVALOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALBEHAVIORFOROVALOBJECT](
	[OVALObjectID] [int] NOT NULL,
	[OVALBehaviorID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCLASSDIRECTIVES]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCLASSDIRECTIVES](
	[OVALClassDirectivesID] [int] IDENTITY(1,1) NOT NULL,
	[OVALClassEnumerationID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCLASSDIRECTIVESFOROVALDIRECTIVES]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCLASSDIRECTIVESFOROVALDIRECTIVES](
	[OVALDirectivesID] [int] NOT NULL,
	[OVALClassDirectivesID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCLASSDIRECTIVESFOROVALRESULTS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCLASSDIRECTIVESFOROVALRESULTS](
	[OVALResultsID] [int] NOT NULL,
	[OVALClassDirectivesID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCLASSENUMERATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCLASSENUMERATION](
	[OVALClassEnumerationID] [int] IDENTITY(1,1) NOT NULL,
	[ClassValue] [nvarchar](50) NOT NULL,
	[ClassDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCOMPONENTGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCOMPONENTGROUP](
	[OVALComponentGroupID] [int] IDENTITY(1,1) NOT NULL,
	[OVALVariableID] [int] NULL,
	[OVALFunctionID] [int] NULL,
	[FunctionName] [nvarchar](50) NULL,
	[FunctionOperation] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCOMPONENTGROUPFORARITHMETICFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCOMPONENTGROUPFORARITHMETICFUNCTION](
	[ArithmeticFunctionID] [int] NOT NULL,
	[OVALComponentGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCOMPONENTGROUPFORCONCATFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCOMPONENTGROUPFORCONCATFUNCTION](
	[ConcatFunctionID] [int] NOT NULL,
	[OVALComponentGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCOMPONENTGROUPFORCOUNTFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCOMPONENTGROUPFORCOUNTFUNCTION](
	[CountFunctionID] [int] NOT NULL,
	[OVALComponentGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCOMPONENTGROUPFOROVALLOCALVARIABLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCOMPONENTGROUPFOROVALLOCALVARIABLE](
	[OVALLocalVariableID] [int] NOT NULL,
	[OVALComponentGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCOMPONENTGROUPFORTIMEDIFFERENCEFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCOMPONENTGROUPFORTIMEDIFFERENCEFUNCTION](
	[TimeDifferenceFunctionID] [int] NOT NULL,
	[OVALComponentGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCOMPONENTGROUPFORUNIQUEFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCOMPONENTGROUPFORUNIQUEFUNCTION](
	[UniqueFunctionID] [int] NOT NULL,
	[OVALComponentGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCONSTANTVARIABLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCONSTANTVARIABLE](
	[OVALConstantVariableID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCONSTANTVARIABLEVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCONSTANTVARIABLEVALUE](
	[OVALConstantVariableID] [int] NOT NULL,
	[ValueID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCRITERIA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCRITERIA](
	[OVALCriteriaID] [int] IDENTITY(1,1) NOT NULL,
	[OperatorEnumerationID] [int] NULL,
	[OperatorValue] [nvarchar](50) NULL,
	[negate] [bit] NULL,
	[comment] [nvarchar](max) NOT NULL,
	[applicabilitycheck] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCRITERIAFOROVALCRITERIA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCRITERIAFOROVALCRITERIA](
	[OVALCriteriaRefID] [int] NOT NULL,
	[OVALCriteriaSubjectID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CriteriaRank] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCRITERIATYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALCRITERIATYPEFOROVALDEFINITIONTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCRITERIATYPEFOROVALDEFINITIONTYPE](
	[OVALDefinitionTypeID] [int] NOT NULL,
	[OVALCriteriaTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCRITERION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCRITERION](
	[OVALCriterionID] [int] IDENTITY(1,1) NOT NULL,
	[OVALTestIDPattern] [nvarchar](500) NOT NULL,
	[negate] [bit] NULL,
	[comment] [nvarchar](max) NULL,
	[applicabilitycheck] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCRITERIONFOROVALCRITERIA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCRITERIONFOROVALCRITERIA](
	[OVALCriteriaID] [int] NOT NULL,
	[OVALCriterionID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALCRITERIONTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALCRITERIONTYPEFOROVALCRITERIATYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALCRITERIONTYPEFOROVALCRITERIATYPE](
	[OVALCriteriaTypeID] [int] NOT NULL,
	[OVALCriterionTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFAULTDIRECTIVES]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFAULTDIRECTIVES](
	[OVALDefaultDirectivesID] [int] IDENTITY(1,1) NOT NULL,
	[include_source_definitions] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITION](
	[OVALDefinitionID] [int] IDENTITY(1,1) NOT NULL,
	[OVALDefinitionIDPattern] [nvarchar](500) NOT NULL,
	[OVALDefinitionVersion] [int] NOT NULL,
	[OVALClassEnumerationID] [int] NULL,
	[ClassValue] [nvarchar](50) NULL,
	[deprecated] [bit] NULL,
	[OVALMetadataID] [int] NULL,
	[notes] [nvarchar](max) NULL,
	[OVALCriteriaID] [int] NULL,
	[signature] [nvarchar](max) NULL,
	[StatusName] [nvarchar](150) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONORGANISATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONORGANISATION](
	[OVALDefinitionID] [int] NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[OrganisationRole] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONS](
	[OVALDefinitionsID] [int] IDENTITY(1,1) NOT NULL,
	[GeneratorTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDEFINITIONSTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALDEFINITIONTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALDEFINITIONTYPEFOROVALSYSTEMTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALDEFINITIONTYPEFOROVALSYSTEMTYPE](
	[OVALSystemTypeID] [int] NOT NULL,
	[OVALDefinitionTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALDIRECTIVE]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALDIRECTIVES]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALDIRECTIVESTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALENTITYATTRIBUTEGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALENTITYCOMPLEXBASE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALENTITYCOMPLEXBASE](
	[OVALEntityComplexBaseID] [int] IDENTITY(1,1) NOT NULL,
	[OVALEntityAttributeGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALENTITYSIMPLEBASE]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALEXTENDDEFINITION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALEXTENDDEFINITION](
	[OVALExtendDefinitionID] [int] IDENTITY(1,1) NOT NULL,
	[OVALDefinitionID] [int] NOT NULL,
	[OVALDefinitionIDPattern] [nvarchar](500) NOT NULL,
	[negate] [bit] NULL,
	[comment] [nvarchar](max) NULL,
	[applicabilitycheck] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALEXTENDDEFINITIONFOROVALCRITERIA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALEXTENDDEFINITIONFOROVALCRITERIA](
	[OVALCriteriaID] [int] NOT NULL,
	[OVALExtendDefinitionID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALEXTENDDEFINITIONTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALEXTENDDEFINITIONTYPE](
	[OVALExtendDefinitionTypeID] [int] IDENTITY(1,1) NOT NULL,
	[OVALDefinitionID] [int] NULL,
	[OVALDefinitionIDPattern] [nvarchar](500) NOT NULL,
	[OVALDefinitionVersion] [int] NOT NULL,
	[variable_instance] [int] NULL,
	[negate] [bit] NULL,
	[ResultEnumerationID] [int] NOT NULL,
	[applicability_check] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALEXTENSIONPOINT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALEXTENSIONPOINT](
	[ExtensionPointID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALEXTENSIONPOINTFOROVALGENERATORTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALEXTENSIONPOINTFOROVALGENERATORTYPE](
	[GeneratorTypeID] [int] NOT NULL,
	[ExtensionPointID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALEXTENSIONPOINTFOROVALMETADATA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALEXTENSIONPOINTFOROVALMETADATA](
	[OVALMetadataID] [int] NOT NULL,
	[ExtensionPointID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALEXTENSIONPOINTFORSYSTEMINFO]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALEXTENSIONPOINTFORSYSTEMINFO](
	[SystemInfoID] [int] NOT NULL,
	[OVALExtensionPointID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALEXTERNALVARIABLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALEXTERNALVARIABLE](
	[OVALExternalVariableID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALFILTER]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALFILTER](
	[OVALFilterID] [int] IDENTITY(1,1) NOT NULL,
	[OVALStateID] [int] NOT NULL,
	[FilterActionValue] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALFILTERFOROVALSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALFILTERFOROVALSET](
	[OVALSetID] [int] NOT NULL,
	[OVALFilterID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALFUNCTIONGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALFUNCTIONGROUP](
	[OVALFunctionGroupID] [int] IDENTITY(1,1) NOT NULL,
	[ArithmeticFunctionID] [int] NULL,
	[BeginFunctionID] [int] NULL,
	[ConcatFunctionID] [int] NULL,
	[CountFunctionID] [int] NULL,
	[EndFunctionID] [int] NULL,
	[EscapeRegexFunctionID] [int] NULL,
	[SplitFunctionID] [int] NULL,
	[SubstringFunctionID] [int] NULL,
	[TimeDifferenceFunctionID] [int] NULL,
	[UniqueFunctionID] [int] NULL,
	[RegexCaptureFunctionID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALFUNCTIONGROUPFOROVALCOMPONENTGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALFUNCTIONGROUPFOROVALCOMPONENTGROUP](
	[OVALComponentGroupID] [int] NOT NULL,
	[OVALFunctionGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALGENERATORTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALITEM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALITEM](
	[OVALItemID] [int] IDENTITY(1,1) NOT NULL,
	[OVALItemIDPattern] [nvarchar](500) NOT NULL,
	[StatusID] [int] NULL,
	[StatusName] [nvarchar](150) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALITEMATTRIBUTEGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALITEMCOMPLEXBASE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALITEMCOMPLEXBASE](
	[OVALItemComplexBaseID] [int] IDENTITY(1,1) NOT NULL,
	[OVALItemAttributeGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALITEMFOROVALSYSTEMOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALITEMFOROVALSYSTEMOBJECT](
	[OVALSystemObjectID] [int] NOT NULL,
	[OVALItemID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALITEMSIMPLEBASE]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALLITERALCOMPONENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALLITERALCOMPONENT](
	[OVALLiteralComponentID] [int] IDENTITY(1,1) NOT NULL,
	[SimpleDataTypeID] [int] NULL,
	[DataTypeName] [nvarchar](50) NULL,
	[LiteralComponentValue] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALLITERALCOMPONENTFOROVALCOMPONENTGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALLITERALCOMPONENTFOROVALCOMPONENTGROUP](
	[OVALComponentGroupID] [int] NOT NULL,
	[OVALLiteralComponentID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALLOCALVARIABLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALLOCALVARIABLE](
	[OVALLocalVariableID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALMESSAGETYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALMESSAGETYPEFOROVALDEFINITIONTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALMESSAGETYPEFOROVALDEFINITIONTYPE](
	[OVALDefinitionTypeID] [int] NOT NULL,
	[OVALMessageTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALMESSAGETYPEFOROVALITEM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALMESSAGETYPEFOROVALITEM](
	[OVALItemID] [int] NOT NULL,
	[MessageTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALMESSAGETYPEFOROVALSYSTEMOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALMESSAGETYPEFOROVALSYSTEMOBJECT](
	[OVALSystemObjectID] [int] NOT NULL,
	[MessageTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALMESSAGETYPEFOROVALTESTEDITEM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALMESSAGETYPEFOROVALTESTEDITEM](
	[OVALTestedItemID] [int] NOT NULL,
	[MessageTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALMESSAGETYPEFOROVALTESTTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALMESSAGETYPEFOROVALTESTTYPE](
	[OVALTestTypeID] [int] NOT NULL,
	[OVALMessageTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALMETADATA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALMETADATA](
	[OVALMetadataID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALMETADATACCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALMETADATACCE](
	[OVALMetadataID] [int] NOT NULL,
	[cce_id] [nvarchar](20) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALMETADATACPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALMETADATACPE](
	[OVALMetadataID] [int] NOT NULL,
	[CPEID] [nvarchar](255) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALMETADATAREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALMETADATAREFERENCE](
	[OVALMetadataID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALMETADATAVULNERABILITY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALMETADATAVULNERABILITY](
	[OVALMetadataID] [int] NOT NULL,
	[VulnerabilityID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECT](
	[OVALObjectID] [int] IDENTITY(1,1) NOT NULL,
	[OVALObjectIDPattern] [nvarchar](500) NOT NULL,
	[OVALObjectVersion] [int] NOT NULL,
	[comment] [nvarchar](max) NOT NULL,
	[deprecated] [bit] NULL,
	[notes] [nvarchar](max) NULL,
	[signature] [nvarchar](max) NULL,
	[DataTypeName] [nvarchar](50) NULL,
	[Namespace] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTCOMPONENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTCOMPONENT](
	[OVALObjectComponentID] [int] IDENTITY(1,1) NOT NULL,
	[OVALObjectID] [int] NOT NULL,
	[OVALObjectIDPattern] [nvarchar](500) NOT NULL,
	[OVALItemEntityName] [nvarchar](500) NOT NULL,
	[OVALItemEntityRecord] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTCOMPONENTFOROVALCOMPONENTGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTCOMPONENTFOROVALCOMPONENTGROUP](
	[OVALComponentGroupID] [int] NOT NULL,
	[OVALObjectComponentID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTFIELD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTFIELD](
	[OVALObjectFieldID] [int] IDENTITY(1,1) NOT NULL,
	[OVALEntityAttributeGroupID] [int] NULL,
	[FieldName] [nvarchar](500) NOT NULL,
	[OperationEnumerationID] [int] NULL,
	[OperationValue] [nvarchar](50) NULL,
	[FieldValue] [nvarchar](500) NULL,
	[DataTypeName] [nvarchar](50) NULL,
	[Namespace] [nvarchar](50) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CheckEnumerationID] [int] NULL,
	[VarCheck] [nvarchar](50) NULL,
	[VarRef] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTFIELDFOROVALOBJECTRECORD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTFIELDFOROVALOBJECTRECORD](
	[OVALObjectRecordID] [int] NOT NULL,
	[OVALObjectFieldID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTFOROVALSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTFOROVALSET](
	[OVALSetID] [int] NOT NULL,
	[OVALObjectID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTFOROVALTEST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTFOROVALTEST](
	[OVALTestID] [int] NOT NULL,
	[OVALObjectID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTRECORD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTRECORD](
	[OVALObjectRecordID] [int] IDENTITY(1,1) NOT NULL,
	[DataTypeName] [nvarchar](50) NOT NULL,
	[OperationValue] [nvarchar](50) NULL,
	[mask] [bit] NULL,
	[OVALVariableIDPattern] [nvarchar](500) NULL,
	[EnumerationValue] [nvarchar](50) NULL,
	[Namespace] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALOBJECTRECORDFOROVALOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALOBJECTRECORDFOROVALOBJECT](
	[OVALObjectID] [int] NOT NULL,
	[OVALObjectRecordID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALRESULTS]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALRESULTSTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALRESULTSTYPE](
	[OVALResultsTypeId] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSET](
	[OVALSetID] [int] IDENTITY(1,1) NOT NULL,
	[SetOperatorValue] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSETFOROVALSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSETFOROVALSET](
	[OVALSetRefID] [int] NOT NULL,
	[OVALSetSubjectID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSTATE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSTATE](
	[OVALStateID] [int] IDENTITY(1,1) NOT NULL,
	[OVALStateIDPattern] [nvarchar](500) NOT NULL,
	[OVALStateVersion] [int] NOT NULL,
	[OVALStateSimpleBaseID] [int] NULL,
	[OVALStateComplexBaseID] [int] NULL,
	[OVALStateType] [nvarchar](100) NULL,
	[DataTypeName] [nvarchar](50) NULL,
	[OperatorEnumerationValue] [nvarchar](50) NULL,
	[comment] [nvarchar](max) NOT NULL,
	[deprecated] [bit] NULL,
	[notes] [nvarchar](max) NULL,
	[signature] [nvarchar](max) NULL,
	[Namespace] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSTATECOMPLEXBASE]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALSTATEFIELD]    Script Date: 11/22/2013 11:59:50 AM ******/
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
	[OperationValue] [nvarchar](50) NULL,
	[CheckEnumerationID] [int] NULL,
	[EnumerationValue] [nvarchar](50) NULL,
	[FieldValue] [nvarchar](max) NULL,
	[Namespace] [nvarchar](50) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VarRef] [nvarchar](500) NULL,
	[OVALVariableID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSTATEFIELDFOROVALSTATERECORD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSTATEFIELDFOROVALSTATERECORD](
	[OVALStateRecordID] [int] NOT NULL,
	[OVALStateFieldID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSTATEFOROVALTEST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSTATEFOROVALTEST](
	[OVALTestID] [int] NOT NULL,
	[OVALStateID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSTATERECORD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSTATERECORD](
	[OVALStateRecordID] [int] IDENTITY(1,1) NOT NULL,
	[OVALStateComplexBaseID] [int] NULL,
	[DataTypeName] [nvarchar](50) NOT NULL,
	[OperationEnumerationID] [int] NULL,
	[OperationValue] [nvarchar](50) NULL,
	[mask] [bit] NULL,
	[OVALVariableIDPattern] [nvarchar](500) NULL,
	[CheckEnumerationID] [int] NULL,
	[EnumerationValue] [nvarchar](50) NULL,
	[Namespace] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSTATERECORDFOROVALSTATE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSTATERECORDFOROVALSTATE](
	[OVALStateID] [int] NOT NULL,
	[OVALStateRecordID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSTATESIMPLEBASE]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALSYSTEMCHARACTERISTICS]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALSYSTEMOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
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
	[FlagID] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSYSTEMTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSYSTEMTYPE](
	[OVALSystemTypeID] [int] IDENTITY(1,1) NOT NULL,
	[OVALSystemCharacteristicsID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALSYSTEMTYPEFOROVALRESULTSTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALSYSTEMTYPEFOROVALRESULTSTYPE](
	[OVALResultsTypeID] [int] NOT NULL,
	[OVALSystemTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALTEST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALTEST](
	[OVALTestID] [int] IDENTITY(1,1) NOT NULL,
	[OVALTestIDPattern] [nvarchar](500) NOT NULL,
	[OVALTestVersion] [int] NOT NULL,
	[ExistenceEnumerationID] [int] NULL,
	[ExistenceValue] [nvarchar](50) NULL,
	[CheckEnumerationID] [int] NULL,
	[EnumerationValue] [nvarchar](50) NOT NULL,
	[OperatorEnumerationID] [int] NULL,
	[OperatorValue] [nvarchar](50) NULL,
	[comment] [nvarchar](max) NOT NULL,
	[deprecated] [bit] NULL,
	[notes] [nvarchar](max) NULL,
	[signature] [nvarchar](max) NULL,
	[DataTypeName] [nvarchar](50) NULL,
	[Namespace] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALTESTEDITEM]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALTESTEDITEMFOROVALTESTTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALTESTEDITEMFOROVALTESTTYPE](
	[OVALTestTypeID] [int] NOT NULL,
	[OVALTestedItemID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALTESTEDVARIABLE]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALTESTEDVARIABLEFOROVALTESTTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALTESTEDVARIABLEFOROVALTESTTYPE](
	[OVALTestTypeID] [int] NOT NULL,
	[OVALTestedVariableId] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALTESTFOROVALTESTS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALTESTFOROVALTESTS](
	[OVALTestsID] [int] NOT NULL,
	[OVALTestID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALTESTS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALTESTS](
	[OVALTestsID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALTESTTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALTESTTYPEFOROVALSYSTEMTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALTESTTYPEFOROVALSYSTEMTYPE](
	[OVALSystemTypeID] [int] NOT NULL,
	[OVALTestTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALVARIABLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALVARIABLE](
	[OVALVariableID] [int] IDENTITY(1,1) NOT NULL,
	[OVALVariableIDPattern] [nvarchar](500) NOT NULL,
	[OVALVariableVersion] [int] NOT NULL,
	[DataTypeName] [nvarchar](50) NOT NULL,
	[comment] [nvarchar](max) NOT NULL,
	[deprecated] [bit] NULL,
	[signature] [nvarchar](max) NULL,
	[OVALExternalVariableID] [int] NULL,
	[OVALConstantVariableID] [int] NULL,
	[OVALLocalVariableID] [int] NULL,
	[Namespace] [nvarchar](50) NULL,
	[VariableType] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALVARIABLECOMPONENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALVARIABLECOMPONENT](
	[OVALVariableComponentID] [int] IDENTITY(1,1) NOT NULL,
	[OVALVariableID] [int] NOT NULL,
	[OVALVariableIDPattern] [nvarchar](500) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALVARIABLECOMPONENTFOROVALCOMPONENTGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALVARIABLECOMPONENTFOROVALCOMPONENTGROUP](
	[OVALComponentGroupID] [int] NOT NULL,
	[OVALVariableComponentID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALVARIABLEFOROVALVARIABLES]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALVARIABLEFOROVALVARIABLES](
	[OVALVariablesID] [int] NOT NULL,
	[OVALVariableID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALVARIABLES]    Script Date: 11/22/2013 11:59:50 AM ******/
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

/****** Object:  Table [dbo].[OVALVARIABLEVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALVARIABLEVALUE](
	[OVALVariableValueID] [int] IDENTITY(1,1) NOT NULL,
	[OVALVariableID] [int] NOT NULL,
	[ValueID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OVALVARIABLEVALUEFOROVALSYSTEMOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OVALVARIABLEVALUEFOROVALSYSTEMOBJECT](
	[OVALSystemObjectID] [int] NOT NULL,
	[OVALVariableValueID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OWASPTOP10]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OWASPTOP10](
	[OWASPTOP10ID] [int] IDENTITY(1,1) NOT NULL,
	[OWASPName] [nvarchar](150) NOT NULL,
	[OWASPDescription] [nvarchar](max) NULL,
	[Detectability] [nvarchar](50) NULL,
	[Rank] [int] NOT NULL,
	[YearTop10] [int] NULL,
	[ReferenceURL] [nvarchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OWASPTOP10ATTACKVECTOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OWASPTOP10ATTACKVECTOR](
	[OWASPTOP10ID] [int] NOT NULL,
	[AttackVectorID] [int] NOT NULL,
	[ExploitabilityLevel] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OWASPTOP10DEFENSETOOLTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OWASPTOP10DEFENSETOOLTYPE](
	[OWASPTOP10ID] [int] NOT NULL,
	[DefenseToolTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OWASPTOP10DETECTABILITY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OWASPTOP10DETECTABILITY](
	[OWASPTOP10ID] [int] NOT NULL,
	[DetectabilityID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OWASPTOP10EXPLOITABILITY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OWASPTOP10EXPLOITABILITY](
	[OWASPTOP10ExploitabilityID] [int] IDENTITY(1,1) NOT NULL,
	[OWASPTOP10ID] [int] NOT NULL,
	[ExploitabilityID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OWASPTOP10IMPACT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OWASPTOP10IMPACT](
	[OWASPTOP10ID] [int] NOT NULL,
	[ImpactID] [int] NOT NULL,
	[ImpactSeverity] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OWASPTOP10PREVALENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OWASPTOP10PREVALENCE](
	[OWASPTOP10ID] [int] NOT NULL,
	[PrevalenceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OWASPTOP10REFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OWASPTOP10REFERENCE](
	[OWASPTOP10ID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OWASPTOP10TOOLINFORMATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OWASPTOP10TOOLINFORMATION](
	[OWASPTOP10ID] [int] NOT NULL,
	[ToolInformationID] [int] NOT NULL,
	[Relationship] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PACKAGING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PACKAGING](
	[PackagingID] [int] IDENTITY(1,1) NOT NULL,
	[PackagingLayerName] [nvarchar](50) NOT NULL,
	[PackagingDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PACKAGINGCOMPRESSION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PACKAGINGCOMPRESSION](
	[PackagingID] [int] NOT NULL,
	[CompressionID] [int] NOT NULL,
	[LayerOrder] [int] NOT NULL,
	[CompressionPassword] [nvarchar](250) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PACKAGINGENCODING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PACKAGINGENCODING](
	[PackagingID] [int] NOT NULL,
	[EncodingID] [int] NOT NULL,
	[LayerOrder] [int] NOT NULL,
	[character_set] [nvarchar](250) NULL,
	[custom_character_set_ref] [nvarchar](250) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PACKAGINGENCRYPTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PACKAGINGENCRYPTION](
	[PackagingID] [int] NOT NULL,
	[EncryptionID] [int] NOT NULL,
	[LayerOrder] [int] NOT NULL,
	[encryption_key] [nvarchar](250) NULL,
	[encryption_key_ref] [nvarchar](250) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PARAMETERSFORPROVIDER]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PARAMETERSFORPROVIDER](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceCategoryID] [int] NOT NULL,
	[Strategy] [nvarchar](50) NULL,
	[Policy] [nvarchar](50) NULL,
	[ProviderID] [int] NOT NULL,
	[Parameters] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PASSWORDQUESTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PASSWORDQUESTION](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Label] [text] NULL,
	[Value] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PATCH]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PATCH](
	[PatchID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PATCHREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PATCHREFERENCE](
	[PatchID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PATTERNFIELDGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PATTERNFIELDGROUP](
	[PatternFieldGroupID] [int] IDENTITY(1,1) NOT NULL,
	[ConditionApplicationID] [int] NULL,
	[apply_condition] [nvarchar](50) NULL,
	[bit_mask] [nvarchar](50) NULL,
	[ConditionID] [int] NULL,
	[condition] [nvarchar](50) NULL,
	[has_changed] [bit] NULL,
	[PatternTypeID] [int] NULL,
	[pattern_type] [nvarchar](50) NULL,
	[regex_syntax] [nvarchar](50) NULL,
	[trend] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PATTERNTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PATTERNTYPE](
	[PatternTypeID] [int] IDENTITY(1,1) NOT NULL,
	[PatternTypeName] [nvarchar](50) NOT NULL,
	[PatternTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PDFFILE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PDFFILE](
	[PDFFileID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSON]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSON](
	[PersonID] [int] IDENTITY(1,1) NOT NULL,
	[PrecedingTitle] [nvarchar](50) NULL,
	[Title] [nvarchar](50) NULL,
	[FirstName] [nvarchar](50) NULL,
	[MiddleName] [nvarchar](50) NULL,
	[LastNamePrefix] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[OtherName] [nvarchar](50) NULL,
	[Alias] [nvarchar](50) NULL,
	[Suffix] [nvarchar](50) NULL,
	[GeneralSuffix] [nvarchar](50) NULL,
	[PersonFunction] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[birthdate] [date] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[TrustLevelID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONBLACKLIST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONBLACKLIST](
	[PersonBlacklistID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[OrganisationID] [int] NULL,
	[AssetID] [int] NULL,
	[PhysicalLocationID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONDEVICE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONDEVICE](
	[PersonDeviceID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[DeviceID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[RACIValue] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[TrustLevelID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONFORAPPLICATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONFORAPPLICATION](
	[ApplicationID] [int] NOT NULL,
	[PersonID] [int] NOT NULL,
	[RelationShip] [nvarchar](max) NULL,
	[RACIValue] [nvarchar](50) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[TrustLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONFORASSET](
	[PersonID] [int] NOT NULL,
	[AssetID] [int] NOT NULL,
	[relationshiptype] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[RACIValue] [nvarchar](50) NULL,
	[TrustLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONFORINCIDENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONFORINCIDENT](
	[PersonID] [int] NOT NULL,
	[IncidentID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONFORORGANISATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONFORORGANISATION](
	[PersonOrganisationID] [int] NOT NULL,
	[PersonID] [int] NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[relationshiptype] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[RACIValue] [nvarchar](50) NULL,
	[TrustLevelID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONFORPERSONGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONFORPERSONGROUP](
	[PersonGroupID] [int] NOT NULL,
	[PersonID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONFORPROJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONFORPROJECT](
	[ProjectID] [int] NOT NULL,
	[PersonID] [int] NOT NULL,
	[PersonRole] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[TrustLevelID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONFORTHREATACTORTTP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONFORTHREATACTORTTP](
	[PersonID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[Information_Source] [nvarchar](250) NULL,
	[ConfidenceLevel] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[notes] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONGEOLOCATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONGEOLOCATION](
	[PersonGeoLocationID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[GeoLocationID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONGROUP](
	[PersonGroupID] [int] IDENTITY(1,1) NOT NULL,
	[PersonGroupName] [nvarchar](100) NOT NULL,
	[PersonGroupDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONPHYSICALLOCATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONPHYSICALLOCATION](
	[PersonPhysicalLocationID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[PhysicalLocationID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONWHITELIST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONWHITELIST](
	[PersonWhitelistID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[OrganisationID] [int] NULL,
	[AssetID] [int] NULL,
	[PhysicalLocationID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PHYSICALLOCATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PHYSICALLOCATION](
	[PhysicalLocationID] [int] IDENTITY(1,1) NOT NULL,
	[PhysicalLocationName] [nvarchar](200) NOT NULL,
	[PhysicalLocationDescription] [nvarchar](max) NULL,
	[TrustLevelID] [int] NULL,
	[VocabularyID] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PIPEOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PIPEOBJECT](
	[PipeObjectID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PKI]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PKI](
	[PKIID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PLATFORM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PLATFORM](
	[PlatformID] [int] IDENTITY(1,1) NOT NULL,
	[PlatformName] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL,
	[PlatformDescription] [nvarchar](max) NULL,
	[structuring_format] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PLATFORMFORCCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PLATFORMFORCCE](
	[PlatformID] [int] NOT NULL,
	[cce_id] [nvarchar](20) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PLATFORMFORTECHNICALCONTEXT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PLATFORMFORTECHNICALCONTEXT](
	[PlatformID] [int] NOT NULL,
	[TechnicalContextID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[POLICY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[POLICY](
	[PolicyID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PORT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PORT](
	[PortID] [int] IDENTITY(1,1) NOT NULL,
	[PortNumber] [int] NULL,
	[DefaultProtocolName] [nchar](100) NULL,
	[DefaultServiceName] [nchar](100) NULL,
	[PortName] [nvarchar](50) NULL,
	[PortDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PORTFOREXPLOIT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PORTFOREXPLOIT](
	[PortID] [int] NOT NULL,
	[ExploitID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PORTFORVULNERABILITY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PORTFORVULNERABILITY](
	[PortID] [int] NOT NULL,
	[VulnerabilityID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PORTPROTOCOL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PORTPROTOCOL](
	[PortID] [int] NOT NULL,
	[ProtocolID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[POSSIBLERESTRICTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[POSSIBLERESTRICTION](
	[PossibleRestrictionID] [int] IDENTITY(1,1) NOT NULL,
	[RestrictionHint] [nvarchar](500) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[POSSIBLERESTRICTIONFOROVALEXTERNALVARIABLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[POSSIBLERESTRICTIONFOROVALEXTERNALVARIABLE](
	[OVALExternalVariableID] [int] NOT NULL,
	[PossibleRestrictionID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[POSSIBLEVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[POSSIBLEVALUE](
	[PossibleValueID] [int] IDENTITY(1,1) NOT NULL,
	[PossibleValueHint] [nvarchar](500) NOT NULL,
	[PossibleValueValue] [nvarchar](500) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[POSSIBLEVALUEFOROVALEXTERNALVARIABLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[POSSIBLEVALUEFOROVALEXTERNALVARIABLE](
	[OVALExternalVariableID] [int] NOT NULL,
	[PossibleValueID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PREVALENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PREVALENCE](
	[PrevalenceID] [int] IDENTITY(1,1) NOT NULL,
	[PrevalenceName] [nvarchar](50) NOT NULL,
	[PrevalenceDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRIORITYLEVEL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRIORITYLEVEL](
	[PriorityLevelID] [int] IDENTITY(1,1) NOT NULL,
	[PriorityLevelName] [nvarchar](50) NULL,
	[PriorityLevelDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRIVILEGESFORROLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRIVILEGESFORROLE](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [uniqueidentifier] NULL,
	[Responsible] [bit] NULL,
	[Accountable] [bit] NULL,
	[Consulted] [bit] NULL,
	[Informed] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROBINGTECHNIQUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROBINGTECHNIQUE](
	[ProbingTechniqueID] [int] IDENTITY(1,1) NOT NULL,
	[ProbingTechniqueDescription] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROBINGTECHNIQUEFORATTACKPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROBINGTECHNIQUEFORATTACKPATTERN](
	[ProbingTechniqueID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROCEDURE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROCEDURE](
	[ProcedureID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROCESS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROCESS](
	[ProcessID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROCESSACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROCESSACTIONNAME](
	[ProcessActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessActionNameName] [nvarchar](150) NOT NULL,
	[ProcessActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROCESSMEMORYACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROCESSMEMORYACTIONNAME](
	[ProcessMemoryActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessMemoryActionNameName] [nvarchar](150) NOT NULL,
	[ProcessMemoryActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROCESSORTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROCESSORTYPE](
	[ProcessorTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessorTypeName] [nvarchar](50) NOT NULL,
	[ProcessorTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROCESSORTYPEREGISTER]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROCESSORTYPEREGISTER](
	[ProcessorTypeID] [int] NOT NULL,
	[RegisterID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROCESSTHREADACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROCESSTHREADACTIONNAME](
	[ProcessThreadActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessThreadActionNameName] [nvarchar](150) NOT NULL,
	[ProcessThreadActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRODUCT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRODUCT](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](150) NULL,
	[CPEID] [nvarchar](255) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ProductDescription] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECT](
	[ProjectID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectName] [nvarchar](500) NOT NULL,
	[ProjectDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ExpectedCompletionDate] [datetimeoffset](7) NULL,
	[DueDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECTFORAPPLICATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECTFORAPPLICATION](
	[ProjectForApplicationID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[PersonID] [int] NULL,
	[ProjectDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[OrganisationID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECTMETHODOLOGY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECTMETHODOLOGY](
	[ProjectMethodologyID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[MethodologyID] [int] NOT NULL,
	[PersonID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ProjectMethodologyDescription] [nvarchar](max) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECTPERSON]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECTPERSON](
	[ProjectPersonID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[PersonID] [int] NOT NULL,
	[ProjectPersonRole] [nvarchar](100) NULL,
	[ProjectPersonDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECTTASK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECTTASK](
	[ProjectTaskID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[TaskID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ProjectTaskName] [nvarchar](250) NULL,
	[ProjectTaskDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECTTASKPERSON]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECTTASKPERSON](
	[ProjectTaskPersonID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectTaskID] [int] NOT NULL,
	[PersonID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ProjectTaskPersonRole] [nvarchar](100) NULL,
	[ProjectTaskDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROTOCOL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROTOCOL](
	[ProtocolID] [int] IDENTITY(1,1) NOT NULL,
	[ProtocolName] [nvarchar](100) NOT NULL,
	[ProtocolRFC] [nvarchar](10) NULL,
	[VocabularyID] [int] NULL,
	[OSILayerID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROTOCOLFORPROTOCOL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROTOCOLFORPROTOCOL](
	[ProtocolRefID] [int] NOT NULL,
	[ProtocolRelationship] [nvarchar](50) NULL,
	[ProtocolSubjectID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROVIDER]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROVIDER](
	[ProviderID] [int] IDENTITY(1,1) NOT NULL,
	[ProviderName] [nvarchar](50) NULL,
	[PluginReference] [nvarchar](50) NULL,
	[ServiceCategoryID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROVIDERSFORACCOUNT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROVIDERSFORACCOUNT](
	[ProviderAccountID] [int] IDENTITY(1,1) NOT NULL,
	[ProviderID] [int] NULL,
	[AccountID] [int] NULL,
	[ValidUntil] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RACIMATRIX]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RACIMATRIX](
	[RACIMatrixID] [int] IDENTITY(1,1) NOT NULL,
	[TaskType] [nvarchar](50) NULL,
	[TaskID] [nvarchar](50) NULL,
	[RACIResponsability] [nvarchar](50) NULL,
	[UserID] [uniqueidentifier] NULL,
	[AccountID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RACITASK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RACITASK](
	[RACITaskID] [int] IDENTITY(1,1) NOT NULL,
	[TaskType] [nvarchar](max) NULL,
	[RACIResponsability] [nvarchar](50) NULL,
	[UserID] [uniqueidentifier] NULL,
	[AccountID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RATIONALE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RATIONALE](
	[RationaleID] [int] IDENTITY(1,1) NOT NULL,
	[RationaleText] [nvarchar](max) NOT NULL,
	[lang] [nvarchar](10) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RATIONALEFORBENCHMARKGROUP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RATIONALEFORBENCHMARKGROUP](
	[RationaleID] [int] NOT NULL,
	[BenchmarkGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RATIONALEFORBENCHMARKRULE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RATIONALEFORBENCHMARKRULE](
	[RationaleID] [int] NOT NULL,
	[BenchmarkRuleID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RECOMMENDATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RECOMMENDATION](
	[RecommendationID] [int] IDENTITY(1,1) NOT NULL,
	[RecommendationVocabularyID] [nvarchar](250) NULL,
	[RecommendationName] [nvarchar](250) NULL,
	[RecommendationLevel] [nvarchar](250) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[RecommendationDescription] [nvarchar](max) NULL,
	[RecommendationRationale] [nvarchar](max) NULL,
	[RemediationProcedure] [nvarchar](max) NULL,
	[RecommendationImpact] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[StatusID] [int] NULL,
	[ScoringStatusID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RECOMMENDATIONAUDITPROCEDURE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RECOMMENDATIONAUDITPROCEDURE](
	[RecommendationAuditProcedureID] [int] IDENTITY(1,1) NOT NULL,
	[RecommendationID] [int] NOT NULL,
	[AuditProcedureID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[RecommendationAuditProcedureName] [nvarchar](250) NULL,
	[RecommendationAuditProcedureDescription] [nvarchar](max) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RECOMMENDATIONCCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RECOMMENDATIONCCE](
	[RecommendationCCEID] [int] IDENTITY(1,1) NOT NULL,
	[RecommendationID] [int] NOT NULL,
	[cce_id] [nvarchar](20) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REFERENCE](
	[ReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[ReferenceSourceID] [nvarchar](50) NULL,
	[Source] [nvarchar](50) NULL,
	[Title] [nvarchar](max) NULL,
	[ReferenceDescription] [nvarchar](max) NULL,
	[Type] [nvarchar](50) NULL,
	[Url] [nvarchar](max) NULL,
	[lang] [nvarchar](10) NULL,
	[notes] [nvarchar](max) NULL,
	[ReferenceVersion] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REFERENCEAUTHOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REFERENCEAUTHOR](
	[ReferenceAuthorID] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[AuthorID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGEXCAPTUREFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGEXCAPTUREFUNCTION](
	[RegexCaptureFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[Regex] [nvarchar](500) NOT NULL,
	[OVALComponentGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGISTER]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGISTER](
	[RegisterID] [int] IDENTITY(1,1) NOT NULL,
	[RegisterName] [nvarchar](50) NOT NULL,
	[RegisterDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGISTRYACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGISTRYACTIONNAME](
	[RegistryActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[RegistryActionNameName] [nvarchar](150) NOT NULL,
	[RegistryActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGULATORYRISK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGULATORYRISK](
	[RegulatoryRiskID] [int] IDENTITY(1,1) NOT NULL,
	[RiskDescription] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RELATIONSHIPTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RELATIONSHIPTYPE](
	[RelationshipTypeID] [int] IDENTITY(1,1) NOT NULL,
	[RelationshipTypeTerm] [nvarchar](50) NULL,
	[RelationshipTypeDomain] [nvarchar](50) NULL,
	[RelationshipTypeRange] [nvarchar](50) NULL,
	[RelationshipTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REPORT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REPORT](
	[ReportID] [int] IDENTITY(1,1) NOT NULL,
	[ReportContent] [nvarchar](max) NULL,
	[ReferenceID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REPORTFORREPORTS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REPORTFORREPORTS](
	[ReportsID] [int] NOT NULL,
	[ReportID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REPORTREQUEST]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REPORTREQUEST](
	[ReportRequestID] [int] IDENTITY(1,1) NOT NULL,
	[ARFReportRequestID] [nvarchar](500) NOT NULL,
	[ReportRequestContent] [nvarchar](max) NULL,
	[ReferenceID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REPORTREQUESTFORREPORTREQUESTS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REPORTREQUESTFORREPORTREQUESTS](
	[ReportRequestsID] [int] NOT NULL,
	[ReportRequestID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REPORTREQUESTS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REPORTREQUESTS](
	[ReportRequestsID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REPORTS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REPORTS](
	[ReportsID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REPOSITORY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REPOSITORY](
	[RepositoryID] [int] IDENTITY(1,1) NOT NULL,
	[RepositoryName] [nvarchar](250) NULL,
	[RepositoryDescription] [nvarchar](max) NULL,
	[RepositoryURL] [nvarchar](500) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RESTRICTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RESTRICTION](
	[RestrictionID] [int] IDENTITY(1,1) NOT NULL,
	[OperationEnumerationValue] [nvarchar](50) NOT NULL,
	[VariableValue] [nvarchar](500) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RESTRICTIONFORPOSSIBLERESTRICTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RESTRICTIONFORPOSSIBLERESTRICTION](
	[PossibleRestrictionID] [int] NOT NULL,
	[RestrictionID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RESULTENUMERATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RESULTENUMERATION](
	[ResultEnumerationID] [int] IDENTITY(1,1) NOT NULL,
	[ResultEnumerationValue] [nvarchar](50) NOT NULL,
	[ResultEnumerationDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RISKRATING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RISKRATING](
	[RiskRatingID] [int] IDENTITY(1,1) NOT NULL,
	[MethodologyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ROPCHAIN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ROPCHAIN](
	[ROPChainID] [int] IDENTITY(1,1) NOT NULL,
	[ROPChainName] [nvarchar](200) NOT NULL,
	[ROPChainDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ROPCHAININSTRUCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ROPCHAININSTRUCTION](
	[ROPChainID] [int] NOT NULL,
	[InstructionID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ROPCHAINREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ROPCHAINREFERENCE](
	[ROPChainID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ROPGADGET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ROPGADGET](
	[ROPGadgetID] [int] IDENTITY(1,1) NOT NULL,
	[ROPGadgetName] [nvarchar](50) NULL,
	[ROPGadgetDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ROPGADGETFORROPCHAIN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ROPGADGETFORROPCHAIN](
	[ROPChainID] [int] NOT NULL,
	[ROPGadgetID] [int] NOT NULL,
	[ROPGadgetOrder] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ROPGADGETINSTRUCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ROPGADGETINSTRUCTION](
	[ROGGadgetID] [int] NOT NULL,
	[InstructionID] [int] NOT NULL,
	[InstructionOrder] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCENARIO]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCENARIO](
	[ScenarioID] [int] IDENTITY(1,1) NOT NULL,
	[ScenarioName] [nvarchar](150) NOT NULL,
	[ScenarioDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCENARIOFOROWASPTOP10]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCENARIOFOROWASPTOP10](
	[OWASPTOP10ID] [int] NOT NULL,
	[ScenarioID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCORINGFORMULA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCORINGFORMULA](
	[ScoringFormulaID] [int] IDENTITY(1,1) NOT NULL,
	[ScoringFormulaName] [nvarchar](150) NOT NULL,
	[ScoringFormulaAbbreviation] [nvarchar](10) NULL,
	[ScoringFormulaDescription] [nvarchar](max) NULL,
	[ScoringFormulaIndividualScore] [nvarchar](500) NULL,
	[ScoringFormulaHostScore] [nvarchar](500) NULL,
	[ScoringFormulaNotes] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCORINGSTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCORINGSTATUS](
	[ScoringStatusID] [int] IDENTITY(1,1) NOT NULL,
	[ScoringStatusName] [nvarchar](250) NULL,
	[ScoringStatusValue] [nvarchar](250) NULL,
	[ScoringStatusDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCORINGSYSTEM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCORINGSYSTEM](
	[ScoringSystemID] [int] IDENTITY(1,1) NOT NULL,
	[ScoringSystemName] [nvarchar](150) NOT NULL,
	[ScoringSystemDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCORINGSYSTEMFORMULAS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCORINGSYSTEMFORMULAS](
	[ScoringSystemID] [int] NOT NULL,
	[ScoringFormulaID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCORINGSYSTEMREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCORINGSYSTEMREFERENCE](
	[ScoringSystemID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCRIPT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCRIPT](
	[ScriptID] [int] IDENTITY(1,1) NOT NULL,
	[CommandsID] [int] NOT NULL,
	[CommandID] [int] NOT NULL,
	[CommandArgumentValue] [nvarchar](max) NULL,
	[ScriptName] [nvarchar](250) NULL,
	[ScriptDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECTION](
	[SectionID] [int] IDENTITY(1,1) NOT NULL,
	[SectionName] [nvarchar](250) NULL,
	[SectionDescription] [nvarchar](max) NULL,
	[SectionValue] [nvarchar](100) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromdate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYATTRIBUTE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYATTRIBUTE](
	[SecurityAttributeID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityAttributeCategoryID] [int] NOT NULL,
	[SecurityAttributeName] [nvarchar](50) NOT NULL,
	[data_disclosure] [nvarchar](50) NULL,
	[SecurityAttributeStateID] [int] NULL,
	[notes] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[durationvalue] [int] NULL,
	[durationunit] [nvarchar](10) NULL,
	[IncidentID] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYATTRIBUTECATEGORY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYATTRIBUTECATEGORY](
	[SecurityAttributeCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityAttributeCategoryName] [nvarchar](50) NOT NULL,
	[SecurityAttributeCategoryDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYATTRIBUTESTATE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYATTRIBUTESTATE](
	[SecurityAttributeStateID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityAttributeCategoryID] [int] NOT NULL,
	[SecurityAttributeStateName] [nvarchar](50) NOT NULL,
	[SecurityAttributeStateDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYATTRIBUTEVARIETY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYATTRIBUTEVARIETY](
	[SecurityAttributeVarietyID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityAttributeCategoryID] [int] NOT NULL,
	[SecurityAttributeVarietyName] [nvarchar](50) NOT NULL,
	[SecurityAttributeVarietyDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCHANGE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCHANGE](
	[SecurityChangeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROL](
	[SecurityControlID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityControlName] [nvarchar](250) NOT NULL,
	[SecurityControlAbbrevation] [nvarchar](20) NULL,
	[SecurityControlDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[SecurityControlFamilyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLFAMILY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLFAMILY](
	[SecurityControlFamilyID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityControlFamilyName] [nvarchar](50) NOT NULL,
	[SecurityControlFamilyDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLFORHUMANRISK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLFORHUMANRISK](
	[HumanRiskID] [int] NOT NULL,
	[SecurityControlID] [int] NOT NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLMAPPING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLMAPPING](
	[SecurityControlID1] [int] NOT NULL,
	[SecurityControlID2] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLSTRENGTH]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLSTRENGTH](
	[SecurityControlID] [int] NOT NULL,
	[ControlStrengthID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLTOOL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLTOOL](
	[SecurityControlID] [int] NOT NULL,
	[ToolInformationID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYMARKING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYMARKING](
	[SecurityMarkingID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYMETRIC]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYMETRIC](
	[SecurityMetricID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYPRINCIPLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYPRINCIPLE](
	[SecurityPrincipleID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityPrincipleName] [nvarchar](250) NOT NULL,
	[SecurityPrincipleDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYPRINCIPLEFORATTACKPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYPRINCIPLEFORATTACKPATTERN](
	[SecurityPrincipleID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYPROGRAM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYPROGRAM](
	[SecurityProgramID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityProgramName] [nvarchar](100) NOT NULL,
	[SecurityProgramDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[SecurityProgramTypeID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYPROGRAMTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYPROGRAMTYPE](
	[SecurityProgramTypeID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityProgramTypeName] [nvarchar](100) NOT NULL,
	[SecurityProgramTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYREQUIREMENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYREQUIREMENT](
	[SecurityRequirementID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityRequirementDescription] [nvarchar](max) NOT NULL,
	[SecurityRequirementTitle] [nvarchar](250) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYREQUIREMENTFORATTACKPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYREQUIREMENTFORATTACKPATTERN](
	[SecurityRequirementID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SEMAPHORE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SEMAPHORE](
	[SemaphoreID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SENSOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SENSOR](
	[SensorID] [int] IDENTITY(1,1) NOT NULL,
	[SensorName] [nvarchar](250) NULL,
	[SensorDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SERVICEACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SERVICEACTIONNAME](
	[ServiceActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceActionNameName] [nvarchar](150) NOT NULL,
	[ServiceActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SERVICECATEGORY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SERVICECATEGORY](
	[ServiceCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceCategoryName] [nvarchar](250) NOT NULL,
	[ServiceCategoryDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[StatusID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SESSION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SESSION](
	[SessionID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [uniqueidentifier] NULL,
	[SessionIDValue] [nvarchar](max) NULL,
	[SessionName] [nvarchar](250) NULL,
	[SessionDescription] [nvarchar](max) NULL,
	[DateStart] [datetimeoffset](7) NULL,
	[DateEnd] [datetimeoffset](7) NULL,
	[StatusID] [int] NULL,
	[Status] [nvarchar](50) NULL,
	[ServiceCategoryID] [int] NULL,
	[Parameters] [image] NULL,
	[SessionCronID] [int] NULL,
	[information] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SESSIONCOOKIE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SESSIONCOOKIE](
	[SessionCookieID] [int] IDENTITY(1,1) NOT NULL,
	[SessionID] [int] NOT NULL,
	[CookieID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[SessionCookieName] [nvarchar](250) NULL,
	[SessionCookieDescription] [nvarchar](max) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SESSIONCOOKIEATTRIBUTEVALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SESSIONCOOKIEATTRIBUTEVALUE](
	[SessionCookieAttributeValueID] [int] IDENTITY(1,1) NOT NULL,
	[SessionCookieID] [int] NOT NULL,
	[AttributeValueID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[SessionCookieAttributeValueName] [nvarchar](250) NULL,
	[SessionCookieAttributeValueDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SESSIONCRON]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SESSIONCRON](
	[SessionCronID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [uniqueidentifier] NULL,
	[CronExpression] [nvarchar](50) NULL,
	[Parameters] [image] NULL,
	[StatusID] [int] NULL,
	[Status] [nvarchar](50) NULL,
	[ServiceCategoryID] [int] NULL,
	[DateStart] [datetimeoffset](7) NULL,
	[DateEnd] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SETOPERATOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SETOPERATOR](
	[SetOperatorID] [int] IDENTITY(1,1) NOT NULL,
	[SetOperatorValue] [nvarchar](50) NOT NULL,
	[SetOperatorDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SEVERITYLEVEL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SEVERITYLEVEL](
	[SeverityLevelID] [int] IDENTITY(1,1) NOT NULL,
	[SeverityLevelName] [nvarchar](50) NOT NULL,
	[SeverityLevelDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIDTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIDTYPE](
	[SIDTypeID] [int] IDENTITY(1,1) NOT NULL,
	[SIDTypeName] [nvarchar](50) NOT NULL,
	[SIDTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIGNATURE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIGNATURE](
	[SignatureID] [int] IDENTITY(1,1) NOT NULL,
	[SignatureName] [nvarchar](150) NOT NULL,
	[SignatureDescription] [nvarchar](max) NULL,
	[SignatureBase64Binary] [nvarchar](max) NULL,
	[SeverityLevelID] [int] NULL,
	[SignatureSeverityLevel] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[SignatureTypeID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIGNATURECPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIGNATURECPE](
	[SignatureID] [int] NOT NULL,
	[CPEID] [nvarchar](255) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[SignatureCPEName] [nvarchar](250) NULL,
	[SignatureCPEDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIGNATUREEXPLOIT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIGNATUREEXPLOIT](
	[SignatureID] [int] NOT NULL,
	[ExploitID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[SignatureExploitName] [nvarchar](250) NULL,
	[SignatureExploitDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIGNATUREMALWAREINSTANCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIGNATUREMALWAREINSTANCE](
	[SignatureID] [int] NOT NULL,
	[MalwareInstanceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[SignatureMalwareInstanceName] [nvarchar](250) NULL,
	[SignatureMalwareInstanceDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIGNATUREPORT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIGNATUREPORT](
	[SignatureID] [int] NOT NULL,
	[PortID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIGNATUREPROTOCOL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIGNATUREPROTOCOL](
	[SignatureID] [int] NOT NULL,
	[ProtocolID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIGNATUREREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIGNATUREREFERENCE](
	[SignatureID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIGNATURETYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIGNATURETYPE](
	[SignatureTypeID] [int] IDENTITY(1,1) NOT NULL,
	[SignatureTypeName] [nvarchar](50) NOT NULL,
	[SignatureTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIGNATURETYPEREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIGNATURETYPEREFERENCE](
	[SignatureTypeID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIMPLEDATATYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIMPLEDATATYPE](
	[SimpleDataTypeID] [int] IDENTITY(1,1) NOT NULL,
	[DataTypeName] [nvarchar](50) NOT NULL,
	[DataTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SOCKETACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SOCKETACTIONNAME](
	[SocketActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[SocketActionNameName] [nvarchar](150) NOT NULL,
	[SocketActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SOCKETADDRESS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SOCKETADDRESS](
	[SocketAddressID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SOURCECLASS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SOURCECLASS](
	[SourceClassID] [int] IDENTITY(1,1) NOT NULL,
	[SourceClassName] [nvarchar](50) NOT NULL,
	[SourceClassDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SOURCETYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SOURCETYPE](
	[SourceTypeID] [int] IDENTITY(1,1) NOT NULL,
	[SourceTypeName] [nvarchar](150) NOT NULL,
	[SourceTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SPLITFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SPLITFUNCTION](
	[SplitFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[SplitDelimiter] [nvarchar](500) NOT NULL,
	[OVALComponentGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STANDARD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STANDARD](
	[StandardID] [int] IDENTITY(1,1) NOT NULL,
	[StandardName] [nvarchar](150) NOT NULL,
	[StandardDescription] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STANDARDREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STANDARDREFERENCE](
	[StandardID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STANDARDVOCABULARY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STANDARDVOCABULARY](
	[StandardID] [int] NOT NULL,
	[VocabularyID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STATUS](
	[StatusID] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](150) NOT NULL,
	[StatusDate] [nvarchar](50) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SUBSTRINGFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SUBSTRINGFUNCTION](
	[SubstringFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[SubstringStart] [int] NOT NULL,
	[SubstringLength] [int] NOT NULL,
	[OVALComponentGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SYNCHRONIZATIONACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SYNCHRONIZATIONACTIONNAME](
	[SynchronizationActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[SynchronizationActionNameName] [nvarchar](150) NOT NULL,
	[SynchronizationActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SYSTEM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SYSTEM](
	[SystemID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SYSTEMACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SYSTEMACTIONNAME](
	[SystemActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[SystemActionNameName] [nvarchar](150) NOT NULL,
	[SystemActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SYSTEMINFO]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SYSTEMINFO](
	[SystemInfoID] [int] IDENTITY(1,1) NOT NULL,
	[IDOS] [int] NOT NULL,
	[architecture] [nvarchar](150) NOT NULL,
	[primaryhostname] [nvarchar](250) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SYSTEMINFOFOROVALSYSTEMCHARACTERISTICS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SYSTEMINFOFOROVALSYSTEMCHARACTERISTICS](
	[OVALSystemCharacteristicsID] [int] NOT NULL,
	[SystemInfo] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SYSTEMTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SYSTEMTYPE](
	[SystemTypeID] [int] IDENTITY(1,1) NOT NULL,
	[SystemTypeName] [nvarchar](200) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SYSTEMTYPEFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SYSTEMTYPEFORASSET](
	[AssetID] [int] NOT NULL,
	[SystemTypeID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SYSTEMTYPEFORTHREATACTORTTP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SYSTEMTYPEFORTHREATACTORTTP](
	[SystemTypeID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TAG]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TAG](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[TagValue] [nvarchar](250) NULL,
	[TagType] [nvarchar](100) NULL,
	[AccountID] [int] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[TagDescription] [nvarchar](max) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TAGFORASSET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TAGFORASSET](
	[TagAssetID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NULL,
	[TagID] [int] NULL,
	[TagValue] [nvarchar](max) NULL,
	[TagAssetDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TASK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TASK](
	[TaskID] [int] IDENTITY(1,1) NOT NULL,
	[TaskName] [nvarchar](50) NULL,
	[TaskDescription] [nvarchar](max) NULL,
	[TaskPriority] [nvarchar](50) NULL,
	[TaskStatus] [nvarchar](50) NULL,
	[CompletionPercentage] [float] NULL,
	[ExpectedCompletionDate] [datetimeoffset](7) NULL,
	[StartDate] [datetimeoffset](7) NULL,
	[DueDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TASKATTACHMENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TASKATTACHMENT](
	[TaskAttachmentID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [int] NULL,
	[Title] [nvarchar](50) NULL,
	[Data] [image] NULL,
	[MimeType] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TASKFORPROJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TASKFORPROJECT](
	[ProjectTaskID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[TaskID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ProjectTaskName] [nvarchar](250) NULL,
	[ProjectTaskDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TASKPERSON]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TASKPERSON](
	[TaskID] [int] NOT NULL,
	[PersonID] [int] NOT NULL,
	[RelationshipType] [nvarchar](100) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TAXONOMY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TAXONOMY](
	[TaxonomyID] [int] IDENTITY(1,1) NOT NULL,
	[TaxonomyName] [nvarchar](50) NOT NULL,
	[TaxonomyDescription] [nvarchar](max) NULL,
	[TaxonomyVersion] [nvarchar](10) NULL,
	[TaxonomyReference] [nvarchar](250) NULL,
	[DateModified] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TAXONOMYNODE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TAXONOMYNODE](
	[TaxonomyNodeID] [int] IDENTITY(1,1) NOT NULL,
	[TaxonomyID] [int] NULL,
	[TaxonomyNodeName] [nvarchar](250) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[TaxonomyNodeDescription] [nvarchar](max) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TAXONOMYREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TAXONOMYREFERENCE](
	[TaxonomyReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[TaxonomyID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[TaxonomyReferenceName] [nvarchar](250) NULL,
	[TaxonomyReferenceDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TECHNICALCONTEXT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TECHNICALCONTEXT](
	[TechnicalContextID] [int] IDENTITY(1,1) NOT NULL,
	[capec_id] [nvarchar](20) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TELEPHONE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TELEPHONE](
	[TelephoneID] [int] IDENTITY(1,1) NOT NULL,
	[TelephoneNumber] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TELEPHONEFORORGANISATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TELEPHONEFORORGANISATION](
	[TelephoneID] [int] NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TELEPHONEFORPERSON]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TELEPHONEFORPERSON](
	[TelephoneID] [int] NOT NULL,
	[PersonID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TESTMECHANISMEFFICACY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TESTMECHANISMEFFICACY](
	[TestMechanismEfficacyID] [int] IDENTITY(1,1) NOT NULL,
	[Efficacy] [nvarchar](50) NOT NULL,
	[EfficacyDescription] [nvarchar](max) NULL,
	[ConfidenceLevel] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TESTMECHANISMEFFICACYFORCYBEROBSERVABLETESTMECHANISM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TESTMECHANISMEFFICACYFORCYBEROBSERVABLETESTMECHANISM](
	[TestMechanismEfficacyID] [int] NOT NULL,
	[CyberObservableTestMechanismID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TESTMECHANISMID]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TESTMECHANISMID](
	[TestMechanismID] [int] NOT NULL,
	[CyberObservableTestMechanismID] [int] NOT NULL,
	[TestMechanismIDREF] [nvarchar](100) NOT NULL,
	[Information_Source] [nvarchar](250) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THEORETICALNOTE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THEORETICALNOTE](
	[TheoreticalNoteID] [int] IDENTITY(1,1) NOT NULL,
	[TheoreticalNoteText] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTION]    Script Date: 11/22/2013 11:59:50 AM ******/
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
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTIONCATEGORY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTIONCATEGORY](
	[ThreatActionCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActionCategoryName] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTIONFORINCIDENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTIONFORINCIDENT](
	[ThreatActionID] [int] NOT NULL,
	[ThreatActorID] [int] NOT NULL,
	[IncidentID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ThreatIntendedEffectID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTIONLOCATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTIONLOCATION](
	[ThreatActionLocationID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActionLocationName] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NOT NULL,
	[PhysicalLocationID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTIONLOCATIONFORTHREATACTIONCATEGORY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTIONLOCATIONFORTHREATACTIONCATEGORY](
	[ThreatActionCategoryID] [int] NOT NULL,
	[ThreatActionLocationID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTIONTARGET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTIONTARGET](
	[ThreatActionTargetID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActionCategoryID] [int] NOT NULL,
	[ThreatActionTargetName] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTIONVARIETY]    Script Date: 11/22/2013 11:59:50 AM ******/
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
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTIONVARIETYFORTHREATACTORTTP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTIONVARIETYFORTHREATACTORTTP](
	[ThreatActionVarietyID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTIONVECTOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTIONVECTOR](
	[ThreatActionVectorID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActionCategoryID] [int] NOT NULL,
	[ThreatActionVectorName] [nvarchar](250) NOT NULL,
	[VocabularyID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTOR](
	[ThreatActorID] [int] IDENTITY(1,1) NOT NULL,
	[ActorExternal] [bit] NULL,
	[ActorInternal] [bit] NULL,
	[role] [nvarchar](50) NULL,
	[country] [nvarchar](50) NULL,
	[notes] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORFORINCIDENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORFORINCIDENT](
	[ThreatActorID] [int] NOT NULL,
	[IncidentID] [int] NOT NULL,
	[ThreatMotiveID] [int] NOT NULL,
	[DateCreated] [datetimeoffset](7) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[ThreatActorRoleID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORFORTHREATCAMPAIGN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORFORTHREATCAMPAIGN](
	[ThreatActorID] [int] NOT NULL,
	[ThreatCampaignID] [int] NOT NULL,
	[ConfidenceLevel] [nvarchar](50) NULL,
	[notes] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORINFRASTRUCTURE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORINFRASTRUCTURE](
	[ThreatActorInfrastuctureID] [int] IDENTITY(1,1) NOT NULL,
	[AttackerInfrastructure] [nvarchar](150) NOT NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORINFRASTRUCTUREFORTHREATACTOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORINFRASTRUCTUREFORTHREATACTOR](
	[ThreatActorID] [int] NOT NULL,
	[ThreatActorInfrastructureID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[notes] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORINFRASTRUCTUREFORTHREATACTORTTP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORINFRASTRUCTUREFORTHREATACTORTTP](
	[ThreatActorInfrastructureID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[Information_Source] [nvarchar](250) NULL,
	[ConfidenceLevel] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORPAOS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORPAOS](
	[ThreatActorPAOSID] [int] IDENTITY(1,1) NOT NULL,
	[PlanningAndOperationalSupport] [nvarchar](200) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORROLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORROLE](
	[ThreatActorRoleID] [int] IDENTITY(1,1) NOT NULL,
	[role] [nvarchar](50) NOT NULL,
	[roleDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORSKILL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORSKILL](
	[ThreatActorSkillID] [int] IDENTITY(1,1) NOT NULL,
	[KnownledgeType] [nvarchar](max) NOT NULL,
	[KnownledgeLevel] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORSKILLFORATTACKPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORSKILLFORATTACKPATTERN](
	[ThreatActorSkillID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORTTP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORTTP](
	[ThreatActorTTPID] [int] IDENTITY(1,1) NOT NULL,
	[TTPTitle] [nvarchar](100) NOT NULL,
	[TTPDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[Information_Source] [nvarchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORTTPFORINCIDENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORTTPFORINCIDENT](
	[ThreatActorTTPID] [int] NOT NULL,
	[IncidentID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORTTPFORINDICATOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORTTPFORINDICATOR](
	[ThreatActorTTPID] [int] IDENTITY(1,1) NOT NULL,
	[IndicatorID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORTTPFORTHREATACTORTTP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORTTPFORTHREATACTORTTP](
	[ThreatActorTTPRefID] [int] NOT NULL,
	[ThreatActorTTPSubjectID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATACTORVARIETY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATACTORVARIETY](
	[ThreatActorVarietyID] [int] IDENTITY(1,1) NOT NULL,
	[ExternalVariety] [bit] NULL,
	[InternalVariety] [bit] NULL,
	[ActorVariety] [nvarchar](50) NOT NULL,
	[ActorVarietyDescription] [nvarchar](200) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATAGENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATAGENT](
	[ThreatAgentID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatAgentName] [nvarchar](100) NOT NULL,
	[ThreatAgentDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATAGENTFOROWASPTOP10]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATAGENTFOROWASPTOP10](
	[OWASPTOP10ID] [int] NOT NULL,
	[ThreatAgentID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGN](
	[ThreatCampaignID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ThreatCampaignTitle] [nvarchar](150) NULL,
	[ThreatCampaignStatus] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNFORTHREATCAMPAIGN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNFORTHREATCAMPAIGN](
	[ThreatCampaignRefID] [int] NOT NULL,
	[ThreatCampaignSubjectID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[ConfidenceLevel] [nvarchar](50) NULL,
	[notes] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNNAME](
	[ThreatCampaignNameID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatCampaignName] [nvarchar](150) NOT NULL,
	[internalname] [bit] NULL,
	[externalname] [bit] NULL,
	[Information_Source] [nvarchar](250) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[ValidFromDate] [datetimeoffset](7) NOT NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNNAMEFORTHREATCAMPAIGN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNNAMEFORTHREATCAMPAIGN](
	[ThreatCampaignID] [int] NOT NULL,
	[ThreatCampaignNameID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[ValidFromDate] [datetimeoffset](7) NOT NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNSTATUS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNSTATUS](
	[ThreatCampaignStatusID] [int] IDENTITY(1,1) NOT NULL,
	[CampaignStatus] [nvarchar](50) NOT NULL,
	[CampainStatusDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATCAMPAIGNTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATCAMPAIGNTYPE](
	[ThreatCampaignTypeID] [int] IDENTITY(1,1) NOT NULL,
	[CampaignTypeTitle] [nvarchar](100) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATINTENDEDEFFECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATINTENDEDEFFECT](
	[ThreatIntendedEffectID] [int] IDENTITY(1,1) NOT NULL,
	[IntendedEffectName] [nvarchar](200) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATINTENDEDEFFECTFORINCIDENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATINTENDEDEFFECTFORINCIDENT](
	[ThreatIntendedEffectID] [int] NOT NULL,
	[IncidentID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[ValidFromDate] [datetimeoffset](7) NOT NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATINTENDEDEFFECTFORTHREATACTORTTP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATINTENDEDEFFECTFORTHREATACTORTTP](
	[ThreatIntendedEffectID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[notes] [nvarchar](max) NULL,
	[ConfidenceLevel] [nvarchar](50) NULL,
	[Information_Source] [nvarchar](250) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NOT NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATINTENDEDEFFECTFORTHREATCAMPAIGN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATINTENDEDEFFECTFORTHREATCAMPAIGN](
	[ThreatIntendedEffectID] [int] NOT NULL,
	[ThreatCampaignID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[ValidFromDate] [datetimeoffset](7) NOT NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATMOTIVE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATMOTIVE](
	[ThreatMotiveID] [int] IDENTITY(1,1) NOT NULL,
	[motive] [nvarchar](50) NOT NULL,
	[motiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATMOTIVEFORTHREATACTOR]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATMOTIVEFORTHREATACTOR](
	[ThreatMotiveID] [int] NOT NULL,
	[ThreatActorID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREATTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREATTYPE](
	[ThreatTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatTypeName] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TICKET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TICKET](
	[TicketID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TIMEDIFFERENCEFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TIMEDIFFERENCEFUNCTION](
	[TimeDifferenceFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[DateTimeFormat1] [nvarchar](50) NOT NULL,
	[DateTimeFormat2] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TIMESHEET]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TIMESHEET](
	[TimesheetID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[TimesheetName] [nvarchar](250) NULL,
	[TimesheetDescription] [nvarchar](max) NULL,
	[TimeValue] [float] NULL,
	[TimeUnitID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ProjectID] [int] NULL,
	[TaskID] [int] NULL,
	[ProjectTaskID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TIMESHEETPERSON]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TIMESHEETPERSON](
	[TimesheetPersonID] [int] IDENTITY(1,1) NOT NULL,
	[TimesheetID] [int] NOT NULL,
	[PersonID] [int] NOT NULL,
	[PersonRole] [nvarchar](250) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[TimesheetPersonDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[StatusID] [int] NULL,
	[SignatureID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TIMEUNIT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TIMEUNIT](
	[TimeUnitID] [int] IDENTITY(1,1) NOT NULL,
	[TimeUnit] [nvarchar](10) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[TimeUnitDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOL](
	[ToolID] [int] IDENTITY(1,1) NOT NULL,
	[ToolGUID] [nvarchar](500) NULL,
	[ToolName] [nvarchar](150) NOT NULL,
	[ToolDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLINFORMATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLINFORMATION](
	[ToolInformationID] [int] IDENTITY(1,1) NOT NULL,
	[ToolInformationGUID] [nvarchar](500) NULL,
	[ToolInformationIDREF] [nvarchar](500) NULL,
	[ToolName] [nvarchar](150) NOT NULL,
	[ToolDescription] [nvarchar](max) NULL,
	[Vendor] [nvarchar](100) NULL,
	[Version] [nvarchar](50) NULL,
	[Service_Pack] [nvarchar](50) NULL,
	[Tool_Specific_Data] [nvarchar](max) NULL,
	[Tool_Hashes] [nvarchar](50) NULL,
	[Tool_Configuration] [nvarchar](max) NULL,
	[Execution_Environment] [nvarchar](max) NULL,
	[Errors] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLINFORMATIONFORTOOL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLINFORMATIONFORTOOL](
	[ToolInformationForToolID] [int] IDENTITY(1,1) NOT NULL,
	[ToolID] [int] NOT NULL,
	[ToolInformationID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLINFORMATIONMETADATA]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLINFORMATIONMETADATA](
	[ToolInformationID] [int] NOT NULL,
	[MetadataID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLINFORMATIONREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLINFORMATIONREFERENCE](
	[ToolInformationID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[ToolReferenceTypeID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLLICENSE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLLICENSE](
	[ToolLicenseID] [int] IDENTITY(1,1) NOT NULL,
	[ToolID] [int] NOT NULL,
	[LicenseID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLREFERENCE](
	[ToolReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[ToolID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[ToolReferenceTypeID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLREFERENCETYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLREFERENCETYPE](
	[ToolReferenceTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ToolReferenceTypeName] [nvarchar](50) NOT NULL,
	[ToolReferenceTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLTYPE](
	[ToolTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ToolTypeGUID] [nvarchar](500) NULL,
	[ToolTypeName] [nvarchar](150) NOT NULL,
	[ToolTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLTYPEFORTOOLINFORMATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLTYPEFORTOOLINFORMATION](
	[ToolInformationID] [int] NOT NULL,
	[ToolTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TRAINING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TRAINING](
	[TrainingID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TRAININGFORPERSON]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TRAININGFORPERSON](
	[PersonID] [int] NOT NULL,
	[TrainingID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TRANSACTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TRANSACTION](
	[TransactionID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [uniqueidentifier] NULL,
	[Date] [datetime] NULL,
	[Amount] [money] NULL,
	[Status] [nvarchar](50) NULL,
	[OrderNumber] [nvarchar](50) NULL,
	[Email] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[ProductID] [nvarchar](50) NULL,
	[ProductDescription] [nvarchar](max) NULL,
	[HolderName] [nvarchar](max) NULL,
	[PaymentMethod] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TREND]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TREND](
	[TrendID] [int] IDENTITY(1,1) NOT NULL,
	[TrendName] [nvarchar](50) NOT NULL,
	[TrendDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TRUSTLEVEL]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TRUSTLEVEL](
	[TrustLevelID] [int] IDENTITY(1,1) NOT NULL,
	[TrustLevelName] [nvarchar](50) NOT NULL,
	[TrustLevelDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UNIQUEFUNCTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UNIQUEFUNCTION](
	[UniqueFunctionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UNIXFILE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UNIXFILE](
	[UnixFileID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UNIXNETWORKROUTEENTRY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UNIXNETWORKROUTEENTRY](
	[UnixNetworkRouteEntryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UNIXPIPEOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UNIXPIPEOBJECT](
	[UnixPipeObjectID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UNIXPROCESS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UNIXPROCESS](
	[UnixProcessID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UNIXUSERACCOUNT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UNIXUSERACCOUNT](
	[UnixUserAccountID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UNIXVOLUME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UNIXVOLUME](
	[UnixVolumeID] [int] IDENTITY(1,1) NOT NULL,
	[VolumeObjectID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[URIOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[URIOBJECT](
	[URIObjectID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USECASE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USECASE](
	[UseCaseID] [int] IDENTITY(1,1) NOT NULL,
	[UseCaseDescription] [nvarchar](max) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USECASEFORBUSINESSRISK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USECASEFORBUSINESSRISK](
	[UseCaseID] [int] NOT NULL,
	[BusinessRiskID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USECASEFORREGULATORYRISK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USECASEFORREGULATORYRISK](
	[UseCaseID] [int] NOT NULL,
	[RegulatoryRiskID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USERACCOUNT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USERACCOUNT](
	[UserAccountID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NULL,
	[UserID] [uniqueidentifier] NULL,
	[UserAccountACL] [int] NULL,
	[UserAccountTypeID] [int] NULL,
	[UserAccountType] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USERACCOUNTTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USERACCOUNTTYPE](
	[UserAccountTypeID] [int] IDENTITY(1,1) NOT NULL,
	[UserAccountTypeName] [nvarchar](100) NULL,
	[UserAccountTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USERACTIONNAME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USERACTIONNAME](
	[UserActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[UserActionNameName] [nvarchar](150) NOT NULL,
	[UserActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USERSESSION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USERSESSION](
	[UserSessionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VALUE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VALUE](
	[ValueID] [int] IDENTITY(1,1) NOT NULL,
	[ValueValue] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VERSION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VERSION](
	[VersionID] [int] IDENTITY(1,1) NOT NULL,
	[VersionValue] [nvarchar](50) NULL,
	[VersionDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VOCABULARY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VOCABULARY](
	[VocabularyID] [int] IDENTITY(1,1) NOT NULL,
	[VocabularyName] [nvarchar](50) NOT NULL,
	[VocabularyVersion] [nvarchar](10) NULL,
	[VocabularyReference] [nvarchar](250) NULL,
	[DateModified] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VOCABULARYREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VOCABULARYREFERENCE](
	[VocabularyID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[VocabularyReferenceDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VOLUMEOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VOLUMEOBJECT](
	[VolumeObjectID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VULNERABILITY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VULNERABILITY](
	[VulnerabilityID] [int] IDENTITY(1,1) NOT NULL,
	[VULGUID] [nvarchar](500) NULL,
	[VULReferential] [nvarchar](50) NULL,
	[VULReferentialID] [nvarchar](500) NULL,
	[VULDescription] [nvarchar](max) NULL,
	[VULConsequence] [nvarchar](max) NULL,
	[VULRemediationAvailable] [bit] NULL,
	[VULSolution] [nvarchar](max) NULL,
	[VULPublishedDate] [datetimeoffset](7) NULL,
	[VULModifiedDate] [datetimeoffset](7) NULL,
	[CVSSBaseScore] [float] NULL,
	[CVSSImpactSubscore] [float] NULL,
	[CVSSExploitabilitySubscore] [float] NULL,
	[CVSSMetricAccessVector] [nvarchar](50) NULL,
	[CVSSMetricAccessComplexity] [nvarchar](50) NULL,
	[CVSSMetricAuthentication] [nvarchar](50) NULL,
	[CVSSMetricConfImpact] [nvarchar](50) NULL,
	[CVSSMetricIntegImpact] [nvarchar](50) NULL,
	[CVSSMetricAvailImpact] [nvarchar](50) NULL,
	[VULPatchAvailable] [bit] NULL,
	[VULExploitable] [bit] NULL,
	[VULType] [nvarchar](50) NULL,
	[VULName] [nvarchar](max) NULL,
	[VULURL] [nvarchar](500) NULL,
	[VULRequest] [nvarchar](max) NULL,
	[VULResponse] [nvarchar](max) NULL,
	[VULDetailedInformation] [nvarchar](max) NULL,
	[VULMD5] [nchar](32) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[SeverityLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[VULCreatedDate] [datetimeoffset](7) NULL,
	[RepositoryID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VULNERABILITYCOMPLIANCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VULNERABILITYCOMPLIANCE](
	[VulnerabilityComplianceID] [int] IDENTITY(1,1) NOT NULL,
	[ComplianceID] [int] NOT NULL,
	[VulnerabilityID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[VulnerabilityComplianceDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VULNERABILITYFORATTACKEXAMPLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VULNERABILITYFORATTACKEXAMPLE](
	[VulnerabilityID] [int] NOT NULL,
	[AttackExampleID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VULNERABILITYFORATTACKPATTERN]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VULNERABILITYFORATTACKPATTERN](
	[VulnerabilityID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL,
	[CVEID] [nvarchar](20) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VULNERABILITYFORCPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VULNERABILITYFORCPE](
	[VulnerabilityID] [int] NOT NULL,
	[CPEID] [nvarchar](255) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VULNERABILITYFORCWE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VULNERABILITYFORCWE](
	[VulnerabilityID] [int] NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VULNERABILITYFORREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VULNERABILITYFORREFERENCE](
	[ReferenceID] [int] NOT NULL,
	[VulnerabilityID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VULNERABILITYFORTHREATACTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VULNERABILITYFORTHREATACTION](
	[VulnerabilityID] [int] NOT NULL,
	[ThreatActionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VULNERABILITYFORTHREATACTORTTP]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VULNERABILITYFORTHREATACTORTTP](
	[VulnerabilityID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VULNERABILITYPATCH]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VULNERABILITYPATCH](
	[VulnerabilityID] [int] NOT NULL,
	[PatchID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntildate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VULNERABILITYRECOMMENDATION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VULNERABILITYRECOMMENDATION](
	[VulnerabilityRecommendationID] [int] IDENTITY(1,1) NOT NULL,
	[VulnerabilityID] [int] NOT NULL,
	[RecommendationID] [int] NOT NULL,
	[VulnerabilityRecommendationName] [nvarchar](500) NULL,
	[VulnerabilityRecommendationDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[PatchID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VULNERABILITYSCORE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VULNERABILITYSCORE](
	[VulnScoreID] [int] IDENTITY(1,1) NOT NULL,
	[VulnerabilityID] [int] NOT NULL,
	[ScoringSystemID] [int] NOT NULL,
	[ScoringFormulaID] [int] NULL,
	[VULVariable] [nvarchar](100) NULL,
	[VULScore] [float] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WAIVER]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WAIVER](
	[WaiverID] [int] IDENTITY(1,1) NOT NULL,
	[WaiverName] [nvarchar](250) NULL,
	[WaiverDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[OrganisationID] [int] NULL,
	[PersonID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WARNING]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WARNING](
	[WarningID] [int] IDENTITY(1,1) NOT NULL,
	[WarningText] [nvarchar](max) NOT NULL,
	[lang] [nvarchar](10) NULL,
	[WarningCategoryID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WARNINGCATEGORY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WARNINGCATEGORY](
	[WarningCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[WarningCategoryName] [nvarchar](50) NOT NULL,
	[WarningCategoryMeaning] [nvarchar](500) NULL,
	[VocabularyID] [int] NULL,
	[lang] [nvarchar](10) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WASC]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WASC](
	[WASCID] [int] IDENTITY(1,1) NOT NULL,
	[WASCThreatType] [nvarchar](50) NOT NULL,
	[WASCRefID] [nvarchar](20) NOT NULL,
	[WASCName] [nvarchar](250) NULL,
	[WASCDescription] [nvarchar](max) NULL,
	[WASCExample] [nvarchar](max) NULL,
	[WASCRefURL] [nvarchar](250) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WASCCWE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WASCCWE](
	[WASCCWEID] [int] IDENTITY(1,1) NOT NULL,
	[WASCID] [int] NOT NULL,
	[WASCRefID] [nvarchar](20) NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WASCFORCAPEC]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WASCFORCAPEC](
	[WASCForCAPECID] [int] IDENTITY(1,1) NOT NULL,
	[capec_id] [nvarchar](20) NOT NULL,
	[WASCID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WASCREFERENCE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WASCREFERENCE](
	[WASCReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[WASCID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WASCTHREATTYPE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WASCTHREATTYPE](
	[ThreatTypeID] [int] NOT NULL,
	[WASCID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WHOISOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WHOISOBJECT](
	[WhoisObjectID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSCOMPUTERACCOUNT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSCOMPUTERACCOUNT](
	[WindowsComputerAccountID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSCRITICALSECTION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSCRITICALSECTION](
	[WindowsCriticalSectionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSDRIVER]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSDRIVER](
	[WindowsDriverID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSEVENT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSEVENT](
	[WindowsEventID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSEVENTLOG]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSEVENTLOG](
	[WindowsEventLogID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSEXECUTABLEFILE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSEXECUTABLEFILE](
	[WindowsExecutableFileID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSFILE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSFILE](
	[WindowsFileID] [int] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSHANDLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSHANDLE](
	[WindowsHandleID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSKERNELHOOK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSKERNELHOOK](
	[WindowsKernelHookID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSKERNELOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSKERNELOBJECT](
	[WindowsKernelObjectID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSMAILSLOT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSMAILSLOT](
	[WindowsMailslotID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSMEMORYPAGEREGION]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSMEMORYPAGEREGION](
	[WindowsMemoryPageRegionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSMUTEX]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSMUTEX](
	[WindowsMutexID] [int] IDENTITY(1,1) NOT NULL,
	[MutexID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSMUTEXHANDLE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSMUTEXHANDLE](
	[WindowsMutexHandleID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsMutexID] [int] NOT NULL,
	[WindowsHandleID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSMUTEXSECURITYATTRIBUTE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSMUTEXSECURITYATTRIBUTE](
	[WindowsMutexSecurityAttributeID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsMutexID] [int] NOT NULL,
	[SecurityAttributeID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSNETWORKROUTEENTRY]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSNETWORKROUTEENTRY](
	[WindowsNetworkRouteEntryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSNETWORKSHARE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSNETWORKSHARE](
	[WindowsNetworkShareID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSPIPEOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSPIPEOBJECT](
	[WindowsPipeObjectID] [int] IDENTITY(1,1) NOT NULL,
	[PipeObjectID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSPREFETCHOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSPREFETCHOBJECT](
	[WindowsPrefetchObjectID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSPROCESS]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSPROCESS](
	[WindowsProcessID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSREGISTRYKEYOBJECT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSREGISTRYKEYOBJECT](
	[WindowsRegistryKeyObjectID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSSEMAPHORE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSSEMAPHORE](
	[WindowsSemaphoreID] [int] IDENTITY(1,1) NOT NULL,
	[SemaphoreID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSSERVICE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSSERVICE](
	[WindowsServiceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSSYSTEM]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSSYSTEM](
	[WindowsSystemID] [int] IDENTITY(1,1) NOT NULL,
	[SystemID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSSYSTEMRESTORE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSSYSTEMRESTORE](
	[WindowsSystemRestoreID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSTASK]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSTASK](
	[WindowsTaskID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [int] NULL,
	[SessionCronID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSTHREAD]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSTHREAD](
	[WindowsThreadID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSUSERACCOUNT]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSUSERACCOUNT](
	[WindowsUserAccountID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NULL,
	[WindowsComputerAccountID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSVOLUME]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSVOLUME](
	[WindowsVolumeID] [int] IDENTITY(1,1) NOT NULL,
	[VolumeObjectID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSWAITABLETIMER]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSWAITABLETIMER](
	[WindowsWaitableTimerID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[X509CERTIFICATE]    Script Date: 11/22/2013 11:59:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[X509CERTIFICATE](
	[X509CertificateID] [int] IDENTITY(1,1) NOT NULL,
	[CertificateID] [int] NULL
) ON [PRIMARY]

GO


