CREATE TABLE [dbo].[AdminActions]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Username] VARCHAR(255) NOT NULL,
    [ActionTime] DATETIME2 NOT NULL,
    [Action] VARCHAR(50) NOT NULL,
    [UserActionId] BIGINT NOT NULL,
    [ValidFrom] DATETIME2 (0) GENERATED ALWAYS AS ROW START,
    [ValidTo] DATETIME2 (0) GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),
    CONSTRAINT [FK_AdminActions_UserActions] FOREIGN KEY ([UserActionId]) REFERENCES [dbo].[UserActions]([Id])
)

WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[AdminActionsHistory]));
GO

CREATE INDEX IX_AdminActions_UserActionId ON [dbo].[AdminActions]([UserActionId]);
GO
