CREATE TABLE [dbo].[UserIdentityHistory]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [FamilyName] VARCHAR(255) NOT NULL,
    [DateOfBirth] DATE NOT NULL,
    [GivenNames] VARCHAR(255) NOT NULL,
    [ValidSince] DATETIME2 NULL,
    [ValidUntil] DATETIME2 NULL,
    [ValidFrom] DATETIME2 (0) NOT NULL,
    [ValidTo] DATETIME2 (0) NOT NULL,
)
GO
