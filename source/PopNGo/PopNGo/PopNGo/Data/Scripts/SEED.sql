USE [PopNGoDB];

-- Insert into PG_User
INSERT INTO [PG_User] ([ASPNETUserID])
    VALUES
    ('6b6c5d33-e6d7-4d18-a565-a657eaf7a8a5'),
    ('bb60f066-da0e-46b5-83d0-4d73d306dc64'),
    ('c4081410-2b09-4992-803a-ee550c92c76a'),
    ('2ce786bf-706b-4ba4-87b3-b3f0cbc6a1ad');

-- Get the IDs of the users we just inserted
DECLARE @user1ID int = (SELECT [ID] FROM [PG_User] WHERE [ASPNETUserID] = '6b6c5d33-e6d7-4d18-a565-a657eaf7a8a5');
DECLARE @user2ID int = (SELECT [ID] FROM [PG_User] WHERE [ASPNETUserID] = 'bb60f066-da0e-46b5-83d0-4d73d306dc64');

-- Insert into Event
INSERT INTO [Event] ([ApiEventID], [EventDate], [EventName], [EventDescription], [EventLocation], [EventImage]) 
VALUES ('event1', '2022-01-01T00:00:00', 'Event 1', 'Description 1', 'Location 1', NULL);
INSERT INTO [Event] ([ApiEventID], [EventDate], [EventName], [EventDescription], [EventLocation], [EventImage]) 
VALUES ('event2', '2022-01-02T00:00:00', 'Event 2', 'Description 2', 'Location 2', NULL);

-- Get the IDs of the events we just inserted
DECLARE @event1ID int = (SELECT [ID] FROM [Event] WHERE [ApiEventID] = 'event1');
DECLARE @event2ID int = (SELECT [ID] FROM [Event] WHERE [ApiEventID] = 'event2');

-- Insert into FavoriteEvents
INSERT INTO [FavoriteEvents] ([UserID], [EventID]) VALUES (@user1ID, @event1ID);
INSERT INTO [FavoriteEvents] ([UserID], [EventID]) VALUES (@user2ID, @event2ID);

-- Insert into EventHistory
INSERT INTO [EventHistory] ([UserID], [EventID], [ViewedDate]) VALUES (@user1ID, @event1ID, '2022-01-01T00:00:00');
INSERT INTO [EventHistory] ([UserID], [EventID], [ViewedDate]) VALUES (@user2ID, @event2ID, '2022-01-02T00:00:00');

-- Insert into Tag
INSERT INTO [Tag] ([Name], [BackgroundColor], [TextColor])
    VALUES
    ('Music', '#d35400', '#FFFFFF'),
    ('Show', '#3498db', '#FFFFFF'),
    ('Concert', '#2ecc71', '#FFFFFF'),
    ('Comedy', '#f1c40f', '#FFFFFF');
