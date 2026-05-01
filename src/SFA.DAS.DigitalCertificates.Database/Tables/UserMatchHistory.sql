CREATE TABLE [dbo].[UserMatchHistory]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [Uln] BIGINT NULL,
    [FamilyName] VARCHAR(255) NOT NULL,
    [DateOfBirth] DATE NOT NULL,
    [CertificateType] VARCHAR(20) NULL,
    [CourseCode] VARCHAR(255) NULL,
    [CourseName] VARCHAR(1000) NULL,
    [CourseLevel] VARCHAR(20) NULL,
    [DateAwarded] INT NULL,
    [ProviderName] VARCHAR(255) NULL,
    [Ukprn] INT NULL,
    [IsMatched] BIT NOT NULL,
    [IsFailed] BIT NOT NULL,
    [ValidFrom] DATETIME2 (0) NOT NULL,
    [ValidTo] DATETIME2 (0) NOT NULL
)
GO
