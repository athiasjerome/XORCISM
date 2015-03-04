/****** 
Copyright (C) 2014-2015 Jerome Athias
Windows objects related tables for XORCISM database (mainly from MITRE CybOX)
This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
******/


USE [XWINDOWS]
GO

/****** Object:  Table [dbo].[WINDOWSCOMPUTERACCOUNT]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSCOMPUTERACCOUNT](
	[WindowsComputerAccountID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSCRITICALSECTION]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSCRITICALSECTION](
	[WindowsCriticalSectionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSDRIVER]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSDRIVER](
	[WindowsDriverID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSDRIVETYPE]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSDRIVETYPE](
	[WindowsDriveTypeID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsDriveTypeName] [nvarchar](50) NOT NULL,
	[WindowsDriveTypeDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSEVENT]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSEVENT](
	[WindowsEventID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSEVENTLOG]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSEVENTLOG](
	[WindowsEventLogID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSEXECUTABLEFILE]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSEXECUTABLEFILE](
	[WindowsExecutableFileID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSFILE]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSFILE](
	[WindowsFileID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsFileGUID] [nvarchar](500) NULL,
	[FileID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[suspected_malicious] [bit] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSGROUP]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSGROUP](
	[WindowsGroupID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsGroupName] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSGROUPFORWINDOWSUSERACCOUNT]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSGROUPFORWINDOWSUSERACCOUNT](
	[WindowsGroupWindowsUserAccountID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsGroupID] [int] NOT NULL,
	[WindowsUserAccountID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSHANDLE]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSHANDLE](
	[WindowsHandleID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsHandleObjectID] [int] NULL,
	[WindowsHandleName] [nvarchar](100) NULL,
	[HandleTypeID] [int] NULL,
	[Object_Address] [int] NULL,
	[Access_Mask] [int] NULL,
	[Pointer_Count] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSHANDLELIST]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSHANDLELIST](
	[WindowsHandleListID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSHANDLELISTHANDLES]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSHANDLELISTHANDLES](
	[WindowsHandleListHandlesID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsHandleListID] [int] NOT NULL,
	[WindowsHandleID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSHANDLETYPE]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSHANDLETYPE](
	[WindowsHandleTypeID] [int] IDENTITY(1,1) NOT NULL,
	[HandleTypeID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSKERNELHOOK]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSKERNELHOOK](
	[WindowsKernelHookID] [int] IDENTITY(1,1) NOT NULL,
	[Digital_Signature_Hooking] [nvarchar](max) NULL,
	[DigitalSignatureInfoHookingID] [int] NULL,
	[Digital_Signature_Hooked] [nvarchar](max) NULL,
	[DigitalSignatureInfoHookedID] [int] NULL,
	[Hooking_Address] [int] NULL,
	[Hook_Description] [nvarchar](max) NULL,
	[Hooked_Function] [nvarchar](100) NULL,
	[FunctionHookedID] [int] NULL,
	[Hooked_Module] [nvarchar](100) NULL,
	[ModuleHookedID] [int] NULL,
	[Hooking_Module] [nvarchar](100) NULL,
	[KernelHookID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[DetectionMethodID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSKERNELOBJECT]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSKERNELOBJECT](
	[WindowsKernelObjectID] [int] IDENTITY(1,1) NOT NULL,
	[IDTEntryListID] [int] NULL,
	[SSDTEntryListID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[CollectionMethodID] [int] NULL,
	[CollectionToolID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSMAILSLOT]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSMAILSLOT](
	[WindowsMailslotID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSMEMORYPAGEREGION]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSMEMORYPAGEREGION](
	[WindowsMemoryPageRegionID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSMUTEX]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSMUTEX](
	[WindowsMutexID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsHandleID] [int] NULL,
	[MutexID] [int] NULL,
	[Security_Attributes] [nvarchar](max) NULL,
	[WindowsMutexDescription] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[VocabularyID] [int] NULL,
	[ConfidenceLevelID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSMUTEXHANDLE]    Script Date: 04/03/2015 19:52:20 ******/
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

/****** Object:  Table [dbo].[WINDOWSMUTEXSECURITYATTRIBUTE]    Script Date: 04/03/2015 19:52:20 ******/
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

/****** Object:  Table [dbo].[WINDOWSNETWORKROUTEENTRY]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSNETWORKROUTEENTRY](
	[WindowsNetworkRouteEntryID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSNETWORKSHARE]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSNETWORKSHARE](
	[WindowsNetworkShareID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSPIPEOBJECT]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSPIPEOBJECT](
	[WindowsPipeObjectID] [int] IDENTITY(1,1) NOT NULL,
	[PipeObjectID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSPREFETCHACCESSEDFILELIST]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSPREFETCHACCESSEDFILELIST](
	[WindowsPrefetchObjectAccessedFileListID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsPrefetchObjectID] [int] NOT NULL,
	[AccessedFileListID] [int] NOT NULL,
	[timestamp] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSPREFETCHCHANGERECORD]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSPREFETCHCHANGERECORD](
	[WindowsPrefetchChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSPREFETCHOBJECT]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSPREFETCHOBJECT](
	[WindowsPrefetchObjectID] [int] IDENTITY(1,1) NOT NULL,
	[Application_File_Name] [nvarchar](500) NULL,
	[FileID] [int] NULL,
	[CPEName] [nvarchar](255) NULL,
	[Prefetch_Hash] [nchar](8) NULL,
	[Times_Executed] [bigint] NULL,
	[First_Run] [datetimeoffset](7) NULL,
	[Last_Run] [datetimeoffset](7) NULL,
	[VolumeObjectID] [int] NULL,
	[WindowsVolumeID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSPRIVILEGE]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSPRIVILEGE](
	[WindowsPrivilegeID] [int] IDENTITY(1,1) NOT NULL,
	[PrivilegeID] [int] NULL,
	[User_Right] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[VocabularyID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSPROCESS]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSPROCESS](
	[WindowsProcessID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessID] [int] NULL,
	[WindowsProcessGUID] [nvarchar](500) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[aslr_enabled] [bit] NULL,
	[dep_enabled] [bit] NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSPROCESSTOKEN]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSPROCESSTOKEN](
	[WindowsProcessTokenID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsProcessID] [int] NOT NULL,
	[TokenID] [int] NOT NULL,
	[IntegrityLevelID] [int] NULL,
	[ConfidenceLevelID] [int] NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSREGISTRYKEYOBJECT]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSREGISTRYKEYOBJECT](
	[WindowsRegistryKeyObjectID] [int] IDENTITY(1,1) NOT NULL,
	[Hive] [nvarchar](500) NULL,
	[operation] [nvarchar](50) NULL,
	[Full_Key] [nvarchar](500) NULL,
	[RegistryHiveID] [int] NULL,
	[Number_Values] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[comment] [nvarchar](max) NULL,
	[RegistryValuesID] [int] NULL,
	[Modified_Time] [datetimeoffset](7) NULL,
	[Creator_Username] [nvarchar](100) NULL,
	[AccountID] [int] NULL,
	[UserAccountID] [int] NULL,
	[WindowsUserAccountID] [int] NULL,
	[WindowsHandleListID] [int] NULL,
	[Number_Subkeys] [int] NULL,
	[RegistrySubkeysID] [int] NULL,
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

/****** Object:  Table [dbo].[WINDOWSSEMAPHORE]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSSEMAPHORE](
	[WindowsSemaphoreID] [int] IDENTITY(1,1) NOT NULL,
	[SemaphoreID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSSERVICE]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSSERVICE](
	[WindowsServiceID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSSYSTEM]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSSYSTEM](
	[WindowsSystemID] [int] IDENTITY(1,1) NOT NULL,
	[SystemID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSSYSTEMRESTORE]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSSYSTEMRESTORE](
	[WindowsSystemRestoreID] [int] IDENTITY(1,1) NOT NULL,
	[Restore_Point_Description] [nvarchar](max) NULL,
	[Restore_Point_Full_Path] [nvarchar](500) NULL,
	[Restore_Point_Name] [nvarchar](250) NULL,
	[Restore_Point_Type] [nvarchar](50) NULL,
	[ACL_Change_SID] [nvarchar](50) NULL,
	[ACL_Change_Username] [nvarchar](50) NULL,
	[Backup_File_Name] [nvarchar](50) NULL,
	[Change_Event] [nvarchar](50) NULL,
	[ChangeLog_Entry_Flags] [nvarchar](50) NULL,
	[ChangeLog_Entry_Sequence_Number] [int] NULL,
	[ChangeLog_Entry_Type] [nvarchar](50) NULL,
	[Change_Log_File_Name] [nvarchar](50) NULL,
	[Created] [datetimeoffset](7) NULL,
	[File_Attributes] [nvarchar](50) NULL,
	[New_File_Name] [nvarchar](50) NULL,
	[Original_File_Name] [nvarchar](50) NULL,
	[Original_Short_File_Name] [nvarchar](50) NULL,
	[Process_Name] [nvarchar](50) NULL,
	[Registry_Hive_List] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSTASK]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSTASK](
	[WindowsTaskID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [int] NULL,
	[SessionCronID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[Status] [nvarchar](50) NULL,
	[TaskStatusID] [int] NULL,
	[Priority] [nvarchar](50) NULL,
	[TaskPriorityID] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[Application_Name] [nvarchar](50) NULL,
	[ApplicationID] [int] NULL,
	[CPEName] [nvarchar](255) NULL,
	[Parameters] [nvarchar](max) NULL,
	[Flags] [nvarchar](max) NULL,
	[Account_Name] [nvarchar](50) NULL,
	[AccountID] [int] NULL,
	[Account_Run_Level] [nvarchar](50) NULL,
	[Account_Logon_Type] [nvarchar](50) NULL,
	[Creator] [nvarchar](50) NULL,
	[Creation_Date] [datetimeoffset](7) NULL,
	[Most_Recent_Run_Time] [datetimeoffset](7) NULL,
	[Exit_Code] [int] NULL,
	[Max_Run_Time] [int] NULL,
	[Next_Run_Time] [datetimeoffset](7) NULL,
	[Comment] [nvarchar](max) NULL,
	[Working_Directory] [nvarchar](500) NULL,
	[DirectoryID] [int] NULL,
	[Work_Item_Data] [nvarchar](max) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSTHREAD]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSTHREAD](
	[WindowsThreadID] [int] IDENTITY(1,1) NOT NULL,
	[Thread_ID] [int] NULL,
	[WindowsHandleID] [int] NULL,
	[ThreadRunningStatusID] [int] NULL,
	[Running_Status] [nvarchar](50) NULL,
	[Context] [nvarchar](max) NULL,
	[Priority] [int] NULL,
	[Creation_Flags] [nvarchar](50) NULL,
	[Creation_Time] [datetimeoffset](7) NULL,
	[Start_Address] [nvarchar](50) NULL,
	[StartMemoryAddressID] [int] NULL,
	[Parameter_Address] [nvarchar](50) NULL,
	[ParameterMemoryAddressID] [int] NULL,
	[Security_Attributes] [nvarchar](50) NULL,
	[Stack_Size] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSUSERACCOUNT]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSUSERACCOUNT](
	[WindowsUserAccountID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsUserAccountGUID] [nvarchar](500) NULL,
	[AccountID] [int] NULL,
	[UserAccountID] [int] NULL,
	[WindowsComputerAccountID] [int] NULL,
	[Security_ID] [nvarchar](50) NULL,
	[Security_Type] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[CreationObjectID] [int] NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[TrustLevelID] [int] NULL,
	[TrustReasonID] [int] NULL,
	[isEncrypted] [bit] NULL,
	[suspected_malicious] [bit] NULL,
	[SuspectedMaliciousReasonID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSUSERACCOUNTCHANGERECORD]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSUSERACCOUNTCHANGERECORD](
	[WindowsUserAccountChangeRecordID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSUSERACCOUNTPRIVILEGE]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSUSERACCOUNTPRIVILEGE](
	[WindowsUserAccountPrivilegeID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsUserAccountID] [int] NOT NULL,
	[WindowsPrivilegeID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSVOLUME]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSVOLUME](
	[WindowsVolumeID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsVolumeGUID] [nvarchar](500) NULL,
	[VolumeObjectID] [int] NULL,
	[Drive_Letter] [nvarchar](5) NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSVOLUMEATTRIBUTE]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSVOLUMEATTRIBUTE](
	[WindowsVolumeAttributeID] [int] IDENTITY(1,1) NOT NULL,
	[AttributeID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSVOLUMEATTRIBUTEENUM]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSVOLUMEATTRIBUTEENUM](
	[WindowsVolumeAttributeEnumID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsVolumeAttributeEnumValue] [nvarchar](50) NOT NULL,
	[WindowsVolumeAttributeEnumDescription] [nvarchar](max) NULL,
	[VocabularyID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSVOLUMEATTRIBUTESLIST]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSVOLUMEATTRIBUTESLIST](
	[WindowsVolumeAttributesListID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsVolumeID] [int] NOT NULL,
	[WindowsVolumeAttributeID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSVOLUMEENCRYPTION]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSVOLUMEENCRYPTION](
	[WindowsVolumeEncryptionID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsVolumeID] [int] NOT NULL,
	[EncryptionID] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[ConfidenceLevelID] [int] NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSWAITABLETIMER]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSWAITABLETIMER](
	[WindowsWaitableTimerID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINDOWSWAITABLETIMEROBJECT]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINDOWSWAITABLETIMEROBJECT](
	[WindowsWaitableTimerObjectID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsHandleID] [int] NULL,
	[WindowsWaitableTimerObjectName] [nvarchar](250) NULL,
	[Security_Attributes] [nvarchar](max) NULL,
	[WaitableTimerTypeID] [int] NULL,
	[CreatedDate] [datetimeoffset](7) NULL,
	[timestamp] [datetimeoffset](7) NULL,
	[ValidFromDate] [datetimeoffset](7) NULL,
	[ValidUntilDate] [datetimeoffset](7) NULL,
	[isEncrypted] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[WINWAITABLETIMER]    Script Date: 04/03/2015 19:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WINWAITABLETIMER](
	[WinWaitableTimerID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO


