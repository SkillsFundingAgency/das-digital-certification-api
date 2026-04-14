CREATE TABLE [dbo].[UserMatchHistory]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [Uln] BIGINT NULL,
    [FamilyName] VARCHAR(255) NOT NULL,
    [DateOfBirth] DATE NOT NULL,
    [CertificateType] VARCHAR(20) NOT NULL,
    [CourseCode] VARCHAR(255) NOT NULL,
    [CourseName] VARCHAR(1000) NOT NULL,
    [CourseLevel] INT NOT NULL,
    [DateAwarded] DATETIME2 NULL,
    [ProviderName] VARCHAR(255) NOT NULL,
    [Ukprn] INT NOT NULL,
    [IsMatched] BIT NOT NULL,
    [IsFailed] BIT NOT NULL,
    [ValidFrom] DATETIME2 (0) NOT NULL,
    [ValidTo] DATETIME2 (0) NOT NULL
)
GO
