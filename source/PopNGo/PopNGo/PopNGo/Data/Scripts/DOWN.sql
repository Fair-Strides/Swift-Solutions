USE [PopNGoDB];

ALTER TABLE [EventHistory] DROP CONSTRAINT FK_EventHistory_UserID;

DROP TABLE [PG_User];

DROP TABLE [EventHistory];
