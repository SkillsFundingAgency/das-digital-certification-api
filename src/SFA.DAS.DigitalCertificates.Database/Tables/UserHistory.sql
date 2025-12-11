CREATE TABLE [dbo].[UserHistory]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [GovUkIdentifier] VARCHAR(100) NOT NULL,
    [EmailAddress] VARCHAR(100) NOT NULL,
    [PhoneNumber] VARCHAR(20) NULL,
    [LastLoginAt] DATETIME2 NULL,
    [IsLocked] BIT NOT NULL CONSTRAINT [DF_UserHistory_IsLocked] DEFAULT (0),
    [ValidFrom] DATETIME2 (0) NOT NULL,
    [ValidTo] DATETIME2 (0) NOT NULL
)