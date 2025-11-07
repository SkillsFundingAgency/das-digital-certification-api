CREATE TABLE [dbo].[UserAuthorisation]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [ULN] BIGINT NOT NULL,
    [AuthorisedAt] DATETIME2 NULL,
    [ValidFrom] DATETIME2 (0) GENERATED ALWAYS AS ROW START,
    [ValidTo] DATETIME2 (0) GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[UserAuthorisationHistory]));
GO

CREATE UNIQUE INDEX UX_UserAuthorisation_ULN ON dbo.UserAuthorisation (ULN);
GO

ALTER TABLE [dbo].[UserAuthorisation] ADD CONSTRAINT FK_UserAuthorisation_User
FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([Id]);
GO

