CREATE TABLE [dbo].[UserMatch]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [Uln] BIGINT NULL,
    [FamilyName] VARCHAR(255) NOT NULL,
    [DateOfBirth] DATE NOT NULL,
    [EventTime] DATETIME2 NOT NULL CONSTRAINT [DF_UserMatch_EventTime] DEFAULT GETUTCDATE(),
    [CertificateType] VARCHAR(20) NULL,
    [CourseCode] VARCHAR(255) NULL,
    [CourseName] VARCHAR(1000) NULL,
    [CourseLevel] VARCHAR(20) NULL,
    [YearAwarded] INT NULL,
    [ProviderName] VARCHAR(255) NULL,
    [Ukprn] INT NULL,
    [IsMatched] BIT NOT NULL CONSTRAINT [DF_UserMatch_IsMatched] DEFAULT (0),
    [IsFailed] BIT NOT NULL CONSTRAINT [DF_UserMatch_IsFailed] DEFAULT (0),
    CONSTRAINT FK_UserMatch_User FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([Id])
);
GO

CREATE INDEX IX_UserMatch_UserId ON [dbo].[UserMatch]([UserId]);
GO
