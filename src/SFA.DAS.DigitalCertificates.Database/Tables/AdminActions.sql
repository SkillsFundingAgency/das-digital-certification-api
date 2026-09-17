CREATE TABLE [dbo].[AdminActions]
(
    [Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_AdminActions_Id] DEFAULT NEWID() PRIMARY KEY,
    [Username] VARCHAR(255) NOT NULL,
    [ActionTime] DATETIME2 NOT NULL CONSTRAINT [DF_AdminActions_ActionTime] DEFAULT SYSUTCDATETIME(),
    [Action] VARCHAR(50) NOT NULL,
    [UserActionId] BIGINT NOT NULL,
    CONSTRAINT [FK_AdminActions_UserActions] FOREIGN KEY ([UserActionId]) REFERENCES [dbo].[UserActions]([Id])
)
GO

CREATE INDEX IX_AdminActions_UserActionId ON [dbo].[AdminActions]([UserActionId]);
GO
