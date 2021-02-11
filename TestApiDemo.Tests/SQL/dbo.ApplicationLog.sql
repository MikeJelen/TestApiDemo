use MikeDemo
go

DROP TABLE IF EXISTS [dbo].[ApplicationLog]
GO

CREATE TABLE [dbo].[ApplicationLog] (
    [system_logging_guid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
    [entered_date] [datetime2] NULL default(getutcdate()),
    [log_application] [varchar](200) NULL,
    [log_date] [datetime2] NULL default(getutcdate()),
    [log_level] [varchar](100) NULL,
    [log_logger] [varchar](max) NULL,
    [log_message] [varchar](max) NULL,
    [log_machine_name] [varchar](max) NULL,
    [log_user_name] [varchar](max) NULL,
    [log_call_site] [varchar](max) NULL,
    [log_thread] [varchar](100) NULL,
    [log_exception] [varchar](max) NULL,
    [log_stacktrace] [varchar](max) NULL,
CONSTRAINT [PK_system_logging] PRIMARY KEY CLUSTERED
(
    [system_logging_guid] ASC
)) 

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ApplicationLog] ADD  CONSTRAINT [DF_ApplicationLog_guid]  DEFAULT (newid()) FOR [system_logging_guid]
GO