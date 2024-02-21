USE [PopNGoDB];

-- Drop the foreign key constraints
ALTER TABLE [EventHistory] DROP CONSTRAINT FK_EventHistory_UserID;
ALTER TABLE [EventHistory] DROP CONSTRAINT FK_EventHistory_EventID;
ALTER TABLE [FavoriteEvents] DROP CONSTRAINT FK_FavoriteEvents_UserID;

-- Drop the tables
DROP TABLE [PG_User];
DROP TABLE [FavoriteEvents];
DROP TABLE [EventHistory];
DROP TABLE [Event];

-- If you also want to drop the database, uncomment the following line
-- DROP DATABASE [PopNGoDB];