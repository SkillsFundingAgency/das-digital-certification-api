CREATE TABLE [dbo].[UserActionsHistory]
(
    [Id] BIGINT NOT NULL,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [ActionType] INT NOT NULL,
    [ActionCode] VARCHAR(50) NULL,
    [ActionTime] DATETIME2 NOT NULL,
    [FamilyName] VARCHAR(255) NOT NULL,
    [GivenNames] VARCHAR(255) NOT NULL,
    [CertificateId] UNIQUEIDENTIFIER NULL,
    [CertificateType] VARCHAR(20) NULL,
    [CourseName] VARCHAR(1000) NULL,
    [ValidFrom] DATETIME2 (0) NOT NULL,
    [ValidTo] DATETIME2 (0) NOT NULL
)
