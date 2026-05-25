DELETE FROM [dbo].[workoutComments];
DELETE FROM [dbo].[scheduleWorkouts];
DELETE FROM [dbo].[workoutExercises];
DELETE FROM [dbo].[workoutPlans];
DELETE FROM [dbo].[users];
DELETE FROM [dbo].[exercises];

DBCC CHECKIDENT ('[dbo].[exercises]', RESEED, 0);
-- pas5word-*123
INSERT INTO [dbo].[users] ([Id], [FullName], [Email], [UserName], [Password]) VALUES
('11111111-1111-1111-1111-111111111111', 'Nguyen Van A', 'user@example.com', 'user@example.com', 'AQAAAAIAAYagAAAAEBcU/xof49BqYC/l4xY39c3CfAPaNGssQONUYlU6MALNX7e9Bihk2R2kF+3Qz2N9kw=='); 

INSERT INTO [dbo].[exercises] ([Name], [Description], [Category]) VALUES
('Barbell Bench Press', 'A classic compound lift targeting the chest, shoulders, and triceps.', 'Chest'), -- ID = 1
('Incline Dumbbell Press', 'An upper-chest focused pressing movement using dumbbells.', 'Chest'), -- ID = 2
('Cable Chest Fly', 'An isolation movement targeting the chest muscles with constant cable tension.', 'Chest'), -- ID = 3
('Barbell Back Squat', 'A fundamental lower body exercise targeting the quadriceps and glutes.', 'Legs'), -- ID = 4
('Leg Press', 'A machine-based lower body press focusing primarily on the quadriceps.', 'Legs'), -- ID = 5
('Leg Extensions', 'An isolation exercise targeting the quadriceps on a machine.', 'Legs'), -- ID = 6
('Conventional Deadlift', 'A heavy compound movement that works the entire posterior chain.', 'Back'), -- ID = 7
('Lat Pulldown', 'A vertical pulling exercise targeting the latissimus dorsi.', 'Back'), -- ID = 8
('Barbell Row', 'A horizontal pulling movement targeting the upper and mid back.', 'Back'), -- ID = 9
('Overhead Press', 'A vertical press targeting the shoulders and upper chest.', 'Shoulders'), -- ID = 10
('Dumbbell Lateral Raise', 'An isolation exercise targeting the lateral deltoids.', 'Shoulders'), -- ID = 11
('Barbell Bicep Curl', 'An isolation movement targeting the biceps.', 'Arms'), -- ID = 12
('Tricep Rope Pushdown', 'An isolation exercise targeting the lateral and medial heads of the triceps.', 'Arms'),-- ID = 13
('Incline Bench Skull Crushers', 'An overhead tricep extension targeting the long head of the triceps.', 'Arms'),-- ID = 14
('Hanging Leg Raise', 'An advanced core exercise targeting the lower abdominals.', 'Core'), -- ID = 15
('Plank', 'An isometric core exercise targeting the deep abdominal muscles.', 'Core'), -- ID = 16
('Assault Bike Interval', 'High-intensity interval cardiovascular training using an air resistance bike.', 'Cardio'),-- ID = 17
('Treadmill Run', 'Steady-state or interval running on a treadmill.', 'Cardio'); -- ID = 18

INSERT INTO [dbo].[workoutPlans] ([Id], [Name], [Description], [UserId]) VALUES
('F1111111-1111-1111-1111-111111111111', 'Push Day Routine', 'Focuses on chest, shoulders, and triceps.', '11111111-1111-1111-1111-111111111111'),
('F2222222-2222-2222-2222-222222222222', 'Leg Day Hypertrophy', 'Focused on building quadriceps, hamstrings, and calves.', '11111111-1111-1111-1111-111111111111'),
('F3333333-3333-3333-3333-333333333333', 'Pull Day & Biceps', 'Targeting the back muscles and building biceps strength.', '11111111-1111-1111-1111-111111111111'),
('F4444444-4444-4444-4444-444444444444', 'Cardio & Core Workout', 'High intensity conditioning session paired with abdominal training.', '11111111-1111-1111-1111-111111111111');

INSERT INTO [dbo].[workoutExercises] ([Id], [ExerciseId], [WorkoutId], [Sets], [Repetitions], [Weight]) VALUES
-- Bài tập cho Push Day (F1111111...)
('D1111111-1111-1111-1111-111111111111', 1, 'F1111111-1111-1111-1111-111111111111', 4, 8, 80.00), -- Barbell Bench Press (ID 1)
('D2222222-2222-2222-2222-222222222222', 2, 'F1111111-1111-1111-1111-111111111111', 3, 10, 24.00), -- Incline Dumbbell Press (ID 2)
('D3333333-3333-3333-3333-333333333333', 10, 'F1111111-1111-1111-1111-111111111111', 3, 8, 40.00), -- Overhead Press (ID 10)
('D4444444-4444-4444-4444-444444444444', 13, 'F1111111-1111-1111-1111-111111111111', 3, 12, 25.00),-- Tricep Rope Pushdown (ID 13)

-- Bài tập cho Leg Day (F2222222...)
('D5555555-5555-5555-5555-555555555555', 4, 'F2222222-2222-2222-2222-222222222222', 4, 6, 100.00), -- Barbell Back Squat (ID 4)
('D6666666-6666-6666-6666-666666666666', 5, 'F2222222-2222-2222-2222-222222222222', 3, 10, 160.00),-- Leg Press (ID 5)
('D7777777-7777-7777-7777-777777777777', 6, 'F2222222-2222-2222-2222-222222222222', 3, 12, 45.00), -- Leg Extensions (ID 6)

-- Bài tập cho Pull Day (F3333333...)
('D8888888-8888-8888-8888-888888888888', 7, 'F3333333-3333-3333-3333-333333333333', 3, 5, 120.00), -- Conventional Deadlift (ID 7)
('D9999999-9999-9999-9999-999999999999', 8, 'F3333333-3333-3333-3333-333333333333', 4, 10, 60.00), -- Lat Pulldown (ID 8)
('DA111111-1111-1111-1111-111111111111', 12, 'F3333333-3333-3333-3333-333333333333', 3, 12, 30.00), -- Barbell Bicep Curl (ID 12)

-- Bài tập cho Cardio & Core (F4444444...)
('DA222222-2222-2222-2222-222222222222', 17, 'F4444444-4444-4444-4444-444444444444', 1, 15, 0.00), -- Assault Bike Interval (ID 17)
('DA333333-3333-3333-3333-333333333333', 16, 'F4444444-4444-4444-4444-444444444444', 3, 60, 0.00); -- Plank (ID 16)

INSERT INTO [dbo].[scheduleWorkouts] ([Id], [ScheduledDate], [WorkoutId]) VALUES
('C1111111-1111-1111-1111-111111111111', '2026-05-20 08:00:00', 'F1111111-1111-1111-1111-111111111111'),
('C2222222-2222-2222-2222-222222222222', '2026-05-21 17:30:00', 'F2222222-2222-2222-2222-222222222222'),
('C3333333-3333-3333-3333-333333333333', '2026-05-23 09:00:00', 'F3333333-3333-3333-3333-333333333333'),
('C4444444-4444-4444-4444-444444444444', '2026-05-24 16:00:00', 'F4444444-4444-4444-4444-444444444444');

INSERT INTO [dbo].[workoutComments] ([Id], [WorkoutId], [Comment], [Date]) VALUES
('B1111111-1111-1111-1111-111111111111', 'F1111111-1111-1111-1111-111111111111', 'Bench press felt heavy today. Will stick to 80kg next week.', '2026-05-20 09:30:00'),
('B2222222-2222-2222-2222-222222222222', 'F2222222-2222-2222-2222-222222222222', 'Leg press sets felt solid. Can potentially increase weight next session.', '2026-05-21 19:00:00'),
('B3333333-3333-3333-3333-333333333333', 'F4444444-4444-4444-4444-444444444444', 'Extremely sweaty session, great cardio pacing!', '2026-05-24 17:15:00');