CREATE TABLE [dbo].[UserAuthorisationHistory]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [ULN] BIGINT NOT NULL,
    [AuthorisedAt] DATETIME2 NULL,
    [ValidFrom] DATETIME2 (0) NOT NULL,
    [ValidTo] DATETIME2 (0) NOT NULL
)