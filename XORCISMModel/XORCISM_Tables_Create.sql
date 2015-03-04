/****** 
Copyright (C) 2012-2015 Jerome Athias
XORCISM database
This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
******/

USE [XORCISM]
GO

/****** Object:  Table [dbo].[ACCESSEDDIRECTORYLIST]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACCESSEDDIRECTORYLIST](
	[AccessedDirectoryListID] [int] IDENTITY(1,1) NOT NULL,
	[AccessedDirectoryListGUID] [nvarchar](500) NULL,
	[AccessedDirectoryListName] [nvarchar](250) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[suspected_malicious] [bit] NULL,
	[SuspectedMaliciousReasonID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACCESSEDFILELIST]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACCESSEDFILELIST](
	[AccessedFileListID] [int] IDENTITY(1,1) NOT NULL,
	[AccessedFileListGUID] [nvarchar](500) NULL,
	[AccessedFileListName] [nvarchar](250) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[suspected_malicious] [bit] NULL,
	[SuspectedMaliciousReasonID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACCESSEDFILELISTFILES]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACCESSEDFILELISTFILES](
	[AccessedFileListFileID] [int] IDENTITY(1,1) NOT NULL,
	[AccessedFileListFileGUID] [nvarchar](500) NULL,
	[AccessedFileListID] [int] NOT NULL,
	[AccessedFileListGUID] [nvarchar](500) NULL,
	[FileID] [int] NOT NULL,
	[FileGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACCESSRECORD]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACCESSRECORD](
	[AccessRecordID] [int] IDENTITY(1,1) NOT NULL,
	[AccessRecordGUID] [nvarchar](500) NULL,
	[RecordGUID] [nvarchar](500) NULL,
	[UserID] [int] NULL,
	[UserGUID] [nvarchar](500) NULL,
	[AccessType] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CollectionMethodID] [int] NULL,
	[suspected_malicious] [bit] NULL,
	[SuspectedMaliciousReasonID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACCESSRECORDEVIDENCE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACCESSRECORDEVIDENCE](
	[AccessRecordEvidenceID] [int] IDENTITY(1,1) NOT NULL,
	[AccessRecordID] [int] NULL,
	[AccessRecordGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACCESSRECORDHASH]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACCESSRECORDHASH](
	[AccessRecordHashID] [int] IDENTITY(1,1) NOT NULL,
	[AccessRecordHashGUID] [nvarchar](500) NULL,
	[AccessRecordID] [int] NOT NULL,
	[HashValue] [nvarchar](max) NOT NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACCOUNT]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACCOUNT](
	[AccountID] [int] IDENTITY(1,1) NOT NULL,
	[AccountGUID] [nvarchar](500) NULL,
	[AccountName] [nvarchar](50) NULL,
	[AccountDomain] [nvarchar](500) NULL,
	[DomainNameID] [int] NULL,
	[DomainNameGUID] [nvarchar](500) NULL,
	[AccountDescription] [nvarchar](max) NULL,
	[Creation_Date] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[Modified_Date] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[Last_Accessed_Time] [datetimeoffset](7) NULL,
	[disabled] [bit] NULL,
	[locked_out] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[OrganisationID] [int] NULL,
	[OrganisationGUID] [nvarchar](500) NULL,
	[PersonID] [int] NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[AssetID] [int] NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[isEncrypted] [bit] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACCOUNTAUTHENTICATION]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACCOUNTAUTHENTICATION](
	[AccountAuthenticationID] [int] IDENTITY(1,1) NOT NULL,
	[AccountAuthenticationGUID] [nvarchar](500) NULL,
	[AccountID] [int] NOT NULL,
	[AccountGUID] [nvarchar](500) NULL,
	[AuthenticationTypeID] [int] NOT NULL,
	[AuthenticationTypeGUID] [nvarchar](500) NULL,
	[Authentication_Data] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[Authentication_Token_Protection_Mechanism] [nvarchar](500) NULL,
	[AuthenticationTokenProtectionMechanismID] [int] NULL,
	[AuthenticationTokenProtectionMechanismGUID] [nvarchar](500) NULL,
	[StructuredAuthenticationMechanismID] [int] NULL,
	[StructuredAuthenticationMechanismGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACCOUNTAUTHENTICATIONTYPE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACCOUNTAUTHENTICATIONTYPE](
	[AccountAuthenticationTypeID] [int] IDENTITY(1,1) NOT NULL,
	[AccountAuthenticationTypeGUID] [nvarchar](500) NULL,
	[AccountID] [int] NOT NULL,
	[AccountGUID] [nvarchar](500) NULL,
	[AuthenticationTypeID] [int] NOT NULL,
	[AuthenticationTypeGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACCOUNTBLACKLIST]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACCOUNTBLACKLIST](
	[AccountBlacklistID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACCOUNTCHANGERECORD]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACCOUNTCHANGERECORD](
	[AccountChangeRecordID] [int] IDENTITY(1,1) NOT NULL,
	[AccountChangeRecordGUID] [nvarchar](500) NULL,
	[AccountID] [int] NOT NULL,
	[AccountGUID] [nvarchar](500) NULL,
	[ChangeRecordID] [int] NOT NULL,
	[ChangeRecordGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACCOUNTDESCRIPTION]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACCOUNTDESCRIPTION](
	[AccountDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACCOUNTWHITELIST]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACCOUNTWHITELIST](
	[AccountWhitelistID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACE](
	[ACEID] [int] IDENTITY(1,1) NOT NULL,
	[ACEGUID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACL]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACL](
	[ACLID] [int] IDENTITY(1,1) NOT NULL,
	[ACLGUID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACLENTRY]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACLENTRY](
	[ACLEntryID] [int] IDENTITY(1,1) NOT NULL,
	[ACLEntryGUID] [nvarchar](500) NULL,
	[ACLID] [int] NULL,
	[ACLGUID] [nvarchar](500) NULL,
	[ACEID] [int] NULL,
	[ACEGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[CreationObjectGUID] [nvarchar](500) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACRONYM]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACRONYM](
	[AcronymID] [int] IDENTITY(1,1) NOT NULL,
	[AcronymGUID] [nvarchar](500) NULL,
	[AcronymAbbreviation] [nvarchar](20) NOT NULL,
	[AcronymPhrase] [nvarchar](500) NOT NULL,
	[AcronymDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTION]    Script Date: 04/03/2015 19:59:08 ******/
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
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ActionDescription] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[isSuspicious] [bit] NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONACTION]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONACTION](
	[ActionRelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[ActionRefID] [int] NOT NULL,
	[ActionRefGUID] [nvarchar](500) NULL,
	[ActionRelationshipTypeID] [int] NOT NULL,
	[ActionRelationshipTypeName] [nvarchar](150) NOT NULL,
	[ActionSubjectID] [int] NOT NULL,
	[ActionSubjectGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONACTIONARGUMENTNAME]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONACTIONARGUMENTNAME](
	[ActionActionArgumentNameID] [int] IDENTITY(1,1) NOT NULL,
	[ActionID] [int] NOT NULL,
	[ActionGUID] [nvarchar](500) NULL,
	[ActionArgumentNameID] [int] NOT NULL,
	[ActionArgumentNameGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONACTIONNAME]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONACTIONNAME](
	[ActionActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[ActionID] [int] NOT NULL,
	[ActionGUID] [nvarchar](500) NULL,
	[ActionNameID] [int] NOT NULL,
	[ActionNameGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONACTIONTYPE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONACTIONTYPE](
	[ActionActionTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ActionID] [int] NOT NULL,
	[ActionGUID] [nvarchar](500) NULL,
	[ActionTypeID] [int] NOT NULL,
	[ActionTypeGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONALIAS]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONALIAS](
	[ActionAliasID] [int] IDENTITY(1,1) NOT NULL,
	[ActionID] [int] NOT NULL,
	[ActionGUID] [nvarchar](500) NULL,
	[ActionAlias] [nvarchar](250) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONARGUMENTNAME]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONARGUMENTNAME](
	[ActionArgumentNameID] [int] IDENTITY(1,1) NOT NULL,
	[ActionArgumentNameGUID] [nvarchar](500) NULL,
	[ActionArgumentNameName] [nvarchar](150) NOT NULL,
	[ActionArgumentNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONASSOCIATION]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONASSOCIATION](
	[ActionAssociationID] [int] IDENTITY(1,1) NOT NULL,
	[ActionObjectAssociationType] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONCOLLECTION]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONCOLLECTION](
	[ActionCollectionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONCONTEXT]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONCONTEXT](
	[ActionContextID] [int] IDENTITY(1,1) NOT NULL,
	[ActionContextGUID] [nvarchar](500) NULL,
	[ActionContextName] [nvarchar](50) NOT NULL,
	[ActionContextDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONDESCRIPTION]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONDESCRIPTION](
	[ActionDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[ActionID] [int] NOT NULL,
	[ActionGUID] [nvarchar](500) NULL,
	[DescriptionID] [int] NOT NULL,
	[DescriptionGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONDISCOVERYMETHOD]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONDISCOVERYMETHOD](
	[ActionDiscoveryMethodID] [int] IDENTITY(1,1) NOT NULL,
	[ActionID] [int] NOT NULL,
	[ActionGUID] [nvarchar](500) NULL,
	[DiscoveryMethodID] [int] NOT NULL,
	[DiscoveryMethodGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONIMPLEMENTATION]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONIMPLEMENTATION](
	[ActionImplementationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONNAME]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONNAME](
	[ActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[ActionNameName] [nvarchar](100) NOT NULL,
	[ActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONOBJECTASSOCIATIONTYPE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONOBJECTASSOCIATIONTYPE](
	[ActionObjectAssociationTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ActionObjectAssociationTypeName] [nvarchar](150) NOT NULL,
	[ActionObjectAssociationTypeDescription] [nvarchar](max) NULL,
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

/****** Object:  Table [dbo].[ACTIONPLAN]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONPLAN](
	[ActionPlanID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONPOOL]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONPOOL](
	[ActionPoolID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONRELATIONSHIPTYPE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONRELATIONSHIPTYPE](
	[ActionRelationshipTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ActionRelationshipTypeName] [nvarchar](150) NOT NULL,
	[ActionRelationshipTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONSTATUS]    Script Date: 04/03/2015 19:59:08 ******/
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

/****** Object:  Table [dbo].[ACTIONTAKEN]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONTAKEN](
	[ActionTakenID] [int] IDENTITY(1,1) NOT NULL,
	[ActionTakenGUID] [nvarchar](500) NULL,
	[ActionName] [nvarchar](100) NOT NULL,
	[ActionDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONTAKENFORINCIDENT]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONTAKENFORINCIDENT](
	[ActionTakenForIncidentID] [int] IDENTITY(1,1) NOT NULL,
	[ActionTakenID] [int] NOT NULL,
	[IncidentID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[CreationObjectID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONTAKENFORTHREATCAMPAIGN]    Script Date: 04/03/2015 19:59:08 ******/
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
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[CreationObjectID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIONTYPE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIONTYPE](
	[ActionTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ActionTypeGUID] [nvarchar](500) NULL,
	[ActionTypeName] [nvarchar](150) NOT NULL,
	[ActionTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIVATIONFUNCTION]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIVATIONFUNCTION](
	[ActivationFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[FunctionID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIVATIONZONE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIVATIONZONE](
	[ActivationZoneID] [int] IDENTITY(1,1) NOT NULL,
	[ActivationZoneGUID] [nvarchar](500) NULL,
	[ActivationZoneText] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ACTIVATIONZONEFORATTACKPATTERN]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACTIVATIONZONEFORATTACKPATTERN](
	[AttackPatternActivationZoneID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternActivationZoneGUID] [nvarchar](500) NULL,
	[ActivationZoneID] [int] NOT NULL,
	[ActivationZoneGUID] [nvarchar](500) NULL,
	[AttackPatternID] [int] NOT NULL,
	[AttackPatternGUID] [nvarchar](500) NULL,
	[capec_id] [nvarchar](20) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ADDRESS]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ADDRESS](
	[AddressID] [int] IDENTITY(1,1) NOT NULL,
	[AddressGUID] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[AddressCategoryID] [int] NULL,
	[category] [nvarchar](150) NULL,
	[Address_Value] [nvarchar](500) NULL,
	[VLAN_Name] [nvarchar](500) NULL,
	[VLAN_Num] [int] NULL,
	[is_source] [bit] NULL,
	[is_destination] [bit] NULL,
	[is_spoofed] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ADDRESSBLACKLIST]    Script Date: 04/03/2015 19:59:08 ******/
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
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ADDRESSCATEGORY]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ADDRESSCATEGORY](
	[AddressCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[AddressCategoryGUID] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[AddressCategoryName] [nvarchar](50) NULL,
	[AddressCategoryDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ADDRESSCOUNTRY]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ADDRESSCOUNTRY](
	[AddressCountryID] [int] IDENTITY(1,1) NOT NULL,
	[AddressID] [int] NULL,
	[AddressGUID] [nvarchar](500) NULL,
	[CountryID] [int] NULL,
	[CountryGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ADDRESSREPUTATION]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ADDRESSREPUTATION](
	[AddressReputationID] [int] IDENTITY(1,1) NOT NULL,
	[AddressID] [int] NULL,
	[AddressGUID] [nvarchar](500) NULL,
	[ReputationID] [int] NULL,
	[ReputationGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ADDRESSWHITELIST]    Script Date: 04/03/2015 19:59:08 ******/
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
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ADVISORY]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ADVISORY](
	[AdvisoryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AFFECTEDRESOURCE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AFFECTEDRESOURCE](
	[AffectedResourceID] [int] IDENTITY(1,1) NOT NULL,
	[AffectedResourceName] [nvarchar](100) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AGENT]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AGENT](
	[AgentID] [int] IDENTITY(1,1) NOT NULL,
	[AgentGUID] [nvarchar](500) NULL,
	[ipaddressIPv4] [nvarchar](50) NULL,
	[AgentStatus] [nvarchar](50) NULL,
	[AgentLoadValue] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[SensorID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ALGEBRAIC]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ALGEBRAIC](
	[AlgebraicID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ALGORITHM]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ALGORITHM](
	[AlgorithmID] [int] IDENTITY(1,1) NOT NULL,
	[AlgorithmName] [nvarchar](50) NULL,
	[AlgorithmVersion] [nvarchar](50) NULL,
	[AlgorithmVersionID] [int] NULL,
	[AlgorithmDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ALGORITHMDESCRIPTION]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ALGORITHMDESCRIPTION](
	[AlgorithmDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ALGORITHMREFERENCE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ALGORITHMREFERENCE](
	[AlgorithmReferenceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ALGORITHMTAG]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ALGORITHMTAG](
	[AlgorithmTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ANTIBEHAVIORALANALYSISSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ANTIBEHAVIORALANALYSISSTRATEGICOBJECTIVE](
	[AntiBehavioralAnalysisStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[AntiBehavioralAnalysisStrategicObjectiveName] [nvarchar](50) NULL,
	[AntiBehavioralAnalysisStrategicObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ANTIBEHAVIORALANALYSISTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ANTIBEHAVIORALANALYSISTACTICALOBJECTIVE](
	[AntiBehavioralAnalysisTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[AntiBehavioralAnalysisTacticalObjectiveName] [nvarchar](50) NULL,
	[AntiBehavioralAnalysisTacticalObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ANTIBEHAVIORANALYSISPROPERTIES]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ANTIBEHAVIORANALYSISPROPERTIES](
	[AntiBehavioralAnalysisPropertiesID] [int] IDENTITY(1,1) NOT NULL,
	[AntiBehavioralAnalysisPropertiesName] [nvarchar](50) NULL,
	[AntiBehavioralAnalysisPropertiesDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ANTICODEANALYSISSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ANTICODEANALYSISSTRATEGICOBJECTIVE](
	[AntiCodeAnalysisStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[AntiCodeAnalysisStrategicObjectiveName] [nvarchar](50) NULL,
	[AntiCodeAnalysisStrategicObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ANTICODEANALYSISTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ANTICODEANALYSISTACTICALOBJECTIVE](
	[AntiCodeAnalysisTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[AntiCodeAnalysisTacticalObjectiveName] [nvarchar](100) NULL,
	[AntiCodeAnalysisTacticalObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ANTIDETECTIONSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ANTIDETECTIONSTRATEGICOBJECTIVE](
	[AntiDetectionStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[AntiDetectionStrategicObjectiveName] [nvarchar](50) NULL,
	[AntiDetectionStrategicObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ANTIDETECTIONTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ANTIDETECTIONTACTICALOBJECTIVE](
	[AntiDetectionTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[AntiDetectionTacticalObjectiveName] [nvarchar](100) NULL,
	[AntiDetectionTacticalObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ANTIREMOVALSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ANTIREMOVALSTRATEGICOBJECTIVE](
	[AntiRemovalStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[AntiRemovalStrategicObjectiveName] [nvarchar](50) NULL,
	[AntiRemovalStrategicObjectiveDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ANTIREMOVALTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ANTIREMOVALTACTICALOBJECTIVE](
	[AntiRemovalTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[AntiRemovalTacticalObjectiveName] [nvarchar](50) NULL,
	[AntiRemovalTacticalObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[API]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[API](
	[APIID] [int] IDENTITY(1,1) NOT NULL,
	[APIGUID] [nvarchar](500) NULL,
	[APIName] [nvarchar](250) NULL,
	[APIDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APICALL]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APICALL](
	[APICallID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APIFUNCTION]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APIFUNCTION](
	[APIFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[APIFunctionGUID] [nvarchar](500) NULL,
	[APIID] [int] NOT NULL,
	[FunctionID] [int] NOT NULL,
	[Function_Name] [nvarchar](100) NULL,
	[Normalized_Function_Name] [nvarchar](100) NULL,
	[Address] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[APIFunctionDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APIMEMORYADDRESS]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APIMEMORYADDRESS](
	[APIMemoryAddressID] [int] IDENTITY(1,1) NOT NULL,
	[APIID] [int] NOT NULL,
	[MemoryAddressID] [int] NOT NULL,
	[FunctionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APIPLATFORM]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APIPLATFORM](
	[APIPlatformID] [int] IDENTITY(1,1) NOT NULL,
	[APIID] [int] NOT NULL,
	[PlatformID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATION]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATION](
	[ApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[ApplicationName] [nvarchar](250) NOT NULL,
	[ApplicationDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONAUTHENTICATIONTYPE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONAUTHENTICATIONTYPE](
	[ApplicationAuthenticationTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[AuthenticationTypeID] [int] NULL,
	[AuthenticationTypeGUID] [nvarchar](500) NULL,
	[AuthenticationRank] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ApplicationAuthenticationTypeDescription] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONBLACKLIST]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONBLACKLIST](
	[ApplicationBlacklistID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONCATEGORIES]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONCATEGORIES](
	[ApplicationCategoriesID] [int] NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[ApplicationCategoryID] [int] NOT NULL,
	[ApplicationCategoryGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[RepositoryID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONCATEGORY]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONCATEGORY](
	[ApplicationCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationCategoryGUID] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[ApplicationCategoryName] [nvarchar](50) NULL,
	[ApplicationCategoryDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONCRITICALITY]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONCRITICALITY](
	[ApplicationCriticalityID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationCriticalityDescription] [nvarchar](max) NULL,
	[ApplicationID] [int] NOT NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[ApplicationCriticalityLevelID] [int] NOT NULL,
	[ApplicationCriticalityLevelGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONCRITICALITYLEVEL]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONCRITICALITYLEVEL](
	[ApplicationCriticalityLevelID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONDEPENDENCY]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONDEPENDENCY](
	[ApplicationDependencyID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationParentID] [int] NULL,
	[ApplicationParentGUID] [nvarchar](500) NULL,
	[ApplicationSubjectID] [int] NULL,
	[ApplicationSubjectGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONDOCUMENT]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONDOCUMENT](
	[ApplicationDocumentID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[DocumentID] [int] NULL,
	[DocumentGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONFILEEXTENSIONBLACKLIST]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONFILEEXTENSIONBLACKLIST](
	[ApplicationFileExtensionBlacklistID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONFILEEXTENSIONWHITELIST]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONFILEEXTENSIONWHITELIST](
	[ApplicationFileExtensionWhitelistID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONFILELIST]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONFILELIST](
	[ApplicationFileListID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[ApplicationFileListRelationship] [nvarchar](50) NULL,
	[ApplicationFileListDescription] [nvarchar](max) NULL,
	[FileListID] [int] NULL,
	[FileListGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONFORASSET]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONFORASSET](
	[AssetApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[ApplicationID] [int] NOT NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONFORORGANISATION]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONFORORGANISATION](
	[OrganisationApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationApplicationGUID] [nvarchar](500) NULL,
	[OrganisationID] [int] NOT NULL,
	[OrganisationGUID] [nvarchar](500) NULL,
	[ApplicationID] [int] NOT NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONFUNCTION]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONFUNCTION](
	[ApplicationFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[FunctionID] [int] NOT NULL,
	[FunctionGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONMIMEWHITELIST]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONMIMEWHITELIST](
	[ApplicationMIMEWhitelistID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[MIMEWhitelistID] [int] NOT NULL,
	[MIMEWhitelistGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONNETWORKZONE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONNETWORKZONE](
	[NetworkZoneApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[NetworkZoneID] [int] NULL,
	[NetworkZoneGUID] [nvarchar](500) NULL,
	[ApplicationID] [int] NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONPERSON]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONPERSON](
	[ApplicationPersonID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[PersonID] [int] NOT NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[Usage] [nvarchar](100) NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONPORTWHITELIST]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONPORTWHITELIST](
	[ApplicationPortWhitelistID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[PortID] [int] NULL,
	[inboundaccepted] [bit] NULL,
	[outboundaccepted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONSECURITYLABEL]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONSECURITYLABEL](
	[ApplicationSecurityLabelID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[SecurityLabelID] [int] NOT NULL,
	[SecurityLabelGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONURI]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONURI](
	[ApplicationURIID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[URIObjectID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONURIWHITELIST]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONURIWHITELIST](
	[ApplicationURIWhitelistID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationURIID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[ValidityID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONVERSION]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONVERSION](
	[ApplicationVersionID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[VersionID] [int] NULL,
	[ApplicationVersionDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPLICATIONWHITELIST]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPLICATIONWHITELIST](
	[ApplicationWhitelistID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPROBATION]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPROBATION](
	[ApprobationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[APPROVAL]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APPROVAL](
	[ApprovalID] [int] IDENTITY(1,1) NOT NULL,
	[ApprobationID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARCHITECTURALPARADIGM]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARCHITECTURALPARADIGM](
	[ArchitecturalParadigmID] [int] IDENTITY(1,1) NOT NULL,
	[ArchitecturalParadigmGUID] [nvarchar](500) NULL,
	[ArchitecturalParadigmName] [nvarchar](150) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARCHITECTURALPARADIGMFORTECHNICALCONTEXT]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARCHITECTURALPARADIGMFORTECHNICALCONTEXT](
	[TechnicalContextArchitecturalParadigmID] [int] IDENTITY(1,1) NOT NULL,
	[ArchitecturalParadigmID] [int] NOT NULL,
	[TechnicalContextID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARCHIVEFILE]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARCHIVEFILE](
	[ArchiveFileID] [int] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NULL,
	[ArchiveFileDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[RepositoryID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFASSET]    Script Date: 04/03/2015 19:59:08 ******/
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

/****** Object:  Table [dbo].[ARFASSETFORASSETS]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFASSETFORASSETS](
	[AssetsID] [int] NOT NULL,
	[ARFAssetID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFEXTENDEDINFO]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFEXTENDEDINFO](
	[ARFExtendedInfoID] [int] IDENTITY(1,1) NOT NULL,
	[ExtendedInfoNCName] [nvarchar](500) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFEXTENDEDINFOFORARFEXTENDEDINFOS]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFEXTENDEDINFOFORARFEXTENDEDINFOS](
	[ARFExtendedInfosID] [int] NOT NULL,
	[ARFExtendedInfoID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFEXTENDEDINFOS]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFEXTENDEDINFOS](
	[ARFExtendedInfosID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFOBJECTREF]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFOBJECTREF](
	[ARFObjectRefID] [int] IDENTITY(1,1) NOT NULL,
	[ARFObjectRefUID] [nvarchar](500) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFOBJECTREFARFASSET]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFOBJECTREFARFASSET](
	[ARFObjectRefID] [int] NOT NULL,
	[ARFAssetID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFOBJECTREFREPORT]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFOBJECTREFREPORT](
	[ARFObjectRefID] [int] NOT NULL,
	[ReportID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFOBJECTREFREPORTREQUEST]    Script Date: 04/03/2015 19:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFOBJECTREFREPORTREQUEST](
	[ARFObjectRefID] [int] NOT NULL,
	[ReportRequestID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFRELATIONSHIP]    Script Date: 04/03/2015 19:59:08 ******/
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

/****** Object:  Table [dbo].[ARFRELATIONSHIPARFASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFRELATIONSHIPARFASSET](
	[ARFRelationshipID] [int] NOT NULL,
	[ARFAssetID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFRELATIONSHIPFORARFRELATIONSHIPS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFRELATIONSHIPFORARFRELATIONSHIPS](
	[ARFRelationshipsID] [int] NOT NULL,
	[ARFRelationshipID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFRELATIONSHIPREPORT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFRELATIONSHIPREPORT](
	[ARFRelationshipID] [int] NOT NULL,
	[ReportID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFRELATIONSHIPREPORTREQUEST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFRELATIONSHIPREPORTREQUEST](
	[ARFRelationshipID] [int] NOT NULL,
	[ReportRequestID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARFRELATIONSHIPS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARFRELATIONSHIPS](
	[ARFRelationshipsID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARITHMETICFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARITHMETICFUNCTION](
	[ArithmeticFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[ArithmeticOperationName] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARITHMETICOPERATION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[ARPCACHE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARPCACHE](
	[ARPCacheID] [int] IDENTITY(1,1) NOT NULL,
	[ARPCacheGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARPCACHECHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARPCACHECHANGERECORD](
	[ARPCacheChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARPCACHEENTRIES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARPCACHEENTRIES](
	[ARPCacheEntriesID] [int] IDENTITY(1,1) NOT NULL,
	[ARPCacheID] [int] NOT NULL,
	[ARPCacheEntryID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[suspected_malicious] [bit] NULL,
	[SuspectedMaliciousReasonID] [int] NULL,
	[SuspectedMaliciousReasonGUID] [nvarchar](500) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARPCACHEENTRY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARPCACHEENTRY](
	[ARPCacheEntryID] [int] IDENTITY(1,1) NOT NULL,
	[ARPCacheEntryGUID] [nvarchar](500) NULL,
	[IP_Address] [int] NULL,
	[Physical_Address] [nvarchar](100) NULL,
	[ARPCacheEntryTypeID] [int] NULL,
	[Network_Interface] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARPCACHEENTRYTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARPCACHEENTRYTYPE](
	[ARPCacheEntryTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ARPCacheEntryTypeName] [nvarchar](50) NULL,
	[ARPCacheEntryTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARTIFACT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARTIFACT](
	[ArtifactID] [int] IDENTITY(1,1) NOT NULL,
	[ArtifactGUID] [nvarchar](500) NULL,
	[HashListID] [int] NULL,
	[Raw_Artifact] [nvarchar](max) NULL,
	[RawArtifactID] [int] NULL,
	[Raw_Artifact_Reference] [nvarchar](250) NULL,
	[ArtifactTypeID] [int] NOT NULL,
	[ArtifactTypeGUID] [nvarchar](500) NULL,
	[content_type] [nvarchar](50) NULL,
	[content_type_version] [nvarchar](50) NULL,
	[suspected_malicious] [bit] NULL,
	[SuspectedMaliciousReasonID] [int] NULL,
	[SuspectedMaliciousReasonGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[RepositoryID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARTIFACTCHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARTIFACTCHANGERECORD](
	[ArtifactChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARTIFACTHASHVALUE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARTIFACTHASHVALUE](
	[ArtifactHashValueID] [int] IDENTITY(1,1) NOT NULL,
	[ArtifactID] [int] NOT NULL,
	[ArtifactGUID] [nvarchar](500) NULL,
	[HashValueID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARTIFACTPACKAGING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARTIFACTPACKAGING](
	[ArtifactPackagingID] [int] IDENTITY(1,1) NOT NULL,
	[ArtifactPackagingGUID] [nvarchar](500) NULL,
	[ArtifactID] [int] NOT NULL,
	[ArtifactGUID] [nvarchar](500) NULL,
	[PackagingID] [int] NOT NULL,
	[PackagingGUID] [nvarchar](500) NULL,
	[is_encrypted] [bit] NULL,
	[is_compressed] [bit] NULL,
	[ArtifactPackagingDescription] [nvarchar](max) NULL,
	[CollectedDate] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[RepositoryID] [int] NULL,
	[RepositoryGUID] [nvarchar](500) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ARTIFACTTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARTIFACTTYPE](
	[ArtifactTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ArtifactTypeGUID] [nvarchar](500) NULL,
	[ArtifactTypeName] [nvarchar](50) NULL,
	[ArtifactTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ImportanceID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASN](
	[ASNID] [int] IDENTITY(1,1) NOT NULL,
	[AddressID] [int] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASOBJECT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASOBJECT](
	[ASObjectID] [int] IDENTITY(1,1) NOT NULL,
	[ASNumber] [int] NULL,
	[ASName] [nvarchar](250) NULL,
	[ASHandle] [nvarchar](50) NULL,
	[Regional_Internet_Registry] [nvarchar](100) NULL,
	[OrganisationID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[aspnet_Applications]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[aspnet_Membership]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[aspnet_Paths]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[aspnet_PersonalizationAllUsers]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[aspnet_PersonalizationPerUser]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[aspnet_Profile]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[aspnet_Roles]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[aspnet_SchemaVersions]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[aspnet_Users]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[aspnet_UsersInRoles]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[aspnet_UsersInRoles](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[aspnet_WebEvent_Events]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[ASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSET](
	[AssetID] [int] IDENTITY(1,1) NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[AssetName] [nvarchar](150) NULL,
	[AssetDescription] [nvarchar](max) NULL,
	[AssetCriticalityLevel] [nvarchar](50) NULL,
	[TaskCriticalAsset] [bit] NULL,
	[DefenseCriticalAsset] [bit] NULL,
	[OSName] [nvarchar](250) NULL,
	[Enabled] [bit] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL,
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
	[ADParticipation] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETADDRESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETADDRESS](
	[AssetAddressID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[AddressID] [int] NOT NULL,
	[AddressGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETARPCACHE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETARPCACHE](
	[AssetARPCacheID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[ARPCacheID] [int] NULL,
	[ARPCacheGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETBLACKLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETBLACKLIST](
	[AssetBlacklistID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[OrganisationID] [int] NULL,
	[OrganisationGUID] [nvarchar](500) NULL,
	[PersonID] [int] NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETCERTIFICATE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETCERTIFICATE](
	[AssetCertificateID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[CertificateID] [int] NOT NULL,
	[CertificateGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[AssetCertificateDescription] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETCERTIFICATEORGANISATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETCERTIFICATEORGANISATION](
	[AssetCertificateOrganisationID] [int] IDENTITY(1,1) NOT NULL,
	[AssetCertificateID] [int] NOT NULL,
	[AssetCertificateGUID] [nvarchar](500) NULL,
	[OrganisationID] [int] NOT NULL,
	[OrganisationGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[AssetCertificateOrganisationDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETCHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETCHANGERECORD](
	[AssetChangeRecordID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[ChangeRecordID] [int] NOT NULL,
	[ChangeRecordGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETCREDENTIAL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETCREDENTIAL](
	[AssetCredentialID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[AuthenticationTypeID] [int] NULL,
	[AuthenticationTypeGUID] [nvarchar](500) NULL,
	[AuthenticationType] [nvarchar](50) NULL,
	[Username] [nvarchar](255) NULL,
	[Password] [nvarchar](255) NULL,
	[PersonID] [int] NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[OrganisationID] [int] NULL,
	[OrganisationGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETCRITICALITYLEVEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETCRITICALITYLEVEL](
	[AssetCriticalityLevelID] [int] IDENTITY(1,1) NOT NULL,
	[AssetCriticalityLevelGUID] [nvarchar](500) NULL,
	[CriticalityLevelID] [int] NULL,
	[AssetCriticalityLevelName] [nvarchar](50) NULL,
	[AssetCriticalityLevelDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETCRITICALITYLEVELFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETCRITICALITYLEVELFORASSET](
	[AssetCriticalityID] [int] IDENTITY(1,1) NOT NULL,
	[AssetCriticalityDescription] [nvarchar](max) NULL,
	[AssetID] [int] NOT NULL,
	[AssetCriticalityLevelID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETDEVICE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETDEVICE](
	[AssetDeviceID] [int] IDENTITY(1,1) NOT NULL,
	[AssetDeviceGUID] [nvarchar](500) NULL,
	[AssetDeviceDescription] [nvarchar](max) NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[DeviceID] [int] NOT NULL,
	[DeviceGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETFORASSET](
	[AssetForAssetID] [int] IDENTITY(1,1) NOT NULL,
	[AssetRefID] [int] NOT NULL,
	[AssetRefGUID] [nvarchar](500) NULL,
	[AssetRelationshipID] [int] NULL,
	[relationshiptype] [nvarchar](50) NULL,
	[relationshipscope] [nvarchar](50) NULL,
	[AssetSubjectID] [int] NOT NULL,
	[AssetSubjectGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETFORORGANISATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETFORORGANISATION](
	[AssetForOrganisationID] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationAssetGUID] [nvarchar](500) NULL,
	[OrganisationID] [int] NOT NULL,
	[OrganisationGUID] [nvarchar](500) NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETFORTHREATACTORTTP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETFORTHREATACTORTTP](
	[AssetForThreatActorTTPID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[ThreatActorTTPGUID] [nvarchar](500) NULL,
	[Information_Source] [nvarchar](250) NULL,
	[ConfidenceLevel] [nvarchar](50) NULL,
	[ConfidenceLevelID] [int] NULL,
	[notes] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETFUNCTION](
	[AssetFunctionID] [int] NOT NULL,
	[AssetFunctionName] [nvarchar](100) NOT NULL,
	[AssetFunctionDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETFUNCTIONFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETFUNCTIONFORASSET](
	[AssetAssetFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetFunctionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETGEOLOCATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETGEOLOCATION](
	[AssetGeoLocationID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[GeoLocationID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[CollectionTimestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETGROUP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETGROUP](
	[AssetGroupID] [int] IDENTITY(1,1) NOT NULL,
	[AssetGroupGUID] [nvarchar](500) NULL,
	[AssetForAssetID] [int] NULL,
	[AssetGroupName] [nvarchar](250) NULL,
	[AssetGroupDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[OrganisationID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETINFORMATION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[ASSETLICENSE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETLICENSE](
	[AssetLicenseID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NULL,
	[LicenseNumber] [nvarchar](max) NULL,
	[LicenseValue] [float] NULL,
	[LicenseID] [int] NULL,
	[LicenseFileID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETLOCATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETLOCATION](
	[AssetLocationID] [int] IDENTITY(1,1) NOT NULL,
	[AssetLocationType] [nvarchar](50) NOT NULL,
	[AssetLocationDescription] [nvarchar](100) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETLOCATIONFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETLOCATIONFORASSET](
	[AssetLocationTimeID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetLocationID] [int] NOT NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETMANAGEMENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETMANAGEMENT](
	[AssetManagementID] [int] IDENTITY(1,1) NOT NULL,
	[ManagementID] [int] NULL,
	[ManagementType] [nvarchar](50) NOT NULL,
	[ManagementDescription] [nvarchar](100) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETMANAGEMENTFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
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
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETMEMORYDUMP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETMEMORYDUMP](
	[AssetMemoryDumpID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETNETWORKZONE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETNETWORKZONE](
	[AssetNetworkZoneID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[NetworkZoneID] [int] NULL,
	[NetworkZoneGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL,
	[ConfidentialityLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETNETWORKZONERESTRICTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETNETWORKZONERESTRICTION](
	[AssetNetworkZoneRestrictionID] [int] IDENTITY(1,1) NOT NULL,
	[AssetNetworkZoneRestrictionDescription] [nvarchar](max) NULL,
	[AssetNetworkZoneID] [int] NOT NULL,
	[AssetNetworkZoneGUID] [nvarchar](500) NULL,
	[RestrictionID] [int] NOT NULL,
	[CreationDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETORGANIZATIONALUNIT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETORGANIZATIONALUNIT](
	[AssetOrganizationalUnitID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationalUnitID] [int] NOT NULL,
	[AssetID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETOWNERSHIP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETOWNERSHIP](
	[AssetOwnershipID] [int] IDENTITY(1,1) NOT NULL,
	[OwnershipID] [int] NULL,
	[OwnershipName] [nvarchar](50) NOT NULL,
	[OwnershipDescription] [nvarchar](100) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETPERIMETER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETPERIMETER](
	[AssetPerimeterID] [int] IDENTITY(1,1) NOT NULL,
	[AssetPerimeterGUID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETPERIMETERASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETPERIMETERASSET](
	[AssetPerimeterAssetID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETPERIMETERNETWORKZONE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETPERIMETERNETWORKZONE](
	[AssetPerimeterNetworkZoneID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETPERIMETERSECURITYCONTROL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETPERIMETERSECURITYCONTROL](
	[AssetPerimeterSecurityControlID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETPHYSICALLOCATION]    Script Date: 04/03/2015 19:59:09 ******/
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
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETPLATFORM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETPLATFORM](
	[AssetPlatformID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[PlatformID] [int] NULL,
	[PlatformGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETPRODUCT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETPRODUCT](
	[AssetProductID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[ProductID] [int] NOT NULL,
	[ProductGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETRELATIONSHIP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETRELATIONSHIP](
	[AssetRelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[relationshiptype] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETREPORTCOLLECTION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[ASSETRISKRATING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETRISKRATING](
	[AssetRiskRatingID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[RiskRatingID] [int] NOT NULL,
	[AssetRiskRatingDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETROLE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETROLE](
	[AssetRoleID] [int] NOT NULL,
	[AssetRoleGUID] [nvarchar](500) NULL,
	[AssetRoleName] [nvarchar](100) NOT NULL,
	[AssetRoleDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETROLEFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETROLEFORASSET](
	[AssetRoleForAssetID] [int] IDENTITY(1,1) NOT NULL,
	[AssetAssetRoleGUID] [nvarchar](500) NULL,
	[AssetRoleID] [int] NOT NULL,
	[AssetRoleGUID] [nvarchar](500) NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETS](
	[AssetsID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETSECURITYCONTROL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETSECURITYCONTROL](
	[AssetSecurityControlID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[SecurityControlID] [int] NOT NULL,
	[SecurityControlGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[ImportanceID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETSENSOR]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETSENSOR](
	[AssetSensorID] [int] IDENTITY(1,1) NOT NULL,
	[AssetSensorGUID] [nvarchar](500) NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[SensorID] [int] NOT NULL,
	[SensorGUID] [nvarchar](500) NULL,
	[AssetSensorName] [nvarchar](50) NULL,
	[AssetSensorDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETSESSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETSESSION](
	[AssetSessionID] [int] IDENTITY(1,1) NOT NULL,
	[AssetSessionGUID] [nvarchar](500) NULL,
	[SessionID] [int] NULL,
	[SessionGUID] [nvarchar](500) NULL,
	[AssetID] [int] NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETSYNTHETICID]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETSYNTHETICID](
	[AssetSyntheticID] [int] IDENTITY(1,1) NOT NULL,
	[AssetSyntheticIDGUID] [nvarchar](500) NULL,
	[resource] [nvarchar](250) NOT NULL,
	[id] [nvarchar](250) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETSYNTHETICIDFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETSYNTHETICIDFORASSET](
	[AssetAssetSyntheticID] [int] NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[AssetSyntheticID] [int] NOT NULL,
	[AssetSyntheticIDGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETTECHNOLOGY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETTECHNOLOGY](
	[AssetTechnologyID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[TechnologyID] [int] NULL,
	[TechnologyGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETVALUE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETVALUE](
	[AssetValueID] [int] IDENTITY(1,1) NOT NULL,
	[AssetValueName] [nvarchar](50) NOT NULL,
	[AssetValueDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETVALUEFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETVALUEFORASSET](
	[AssetValueForAssetID] [int] IDENTITY(1,1) NOT NULL,
	[AssetAssetValueGUID] [nvarchar](500) NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[AssetValueID] [int] NOT NULL,
	[AssetValueGUID] [nvarchar](500) NULL,
	[ValueValue] [float] NULL,
	[iso_currency_code] [nvarchar](3) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETVARIETY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETVARIETY](
	[AssetVarietyID] [int] IDENTITY(1,1) NOT NULL,
	[AssetVarietyName] [nvarchar](100) NOT NULL,
	[AssetVarietyDescription] [nvarchar](500) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETVARIETYFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETVARIETYFORASSET](
	[AssetAssetVarietyID] [int] IDENTITY(1,1) NOT NULL,
	[AssetVarietyID] [int] NOT NULL,
	[AssetVarietyGUID] [nvarchar](500) NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSETWHITELIST]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[ASSETZONE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSETZONE](
	[AssetZoneID] [int] IDENTITY(1,1) NOT NULL,
	[AssetZoneGUID] [nvarchar](500) NULL,
	[AssetID] [int] NULL,
	[ZoneID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSOCIATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSOCIATION](
	[AssociationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSOCIATIONRULE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSOCIATIONRULE](
	[AssociationRuleID] [int] IDENTITY(1,1) NOT NULL,
	[RuleID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSURANCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSURANCE](
	[AssuranceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ASSURANCEREQUIREMENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ASSURANCEREQUIREMENT](
	[AssuranceRequirementID] [int] IDENTITY(1,1) NOT NULL,
	[RequirementID] [int] NULL,
	[RequirementGUID] [nvarchar](500) NULL,
	[AssuranceRequirementGUID] [nvarchar](500) NULL,
	[AssuranceRequirementTitle] [nvarchar](250) NULL,
	[AssuranceRequirementDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACHMENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACHMENT](
	[AttachmentID] [int] IDENTITY(1,1) NOT NULL,
	[AttachmentGUID] [nvarchar](500) NULL,
	[FileID] [int] NULL,
	[FileGUID] [nvarchar](500) NULL,
	[MIMEID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTACHMENTREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATTACHMENTREFERENCE](
	[AttachmentReferenceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTRIBUTE]    Script Date: 04/03/2015 19:59:09 ******/
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
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ATTRIBUTEVALUE]    Script Date: 04/03/2015 19:59:09 ******/
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
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AUDIT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AUDIT](
	[AuditID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AUDITFINDING]    Script Date: 04/03/2015 19:59:09 ******/
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
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AUDITLOGEVENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AUDITLOGEVENT](
	[AuditLogEventID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AUDITPROCEDURE]    Script Date: 04/03/2015 19:59:09 ******/
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
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AUTHENTICATIONTOKENPROTECTIONMECHANISM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AUTHENTICATIONTOKENPROTECTIONMECHANISM](
	[AuthenticationTokenProtectionMechanismID] [int] IDENTITY(1,1) NOT NULL,
	[AuthenticationTokenProtectionMechanismGUID] [nvarchar](500) NULL,
	[AuthenticationTokenProtectionMechanismName] [nvarchar](50) NULL,
	[AuthenticationTokenProtectionMechanismDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AUTHENTICATIONTOKENPROTECTIONMECHANISMBLACKLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AUTHENTICATIONTOKENPROTECTIONMECHANISMBLACKLIST](
	[AuthenticationTokenProtectionMechanismBlacklistID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AUTHENTICATIONTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AUTHENTICATIONTYPE](
	[AuthenticationTypeID] [int] IDENTITY(1,1) NOT NULL,
	[AuthenticationTypeGUID] [nvarchar](500) NULL,
	[AuthenticationTypeName] [nvarchar](50) NULL,
	[AuthenticationTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[VocabularyID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AUTHENTICATIONTYPEBLACKLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AUTHENTICATIONTYPEBLACKLIST](
	[AuthenticationTypeBlacklistID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AUTHENTICATIONTYPEDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AUTHENTICATIONTYPEDESCRIPTION](
	[AuthenticationTypeDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AUTHENTICATIONTYPEREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AUTHENTICATIONTYPEREFERENCE](
	[AuthenticationTypeReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[AuthenticationTypeID] [int] NULL,
	[AuthenticationTypeGUID] [nvarchar](500) NULL,
	[ReferenceID] [int] NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AUTHOR]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AUTHOR](
	[AuthorID] [int] IDENTITY(1,1) NOT NULL,
	[AuthorName] [nvarchar](250) NOT NULL,
	[PersonID] [int] NULL,
	[OrganisationID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AVAILABILITYLOSSTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AVAILABILITYLOSSTYPE](
	[AvailabilityLossTypeID] [int] IDENTITY(1,1) NOT NULL,
	[AvailabilityLossTypeName] [nvarchar](50) NULL,
	[AvailabilityLossTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AVAILABILITYVIOLATIONPROPERTIES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AVAILABILITYVIOLATIONPROPERTIES](
	[AvailabilityViolationPropertiesID] [int] IDENTITY(1,1) NOT NULL,
	[AvailabilityViolationPropertiesName] [nvarchar](50) NULL,
	[AvailabilityViolationPropertiesDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ieEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AVAILABILITYVIOLATIONSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AVAILABILITYVIOLATIONSTRATEGICOBJECTIVE](
	[AvailabilityViolationStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[AvailabilityViolationStrategicObjectiveName] [nvarchar](50) NULL,
	[AvailabilityViolationStrategicObjectiveDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AVAILABILITYVIOLATIONTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AVAILABILITYVIOLATIONTACTICALOBJECTIVE](
	[AvailabilityViolationTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[AvailabilityViolationTacticalObjectiveName] [nvarchar](50) NULL,
	[AvailabilityViolationTacticalObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BANNER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BANNER](
	[BannerID] [int] IDENTITY(1,1) NOT NULL,
	[BannerGUID] [nvarchar](500) NULL,
	[BannerName] [nvarchar](50) NULL,
	[BannerDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BANNERREGEX]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BANNERREGEX](
	[BannerRegexID] [int] IDENTITY(1,1) NOT NULL,
	[BannerRegexGUID] [nvarchar](500) NULL,
	[BannerID] [int] NOT NULL,
	[BannerGUID] [nvarchar](500) NULL,
	[RegexID] [int] NOT NULL,
	[RegexGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEGINFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[BEHAVIOMETRIC]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIOMETRIC](
	[BehaviometricID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIOR]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIOR](
	[BehaviorID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORACTIONCOMPOSITION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORACTIONCOMPOSITION](
	[BehaviorActionCompositionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORALCHARACTERISTIC]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORALCHARACTERISTIC](
	[BehavioralCharacteristicID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORASSOCIATEDCODE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORASSOCIATEDCODE](
	[BehaviorAssociatedCodeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORCOLLECTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORCOLLECTION](
	[BehaviorCollectionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORDESCRIPTION](
	[BehaviorDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORDISCOVERYMETHOD]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[BEHAVIORIDMATCHINGPATTERN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORIDMATCHINGPATTERN](
	[BehaviorIDPatternID] [int] IDENTITY(1,1) NOT NULL,
	[BehaviorIDPatternGUID] [nvarchar](500) NULL,
	[BehaviorID] [int] NOT NULL,
	[BehaviorGUID] [nvarchar](500) NULL,
	[BehaviorIDMatchingPattern] [nvarchar](500) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORIDPATTERN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORIDPATTERN](
	[BehaviorIDPatternID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORPURPOSE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORPURPOSE](
	[BehaviorPurposeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BEHAVIORRELATIONSHIPS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BEHAVIORRELATIONSHIPS](
	[BehaviorRelationShipsID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BIOMETRIC]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BIOMETRIC](
	[BiometricID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BREACH]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BREACH](
	[BreachID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BREACHDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BREACHDESCRIPTION](
	[BreachDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BREACHEVIDENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BREACHEVIDENCE](
	[BreachEvidenceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BREACHFINDING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BREACHFINDING](
	[BreachFindingID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BREACHNOTIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BREACHNOTIFICATION](
	[BreachNotificationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BREACHTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BREACHTAG](
	[BreachTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BREAK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BREAK](
	[BreakID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BROWSER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BROWSER](
	[BrowserID] [int] IDENTITY(1,1) NOT NULL,
	[SoftwareID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BROWSERCHARACTERISTIC]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BROWSERCHARACTERISTIC](
	[BrowserCharacteristicID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BULLETIN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BULLETIN](
	[BulletinID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BUSINESSIMPACT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUSINESSIMPACT](
	[BusinessImpactID] [int] IDENTITY(1,1) NOT NULL,
	[BusinessImpactGUID] [nvarchar](500) NULL,
	[ImpactLevel] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BUSINESSIMPACTFORBUSINESSRISK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUSINESSIMPACTFORBUSINESSRISK](
	[BusinessRiskBusinessImpactID] [int] IDENTITY(1,1) NOT NULL,
	[BusinessImpactID] [int] NOT NULL,
	[BusinessImpactGUID] [nvarchar](500) NULL,
	[BusinessRiskID] [int] NOT NULL,
	[BusinessRiskGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BUSINESSIMPACTFORREGULATORYRISK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUSINESSIMPACTFORREGULATORYRISK](
	[RegulatoryRiskBusinessImpactID] [int] IDENTITY(1,1) NOT NULL,
	[BusinessImpactID] [int] NOT NULL,
	[BusinessImpactGUID] [nvarchar](500) NULL,
	[RegulatoryRiskID] [int] NOT NULL,
	[RegulatoryRiskGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BUSINESSPROCESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUSINESSPROCESS](
	[BusinessProcessID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BUSINESSRISK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUSINESSRISK](
	[BusinessRiskID] [int] IDENTITY(1,1) NOT NULL,
	[RiskDescription] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BYTERUN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BYTERUN](
	[ByteRunID] [int] IDENTITY(1,1) NOT NULL,
	[Offset] [int] NULL,
	[File_System_Offset] [int] NULL,
	[Image_Offset] [int] NULL,
	[Length] [int] NULL,
	[HashListID] [int] NULL,
	[Byte_Run_Data] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BYTERUNS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BYTERUNS](
	[ByteRunsID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[BYTESRUNSBYTERUN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BYTESRUNSBYTERUN](
	[ByteRunsButeRunID] [int] IDENTITY(1,1) NOT NULL,
	[ByteRunsID] [int] NOT NULL,
	[ByteRunID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CAPABILITYOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CAPABILITYOBJECTIVE](
	[CapabilityObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[CapabilityObjectiveGUID] [nvarchar](500) NULL,
	[ObjectiveID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CAPABILITYOBJECTIVERELATIONSHIP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CAPABILITYOBJECTIVERELATIONSHIP](
	[CapabilityObjectiveRelashionshipID] [int] IDENTITY(1,1) NOT NULL,
	[CapabilityObjectiveRelashionshipName] [nvarchar](50) NULL,
	[CapabilityObjectiveRelashionshipDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CATEGORY](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](150) NOT NULL,
	[CategoryDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CATEGORYDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CATEGORYDESCRIPTION](
	[CategoryDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CATEGORYREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CATEGORYREFERENCE](
	[CategoryReferenceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CATEGORYTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CATEGORYTAG](
	[CategoryTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCE](
	[CCEID] [int] IDENTITY(1,1) NOT NULL,
	[cce_id] [nvarchar](20) NOT NULL,
	[platform] [nvarchar](20) NULL,
	[PlatformID] [int] NULL,
	[modified] [datetimeoffset](7) NULL,
	[description] [nvarchar](max) NULL,
	[parameter] [nvarchar](250) NULL,
	[technical_mechanism] [nvarchar](250) NULL,
	[reference] [nvarchar](100) NULL,
	[resource_id] [nvarchar](4000) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[CreationObjectGUID] [nvarchar](500) NULL,
	[SourceID] [int] NULL,
	[SourceGUID] [nvarchar](500) NULL,
	[RepositoryID] [int] NULL,
	[RepositoryGUID] [nvarchar](500) NULL,
	[VocabularyID] [int] NULL,
	[VocabularyGUID] [nvarchar](500) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ImportanceID] [int] NULL,
	[ImportanceGUID] [nvarchar](500) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceLevelGUID] [nvarchar](500) NULL,
	[ConfidenceReasonID] [int] NULL,
	[ConfidenceReasonGUID] [nvarchar](500) NULL,
	[TrustLevelID] [int] NULL,
	[TrustLevelGUID] [nvarchar](500) NULL,
	[TrustReasonID] [int] NULL,
	[TrustReasonGUID] [nvarchar](500) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCEFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCEFORASSET](
	[AssetCCEID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[CCEID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCEFORCPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCEFORCPE](
	[CPECCEID] [int] IDENTITY(1,1) NOT NULL,
	[cce_id] [nvarchar](20) NULL,
	[CCEID] [int] NOT NULL,
	[CPEID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCEFORTHREATACTORTTP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCEFORTHREATACTORTTP](
	[ThreatActorTTPCCEID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[CCEID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCEPARAMETER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCEPARAMETER](
	[CCEParameterID] [int] IDENTITY(1,1) NOT NULL,
	[CCEParameterText] [nvarchar](max) NOT NULL,
	[CCEParameterDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCEPARAMETERFORCCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCEPARAMETERFORCCE](
	[CCECCEParameterID] [int] IDENTITY(1,1) NOT NULL,
	[CCEID] [int] NULL,
	[CCEParameterID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCEPARAMETERTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCEPARAMETERTAG](
	[CCEParameterTagID] [int] IDENTITY(1,1) NOT NULL,
	[CCEParameterID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCEREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCEREFERENCE](
	[CCEReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[resource_id] [nvarchar](4000) NOT NULL,
	[ReferenceText] [nvarchar](max) NOT NULL,
	[ReferenceID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCEREFERENCEFORCCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCEREFERENCEFORCCE](
	[CCECCEReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[CCEReferenceID] [int] NOT NULL,
	[CCEID] [int] NULL,
	[cce_id] [nvarchar](20) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCERESOURCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCERESOURCE](
	[CCEResourceID] [int] IDENTITY(1,1) NOT NULL,
	[resource_id] [nvarchar](4000) NOT NULL,
	[modified] [nchar](10) NULL,
	[ResourceTitle] [nvarchar](max) NULL,
	[ResourcePublisher] [nvarchar](150) NULL,
	[issued] [nchar](10) NULL,
	[ResourceVersion] [nvarchar](50) NULL,
	[ResourceFormat] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCERESOURCEAUTHOR]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCERESOURCEAUTHOR](
	[CCEResourceAuthorID] [int] IDENTITY(1,1) NOT NULL,
	[CCEResourceID] [int] NOT NULL,
	[AuthorID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCERESOURCEFORCCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCERESOURCEFORCCE](
	[CCECCEResourceID] [int] IDENTITY(1,1) NOT NULL,
	[CCEResourceID] [int] NOT NULL,
	[CCEID] [int] NULL,
	[cce_id] [nvarchar](20) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCERESOURCEFORCCEREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCERESOURCEFORCCEREFERENCE](
	[CCEReferenceCCEResourceID] [int] IDENTITY(1,1) NOT NULL,
	[CCEResourceID] [int] NOT NULL,
	[CCEReferenceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCETECHNICALMECHANISM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCETECHNICALMECHANISM](
	[CCETechnicalMechanismID] [int] IDENTITY(1,1) NOT NULL,
	[TechnicalMechanismText] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCETECHNICALMECHANISMFORCCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCETECHNICALMECHANISMFORCCE](
	[CCECCETechnicalMechanismID] [int] IDENTITY(1,1) NOT NULL,
	[CCEID] [int] NULL,
	[CCETechnicalMechanismID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CCETECHNICALMECHANISMTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CCETECHNICALMECHANISMTAG](
	[CCETechnicalMechanismTagID] [int] IDENTITY(1,1) NOT NULL,
	[CCETechnicalMechanismID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CERTIFICATE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CERTIFICATE](
	[CertificateID] [int] IDENTITY(1,1) NOT NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CERTIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CERTIFICATION](
	[CertificationID] [int] IDENTITY(1,1) NOT NULL,
	[CertificationGUID] [nvarchar](500) NULL,
	[CertificationAcronym] [nvarchar](50) NULL,
	[CertificationName] [nvarchar](500) NOT NULL,
	[CertificationDescription] [nvarchar](max) NULL,
	[lang] [nvarchar](10) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CERTIFICATIONSKILL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CERTIFICATIONSKILL](
	[CertificationSkillID] [int] IDENTITY(1,1) NOT NULL,
	[CertificationID] [int] NULL,
	[SkillID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHANGECONTROL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHANGECONTROL](
	[ChangeControlID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityControlID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHANGELOGENTRYTYPEENUM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHANGELOGENTRYTYPEENUM](
	[ChangeLogEntryTypeEnumID] [int] IDENTITY(1,1) NOT NULL,
	[ChangeLogEntryType] [nvarchar](50) NULL,
	[ChangeLogEntryTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHANGERECORD](
	[ChangeRecordID] [int] IDENTITY(1,1) NOT NULL,
	[ChangeRecordGUID] [nvarchar](500) NULL,
	[ChangedObjectGUID] [nvarchar](500) NULL,
	[BeforeChange] [nvarchar](max) NULL,
	[AfterChange] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHANGEREQUEST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHANGEREQUEST](
	[ChangeRequestID] [int] IDENTITY(1,1) NOT NULL,
	[ChangeRequestGUID] [nvarchar](500) NULL,
	[ImportanceID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[StatusID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHANGEREQUESTAPPROVAL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHANGEREQUESTAPPROVAL](
	[ChangeRequestApprovalID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHANGEREQUESTCHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHANGEREQUESTCHANGERECORD](
	[ChangeRequestChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHAPTER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHAPTER](
	[ChapterID] [int] IDENTITY(1,1) NOT NULL,
	[SectionID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHARACTER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHARACTER](
	[CharacterID] [int] IDENTITY(1,1) NOT NULL,
	[CharacterGUID] [nvarchar](500) NULL,
	[CharacterValue] [nchar](1) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHARACTERBLACKLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHARACTERBLACKLIST](
	[CharacterBlacklistID] [int] IDENTITY(1,1) NOT NULL,
	[CharacterID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHARACTERENCODING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHARACTERENCODING](
	[CharacterEncodingID] [int] IDENTITY(1,1) NOT NULL,
	[CharacterEncodingName] [nvarchar](50) NOT NULL,
	[CharacterEncodingDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHARACTERISTIC]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHARACTERISTIC](
	[CharacteristicID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHARACTERSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHARACTERSET](
	[CharacterSetID] [int] IDENTITY(1,1) NOT NULL,
	[CharacterSetGUID] [nvarchar](500) NULL,
	[CharacterSetName] [nvarchar](50) NULL,
	[CharacterSetValue] [nvarchar](250) NULL,
	[CharacterSetDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHARACTERSETBLACKLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHARACTERSETBLACKLIST](
	[CharacterSetBlacklistID] [int] IDENTITY(1,1) NOT NULL,
	[CharacterSetID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHARACTERSETWHITELIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHARACTERSETWHITELIST](
	[CharacterSetWhitelistID] [int] IDENTITY(1,1) NOT NULL,
	[CharacterSetID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHARACTERWHITELIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHARACTERWHITELIST](
	[CharacterWhitelistID] [int] IDENTITY(1,1) NOT NULL,
	[CharacterID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHARSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHARSET](
	[CharSetID] [int] IDENTITY(1,1) NOT NULL,
	[CharacterSetID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHECKENUMERATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHECKENUMERATION](
	[CheckEnumerationID] [int] IDENTITY(1,1) NOT NULL,
	[EnumerationValue] [nvarchar](50) NOT NULL,
	[EnumerationDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHECKLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHECKLIST](
	[ChecklistID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[AnswerSchemes] [nvarchar](50) NULL,
	[ChecklistCategoryID] [int] NULL,
	[MethodologyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHECKLISTANSWER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHECKLISTANSWER](
	[AnswerID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionID] [int] NULL,
	[Answer] [nvarchar](max) NULL,
	[AnswerComments] [nvarchar](max) NULL,
	[AttachmentID] [int] NULL,
	[AttachmentData] [image] NULL,
	[MIMEID] [int] NULL,
	[AttachmentMimeType] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[OrganisationID] [int] NULL,
	[PersonID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHECKLISTCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHECKLISTCATEGORY](
	[ChecklistCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NULL,
	[Title] [nchar](10) NULL,
	[ChecklistCategoryDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHECKLISTCHAPTER]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[CHECKLISTQUESTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHECKLISTQUESTION](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionRefID] [nvarchar](50) NULL,
	[Title] [nvarchar](max) NULL,
	[LongName] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Target] [nvarchar](max) NULL,
	[ChapterID] [int] NULL,
	[Tags] [nvarchar](max) NULL,
	[lang] [nvarchar](10) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHECKLISTQUESTIONCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHECKLISTQUESTIONCATEGORY](
	[QuestionCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionID] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CHECKLISTQUESTIONSECURITYCONTROL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CHECKLISTQUESTIONSECURITYCONTROL](
	[QuestionSecurityControlID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionID] [int] NOT NULL,
	[SecurityControlID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CIAIMPACTFORATTACKPATTERN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CIAIMPACTFORATTACKPATTERN](
	[AttackPatternCIAImpactID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternID] [int] NULL,
	[AttackPatternGUID] [nvarchar](500) NULL,
	[Confidentiality_Impact] [nvarchar](50) NULL,
	[Integrity_Impact] [nvarchar](50) NULL,
	[Availability_Impact] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CLASSIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CLASSIFICATION](
	[ClassificationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CLASSIFICATIONCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CLASSIFICATIONCATEGORY](
	[ClassificationCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ClassificationCategoryGUID] [nvarchar](500) NULL,
	[ClassificationCategoryName] [nvarchar](250) NULL,
	[ClassificationCategoryDescription] [nvarchar](max) NULL,
	[CategoryID] [int] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CLASSIFICATIONLEVEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CLASSIFICATIONLEVEL](
	[ClassificationLevelID] [int] IDENTITY(1,1) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CLASSIFICATIONRESTRICTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CLASSIFICATIONRESTRICTION](
	[ClassificationRestrictionID] [int] IDENTITY(1,1) NOT NULL,
	[ClassificationID] [int] NOT NULL,
	[RestrictionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidityID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CLUSTEREDGENODEPAIR]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CLUSTEREDGENODEPAIR](
	[ClusterEdgeNodePairID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COASTAGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COASTAGE](
	[COAStageID] [int] IDENTITY(1,1) NOT NULL,
	[COAStageGUID] [nvarchar](500) NULL,
	[COAStageName] [nvarchar](100) NOT NULL,
	[COAStageDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CODE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CODE](
	[CodeID] [int] IDENTITY(1,1) NOT NULL,
	[CodeGUID] [nvarchar](500) NULL,
	[Block_Nature] [nvarchar](50) NULL,
	[ScriptID] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[Type] [nvarchar](50) NULL,
	[CodeTypeID] [int] NULL,
	[Purpose] [nvarchar](50) NULL,
	[CodePurposeID] [int] NULL,
	[Code_Language] [nvarchar](50) NULL,
	[CodeLanguageID] [int] NULL,
	[TargetedPlatformsID] [int] NULL,
	[Processor_Family] [nvarchar](50) NULL,
	[Discovery_Method] [nvarchar](50) NULL,
	[MeasureSourceID] [int] NULL,
	[Start_Address] [nvarchar](50) NULL,
	[MemoryAddressID] [int] NULL,
	[Code_Segment] [nvarchar](max) NULL,
	[Code_Segment_XOR] [nvarchar](max) NULL,
	[CodeSegmentXORID] [int] NULL,
	[DigitalSignaturesID] [int] NULL,
	[Extracted_Features] [nvarchar](max) NULL,
	[ExtractedFeaturesID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[RepositoryID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CODEFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CODEFUNCTION](
	[CodeFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[CodeFunctionGUID] [nvarchar](500) NULL,
	[CodeID] [int] NULL,
	[CodeGUID] [nvarchar](500) NULL,
	[FunctionID] [int] NULL,
	[FunctionGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CODELANGUAGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CODELANGUAGE](
	[CodeLanguageID] [int] IDENTITY(1,1) NOT NULL,
	[LanguageID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CODELANGUAGES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CODELANGUAGES](
	[CodeLanguagesID] [int] IDENTITY(1,1) NOT NULL,
	[CodeID] [int] NOT NULL,
	[LanguageID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CodeLanguageDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[DiscoveryMethodID] [int] NULL,
	[DiscoveryToolID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CODELICENSE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CODELICENSE](
	[CodeLicenseID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CODELINE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CODELINE](
	[CodeLineID] [int] IDENTITY(1,1) NOT NULL,
	[CodeLineGUID] [nvarchar](500) NULL,
	[LineOfCode] [nvarchar](max) NULL,
	[KnownVulnerable] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CODELINEFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CODELINEFUNCTION](
	[CodeLineFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[CodeLineID] [int] NOT NULL,
	[FunctionID] [int] NULL,
	[LanguageFunctionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CODELINES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CODELINES](
	[CodeLinesID] [int] IDENTITY(1,1) NOT NULL,
	[CodeID] [int] NOT NULL,
	[CodeLineID] [int] NOT NULL,
	[ordinal_position] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CODEPROCESSORTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CODEPROCESSORTYPE](
	[CodeProcessorTypeID] [int] IDENTITY(1,1) NOT NULL,
	[CodeID] [int] NOT NULL,
	[ProcessorTypeID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CODEPURPOSE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CODEPURPOSE](
	[CodePurposeID] [int] IDENTITY(1,1) NOT NULL,
	[CodePurposeEnumID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CODEPURPOSEENUM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CODEPURPOSEENUM](
	[CodePurposeEnumID] [int] IDENTITY(1,1) NOT NULL,
	[CodePurpose] [nvarchar](50) NULL,
	[CodePurposeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CODESEGMENTXOR]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CODESEGMENTXOR](
	[CodeSegmentXORID] [int] IDENTITY(1,1) NOT NULL,
	[xor_pattern] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CODETYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CODETYPE](
	[CodeTypeID] [int] IDENTITY(1,1) NOT NULL,
	[CodeTypeEnumID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CODETYPEENUM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CODETYPEENUM](
	[CodeTypeEnumID] [int] IDENTITY(1,1) NOT NULL,
	[CodeType] [nvarchar](50) NULL,
	[CodeTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COLLECTIONMETHOD]    Script Date: 04/03/2015 19:59:09 ******/
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
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COLLECTIONMETHODDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COLLECTIONMETHODDESCRIPTION](
	[CollectionMethodDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COLLECTIONMETHODREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COLLECTIONMETHODREFERENCE](
	[CollectionMethodReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[CollectionMethodID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[CollectionMethodDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COLLECTIONMETHODTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COLLECTIONMETHODTAG](
	[CollectionMethodTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COLSTAGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COLSTAGE](
	[COLStageID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMMAND]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMMAND](
	[CommandID] [int] IDENTITY(1,1) NOT NULL,
	[CommandName] [nvarchar](50) NOT NULL,
	[CommandDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[KnownVulnerable] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMMANDANDCONTROLPROPERTIES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMMANDANDCONTROLPROPERTIES](
	[CommandandControlPropertiesID] [int] IDENTITY(1,1) NOT NULL,
	[CommandandControlPropertiesName] [nvarchar](50) NULL,
	[CommandandControlPropertiesDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMMANDANDCONTROLSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMMANDANDCONTROLSTRATEGICOBJECTIVE](
	[CommandandControlStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[CommandandControlStrategicObjectiveName] [nvarchar](50) NULL,
	[CommandandControlStrategicObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMMANDANDCONTROLTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMMANDANDCONTROLTACTICALOBJECTIVE](
	[CommandandControlTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[CommandandControlTacticalObjectiveName] [nvarchar](50) NULL,
	[CommandandControlTacticalObjectiveDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMMANDS]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[COMMONCAPABILITYPROPERTIES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMMONCAPABILITYPROPERTIES](
	[CommonCapabilityPropertiesID] [int] IDENTITY(1,1) NOT NULL,
	[CommonCapabilityPropertiesName] [nvarchar](50) NULL,
	[CommonCapabilityPropertiesDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMPLIANCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMPLIANCE](
	[ComplianceID] [int] IDENTITY(1,1) NOT NULL,
	[ComplianceGUID] [nvarchar](500) NULL,
	[ComplianceName] [nvarchar](50) NULL,
	[ComplianceVersion] [nvarchar](50) NULL,
	[ComplianceDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ImportanceID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMPLIANCECATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
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
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMPLIANCECERTIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMPLIANCECERTIFICATION](
	[ComplianceCertificationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMPLIANCECHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMPLIANCECHANGERECORD](
	[ComplianceChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMPLIANCEDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMPLIANCEDESCRIPTION](
	[ComplianceDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMPLIANCEREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[COMPLIANCETAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMPLIANCETAG](
	[ComplianceTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMPONENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMPONENT](
	[ComponentID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMPRESSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMPRESSION](
	[CompressionID] [int] IDENTITY(1,1) NOT NULL,
	[compression_mechanism] [nvarchar](50) NULL,
	[compression_mechanism_ref] [nvarchar](250) NULL,
	[CompressionDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMPRESSIONMECHANISM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMPRESSIONMECHANISM](
	[CompressionMechanismID] [int] NOT NULL,
	[MechanismID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMPRESSIONMECHANISMDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMPRESSIONMECHANISMDESCRIPTION](
	[CompressionMechanismDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMPRESSIONMECHANISMTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMPRESSIONMECHANISMTAG](
	[CompressionMechanismTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COMPRESSIONREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMPRESSIONREFERENCE](
	[CompressionReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[CompressionID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONCATFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONCATFUNCTION](
	[ConcatFunctionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONDITION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[CONDITIONAPPLICATION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[CONFIDENCELEVEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONFIDENCELEVEL](
	[ConfidenceLevelID] [int] IDENTITY(1,1) NOT NULL,
	[ConfidenceLevelGUID] [nvarchar](500) NULL,
	[ConfidenceLevelName] [nvarchar](100) NOT NULL,
	[ConfidenceLevelDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONFIDENCEREASON]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONFIDENCEREASON](
	[ConfidenceReasonID] [int] IDENTITY(1,1) NOT NULL,
	[ConfidenceReasonName] [nvarchar](50) NULL,
	[ConfidenceReasonDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONFIDENTIALITYLEVEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONFIDENTIALITYLEVEL](
	[ConfidentialityLevelID] [int] IDENTITY(1,1) NOT NULL,
	[ClassificationID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONNECTION]    Script Date: 04/03/2015 19:59:09 ******/
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
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONNECTIONFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONNECTIONFORASSET](
	[AssetConnectionID] [int] IDENTITY(1,1) NOT NULL,
	[AssetConnectionGUID] [nvarchar](500) NULL,
	[ConnectionID] [int] NOT NULL,
	[ConnectionGUID] [nvarchar](500) NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[TrustLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONTACT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONTACT](
	[ContactID] [int] IDENTITY(1,1) NOT NULL,
	[ContactTypeID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONTACTTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONTACTTYPE](
	[ContactTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ContactTypeGUID] [nvarchar](500) NULL,
	[ContactTypeName] [nvarchar](50) NOT NULL,
	[ContactTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONTENTENUMERATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONTENTENUMERATION](
	[ContentEnumerationID] [int] IDENTITY(1,1) NOT NULL,
	[ContentEnumerationValue] [nvarchar](50) NOT NULL,
	[ContentEnumerationDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONTEXT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONTEXT](
	[ContextID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONTROL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONTROL](
	[ControlID] [int] IDENTITY(1,1) NOT NULL,
	[ControlGUID] [nvarchar](500) NULL,
	[ControlName] [nvarchar](50) NULL,
	[ControlDescription] [nvarchar](max) NULL,
	[ReliabilityID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONTROLCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONTROLCATEGORY](
	[ControlCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ControlCategoryGUID] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONTROLDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONTROLDESCRIPTION](
	[ControlDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[ControlID] [int] NULL,
	[ControlGUID] [nvarchar](500) NULL,
	[DescriptionID] [int] NULL,
	[DescriptionGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONTROLREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONTROLREFERENCE](
	[ControlReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[ControlID] [int] NULL,
	[ControlGUID] [nvarchar](500) NULL,
	[ReferenceID] [int] NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONTROLSTRENGTH]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONTROLSTRENGTH](
	[ControlStrengthID] [int] IDENTITY(1,1) NOT NULL,
	[ControlStrengthGUID] [nvarchar](500) NULL,
	[ControlStrengthName] [nvarchar](50) NULL,
	[ControlStrengthDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CONTROLTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONTROLTAG](
	[ControlTagID] [int] IDENTITY(1,1) NOT NULL,
	[ControlID] [int] NULL,
	[ControlGUID] [nvarchar](500) NULL,
	[TagID] [int] NULL,
	[TagGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COOKIE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COOKIE](
	[CookieID] [int] IDENTITY(1,1) NOT NULL,
	[CookieGUID] [nvarchar](500) NULL,
	[CookieNameValue] [nvarchar](250) NULL,
	[CookieNameID] [int] NULL,
	[CookieNameGUID] [nvarchar](500) NULL,
	[CookieValue] [nvarchar](max) NULL,
	[CookieDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COOKIEAPPLICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COOKIEAPPLICATION](
	[CookieApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[CookieApplicationRelationship] [nvarchar](50) NULL,
	[CookieApplicationDescription] [nvarchar](max) NULL,
	[CookieID] [int] NULL,
	[CookieGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COOKIECPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COOKIECPE](
	[CookieCPEID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COOKIEFILE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COOKIEFILE](
	[CookieFileID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COOKIENAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COOKIENAME](
	[CookieNameID] [int] IDENTITY(1,1) NOT NULL,
	[CookieNameGUID] [nvarchar](500) NULL,
	[CookieNameValue] [nvarchar](250) NULL,
	[CookieNameDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COOKIENAMEAPPLICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COOKIENAMEAPPLICATION](
	[CookieNameApplicationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COOKIENAMEORGANISATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COOKIENAMEORGANISATION](
	[CookieNameOrganisationID] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationID] [int] NULL,
	[OrganisationGUID] [nvarchar](500) NULL,
	[CookieNameOrganisationRelationship] [nvarchar](50) NULL,
	[CookieNameOrganisationDescription] [nvarchar](max) NULL,
	[CookieNameID] [int] NULL,
	[CookieNameGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COOKIENAMEPRODUCT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COOKIENAMEPRODUCT](
	[CookieNameProductID] [int] IDENTITY(1,1) NOT NULL,
	[CookieNameID] [int] NULL,
	[ProductID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COOKIEPERSON]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COOKIEPERSON](
	[CookiePersonID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COOKIESECURITYCONTROL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COOKIESECURITYCONTROL](
	[CookieSecurityControlID] [int] IDENTITY(1,1) NOT NULL,
	[CookieID] [int] NULL,
	[CookieGUID] [nvarchar](500) NULL,
	[CookieSecurityControlRelationship] [nvarchar](50) NULL,
	[CookieSecurityControlDescription] [nvarchar](max) NULL,
	[SecurityControlID] [int] NULL,
	[SecurityControlGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COUNTFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COUNTFUNCTION](
	[CountFunctionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COUNTRY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COUNTRY](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[CountryGUID] [nvarchar](500) NULL,
	[CountryCode] [nchar](2) NOT NULL,
	[CountryName] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromdate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COUNTRYLAW]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COUNTRYLAW](
	[CountryLawID] [int] IDENTITY(1,1) NOT NULL,
	[CountryID] [int] NOT NULL,
	[LawID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COUNTRYLOCALE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COUNTRYLOCALE](
	[CountryLocaleID] [int] IDENTITY(1,1) NOT NULL,
	[CountryID] [int] NOT NULL,
	[LocaleID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COUNTRYTELEPHONE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COUNTRYTELEPHONE](
	[CountryTelephoneID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COUNTRYZONE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COUNTRYZONE](
	[CountryZoneID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COURSEOFACTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COURSEOFACTION](
	[CourseOfActionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COURSEOFACTIONTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COURSEOFACTIONTYPE](
	[CourseOfActionTypeID] [int] IDENTITY(1,1) NOT NULL,
	[CourseOfActionTypeGUID] [nvarchar](500) NULL,
	[CourseOfActionTypeName] [nvarchar](50) NULL,
	[CourseOfActionTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[COURSEOFLAW]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COURSEOFLAW](
	[CourseOfLawID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPE](
	[CPEID] [int] IDENTITY(1,1) NOT NULL,
	[CPEName] [nvarchar](255) NOT NULL,
	[CPETitle] [nvarchar](255) NULL,
	[NVDID] [int] NULL,
	[ModificationDate] [datetime] NULL,
	[Status] [nvarchar](50) NULL,
	[CPEDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEBANNER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEBANNER](
	[CPEBannerID] [int] IDENTITY(1,1) NOT NULL,
	[CPEID] [int] NOT NULL,
	[BannerID] [int] NOT NULL,
	[ConfidenceLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[RepositoryID] [int] NULL,
	[CreationObjectID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEBLACKLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEBLACKLIST](
	[CPEBlacklistID] [int] IDENTITY(1,1) NOT NULL,
	[CPEID] [int] NOT NULL,
	[OrganisationID] [int] NULL,
	[PersonID] [int] NULL,
	[AssetID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEFILELIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEFILELIST](
	[CPEFileListID] [int] IDENTITY(1,1) NOT NULL,
	[CPEID] [int] NULL,
	[CPEName] [nvarchar](255) NULL,
	[CPEFileListRelationship] [nvarchar](50) NULL,
	[CPEFileListDescription] [nvarchar](max) NULL,
	[FileListID] [int] NULL,
	[FileListGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEFORAPPLICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEFORAPPLICATION](
	[ApplicationCPEID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[CPEID] [int] NOT NULL,
	[CreationDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEFORASSET](
	[AssetCPEID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[CPEID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[TrustLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEFORFIXACTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEFORFIXACTION](
	[FixActionCPEID] [int] IDENTITY(1,1) NOT NULL,
	[CPEID] [int] NOT NULL,
	[FixActionID] [int] NOT NULL,
	[relationshiptype] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEFORORGANISATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEFORORGANISATION](
	[OrganisationCPEID] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[CPEID] [int] NOT NULL,
	[Usage] [nvarchar](100) NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEFORPLATFORM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEFORPLATFORM](
	[PlatformCPEID] [int] IDENTITY(1,1) NOT NULL,
	[PlatformID] [int] NOT NULL,
	[CPEID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEFORPRODUCT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEFORPRODUCT](
	[ProductCPEID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[ProductGUID] [nvarchar](500) NULL,
	[CPEID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEFORTOOL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEFORTOOL](
	[ToolCPEID] [int] NOT NULL,
	[ToolID] [int] NOT NULL,
	[ToolGUID] [nvarchar](500) NULL,
	[CPEID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEGOOGLEDORK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEGOOGLEDORK](
	[CPEGoogleDorkID] [int] IDENTITY(1,1) NOT NULL,
	[CPEID] [int] NOT NULL,
	[GoogleDorkID] [int] NOT NULL,
	[ConfidenceLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPELOGICALTEST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPELOGICALTEST](
	[CPELogicalTestID] [int] IDENTITY(1,1) NOT NULL,
	[negate] [bit] NULL,
	[OperatorEnumerationID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEPATCH]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEPATCH](
	[CPEPatchID] [int] IDENTITY(1,1) NOT NULL,
	[CPEID] [int] NULL,
	[PatchID] [int] NULL,
	[PatchGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEPORT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEPORT](
	[CPEPortID] [int] IDENTITY(1,1) NOT NULL,
	[CPEID] [int] NOT NULL,
	[PortID] [int] NOT NULL,
	[CPEPortUsage] [nvarchar](50) NULL,
	[CPEPortDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEREFERENCE](
	[CPEReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[CPEID] [int] NULL,
	[ReferenceID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPETAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPETAG](
	[CPETagID] [int] IDENTITY(1,1) NOT NULL,
	[CPEID] [int] NOT NULL,
	[TagID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidityID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPETECHNOLOGY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPETECHNOLOGY](
	[CPETechnologyID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEURI]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEURI](
	[CPEURIID] [int] IDENTITY(1,1) NOT NULL,
	[CPEID] [int] NOT NULL,
	[URIObjectID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CPEWHITELIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CPEWHITELIST](
	[CPEWhitelistID] [int] IDENTITY(1,1) NOT NULL,
	[CPEID] [int] NOT NULL,
	[OrganisationID] [int] NULL,
	[OrganisationGUID] [nvarchar](500) NULL,
	[PersonID] [int] NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[AssetID] [int] NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CREATIONOBJECT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CREATIONOBJECT](
	[CreationObjectID] [int] IDENTITY(1,1) NOT NULL,
	[CreationObjectGUID] [nvarchar](500) NULL,
	[ObjectID] [int] NULL,
	[RecordGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[OrganisationID] [int] NULL,
	[OrganisationGUID] [nvarchar](500) NULL,
	[PersonID] [int] NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[AccountID] [int] NULL,
	[AccountGUID] [nvarchar](500) NULL,
	[UserID] [int] NULL,
	[UserGUID] [nvarchar](500) NULL,
	[AssetID] [int] NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[SensorID] [int] NULL,
	[SensorGUID] [nvarchar](500) NULL,
	[ToolID] [int] NULL,
	[ToolGUID] [nvarchar](500) NULL,
	[ToolFunctionID] [int] NULL,
	[ToolFunctionGUID] [nvarchar](500) NULL,
	[ToolCodeID] [int] NULL,
	[ToolCodeGUID] [nvarchar](500) NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[suspected_malicious] [bit] NULL,
	[SuspectedMaliciousReasonID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CREATIONOBJECTHASH]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CREATIONOBJECTHASH](
	[CreationObjectHashID] [int] IDENTITY(1,1) NOT NULL,
	[CreationObjectID] [int] NOT NULL,
	[CreationObjectGUID] [nvarchar](500) NULL,
	[CreationObjectHashValue] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[CreationDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CREDENTIAL]    Script Date: 04/03/2015 19:59:09 ******/
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
	[timestamp] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidityID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CREDENTIALACCESSRECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CREDENTIALACCESSRECORD](
	[CredentialAccessRecordID] [int] IDENTITY(1,1) NOT NULL,
	[CredentialID] [int] NOT NULL,
	[AccessRecordID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationRecordID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CREDENTIALREPOSITORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CREDENTIALREPOSITORY](
	[CredentialRepositoryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CRITICALITYLEVEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CRITICALITYLEVEL](
	[CriticalityLevelID] [int] IDENTITY(1,1) NOT NULL,
	[CriticalityLevelGUID] [nvarchar](500) NULL,
	[CriticalityLevelName] [nvarchar](50) NULL,
	[CriticalityLevelDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CUSTOMOBJECT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CUSTOMOBJECT](
	[CustomObjectID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWE](
	[CWEID] [nvarchar](50) NOT NULL,
	[CWEGUID] [nvarchar](500) NULL,
	[CWEName] [nvarchar](max) NULL,
	[CWEStatus] [nvarchar](50) NULL,
	[CWEAbstraction] [nvarchar](50) NULL,
	[CWEDescriptionSummary] [nvarchar](max) NULL,
	[CWEExtendedDescription] [nvarchar](max) NULL,
	[CWECausalNature] [nvarchar](50) NULL,
	[CWEBackgroundDetails] [nvarchar](max) NULL,
	[Maintenance_Notes] [nvarchar](max) NULL,
	[Relationship_Notes] [nvarchar](max) NULL,
	[Terminology_Notes] [nvarchar](max) NULL,
	[White_Box_Definitions] [nvarchar](max) NULL,
	[Platform_Notes] [nvarchar](max) NULL,
	[Other_Notes] [nvarchar](max) NULL,
	[Research_Gaps] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[CWEURL] [nvarchar](500) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ImportanceID] [int] NULL,
	[CriticalityLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEAFFECTEDFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEAFFECTEDFUNCTION](
	[CWEAffectedFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[FunctionID] [int] NOT NULL,
	[FunctionGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEAFFECTEDRESOURCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEAFFECTEDRESOURCE](
	[CWEAffectedResourceID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[AffectedResourceID] [int] NOT NULL,
	[AffectedResourceGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEALTERNATETERM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEALTERNATETERM](
	[CWEAlternateTermID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[AlternateTerm] [nvarchar](max) NOT NULL,
	[AlternateTermDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEALTERNATETERMTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEALTERNATETERMTAG](
	[CWEAlternateTermTagID] [int] IDENTITY(1,1) NOT NULL,
	[CWEAlternateTermID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEARCHITECTURALPARADIGM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEARCHITECTURALPARADIGM](
	[CWEArchitecturalParadigmID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NULL,
	[ArchitecturalParadigmID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEATTACKCONSEQUENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEATTACKCONSEQUENCE](
	[CWEAttackConsequenceID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[CWEAttackConsequenceOrder] [int] NOT NULL,
	[Consequence_Note] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEATTACKCONSEQUENCESCOPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEATTACKCONSEQUENCESCOPE](
	[CWEAttackConsequenceScopeID] [int] IDENTITY(1,1) NOT NULL,
	[CWEAttackConsequenceID] [int] NULL,
	[AttackScopeID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEATTACKCONSEQUENCETAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEATTACKCONSEQUENCETAG](
	[CWEAttackConsequenceTagID] [int] IDENTITY(1,1) NOT NULL,
	[CWEAttackConsequenceID] [int] NULL,
	[TagID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEATTACKTECHNICALIMPACT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEATTACKTECHNICALIMPACT](
	[CWEAttackTechnicalImpactID] [int] IDENTITY(1,1) NOT NULL,
	[CWEAttackConsequenceID] [int] NULL,
	[AttackTechnicalImpactID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEDEMONSTRATIVEEXAMPLE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEDEMONSTRATIVEEXAMPLE](
	[CWEDemonstrativeExampleID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[DemonstrativeExampleID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEDESCRIPTION](
	[CWEDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEDETECTIONMETHOD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEDETECTIONMETHOD](
	[CWEDetectionMethodID] [int] IDENTITY(1,1) NOT NULL,
	[CWEDetectionMethodGUID] [nvarchar](500) NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[CWEGUID] [nvarchar](500) NULL,
	[DetectionMethodID] [int] NOT NULL,
	[DetectionMethodGUID] [nvarchar](500) NULL,
	[CWEDetectionMethodDescription] [nvarchar](max) NULL,
	[CWEDetectionMethodEffectiveness] [nvarchar](max) NULL,
	[CWEDetectionMethodEffectivenessNotes] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEEXPLOITATIONFACTOR]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEEXPLOITATIONFACTOR](
	[CWEExploitationFactorID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NULL,
	[ExploitationFactorID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEFOROWASPTOP10]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEFOROWASPTOP10](
	[CWEOWASPTOP10ID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[OWASPTOP10ID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[Mapping_Fit] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEFUNCTIONALAREA]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEFUNCTIONALAREA](
	[CWEFunctionalAreaID] [int] IDENTITY(1,1) NOT NULL,
	[CWEFunctionalAreaGUID] [nvarchar](500) NULL,
	[CWEID] [nvarchar](50) NULL,
	[FunctionalAreaID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWELANGUAGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWELANGUAGE](
	[CWELanguageID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[LanguageID] [int] NOT NULL,
	[LanguageGUID] [nvarchar](500) NULL,
	[Prevalence] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWELANGUAGECLASS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWELANGUAGECLASS](
	[CWELanguageClassID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[LanguageClassID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEMODEOFINTRODUCTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEMODEOFINTRODUCTION](
	[CWEModeOfIntroductionID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NULL,
	[ModeOfIntroductionDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEMODEOFINTRODUCTIONTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEMODEOFINTRODUCTIONTAG](
	[CWEModeOfIntroductionTagID] [int] IDENTITY(1,1) NOT NULL,
	[CWEModeOfIntroductionID] [int] NULL,
	[TagID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEORDINALITY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEORDINALITY](
	[CWEOrdinalityID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NULL,
	[WeaknessOrdinality] [nvarchar](50) NULL,
	[Ordinality_Description] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEOS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEOS](
	[CWEOSID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NULL,
	[Operating_System_Name] [nvarchar](50) NULL,
	[Prevalence] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEOSCLASS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEOSCLASS](
	[CWEOSClassID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NULL,
	[OSClassID] [int] NULL,
	[Prevalence] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEREFERENCE](
	[CWEReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[Reference_Section] [nvarchar](500) NULL,
	[LocalReferenceID] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWERELATIONSHIPCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWERELATIONSHIPCATEGORY](
	[CWERelationshipCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[RelationshipNature] [nvarchar](50) NOT NULL,
	[RelationshipTargetCWEID] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWERELEVANTPROPERTY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWERELEVANTPROPERTY](
	[CWERelevantPropertyID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NULL,
	[Relevant_Property] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWEREPOSITORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWEREPOSITORY](
	[CWERepositoryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWERESEARCHGAP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWERESEARCHGAP](
	[CWEResearchGapID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NULL,
	[ResearchGapText] [nvarchar](max) NULL,
	[ResearchGapTextClean] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWETAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWETAG](
	[CWETagID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWETAXONOMYNODE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWETAXONOMYNODE](
	[CWETaxonomyNodeID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[TaxonomyNodeID] [int] NOT NULL,
	[Mapping_Fit] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWETECHNOLOGY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWETECHNOLOGY](
	[CWETechnologyID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NULL,
	[TechnologyID] [int] NULL,
	[Prevalence] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWETHEORETICALNOTE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWETHEORETICALNOTE](
	[CWETheoreticalNoteID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[TheoreticalNoteID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWETIMEOFINTRODUCTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWETIMEOFINTRODUCTION](
	[CWETimeOfIntroductionID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[PhaseID] [int] NULL,
	[IntroductoryPhase] [nvarchar](100) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CWETOP25]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CWETOP25](
	[CWETOP25ID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[YearTop25] [int] NOT NULL,
	[Rank] [int] NOT NULL,
	[Score] [float] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATACLASSIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATACLASSIFICATION](
	[DataClassificationID] [int] IDENTITY(1,1) NOT NULL,
	[InformationTypeID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATADICTIONARY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATADICTIONARY](
	[DataDictionaryID] [int] IDENTITY(1,1) NOT NULL,
	[DictionaryID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATAEXFILTRATIONPROPERTIES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATAEXFILTRATIONPROPERTIES](
	[DataExfiltrationPropertiesID] [int] IDENTITY(1,1) NOT NULL,
	[DataExfiltrationPropertiesName] [nvarchar](50) NULL,
	[DataExfiltrationPropertiesDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATAEXFILTRATIONSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATAEXFILTRATIONSTRATEGICOBJECTIVE](
	[DataExfiltrationStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[DataExfiltrationStrategicObjectiveName] [nvarchar](50) NULL,
	[DataExfiltrationStrategicObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATAEXFILTRATIONTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATAEXFILTRATIONTACTICALOBJECTIVE](
	[DataExfiltrationTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[DataExfiltrationTacticalObjectiveName] [nvarchar](50) NULL,
	[DataExfiltrationTacticalObjectiveDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATAFEED]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATAFEED](
	[DataFeedID] [int] IDENTITY(1,1) NOT NULL,
	[FeedID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATAFORMAT]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[DATAMODEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATAMODEL](
	[DataModelID] [int] IDENTITY(1,1) NOT NULL,
	[ModelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATASEGMENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATASEGMENT](
	[DataSegmentID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATASIZEUNIT]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[DATATHEFTPROPERTIES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATATHEFTPROPERTIES](
	[DataTheftPropertiesID] [int] IDENTITY(1,1) NOT NULL,
	[DataTheftPropertiesName] [nvarchar](50) NULL,
	[DataTheftPropertiesDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATATHEFTSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATATHEFTSTRATEGICOBJECTIVE](
	[DataTheftStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[DataTheftStrategicObjectiveName] [nvarchar](50) NULL,
	[DataTheftStrategicObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATATHEFTTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATATHEFTTACTICALOBJECTIVE](
	[DataTheftTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[DataTheftTacticalObjectiveName] [nvarchar](50) NULL,
	[DataTheftTacticalObjectiveDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATATRANSFER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATATRANSFER](
	[DataTransferID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATATRANSFORMATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATATRANSFORMATION](
	[DataTransformationID] [int] IDENTITY(1,1) NOT NULL,
	[TransformationID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATATYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DATATYPE](
	[DataTypeID] [int] IDENTITY(1,1) NOT NULL,
	[DataTypeName] [nvarchar](50) NOT NULL,
	[DataTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DATETIMEFORMAT]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[DEBUGGINGACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DEBUGGINGACTIONNAME](
	[DebuggingActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[DebuggingActionNameName] [nvarchar](150) NOT NULL,
	[DebuggingActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEFENSETOOL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DEFENSETOOL](
	[DefenseToolID] [int] IDENTITY(1,1) NOT NULL,
	[DefenseToolGUID] [nvarchar](500) NULL,
	[ToolID] [int] NULL,
	[DefenseToolName] [nvarchar](50) NULL,
	[DefenseToolDescription] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[ReliabilityID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEFENSETOOLTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DEFENSETOOLTYPE](
	[DefenseToolTypeID] [int] IDENTITY(1,1) NOT NULL,
	[DefenseToolTypeName] [nvarchar](100) NOT NULL,
	[DefenseToolTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEMONSTRATIVEEXAMPLE]    Script Date: 04/03/2015 19:59:09 ******/
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
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[LanguageID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEMONSTRATIVEEXAMPLECODE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DEMONSTRATIVEEXAMPLECODE](
	[DemonstrativeExampleCodeID] [int] IDENTITY(1,1) NOT NULL,
	[DemonstrativeExampleID] [int] NOT NULL,
	[CodeID] [int] NOT NULL,
	[Block_Nature] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEMONSTRATIVEEXAMPLEREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DEMONSTRATIVEEXAMPLEREFERENCE](
	[DemonstrativeExampleReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[DemonstrativeExampleID] [int] NULL,
	[ReferenceID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEMONSTRATIVEEXAMPLEVULNERABILITY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DEMONSTRATIVEEXAMPLEVULNERABILITY](
	[DemonstrativeExampleVulnerabilityID] [int] IDENTITY(1,1) NOT NULL,
	[DemonstrativeExampleID] [int] NULL,
	[VulnerabilityID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DESCRIPTION](
	[DescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[DescriptionGUID] [nvarchar](500) NULL,
	[DescriptionText] [nvarchar](max) NULL,
	[LocaleID] [int] NULL,
	[VersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidentialityLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DESCRIPTIONCHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DESCRIPTIONCHANGERECORD](
	[DescriptionChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DESCRIPTIONREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DESCRIPTIONREFERENCE](
	[DescriptionReferenceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DESCRIPTIONTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DESCRIPTIONTAG](
	[DescriptionTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DESTRUCTIONPROPERTIES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DESTRUCTIONPROPERTIES](
	[DestructionPropertiesID] [int] IDENTITY(1,1) NOT NULL,
	[DestructionPropertiesName] [nvarchar](50) NULL,
	[DestructionPropertiesDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DESTRUCTIONSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DESTRUCTIONSTRATEGICOBJECTIVE](
	[DestructionStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[DestructionStrategicObjectiveName] [nvarchar](50) NULL,
	[DestructionStrategicObjectiveDestruction] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DESTRUCTIONTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DESTRUCTIONTACTICALOBJECTIVE](
	[DestructionTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[DestructionTacticalObjectiveName] [nvarchar](50) NULL,
	[DestructionTacticalObjectiveDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DETECTABILITY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DETECTABILITY](
	[DetectabilityID] [int] IDENTITY(1,1) NOT NULL,
	[DetectabilityName] [nvarchar](50) NOT NULL,
	[DetectabilityDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DETECTIONMETHOD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DETECTIONMETHOD](
	[DetectionMethodID] [int] IDENTITY(1,1) NOT NULL,
	[DetectionMethodGUID] [nvarchar](500) NULL,
	[MethodID] [int] NULL,
	[DetectionMethodVocabularyID] [nvarchar](50) NULL,
	[DetectionMethodName] [nvarchar](100) NOT NULL,
	[DetectionMethodDescription] [nvarchar](max) NULL,
	[DetectionMethodEffectiveness] [nvarchar](50) NULL,
	[DetectionMethodEffectivenessNotes] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEVICE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DEVICE](
	[DeviceID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceGUID] [nvarchar](500) NULL,
	[Device_Type] [nvarchar](250) NOT NULL,
	[Manufacturer] [nvarchar](250) NULL,
	[OrganisationID] [int] NULL,
	[Model] [nvarchar](250) NULL,
	[Firmware_Version] [nvarchar](50) NULL,
	[CPEID] [int] NULL,
	[CPEName] [nvarchar](255) NULL,
	[Serial_Number] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[ClockSpeedFrequency] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEVICEBLACKLIST]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[DEVICECOMPONENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DEVICECOMPONENT](
	[DeviceComponentID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceComponentGUID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEVICEDRIVERACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DEVICEDRIVERACTIONNAME](
	[DeviceDriverActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceDriverActionNameName] [nvarchar](150) NOT NULL,
	[DeviceDriverActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEVICETYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DEVICETYPE](
	[DeviceTypeID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceTypeGUID] [nvarchar](500) NULL,
	[DeviceTypeName] [nvarchar](50) NULL,
	[DeviceTypeDescription] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DEVICEWHITELIST]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[DICTIONARY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DICTIONARY](
	[DictionaryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DIGITALSIGNATUREINFO]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DIGITALSIGNATUREINFO](
	[DigitalSignatureInfoID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DIGITALSIGNATURES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DIGITALSIGNATURES](
	[DigitalSignaturesID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DIRECTORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DIRECTORY](
	[DirectoryID] [int] IDENTITY(1,1) NOT NULL,
	[DirectoryGUID] [nvarchar](500) NULL,
	[DirectoryPathname] [nvarchar](500) NULL,
	[DirectoryDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DIRECTORYACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DIRECTORYACTIONNAME](
	[DirectoryActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[DirectoryActionNameName] [nvarchar](150) NOT NULL,
	[DirectoryActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DIRECTORYLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DIRECTORYLIST](
	[DirectoryListID] [int] IDENTITY(1,1) NOT NULL,
	[DirectoryListGUID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DISCOVERYMETHOD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DISCOVERYMETHOD](
	[DiscoveryMethodID] [int] IDENTITY(1,1) NOT NULL,
	[DiscoveryMethodGUID] [nvarchar](500) NULL,
	[DiscoveryMethodName] [nvarchar](150) NULL,
	[MeasureSourceID] [int] NULL,
	[DiscoveryMethodDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DISK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DISK](
	[DiskID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DISKACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DISKACTIONNAME](
	[DiskActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[DiskActionNameName] [nvarchar](150) NULL,
	[DiskActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DISKPARTITION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DISKPARTITION](
	[DiskPartitionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DNSACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DNSACTIONNAME](
	[DNSActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[DNSActionNameName] [nvarchar](150) NOT NULL,
	[DNSActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DNSCACHE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DNSCACHE](
	[DNSCacheID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DNSQUERY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DNSQUERY](
	[DNSQueryID] [int] IDENTITY(1,1) NOT NULL,
	[DNDQueryGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DNSRECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DNSRECORD](
	[DNSRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOCUMENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOCUMENT](
	[DocumentID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOCUMENTCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOCUMENTCATEGORY](
	[DocumentCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentID] [int] NULL,
	[CategoryID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOCUMENTCLASSIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOCUMENTCLASSIFICATION](
	[DocumentClassificationID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentID] [int] NOT NULL,
	[DocumentGUID] [nvarchar](500) NULL,
	[ClassificationID] [int] NOT NULL,
	[ClassificationGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOCUMENTTITLE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOCUMENTTITLE](
	[DocumentTitleID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentID] [int] NOT NULL,
	[DocumentGUID] [nvarchar](500) NULL,
	[TitleID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOCUMENTVERSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOCUMENTVERSION](
	[DocumentVersionID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentID] [int] NOT NULL,
	[DocumentGUID] [nvarchar](500) NULL,
	[VersionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOCXMLDOCUMENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOCXMLDOCUMENT](
	[DocXMLDocumentID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOMAIN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOMAIN](
	[DomainID] [int] IDENTITY(1,1) NOT NULL,
	[DomainGUID] [nvarchar](500) NULL,
	[DomainName] [nvarchar](500) NOT NULL,
	[DomainDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOMAINBLACKLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOMAINBLACKLIST](
	[DomainBlacklistID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOMAINEMAILADDRESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOMAINEMAILADDRESS](
	[DomainEmailAddressID] [int] IDENTITY(1,1) NOT NULL,
	[DomainID] [int] NOT NULL,
	[EmailAddressID] [int] NOT NULL,
	[emailaddress] [nvarchar](100) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOMAINNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOMAINNAME](
	[DomainNameID] [int] IDENTITY(1,1) NOT NULL,
	[DomainNameValue] [nvarchar](500) NULL,
	[DomainNameTypeID] [int] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOMAINNAMEBLACKLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOMAINNAMEBLACKLIST](
	[DomainNameBlacklistID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOMAINNAMECHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOMAINNAMECHANGERECORD](
	[DomainNameChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOMAINNAMEREPUTATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOMAINNAMEREPUTATION](
	[DomainNameReputationID] [int] IDENTITY(1,1) NOT NULL,
	[DomainNameID] [int] NULL,
	[ReputationID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOMAINNAMETYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOMAINNAMETYPE](
	[DomainNameTypeID] [int] IDENTITY(1,1) NOT NULL,
	[DomainNameTypeValue] [nvarchar](50) NULL,
	[DomainNameTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[VocabularyGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOMAINORGANISATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOMAINORGANISATION](
	[DomainOrganisationID] [int] IDENTITY(1,1) NOT NULL,
	[DomainID] [int] NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[DomainOrganisationDescription] [nvarchar](max) NULL,
	[RelationshipTypeID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOMAINPERSON]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOMAINPERSON](
	[DomainPersonID] [int] IDENTITY(1,1) NOT NULL,
	[DomainID] [int] NOT NULL,
	[PersonID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOMAINTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOMAINTYPE](
	[DomainTypeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOMAINTYPEENUM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOMAINTYPEENUM](
	[DomainTypeEnumID] [int] IDENTITY(1,1) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOMAINWHITELIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOMAINWHITELIST](
	[DomainWhitelistID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DOWNTIME]    Script Date: 04/03/2015 19:59:09 ******/
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
	[DowntimePlanned] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [nchar](10) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[DPE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[EDGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EDGE](
	[EdgeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EFFECTIVENESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EFFECTIVENESS](
	[EffectivenessID] [int] IDENTITY(1,1) NOT NULL,
	[EffectivenessGUID] [nvarchar](500) NULL,
	[EffectivenessName] [nvarchar](100) NOT NULL,
	[EffectivenessDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EFFECTTYPE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[EMAIL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAIL](
	[EmailID] [int] IDENTITY(1,1) NOT NULL,
	[emailaddress] [nvarchar](100) NOT NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILADDRESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILADDRESS](
	[EmailAddressID] [int] NOT NULL,
	[EmailAddressGUID] [nvarchar](500) NULL,
	[EmailID] [int] NULL,
	[emailaddress] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[suspected_malicious] [bit] NULL,
	[SuspectedMaliciousReasonID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILFORORGANISATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILFORORGANISATION](
	[emailaddress] [nvarchar](100) NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[TrustLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILFORPERSON]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[EMAILHEADER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILHEADER](
	[EmailHeaderID] [int] IDENTITY(1,1) NOT NULL,
	[EmailHeaderGUID] [nvarchar](500) NULL,
	[HeaderID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[Received_Lines] [int] NULL,
	[EmailTo] [int] NULL,
	[EmailCC] [int] NULL,
	[EmailBCC] [int] NULL,
	[EmailFrom] [int] NULL,
	[EmailSubject] [nvarchar](500) NULL,
	[In_Reply_To] [nvarchar](50) NULL,
	[DateSent] [datetimeoffset](7) NULL,
	[Message_ID] [nvarchar](50) NULL,
	[Sender] [int] NULL,
	[Reply_To] [int] NULL,
	[Errors_To] [nvarchar](100) NULL,
	[Boundary] [nvarchar](50) NULL,
	[Content_Type] [nvarchar](50) NULL,
	[MIMEID] [int] NULL,
	[MIME_Version] [nvarchar](50) NULL,
	[Precedence] [nvarchar](50) NULL,
	[User_Agent] [nvarchar](500) NULL,
	[UserAgentID] [int] NULL,
	[UserAgentGUID] [nvarchar](500) NULL,
	[X_Mailer] [nvarchar](100) NULL,
	[X_Originating_IP] [int] NULL,
	[X_Priority] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[suspected_malicious] [bit] NULL,
	[SuspectedMaliciousReasonID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILHEADERTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILHEADERTAG](
	[EmailHeaderTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILMESSAGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILMESSAGE](
	[EmailMessageID] [int] IDENTITY(1,1) NOT NULL,
	[EmailMessageGUID] [nvarchar](500) NULL,
	[MessageID] [int] NULL,
	[EmailMessageIsEncrypted] [bit] NULL,
	[isEncrypted] [bit] NULL,
	[Email_Server] [nvarchar](50) NULL,
	[CPEID] [nvarchar](255) NULL,
	[AssetEmailServerID] [int] NULL,
	[AssetEmailServerGUID] [nvarchar](500) NULL,
	[AssetSourceID] [int] NULL,
	[AssetSourceGUID] [nvarchar](500) NULL,
	[AssetDestinationID] [int] NULL,
	[AssetDestinationGUID] [nvarchar](500) NULL,
	[Raw_Body] [nvarchar](max) NULL,
	[suspected_malicious] [bit] NULL,
	[SuspectedMaliciousReasonID] [int] NULL,
	[ImportanceID] [int] NULL,
	[Raw_Header] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILMESSAGEATTACHMENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILMESSAGEATTACHMENT](
	[EmailMessageAttachmentID] [int] IDENTITY(1,1) NOT NULL,
	[EmailMessageID] [int] NOT NULL,
	[EmailMessageGUID] [nvarchar](500) NULL,
	[AttachmentID] [int] NOT NULL,
	[AttachmentGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILMESSAGECLASSIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILMESSAGECLASSIFICATION](
	[EmailMessageClassificationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILMESSAGELINK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILMESSAGELINK](
	[EmailMessageLinkID] [int] IDENTITY(1,1) NOT NULL,
	[EmailMessageID] [int] NOT NULL,
	[LinkID] [int] NOT NULL,
	[suspected_malicious] [bit] NULL,
	[SuspectedMaliciousReasonID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILMESSAGERESTRICTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILMESSAGERESTRICTION](
	[EmailMessageRestrictionID] [int] IDENTITY(1,1) NOT NULL,
	[EmailMessageID] [int] NOT NULL,
	[RestrictionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILMESSAGETAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILMESSAGETAG](
	[EmailMessageTagID] [int] IDENTITY(1,1) NOT NULL,
	[EmailMessageID] [int] NOT NULL,
	[TagID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILRECEIVEDLINELIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILRECEIVEDLINELIST](
	[EmailReceivedLineListID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILRECIPIENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILRECIPIENT](
	[EmailRecipientID] [int] IDENTITY(1,1) NOT NULL,
	[EmailAddressID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILRECIPIENTS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILRECIPIENTS](
	[EmailRecipientsID] [int] IDENTITY(1,1) NOT NULL,
	[EmailRecipientsGUID] [nvarchar](500) NULL,
	[GroupID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EMAILRECIPIENTSLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMAILRECIPIENTSLIST](
	[EmailRecipientsListID] [int] IDENTITY(1,1) NOT NULL,
	[EmailRecipientsID] [int] NOT NULL,
	[EmailRecipientsGUID] [nvarchar](500) NULL,
	[EmailRecipientID] [int] NOT NULL,
	[EmailRecipientGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENCODING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENCODING](
	[EncodingID] [int] IDENTITY(1,1) NOT NULL,
	[algorithm] [nvarchar](50) NOT NULL,
	[EncodingAlgorithmID] [int] NULL,
	[EncodingDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENCODINGALGORITHM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENCODINGALGORITHM](
	[EncodingAlgorithmID] [int] IDENTITY(1,1) NOT NULL,
	[AlgorithmID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENCRYPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENCRYPTION](
	[EncryptionID] [int] IDENTITY(1,1) NOT NULL,
	[encryption_mechanism] [nvarchar](50) NOT NULL,
	[EncryptionMechanismID] [int] NULL,
	[encryption_mechanism_ref] [nvarchar](250) NULL,
	[EncryptionDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENCRYPTIONKEY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENCRYPTIONKEY](
	[EncryptionKeyID] [int] IDENTITY(1,1) NOT NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENCRYPTIONMECHANISM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENCRYPTIONMECHANISM](
	[EncryptionMechanismID] [int] IDENTITY(1,1) NOT NULL,
	[MechanismID] [int] NULL,
	[EncryptionMechanismName] [nvarchar](250) NULL,
	[EncryptionMechanismDescription] [nvarchar](max) NULL,
	[TrustLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENCRYPTIONREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENCRYPTIONREFERENCE](
	[EncryptionReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[EncryptionID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENDFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[ENDIANNESSTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENDIANNESSTYPE](
	[EndiannessTypeID] [int] IDENTITY(1,1) NOT NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENDPOINT]    Script Date: 04/03/2015 19:59:09 ******/
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
	[CPEName] [nvarchar](255) NULL,
	[SessionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENGINE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[ENTITY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENTITY](
	[EntityID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENTITYDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENTITYDESCRIPTION](
	[EntityDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[EntityID] [int] NOT NULL,
	[DescriptionID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENTITYNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENTITYNAME](
	[EntityNameID] [int] IDENTITY(1,1) NOT NULL,
	[EntityID] [int] NOT NULL,
	[NameID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENTITYRESTRICTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENTITYRESTRICTION](
	[EntityRestrictionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENTITYTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENTITYTYPE](
	[EntityTypeID] [int] IDENTITY(1,1) NOT NULL,
	[EntityID] [int] NOT NULL,
	[TypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENTRYPOINT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENTRYPOINT](
	[EntryPointID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENTRYVARIABLE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENTRYVARIABLE](
	[EntryVariableID] [int] IDENTITY(1,1) NOT NULL,
	[VariableID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENUMERATIONVERSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENUMERATIONVERSION](
	[EnumerationVersionID] [int] IDENTITY(1,1) NOT NULL,
	[EnumerationName] [nvarchar](250) NULL,
	[VersionID] [int] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ENVIRONMENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ENVIRONMENT](
	[EnvironmentID] [int] IDENTITY(1,1) NOT NULL,
	[CapecEnvironmentID] [nvarchar](50) NULL,
	[EnvironmentTitle] [nvarchar](150) NOT NULL,
	[EnvironmentDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ESCAPEREGEXFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ESCAPEREGEXFUNCTION](
	[EscapeRegexFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[OVALComponentGroupID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVALUATIONMETHOD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVALUATIONMETHOD](
	[EvaluationMethodID] [int] IDENTITY(1,1) NOT NULL,
	[MethodID] [int] NULL,
	[VocabularyID] [int] NULL,
	[ReliabilityID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENT](
	[EventID] [int] IDENTITY(1,1) NOT NULL,
	[EventGUID] [nvarchar](500) NULL,
	[EventName] [nvarchar](50) NULL,
	[EventTypeID] [int] NULL,
	[start_datetime] [datetimeoffset](7) NULL,
	[stop_datetime] [datetimeoffset](7) NULL,
	[AnomalyEvent] [bit] NULL,
	[AnomalyDescription] [nvarchar](max) NULL,
	[AuditRecordEvent] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTCOLLECTIONMETHOD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTCOLLECTIONMETHOD](
	[EventCollectionMethodID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NOT NULL,
	[EventGUID] [nvarchar](500) NULL,
	[CollectionMethodID] [int] NOT NULL,
	[CollectionMethodGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[AssetID] [int] NULL,
	[DeviceID] [int] NULL,
	[ProductID] [int] NULL,
	[CPEID] [int] NULL,
	[CPEName] [nvarchar](255) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTCOMMENT]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[EVENTCOMMENTFOREVENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTCOMMENTFOREVENT](
	[EventEventCommentID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NOT NULL,
	[EventCommentID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTENDPOINT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTENDPOINT](
	[EndPointEventID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NOT NULL,
	[EndPointID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTFILTER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTFILTER](
	[EventFilterID] [int] IDENTITY(1,1) NOT NULL,
	[EventFilterContent] [nvarchar](max) NULL,
	[EventFilterDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTFORASSET](
	[AssetEventID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[relationshiptype] [nvarchar](50) NULL,
	[relationshipscope] [nvarchar](50) NULL,
	[EventID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTFOREVENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTFOREVENT](
	[EventForEventID] [int] IDENTITY(1,1) NOT NULL,
	[EventRefID] [int] NOT NULL,
	[relationshiptype] [nvarchar](50) NULL,
	[relationshipscope] [nvarchar](50) NULL,
	[EventSubjectID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTFORINCIDENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTFORINCIDENT](
	[IncidentEventID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NOT NULL,
	[IncidentID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTPROPERTY]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[EVENTPROPERTYADDRESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTPROPERTYADDRESS](
	[EventPropertyAddressID] [int] IDENTITY(1,1) NOT NULL,
	[EventPropertyID] [int] NOT NULL,
	[AddressID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTPROPERTYFOREVENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTPROPERTYFOREVENT](
	[EventEventPropertyID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NOT NULL,
	[EventPropertyID] [int] NOT NULL,
	[EventPropertyValue] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTSIGNATURE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTSIGNATURE](
	[EventSignatureID] [int] IDENTITY(1,1) NOT NULL,
	[EventSignatureGUID] [nvarchar](500) NULL,
	[EventID] [int] NOT NULL,
	[EventGUID] [nvarchar](500) NULL,
	[SignatureID] [int] NOT NULL,
	[SignatureGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[total_packets_collected] [int] NULL,
	[total_bytes_collected] [int] NULL,
	[data_flow_direction] [nvarchar](50) NULL,
	[connection_start_datetime] [datetimeoffset](7) NULL,
	[connection_end_datetime] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTSUPPRESSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTSUPPRESSION](
	[EventSuppressionID] [int] IDENTITY(1,1) NOT NULL,
	[EventSuppressionContent] [nvarchar](max) NULL,
	[EventSuppressionDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVENTTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVENTTYPE](
	[EventTypeID] [int] IDENTITY(1,1) NOT NULL,
	[EventTypeName] [nvarchar](150) NOT NULL,
	[EventTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVIDENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVIDENCE](
	[EvidenceID] [int] IDENTITY(1,1) NOT NULL,
	[EvidenceGUID] [nvarchar](500) NULL,
	[EvidenceName] [nvarchar](50) NULL,
	[EvidenceDescription] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[ConfidentialityLevelID] [int] NULL,
	[ImportanceID] [int] NULL,
	[SourceID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ReliabilityID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVIDENCEACCESSRECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVIDENCEACCESSRECORD](
	[EvidenceAccessRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVIDENCEACL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVIDENCEACL](
	[EvidenceACLID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVIDENCECATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVIDENCECATEGORY](
	[EvidenceCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[EvidenceCategoryGUID] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[EvidenceCategoryName] [nvarchar](50) NULL,
	[EvidenceCategoryDescription] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[ReliabilityID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EVIDENCERESTRICTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EVIDENCERESTRICTION](
	[EvidenceRestrictionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXCELFILE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXCELFILE](
	[ExcelFileID] [int] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXIFTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXIFTAG](
	[EXIFTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXISTENCEENUMERATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXISTENCEENUMERATION](
	[ExistenceEnumerationID] [int] IDENTITY(1,1) NOT NULL,
	[ExistenceValue] [nvarchar](50) NOT NULL,
	[ExistenceDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOIT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOIT](
	[ExploitID] [int] IDENTITY(1,1) NOT NULL,
	[ExploitGUID] [nvarchar](500) NULL,
	[ExploitReferential] [nvarchar](50) NULL,
	[ExploitRefID] [nvarchar](250) NULL,
	[SourceID] [int] NULL,
	[SourceGUID] [nvarchar](500) NULL,
	[ExploitName] [nvarchar](250) NULL,
	[ExploitLocation] [nvarchar](max) NULL,
	[Date] [date] NULL,
	[Verification] [bit] NULL,
	[Platform] [nvarchar](50) NULL,
	[Author] [nvarchar](250) NULL,
	[AuthorID] [int] NULL,
	[PersonID] [int] NULL,
	[RPORT] [int] NULL,
	[ExploitDescription] [nvarchar](max) NULL,
	[ExploitType] [nchar](50) NULL,
	[CodeID] [int] NULL,
	[ExploitCode] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ReliabilityID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[LastCheckDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITABILITY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITABILITY](
	[ExploitabilityID] [int] IDENTITY(1,1) NOT NULL,
	[ExploitabilityLevel] [nvarchar](50) NOT NULL,
	[ExploitabilityDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITACCESSRECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITACCESSRECORD](
	[ExploitAccessRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITATIONFACTOR]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITATIONFACTOR](
	[ExploitationFactorID] [int] IDENTITY(1,1) NOT NULL,
	[ExploitationFactorGUID] [nvarchar](500) NULL,
	[ExploitationFactorName] [nvarchar](250) NULL,
	[ExploitationFactorDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITAUTHOR]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITAUTHOR](
	[ExploitAuthorID] [int] IDENTITY(1,1) NOT NULL,
	[ExploitID] [int] NOT NULL,
	[ExploitGUID] [nvarchar](500) NULL,
	[AuthorID] [int] NOT NULL,
	[AuthorGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITCATEGORY](
	[ExploitCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITCHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITCHANGERECORD](
	[ExploitChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITDESCRIPTION](
	[ExploitDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITFILE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITFILE](
	[ExploitFileID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITFORCPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITFORCPE](
	[CPEExploitID] [int] IDENTITY(1,1) NOT NULL,
	[CPEExploitGUID] [nvarchar](500) NULL,
	[CPEID] [int] NULL,
	[CPEName] [nvarchar](255) NULL,
	[ExploitID] [int] NOT NULL,
	[ExploitGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ExploitCPEName] [nvarchar](250) NULL,
	[ExploitCPEDescription] [nvarchar](max) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITFORFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITFORFUNCTION](
	[ExploitFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[ExploitFunctionGUID] [nvarchar](500) NULL,
	[ExploitID] [int] NOT NULL,
	[ExploitGUID] [nvarchar](500) NULL,
	[ExploitFunctionRelationship] [nvarchar](50) NULL,
	[FunctionID] [int] NOT NULL,
	[FunctionGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITFORREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITFORREFERENCE](
	[ExploitReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[ExploitReferenceGUID] [nvarchar](500) NULL,
	[ReferenceID] [int] NOT NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[ExploitID] [int] NOT NULL,
	[ExploitGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITFORTECHNOLOGY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITFORTECHNOLOGY](
	[ExploitTechnologyID] [int] IDENTITY(1,1) NOT NULL,
	[TechnologyID] [int] NULL,
	[ExploitID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITFORTHREATACTORTTP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITFORTHREATACTORTTP](
	[ThreatActorTTPExploitID] [int] IDENTITY(1,1) NOT NULL,
	[ExploitID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITFORURI]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITFORURI](
	[ExploitURIID] [int] IDENTITY(1,1) NOT NULL,
	[URIObjectID] [int] NULL,
	[ExploitID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITFORVULNERABILITY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITFORVULNERABILITY](
	[VulnerabilityExploitID] [int] IDENTITY(1,1) NOT NULL,
	[VulnerabilityExploitGUID] [nvarchar](500) NULL,
	[VulnerabilityExploitDescription] [nvarchar](max) NULL,
	[ExploitID] [int] NOT NULL,
	[ExploitGUID] [nvarchar](500) NULL,
	[VulnerabilityID] [int] NOT NULL,
	[VulnerabilityGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[RepositoryID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITLANGUAGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITLANGUAGE](
	[ExploitLanguageID] [int] IDENTITY(1,1) NOT NULL,
	[ExploitID] [int] NULL,
	[ExploitGUID] [nvarchar](500) NULL,
	[LanguageID] [int] NULL,
	[LanguageGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITLIKELIHOOD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITLIKELIHOOD](
	[ExploitLikelihoodID] [int] IDENTITY(1,1) NOT NULL,
	[Likelihood] [nvarchar](50) NOT NULL,
	[LikelihoodDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITLIKELIHOODFORATTACKPATTERN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITLIKELIHOODFORATTACKPATTERN](
	[AttackPatternExploitLikelihoodID] [int] IDENTITY(1,1) NOT NULL,
	[ExploitLikelihoodID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[Explanation] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITLIKELIHOODFORCWE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITLIKELIHOODFORCWE](
	[ExploitLikelihoodForCWEID] [int] IDENTITY(1,1) NOT NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[ExploitLikelihoodID] [int] NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[RepositoryID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITOSINSTRUCTIONMEMORYADDRESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITOSINSTRUCTIONMEMORYADDRESS](
	[ExploitOSInstructionMemoryAddressID] [int] IDENTITY(1,1) NOT NULL,
	[ExploitID] [int] NOT NULL,
	[OSInstructionMemoryAddressID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITPARAMETER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITPARAMETER](
	[ExploitParameterID] [int] IDENTITY(1,1) NOT NULL,
	[ExploitParameterName] [nvarchar](100) NOT NULL,
	[DefaultValue] [nvarchar](500) NULL,
	[ExploitParameterDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITPARAMETERFOREXPLOIT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITPARAMETERFOREXPLOIT](
	[ExploitParametersID] [int] IDENTITY(1,1) NOT NULL,
	[ExploitID] [int] NOT NULL,
	[ExploitParameterID] [int] NOT NULL,
	[OrderRank] [int] NULL,
	[DefaultValue] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITPLATFORM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITPLATFORM](
	[ExploitPlatformID] [int] IDENTITY(1,1) NOT NULL,
	[ExploitID] [int] NULL,
	[PlatformID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITRESTRICTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITRESTRICTION](
	[ExploitRestrictionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPLOITTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPLOITTAG](
	[ExploitTagID] [int] IDENTITY(1,1) NOT NULL,
	[ExploitID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXPOSURELEVEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXPOSURELEVEL](
	[ExposureLevelID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EXTRACTEDFEATURES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EXTRACTEDFEATURES](
	[ExtractedFeaturesID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FACILITY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FACILITY](
	[FacilityID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FACILITYPHYSICALLOCATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FACILITYPHYSICALLOCATION](
	[FacilityPhysicalLocationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FACTORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FACTORY](
	[FactoryID] [int] IDENTITY(1,1) NOT NULL,
	[ManufacturID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FACTORYASSURANCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FACTORYASSURANCE](
	[FactoryAssuranceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FACTORYCOMPLIANCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FACTORYCOMPLIANCE](
	[FactoryComplianceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FACTORYPOLICY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FACTORYPOLICY](
	[FactoryPolicyID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FAX]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FAX](
	[FaxID] [int] IDENTITY(1,1) NOT NULL,
	[TelephoneID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FEED]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FEED](
	[FeedID] [int] IDENTITY(1,1) NOT NULL,
	[RepositoryID] [int] NULL,
	[ReferenceID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIELD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIELD](
	[FieldID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILE](
	[FileID] [int] IDENTITY(1,1) NOT NULL,
	[FileGUID] [nvarchar](500) NULL,
	[FileName] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILEACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILEACTIONNAME](
	[FileActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[FileActionNameName] [nvarchar](150) NOT NULL,
	[FileActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILECHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILECHANGERECORD](
	[FileChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILECLASSIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILECLASSIFICATION](
	[FileClassificationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILEDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILEDESCRIPTION](
	[FileDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[DescriptionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILEENCRYPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILEENCRYPTION](
	[FileEncryptionID] [int] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[EncryptionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILEEXTENSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILEEXTENSION](
	[FileExtensionID] [int] IDENTITY(1,1) NOT NULL,
	[FileExtensionGUID] [nvarchar](500) NULL,
	[FileExtensionName] [nvarchar](50) NULL,
	[FileExtensionDescription] [nvarchar](max) NULL,
	[FileExtensionValue] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidityID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILEEXTENSIONBLACKLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILEEXTENSIONBLACKLIST](
	[FileExtensionBlacklistID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILEEXTENSIONWHITELIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILEEXTENSIONWHITELIST](
	[FileExtensionWhitelistID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILELIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILELIST](
	[FileListID] [int] IDENTITY(1,1) NOT NULL,
	[FileListGUID] [nvarchar](500) NULL,
	[FileListName] [nvarchar](50) NULL,
	[FileListDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILELISTFILES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILELISTFILES](
	[FileListFileID] [int] IDENTITY(1,1) NOT NULL,
	[FileListID] [int] NOT NULL,
	[FileListGUID] [nvarchar](500) NULL,
	[FileID] [int] NOT NULL,
	[FileGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILEREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILEREFERENCE](
	[FileReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[FileReferenceGUID] [datetimeoffset](7) NULL,
	[FileID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILEREPOSITORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILEREPOSITORY](
	[FileRepositoryID] [int] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[FileGUID] [nvarchar](500) NULL,
	[RepositoryID] [int] NOT NULL,
	[RepositoryGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILERESTRICTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILERESTRICTION](
	[FileRestrictionID] [int] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[RestrictionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILERESTRICTIONCHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILERESTRICTIONCHANGERECORD](
	[FileRestrictionChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILETAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILETAG](
	[FileTagID] [int] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[TagID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILEVERSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILEVERSION](
	[FileVersionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILTER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FILTER](
	[FilterID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FILTERACTION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[FINDING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDING](
	[FindingID] [int] IDENTITY(1,1) NOT NULL,
	[FindingGUID] [nvarchar](500) NULL,
	[FindingName] [nvarchar](250) NULL,
	[FindingDescription] [nvarchar](max) NULL,
	[ImportanceID] [int] NULL,
	[AssetID] [int] NULL,
	[EndPointID] [int] NULL,
	[ApplicationID] [int] NULL,
	[FindingStatus] [nvarchar](50) NULL,
	[CriticalityLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
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
	[timestamp] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[JobID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGASSET](
	[FindingAssetID] [int] IDENTITY(1,1) NOT NULL,
	[FindingAssetGUID] [nvarchar](500) NULL,
	[FindingID] [int] NULL,
	[FindingGUID] [nvarchar](500) NULL,
	[AssetID] [int] NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[FindingAssetRelationship] [nvarchar](250) NULL,
	[FindingAssetDescription] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGCATEGORY](
	[FindingCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[FindingCategoryGUID] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[FindingCategoryName] [nvarchar](50) NULL,
	[FindingCategoryDescription] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[ImportanceID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGCATEGORYRACIMATRIX]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGCATEGORYRACIMATRIX](
	[FindingCategoryRACIMatrixID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGCHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGCHANGERECORD](
	[FindingChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGCODE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGCODE](
	[FindingCodeID] [int] IDENTITY(1,1) NOT NULL,
	[FindingID] [int] NOT NULL,
	[CodeID] [int] NOT NULL,
	[CodeLineID] [int] NULL,
	[FindingCodeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[ImportanceID] [int] NULL,
	[CriticalityLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGDESCRIPTION](
	[FindingDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[FindingDescriptionGUID] [nvarchar](500) NULL,
	[FindingID] [int] NOT NULL,
	[FindingGUID] [nvarchar](500) NULL,
	[DescriptionID] [int] NOT NULL,
	[DescriptionGUID] [nvarchar](500) NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGEVIDENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGEVIDENCE](
	[FindingEvidenceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGHTTPSESSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGHTTPSESSION](
	[FindingHTTPSessionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGIMPACT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGIMPACT](
	[FindingImpactID] [int] IDENTITY(1,1) NOT NULL,
	[FindingID] [int] NULL,
	[FindingGUID] [nvarchar](500) NULL,
	[ImpactID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGMATURITY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGMATURITY](
	[FindingMaturityID] [int] IDENTITY(1,1) NOT NULL,
	[FindingID] [int] NULL,
	[FindingGUID] [nvarchar](500) NULL,
	[SecurityDomainMaturityID] [int] NULL,
	[SecurityDomainMaturityGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGPERSON]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGPERSON](
	[FindingPersonID] [int] IDENTITY(1,1) NOT NULL,
	[FindingPersonGUID] [nvarchar](500) NULL,
	[FindingID] [int] NULL,
	[FindingGUID] [nvarchar](500) NULL,
	[FindingPersonRelationship] [nvarchar](50) NULL,
	[FindingPersonDescription] [nvarchar](max) NULL,
	[PersonID] [int] NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGRACIMATRIX]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGRACIMATRIX](
	[FindingRACIMatrixID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGRECOMMENDATION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[FINDINGREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGREFERENCE](
	[FindingReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[FindingID] [int] NOT NULL,
	[FindingGUID] [nvarchar](500) NULL,
	[ReferenceID] [int] NOT NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[ImportanceID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGSTATUS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGSTATUS](
	[FindingStatusID] [int] IDENTITY(1,1) NOT NULL,
	[FindingStatusDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FINDINGTAG](
	[FindingTagID] [int] IDENTITY(1,1) NOT NULL,
	[FindingID] [int] NOT NULL,
	[FindingGUID] [nvarchar](500) NULL,
	[TagID] [int] NULL,
	[TagGUID] [nvarchar](500) NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[ImportanceID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FINDINGVULNERABILITY]    Script Date: 04/03/2015 19:59:09 ******/
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
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIREWALLRULE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIREWALLRULE](
	[FirewallRuleID] [int] IDENTITY(1,1) NOT NULL,
	[FirewallRuleGUID] [nvarchar](500) NULL,
	[RuleID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ToolGenerationID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[ReliabilityID] [int] NULL,
	[ReliabilityReasonID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL,
	[ToolDeploymentID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIREWALLRULEADDRESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIREWALLRULEADDRESS](
	[FirewallRuleAddressID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIREWALLRULECHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIREWALLRULECHANGERECORD](
	[FirewallRuleChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIREWALLRULECHANGEREQUEST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIREWALLRULECHANGEREQUEST](
	[FirewallRuleChangeRequestID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIXACTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIXACTION](
	[FixActionID] [int] IDENTITY(1,1) NOT NULL,
	[FixActionGUID] [nvarchar](500) NULL,
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
	[XCCDFContent] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIXACTIONCOST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIXACTIONCOST](
	[FixActionCostID] [int] IDENTITY(1,1) NOT NULL,
	[cost_corrective_action] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIXACTIONFORFIXACTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIXACTIONFORFIXACTION](
	[FixActionRelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[FixActionRefID] [int] NOT NULL,
	[relationshiptype] [nvarchar](50) NULL,
	[FixActionSubjectID] [int] NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIXACTIONFORINCIDENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIXACTIONFORINCIDENT](
	[FixActionForIncidentID] [int] IDENTITY(1,1) NOT NULL,
	[FixActionID] [int] NOT NULL,
	[IncidentID] [int] NOT NULL,
	[FixActionCostID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIXACTIONFORVULNERABILITY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIXACTIONFORVULNERABILITY](
	[VulnerabilityFixActionID] [int] IDENTITY(1,1) NOT NULL,
	[FixActionID] [int] NOT NULL,
	[FixActionGUID] [nvarchar](500) NULL,
	[VulnerabilityID] [int] NOT NULL,
	[VulnerabilityGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIXACTIONPATCH]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FIXACTIONPATCH](
	[FixActionPatchID] [int] IDENTITY(1,1) NOT NULL,
	[FixActionID] [int] NULL,
	[FixActionGUID] [nvarchar](500) NULL,
	[FixActionPatchRelationship] [nvarchar](50) NULL,
	[FixActionPatchDescription] [nvarchar](max) NULL,
	[PatchID] [int] NULL,
	[PatchGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FIXSYSTEM]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[FLAG]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[FRAMEWORK]    Script Date: 04/03/2015 19:59:09 ******/
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
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FRAMEWORKFORTECHNICALCONTEXT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FRAMEWORKFORTECHNICALCONTEXT](
	[TechnicalContextFrameworkID] [int] IDENTITY(1,1) NOT NULL,
	[TechnicalContextFrameworkGUID] [nvarchar](500) NULL,
	[FrameworkID] [int] NOT NULL,
	[TechnicalContextID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FRAMEWORKREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[FRAUDSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FRAUDSTRATEGICOBJECTIVE](
	[FraudStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[FraudStrategicObjectiveName] [nvarchar](50) NULL,
	[FraudStrategicObjectiveDestruction] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FRAUDTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FRAUDTACTICALOBJECTIVE](
	[FraudTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[FraudTacticalObjectiveName] [nvarchar](50) NULL,
	[FraudTacticalObjectiveDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FREQUENCY]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[FTPACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FTPACTIONNAME](
	[FTPActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[FTPActionNameName] [nvarchar](150) NOT NULL,
	[FTPActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FUNCTION](
	[FunctionID] [int] IDENTITY(1,1) NOT NULL,
	[FunctionName] [nvarchar](100) NOT NULL,
	[VocabularyID] [int] NULL,
	[KnownVulnerable] [bit] NULL,
	[deprecated] [int] NULL,
	[FunctionVersion] [nvarchar](50) NULL,
	[ConfidenceLevelID] [int] NULL,
	[FunctionDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FUNCTIONALAREA]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FUNCTIONALAREA](
	[FunctionalAreaID] [int] IDENTITY(1,1) NOT NULL,
	[FunctionalAreaGUID] [nvarchar](500) NULL,
	[FunctionalAreaName] [nvarchar](250) NULL,
	[FunctionalAreaDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ImportanceID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FUNCTIONARGUMENT]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[FUNCTIONCHARACTERDELIMITER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FUNCTIONCHARACTERDELIMITER](
	[FunctionCharacterDelimiterID] [int] IDENTITY(1,1) NOT NULL,
	[FunctionID] [int] NOT NULL,
	[CharacterDelimiterID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FUNCTIONDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FUNCTIONDESCRIPTION](
	[FunctionDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FUNCTIONREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FUNCTIONREFERENCE](
	[FunctionReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[FunctionID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[FunctionReferenceDescription] [nvarchar](max) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FUNCTIONRELATIONSHIP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FUNCTIONRELATIONSHIP](
	[FunctionRelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[FunctionRelationshipGUID] [nvarchar](500) NULL,
	[FunctionParentID] [int] NOT NULL,
	[FunctionSubjectID] [int] NOT NULL,
	[FunctionRelationshipDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FUNCTIONRELATIONSHIPREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FUNCTIONRELATIONSHIPREFERENCE](
	[FunctionRelationshipReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[FunctionRelationshipGUID] [nvarchar](500) NULL,
	[FunctionRelationshipID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FUNCTIONTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FUNCTIONTAG](
	[FunctionTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GEOLOCATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GEOLOCATION](
	[GeoLocationID] [int] IDENTITY(1,1) NOT NULL,
	[GeoLocationGUID] [nvarchar](500) NULL,
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
	[CreationObjectID] [int] NULL,
	[CreationObjectGUID] [nvarchar](500) NULL,
	[timestamp] [datetimeoffset](7) NOT NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionMethodGUID] [nvarchar](500) NULL,
	[isEncrypted] [bit] NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GOOGLEDORK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GOOGLEDORK](
	[GoogleDorkID] [int] IDENTITY(1,1) NOT NULL,
	[DorkValue] [nvarchar](500) NULL,
	[DorkExpectedPattern] [nvarchar](500) NULL,
	[DorkDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GOOGLEDORKURI]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GOOGLEDORKURI](
	[GoogleDorkURIID] [int] IDENTITY(1,1) NOT NULL,
	[GoogleDorkID] [int] NOT NULL,
	[URIObjectID] [int] NOT NULL,
	[ConfidenceLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GROUP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GROUP](
	[GroupID] [int] IDENTITY(1,1) NOT NULL,
	[GroupGUID] [nvarchar](500) NULL,
	[GroupName] [nvarchar](50) NULL,
	[GroupDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GROUPINGRELATIONSHIP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GROUPINGRELATIONSHIP](
	[GroupingRelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[GroupingRelationshipName] [nvarchar](150) NOT NULL,
	[GroupingRelationshipDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GUIACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GUIACTIONNAME](
	[GUIActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[GUIActionNameName] [nvarchar](150) NOT NULL,
	[GUIActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GUIDELINE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GUIDELINE](
	[GuidelineID] [int] IDENTITY(1,1) NOT NULL,
	[GuidelineGUID] [nvarchar](500) NULL,
	[GuidelineText] [nvarchar](max) NOT NULL,
	[GuidelineDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GUIDELINEFORATTACKPATTERN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GUIDELINEFORATTACKPATTERN](
	[AttackPatternGuidelineID] [int] IDENTITY(1,1) NOT NULL,
	[GuidelineID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GUIDIALOGBOX]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GUIDIALOGBOX](
	[GUIDialogboxID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GUIOBJECT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GUIOBJECT](
	[GUIObjectID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[GUIWINDOW]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GUIWINDOW](
	[GUIWindowID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HANDLETYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HANDLETYPE](
	[HandleTypeID] [int] IDENTITY(1,1) NOT NULL,
	[HandleType] [nvarchar](50) NOT NULL,
	[HandleTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HARDWARE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HARDWARE](
	[HardwareID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HASHLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HASHLIST](
	[HashListID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[RepositoryID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HASHLISTVALUES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HASHLISTVALUES](
	[HashListValuesID] [int] IDENTITY(1,1) NOT NULL,
	[HashListID] [int] NOT NULL,
	[HashValueID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HASHNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HASHNAME](
	[HashNameID] [int] IDENTITY(1,1) NOT NULL,
	[HashingAlgorithmName] [nvarchar](50) NOT NULL,
	[HashingAlgorithmDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[TrustLevelID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HASHVALUE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HASHVALUE](
	[HashValueID] [int] IDENTITY(1,1) NOT NULL,
	[HashNameID] [int] NULL,
	[HashValueValue] [nvarchar](100) NOT NULL,
	[CollectedDate] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[RepositoryID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HEADER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HEADER](
	[HeaderID] [int] IDENTITY(1,1) NOT NULL,
	[HeaderGUID] [nvarchar](500) NULL,
	[HeaderName] [nvarchar](50) NULL,
	[HeaderDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HEADERDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HEADERDESCRIPTION](
	[HeaderDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[HeaderID] [int] NOT NULL,
	[DescriptionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HEADERREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HEADERREFERENCE](
	[HeaderReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[HeaderID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HEADERTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HEADERTAG](
	[HeaderTagID] [int] IDENTITY(1,1) NOT NULL,
	[HeaderID] [int] NOT NULL,
	[TagID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HIVELIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HIVELIST](
	[HiveListID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HOOKING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HOOKING](
	[HookingID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HOOKINGACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HOOKINGACTIONNAME](
	[HookingActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[HookingActionNameName] [nvarchar](150) NOT NULL,
	[HookingActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HOST]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[HOSTENDPOINT]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[HOSTFIELD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HOSTFIELD](
	[HostFieldID] [int] IDENTITY(1,1) NOT NULL,
	[Domain_Name] [nvarchar](500) NULL,
	[Port] [int] NULL,
	[PortID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HOSTNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HOSTNAME](
	[HostNameID] [int] IDENTITY(1,1) NOT NULL,
	[HostNameGUID] [nvarchar](500) NULL,
	[is_domain_name] [bit] NULL,
	[Hostname_Value] [nvarchar](250) NULL,
	[Naming_System] [nvarchar](250) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPACTIONNAME](
	[HTTPActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[HTTPActionNameName] [nvarchar](150) NOT NULL,
	[HTTPActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPCLIENTREQUEST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPCLIENTREQUEST](
	[HTTPClientRequestID] [int] IDENTITY(1,1) NOT NULL,
	[HTTPClientRequestGUID] [nvarchar](500) NULL,
	[HTTP_Request_Line] [int] NULL,
	[HTTP_Request_Header] [int] NULL,
	[HTTP_Message_Body] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPHEADER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPHEADER](
	[HTTPHeaderID] [int] IDENTITY(1,1) NOT NULL,
	[HTTPHeaderGUID] [nvarchar](500) NULL,
	[HeaderID] [int] NULL,
	[HTTPHeaderName] [nvarchar](50) NULL,
	[HTTPHeaderDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPHEADERCPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPHEADERCPE](
	[HTTPHeaderCPEID] [int] IDENTITY(1,1) NOT NULL,
	[CPEID] [int] NULL,
	[CPEName] [nvarchar](255) NOT NULL,
	[HTTPHeaderID] [int] NOT NULL,
	[isspecific] [bit] NULL,
	[isknownvulnerable] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[VocabularyID] [int] NULL,
	[ReferenceID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPHEADERDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPHEADERDESCRIPTION](
	[HTTPHeaderDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[HTTPHeaderID] [int] NOT NULL,
	[DescriptionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPHEADERPRODUCT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPHEADERPRODUCT](
	[HTTPHeaderProductID] [int] IDENTITY(1,1) NOT NULL,
	[HTTPHeaderID] [int] NULL,
	[HTTPHeaderGUID] [nvarchar](500) NULL,
	[ProductID] [int] NULL,
	[ProductGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPMESSAGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPMESSAGE](
	[HTTPMessageID] [int] IDENTITY(1,1) NOT NULL,
	[MessageID] [int] NULL,
	[Length] [int] NULL,
	[Message_Body] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[DiscoveryMethodID] [int] NULL,
	[DiscoveryToolID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPMETHOD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPMETHOD](
	[HTTPMethodID] [int] IDENTITY(1,1) NOT NULL,
	[HTTPMethodEnumID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPMETHODENUM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPMETHODENUM](
	[HTTPMethodEnumID] [int] IDENTITY(1,1) NOT NULL,
	[HTTPMethodName] [nvarchar](50) NULL,
	[HTTPMethodDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[knowndangerous] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPREQUESTHEADER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPREQUESTHEADER](
	[HTTPRequestHeaderID] [int] IDENTITY(1,1) NOT NULL,
	[Raw_Header] [nvarchar](max) NULL,
	[Parsed_Header] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPREQUESTHEADERFIELDS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPREQUESTHEADERFIELDS](
	[HTTPRequestHeaderFieldsID] [int] IDENTITY(1,1) NOT NULL,
	[Accept] [nvarchar](500) NULL,
	[Accept_Charset] [nvarchar](500) NULL,
	[Accept_Language] [nvarchar](500) NULL,
	[Accept_Datetime] [nvarchar](500) NULL,
	[Accept_Encoding] [nvarchar](500) NULL,
	[AuthorizationHeader] [nvarchar](500) NULL,
	[Cache_Control] [nvarchar](500) NULL,
	[Connection] [nvarchar](500) NULL,
	[Cookie] [nvarchar](max) NULL,
	[CookieID] [int] NULL,
	[Content_Length] [int] NULL,
	[Content_MD5] [nvarchar](50) NULL,
	[Content_Type] [nvarchar](50) NULL,
	[ContentMIMEID] [int] NULL,
	[Date] [datetimeoffset](7) NULL,
	[Expect] [nvarchar](500) NULL,
	[FromHeader] [nvarchar](500) NULL,
	[FromEmailAddressID] [int] NULL,
	[HostFieldID] [int] NULL,
	[If_Match] [nvarchar](500) NULL,
	[If_Modified_Since] [datetimeoffset](7) NULL,
	[If_None_Match] [nvarchar](500) NULL,
	[If_Range] [nvarchar](500) NULL,
	[If_Unmodified_Since] [datetimeoffset](7) NULL,
	[Max_Forwards] [int] NULL,
	[Pragma] [nvarchar](500) NULL,
	[Proxy_Authorization] [nvarchar](500) NULL,
	[Range] [nvarchar](500) NULL,
	[Referer] [nvarchar](max) NULL,
	[RefererURIID] [int] NULL,
	[TE] [nvarchar](500) NULL,
	[User_Agent] [nvarchar](500) NULL,
	[UserAgentID] [int] NULL,
	[Via] [nvarchar](500) NULL,
	[Warning] [nvarchar](500) NULL,
	[DNT] [nvarchar](500) NULL,
	[X_Requested_With] [nvarchar](500) NULL,
	[X_Forwarded_For] [nvarchar](500) NULL,
	[X_ATT_DeviceId] [nvarchar](500) NULL,
	[X_Wap_Profile] [nvarchar](500) NULL,
	[X_Wap_ProfileURIID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[TrustLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPREQUESTLINE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPREQUESTLINE](
	[HTTPRequestLineID] [int] IDENTITY(1,1) NOT NULL,
	[HTTP_Method] [int] NULL,
	[Value] [nvarchar](500) NULL,
	[Version] [nvarchar](50) NULL,
	[CreationDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPREQUESTRESPONSE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPREQUESTRESPONSE](
	[HTTPRequestResponseID] [int] IDENTITY(1,1) NOT NULL,
	[HTTPRequestResponseGUID] [nvarchar](500) NULL,
	[HTTP_Client_Request] [int] NULL,
	[HTTP_Server_Response] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPRESPONSEHEADER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPRESPONSEHEADER](
	[HTTPResponseHeaderID] [int] IDENTITY(1,1) NOT NULL,
	[Raw_Header] [nvarchar](max) NULL,
	[Parsed_Header] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPRESPONSEHEADERFIELDS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPRESPONSEHEADERFIELDS](
	[HTTPResponseHeaderFieldsID] [int] IDENTITY(1,1) NOT NULL,
	[Access_Control_Allow_Origin] [nvarchar](500) NULL,
	[Accept_Ranges] [nvarchar](500) NULL,
	[Age] [int] NULL,
	[Cache_Control] [nvarchar](500) NULL,
	[Connection] [nvarchar](500) NULL,
	[Content_Encoding] [nvarchar](50) NULL,
	[Content_Language] [nvarchar](50) NULL,
	[Content_Length] [int] NULL,
	[Content_Location] [nvarchar](500) NULL,
	[Content_MD5] [nvarchar](50) NULL,
	[Content_Disposition] [nvarchar](500) NULL,
	[Content_Range] [nvarchar](50) NULL,
	[Content_Type] [nvarchar](50) NULL,
	[ContentMIMEID] [int] NULL,
	[Date] [datetimeoffset](7) NULL,
	[ETag] [nvarchar](50) NULL,
	[Expires] [datetimeoffset](7) NULL,
	[Last_Modified] [datetimeoffset](7) NULL,
	[Link] [nvarchar](500) NULL,
	[Location] [nvarchar](500) NULL,
	[LocationURIID] [int] NULL,
	[P3P] [nvarchar](500) NULL,
	[Pragma] [nvarchar](500) NULL,
	[Proxy_Authenticate] [nvarchar](50) NULL,
	[Refresh] [int] NULL,
	[Retry_After] [int] NULL,
	[Server] [nvarchar](500) NULL,
	[Set_Cookie] [nvarchar](500) NULL,
	[Strict_Transport_Security] [nvarchar](500) NULL,
	[Trailer] [nvarchar](500) NULL,
	[Transfer_Encoding] [nvarchar](500) NULL,
	[Vary] [nvarchar](500) NULL,
	[VaryURIID] [int] NULL,
	[Via] [nvarchar](500) NULL,
	[Warning] [nvarchar](500) NULL,
	[WWW_Authenticate] [nvarchar](500) NULL,
	[X_Frame_Options] [nvarchar](500) NULL,
	[X_XSS_Protection] [nvarchar](500) NULL,
	[X_Content_Type_Options] [nvarchar](500) NULL,
	[X_Forwarded_Proto] [nvarchar](500) NULL,
	[X_Powered_By] [nvarchar](500) NULL,
	[X_UA_Compatible] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPSERVERRESPONSE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPSERVERRESPONSE](
	[HTTPServerResponseID] [int] IDENTITY(1,1) NOT NULL,
	[HTTP_Status_Line] [int] NULL,
	[HTTP_Response_Header] [int] NULL,
	[HTTP_Message_Body] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPSESSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPSESSION](
	[HTTPSessionID] [int] IDENTITY(1,1) NOT NULL,
	[HTTPSessionGUID] [nvarchar](500) NULL,
	[SessionID] [int] NULL,
	[SessionGUID] [nvarchar](500) NULL,
	[HTTP_Request_ResponseID] [int] NULL,
	[HTTPRequestResponseGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPSESSIONCOOKIE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPSESSIONCOOKIE](
	[HTTPSessionCookieID] [int] IDENTITY(1,1) NOT NULL,
	[HTTPSessionCookieGUID] [nvarchar](500) NULL,
	[HTTPSessionID] [int] NULL,
	[HTTPSessionGUID] [nvarchar](500) NULL,
	[HTTPSessionCookieRelationship] [nvarchar](50) NULL,
	[HTTPSessionDescription] [nvarchar](max) NULL,
	[CookieID] [int] NULL,
	[CookieGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HTTPSTATUSLINE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HTTPSTATUSLINE](
	[HTTPStatusLineID] [int] IDENTITY(1,1) NOT NULL,
	[Version] [nvarchar](50) NULL,
	[VersionID] [int] NULL,
	[Status_Code] [int] NULL,
	[Reason_Phrase] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[HUMANRISK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HUMANRISK](
	[HumanRiskID] [int] IDENTITY(1,1) NOT NULL,
	[HumanRiskName] [nvarchar](100) NOT NULL,
	[HumanRiskDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ICOMHANDLERACTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ICOMHANDLERACTION](
	[IComHandlerActionID] [int] IDENTITY(1,1) NOT NULL,
	[COM_Data] [nvarchar](max) NULL,
	[COM_Class_ID] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IDENTIFICATIONSYSTEM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IDENTIFICATIONSYSTEM](
	[IdentificationSystemID] [int] IDENTITY(1,1) NOT NULL,
	[SystemURI] [nvarchar](250) NOT NULL,
	[IdentifierValueDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IDENTIFIER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IDENTIFIER](
	[IdentifierID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IDTENTRY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IDTENTRY](
	[IDTEntryID] [int] IDENTITY(1,1) NOT NULL,
	[Type_Attr] [nvarchar](50) NULL,
	[Offset_High] [nvarchar](50) NULL,
	[Offset_Low] [nvarchar](50) NULL,
	[Offset_Middle] [nvarchar](50) NULL,
	[Selector] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IDTENTRYLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IDTENTRYLIST](
	[IDTEntryListID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IDTENTRYLISTENTRIES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IDTENTRYLISTENTRIES](
	[IDTEntryListEntriesID] [int] IDENTITY(1,1) NOT NULL,
	[IDTEntryListID] [int] NOT NULL,
	[IDTEntryID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IEXECACTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IEXECACTION](
	[IExecActionID] [int] IDENTITY(1,1) NOT NULL,
	[Exec_Arguments] [nvarchar](max) NULL,
	[Exec_Program_Path] [nvarchar](500) NULL,
	[Exec_Working_Directory] [nvarchar](500) NULL,
	[DirectoryID] [int] NULL,
	[Exec_Program_Hashes] [nvarchar](max) NULL,
	[HashListID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IMAGEFILE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IMAGEFILE](
	[ImageFileID] [int] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NULL,
	[ImageFileFormatID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[image_is_compressed] [bit] NULL,
	[Image_Height] [int] NULL,
	[Image_Width] [int] NULL,
	[Bits_Per_Pixel] [int] NULL,
	[Compression_Algorithm] [nvarchar](100) NULL,
	[CompressionID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IMAGEFILEEXIFTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IMAGEFILEEXIFTAG](
	[ImageFileEXIFTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IMAGEFILEFORMAT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IMAGEFILEFORMAT](
	[ImageFileFormatID] [int] IDENTITY(1,1) NOT NULL,
	[ImageFileFormatName] [nvarchar](50) NULL,
	[ImageFileFormatDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IMAGEFILETYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IMAGEFILETYPE](
	[ImageFileTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ImageFileTypeName] [nvarchar](50) NULL,
	[ImageFileTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[TrustLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IMPACT]    Script Date: 04/03/2015 19:59:09 ******/
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
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IMPACTQUALIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IMPACTQUALIFICATION](
	[ImpactQualificationID] [int] IDENTITY(1,1) NOT NULL,
	[ImpactQualificationName] [nvarchar](50) NULL,
	[ImpactQualificationDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IMPACTRATING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IMPACTRATING](
	[ImpactRatingID] [int] IDENTITY(1,1) NOT NULL,
	[ImpactRatingName] [nvarchar](50) NULL,
	[ImpactRatingDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IMPORTANCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IMPORTANCE](
	[ImportanceID] [int] IDENTITY(1,1) NOT NULL,
	[ImportanceGUID] [nvarchar](500) NULL,
	[ImportanceLevel] [nvarchar](50) NULL,
	[ImportanceDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IMPORTANCETYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IMPORTANCETYPE](
	[ImportanceTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ImportanceTypeName] [nvarchar](50) NOT NULL,
	[ImportanceTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INDICATOR]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATOR](
	[IndicatorID] [int] IDENTITY(1,1) NOT NULL,
	[IndicatorGUID] [nvarchar](500) NULL,
	[IndicatorTitle] [nvarchar](100) NOT NULL,
	[IndicatorDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceLevel] [nvarchar](50) NULL,
	[ConfidenceReasonID] [int] NULL,
	[LikelyImpact] [nvarchar](100) NULL,
	[Producer] [nvarchar](100) NULL,
	[negate] [bit] NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[RepositoryID] [int] NULL,
	[ImportanceID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INDICATORENVIRONMENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATORENVIRONMENT](
	[IndicatorEnvironmentID] [int] IDENTITY(1,1) NOT NULL,
	[IndicatorEnvironmentGUID] [nvarchar](500) NULL,
	[IndicatorID] [int] NULL,
	[IndicatorGUID] [nvarchar](500) NULL,
	[EnvironmentID] [int] NULL,
	[EnvironmentGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INDICATORFORINDICATOR]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATORFORINDICATOR](
	[IndicatorRefID] [int] NOT NULL,
	[IndicatorSubjectID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INDICATORID]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[INDICATORIDFORINCIDENTIOC]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATORIDFORINCIDENTIOC](
	[IndicatorIDID] [int] NOT NULL,
	[IncidentIOCID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INDICATORIDFORINDICATOR]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATORIDFORINDICATOR](
	[IndicatorIDID] [int] NOT NULL,
	[IndicatorID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INDICATORTESTMECHANISM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATORTESTMECHANISM](
	[IndicatorTestMechanismID] [int] IDENTITY(1,1) NOT NULL,
	[IndicatorID] [int] NULL,
	[TestMechanismID] [int] NULL,
	[Product_Name] [nvarchar](50) NULL,
	[Version] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INDICATORTESTMECHANISMCPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATORTESTMECHANISMCPE](
	[IndicatorTestMechanismCPEID] [int] IDENTITY(1,1) NOT NULL,
	[IndicatorTestMechanismID] [int] NULL,
	[CPEID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[RepositoryID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INDICATORTESTMECHANISMEVENTFILTER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATORTESTMECHANISMEVENTFILTER](
	[IndicatorTestMechanismEventFilterID] [int] IDENTITY(1,1) NOT NULL,
	[IndicatorTestMechanismID] [int] NULL,
	[EventFilterID] [int] NULL,
	[RuleID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INDICATORTESTMECHANISMEVENTSUPPRESSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATORTESTMECHANISMEVENTSUPPRESSION](
	[IndicatorTestMechanismEventSuppressionID] [int] IDENTITY(1,1) NOT NULL,
	[IndicatorTestMechanismID] [int] NULL,
	[EventSuppressionID] [int] NULL,
	[RuleID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INDICATORTESTMECHANISMRATEFILTER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATORTESTMECHANISMRATEFILTER](
	[IndicatorTestMechanismRateFilterID] [int] IDENTITY(1,1) NOT NULL,
	[IndicatorTestMechanismID] [int] NULL,
	[RateFilterID] [int] NULL,
	[RuleID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INDICATORTESTMECHANISMRULE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATORTESTMECHANISMRULE](
	[IndicatorTestMechanismRuleID] [int] IDENTITY(1,1) NOT NULL,
	[IndicatorTestMechanismID] [int] NULL,
	[RuleID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INDICATORTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATORTYPE](
	[IndicatorTypeID] [int] IDENTITY(1,1) NOT NULL,
	[IndicatorTypeGUID] [nvarchar](500) NULL,
	[IndicatorTypeName] [nvarchar](50) NULL,
	[IndicatorTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INFECTIONPROPAGATIONPROPERTIES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INFECTIONPROPAGATIONPROPERTIES](
	[InfectionPropagationPropertiesID] [int] IDENTITY(1,1) NOT NULL,
	[InfectionPropagationPropertiesName] [nvarchar](50) NULL,
	[InfectionPropagationPropertiesDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INFECTIONPROPAGATIONSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INFECTIONPROPAGATIONSTRATEGICOBJECTIVE](
	[InfectionPropagationStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[InfectionPropagationStrategicObjectiveName] [nvarchar](50) NULL,
	[InfectionPropagationStrategicObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INFECTIONPROPAGATIONTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INFECTIONPROPAGATIONTACTICALOBJECTIVE](
	[InfectionPropagationTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[InfectionPropagationTacticalObjectiveName] [nvarchar](50) NULL,
	[InfectionPropagationTacticalObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INFLUENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INFLUENCE](
	[InfluenceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INFORMATIONSOURCEROLE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INFORMATIONSOURCEROLE](
	[InformationSourceRoleID] [int] IDENTITY(1,1) NOT NULL,
	[InformationSourceRoleGUID] [nvarchar](500) NULL,
	[InformationSourceRoleName] [nvarchar](50) NULL,
	[InformationSourceRoleDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INFORMATIONSOURCETYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INFORMATIONSOURCETYPE](
	[InformationSourceTypeID] [int] IDENTITY(1,1) NOT NULL,
	[InformationSourceTypeGUID] [nvarchar](500) NULL,
	[InformationSourceTypeName] [nvarchar](150) NOT NULL,
	[InformationSourceTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INFORMATIONTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INFORMATIONTYPE](
	[InformationTypeID] [int] IDENTITY(1,1) NOT NULL,
	[InformationTypeGUID] [nvarchar](500) NULL,
	[InformationTypeName] [nvarchar](150) NOT NULL,
	[InformationTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INFORMATIONTYPEFORTHREATACTORTTP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INFORMATIONTYPEFORTHREATACTORTTP](
	[InformationTypeID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INFRASTRUCTURE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INFRASTRUCTURE](
	[InfrastructureID] [int] IDENTITY(1,1) NOT NULL,
	[InfrastructureGUID] [nvarchar](500) NULL,
	[isCritical] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INJECTIONVECTOR]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INJECTIONVECTOR](
	[InjectionVectorID] [int] IDENTITY(1,1) NOT NULL,
	[InjectionVectorGUID] [nvarchar](500) NULL,
	[InjectionVectorText] [nvarchar](max) NOT NULL,
	[InjectionVectorDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INJECTIONVECTORFORATTACKPATTERN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INJECTIONVECTORFORATTACKPATTERN](
	[AttackPatternInjectionVectorID] [int] IDENTITY(1,1) NOT NULL,
	[InjectionVectorID] [int] NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INSTANCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INSTANCE](
	[InstanceID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INSTRUCTION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[INTEGRITYLEVEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INTEGRITYLEVEL](
	[IntegrityLevelID] [int] IDENTITY(1,1) NOT NULL,
	[IntegrityLevel] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[IntegrityLevelDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INTEGRITYVIOLATIONSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INTEGRITYVIOLATIONSTRATEGICOBJECTIVE](
	[IntegrityViolationStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[IntegrityViolationStrategicObjectiveName] [nvarchar](50) NULL,
	[IntegrityViolationStrategicObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INTEGRITYVIOLATIONTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INTEGRITYVIOLATIONTACTICALOBJECTIVE](
	[IntegrityViolationTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[IntegrityViolationTacticalObjectiveName] [nvarchar](50) NULL,
	[IntegrityViolationTacticalObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INTERACTIONLEVEL]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[INTERACTIONPOINTS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INTERACTIONPOINTS](
	[InteractionPointsID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INTERACTIONPOINTSECURITYCONTROL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INTERACTIONPOINTSECURITYCONTROL](
	[InteractionPointSecurityControlID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INTERFACE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[INTERFACEFORSYSTEMINFO]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INTERFACEFORSYSTEMINFO](
	[SystemInfoID] [int] NOT NULL,
	[InterfaceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INTRUSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INTRUSION](
	[IntrusionID] [int] IDENTITY(1,1) NOT NULL,
	[BreachID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[INVESTIGATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INVESTIGATION](
	[InvestigationID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IPCACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IPCACTIONNAME](
	[IPCActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[IPCActionNameName] [nvarchar](150) NOT NULL,
	[IPCActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IPFIXDATASET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IPFIXDATASET](
	[IPFIXDataSetID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IPFIXMESSAGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IPFIXMESSAGE](
	[IPFIXMessageID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IPFIXMESSAGEHEADER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IPFIXMESSAGEHEADER](
	[IPFIXMessageHeaderID] [int] IDENTITY(1,1) NOT NULL,
	[VersionNumber] [nvarchar](50) NULL,
	[Byte_Length] [int] NULL,
	[Export_Timestamp] [int] NULL,
	[Sequence_Number] [int] NULL,
	[Observation_Domain_ID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IPFIXOPTIONSTEMPLATERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IPFIXOPTIONSTEMPLATERECORD](
	[IPFIXOptionsTemplateRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IPFIXOPTIONSTEMPLATERECORDFIELDSPECIFIERS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IPFIXOPTIONSTEMPLATERECORDFIELDSPECIFIERS](
	[IPFIXOptionsTemplateRecordFieldSpecifiersID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IPFIXOPTIONSTEMPLATERECORDHEADER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IPFIXOPTIONSTEMPLATERECORDHEADER](
	[IPFIXOptionsTemplateRecordHeaderID] [int] IDENTITY(1,1) NOT NULL,
	[Template_ID] [int] NULL,
	[Field_Count] [int] NULL,
	[Scope_Field_Count] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IPFIXOPTIONSTEMPLATESET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IPFIXOPTIONSTEMPLATESET](
	[IPFIXOptionsTemplateSetID] [int] IDENTITY(1,1) NOT NULL,
	[Padding] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IPFIXSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IPFIXSET](
	[IPFIXSetID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IPFIXSETHEADER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IPFIXSETHEADER](
	[IPFIXSetHeaderID] [int] IDENTITY(1,1) NOT NULL,
	[Set_ID] [int] NULL,
	[Length] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IPFIXTEMPLATERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IPFIXTEMPLATERECORD](
	[IPFIXTemplateRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IPFIXTEMPLATERECORDFIELDSPECIFIER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IPFIXTEMPLATERECORDFIELDSPECIFIER](
	[IPFIXTemplateRecordFieldSpecifierID] [int] IDENTITY(1,1) NOT NULL,
	[Enterprise_Bit] [bit] NULL,
	[Information_Element_ID] [nchar](15) NULL,
	[Field_Length] [int] NULL,
	[Enterprise_Number] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IPFIXTEMPLATERECORDHEADER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IPFIXTEMPLATERECORDHEADER](
	[IPFIXTemplateRecordHeaderID] [int] IDENTITY(1,1) NOT NULL,
	[Template_ID] [int] NULL,
	[Field_Count] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IPFIXTEMPLATESET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IPFIXTEMPLATESET](
	[IPFIXTemplateSetID] [int] IDENTITY(1,1) NOT NULL,
	[Padding] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[IRCACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IRCACTIONNAME](
	[IRCActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[IRCActionNameName] [nvarchar](150) NOT NULL,
	[IRCActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ISHOWMESSAGEACTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ISHOWMESSAGEACTION](
	[IShowMessageActionID] [int] IDENTITY(1,1) NOT NULL,
	[Show_Message_Body] [nvarchar](max) NULL,
	[Show_Message_Title] [nvarchar](500) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ISOCURRENCY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ISOCURRENCY](
	[iso_currency_code] [nvarchar](3) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[JOB]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[JOB](
	[JobID] [int] IDENTITY(1,1) NOT NULL,
	[JobGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ProviderID] [int] NULL,
	[DateStart] [datetimeoffset](7) NULL,
	[DateEnd] [datetimeoffset](7) NULL,
	[Status] [nvarchar](50) NULL,
	[AgentID] [int] NULL,
	[SessionID] [int] NULL,
	[AssetSessionID] [int] NULL,
	[Parameters] [image] NULL,
	[XmlResult] [image] NULL,
	[ErrorReason] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[KERNELHOOK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[KERNELHOOK](
	[KernelHookID] [int] IDENTITY(1,1) NOT NULL,
	[KernelHookTypeEnumID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[KERNELHOOKTYPEENUM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[KERNELHOOKTYPEENUM](
	[KernelHookTypeEnumID] [int] IDENTITY(1,1) NOT NULL,
	[KernelHookType] [nvarchar](50) NULL,
	[KernelHookTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[KEYWORD]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[KILLCHAIN]    Script Date: 04/03/2015 19:59:09 ******/
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
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[KILLCHAINFORTHREATACTORTTP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[KILLCHAINFORTHREATACTORTTP](
	[KillChainID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[KILLCHAINPHASE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[KILLCHAINPHASE](
	[KillChainPhaseID] [int] IDENTITY(1,1) NOT NULL,
	[KillChainPhaseGID] [nvarchar](100) NULL,
	[KillChainPhaseName] [nvarchar](100) NOT NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[KILLCHAINPHASEFORKILLCHAIN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[KILLCHAINPHASEFORKILLCHAIN](
	[KillChainKillChainPhaseID] [int] IDENTITY(1,1) NOT NULL,
	[KillChainID] [int] NOT NULL,
	[KillChainPhaseID] [int] NOT NULL,
	[ordinality] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[KILLCHAINPHASEFORTHREATACTORTTP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[KILLCHAINPHASEFORTHREATACTORTTP](
	[ThreatActorTTPKillChainPhaseID] [int] IDENTITY(1,1) NOT NULL,
	[KillChainPhaseID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LABEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LABEL](
	[LabelID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LANGUAGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LANGUAGE](
	[LanguageID] [int] IDENTITY(1,1) NOT NULL,
	[LanguageGUID] [nvarchar](500) NULL,
	[LanguageName] [nvarchar](50) NULL,
	[LanguageDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LANGUAGECHARACTEREOL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LANGUAGECHARACTEREOL](
	[LanguageCharacterEOLID] [int] IDENTITY(1,1) NOT NULL,
	[LanguageID] [int] NOT NULL,
	[CharacterID] [int] NOT NULL,
	[ordinal_position] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LANGUAGECLASS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LANGUAGECLASS](
	[LanguageClassID] [int] IDENTITY(1,1) NOT NULL,
	[LanguageClassDescription] [nvarchar](500) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LANGUAGEFORAPPLICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LANGUAGEFORAPPLICATION](
	[ApplicationLanguageID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[LanguageID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LANGUAGEFORTECHNICALCONTEXT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LANGUAGEFORTECHNICALCONTEXT](
	[TechnicalContextLanguageID] [int] IDENTITY(1,1) NOT NULL,
	[LanguageID] [int] NOT NULL,
	[LanguageGUID] [nvarchar](500) NULL,
	[TechnicalContextID] [int] NOT NULL,
	[TechnicalContextGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LANGUAGEFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LANGUAGEFUNCTION](
	[LanguageFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[LanguageFunctionGUID] [nvarchar](500) NULL,
	[LanguageID] [int] NOT NULL,
	[LanguageGUID] [nvarchar](50) NULL,
	[FunctionID] [int] NOT NULL,
	[FunctionGUID] [nvarchar](500) NULL,
	[LanguageFunctionDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[isDeprecated] [bit] NULL,
	[CreationObjectID] [int] NULL,
	[CreationObjectGUID] [nvarchar](500) NULL,
	[isKnownVulnerable] [bit] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionMethodGUID] [nvarchar](500) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceLevelGUID] [nvarchar](500) NULL,
	[ConfidenceReasonID] [int] NULL,
	[ConfidenceReasonGUID] [nvarchar](500) NULL,
	[TrustLevelID] [int] NULL,
	[TrustLevelGUID] [nvarchar](500) NULL,
	[TrustReasonID] [int] NULL,
	[TrustReasonGUID] [nvarchar](500) NULL,
	[VocabularyID] [int] NULL,
	[VocabularyGUID] [nvarchar](500) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LANGUAGEFUNCTIONREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LANGUAGEFUNCTIONREFERENCE](
	[LanguageFunctionReferenceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LANGUAGEFUNCTIONTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LANGUAGEFUNCTIONTAG](
	[LanguageFunctionTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LAW]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LAW](
	[LawID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LIBRARY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LIBRARY](
	[LibraryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LIBRARYACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LIBRARYACTIONNAME](
	[LibraryActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[LibraryActionNameName] [nvarchar](150) NOT NULL,
	[LibraryActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LIBRARYDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LIBRARYDESCRIPTION](
	[LibraryDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LIBRARYREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LIBRARYREFERENCE](
	[LibraryReferenceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LIBRARYTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LIBRARYTAG](
	[LibraryTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LICENSE]    Script Date: 04/03/2015 19:59:09 ******/
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
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LICENSEACCESSRECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LICENSEACCESSRECORD](
	[LicenseAccessRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LICENSECHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LICENSECHANGERECORD](
	[LicenseChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LICENSERESTRICTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LICENSERESTRICTION](
	[LicenseRestrictionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LICENSETYPE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[LINK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LINK](
	[LinkID] [int] IDENTITY(1,1) NOT NULL,
	[LinkGUID] [nvarchar](500) NULL,
	[ReferenceID] [int] NULL,
	[LinkURL] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[suspected_malicious] [bit] NULL,
	[SuspectedMaliciousReasonID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LINKTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LINKTYPE](
	[LinkTypeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LINUXPACKAGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LINUXPACKAGE](
	[LinuxPackageID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOCALE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOCALE](
	[LocaleID] [int] IDENTITY(1,1) NOT NULL,
	[LocaleGUID] [nvarchar](500) NULL,
	[LCIDHex] [nchar](4) NULL,
	[LCIDDec] [int] NULL,
	[LocaleValue] [nvarchar](50) NULL,
	[LocaleDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOCALEDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOCALEDESCRIPTION](
	[LocaleDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOCALEREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOCALEREFERENCE](
	[LocaleReferenceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOCATIONPOINT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOCATIONPOINT](
	[LocationPointID] [int] IDENTITY(1,1) NOT NULL,
	[latitude] [int] NOT NULL,
	[longitude] [int] NOT NULL,
	[elevation] [int] NULL,
	[radius] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOCATIONPOINTFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[LOCATIONPOINTFORORGANISATION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[LOCATIONPOINTFORPERSON]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[LOCATIONREGION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOCATIONREGION](
	[LocationRegionID] [int] IDENTITY(1,1) NOT NULL,
	[regionname] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOCATIONREGIONFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[LOGFILE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOGFILE](
	[LogFileID] [int] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOSSDURATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOSSDURATION](
	[LossDurationID] [int] NOT NULL,
	[LossDurationName] [nvarchar](50) NULL,
	[LossDurationDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOSSFACTOR]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOSSFACTOR](
	[LossFactorID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOSSFORM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOSSFORM](
	[LossFormID] [int] IDENTITY(1,1) NOT NULL,
	[LossFormName] [nvarchar](50) NOT NULL,
	[LossFormDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOSSPROPERTY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOSSPROPERTY](
	[LossPropertyID] [int] IDENTITY(1,1) NOT NULL,
	[LossPropertyGUID] [nvarchar](500) NULL,
	[LossPropertyName] [nvarchar](50) NOT NULL,
	[LossPropertyDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[LOSSPROPERTYFORINCIDENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOSSPROPERTYFORINCIDENT](
	[IncidentID] [int] NOT NULL,
	[LossPropertyID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MACHINEACCESSCONTROLPROPERTIES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MACHINEACCESSCONTROLPROPERTIES](
	[MachineAccessControlPropertiesID] [int] IDENTITY(1,1) NOT NULL,
	[MachineAccessControlPropertiesName] [nvarchar](50) NULL,
	[MachineAccessControlPropertiesDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MACHINEACCESSCONTROLSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MACHINEACCESSCONTROLSTRATEGICOBJECTIVE](
	[MachineAccessControlStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[MachineAccessControlStrategicObjectiveName] [nvarchar](50) NULL,
	[MachineAccessControlStrategicObjectiveDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MACHINEACCESSCONTROLTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MACHINEACCESSCONTROLTACTICALOBJECTIVE](
	[MachineAccessControlTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[MachineAccessControlTacticalObjectiveName] [nvarchar](50) NULL,
	[MachineAccessControlTacticalObjectiveDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MAINTENANCENOTE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MAINTENANCENOTE](
	[MaintenanceNoteID] [int] IDENTITY(1,1) NOT NULL,
	[MaintenanceNoteText] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MANAGEMENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MANAGEMENT](
	[ManagementID] [int] IDENTITY(1,1) NOT NULL,
	[ManagementName] [nvarchar](50) NULL,
	[ManagementDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MATURITYLEVEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MATURITYLEVEL](
	[MaturityLevelID] [int] IDENTITY(1,1) NOT NULL,
	[MaturityLevelGUID] [nvarchar](500) NULL,
	[MaturityLevelVocabularyID] [nvarchar](10) NULL,
	[MaturityLevelName] [nvarchar](50) NULL,
	[MaturityLevelDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MATURITYRATING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MATURITYRATING](
	[MaturityRatingID] [int] IDENTITY(1,1) NOT NULL,
	[ScoringSystemID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEASURESOURCE]    Script Date: 04/03/2015 19:59:09 ******/
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
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[TrustLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEASURESOURCECONTRIBUTOR]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[MEASURESOURCEINFORMATIONSOURCETYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEASURESOURCEINFORMATIONSOURCETYPE](
	[MeasureSourceID] [int] NOT NULL,
	[InformationSourceTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEASURESOURCEPLATFORM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEASURESOURCEPLATFORM](
	[MeasureSourceID] [int] NOT NULL,
	[PlatformID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEASURESOURCESYSTEM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEASURESOURCESYSTEM](
	[MeasureSourceID] [int] NOT NULL,
	[SystemID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEASURESOURCETOOL]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[MEASURESOURCETOOLTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEASURESOURCETOOLTYPE](
	[MeasureSourceID] [int] NOT NULL,
	[ToolTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MECHANISM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MECHANISM](
	[MechanismID] [int] IDENTITY(1,1) NOT NULL,
	[MechanismGUID] [nvarchar](500) NULL,
	[MechanismName] [nvarchar](250) NULL,
	[MechanismDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MECHANISMDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MECHANISMDESCRIPTION](
	[MechanismDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MECHANISMREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MECHANISMREFERENCE](
	[MechanismReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[MechanismReferenceGUID] [nvarchar](500) NULL,
	[MechanismID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MECHANISMRELATIONSHIP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MECHANISMRELATIONSHIP](
	[MechanismRelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[MechanismRelationshipGUID] [nvarchar](500) NULL,
	[MechanismParentID] [int] NOT NULL,
	[MechanismSubjectID] [int] NOT NULL,
	[MechanismRelationshipDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MECHANISMTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MECHANISMTAG](
	[MechanismTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEMORYADDRESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEMORYADDRESS](
	[MemoryAddressID] [int] IDENTITY(1,1) NOT NULL,
	[MemoryAddressGUID] [nvarchar](500) NULL,
	[MemoryAddressValue] [nvarchar](50) NOT NULL,
	[MemoryAddressDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEMORYADDRESSREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEMORYADDRESSREFERENCE](
	[MemoryAddressReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[MemoryAddressID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEMORYDUMP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEMORYDUMP](
	[MemoryDumpID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEMORYOBJECT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEMORYOBJECT](
	[MemoryObjectID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MEMORYSECTIONLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEMORYSECTIONLIST](
	[MemorySectionListID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MESSAGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MESSAGE](
	[MessageID] [int] IDENTITY(1,1) NOT NULL,
	[MessageGUID] [nvarchar](500) NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MESSAGECONFIDENTIALITYLEVEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MESSAGECONFIDENTIALITYLEVEL](
	[MessageConfidentialityLevelID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MESSAGELEVEL]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[MESSAGESMS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MESSAGESMS](
	[MessageSMSID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[METADATA]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[METHOD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[METHOD](
	[MethodID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[METHODOLOGY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[METHODOLOGY](
	[MethodologyID] [int] IDENTITY(1,1) NOT NULL,
	[MethodologyGUID] [nvarchar](500) NULL,
	[MethodologyName] [nvarchar](100) NOT NULL,
	[MethodologyDescription] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[lang] [nvarchar](10) NULL,
	[VocabularyID] [int] NULL,
	[MethodologyReference] [nvarchar](500) NULL,
	[MethodologyVersion] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[METHODOLOGYCHAPTER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[METHODOLOGYCHAPTER](
	[MethodologyChapterID] [int] IDENTITY(1,1) NOT NULL,
	[ChapterID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[METHODOLOGYDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[METHODOLOGYDESCRIPTION](
	[MethodologyDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[MethodologyID] [int] NOT NULL,
	[DescriptionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[METHODOLOGYNODE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[METHODOLOGYNODE](
	[MethodologyNodeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[METHODOLOGYREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[METHODOLOGYREFERENCE](
	[MethodologyReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[MethodologyID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[METHODOLOGYTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[METHODOLOGYTAG](
	[MethodologyTagID] [int] IDENTITY(1,1) NOT NULL,
	[MethodologyID] [int] NOT NULL,
	[TagID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[METHODOLOGYTECHNIQUE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[METHODOLOGYTECHNIQUE](
	[MethodologyTechniqueID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[METHODOLOGYTEST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[METHODOLOGYTEST](
	[MethodologyTestID] [int] IDENTITY(1,1) NOT NULL,
	[MethodologyTestGUID] [nvarchar](500) NULL,
	[MethodologyID] [int] NULL,
	[MethodologyGUID] [nvarchar](500) NULL,
	[TestID] [int] NULL,
	[TestGUID] [nvarchar](500) NULL,
	[TestVocabularyID] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[METRIC]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[METRIC](
	[MetricID] [int] IDENTITY(1,1) NOT NULL,
	[MetricGUID] [nvarchar](500) NULL,
	[MetricName] [nvarchar](250) NULL,
	[MetricDescription] [nvarchar](max) NULL,
	[MetricExamples] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[METRICCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[METRICCATEGORY](
	[MetricCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[METRICCHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[METRICCHANGERECORD](
	[MetricChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[METRICDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[METRICDESCRIPTION](
	[MetricDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[MetricID] [int] NOT NULL,
	[DescriptionID] [int] NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[METRICREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[METRICREFERENCE](
	[MetricReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[MetricID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[METRICTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[METRICTAG](
	[MetricTagID] [int] IDENTITY(1,1) NOT NULL,
	[MetricID] [int] NOT NULL,
	[TagID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MIME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MIME](
	[MIMEID] [int] IDENTITY(1,1) NOT NULL,
	[MIMEType] [nvarchar](50) NULL,
	[MIMETypeDescription] [nvarchar](max) NULL,
	[MIMEVersion] [nvarchar](50) NULL,
	[MIMETypeReference] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MIMEVERSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MIMEVERSION](
	[MIMEVersionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MIMEWHITELIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MIMEWHITELIST](
	[MIMEWhitelistID] [int] IDENTITY(1,1) NOT NULL,
	[MIMEID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MININGSCHEMA]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MININGSCHEMA](
	[MiningSchemaID] [int] IDENTITY(1,1) NOT NULL,
	[SchemaID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATION](
	[MitigationID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationGUID] [nvarchar](500) NULL,
	[MitigationVocabularyID] [nvarchar](50) NULL,
	[MitigationName] [nvarchar](250) NULL,
	[SolutionMitigationText] [nvarchar](max) NOT NULL,
	[EffectivenessID] [int] NULL,
	[Mitigation_Effectiveness_Notes] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ShortTerm] [bit] NULL,
	[LongTerm] [bit] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONCODE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONCODE](
	[MitigationCodeID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationID] [int] NOT NULL,
	[CodeID] [int] NOT NULL,
	[Block_Nature] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONEFFECTIVENESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONEFFECTIVENESS](
	[MitigationEffectivenessID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationID] [int] NULL,
	[EffectivenessID] [int] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONFORATTACKPATTERN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONFORATTACKPATTERN](
	[AttackPatternMitigationID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationID] [int] NOT NULL,
	[MitigationGUID] [nvarchar](500) NULL,
	[AttackPatternID] [int] NOT NULL,
	[AttackPatternGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONFORCWE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONFORCWE](
	[CWEMitigationID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationID] [int] NOT NULL,
	[MitigationGUID] [nvarchar](500) NULL,
	[MitigationVocabularyID] [nvarchar](50) NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[MitigationPhaseID] [int] NULL,
	[MitigationStrategyID] [int] NULL,
	[CWEMitigationDescription] [nvarchar](max) NULL,
	[EffectivenessID] [int] NULL,
	[CWEMitigationEffectivenessNotes] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONPHASE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONPHASE](
	[MitigationPhaseID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationPhaseGUID] [nvarchar](500) NULL,
	[PhaseID] [int] NULL,
	[PhaseGUID] [nvarchar](500) NULL,
	[MitigationPhaseName] [nvarchar](250) NOT NULL,
	[MitigationPhaseDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ImportanceID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONPHASEFORMITIGATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONPHASEFORMITIGATION](
	[MitigationMitigationPhaseID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationID] [int] NOT NULL,
	[MitigationGUID] [nvarchar](500) NULL,
	[MitigationPhaseID] [int] NOT NULL,
	[MitigationPhaseGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONPHASETAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONPHASETAG](
	[MitigationPhaseTagID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationPhaseID] [int] NULL,
	[MitigationPhaseGUID] [nvarchar](500) NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONREFERENCE](
	[MitigationReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationID] [int] NOT NULL,
	[MitigationGUID] [nvarchar](500) NULL,
	[ReferenceID] [int] NOT NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[MitigationReferenceDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONSTRATEGY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONSTRATEGY](
	[MitigationStrategyID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationStrategyGUID] [nvarchar](500) NULL,
	[StrategyID] [int] NULL,
	[StrategyGUID] [nvarchar](500) NULL,
	[MitigationStrategyName] [nvarchar](250) NULL,
	[MitigationStrategyDescription] [nvarchar](max) NULL,
	[MitigationStrategyVocabularyID] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONSTRATEGYFORMITIGATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONSTRATEGYFORMITIGATION](
	[MitigationMitigationStrategyID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationID] [int] NOT NULL,
	[MitigationGUID] [nvarchar](500) NULL,
	[MitigationStrategyID] [int] NOT NULL,
	[MitigationStrategyGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NOT NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MITIGATIONSTRATEGYTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MITIGATIONSTRATEGYTAG](
	[MitigationStrategyTagID] [int] IDENTITY(1,1) NOT NULL,
	[MitigationStrategyID] [int] NULL,
	[MitigationStrategyGUID] [nvarchar](500) NULL,
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

/****** Object:  Table [dbo].[MMSMESSAGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MMSMESSAGE](
	[MMSMessageID] [int] IDENTITY(1,1) NOT NULL,
	[MessageID] [int] NULL,
	[SMSMessageID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MOBILEDEVICE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MOBILEDEVICE](
	[MobileDeviceID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MODEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MODEL](
	[ModelID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MODELCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MODELCATEGORY](
	[ModelCategoryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MODELDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MODELDESCRIPTION](
	[ModelDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[ModelID] [int] NOT NULL,
	[DescriptionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MODULE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MODULE](
	[ModuleID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleName] [nvarchar](500) NULL,
	[ModuleDescription] [nvarchar](max) NULL,
	[ModuleVersion] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MUTEX]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MUTEX](
	[MutexID] [int] IDENTITY(1,1) NOT NULL,
	[MutexName] [nvarchar](250) NULL,
	[MutexDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MUTEXNAME]    Script Date: 04/03/2015 19:59:09 ******/
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
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MUTEXNAMES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MUTEXNAMES](
	[MutexNamesID] [int] IDENTITY(1,1) NOT NULL,
	[MutexID] [int] NOT NULL,
	[MutexNameID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MUTEXTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MUTEXTYPE](
	[MutexTypeID] [int] IDENTITY(1,1) NOT NULL,
	[MutexType] [nvarchar](50) NULL,
	[MutexTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NAICS]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[NAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NAME](
	[NameID] [int] IDENTITY(1,1) NOT NULL,
	[NameText] [nvarchar](500) NULL,
	[LocaleID] [int] NULL,
	[VersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETROUTE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETROUTE](
	[NetRouteID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORK](
	[NetworkID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKACTIONNAME](
	[NetworkActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[NetworkActionNameName] [nvarchar](150) NOT NULL,
	[NetworkActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKCONNECTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKCONNECTION](
	[NetworkConnectionID] [int] IDENTITY(1,1) NOT NULL,
	[NetworkConnectionGUID] [nvarchar](500) NULL,
	[tls_used] [bit] NULL,
	[Creation_Time] [datetimeoffset](7) NULL,
	[ProtocolLayer3ID] [int] NULL,
	[Layer3_Protocol] [nvarchar](50) NULL,
	[ProtocolLayer4ID] [int] NULL,
	[Layer4_Protocol] [nvarchar](50) NULL,
	[ProtocolLayer7ID] [int] NULL,
	[Layer7_Protocol] [nvarchar](50) NULL,
	[SourceSocketAddressID] [int] NULL,
	[SourceTCPStateID] [int] NULL,
	[Source_TCP_State] [nvarchar](50) NULL,
	[DestinationSocketAddressID] [int] NULL,
	[DestinationTCPStateID] [int] NULL,
	[Destination_TCP_State] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKCONNECTIONLAYER7]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKCONNECTIONLAYER7](
	[NetworkConnectionLayer7ID] [int] IDENTITY(1,1) NOT NULL,
	[NetworkConnectionID] [int] NULL,
	[NetworkConnectionGUID] [nvarchar](500) NULL,
	[HTTPSessionID] [int] NULL,
	[HTTPSessionGUID] [nvarchar](500) NULL,
	[DNSQueryID] [int] NULL,
	[DNDQueryGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKFLOW]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKFLOW](
	[NetworkFlowID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKFLOWLABEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKFLOWLABEL](
	[NetworkFlowLabelID] [int] IDENTITY(1,1) NOT NULL,
	[Src_Socket_Address] [int] NULL,
	[Dest_Socket_Address] [int] NULL,
	[IP_Protocol] [int] NULL,
	[Ingress_Interface_Index] [int] NULL,
	[Egress_Interface_Index] [int] NULL,
	[IP_Type_Of_Service] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKINTERFACE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKINTERFACE](
	[NetworkInterfaceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKPACKET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKPACKET](
	[NetworkPacketID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKROUTE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKROUTE](
	[NetworkRouteID] [int] IDENTITY(1,1) NOT NULL,
	[NetworkRouteGUID] [nvarchar](500) NULL,
	[NetRouteID] [int] NULL,
	[is_ipv6] [bit] NULL,
	[is_autoconfigure_address] [bit] NULL,
	[is_immortal] [bit] NULL,
	[is_loopback] [bit] NULL,
	[is_publish] [bit] NULL,
	[DestinationAddressID] [int] NULL,
	[OriginAddressID] [int] NULL,
	[NetmaskID] [int] NULL,
	[GatewayAddressID] [int] NULL,
	[Metric] [int] NULL,
	[NetworkRouteTypeID] [int] NULL,
	[NetworkRouteType] [nvarchar](50) NULL,
	[ProtocolID] [int] NULL,
	[NetworkRouteProtocol] [nvarchar](50) NULL,
	[NetworkRouteInterface] [nvarchar](50) NULL,
	[PreferredLifetime] [int] NULL,
	[ValidLifetime] [int] NULL,
	[RouteAge] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKROUTEENTRY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKROUTEENTRY](
	[NetworkRouteEntryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKROUTETYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKROUTETYPE](
	[NetworkRouteTypeID] [int] IDENTITY(1,1) NOT NULL,
	[RouteType] [nvarchar](50) NULL,
	[RouteTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKSHARE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKSHARE](
	[NetworkShareID] [int] IDENTITY(1,1) NOT NULL,
	[NetworkShareGUID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKSHAREACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKSHAREACTIONNAME](
	[NetworkShareActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[NetworkShareActionNameName] [nvarchar](150) NOT NULL,
	[NetworkShareActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKSOCKET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKSOCKET](
	[NetworkSocketID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKSUBNET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKSUBNET](
	[NetworkSubnetID] [int] IDENTITY(1,1) NOT NULL,
	[NetworkSubnetGUID] [nvarchar](500) NULL,
	[NetworkSubnetName] [nvarchar](50) NULL,
	[NetworkSubnetDescription] [nvarchar](max) NULL,
	[NetworkSubnetNumberOfIPAddresses] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKSUBNETROUTES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKSUBNETROUTES](
	[NetworkSubnetRoutesID] [int] IDENTITY(1,1) NOT NULL,
	[NetworkSubnetID] [int] NULL,
	[NetworkSubnetGUID] [nvarchar](500) NULL,
	[NetworkRouteID] [int] NULL,
	[NetworkRouteGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKZONE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKZONE](
	[NetworkZoneID] [int] IDENTITY(1,1) NOT NULL,
	[NetworkZoneGUID] [nvarchar](500) NULL,
	[ZoneID] [int] NULL,
	[ZoneGUID] [nvarchar](500) NULL,
	[NetworkZoneName] [nvarchar](250) NULL,
	[NetworkZoneDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKZONEDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKZONEDESCRIPTION](
	[NetworkZoneDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[NetworkZoneID] [int] NULL,
	[NetworkZoneGUID] [nvarchar](500) NULL,
	[DescriptionID] [int] NULL,
	[DescriptionGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKZONERESTRICTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKZONERESTRICTION](
	[NetworkZoneRestrictionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NETWORKZONETAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NETWORKZONETAG](
	[NetworkZoneTagID] [int] IDENTITY(1,1) NOT NULL,
	[ConfidentialityLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NEURALNETWORK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NEURALNETWORK](
	[NeuralNetworkID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[NOTIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NOTIFICATION](
	[NotificationID] [int] IDENTITY(1,1) NOT NULL,
	[NotificationGUID] [nvarchar](500) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[UserID] [uniqueidentifier] NULL,
	[NotificationMessage] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ImportanceID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OBFUSCATIONTECHNIQUE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OBFUSCATIONTECHNIQUE](
	[ObfuscationTechniqueID] [int] IDENTITY(1,1) NOT NULL,
	[ObfuscationTechniqueGUID] [nvarchar](500) NULL,
	[TechniqueID] [int] NULL,
	[ObfuscationTechniqueName] [nvarchar](150) NOT NULL,
	[ObfuscationTechniqueDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OBFUSCATIONTECHNIQUETAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OBFUSCATIONTECHNIQUETAG](
	[ObfuscationTechniqueTagID] [int] IDENTITY(1,1) NOT NULL,
	[ObfuscationTechniqueID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OBJECTIVE](
	[ObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[ObjectiveGUID] [nvarchar](500) NULL,
	[ObjectiveCategoryID] [int] NULL,
	[ObjectiveVocabularyID] [nvarchar](50) NULL,
	[ObjectiveName] [nvarchar](250) NULL,
	[ObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OBJECTIVECATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OBJECTIVECATEGORY](
	[ObjectiveCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ObjectiveCategoryGUID] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[ObjectiveCategoryName] [nvarchar](100) NULL,
	[ObjectiveCategoryDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OBJECTIVETAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OBJECTIVETAG](
	[ObjectiveTagID] [int] IDENTITY(1,1) NOT NULL,
	[ObjectiveID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OBJECTRELATIONSHIP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OBJECTRELATIONSHIP](
	[ObjectRelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[ObjectRelationshipName] [nvarchar](150) NOT NULL,
	[ObjectRelationshipDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OBJECTSTATE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OBJECTSTATE](
	[ObjectStateID] [int] IDENTITY(1,1) NOT NULL,
	[ObjectStateName] [nvarchar](150) NOT NULL,
	[ObjectStateDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OBJECTTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OBJECTTYPE](
	[ObjectTypeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OBSERVATIONMETHOD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OBSERVATIONMETHOD](
	[ObservationMethodID] [int] IDENTITY(1,1) NOT NULL,
	[ObservationMethodGUID] [nvarchar](500) NULL,
	[ObservationMethodName] [nvarchar](150) NOT NULL,
	[ObservationMethodDescription] [nvarchar](max) NULL,
	[MeasureSourceID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OFFSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OFFSET](
	[OffsetID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ONTOLOGY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ONTOLOGY](
	[OntologyID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OPCODE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OPCODE](
	[OpcodeID] [int] IDENTITY(1,1) NOT NULL,
	[OpcodeName] [nvarchar](50) NOT NULL,
	[OpcodeDescription] [nvarchar](max) NULL,
	[OpcodeHEXValue] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OPCODEFORCPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OPCODEFORCPE](
	[CPEID] [nvarchar](255) NOT NULL,
	[OpcodeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OPERATIONENUMERATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OPERATIONENUMERATION](
	[OperationEnumerationID] [int] IDENTITY(1,1) NOT NULL,
	[OperationValue] [nvarchar](50) NOT NULL,
	[OperationDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OPERATIONENUMERATIONFORSIMPLEDATATYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OPERATIONENUMERATIONFORSIMPLEDATATYPE](
	[SimpleDataTypeID] [int] NOT NULL,
	[OperationEnumerationID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OPERATORENUMERATION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[ORGANISATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANISATION](
	[OrganisationID] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationGUID] [nvarchar](500) NULL,
	[OrganisationName] [nvarchar](100) NOT NULL,
	[OrganisationType] [nvarchar](100) NULL,
	[OrganisationKnownAs] [nvarchar](100) NULL,
	[industry] [nvarchar](6) NULL,
	[CountryID] [int] NULL,
	[employee_count] [nvarchar](50) NULL,
	[revenueamount] [int] NULL,
	[iso_currency_code] [nvarchar](3) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[TrustLevelID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANISATIONACCESSRECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANISATIONACCESSRECORD](
	[OrganisationAccessRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANISATIONCHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANISATIONCHANGERECORD](
	[OrganisationChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANISATIONDOMAINNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANISATIONDOMAINNAME](
	[OrganisationDomainNameID] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[OrganisationGUID] [nvarchar](500) NULL,
	[OrganisationDomainNameRelationship] [nvarchar](50) NULL,
	[DomainNameID] [int] NULL,
	[DomainNameGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANISATIONFORTHREATACTORTTP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANISATIONFORTHREATACTORTTP](
	[ThreatActorTTPOrganisationID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[ThreatActorTTPGUID] [nvarchar](500) NULL,
	[OrganisationID] [int] NOT NULL,
	[OrganisationGUID] [nvarchar](500) NULL,
	[Information_Source] [nvarchar](250) NULL,
	[ConfidenceLevel] [nvarchar](50) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[notes] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANISATIONLICENSE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANISATIONLICENSE](
	[OrganisationLicenseID] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[LicenseID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANISATIONPOLICY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANISATIONPOLICY](
	[OrganisationPolicyID] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[PolicyID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANISATIONPROJECT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANISATIONPROJECT](
	[OrganisationProjectID] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationID] [int] NULL,
	[OrganisationGUID] [nvarchar](500) NULL,
	[OrganisationProjectRelationship] [nvarchar](50) NULL,
	[OrganisationProjectDescription] [nvarchar](max) NULL,
	[ProjectID] [int] NULL,
	[ProjectGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromdate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANISATIONSCHEDULE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANISATIONSCHEDULE](
	[OrganisationScheduleID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANISATIONTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANISATIONTAG](
	[OrganisationTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANISATIONTECHNOLOGY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANISATIONTECHNOLOGY](
	[OrganisationTechnologyID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANISATIONWORKINGHOURS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANISATIONWORKINGHOURS](
	[OrganisationWorkingHoursID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANIZATIONALUNIT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANIZATIONALUNIT](
	[OrganizationalUnitID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationalUnitGUID] [nvarchar](500) NULL,
	[OUName] [nvarchar](250) NOT NULL,
	[OUDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANIZATIONALUNITFORORGANISATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANIZATIONALUNITFORORGANISATION](
	[OrganisationUnitsID] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[OrganisationGUID] [nvarchar](500) NULL,
	[OrganizationalUnitID] [int] NOT NULL,
	[OrganizationalUnitGUID] [nvarchar](500) NULL,
	[OUChildName] [nvarchar](150) NULL,
	[OUChildDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ORGANIZATIONALUNITPOLICY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORGANIZATIONALUNITPOLICY](
	[OrganizationalUnitPolicyID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationalUnitID] [int] NOT NULL,
	[OrganizationalUnitGUID] [nvarchar](500) NULL,
	[OrganizationalUnitRelationship] [nvarchar](50) NULL,
	[PolicyID] [int] NOT NULL,
	[PolicyGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OS](
	[OSID] [int] IDENTITY(1,1) NOT NULL,
	[Operating_System_Name] [nvarchar](50) NULL,
	[OSname] [nchar](10) NOT NULL,
	[OSversion] [nchar](10) NULL,
	[LocaleID] [int] NULL,
	[OSlang] [nchar](10) NULL,
	[OSSP] [nchar](10) NULL,
	[Platform] [nchar](10) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OSCLASS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSCLASS](
	[OSClassID] [int] IDENTITY(1,1) NOT NULL,
	[OSClassGUID] [nvarchar](500) NULL,
	[Operating_System_Class_Description] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OSFAMILY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSFAMILY](
	[OSFamilyID] [int] IDENTITY(1,1) NOT NULL,
	[FamilyName] [nvarchar](50) NULL,
	[FamilyDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OSFAMILYFOROS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSFAMILYFOROS](
	[OSFamilyOSID] [int] IDENTITY(1,1) NOT NULL,
	[OSID] [int] NOT NULL,
	[OSFamilyID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OSFAMILYPLATFORM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSFAMILYPLATFORM](
	[OSFamilyPlatformID] [int] IDENTITY(1,1) NOT NULL,
	[OSFamilyID] [int] NULL,
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

/****** Object:  Table [dbo].[OSILAYER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSILAYER](
	[OSILayerID] [int] IDENTITY(1,1) NOT NULL,
	[OSILayerName] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OSILAYERFORATTACKSURFACE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSILAYERFORATTACKSURFACE](
	[AttackSurfaceOSILayerID] [int] IDENTITY(1,1) NOT NULL,
	[OSILayerID] [int] NOT NULL,
	[AttackSurfaceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OSINSTRUCTIONMEMORYADDRESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSINSTRUCTIONMEMORYADDRESS](
	[OSInstructionMemoryAddressID] [int] IDENTITY(1,1) NOT NULL,
	[OSID] [int] NOT NULL,
	[InstructionID] [int] NOT NULL,
	[MemoryAddressID] [int] NOT NULL,
	[OSPatchLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OSPATCH]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSPATCH](
	[OSPatchID] [int] IDENTITY(1,1) NOT NULL,
	[OSPatchGUID] [nvarchar](500) NULL,
	[OSID] [int] NULL,
	[OSGUID] [nvarchar](500) NULL,
	[PatchID] [int] NULL,
	[PatchGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionMethodGUID] [nvarchar](500) NULL,
	[TrustLevelID] [int] NULL,
	[TrustLevelGUID] [nvarchar](500) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OSPATCHLEVEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSPATCHLEVEL](
	[OSPatchLevelID] [int] IDENTITY(1,1) NOT NULL,
	[OSPatchLevelGUID] [nvarchar](500) NOT NULL,
	[OSPatchLevelDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OSPATCHLEVELPATCH]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSPATCHLEVELPATCH](
	[OSPatchesID] [int] IDENTITY(1,1) NOT NULL,
	[OSPatchLevelID] [int] NOT NULL,
	[OSPatchID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OUTPUTFIELD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OUTPUTFIELD](
	[OutputFieldID] [int] IDENTITY(1,1) NOT NULL,
	[FieldID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OWASPTOP10]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OWASPTOP10](
	[OWASPTOP10ID] [int] IDENTITY(1,1) NOT NULL,
	[OWASPTOP10GUID] [nvarchar](500) NULL,
	[OWASPTOP10RefID] [nvarchar](10) NULL,
	[OWASPName] [nvarchar](150) NOT NULL,
	[OWASPDescription] [nvarchar](max) NULL,
	[Detectability] [nvarchar](50) NULL,
	[Rank] [int] NOT NULL,
	[YearTop10] [int] NULL,
	[ReferenceURL] [nvarchar](250) NULL,
	[OWASPTOP10Type] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OWASPTOP10ATTACKVECTOR]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[OWASPTOP10DEFENSETOOLTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OWASPTOP10DEFENSETOOLTYPE](
	[OWASPTOP10ID] [int] NOT NULL,
	[DefenseToolTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OWASPTOP10DETECTABILITY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OWASPTOP10DETECTABILITY](
	[OWASPTOP10ID] [int] NOT NULL,
	[DetectabilityID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OWASPTOP10EXPLOITABILITY]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[OWASPTOP10IMPACT]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[OWASPTOP10MAPPING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OWASPTOP10MAPPING](
	[OWASPTOP10MappingID] [int] IDENTITY(1,1) NOT NULL,
	[OWASPTOP10RefID] [int] NOT NULL,
	[OWASPNameRef] [nvarchar](150) NULL,
	[RankRef] [int] NULL,
	[YearRef] [int] NULL,
	[OWASPTOP10SubjectID] [int] NOT NULL,
	[OWASPNameSubject] [nvarchar](150) NULL,
	[RankSubject] [int] NULL,
	[YearSubject] [int] NULL,
	[CreationDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OWASPTOP10PREVALENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OWASPTOP10PREVALENCE](
	[OWASPTOP10ID] [int] NOT NULL,
	[PrevalenceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OWASPTOP10REFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[OWASPTOP10TOOLINFORMATION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[OWNERSHIP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OWNERSHIP](
	[OwnershipID] [int] IDENTITY(1,1) NOT NULL,
	[OwnershipName] [nvarchar](50) NULL,
	[OwnershipDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PACKAGEINTENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PACKAGEINTENT](
	[PackageIntentID] [int] IDENTITY(1,1) NOT NULL,
	[PackageIntentGUID] [nvarchar](500) NULL,
	[PackageIntentName] [nvarchar](50) NULL,
	[PackageIntentDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromdate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ImportanceID] [int] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PACKAGING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PACKAGING](
	[PackagingID] [int] IDENTITY(1,1) NOT NULL,
	[PackagingGUID] [nvarchar](500) NULL,
	[PackagingLayerName] [nvarchar](50) NOT NULL,
	[PackagingDescription] [nvarchar](max) NULL,
	[is_encrypted] [bit] NULL,
	[is_compressed] [bit] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[RepositoryID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PACKAGINGCOMPRESSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PACKAGINGCOMPRESSION](
	[PackagingCompressionID] [int] IDENTITY(1,1) NOT NULL,
	[PackagingCompressionGUID] [nvarchar](500) NULL,
	[PackagingCompressionDescription] [nvarchar](max) NULL,
	[PackagingID] [int] NOT NULL,
	[PackagingGUID] [nvarchar](500) NULL,
	[CompressionID] [int] NOT NULL,
	[CompressionGUID] [nvarchar](500) NULL,
	[LayerOrder] [int] NOT NULL,
	[CompressionPassword] [nvarchar](250) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PACKAGINGENCODING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PACKAGINGENCODING](
	[PackagingEncodingID] [int] NOT NULL,
	[PackagingEncodingGUID] [nvarchar](500) NULL,
	[PackagingID] [int] NOT NULL,
	[PackagingGUID] [nvarchar](500) NULL,
	[EncodingID] [int] NOT NULL,
	[EncodingGUID] [nvarchar](500) NULL,
	[LayerOrder] [int] NOT NULL,
	[algorithm] [nvarchar](250) NULL,
	[character_set] [nvarchar](250) NULL,
	[CharacterSetID] [int] NULL,
	[custom_character_set_ref] [nvarchar](250) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PACKAGINGENCRYPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PACKAGINGENCRYPTION](
	[PackagingEncryptionID] [int] NOT NULL,
	[PackagingEncryptionGUID] [nvarchar](500) NULL,
	[PackagingID] [int] NOT NULL,
	[PackagingGUID] [nvarchar](500) NULL,
	[EncryptionID] [int] NOT NULL,
	[EncryptionGUID] [nvarchar](500) NULL,
	[LayerOrder] [int] NOT NULL,
	[encryption_key] [nvarchar](250) NULL,
	[encryption_key_ref] [nvarchar](250) NULL,
	[PackagingEncryptionDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PARAGRAPH]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PARAGRAPH](
	[ParagraphID] [int] IDENTITY(1,1) NOT NULL,
	[SectionID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PARAMETER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PARAMETER](
	[ParameterID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PARAMETERDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PARAMETERDESCRIPTION](
	[ParameterDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[ParameterID] [int] NOT NULL,
	[DescriptionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PARAMETERSFORPROVIDER]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[PARAMETERTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PARAMETERTAG](
	[ParameterTagID] [int] IDENTITY(1,1) NOT NULL,
	[ParameterID] [int] NOT NULL,
	[TagID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PASSWORDQUESTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PASSWORDQUESTION](
	[PasswordQuestionID] [int] IDENTITY(1,1) NOT NULL,
	[Label] [text] NULL,
	[Value] [text] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PATCH]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PATCH](
	[PatchID] [int] IDENTITY(1,1) NOT NULL,
	[PatchGUID] [nvarchar](500) NULL,
	[PatchVocabularyID] [nvarchar](50) NULL,
	[PatchTitle] [nvarchar](50) NULL,
	[PatchDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[RepositoryID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PATCHFILE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PATCHFILE](
	[PatchFileID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PATCHREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PATCHREFERENCE](
	[PatchReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[PatchID] [int] NOT NULL,
	[PatchGUID] [nvarchar](500) NULL,
	[PatchReferenceRelationship] [nvarchar](50) NULL,
	[PatchReferenceDescription] [nvarchar](max) NULL,
	[ReferenceID] [int] NOT NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PATCHREPOSITORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PATCHREPOSITORY](
	[PatchRepositoryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PATTERNFIELDGROUP]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[PATTERNTYPE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[PAYLOAD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PAYLOAD](
	[PayloadID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPayloadID] [int] NULL,
	[PayloadGUID] [nvarchar](500) NULL,
	[PayloadName] [nvarchar](50) NULL,
	[PayloadText] [nvarchar](max) NULL,
	[PayloadDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PCAPFILE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PCAPFILE](
	[PCAPFileID] [int] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PDFFILE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PDFFILE](
	[PDFFileID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERFORMANCEREQUIREMENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERFORMANCEREQUIREMENT](
	[PerformanceRequirementID] [int] IDENTITY(1,1) NOT NULL,
	[RequirementID] [int] NULL,
	[RequirementGUID] [nvarchar](500) NULL,
	[PerformanceRequirementGUID] [nvarchar](500) NULL,
	[PerformanceRequirementTitle] [nvarchar](250) NULL,
	[PerformanceRequirementDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERIMETER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERIMETER](
	[PerimeterID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERIMETERDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERIMETERDESCRIPTION](
	[PerimeterDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERIMETERZONE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERIMETERZONE](
	[PerimeterZoneID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERMISSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERMISSION](
	[PermissionID] [int] IDENTITY(1,1) NOT NULL,
	[PermissionName] [nvarchar](50) NULL,
	[PermissionDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[CreationObjectID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERMISSIONDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERMISSIONDESCRIPTION](
	[PermissionDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[PermissionID] [int] NOT NULL,
	[DescriptionID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSISTENCEPROPERTIES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSISTENCEPROPERTIES](
	[PersistencePropertiesID] [int] IDENTITY(1,1) NOT NULL,
	[PersistencePropertiesName] [nvarchar](50) NULL,
	[PersistencePropertiesDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSISTENCESTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSISTENCESTRATEGICOBJECTIVE](
	[PersistenceStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[PersistenceStrategicObjectiveName] [nvarchar](50) NULL,
	[PersistenceStrategicObjectiveDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSISTENCETACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSISTENCETACTICALOBJECTIVE](
	[PersistenceTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[PersistenceTacticalObjectiveName] [nvarchar](50) NULL,
	[PersistenceTacticalObjectiveDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSON]    Script Date: 04/03/2015 19:59:09 ******/
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
	[FullName] [nvarchar](100) NULL,
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
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONASSURANCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONASSURANCE](
	[PersonAssuranceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONBLACKLIST]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[PERSONCERTIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONCERTIFICATION](
	[PersonCertificationID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NULL,
	[CertificationID] [int] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONDEVICE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[PERSONDOMAINNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONDOMAINNAME](
	[PersonDomainNameID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[PersonDomainNameRelationship] [nvarchar](50) NULL,
	[DomainNameID] [int] NULL,
	[DomainNameGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONFORAPPLICATION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[PERSONFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[PERSONFORINCIDENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONFORINCIDENT](
	[IncidentPersonID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[IncidentID] [int] NOT NULL,
	[IncidentPersonRole] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONFORORGANISATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONFORORGANISATION](
	[PersonOrganisationID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[relationshiptype] [nvarchar](50) NOT NULL,
	[ScheduleID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[RACIValue] [nvarchar](50) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONFORPERSONGROUP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONFORPERSONGROUP](
	[PersonGroupPersonID] [int] IDENTITY(1,1) NOT NULL,
	[PersonGroupID] [int] NOT NULL,
	[PersonID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONFORPROJECT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONFORPROJECT](
	[ProjectPersonID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[ProjectGUID] [nvarchar](500) NULL,
	[PersonID] [int] NOT NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[PersonRole] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[TrustLevelID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONFORTHREATACTORTTP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONFORTHREATACTORTTP](
	[ThreatActorTTPPersonID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatActorTTPPersonGUID] [nvarchar](500) NULL,
	[PersonID] [int] NOT NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[ThreatActorTTPPersonRelationship] [nvarchar](50) NULL,
	[ThreatActorTTPID] [int] NOT NULL,
	[ThreatActorTTPGUID] [nvarchar](500) NULL,
	[Information_Source] [nvarchar](250) NULL,
	[ConfidenceLevel] [nvarchar](50) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[notes] [nvarchar](max) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONGEOLOCATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONGEOLOCATION](
	[PersonGeoLocationID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[GeoLocationID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONGROUP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONGROUP](
	[PersonGroupID] [int] IDENTITY(1,1) NOT NULL,
	[PersonGroupName] [nvarchar](100) NOT NULL,
	[PersonGroupDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[TrustLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONLICENSE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONLICENSE](
	[PersonLicenseID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[LicenseID] [int] NOT NULL,
	[LicenseGUID] [nvarchar](500) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONPERMISSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONPERMISSION](
	[PersonPermissionID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[PermissionID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONPHYSICALLOCATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONPHYSICALLOCATION](
	[PersonPhysicalLocationID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[PhysicalLocationID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONSCHEDULE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONSCHEDULE](
	[PersonScheduleID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONSKILL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONSKILL](
	[PersonSkillID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[SkillID] [int] NULL,
	[SkillGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONTAG](
	[PersonTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PERSONWHITELIST]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[PERSONWORKINGHOURS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERSONWORKINGHOURS](
	[PersonWorkingHoursID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PGPSIGNATURE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PGPSIGNATURE](
	[PGPSignatureID] [int] IDENTITY(1,1) NOT NULL,
	[SignatureID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PHASE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PHASE](
	[PhaseID] [int] IDENTITY(1,1) NOT NULL,
	[PhaseGUID] [nvarchar](500) NULL,
	[PhaseName] [nvarchar](150) NULL,
	[PhaseDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PHASEMAPPING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PHASEMAPPING](
	[PhaseMappingID] [int] IDENTITY(1,1) NOT NULL,
	[PhaseRefID] [int] NULL,
	[PhaseRefGUID] [nvarchar](500) NULL,
	[PhaseRelationship] [nvarchar](50) NULL,
	[PhaseMappingDescription] [nvarchar](max) NULL,
	[PhaseSubjectID] [int] NULL,
	[PhaseSubjectGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PHASETAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PHASETAG](
	[PhaseTagID] [int] IDENTITY(1,1) NOT NULL,
	[PhaseID] [int] NULL,
	[PhaseGUID] [nvarchar](500) NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PHONECALL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PHONECALL](
	[PhoneCallID] [int] IDENTITY(1,1) NOT NULL,
	[TelephoneCallID] [int] NULL,
	[duration] [time](7) NULL,
	[isSpam] [bit] NULL,
	[isSocialEngineering] [bit] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PHONECALLTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PHONECALLTAG](
	[PhoneCallTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PHYSICALLOCATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PHYSICALLOCATION](
	[PhysicalLocationID] [int] IDENTITY(1,1) NOT NULL,
	[PhysicalLocationName] [nvarchar](200) NOT NULL,
	[PhysicalLocationDescription] [nvarchar](max) NULL,
	[TrustLevelID] [int] NULL,
	[VocabularyID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PHYSICALLOCATIONASSURANCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PHYSICALLOCATIONASSURANCE](
	[PhysicalLocationAssuranceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PHYSICALLOCATIONCLASSIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PHYSICALLOCATIONCLASSIFICATION](
	[PhysicalLocationClassificationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PHYSICALLOCATIONCONTROL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PHYSICALLOCATIONCONTROL](
	[PhysicalLocationControlID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PHYSICALLOCATIONDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PHYSICALLOCATIONDESCRIPTION](
	[PhysicalLocationDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[PhysicalLocationID] [int] NOT NULL,
	[PhysicalLocationGUID] [nvarchar](500) NULL,
	[DescriptionID] [int] NULL,
	[DescriptionGUID] [nvarchar](500) NULL,
	[ConfidentialityLevelID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PHYSICALLOCATIONRESTRICTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PHYSICALLOCATIONRESTRICTION](
	[PhysicalLocationRestrictionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PHYSICALLOCATIONSECURITYCONTROL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PHYSICALLOCATIONSECURITYCONTROL](
	[PhysicalLocationSecurityControlID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PHYSICALLOCATIONTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PHYSICALLOCATIONTAG](
	[PhysicalLocationTagID] [int] IDENTITY(1,1) NOT NULL,
	[PhysicalLocationID] [int] NULL,
	[PhysicalLocationGUID] [nvarchar](500) NULL,
	[TagID] [int] NULL,
	[TagGUID] [nvarchar](500) NULL,
	[ConfidentialityLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PHYSIOLOGICALCHARACTERISTIC]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PHYSIOLOGICALCHARACTERISTIC](
	[PhysiologicalCharacteristicID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PIPEOBJECT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PIPEOBJECT](
	[PipeObjectID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PKI]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PKI](
	[PKIID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PLAN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PLAN](
	[PlanID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PLATFORM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PLATFORM](
	[PlatformID] [int] IDENTITY(1,1) NOT NULL,
	[PlatformGUID] [nvarchar](500) NULL,
	[PlatformName] [nvarchar](50) NOT NULL,
	[VocabularyID] [int] NULL,
	[PlatformDescription] [nvarchar](max) NULL,
	[structuring_format] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PLATFORMFORCCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PLATFORMFORCCE](
	[CCEPlatformID] [int] IDENTITY(1,1) NOT NULL,
	[CCEID] [int] NULL,
	[PlatformID] [int] NOT NULL,
	[cce_id] [nvarchar](20) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PLATFORMFORTECHNICALCONTEXT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PLATFORMFORTECHNICALCONTEXT](
	[TechnicalContextPlatformID] [int] IDENTITY(1,1) NOT NULL,
	[TechnicalContextPlatformGUID] [nvarchar](500) NULL,
	[PlatformID] [int] NOT NULL,
	[PlatformGUID] [nvarchar](500) NULL,
	[TechnicalContextID] [int] NOT NULL,
	[TechnicalContextGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PLATFORMMAPPING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PLATFORMMAPPING](
	[PlatformMappingID] [int] IDENTITY(1,1) NOT NULL,
	[PlaformRefID] [int] NULL,
	[PlatformRefGUID] [nvarchar](500) NULL,
	[PlatformRelationship] [nvarchar](50) NULL,
	[PlatformSubjectID] [int] NULL,
	[PlatformSubjectGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PLATFORMSPECIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PLATFORMSPECIFICATION](
	[PlatformSpecificationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PLATFORMTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PLATFORMTAG](
	[PlatformTagID] [int] IDENTITY(1,1) NOT NULL,
	[PlatformID] [int] NULL,
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

/****** Object:  Table [dbo].[PLUGIN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PLUGIN](
	[PluginID] [int] IDENTITY(1,1) NOT NULL,
	[PluginGUID] [nvarchar](500) NULL,
	[PluginName] [nvarchar](500) NULL,
	[PluginDescription] [nvarchar](max) NULL,
	[ModuleID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PLUGINPARAMETER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PLUGINPARAMETER](
	[PluginParameterID] [int] IDENTITY(1,1) NOT NULL,
	[PluginID] [int] NOT NULL,
	[ParameterID] [int] NOT NULL,
	[ordinal_position] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PLUGINREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PLUGINREFERENCE](
	[PluginReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[PluginID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PLUGINTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PLUGINTAG](
	[PluginTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PLUGINVERSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PLUGINVERSION](
	[PluginVersionID] [int] IDENTITY(1,1) NOT NULL,
	[PluginID] [int] NOT NULL,
	[VersionID] [int] NOT NULL,
	[PluginVersionDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[POLICY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[POLICY](
	[PolicyID] [int] IDENTITY(1,1) NOT NULL,
	[PolicyName] [nvarchar](500) NULL,
	[PolicyDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[POLICYTERM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[POLICYTERM](
	[PolicyTermID] [int] IDENTITY(1,1) NOT NULL,
	[AcronymID] [int] NULL,
	[PolicyTerm] [nvarchar](500) NOT NULL,
	[PolicyTermDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[POLICYTERMFORPOLICY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[POLICYTERMFORPOLICY](
	[PolicyTermForPolicyID] [int] IDENTITY(1,1) NOT NULL,
	[PolicyID] [int] NOT NULL,
	[PolicyTermID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PORT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PORT](
	[PortID] [int] IDENTITY(1,1) NOT NULL,
	[Port_Value] [int] NOT NULL,
	[ProtocolID] [int] NULL,
	[DefaultProtocolName] [nchar](100) NULL,
	[DefaultServiceName] [nchar](100) NULL,
	[PortName] [nvarchar](50) NULL,
	[PortDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PORTFOREXPLOIT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PORTFOREXPLOIT](
	[ExploitPortID] [int] IDENTITY(1,1) NOT NULL,
	[ExploitID] [int] NOT NULL,
	[ExploitGUID] [nvarchar](500) NULL,
	[ExploitPortRelationship] [nvarchar](50) NULL,
	[PortID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PORTFORVULNERABILITY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PORTFORVULNERABILITY](
	[VulnerabilityPortID] [int] IDENTITY(1,1) NOT NULL,
	[VulnerabilityID] [int] NOT NULL,
	[VulnerabilityGUID] [nvarchar](500) NULL,
	[VulnerabilityPortRelationship] [nvarchar](50) NULL,
	[PortID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[POSSIBLERESTRICTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[POSSIBLERESTRICTION](
	[PossibleRestrictionID] [int] IDENTITY(1,1) NOT NULL,
	[RestrictionHint] [nvarchar](500) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[POSTALADDRESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[POSTALADDRESS](
	[PostalAddressID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PREVALENCE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[PRIORITYLEVEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRIORITYLEVEL](
	[PriorityLevelID] [int] IDENTITY(1,1) NOT NULL,
	[PriorityLevelName] [nvarchar](50) NULL,
	[PriotityCode] [nvarchar](5) NULL,
	[Sequencing] [nvarchar](5) NULL,
	[PriorityLevelDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRIVACYNOTIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRIVACYNOTIFICATION](
	[PrivacyNotificationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRIVACYRULE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRIVACYRULE](
	[PrivacyRuleID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRIVILEGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRIVILEGE](
	[PrivilegeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRIVILEGEESCALATIONPROPERTIES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRIVILEGEESCALATIONPROPERTIES](
	[PrivilegeEscalationPropertiesID] [int] IDENTITY(1,1) NOT NULL,
	[PrivilegeEscalationPropertiesName] [nvarchar](50) NULL,
	[PrivilegeEscalationPropertiesDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRIVILEGEESCALATIONSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRIVILEGEESCALATIONSTRATEGICOBJECTIVE](
	[PrivilegeEscalationStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[PrivilegeEscalationStrategicObjectiveName] [nvarchar](50) NULL,
	[PrivilegeEscalationStrategicObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRIVILEGEESCALATIONTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRIVILEGEESCALATIONTACTICALOBJECTIVE](
	[PrivilegeEscalationTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[PrivilegeEscalationTacticalObjectiveName] [nvarchar](50) NULL,
	[PrivilegeEscalationTacticalObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRIVILEGESFORROLE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[PROBINGSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROBINGSTRATEGICOBJECTIVE](
	[ProbingStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[ProbingStrategicObjectiveName] [nvarchar](50) NULL,
	[ProbingStrategicObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROBINGTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROBINGTACTICALOBJECTIVE](
	[ProbingTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[ProbingTacticalObjectiveName] [nvarchar](50) NULL,
	[ProbingTacticalObjectiveDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROBINGTECHNIQUE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROBINGTECHNIQUE](
	[ProbingTechniqueID] [int] IDENTITY(1,1) NOT NULL,
	[ProbingTechniqueGUID] [nvarchar](500) NULL,
	[TechniqueID] [int] NULL,
	[ProbingTechniqueName] [nvarchar](150) NULL,
	[ProbingTechniqueDescription] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROBINGTECHNIQUEFORATTACKPATTERN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROBINGTECHNIQUEFORATTACKPATTERN](
	[AttackPatternProbingTechniqueID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternID] [int] NOT NULL,
	[capec_id] [nvarchar](20) NULL,
	[ProbingTechniqueID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROCEDURE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROCEDURE](
	[ProcedureID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROCESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROCESS](
	[ProcessID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROCESSACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROCESSACTIONNAME](
	[ProcessActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessActionNameName] [nvarchar](150) NOT NULL,
	[ProcessActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROCESSMEMORYACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROCESSMEMORYACTIONNAME](
	[ProcessMemoryActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessMemoryActionNameName] [nvarchar](150) NOT NULL,
	[ProcessMemoryActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROCESSORTYPE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[PROCESSORTYPEMAPPING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROCESSORTYPEMAPPING](
	[ProcessorTypeMappingID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessorTypeRefID] [int] NOT NULL,
	[ProcessorTypeSubjectID] [int] NOT NULL,
	[ConfidenceLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROCESSORTYPEREGISTER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROCESSORTYPEREGISTER](
	[ProcessorTypeID] [int] NOT NULL,
	[RegisterID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROCESSTHREADACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROCESSTHREADACTIONNAME](
	[ProcessThreadActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessThreadActionNameName] [nvarchar](150) NOT NULL,
	[ProcessThreadActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRODUCT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRODUCT](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductGUID] [nvarchar](500) NULL,
	[ProductName] [nvarchar](150) NULL,
	[ProductVendor] [nvarchar](150) NULL,
	[OrganisationID] [int] NULL,
	[CPEName] [nvarchar](255) NULL,
	[ProductEdition] [nvarchar](50) NULL,
	[ProductUpdate] [nvarchar](50) NULL,
	[ProductVersion] [nvarchar](50) NULL,
	[CPEID] [int] NULL,
	[ProductLanguage] [nvarchar](50) NULL,
	[LocaleID] [int] NULL,
	[DeviceID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ProductDescription] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRODUCTCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRODUCTCATEGORY](
	[ProductCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ProductCategoryGUID] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[ProductCategoryName] [nvarchar](50) NULL,
	[ProductCategoryShortName] [nvarchar](10) NULL,
	[ProductCategoryDescription] [nvarchar](max) NULL,
	[OrganisationID] [int] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRODUCTCATEGORYFORPRODUCT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRODUCTCATEGORYFORPRODUCT](
	[ProductCategoryForProductID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[ProductGUID] [nvarchar](500) NULL,
	[ProductCategoryID] [int] NOT NULL,
	[ProductCategoryGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRODUCTEXPLOIT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRODUCTEXPLOIT](
	[ProductExploitID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[ProductGUID] [nvarchar](500) NULL,
	[ExploitID] [int] NULL,
	[ExploitGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRODUCTFILE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRODUCTFILE](
	[ProductFileID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[ProductFileRelationship] [nvarchar](50) NULL,
	[ProductFileDescription] [nvarchar](max) NULL,
	[FileID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRODUCTFILELIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRODUCTFILELIST](
	[ProductFileListID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[ProductGUID] [nvarchar](500) NULL,
	[ProductFileListRelationship] [nvarchar](50) NULL,
	[ProductFileListDescription] [nvarchar](max) NULL,
	[FileListID] [int] NULL,
	[FileListGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRODUCTMAPPING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRODUCTMAPPING](
	[ProductMappingID] [int] IDENTITY(1,1) NOT NULL,
	[ProductRefID] [int] NULL,
	[ProductRefGUID] [nvarchar](500) NULL,
	[ProductRelationship] [nvarchar](50) NULL,
	[ProductMappingDescription] [nvarchar](max) NULL,
	[ProductSubjectID] [int] NULL,
	[ProductSubjectGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRODUCTPATCH]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRODUCTPATCH](
	[ProductPatchID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[ProductGUID] [nvarchar](500) NULL,
	[ProductPatchRelationship] [nvarchar](50) NULL,
	[ProductPatchDescription] [nvarchar](max) NULL,
	[PatchID] [int] NULL,
	[PatchGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRODUCTPLATFORM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRODUCTPLATFORM](
	[ProductPlaformID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[ProductGUID] [nvarchar](500) NULL,
	[ProductPlatformRelationship] [nvarchar](50) NULL,
	[PlatformID] [int] NULL,
	[PlatformGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromdate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRODUCTPORT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRODUCTPORT](
	[ProductPortID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[ProductGUID] [nvarchar](500) NULL,
	[ProductPortRelationship] [nvarchar](50) NULL,
	[PortID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PRODUCTTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRODUCTTAG](
	[ProductTagID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECT](
	[ProjectID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectGUID] [nvarchar](500) NULL,
	[ProjectName] [nvarchar](500) NOT NULL,
	[ProjectDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ExpectedCompletionDate] [datetimeoffset](7) NULL,
	[DueDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidentialityLevelID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ImportanceID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECTDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECTDESCRIPTION](
	[ProjectDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NULL,
	[ProjectGUID] [nvarchar](500) NULL,
	[DescriptionID] [int] NULL,
	[DescriptionGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECTFINDING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECTFINDING](
	[ProjectFindingID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NULL,
	[ProjectGUID] [nvarchar](500) NULL,
	[FindingID] [int] NULL,
	[FindingGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECTFORAPPLICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECTFORAPPLICATION](
	[ProjectApplicationID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[ProjectGUID] [nvarchar](500) NULL,
	[ApplicationID] [int] NOT NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[PersonID] [int] NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[ProjectDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[OrganisationID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECTMAPPING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECTMAPPING](
	[ProjectMappingID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectRefID] [int] NULL,
	[ProjectRefGUID] [nvarchar](500) NULL,
	[ProjectRelationship] [nvarchar](50) NULL,
	[ProjectMappingDescription] [nvarchar](max) NULL,
	[ProjectSubjectID] [int] NULL,
	[ProjectSubjectGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECTMETHODOLOGY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECTMETHODOLOGY](
	[ProjectMethodologyID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[ProjectGUID] [nvarchar](500) NULL,
	[MethodologyID] [int] NOT NULL,
	[MethodologyGUID] [nvarchar](500) NULL,
	[PersonID] [int] NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ProjectMethodologyDescription] [nvarchar](max) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECTPERSON]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECTPERSON](
	[ProjectPersonID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[ProjectGUID] [nvarchar](500) NULL,
	[PersonID] [int] NOT NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[ProjectPersonRole] [nvarchar](100) NULL,
	[ProjectPersonDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECTTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECTTAG](
	[ProjectTagID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NULL,
	[ProjectGUID] [nvarchar](500) NULL,
	[TagID] [int] NULL,
	[TagGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECTTASK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECTTASK](
	[ProjectTaskID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[ProjectGUID] [nvarchar](500) NULL,
	[TaskID] [int] NOT NULL,
	[TaskGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ProjectTaskName] [nvarchar](250) NULL,
	[ProjectTaskDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECTTASKFINDING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECTTASKFINDING](
	[ProjectTaskFindingID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectTaskID] [int] NULL,
	[ProjectTaskGUID] [nvarchar](500) NULL,
	[FindingID] [int] NULL,
	[FindingGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECTTASKPERSON]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECTTASKPERSON](
	[ProjectTaskPersonID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectTaskID] [int] NOT NULL,
	[ProjectTaskGUID] [nvarchar](500) NULL,
	[PersonID] [int] NOT NULL,
	[PersonGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ProjectTaskPersonRole] [nvarchar](100) NULL,
	[ProjectTaskDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROJECTTECHNIQUE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROJECTTECHNIQUE](
	[ProjectTechniqueID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROPERTYTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROPERTYTYPE](
	[PropertyTypeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROTOCOL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROTOCOL](
	[ProtocolID] [int] IDENTITY(1,1) NOT NULL,
	[ProtocolAbbreviation] [nvarchar](20) NULL,
	[ProtocolName] [nvarchar](100) NOT NULL,
	[ProtocolDescription] [nvarchar](max) NULL,
	[ProtocolRFC] [nvarchar](10) NULL,
	[ProtocolBAF] [nvarchar](10) NULL,
	[VocabularyID] [int] NULL,
	[OSILayerID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROTOCOLCOMMAND]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROTOCOLCOMMAND](
	[ProtocolCommandID] [int] IDENTITY(1,1) NOT NULL,
	[ProtocolID] [int] NULL,
	[CommandID] [int] NULL,
	[KnownVulnerable] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROTOCOLFORPROTOCOL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROTOCOLFORPROTOCOL](
	[ProtocolRelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[ProtocolRefID] [int] NOT NULL,
	[ProtocolRelationshipName] [nvarchar](50) NULL,
	[ProtocolSubjectID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROTOCOLHEADER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROTOCOLHEADER](
	[ProtocolHeaderID] [int] IDENTITY(1,1) NOT NULL,
	[ProtocolHeaderGUID] [nvarchar](500) NULL,
	[Protocol_Field_Name] [nvarchar](50) NULL,
	[Protocol_Field_Description] [nvarchar](max) NULL,
	[Protocol_Operation_Code] [nvarchar](5) NULL,
	[Protocol_Data] [nvarchar](max) NULL,
	[Protocol_Flag_Value] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROTOCOLREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROTOCOLREFERENCE](
	[ProtocolReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[ProtocolID] [int] NULL,
	[ReferenceID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROVIDER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PROVIDER](
	[ProviderID] [int] IDENTITY(1,1) NOT NULL,
	[ProviderGUID] [nvarchar](500) NULL,
	[ProviderName] [nvarchar](50) NULL,
	[PluginReference] [nvarchar](50) NULL,
	[ServiceCategoryID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PROVIDERSFORACCOUNT]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[RACIMATRIX]    Script Date: 04/03/2015 19:59:09 ******/
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
	[AccountID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RACITASK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RACITASK](
	[RACITaskID] [int] IDENTITY(1,1) NOT NULL,
	[TaskType] [nvarchar](max) NULL,
	[RACIResponsability] [nvarchar](50) NULL,
	[UserID] [uniqueidentifier] NULL,
	[AccountID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RATEFILTER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RATEFILTER](
	[RateFilterID] [int] IDENTITY(1,1) NOT NULL,
	[RateFilterContent] [nvarchar](max) NULL,
	[RateFilterDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RAWARTIFACT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RAWARTIFACT](
	[RawArtifactID] [int] IDENTITY(1,1) NOT NULL,
	[RawArtifactGUID] [nvarchar](500) NULL,
	[byte_order] [nvarchar](50) NULL,
	[is_encrypted] [bit] NULL,
	[is_compressed] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[SourceID] [int] NULL,
	[RepositoryID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RAWARTIFACTDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RAWARTIFACTDESCRIPTION](
	[RawArtifactDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RAWARTIFACTTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RAWARTIFACTTAG](
	[RawArtifactTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REASON]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REASON](
	[ReasonID] [int] IDENTITY(1,1) NOT NULL,
	[ReasonGUID] [nvarchar](500) NULL,
	[ReasonName] [nvarchar](50) NULL,
	[ReasonDescription] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RECOMMENDATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RECOMMENDATION](
	[RecommendationID] [int] IDENTITY(1,1) NOT NULL,
	[RecommendationGUID] [nvarchar](500) NULL,
	[RecommendationVocabularyID] [nvarchar](250) NULL,
	[RecommendationName] [nvarchar](250) NULL,
	[RecommendationLevel] [nvarchar](250) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[RecommendationDescription] [nvarchar](max) NULL,
	[RecommendationRationale] [nvarchar](max) NULL,
	[RemediationProcedure] [nvarchar](max) NULL,
	[RecommendationImpact] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[StatusID] [int] NULL,
	[ScoringStatusID] [int] NULL,
	[LocaleID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RECOMMENDATIONAUDITPROCEDURE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[RECOMMENDATIONCCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RECOMMENDATIONCCE](
	[RecommendationCCEID] [int] IDENTITY(1,1) NOT NULL,
	[RecommendationID] [int] NOT NULL,
	[RecommendationGUID] [nvarchar](500) NULL,
	[CCEID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RECOMMENDATIONTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RECOMMENDATIONTAG](
	[RecommendationTagID] [int] IDENTITY(1,1) NOT NULL,
	[RecommendationID] [int] NOT NULL,
	[RecommendationGUID] [nvarchar](500) NULL,
	[TagID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RECOMMENDATIONTIP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RECOMMENDATIONTIP](
	[RecommendationTipID] [int] NOT NULL,
	[RecommendationTypeGUID] [nvarchar](500) NULL,
	[RecommendationID] [int] NULL,
	[RecommendationGUID] [nvarchar](500) NULL,
	[TipID] [int] NULL,
	[TipGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REFERENCE](
	[ReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[ReferenceSourceID] [nvarchar](50) NULL,
	[Source] [nvarchar](50) NULL,
	[SourceTrustLevelID] [int] NULL,
	[SourceTrustReasonID] [int] NULL,
	[ReferenceTitle] [nvarchar](max) NULL,
	[ReferenceDescription] [nvarchar](max) NULL,
	[Type] [nvarchar](50) NULL,
	[ReferenceCategoryID] [int] NULL,
	[ReferenceURL] [nvarchar](max) NULL,
	[ReferenceFilePath] [nvarchar](500) NULL,
	[lang] [nvarchar](10) NULL,
	[LocaleID] [int] NULL,
	[notes] [nvarchar](max) NULL,
	[ReferenceVersion] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[Reference_Publication] [nvarchar](250) NULL,
	[Reference_Edition] [nvarchar](100) NULL,
	[Reference_PubDate] [nvarchar](50) NULL,
	[Reference_Publisher] [nvarchar](100) NULL,
	[ReferenceISBN] [nvarchar](50) NULL,
	[Reference_Date] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REFERENCEAUTHOR]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REFERENCEAUTHOR](
	[ReferenceAuthorID] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[AuthorID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REFERENCECATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REFERENCECATEGORY](
	[ReferenceCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceCategoryGUID] [nvarchar](500) NULL,
	[CategoryID] [int] NOT NULL,
	[ReferenceCategoryName] [nvarchar](50) NULL,
	[ReferenceCategoryDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REFERENCECATEGORYTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REFERENCECATEGORYTAG](
	[ReferenceCategoryTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REFERENCECHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REFERENCECHANGERECORD](
	[ReferenceChangeRecordID] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[ChangeRecordID] [int] NOT NULL,
	[ChangeRecordGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REFERENCEDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REFERENCEDESCRIPTION](
	[ReferenceDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[DescriptionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REFERENCEMAPPING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REFERENCEMAPPING](
	[ReferenceMappingID] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceRefID] [int] NULL,
	[RelationShipText] [nvarchar](50) NULL,
	[ReferenceSubjectID] [int] NULL,
	[ReferenceMappingDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REFERENCETAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REFERENCETAG](
	[ReferenceTagID] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[TagID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGEX]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGEX](
	[RegexID] [int] IDENTITY(1,1) NOT NULL,
	[RegularExpression] [nvarchar](500) NULL,
	[RegexDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ConfidenceLevelID] [int] NULL,
	[TrustLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGEXCAPTUREFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[REGEXLANGUAGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGEXLANGUAGE](
	[RegexLanguageID] [int] IDENTITY(1,1) NOT NULL,
	[RegexID] [int] NOT NULL,
	[LanguageID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGEXREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGEXREFERENCE](
	[RegexReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[RegexID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGISTER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGISTER](
	[RegisterID] [int] IDENTITY(1,1) NOT NULL,
	[RegisterName] [nvarchar](50) NOT NULL,
	[RegisterDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGISTRYACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGISTRYACTIONNAME](
	[RegistryActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[RegistryActionNameName] [nvarchar](150) NOT NULL,
	[RegistryActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGISTRYDATATYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGISTRYDATATYPE](
	[RegistryDatatypeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGISTRYDATATYPEREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGISTRYDATATYPEREFERENCE](
	[RegistryDatatypeReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[RegistryDatatypeID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGISTRYDATATYPESENUM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGISTRYDATATYPESENUM](
	[RegistryDatatypesEnumID] [int] IDENTITY(1,1) NOT NULL,
	[RegistryDatatypeName] [nvarchar](50) NOT NULL,
	[RegistryDatatypeDescription] [nvarchar](max) NULL,
	[RegistryDatatypeReference] [nvarchar](500) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGISTRYHIVEENUM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGISTRYHIVEENUM](
	[RegistryHiveEnumID] [int] IDENTITY(1,1) NOT NULL,
	[RegistryHiveName] [nvarchar](100) NOT NULL,
	[RegistryHiveDescription] [nvarchar](max) NULL,
	[RegistryHiveReference] [nvarchar](500) NULL,
	[ReferenceID] [int] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGISTRYSUBKEYS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGISTRYSUBKEYS](
	[RegistrySubkeysID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGISTRYSUBKEYSKEYS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGISTRYSUBKEYSKEYS](
	[RegistrySubkeysKeysID] [int] IDENTITY(1,1) NOT NULL,
	[RegistrySubkeysID] [int] NOT NULL,
	[WindowsRegistryKeyObjectID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGISTRYVALUE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGISTRYVALUE](
	[RegistryValueID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NULL,
	[Data] [nvarchar](max) NULL,
	[RegistryDatatypeID] [int] NULL,
	[ByteRunsID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGISTRYVALUES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGISTRYVALUES](
	[RegistryValuesID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGISTRYVALUESREGISTRYVALUE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGISTRYVALUESREGISTRYVALUE](
	[RegistryValuesRegistryValueID] [int] IDENTITY(1,1) NOT NULL,
	[RegistryValuesID] [int] NOT NULL,
	[RegistryValueID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGULAREXPRESSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGULAREXPRESSION](
	[RegularExpressionID] [int] IDENTITY(1,1) NOT NULL,
	[RegexID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REGULATORYRISK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGULATORYRISK](
	[RegulatoryRiskID] [int] IDENTITY(1,1) NOT NULL,
	[RegulatoryRiskGUID] [nvarchar](500) NULL,
	[RiskDescription] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RELATIONSHIPTYPE]    Script Date: 04/03/2015 19:59:09 ******/
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
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RELIABILITY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RELIABILITY](
	[ReliabilityID] [int] IDENTITY(1,1) NOT NULL,
	[ReliabilityGUID] [nvarchar](500) NULL,
	[ReliabilityName] [nvarchar](50) NULL,
	[ReliabilityDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RELIABILITYREASON]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RELIABILITYREASON](
	[ReliabilityReasonID] [int] IDENTITY(1,1) NOT NULL,
	[ReliabilityReasonGUID] [nvarchar](500) NULL,
	[ReasonID] [int] NULL,
	[ReasonGUID] [nvarchar](500) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REMOTEMACHINEMANIPULATIONSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REMOTEMACHINEMANIPULATIONSTRATEGICOBJECTIVE](
	[RemoteMachineManipulationStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[RemoteMachineManipulationStrategicObjectiveName] [nvarchar](50) NULL,
	[RemoteMachineManipulationStrategicObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REMOTEMACHINEMANIPULATIONTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REMOTEMACHINEMANIPULATIONTACTICALOBJECTIVE](
	[RemoteMachineManipulationTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[RemoteMachineManipulationTacticalObjectiveName] [nvarchar](50) NULL,
	[RemoteMachineManipulationTacticalObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REPORT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REPORT](
	[ReportID] [int] IDENTITY(1,1) NOT NULL,
	[ReportGUID] [nvarchar](500) NULL,
	[ReportContent] [nvarchar](max) NULL,
	[ReferenceID] [int] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REPORTFORREPORTS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REPORTFORREPORTS](
	[ReportsID] [int] NOT NULL,
	[ReportID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REPORTREQUEST]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[REPORTREQUESTFORREPORTREQUESTS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REPORTREQUESTFORREPORTREQUESTS](
	[ReportRequestsID] [int] NOT NULL,
	[ReportRequestID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REPORTREQUESTS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REPORTREQUESTS](
	[ReportRequestsID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REPORTS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REPORTS](
	[ReportsID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REPOSITORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REPOSITORY](
	[RepositoryID] [int] IDENTITY(1,1) NOT NULL,
	[RepositoryGUID] [nvarchar](500) NULL,
	[RepositoryName] [nvarchar](250) NULL,
	[RepositoryDescription] [nvarchar](max) NULL,
	[RepositoryURL] [nvarchar](500) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REPOSITORYRESTRICTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REPOSITORYRESTRICTION](
	[RepositoryRestrictionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REPUTATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REPUTATION](
	[ReputationID] [int] IDENTITY(1,1) NOT NULL,
	[ReputationGUID] [nvarchar](500) NULL,
	[ReputationTitle] [nvarchar](50) NULL,
	[ReputationDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REQUIREMENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REQUIREMENT](
	[RequirementID] [int] IDENTITY(1,1) NOT NULL,
	[RequirementGUID] [nvarchar](500) NULL,
	[RequirementTitle] [nvarchar](250) NULL,
	[RequirementDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REQUIREMENTCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REQUIREMENTCATEGORY](
	[RequirementCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[RequirementID] [int] NULL,
	[CategoryID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REQUIREMENTDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REQUIREMENTDESCRIPTION](
	[RequirementDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[RequirementID] [int] NULL,
	[DescriptionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[REQUIREMENTMAPPING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REQUIREMENTMAPPING](
	[RequirementMappingID] [int] IDENTITY(1,1) NOT NULL,
	[RequirementRefID] [int] NULL,
	[RequirementRefGUID] [nvarchar](500) NULL,
	[RequirementSubjectID] [int] NULL,
	[RequirementSubjectGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RESTRICTION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[RESULTENUMERATION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[RISKRATING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RISKRATING](
	[RiskRatingID] [int] IDENTITY(1,1) NOT NULL,
	[RiskRatingGUID] [nvarchar](500) NULL,
	[RiskRatingName] [nvarchar](250) NULL,
	[RiskRatingDescription] [nvarchar](max) NULL,
	[MethodologyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ROLE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ROLE](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleGUID] [nvarchar](500) NULL,
	[RoleName] [nvarchar](100) NULL,
	[RoleDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ROPCHAIN]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[ROPCHAININSTRUCTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ROPCHAININSTRUCTION](
	[ROPChainID] [int] NOT NULL,
	[InstructionID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ROPCHAINREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[ROPGADGET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ROPGADGET](
	[ROPGadgetID] [int] IDENTITY(1,1) NOT NULL,
	[ROPGadgetGUID] [nvarchar](500) NULL,
	[ROPGadgetName] [nvarchar](50) NULL,
	[ROPGadgetDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ReliabilityID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ROPGADGETFORROPCHAIN]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[ROPGADGETINSTRUCTION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[ROPGADGETTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ROPGADGETTAG](
	[ROPGadgetTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RSAPUBLICKEY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RSAPUBLICKEY](
	[RSAPublicKeyID] [int] IDENTITY(1,1) NOT NULL,
	[RSAPublicKeyGUID] [nvarchar](500) NULL,
	[Modulus] [nvarchar](50) NOT NULL,
	[Exponent] [int] NOT NULL,
	[isEncrypted] [bit] NULL,
	[CreationDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RSAPUBLICKEYACCESSRECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RSAPUBLICKEYACCESSRECORD](
	[RSAPlublicKeyAccessRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RULE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RULE](
	[RuleID] [int] IDENTITY(1,1) NOT NULL,
	[RuleGUID] [nvarchar](500) NULL,
	[RuleTitle] [nvarchar](250) NULL,
	[RuleVersion] [int] NULL,
	[RuleDescription] [nvarchar](max) NULL,
	[RuleContent] [nvarchar](max) NULL,
	[RuleVocabularyID] [nvarchar](10) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RULECATEGORIES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RULECATEGORIES](
	[RuleCategoriesID] [int] IDENTITY(1,1) NOT NULL,
	[RuleID] [int] NULL,
	[RuleCategoryID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RULECATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RULECATEGORY](
	[RuleCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NULL,
	[RuleCategoryName] [nvarchar](50) NULL,
	[RuleCategoryDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RULEPRODUCT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RULEPRODUCT](
	[RuleProductID] [int] IDENTITY(1,1) NOT NULL,
	[RuleID] [int] NULL,
	[RuleProductRelationship] [nvarchar](50) NULL,
	[ProductID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RULEPROTOCOL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RULEPROTOCOL](
	[RuleProtocolID] [int] IDENTITY(1,1) NOT NULL,
	[RuleID] [int] NULL,
	[ProtocolID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RULEREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RULEREFERENCE](
	[RuleReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[RuleID] [int] NULL,
	[ReferenceID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCENARIO]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[SCENARIOFOROWASPTOP10]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCENARIOFOROWASPTOP10](
	[OWASPTOP10ScenarioID] [int] IDENTITY(1,1) NOT NULL,
	[OWASPTOP10ID] [int] NOT NULL,
	[ScenarioID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCHEDULE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCHEDULE](
	[ScheduleID] [int] NOT NULL,
	[ScheduleGUID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCHEMA]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCHEMA](
	[SchemaID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCORINGFORMULA]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[SCORINGSTATUS]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[SCORINGSYSTEM]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[SCORINGSYSTEMDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCORINGSYSTEMDESCRIPTION](
	[ScoringSystemDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCORINGSYSTEMFORMULAS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCORINGSYSTEMFORMULAS](
	[ScoringSystemID] [int] NOT NULL,
	[ScoringFormulaID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCORINGSYSTEMREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[SCORINGSYSTEMTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCORINGSYSTEMTAG](
	[ScoringSystemTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCRIPT]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[SCRIPTDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCRIPTDESCRIPTION](
	[ScriptDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[ScriptID] [int] NOT NULL,
	[DescriptionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCRIPTTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCRIPTTAG](
	[ScriptTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCRIPTVERSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCRIPTVERSION](
	[ScriptVersionID] [int] IDENTITY(1,1) NOT NULL,
	[ScriptID] [int] NOT NULL,
	[VersionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECONDARYOPERATIONPROPERTIES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECONDARYOPERATIONPROPERTIES](
	[SecondaryOperationPropertiesID] [int] IDENTITY(1,1) NOT NULL,
	[SecondaryOperationPropertiesName] [nvarchar](50) NULL,
	[SecondaryOperationPropertiesDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECONDARYOPERATIONSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECONDARYOPERATIONSTRATEGICOBJECTIVE](
	[SecondaryOperationStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[SecondaryOperationStrategicObjectiveName] [nvarchar](50) NULL,
	[SecondaryOperationStrategicObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECONDARYOPERATIONTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECONDARYOPERATIONTACTICALOBJECTIVE](
	[SecondaryOperationTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[SecondaryOperationTacticalObjectiveName] [nvarchar](50) NULL,
	[SecondaryOperationTacticalObjectiveDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECTION]    Script Date: 04/03/2015 19:59:09 ******/
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
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECTIONDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECTIONDESCRIPTION](
	[SectionDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[SectionID] [int] NOT NULL,
	[DescriptionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECTIONREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECTIONREFERENCE](
	[SectionReferenceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECTIONTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECTIONTAG](
	[SectionTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYATTRIBUTE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[SECURITYATTRIBUTECATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[SECURITYATTRIBUTESTATE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[SECURITYATTRIBUTEVARIETY]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[SECURITYCHANGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCHANGE](
	[SecurityChangeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCOMPROMISEENUM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCOMPROMISEENUM](
	[SecurityCompromiseEnumID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityCompromiseEnumName] [nvarchar](50) NULL,
	[SecurityCompromiseEnumDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROL](
	[SecurityControlID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityControlGUID] [nvarchar](500) NULL,
	[ControlID] [int] NULL,
	[SecurityControlName] [nvarchar](250) NOT NULL,
	[SecurityControlAbbrevation] [nvarchar](20) NULL,
	[SecurityControlDescription] [nvarchar](max) NULL,
	[BaselineImpact] [nvarchar](10) NULL,
	[StatementDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[SecurityControlVocabularyID] [nvarchar](50) NULL,
	[SecurityControlFamilyID] [int] NULL,
	[SecurityControlParentID] [int] NULL,
	[SecurityControlTypeID] [int] NULL,
	[RepositoryID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ReliabilityID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLCHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLCHANGERECORD](
	[SecurityControlChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLDESCRIPTION](
	[SecurityControlDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLENVIRONMENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLENVIRONMENT](
	[SecurityControlEnvironmentID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLFAMILY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLFAMILY](
	[SecurityControlFamilyID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityControlFamilyName] [nvarchar](50) NOT NULL,
	[SecurityControlFamilyDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLFAMILYTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLFAMILYTAG](
	[SecurityControlFamilyTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLFORHUMANRISK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLFORHUMANRISK](
	[HumanRiskSecurityControlID] [int] IDENTITY(1,1) NOT NULL,
	[HumanRiskID] [int] NOT NULL,
	[SecurityControlID] [int] NOT NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLMAPPING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLMAPPING](
	[SecurityControlMappingID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityControlRefID] [int] NOT NULL,
	[SecurityControlRefGUID] [nvarchar](500) NULL,
	[SecurityControlRelationship] [nvarchar](50) NULL,
	[SecurityControlMappingDescription] [nvarchar](max) NULL,
	[SecurityControlSubjectID] [int] NOT NULL,
	[SecurityControlSubjectGUID] [nvarchar](500) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLPRIORITY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLPRIORITY](
	[SecurityControlPriorityID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityControlID] [int] NULL,
	[PriorityLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLREFERENCE](
	[SecurityControlReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityControlID] [int] NULL,
	[SecurityControlGUID] [nvarchar](500) NULL,
	[ReferenceID] [int] NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLSTRENGTH]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLSTRENGTH](
	[SecurityControlStrenghtID] [int] NOT NULL,
	[SecurityControlID] [int] NOT NULL,
	[ControlStrengthID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLTAG](
	[SecurityControlTagID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityControlID] [int] NULL,
	[SecurityControlGUID] [nvarchar](500) NULL,
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

/****** Object:  Table [dbo].[SECURITYCONTROLTEST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLTEST](
	[SecurityControlTestID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityControlTestGUID] [nvarchar](500) NULL,
	[SecurityControlID] [int] NULL,
	[SecurityControlGUID] [nvarchar](500) NULL,
	[TestID] [int] NULL,
	[TestGUID] [nvarchar](500) NULL,
	[TestVocabularyID] [nvarchar](50) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLTOOL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLTOOL](
	[SecurityControlToolID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityControlID] [int] NOT NULL,
	[SecuriyControlGUID] [nvarchar](500) NULL,
	[RelationshipName] [nvarchar](50) NULL,
	[ToolInformationID] [int] NOT NULL,
	[ToolInformationGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLTYPE](
	[SecurityControlTypeID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityControlTypeName] [nvarchar](50) NULL,
	[SecurityControlTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYCONTROLTYPETAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYCONTROLTYPETAG](
	[SecurityControlTypeTagID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityControlTypeID] [int] NULL,
	[TagID] [int] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYDEGRADATIONPROPERTIES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYDEGRADATIONPROPERTIES](
	[SecurityDegradationPropertiesID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityDegradationPropertiesName] [nvarchar](50) NULL,
	[SecurityDegradationPropertiesDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYDEGRADATIONSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYDEGRADATIONSTRATEGICOBJECTIVE](
	[SecurityDegradationStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityDegradationStrategicObjectiveName] [nvarchar](50) NULL,
	[SecurityDegradationStrategicObjectiveDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYDEGRADATIONTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYDEGRADATIONTACTICALOBJECTIVE](
	[SecurityDegradationTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityDegradationTacticalObjectiveName] [nvarchar](50) NULL,
	[SecurityDegradationTacticalObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYDOMAIN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYDOMAIN](
	[SecurityDomainID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityDomainGUID] [nvarchar](500) NULL,
	[SecurityDomainName] [nvarchar](100) NULL,
	[SecurityDomainDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYDOMAINMATURITY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYDOMAINMATURITY](
	[SecurityDomainMaturityID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityDomainID] [int] NULL,
	[SecurityDomainGUID] [nvarchar](500) NULL,
	[MaturityLevelID] [int] NULL,
	[MaturityLevelGUID] [nvarchar](500) NULL,
	[OrganisationID] [int] NULL,
	[OrganizationalUnitID] [int] NULL,
	[PersonID] [int] NULL,
	[SecurityDomainMaturityDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYDOMAINOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYDOMAINOBJECTIVE](
	[SecurityDomainObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityDomainID] [int] NULL,
	[SecurityDomainGUID] [nvarchar](500) NULL,
	[ObjectiveID] [int] NULL,
	[ObjectiveGUID] [nvarchar](500) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYDOMAINPROCESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYDOMAINPROCESS](
	[SecurityDomainProcessID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityDomainID] [int] NOT NULL,
	[SecurityDomainGUID] [nvarchar](500) NULL,
	[SecurityProcessID] [int] NOT NULL,
	[SecurityProcessGUID] [nvarchar](500) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYDOMAINTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYDOMAINTAG](
	[SecurityDomainTagID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityDomainID] [int] NULL,
	[SecurityDomainGUID] [nvarchar](500) NULL,
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

/****** Object:  Table [dbo].[SECURITYEVALUATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYEVALUATION](
	[SecurityEvaluationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYLABEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYLABEL](
	[SecurityLabelID] [int] IDENTITY(1,1) NOT NULL,
	[LabelID] [int] NULL,
	[SecurityLabelName] [nvarchar](250) NULL,
	[SecurityLabelDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYLABELREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYLABELREFERENCE](
	[SecurityLabelReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityLabelID] [int] NOT NULL,
	[SecurityLabelGUID] [nvarchar](500) NULL,
	[ReferenceID] [int] NOT NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYMARKING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYMARKING](
	[SecurityMarkingID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYMETRIC]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYMETRIC](
	[SecurityMetricID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityMetricGUID] [nvarchar](500) NULL,
	[SecurityMetricName] [nvarchar](250) NULL,
	[SecurityMetricDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYMETRICDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYMETRICDESCRIPTION](
	[SecurityMetricDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityMetricID] [int] NULL,
	[SecurityMetricGUID] [nvarchar](500) NULL,
	[DescriptionID] [int] NULL,
	[DescriptionGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYMETRICREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYMETRICREFERENCE](
	[SecurityMetricReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityMetricID] [int] NULL,
	[SecurityMetricGUID] [nvarchar](500) NULL,
	[ReferenceID] [int] NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYMETRICTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYMETRICTAG](
	[SecurityMetricTagID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityMetricID] [int] NULL,
	[SecurityMetricGUID] [nvarchar](500) NULL,
	[TagID] [int] NULL,
	[TagGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYNOTIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYNOTIFICATION](
	[SecurityNotificationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYPRINCIPLE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYPRINCIPLE](
	[SecurityPrincipleID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityPrincipleGUID] [nvarchar](500) NULL,
	[SecurityPrincipleName] [nvarchar](250) NOT NULL,
	[SecurityPrincipleDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYPRINCIPLEDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYPRINCIPLEDESCRIPTION](
	[SecurityPrincipleDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityPrincipleID] [int] NULL,
	[SecurityPrincipleGUID] [nvarchar](500) NULL,
	[DescriptionID] [int] NULL,
	[DescriptionGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYPRINCIPLEFORATTACKPATTERN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYPRINCIPLEFORATTACKPATTERN](
	[AttackPatternSecurityPrincipleID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityPrincipleID] [int] NOT NULL,
	[SecurityPrincipleGUID] [nvarchar](500) NULL,
	[AttackPatternID] [int] NOT NULL,
	[AttackPatternGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYPRINCIPLEREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYPRINCIPLEREFERENCE](
	[SecurityPrincipleReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityPrincipleID] [int] NULL,
	[SecurityPrincipleGUID] [nvarchar](500) NULL,
	[ReferenceID] [int] NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYPRINCIPLETAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYPRINCIPLETAG](
	[SecurityPrincipleTagID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityPrincipleTagGUID] [nvarchar](500) NULL,
	[SecurityPrincipleID] [int] NULL,
	[SecurityPrincipleGUID] [nvarchar](500) NULL,
	[TagID] [int] NULL,
	[TagGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYPROCESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYPROCESS](
	[SecurityProcessID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityProcessGUID] [nvarchar](500) NULL,
	[SecurityProcessName] [nvarchar](250) NULL,
	[SecurityProcessDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYPROCESSMATURITYLEVEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYPROCESSMATURITYLEVEL](
	[SecurityProcessMaturityLevelID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityProcessMaturityLevelGUID] [nvarchar](500) NULL,
	[SecurityProcessID] [int] NULL,
	[SecurityProcessGUID] [nvarchar](500) NULL,
	[MaturityLevelID] [int] NULL,
	[MaturityLevelGUID] [nvarchar](500) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYPROGRAM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYPROGRAM](
	[SecurityProgramID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityProgramGUID] [nvarchar](500) NULL,
	[SecurityProgramName] [nvarchar](100) NOT NULL,
	[SecurityProgramDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[SecurityProgramTypeID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYPROGRAMPROJECT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYPROGRAMPROJECT](
	[SecurityProgramProjectID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityProgramID] [int] NULL,
	[SecurityProgramGUID] [nvarchar](500) NULL,
	[ProjectID] [int] NULL,
	[ProjectGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYPROGRAMTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYPROGRAMTYPE](
	[SecurityProgramTypeID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityProgramTypeName] [nvarchar](100) NOT NULL,
	[SecurityProgramTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYREQUIREMENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYREQUIREMENT](
	[SecurityRequirementID] [int] IDENTITY(1,1) NOT NULL,
	[RequirementID] [int] NULL,
	[RequirementGUID] [nvarchar](500) NULL,
	[SecurityRequirementGUID] [nvarchar](500) NULL,
	[SecurityRequirementTitle] [nvarchar](250) NULL,
	[SecurityRequirementDescription] [nvarchar](max) NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYREQUIREMENTCONTROL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYREQUIREMENTCONTROL](
	[SecurityRequirementControlID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityRequirementID] [int] NULL,
	[SecurityRequirementGUID] [nvarchar](500) NULL,
	[SecurityControlID] [int] NULL,
	[SecurityControlGUID] [nvarchar](500) NULL,
	[EffectivenessID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYREQUIREMENTFORATTACKPATTERN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYREQUIREMENTFORATTACKPATTERN](
	[AttackPatternSecurityRequirementID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityRequirementID] [int] NOT NULL,
	[SecurityRequirementGUID] [nvarchar](500) NULL,
	[AttackPatternID] [int] NOT NULL,
	[AttackPatternGUID] [nvarchar](500) NULL,
	[capec_id] [nvarchar](20) NULL,
	[AttackPatternSecurityRequirementDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYREQUIREMENTMAPPING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYREQUIREMENTMAPPING](
	[SecurityRequirementMappingID] [int] IDENTITY(1,1) NOT NULL,
	[AssuranceRequirementID] [int] NULL,
	[AssuranceRequirementGUID] [nvarchar](500) NULL,
	[SecurityRequirementRefID] [int] NOT NULL,
	[SecurityRequirementRefGUID] [nvarchar](500) NULL,
	[SecurityRequirementRelationship] [nvarchar](50) NULL,
	[SecurityRequirementDescription] [nvarchar](max) NULL,
	[SecurityRequirementSubjectID] [int] NULL,
	[SecurityRequirementSubjectGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYREQUIREMENTTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYREQUIREMENTTAG](
	[SecurityRequirementTagID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityRequirementID] [int] NULL,
	[SecurityRequirementGUID] [nvarchar](500) NULL,
	[TagID] [int] NULL,
	[TagGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYREQUIREMENTTEST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYREQUIREMENTTEST](
	[SecurityRequirementTestID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityRequirementTestGUID] [nvarchar](500) NULL,
	[SecurityRequirementID] [int] NULL,
	[SecurityRequirementGUID] [nvarchar](500) NULL,
	[TestID] [int] NULL,
	[TestGUID] [nvarchar](500) NULL,
	[TestVocabularyID] [int] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SECURITYRISKANALYSIS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SECURITYRISKANALYSIS](
	[SecurityRiskAnalysisID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SEMAPHORE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SEMAPHORE](
	[SemaphoreID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SENSOR]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SENSOR](
	[SensorID] [int] IDENTITY(1,1) NOT NULL,
	[SensorGUID] [nvarchar](500) NULL,
	[SensorName] [nvarchar](250) NULL,
	[SensorDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[SensorVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SENSORTOOL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SENSORTOOL](
	[SensorToolID] [int] IDENTITY(1,1) NOT NULL,
	[SensorID] [int] NOT NULL,
	[ToolID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[SensorToolDescription] [nvarchar](max) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SERVICEACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
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
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SERVICECATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
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
	[StatusID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SESSION]    Script Date: 04/03/2015 19:59:09 ******/
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
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SESSIONCOOKIE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SESSIONCOOKIE](
	[SessionCookieID] [int] IDENTITY(1,1) NOT NULL,
	[SessionID] [int] NOT NULL,
	[SessionGUID] [nvarchar](500) NULL,
	[CookieID] [int] NOT NULL,
	[CookieGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[SessionCookieName] [nvarchar](250) NULL,
	[SessionCookieDescription] [nvarchar](max) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SESSIONCOOKIEATTRIBUTEVALUE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SESSIONCOOKIEATTRIBUTEVALUE](
	[SessionCookieAttributeValueID] [int] IDENTITY(1,1) NOT NULL,
	[SessionCookieID] [int] NOT NULL,
	[SessionCookieGUID] [nvarchar](500) NULL,
	[AttributeValueID] [int] NOT NULL,
	[AttributeValueGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[SessionCookieAttributeValueName] [nvarchar](250) NULL,
	[SessionCookieAttributeValueDescription] [nvarchar](max) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SESSIONCRON]    Script Date: 04/03/2015 19:59:09 ******/
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
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SETOPERATOR]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SETOPERATOR](
	[SetOperatorID] [int] IDENTITY(1,1) NOT NULL,
	[SetOperatorValue] [nvarchar](50) NOT NULL,
	[SetOperatorDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SEVERITYLEVEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SEVERITYLEVEL](
	[SeverityLevelID] [int] IDENTITY(1,1) NOT NULL,
	[SeverityLevelGUID] [nvarchar](500) NULL,
	[SeverityLevelName] [nvarchar](50) NOT NULL,
	[SeverityLevelDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SHELLCODE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SHELLCODE](
	[ShellCodeID] [int] IDENTITY(1,1) NOT NULL,
	[CodeID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ShellCodeName] [nvarchar](250) NULL,
	[ShellCodeDescription] [nvarchar](max) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[TrustLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIDTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIDTYPE](
	[SIDTypeID] [int] IDENTITY(1,1) NOT NULL,
	[SIDTypeName] [nvarchar](50) NOT NULL,
	[SIDTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIGNAL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIGNAL](
	[SignalID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIGNATURE]    Script Date: 04/03/2015 19:59:09 ******/
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
	[SignatureTypeID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIGNATURECPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIGNATURECPE](
	[CPESignatureID] [int] IDENTITY(1,1) NOT NULL,
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

/****** Object:  Table [dbo].[SIGNATUREEXPLOIT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIGNATUREEXPLOIT](
	[ExploitSignatureID] [int] IDENTITY(1,1) NOT NULL,
	[SignatureID] [int] NOT NULL,
	[ExploitID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[SignatureExploitName] [nvarchar](250) NULL,
	[SignatureExploitDescription] [nvarchar](max) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[TrustLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIGNATUREMALWAREINSTANCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIGNATUREMALWAREINSTANCE](
	[MalwareInstanceSignatureID] [int] IDENTITY(1,1) NOT NULL,
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

/****** Object:  Table [dbo].[SIGNATUREPORT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIGNATUREPORT](
	[SignatureID] [int] NOT NULL,
	[PortID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIGNATUREPROTOCOL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIGNATUREPROTOCOL](
	[SignatureID] [int] NOT NULL,
	[ProtocolID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIGNATUREREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIGNATUREREFERENCE](
	[SignatureReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[SignatureID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIGNATURETYPE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[SIGNATURETYPEREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIGNATURETYPEREFERENCE](
	[SitgnatureTypeReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[SignatureTypeID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SIMPLEDATATYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SIMPLEDATATYPE](
	[SimpleDataTypeID] [int] IDENTITY(1,1) NOT NULL,
	[DataTypeName] [nvarchar](50) NOT NULL,
	[DataTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SKILL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SKILL](
	[SkillID] [int] IDENTITY(1,1) NOT NULL,
	[SkillGUID] [nvarchar](500) NULL,
	[SkillName] [nvarchar](250) NULL,
	[SkillDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SKILLCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SKILLCATEGORY](
	[SkillCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[SkillCategoryGUID] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[SkillCategoryName] [nvarchar](100) NULL,
	[SkillCategoryDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SKILLCATEGORYTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SKILLCATEGORYTAG](
	[SkillCategoryTagID] [int] IDENTITY(1,1) NOT NULL,
	[SkillCategoryID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SKILLLEVEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SKILLLEVEL](
	[SkillLevelID] [int] IDENTITY(1,1) NOT NULL,
	[SkillLevelValue] [nvarchar](50) NULL,
	[SkillLevelDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SKILLTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SKILLTAG](
	[SkillTagID] [int] IDENTITY(1,1) NOT NULL,
	[SkillID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SMSMESSAGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SMSMESSAGE](
	[SMSMessageID] [int] IDENTITY(1,1) NOT NULL,
	[MessageID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SOCKETACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SOCKETACTIONNAME](
	[SocketActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[SocketActionNameName] [nvarchar](150) NOT NULL,
	[SocketActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SOCKETADDRESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SOCKETADDRESS](
	[SocketAddressID] [int] IDENTITY(1,1) NOT NULL,
	[AddressID] [int] NULL,
	[HostNameID] [int] NULL,
	[PortID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SOFTWARE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SOFTWARE](
	[SoftwareID] [int] IDENTITY(1,1) NOT NULL,
	[SoftwareGUID] [nvarchar](500) NULL,
	[ProductID] [int] NULL,
	[ProductGUID] [nvarchar](500) NULL,
	[ApplicationID] [int] NULL,
	[ApplicationGUID] [nvarchar](500) NULL,
	[CPEID] [int] NULL,
	[SWIDTAG] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SOFTWARECHARACTERISTIC]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SOFTWARECHARACTERISTIC](
	[SoftwareCharacteristicID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SOFTWAREFILELIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SOFTWAREFILELIST](
	[SoftwareFileListID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SOFTWARELICENSE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SOFTWARELICENSE](
	[SoftwareLicenseID] [int] IDENTITY(1,1) NOT NULL,
	[SoftwareID] [int] NOT NULL,
	[SoftwareGUID] [nvarchar](500) NULL,
	[LicenseID] [int] NOT NULL,
	[LicenseGUID] [nvarchar](500) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SOURCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SOURCE](
	[SourceID] [int] IDENTITY(1,1) NOT NULL,
	[SourceGUID] [nvarchar](500) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SOURCECLASS]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[SOURCETYPE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[SPLITFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[SPYINGSTRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SPYINGSTRATEGICOBJECTIVE](
	[SpyingStrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[SpyingStrategicObjectiveName] [nvarchar](50) NULL,
	[SpyingStrategicObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SPYINGTACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SPYINGTACTICALOBJECTIVE](
	[SpyingTacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[SpyingTacticalObjectiveName] [nvarchar](50) NULL,
	[SpyingTacticalObjectiveDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SSDTENTRY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SSDTENTRY](
	[SSDTEntryID] [int] IDENTITY(1,1) NOT NULL,
	[Service_Table_Base] [nvarchar](50) NULL,
	[Service_Counter_Table_Base] [nvarchar](50) NULL,
	[Number_Of_Services] [int] NULL,
	[Argument_Table_Base] [nvarchar](50) NULL,
	[hooked] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STAGE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STAGE](
	[StageID] [int] IDENTITY(1,1) NOT NULL,
	[StageGUID] [nvarchar](500) NULL,
	[StageName] [nvarchar](250) NULL,
	[StageDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STAGECATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STAGECATEGORY](
	[StageCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STAGEDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STAGEDESCRIPTION](
	[StageDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[StageID] [int] NOT NULL,
	[DescriptionID] [int] NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STANDARD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STANDARD](
	[StandardID] [int] IDENTITY(1,1) NOT NULL,
	[StandardGUID] [nvarchar](500) NULL,
	[StandardVocabularyID] [nvarchar](50) NULL,
	[StandardName] [nvarchar](500) NOT NULL,
	[StandardDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STANDARDCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STANDARDCATEGORY](
	[StandardCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STANDARDOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STANDARDOBJECTIVE](
	[StandardObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[StandardObjectiveVocabularyID] [nvarchar](50) NULL,
	[StandardID] [int] NULL,
	[ObjectiveID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STANDARDORGANISATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STANDARDORGANISATION](
	[StandardOrganisationID] [int] IDENTITY(1,1) NOT NULL,
	[StandardID] [int] NOT NULL,
	[RelationshipName] [nvarchar](50) NULL,
	[OrganisationID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STANDARDREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STANDARDREFERENCE](
	[StandardReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[StandardID] [int] NOT NULL,
	[StandardGUID] [nvarchar](500) NULL,
	[ReferenceID] [int] NOT NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STANDARDRELATIONSHIP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STANDARDRELATIONSHIP](
	[StandardRelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[StandardRefID] [int] NOT NULL,
	[RelationshipName] [nvarchar](50) NULL,
	[StandardSubjectID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[ReferenceURL] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STANDARDSECTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STANDARDSECTION](
	[StandardSectionID] [int] IDENTITY(1,1) NOT NULL,
	[StandardID] [int] NULL,
	[StandardGUID] [nvarchar](500) NULL,
	[SectionID] [int] NULL,
	[SectionGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STANDARDSECTIONMAPPING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STANDARDSECTIONMAPPING](
	[StandardSectionMappingID] [int] IDENTITY(1,1) NOT NULL,
	[StandardSectionRefID] [int] NULL,
	[StandardSectionSubjectID] [int] NULL,
	[ReferenceID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[MappingComment] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STANDARDSECURITYREQUIREMENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STANDARDSECURITYREQUIREMENT](
	[StandardSecurityRequirementID] [int] IDENTITY(1,1) NOT NULL,
	[StandardID] [int] NULL,
	[SecurityRequirementID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STANDARDTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STANDARDTAG](
	[StandardTagID] [int] IDENTITY(1,1) NOT NULL,
	[StandardID] [int] NULL,
	[TagID] [int] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STANDARDVOCABULARY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STANDARDVOCABULARY](
	[StandardVocabularyID] [int] IDENTITY(1,1) NOT NULL,
	[StandardID] [int] NOT NULL,
	[VocabularyID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STARTUPINFO]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STARTUPINFO](
	[StartupInfoID] [int] IDENTITY(1,1) NOT NULL,
	[lpDesktop] [nvarchar](500) NULL,
	[lpTitle] [nvarchar](500) NULL,
	[dwX] [int] NULL,
	[dwY] [int] NULL,
	[dwXSize] [int] NULL,
	[dwYSize] [int] NULL,
	[dwXCountChars] [int] NULL,
	[dwYCountChars] [int] NULL,
	[dwFillAttribute] [int] NULL,
	[dwFlags] [int] NULL,
	[wShowWindow] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STATUS]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[STRATEGICOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STRATEGICOBJECTIVE](
	[StrategicObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[StrategicObjectiveGUID] [nvarchar](500) NULL,
	[ObjectiveID] [int] NULL,
	[StrategicObjectiveName] [nvarchar](100) NULL,
	[StrategicObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STRATEGY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STRATEGY](
	[StrategyID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[STRUCTUREDAUTHENTICATIONMECHANISM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[STRUCTUREDAUTHENTICATIONMECHANISM](
	[StructuredAuthenticationMechanismID] [int] IDENTITY(1,1) NOT NULL,
	[StructuredAuthenticationMechanismGUID] [nvarchar](500) NULL,
	[StructuredAuthenticationMechanismDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SUBCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SUBCATEGORY](
	[SubCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryParentID] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SUBJECTPUBLICKEY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SUBJECTPUBLICKEY](
	[SubjectPublicKeyID] [int] IDENTITY(1,1) NOT NULL,
	[Public_Key_Algorithm] [nvarchar](250) NOT NULL,
	[EncryptionID] [int] NULL,
	[RSA_Public_Key] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SUBSTRINGFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[SUPPLYCHAIN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SUPPLYCHAIN](
	[SupplyChainID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SUPPLYCHAINASSURANCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SUPPLYCHAINASSURANCE](
	[SupplyChainAssuranceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SUPPLYCHAINCOMPLIANCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SUPPLYCHAINCOMPLIANCE](
	[SupplyChainComplianceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SUPPRESSIONTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SUPPRESSIONTYPE](
	[SuppressionTypeID] [int] IDENTITY(1,1) NOT NULL,
	[SuppressionTypeName] [nvarchar](50) NULL,
	[SuppressionTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SUSPECTEDMALICIOUSREASON]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SUSPECTEDMALICIOUSREASON](
	[SuspectedMaliciousReasonID] [int] IDENTITY(1,1) NOT NULL,
	[SuspectedMaliciousReasonGUID] [nvarchar](500) NULL,
	[SuspectedMaliciousReasonName] [nvarchar](50) NULL,
	[ReasonID] [int] NULL,
	[SuspectedMaliciousReasonDescription] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SWENTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SWENTAG](
	[SWENTAGID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SWIDTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SWIDTAG](
	[SWIDTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SWIDTAGCPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SWIDTAGCPE](
	[SWIDTagCPEID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SYNCHRONIZATIONACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SYNCHRONIZATIONACTIONNAME](
	[SynchronizationActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[SynchronizationActionNameName] [nvarchar](150) NOT NULL,
	[SynchronizationActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SYSTEM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SYSTEM](
	[SystemID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SYSTEMACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SYSTEMACTIONNAME](
	[SystemActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[SystemActionNameName] [nvarchar](150) NOT NULL,
	[SystemActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[EnumerationVersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SYSTEMINFO]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SYSTEMINFO](
	[SystemInfoID] [int] IDENTITY(1,1) NOT NULL,
	[OSID] [int] NOT NULL,
	[architecture] [nvarchar](150) NOT NULL,
	[primaryhostname] [nvarchar](250) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SYSTEMINFOFOROVALSYSTEMCHARACTERISTICS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SYSTEMINFOFOROVALSYSTEMCHARACTERISTICS](
	[OVALSystemCharacteristicsID] [int] NOT NULL,
	[SystemInfo] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SYSTEMTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SYSTEMTYPE](
	[SystemTypeID] [int] IDENTITY(1,1) NOT NULL,
	[SystemTypeGUID] [nvarchar](500) NULL,
	[SystemTypeName] [nvarchar](200) NOT NULL,
	[SystemTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SYSTEMTYPEFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SYSTEMTYPEFORASSET](
	[AssetSystemTypeID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[SystemTypeID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SYSTEMTYPEFORTHREATACTORTTP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SYSTEMTYPEFORTHREATACTORTTP](
	[SystemTypeID] [int] NOT NULL,
	[ThreatActorTTPID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TACTIC]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TACTIC](
	[TacticID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TACTICALOBJECTIVE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TACTICALOBJECTIVE](
	[TacticalObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[TacticalObjectiveGUID] [nvarchar](500) NULL,
	[ObjectiveID] [int] NULL,
	[TacticalObjectiveName] [nvarchar](100) NULL,
	[TacticalObjectiveDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TACTICCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TACTICCATEGORY](
	[TacticCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TAG](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[TagGUID] [nvarchar](500) NULL,
	[TagValue] [nvarchar](250) NULL,
	[casesensitive] [bit] NULL,
	[TagDescription] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[ImportanceID] [int] NULL,
	[TagType] [nvarchar](100) NULL,
	[CollectionMethodID] [int] NULL,
	[ToolID] [int] NULL,
	[ToolGUID] [nvarchar](500) NULL,
	[SourceID] [int] NULL,
	[SourceGUID] [nvarchar](500) NULL,
	[AssetID] [int] NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[AccountID] [int] NULL,
	[AccountGUID] [nvarchar](500) NULL,
	[UserID] [int] NULL,
	[UserGUID] [nvarchar](500) NULL,
	[VocabularyID] [int] NULL,
	[VocabularyGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TAGBLACKLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TAGBLACKLIST](
	[TagBlacklistID] [int] IDENTITY(1,1) NOT NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TAGCLASSIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TAGCLASSIFICATION](
	[TagClassificationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TAGFORASSET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TAGFORASSET](
	[TagAssetID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NULL,
	[AssetGUID] [nvarchar](500) NULL,
	[TagID] [int] NULL,
	[TagValue] [nvarchar](max) NULL,
	[TagAssetDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TAGRESTRICTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TAGRESTRICTION](
	[TagRestrictionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TAGTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TAGTAG](
	[TagTagID] [int] IDENTITY(1,1) NOT NULL,
	[TagTagGUID] [nvarchar](500) NULL,
	[TagParentID] [int] NULL,
	[TagParentGUID] [nvarchar](500) NULL,
	[TagSubjectID] [int] NULL,
	[TagSubjectGUID] [nvarchar](500) NULL,
	[TagRelationship] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[SourceID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ImportanceID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TARGET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TARGET](
	[TargetID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TARGETEDPLATFORMS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TARGETEDPLATFORMS](
	[TargetedPlatformsID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TARGETEDPLATFORMSPECIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TARGETEDPLATFORMSPECIFICATION](
	[TargetedPlatformsSpecification] [int] IDENTITY(1,1) NOT NULL,
	[TargetedPlatformsID] [int] NOT NULL,
	[PlatformSpecificationID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TARGETS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TARGETS](
	[TargetsID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TASK]    Script Date: 04/03/2015 19:59:09 ******/
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
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TASKACTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TASKACTION](
	[TaskActionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TASKACTIONLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TASKACTIONLIST](
	[TaskActionListID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TASKACTIONTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TASKACTIONTYPE](
	[TaskActionTypeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TASKATTACHMENT]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[TASKDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TASKDESCRIPTION](
	[TaskDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [int] NOT NULL,
	[TaskGUID] [nvarchar](500) NULL,
	[DescriptionID] [int] NOT NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TASKFLAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TASKFLAG](
	[TaskFlagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TASKFORPROJECT]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[TASKPERSON]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[TASKPRIORITY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TASKPRIORITY](
	[TaskPriorityID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TASKPRIORITYENUM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TASKPRIORITYENUM](
	[TaskPriorityEnumID] [int] IDENTITY(1,1) NOT NULL,
	[TaskPriority] [nvarchar](50) NULL,
	[TaskPriorityDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TASKSTATUS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TASKSTATUS](
	[TaskStatusID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TASKSTATUSENUM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TASKSTATUSENUM](
	[TaskStatusEnumID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](50) NULL,
	[TaskStatusDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TASKTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TASKTAG](
	[TaskTagID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [int] NULL,
	[TagID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TASKTRIGGER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TASKTRIGGER](
	[TaskTriggerID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TASKTRIGGERFREQUENCY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TASKTRIGGERFREQUENCY](
	[TaskTriggerFrequencyID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TAXONOMY]    Script Date: 04/03/2015 19:59:09 ******/
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
	[CreatedDate] [datetimeoffset](7) NULL,
	[DateModified] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TAXONOMYNODE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TAXONOMYNODE](
	[TaxonomyNodeID] [int] IDENTITY(1,1) NOT NULL,
	[TaxonomyID] [int] NULL,
	[TaxonomyNodeName] [nvarchar](250) NULL,
	[TaxonomyMappedNodeID] [nvarchar](50) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[TaxonomyNodeDescription] [nvarchar](max) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TAXONOMYREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
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
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TCPSTATE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TCPSTATE](
	[TCPStateID] [int] IDENTITY(1,1) NOT NULL,
	[TCPStateValue] [nvarchar](50) NULL,
	[TCPStateDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TECHNICALCONTEXT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TECHNICALCONTEXT](
	[TechnicalContextID] [int] IDENTITY(1,1) NOT NULL,
	[AttackPatternID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TECHNIQUE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TECHNIQUE](
	[TechniqueID] [int] IDENTITY(1,1) NOT NULL,
	[TechniqueGUID] [nvarchar](500) NULL,
	[TechniqueName] [nvarchar](250) NULL,
	[TechniqueDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[ValidityID] [int] NULL,
	[CreationObjectID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TECHNIQUECATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TECHNIQUECATEGORY](
	[TechniqueCategoryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TECHNIQUEDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TECHNIQUEDESCRIPTION](
	[TechniqueDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TECHNIQUEREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TECHNIQUEREFERENCE](
	[TechniqueReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[TechniqueID] [int] NOT NULL,
	[TechniqueGUID] [nvarchar](500) NULL,
	[ReferenceID] [int] NOT NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[TechniqueReferenceDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[VocabularyID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ConfidentialityLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TECHNIQUEREFERENCETAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TECHNIQUEREFERENCETAG](
	[TechniqueReferenceTagID] [int] IDENTITY(1,1) NOT NULL,
	[TechniqueReferenceID] [int] NULL,
	[TechniqueReferenceGUID] [nvarchar](500) NULL,
	[TagID] [int] NULL,
	[TagGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[ImportanceID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[ConfidentialityLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TECHNIQUERESTRICTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TECHNIQUERESTRICTION](
	[TechniqueRestrictionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TECHNIQUESTEP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TECHNIQUESTEP](
	[TechniqueStepID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TECHNIQUETAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TECHNIQUETAG](
	[TechniqueTagID] [int] IDENTITY(1,1) NOT NULL,
	[TechniqueID] [int] NOT NULL,
	[TagID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TECHNOLOGY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TECHNOLOGY](
	[TechnologyID] [int] IDENTITY(1,1) NOT NULL,
	[TechnologyGUID] [nvarchar](500) NULL,
	[TechnologyName] [nvarchar](250) NULL,
	[TechnologyDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TECHNOLOGYDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TECHNOLOGYDESCRIPTION](
	[TechnologyDescriptionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TECHNOLOGYTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TECHNOLOGYTAG](
	[TechnologyTagID] [int] IDENTITY(1,1) NOT NULL,
	[TechnologyID] [int] NOT NULL,
	[TagID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TECHNOLOGYURI]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TECHNOLOGYURI](
	[TechnologyURIID] [int] IDENTITY(1,1) NOT NULL,
	[TechnologyID] [int] NOT NULL,
	[URIObjectID] [int] NOT NULL,
	[ConfidenceLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[RepositoryID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TELEPHONE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TELEPHONE](
	[TelephoneID] [int] IDENTITY(1,1) NOT NULL,
	[TelephoneGUID] [nvarchar](500) NULL,
	[TelephoneNumber] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[LastCheckedDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TELEPHONECALL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TELEPHONECALL](
	[TelephoneCallID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TELEPHONEFORORGANISATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TELEPHONEFORORGANISATION](
	[OrganisationTelephoneID] [int] IDENTITY(1,1) NOT NULL,
	[TelephoneID] [int] NOT NULL,
	[OrganisationID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TELEPHONEFORPERSON]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TELEPHONEFORPERSON](
	[PersonTelephoneID] [int] IDENTITY(1,1) NOT NULL,
	[TelephoneID] [int] NOT NULL,
	[PersonID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TELEPHONETAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TELEPHONETAG](
	[TelephoneTagID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TEST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TEST](
	[TestID] [int] IDENTITY(1,1) NOT NULL,
	[TestGUID] [nvarchar](500) NULL,
	[TestName] [nvarchar](100) NULL,
	[TestDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TESTMECHANISMEFFICACY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TESTMECHANISMEFFICACY](
	[TestMechanismEfficacyID] [int] IDENTITY(1,1) NOT NULL,
	[Efficacy] [nvarchar](50) NOT NULL,
	[EfficacyDescription] [nvarchar](max) NULL,
	[ConfidenceLevel] [nvarchar](50) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TESTMECHANISMID]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TESTMECHANISMID](
	[TestMechanismID] [int] NOT NULL,
	[CyberObservableTestMechanismID] [int] NOT NULL,
	[TestMechanismIDREF] [nvarchar](100) NOT NULL,
	[Information_Source] [nvarchar](250) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THEORETICALNOTE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THEORETICALNOTE](
	[TheoreticalNoteID] [int] IDENTITY(1,1) NOT NULL,
	[TheoreticalNoteText] [nvarchar](max) NULL,
	[TheoreticalNoteTextClean] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[THREADRUNNINGSTATUS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THREADRUNNINGSTATUS](
	[ThreadRunningStatusID] [int] IDENTITY(1,1) NOT NULL,
	[Running_Status] [nvarchar](50) NOT NULL,
	[ThreadRunningStatusDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TICKET]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TICKET](
	[TicketID] [int] IDENTITY(1,1) NOT NULL,
	[TicketGUID] [nvarchar](500) NULL,
	[StatusID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TICKETCHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TICKETCHANGERECORD](
	[TicketChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TICKETCHANGEREQUEST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TICKETCHANGEREQUEST](
	[TicketChangeRequestID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TICKETNOTIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TICKETNOTIFICATION](
	[TicketNotificationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TICKETRACIMATRIX]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TICKETRACIMATRIX](
	[TicketRACIMatrixID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TIMEDIFFERENCEFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[TIMELINE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TIMELINE](
	[TimelineID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TIMESHEET]    Script Date: 04/03/2015 19:59:09 ******/
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
	[ProjectTaskID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TIMESHEETPERSON]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[TIMEUNIT]    Script Date: 04/03/2015 19:59:09 ******/
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
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TIP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TIP](
	[TipID] [int] IDENTITY(1,1) NOT NULL,
	[TipGUID] [nvarchar](500) NULL,
	[TipName] [nvarchar](250) NULL,
	[TipDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TIPCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TIPCATEGORY](
	[TipCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[TipCategoryGUID] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[TipCategoryName] [nvarchar](50) NULL,
	[TipCategoryDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TIPREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TIPREFERENCE](
	[TipReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[TipID] [int] NOT NULL,
	[TipGUID] [nvarchar](500) NULL,
	[ReferenceID] [int] NOT NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[TipReferenceDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TITLE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TITLE](
	[TitleID] [int] IDENTITY(1,1) NOT NULL,
	[TitleText] [nvarchar](500) NULL,
	[LocaleID] [int] NULL,
	[VersionID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOKEN]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOKEN](
	[TokenID] [int] IDENTITY(1,1) NOT NULL,
	[TokenParentID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[TokenName] [nvarchar](250) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOL]    Script Date: 04/03/2015 19:59:09 ******/
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
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[ReliabilityID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLACCESSRECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLACCESSRECORD](
	[ToolAccessRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLCHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLCHANGERECORD](
	[ToolChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLCODE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLCODE](
	[ToolCodeID] [int] IDENTITY(1,1) NOT NULL,
	[ToolCodeGUID] [nvarchar](500) NULL,
	[ToolID] [int] NULL,
	[ToolGUID] [nvarchar](500) NULL,
	[CodeID] [int] NULL,
	[CodeGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[CreationObjectGUID] [nvarchar](500) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionMethodGUID] [nvarchar](500) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceLevelGUID] [nvarchar](500) NULL,
	[ConfidenceReasonID] [int] NULL,
	[ConfidenceReasonGUID] [nvarchar](500) NULL,
	[SourceID] [int] NULL,
	[SourceGUID] [nvarchar](500) NULL,
	[RepositoryID] [int] NULL,
	[RepositoryGUID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLFUNCTION](
	[ToolFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[ToolFunctionGUID] [nvarchar](500) NULL,
	[ToolID] [int] NULL,
	[ToolGUID] [nvarchar](500) NULL,
	[FunctionID] [int] NULL,
	[FunctionGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[CollectionMethodID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLINFORMATION]    Script Date: 04/03/2015 19:59:09 ******/
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
	[Tool_Hashes] [nvarchar](max) NULL,
	[Tool_Configuration] [nvarchar](max) NULL,
	[Execution_Environment] [nvarchar](max) NULL,
	[Errors] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLINFORMATIONDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLINFORMATIONDESCRIPTION](
	[ToolInformationDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[ToolInformationID] [int] NOT NULL,
	[DescriptionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLINFORMATIONFORTOOL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLINFORMATIONFORTOOL](
	[ToolInformationForToolID] [int] IDENTITY(1,1) NOT NULL,
	[ToolID] [int] NOT NULL,
	[ToolInformationID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLINFORMATIONMETADATA]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLINFORMATIONMETADATA](
	[ToolInformationID] [int] NOT NULL,
	[MetadataID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLINFORMATIONREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLINFORMATIONREFERENCE](
	[ToolInformationReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[ToolInformationID] [int] NOT NULL,
	[ToolInformationGUID] [nvarchar](500) NULL,
	[ReferenceID] [int] NOT NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[ToolReferenceTypeID] [int] NULL,
	[ToolReferenceTypeGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLLICENSE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[TOOLREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLREFERENCE](
	[ToolReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[ToolID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[ToolReferenceTypeID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ConfidentialityLevelID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLREFERENCETYPE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[TOOLREPOSITORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLREPOSITORY](
	[ToolRepositoryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLTAG](
	[ToolTagID] [int] IDENTITY(1,1) NOT NULL,
	[ToolTagGUID] [nvarchar](500) NULL,
	[ToolID] [int] NULL,
	[ToolGUID] [nvarchar](500) NULL,
	[TagID] [int] NULL,
	[TagGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValdFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLTECHNOLOGY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLTECHNOLOGY](
	[ToolTechnologyID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLTYPE](
	[ToolTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ToolTypeGUID] [nvarchar](500) NULL,
	[ToolTypeName] [nvarchar](150) NOT NULL,
	[ToolTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[isEncrypted] [bit] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLTYPEFORTOOLINFORMATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLTYPEFORTOOLINFORMATION](
	[ToolInformationID] [int] NOT NULL,
	[ToolTypeID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TOOLUSERAGENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TOOLUSERAGENT](
	[ToolUserAgentID] [int] IDENTITY(1,1) NOT NULL,
	[ToolID] [int] NULL,
	[UserAgentID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TRAINING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TRAINING](
	[TrainingID] [int] IDENTITY(1,1) NOT NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TRAININGFORPERSON]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TRAININGFORPERSON](
	[TrainingPersonID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[TrainingID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TRANSACTION]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[TRANSFORMATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TRANSFORMATION](
	[TransformationID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TREND]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[TRIGGERFREQUENCYENUM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TRIGGERFREQUENCYENUM](
	[TriggerFrequencyEnumID] [int] IDENTITY(1,1) NOT NULL,
	[TriggerFrequency] [nvarchar](50) NULL,
	[TriggerFrequencyDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TRIGGERLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TRIGGERLIST](
	[TriggerListID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TRIGGERTYPEENUM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TRIGGERTYPEENUM](
	[TriggerTypeEnumID] [int] IDENTITY(1,1) NOT NULL,
	[TriggerType] [nvarchar](50) NULL,
	[TriggerTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TRUSTLEVEL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TRUSTLEVEL](
	[TrustLevelID] [int] IDENTITY(1,1) NOT NULL,
	[TrustLevelGUID] [nvarchar](500) NULL,
	[TrustLevelName] [nvarchar](50) NOT NULL,
	[TrustLevelDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TRUSTREASON]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TRUSTREASON](
	[TrustReasonID] [int] IDENTITY(1,1) NOT NULL,
	[TrustReasonGUID] [nvarchar](500) NULL,
	[ReasonID] [int] NULL,
	[TrustReasonName] [nvarchar](50) NULL,
	[TrustReasonDescription] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TYPE](
	[TypeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UNIDIRECTIONALFLOWRECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UNIDIRECTIONALFLOWRECORD](
	[UnidirectionalFlowRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UNIQUEFUNCTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UNIQUEFUNCTION](
	[UniqueFunctionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UNIT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UNIT](
	[UnitID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UNIXFILE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UNIXFILE](
	[UnixFileID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UNIXNETWORKROUTEENTRY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UNIXNETWORKROUTEENTRY](
	[UnixNetworkRouteEntryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UNIXPIPEOBJECT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UNIXPIPEOBJECT](
	[UnixPipeObjectID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UNIXPROCESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UNIXPROCESS](
	[UnixProcessID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UNIXUSERACCOUNT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UNIXUSERACCOUNT](
	[UnixUserAccountID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UNIXVOLUME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UNIXVOLUME](
	[UnixVolumeID] [int] IDENTITY(1,1) NOT NULL,
	[VolumeObjectID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[URGENCY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[URGENCY](
	[UrgencyID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[URIOBJECT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[URIOBJECT](
	[URIObjectID] [int] IDENTITY(1,1) NOT NULL,
	[URIValue] [nvarchar](500) NULL,
	[URITypeID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[URITYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[URITYPE](
	[URITypeID] [int] IDENTITY(1,1) NOT NULL,
	[URITypeName] [nvarchar](50) NULL,
	[URITypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[URL]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[URL](
	[URLID] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[URLHISTORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[URLHISTORY](
	[URLHistoryID] [int] IDENTITY(1,1) NOT NULL,
	[URLHistoryGUID] [nvarchar](500) NULL,
	[BrowserToolInformationID] [int] NULL,
	[ToolInformationGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[URLHISTORYENTRIES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[URLHISTORYENTRIES](
	[URLHistoryEntriesID] [int] IDENTITY(1,1) NOT NULL,
	[URLHistoryID] [int] NULL,
	[URLHistoryGUID] [nvarchar](500) NULL,
	[URLHistoryEntryID] [int] NULL,
	[URLHistoryEntryGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[URLHISTORYENTRY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[URLHISTORYENTRY](
	[URLHistoryEntryID] [int] IDENTITY(1,1) NOT NULL,
	[URLHistoryEntryGUID] [nvarchar](500) NULL,
	[URIObjectID] [int] NULL,
	[HostnameID] [int] NULL,
	[Referrer_URL] [int] NULL,
	[Page_Title] [nvarchar](500) NULL,
	[User_Profile_Name] [nvarchar](250) NULL,
	[Visit_Count] [int] NULL,
	[Manually_Entered_Count] [int] NULL,
	[Modification_DateTime] [datetimeoffset](7) NULL,
	[Expiration_DateTime] [datetimeoffset](7) NULL,
	[First_Visit_DateTime] [datetimeoffset](7) NULL,
	[Last_Visit_DateTime] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[CollectionMethoID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USAGETYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USAGETYPE](
	[UsageTypeID] [int] NOT NULL,
	[TypeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USECASE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USECASE](
	[UseCaseID] [int] IDENTITY(1,1) NOT NULL,
	[UseCaseGUID] [nvarchar](500) NULL,
	[UseCaseDescription] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USECASECATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USECASECATEGORY](
	[UseCaseCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[UseCaseCategoryGUID] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[UseCasecategoryName] [nvarchar](50) NULL,
	[UseCaseCategoryDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USECASEFORBUSINESSRISK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USECASEFORBUSINESSRISK](
	[BusinessRiskUseCaseID] [int] IDENTITY(1,1) NOT NULL,
	[BusinessRiskUseCaseGUID] [nvarchar](500) NULL,
	[UseCaseID] [int] NOT NULL,
	[UseCaseGUID] [nvarchar](500) NULL,
	[BusinessRiskID] [int] NOT NULL,
	[BusinessRiskGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USECASEFORREGULATORYRISK]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USECASEFORREGULATORYRISK](
	[RegulatoryRiskUseCaseID] [int] IDENTITY(1,1) NOT NULL,
	[RegulatoryRiskUseCaseGUID] [nvarchar](500) NULL,
	[UseCaseID] [int] NOT NULL,
	[UseCaseGUID] [nvarchar](500) NULL,
	[RegulatoryRiskID] [int] NOT NULL,
	[RegulatoryRiskGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USER]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USER](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserGUID] [nvarchar](500) NULL,
	[UserName] [nvarchar](250) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromdate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USERACCOUNT]    Script Date: 04/03/2015 19:59:09 ******/
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
	[UserAccountTypeName] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USERACCOUNTTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USERACCOUNTTYPE](
	[UserAccountTypeID] [int] IDENTITY(1,1) NOT NULL,
	[UserAccountTypeGUID] [nvarchar](500) NULL,
	[UserAccountTypeName] [nvarchar](100) NULL,
	[UserAccountTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USERACTIONNAME]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USERACTIONNAME](
	[UserActionNameID] [int] IDENTITY(1,1) NOT NULL,
	[UserActionNameName] [nvarchar](150) NOT NULL,
	[UserActionNameDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[EnumerationVersionID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USERAGENT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USERAGENT](
	[UserAgentID] [int] IDENTITY(1,1) NOT NULL,
	[UserAgentGUID] [nvarchar](500) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USERAGENTBLACKLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USERAGENTBLACKLIST](
	[UserAgentBlacklistID] [int] IDENTITY(1,1) NOT NULL,
	[UserAgentBlacklistGUID] [nvarchar](500) NULL,
	[UserAgentBlacklistName] [nvarchar](50) NULL,
	[UserAgentBlacklistDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USERAGENTCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USERAGENTCATEGORY](
	[UserAgentCategoryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[USERSESSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USERSESSION](
	[UserSessionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VALIDITY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VALIDITY](
	[ValidityID] [int] IDENTITY(1,1) NOT NULL,
	[Not_Before] [datetimeoffset](7) NULL,
	[Not_After] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VALUE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VALUE](
	[ValueID] [int] IDENTITY(1,1) NOT NULL,
	[ValueValue] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VALUEBLACKLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VALUEBLACKLIST](
	[ValueBlacklistID] [int] IDENTITY(1,1) NOT NULL,
	[ValueID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VALUEGROUP]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VALUEGROUP](
	[ValueGroupID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VALUEMAPPING]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VALUEMAPPING](
	[ValueMappingID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VALUEWHITELIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VALUEWHITELIST](
	[ValueWhitelistID] [int] IDENTITY(1,1) NOT NULL,
	[ValueID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VARIABLE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VARIABLE](
	[VariableID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VERSION]    Script Date: 04/03/2015 19:59:09 ******/
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
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VIEWPORT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VIEWPORT](
	[ViewPortID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VOCABULARY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VOCABULARY](
	[VocabularyID] [int] IDENTITY(1,1) NOT NULL,
	[VocabularyGUID] [nvarchar](500) NULL,
	[VocabularyName] [nvarchar](250) NOT NULL,
	[VocabularyVersion] [nvarchar](10) NULL,
	[VocabularyReference] [nvarchar](250) NULL,
	[DateModified] [datetimeoffset](7) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VOCABULARYCATEGORIES]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VOCABULARYCATEGORIES](
	[VocabularyCategoriesID] [int] IDENTITY(1,1) NOT NULL,
	[VocabularyID] [int] NULL,
	[VocabularyCategoryID] [int] NULL,
	[CategoryID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VOCABULARYCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VOCABULARYCATEGORY](
	[VocabularyCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[VocabularyCategoryGUID] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[VocabularyCategoryName] [nvarchar](250) NULL,
	[VocabularyCategoryDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VOCABULARYCHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VOCABULARYCHANGERECORD](
	[VocabularyChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VOCABULARYDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VOCABULARYDESCRIPTION](
	[VocabularyDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[VocabularyDescribedID] [int] NOT NULL,
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

/****** Object:  Table [dbo].[VOCABULARYREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[VOCABULARYTAG]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VOCABULARYTAG](
	[VocabularyTagID] [int] IDENTITY(1,1) NOT NULL,
	[VocabularyTaggedID] [int] NOT NULL,
	[TagID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[VocabularyID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VOCABULARYVERSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VOCABULARYVERSION](
	[VocabularyVersionID] [int] IDENTITY(1,1) NOT NULL,
	[VocabularyVersionGUID] [nvarchar](500) NULL,
	[VocabularyID] [int] NOT NULL,
	[VocabularyGUID] [nvarchar](500) NULL,
	[VersionID] [int] NOT NULL,
	[ChangeLog] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VOLUMEOBJECT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VOLUMEOBJECT](
	[VolumeObjectID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VULNERABLECONFIGURATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VULNERABLECONFIGURATION](
	[VulnerableConfigurationID] [int] IDENTITY(1,1) NOT NULL,
	[VulnerabilityID] [int] NULL,
	[ConfigurationOrder] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[VULNERABLECONFIGURATIONCPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VULNERABLECONFIGURATIONCPE](
	[VulnerableConfigurationCPEID] [int] IDENTITY(1,1) NOT NULL,
	[VulnerableConfigurationID] [int] NULL,
	[LogicalTestLevel] [int] NULL,
	[LogicalTestLevelOrder] [int] NULL,
	[CPELogicalTestID] [int] NULL,
	[CPEID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WAITABLETIMERTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WAITABLETIMERTYPE](
	[WaitableTimerTypeID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WAITABLETIMERTYPEENUM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WAITABLETIMERTYPEENUM](
	[WaitaibleTimerTypeEnumID] [int] IDENTITY(1,1) NOT NULL,
	[WaitaibleTimerTypeName] [nvarchar](100) NOT NULL,
	[WaitableTimerTypeDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WAIVER]    Script Date: 04/03/2015 19:59:09 ******/
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
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WAIVERREASON]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WAIVERREASON](
	[WaiverReasonID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WARNING]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[WARNINGCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
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

/****** Object:  Table [dbo].[WASC]    Script Date: 04/03/2015 19:59:09 ******/
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
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WASCCWE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WASCCWE](
	[WASCCWEID] [int] IDENTITY(1,1) NOT NULL,
	[WASCID] [int] NOT NULL,
	[WASCRefID] [nvarchar](20) NULL,
	[CWEID] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WASCFORCAPEC]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WASCFORCAPEC](
	[WASCForCAPECID] [int] IDENTITY(1,1) NOT NULL,
	[WASCID] [int] NOT NULL,
	[WASCRefID] [nvarchar](20) NULL,
	[AttackPatternID] [int] NULL,
	[capec_id] [nvarchar](20) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[RepositoryID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WASCREFERENCE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WASCREFERENCE](
	[WASCReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[WASCID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[ReferenceGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WASCTHREATTYPE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WASCTHREATTYPE](
	[WASCThreatTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ThreatTypeID] [int] NOT NULL,
	[WASCID] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WEAKNESS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WEAKNESS](
	[WeaknessID] [int] IDENTITY(1,1) NOT NULL,
	[WeaknessGUID] [nvarchar](500) NULL,
	[CWEID] [nvarchar](50) NULL,
	[WeaknessName] [nvarchar](250) NULL,
	[WeaknessDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WEAKNESSCWE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WEAKNESSCWE](
	[WeaknessCWEID] [int] IDENTITY(1,1) NOT NULL,
	[WeaknessCWEGUID] [nvarchar](500) NULL,
	[WeaknessID] [int] NOT NULL,
	[WeaknessGUID] [nvarchar](500) NULL,
	[CWEID] [nvarchar](50) NULL,
	[WeaknessCWEDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[ConfidenceReasonID] [int] NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WHOISCHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WHOISCHANGERECORD](
	[WhoisChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WHOISOBJECT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WHOISOBJECT](
	[WhoisObjectID] [int] IDENTITY(1,1) NOT NULL,
	[WhoisObjectGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WORD](
	[WordID] [int] IDENTITY(1,1) NOT NULL,
	[WordGUID] [nvarchar](500) NULL,
	[WordValue] [nvarchar](50) NULL,
	[LocaleID] [int] NULL,
	[WordDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WORDBLACKLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WORDBLACKLIST](
	[WordBlacklistID] [int] IDENTITY(1,1) NOT NULL,
	[WordListID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WORDFILE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WORDFILE](
	[WordFileID] [int] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WORDLIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WORDLIST](
	[WordListID] [int] IDENTITY(1,1) NOT NULL,
	[WordListGUID] [nvarchar](500) NULL,
	[VersionID] [int] NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WORDLISTCATEGORY]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WORDLISTCATEGORY](
	[WordListCategoryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WORDLISTWORDS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WORDLISTWORDS](
	[WordListWordID] [int] IDENTITY(1,1) NOT NULL,
	[WordListID] [int] NOT NULL,
	[WordID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WORDWHITELIST]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WORDWHITELIST](
	[WordWhitelistID] [int] IDENTITY(1,1) NOT NULL,
	[WordListID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WORKINGHOURS]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WORKINGHOURS](
	[WorkingHoursID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[X509CERTIFICATE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[X509CERTIFICATE](
	[X509CertificateID] [int] IDENTITY(1,1) NOT NULL,
	[X509CertificateGUID] [nvarchar](500) NULL,
	[CertificateID] [int] NULL,
	[Version] [int] NULL,
	[Serial_Number] [nvarchar](500) NULL,
	[Signature_Algorithm] [nvarchar](250) NULL,
	[EncryptionID] [int] NULL,
	[Issuer] [nvarchar](250) NULL,
	[IssuerOrganisationID] [int] NULL,
	[ValidityID] [int] NULL,
	[Subject] [nvarchar](250) NULL,
	[SubjectOrganisationID] [int] NULL,
	[SubjectPersonID] [int] NULL,
	[Subject_Public_Key] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[X509CERTIFICATEACCESSRECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[X509CERTIFICATEACCESSRECORD](
	[X509CertificateAccessRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[X509CERTIFICATECHANGERECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[X509CERTIFICATECHANGERECORD](
	[X509CertificateChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[X509CERTIFICATENONSTANDARDEXTENSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[X509CERTIFICATENONSTANDARDEXTENSION](
	[X509CertificateNonStandardExtensionID] [int] IDENTITY(1,1) NOT NULL,
	[X509CertificateID] [int] NOT NULL,
	[X509NonStandardExtensionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[X509CERTIFICATEOBJECT]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[X509CERTIFICATEOBJECT](
	[X509CertificateObjectID] [int] IDENTITY(1,1) NOT NULL,
	[X509CertificateID] [int] NOT NULL,
	[X509CertificateGUID] [nvarchar](500) NULL,
	[X509SignatureID] [int] NOT NULL,
	[X509SignatureGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[X509CERTIFICATESTANDARDEXTENSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[X509CERTIFICATESTANDARDEXTENSION](
	[X509CertificateStandardExtensionID] [int] IDENTITY(1,1) NOT NULL,
	[X509CertificateID] [int] NOT NULL,
	[X509CertificateGUID] [nvarchar](500) NULL,
	[X509V3ExtensionID] [int] NOT NULL,
	[X509V3ExtensionGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[X509NONSTANDARDEXTENSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[X509NONSTANDARDEXTENSION](
	[X509NonStandardExtensionID] [int] IDENTITY(1,1) NOT NULL,
	[X509NonStandardExtensionGUID] [nvarchar](500) NULL,
	[Netscape_Comment] [nvarchar](max) NULL,
	[Netscape_Certificate_Type] [nvarchar](max) NULL,
	[Old_Authority_Key_Identifier] [nvarchar](max) NULL,
	[Old_Primary_Key_Attributes] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[X509SIGNATURE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[X509SIGNATURE](
	[X509SignatureID] [int] IDENTITY(1,1) NOT NULL,
	[X509SignatureGUID] [nvarchar](500) NULL,
	[SignatureID] [int] NULL,
	[Signature_Algorithm] [nvarchar](500) NULL,
	[EncryptionID] [int] NULL,
	[Signature] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[X509V3EXTENSION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[X509V3EXTENSION](
	[X509V3ExtensionID] [int] IDENTITY(1,1) NOT NULL,
	[X509V3ExtensionGUID] [nvarchar](500) NULL,
	[Basic_Constraints] [nvarchar](max) NULL,
	[Name_Constraints] [nvarchar](max) NULL,
	[Policy_Constraints] [nvarchar](max) NULL,
	[Key_Usage] [nvarchar](max) NULL,
	[Extended_Key_Usage] [nvarchar](max) NULL,
	[Subject_Key_Identifier] [nvarchar](max) NULL,
	[Authority_Key_Identifier] [nvarchar](max) NULL,
	[Subject_Alternative_Name] [nvarchar](max) NULL,
	[Issuer_Alternative_Name] [nvarchar](max) NULL,
	[Subject_Directory_Attributes] [nvarchar](max) NULL,
	[CRL_Distribution_Points] [nvarchar](max) NULL,
	[Inhibit_Any_Policy] [int] NULL,
	[Private_Key_Usage_Period] [int] NULL,
	[Certificate_Policies] [nvarchar](max) NULL,
	[Policy_Mappings] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[X509V3EXTENSIONACCESSRECORD]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[X509V3EXTENSIONACCESSRECORD](
	[X509V3ExtensionAccessRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[X509V3EXTENSIONPOLICYTERM]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[X509V3EXTENSIONPOLICYTERM](
	[X509V3ExtensionPolicyTermID] [int] IDENTITY(1,1) NOT NULL,
	[X509V3ExtensionID] [int] NOT NULL,
	[PolicyTermID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ZONE]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ZONE](
	[ZoneID] [int] IDENTITY(1,1) NOT NULL,
	[ZoneGUID] [nvarchar](500) NULL,
	[ZoneName] [nvarchar](250) NULL,
	[ZoneDescription] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ValidityID] [int] NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ZONECLASSIFICATION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ZONECLASSIFICATION](
	[ZoneClassificationID] [int] IDENTITY(1,1) NOT NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ZONEDESCRIPTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ZONEDESCRIPTION](
	[ZoneDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ZONERESTRICTION]    Script Date: 04/03/2015 19:59:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ZONERESTRICTION](
	[ZoneRestrictionID] [int] IDENTITY(1,1) NOT NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO


