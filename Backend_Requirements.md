# Tài Liệu Đặc Tả Yêu Cầu Backend (API & Database Specification)
## Dành cho dự án Workout Tracker

Tài liệu này được biên soạn dựa trên phân tích mã nguồn Frontend (thư mục `/FE`) đối chiếu với cấu trúc cơ sở dữ liệu SQL và thực thể Domain của Backend hiện tại (thư mục `/BE` sử dụng .NET Clean Architecture).

---

## 1. Bản Đồ Đối Chiếu Thực Thể (Entity Mapping)

Để đảm bảo đồng bộ giữa Client và Server, dưới đây là bảng đối chiếu cấu trúc dữ liệu giữa Frontend (TypeScript Types) và Backend (C# Entities & SQL Tables):

| Thực thể FE (TypeScript) | Thực thể BE (C# Domain) | Bảng Cơ Sở Dữ Liệu (SQL) | Mô tả |
| :--- | :--- | :--- | :--- |
| **`User`** | `User.cs` | `[dbo].[users]` | Thông tin tài khoản người dùng |
| **`Workout`** | `WorkoutPlan.cs` | `[dbo].[workoutPlans]` | Giáo án/Lịch trình bài tập lớn |
| **`WorkoutExercise`** | `WorkoutExercise.cs` | `[dbo].[workoutExercises]` | Chi tiết bài tập thuộc một Workout (số set, reps, tạ) |
| **`Exercise`** | `Exercise.cs` | `[dbo].[exercises]` | Danh mục các bài tập có sẵn trong hệ thống |
| **`WorkoutSchedule`** | `ScheduleWorkout.cs` | `[dbo].[scheduleWorkouts]` | Lịch hẹn tập thực tế của người dùng |
| **`WorkoutComment`** | `WorkoutComments.cs` | `[dbo].[workoutComments]` | Bình luận/Ghi chú sau mỗi buổi tập |

---

## 2. Danh Sách Chi Tiết API Yêu Cầu

Tất cả các API yêu cầu xác thực cần gửi kèm Header:
```http
Authorization: Bearer <JWT_TOKEN>
```
*Token JWT sau khi giải mã (decode) phía FE sẽ trích xuất ra các trường: `sub` (UserId), `email` (Email), và `unique_name` / `name` (Username/FullName).*

### 2.1. Nhóm Xác Thực (Authentication - `/auth`)

#### 1. Đăng nhập (`POST /auth/login`)
*   **Request Body**:
    ```json
    {
      "userName": "user@example.com", // Lưu ý: FE truyền Email hoặc Username vào trường này
      "password": "yourpassword"
    }
    ```
*   **Response (200 OK)**:
    ```json
    {
      "token": "string (JWT Token chứa các Claim: sub, email, unique_name, name)"
    }
    ```

#### 2. Đăng ký tài khoản (`POST /auth/register`)
*   **Request Body**:
    ```json
    {
      "userName": "username",
      "email": "user@example.com",
      "fullName": "string",
      "password": "yourpassword"
    }
    ```
*   **Response (200 OK)**: Chứa token JWT đăng nhập ngay lập tức.
    ```json
    {
      "token": "string (JWT Token)"
    }
    ```

---

### 2.2. Nhóm Bài Tập Danh Mục (Exercises - `/exercises`)

#### 1. Lấy danh sách toàn bộ bài tập danh mục (`GET /exercises`)
*   **Mô tả**: Trả về danh sách bài tập chuẩn hệ thống (không lọc theo user) để người dùng chọn khi thiết kế buổi tập.
*   **Response (200 OK)**:
    ```json
    [
      {
        "id": 1,
        "name": "Barbell Bench Press",
        "category": "Chest",
        "difficulty": "Intermediate" // "Beginner" | "Intermediate" | "Advanced"
      }
    ]
    ```

---

### 2.3. Nhóm Giáo Án Tập Luyện (Workouts - `/workouts`)

#### 1. Lấy tất cả giáo án của User hiện tại (`GET /workouts`)
*   **Lọc dữ liệu**: Chỉ trả về các giáo án mà `UserId` bằng `sub` trích xuất từ Token JWT.
*   **Response (200 OK)**:
    ```json
    [
      {
        "id": "guid-string",
        "name": "Hypertrophy Push A",
        "description": "Focusing on chest development..."
      }
    ]
    ```

#### 2. Lấy chi tiết một giáo án (`GET /workouts/{id}`)
*   **Response (200 OK)**:
    ```json
    {
      "id": "guid-string",
      "name": "Hypertrophy Push A",
      "description": "Focusing on chest development..."
    }
    ```

#### 3. Tạo mới giáo án (`POST /workouts`)
*   **Request Body**:
    ```json
    {
      "name": "Heavy Pull Day",
      "description": "Targeting deadlifts, upper-back thickness..."
    }
    ```
    *Mẹo Backend: Tự động gán `UserId` từ JWT Token của user gửi request.*
*   **Response (201 Created)**: Trả về đối tượng vừa tạo kèm `id` (GUID).

#### 4. Cập nhật thông tin giáo án (`PUT /workouts/{id}`)
*   **Request Body**:
    ```json
    {
      "name": "New Name",
      "description": "New Description"
    }
    ```
*   **Response (200 OK)**: Trả về thực thể `WorkoutPlan` sau cập nhật.

#### 5. Xóa giáo án (`DELETE /workouts/{id}`)
*   **Yêu cầu**: Cần xử lý xóa Cascade các bản ghi liên quan ở các bảng phụ (xem phần Cải thiện).
*   **Response (204 No Content / 200 OK)**

---

### 2.4. Chi Tiết Bài Tập Trong Giáo Án (Workout Exercises - `/workout-exercises`)

#### 1. Lấy danh sách các bài tập của một Giáo án (`GET /workout-exercises/{workoutId}`)
*   **Response (200 OK)**: Cần trả kèm tên bài tập (`exerciseName`) thông qua quan hệ truy vấn kết hợp (Join).
    ```json
    [
      {
        "id": "guid-string",
        "workoutId": "guid-string",
        "exerciseId": 1,
        "sets": 4,
        "repetitions": 8,
        "weight": 80.0,
        "exerciseName": "Barbell Bench Press" // JOIN từ bảng exercises
      }
    ]
    ```

#### 2. Thêm bài tập vào Giáo án (`POST /workout-exercises`)
*   **Request Body**:
    ```json
    {
      "workoutId": "guid-string",
      "exerciseId": 1,
      "sets": 4,
      "repetitions": 8,
      "weight": 80.0
    }
    ```
*   **Response (201 Created)**: Trả về `WorkoutExercise` kèm tên bài tập.

#### 3. Cập nhật thông số bài tập (`PUT /workout-exercises/{id}`)
*   **Request Body**:
    ```json
    {
      "sets": 4,
      "repetitions": 10,
      "weight": 85.0,
      "exerciseId": 1
    }
    ```
*   **Response (200 OK)**: Trả về đối tượng sau cập nhật.

#### 4. Xóa bài tập khỏi Giáo án (`DELETE /workout-exercises/{id}`)
*   **Response (204 No Content / 200 OK)**

---

### 2.5. Nhóm Lịch Trình Luyện Tập (Schedules - `/workout-schedules`)

#### 1. Lấy toàn bộ lịch tập (`GET /workout-schedules`)
*   **Response (200 OK)**: Cần kèm `workoutName` và sắp xếp theo `scheduledDate` tăng dần (Ascending).
    ```json
    [
      {
        "id": "guid-string",
        "scheduledDate": "2026-06-02T08:00:00.000Z",
        "workoutId": "guid-string",
        "workoutName": "Push Day Routine" // JOIN từ bảng workoutPlans
      }
    ]
    ```

#### 2. Lấy danh sách lịch tập của một Giáo án cụ thể (`GET /workout-schedules/workout/{workoutId}`)
*   **Response (200 OK)**: Trả về danh sách tương tự như trên lọc theo `workoutId`.

#### 3. Thiết lập lịch tập mới (`POST /workout-schedules`)
*   **Request Body**:
    ```json
    {
      "scheduledDate": "2026-06-03T07:00:00.000Z",
      "workoutId": "guid-string"
    }
    ```
*   **Response (201 Created)**

#### 4. Cập nhật thời gian lịch tập (`PUT /workout-schedules/{id}`)
*   **Request Body**:
    ```json
    {
      "scheduledDate": "2026-06-04T07:00:00.000Z"
    }
    ```
*   **Response (200 OK)**

#### 5. Hủy lịch tập (`DELETE /workout-schedules/{id}`)
*   **Response (204 No Content)**

---

### 2.6. Nhóm Bình Luận & Ghi Chú (Comments - `/workout-comments`)

#### 1. Lấy bình luận của một Giáo án (`GET /workout-comments/{workoutId}`)
*   **Yêu cầu**: Sắp xếp bình luận mới nhất lên đầu (`Date` Descending).
*   **Response (200 OK)**:
    ```json
    [
      {
        "id": "guid-string",
        "workoutId": "guid-string",
        "comment": "Nice session!",
        "userName": "User Full Name", // Cần JOIN từ bảng users qua UserId
        "userId": "guid-string-user",
        "createdAt": "2026-06-02T16:00:00.000Z" // Map từ cột Date trong Database
      }
    ]
    ```

#### 2. Viết bình luận mới (`POST /workout-comments`)
*   **Request Body**:
    ```json
    {
      "workoutId": "guid-string",
      "comment": "This workout was awesome!"
    }
    ```
*   **Response (201 Created)**: Trả về bản ghi bình luận đầy đủ kèm `userName` và `userId` của người gửi bình luận.

#### 3. Chỉnh sửa bình luận (`PUT /workout-comments/{id}`)
*   **Request Body**:
    ```json
    {
      "comment": "Updated comment text"
    }
    ```
*   **Response (200 OK)**

#### 4. Xóa bình luận (`DELETE /workout-comments/{id}`)
*   **Response (204 No Content)**

---

### 2.7. Báo Cáo & Thống Kê (Reports - `/reports`)

#### 1. Lấy dữ liệu thống kê tổng hợp (`GET /reports`)
*   **Response (200 OK)**:
    ```json
    {
      "totalWorkouts": 12,       // Tổng số buổi tập đã hoàn thành trong lịch trình (IsCompleted = true)
      "totalVolume": 45200.0,    // Tổng khối lượng tạ tích lũy = Tổng của (Sets * Repetitions * Weight)
      "streakDays": 5,           // Chuỗi ngày tập liên tục hiện tại
      "workoutsThisWeek": 3,     // Số buổi tập đã tập trong tuần hiện tại
      "weeklyWorkouts": [        // Thống kê 4 tuần gần nhất
        {
          "week": "Wk 22",       // Tên tuần
          "count": 4,            // Số buổi trong tuần
          "volume": 12000.0      // Khối lượng tạ trong tuần đó
        }
      ],
      "recentActivity": [        // 5 buổi tập gần đây nhất
        {
          "id": "schedule-guid",
          "workoutName": "Hypertrophy Push A",
          "date": "2026-06-01T08:00:00.000Z",
          "exercisesCount": 4    // Số lượng bài tập có trong giáo án đó
        }
      ]
    }
    ```

---

## 3. Các Điểm Cần Cải Thiện & Lưu Ý Quan Trọng (Notes & Improvements)

> [!WARNING]
> Phía dưới đây là danh sách lỗi thiết kế cơ sở dữ liệu hiện tại trong file `WorkoutTracer.sql` và mã nguồn Backend cần được cập nhật ngay lập tức để tương thích tốt với Frontend.

### 1. Thiếu thông tin liên kết User trong Thực thể `WorkoutComments`
*   **Vấn đề**: 
    Trong file `WorkoutTracer.sql` và lớp `WorkoutComments.cs`, bảng bình luận chỉ có cấu trúc: `Id`, `WorkoutId`, `Comment`, `Date`.
    Nó hoàn toàn không lưu thông tin người viết bình luận (`UserId`). Frontend mong muốn hiển thị thông tin tác giả viết bình luận (`userName` và `userId`).
*   **Đề xuất cải thiện**: 
    1. Cập nhật DB: Thêm cột `UserId` (FK liên kết đến bảng `users`) vào bảng `workoutComments`.
    2. Cập nhật Domain Entity C#:
       ```csharp
       public Guid UserId { get; set; }
       [ForeignKey("UserId")]
       public virtual User User { get; set; } = null!;
       ```
    3. Khi client gọi `POST /workout-comments`, BE sẽ tự động lấy UserId từ Token để gán vào bản ghi bình luận.

### 2. Tên các thuộc tính JSON trả về khác với Database
Để khớp với Frontend mà không cần viết quá nhiều mapping phức tạp ở client:
*   Bảng `workoutComments` có trường `Date`. Trả về JSON nên đổi tên (Serialize) thành `createdAt` hoặc cấu hình viết thường chữ cái đầu (camelCase) thành `createdAt` trong JSON.
*   Bảng `scheduleWorkouts` có trường `ScheduledDate`. JSON trả về cần khớp định dạng `scheduledDate`.

### 3. Tối ưu hiệu năng thông qua Eager Loading (EF Core JOIN)
*   **Tránh N+1 Query**: Khi lấy chi tiết danh sách bài tập hoặc danh sách lịch trình, BE cần sử dụng `.Include()` hoặc câu lệnh SQL JOIN để đính kèm dữ liệu từ bảng `exercises` (lấy `Name`) và `workoutPlans` (lấy `Name`).
*   Ví dụ API `GET /workout-schedules`:
    ```csharp
    // C# LINQ ví dụ
    var schedules = await _context.ScheduleWorkouts
        .Include(s => s.Workout)
        .OrderBy(s => s.ScheduledDate)
        .Select(s => new WorkoutScheduleDto {
            Id = s.Id,
            ScheduledDate = s.ScheduledDate,
            WorkoutId = s.WorkoutId,
            WorkoutName = s.Workout.Name // Đảm bảo trường này được map sang JSON gửi về FE
        }).ToListAsync();
    ```

### 4. Ràng buộc xóa Cascade (Cascading Delete)
*   Khi người dùng xóa một giáo án (`WorkoutPlan`):
    *   Cần xóa tất cả liên kết bài tập tương ứng trong `workoutExercises`.
    *   Cần xóa các bình luận tương ứng trong `workoutComments`.
    *   Cần xóa lịch hẹn tương ứng trong `scheduleWorkouts`.
*   Nên thiết lập CASCADE DELETE trên mức Database SQL hoặc cấu hình Cascade Delete trong `DbContext` để tránh lỗi ràng buộc khóa ngoại (Foreign Key Constraint).

### 5. Cải thiện API Đăng nhập (`POST /auth/login`)
*   Hiện tại, trong form Đăng nhập, người dùng nhập `email`, nhưng payload gửi lên Backend lại dùng key là `"userName"`.
*   **Đề xuất cải thiện**: BE nên viết logic đăng nhập linh hoạt ở Controller/Application layer:
    ```csharp
    // Kiểm tra xem đầu vào userName có chứa ký tự '@' không để tìm kiếm theo Email hoặc UserName tương ứng
    var user = request.UserName.Contains("@") 
        ? await _userRepository.GetByEmailAsync(request.UserName)
        : await _userRepository.GetByUserNameAsync(request.UserName);
    ```

### 6. Cơ chế Auto-complete / Search trong Exercises API
*   API `GET /exercises` hiện tại chỉ trả về toàn bộ bài tập. Khi số lượng bài tập lên tới hàng trăm, việc tải tất cả bài tập về client sẽ gây nghẽn băng thông.
*   **Đề xuất cải thiện**: Hỗ trợ phân trang (`page`, `pageSize`) và tìm kiếm từ khóa (`search`) ở Endpoint `/exercises`:
    *   `GET /exercises?search=press&page=1&pageSize=10`
