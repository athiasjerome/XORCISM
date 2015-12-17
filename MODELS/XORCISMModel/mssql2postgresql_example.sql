SELECT convert_mssqlddl2pgsql('
CREATE TABLE [dbo].[ACCOUNT](
	[AccountID] [int] IDENTITY(1,1) NOT NULL,
	[AccountName] [nvarchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[ValidFromDate] [datetimeoffset](7) NOT NULL,
	[ValidUntilDate] [datetimeoffset](7) NOT NULL,
	[AccountDescription] [nvarchar](max) NULL,
	[AccountDomain] [nvarchar](500) NULL,
	[disabled] [bit] NULL,
	[locked_out] [bit] NULL,
 CONSTRAINT [PK_ACCOUNT] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
', false, true, 'public');