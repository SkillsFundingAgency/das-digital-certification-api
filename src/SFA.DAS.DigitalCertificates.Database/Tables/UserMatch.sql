CREATE TABLE [dbo].[UserMatch]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [Uln] BIGINT NULL,
    [FamilyName] VARCHAR(255) NOT NULL,
    [DateOfBirth] DATETIME2 NOT NULL,
    [CertificateType] VARCHAR(20) NOT NULL,
    [CourseCode] VARCHAR(255) NOT NULL,
    [CourseName] VARCHAR(1000) NOT NULL,
    [CourseLevel] INT NOT NULL,
    [DateAwarded] DATETIME2 NULL,
    [ProviderName] VARCHAR(255) NOT NULL,
    [Ukprn] INT NOT NULL,
    [IsMatched] BIT NOT NULL CONSTRAINT [DF_UserMatch_IsMatched] DEFAULT (0),
    [IsFailed] BIT NOT NULL CONSTRAINT [DF_UserMatch_IsFailed] DEFAULT (0),
    [ValidFrom] DATETIME2 (0) GENERATED ALWAYS AS ROW START,
    [ValidTo] DATETIME2 (0) GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),
    CONSTRAINT FK_UserMatch_User FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([Id])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[UserMatchHistory]));
GO

CREATE INDEX IX_UserMatch_UserId ON [dbo].[UserMatch]([UserId]);
GO
