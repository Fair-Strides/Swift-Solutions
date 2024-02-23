USE [PopNGoDB];

ALTER TABLE [EventHistory] DROP CONSTRAINT FK_EventHistory_UserID;
ALTER TABLE [EventHistory] DROP CONSTRAINT FK_EventHistory_EventID;

DROP TABLE [PG_User];

DROP TABLE [EventHistory];

DROP TABLE [Event];