CREATE DATABASE [PopNGoDB];

USE [PopNGoDB];

CREATE TABLE [PG_User] (
  [ID] INTEGER PRIMARY KEY IDENTITY(1, 1),
  [ASPNETUserID] NVARCHAR(255) NOT NULL
);
