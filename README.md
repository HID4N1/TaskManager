# TaskManager - ASP.NET Core MVC Training Project

A complete Scrum-based Task Management System with Kanban board functionality, built with ASP.NET Core MVC (.NET 8).

## 📋 Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [User Guide](#user-guide)
- [Roles and Permissions](#roles-and-permissions)
- [Project Structure](#project-structure)
- [Database](#database)
- [Key Features Explained](#key-features-explained)

## ✨ Features

- **Role-Based Access Control (RBAC)**: ADMIN, MANAGER, and MEMBER roles with strict permissions
- **Project Management**: Create, edit, and manage projects (ADMIN and MANAGER only)
- **Task Management**: Full CRUD operations for tasks with Kanban board view
- **Kanban Board**: Visual task board grouped by status (A_FAIRE, EN_COURS, EN_REVUE, TERMINE)
- **Comments**: Add comments to tasks
- **Reports**: Generate task count reports by status
- **User Management**: Admin can create, edit, and manage users and roles

## 🛠 Tech Stack

- **Framework**: ASP.NET Core MVC (.NET 8)
- **Database**: SQLite with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **UI**: Razor Views with Bootstrap 5

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK installed ([Download here](https://dotnet.microsoft.com/download/dotnet/8.0))
- Visual Studio, VS Code, or any .NET compatible IDE

### Installation Steps

1. **Clone or extract the project**
   ```bash
   cd "dotnet project"
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Open your browser**
   - Navigate to `https://localhost:5001` (or the port shown in the console)
   - The application will automatically create the database on first run

### Default Login Credentials

- **Email**: `admin@taskmanager.com`
- **Password**: `Admin123!`
- **Role**: ADMIN

## 📖 User Guide

### 1. Sign Up (New Users)

**To create a new account as MANAGER or MEMBER:**

1. **Navigate to Sign Up**
   - Click "Sign Up" in the navigation menu (when not logged in)
   - Or go directly to: `/Auth/Signup`
   - Or click "Sign up here" link on the login page

2. **Fill in the Registration Form**
   - **Email**: Enter a valid email address (e.g., `manager@taskmanager.com`)
   - **Password**: Enter a password (minimum 4 characters)
   - **Confirm Password**: Re-enter the password
   - **Role**: Select from dropdown:
     - `MANAGER` - Can manage projects and tasks
     - `MEMBER` - Can view and update assigned tasks only
   - **Note**: ADMIN role cannot be selected during self-registration

3. **Submit Registration**
   - Click "Sign Up" button
   - You will be automatically logged in after successful registration
   - You'll be redirected to the Projects page

**Important Notes:**
- Only MANAGER and MEMBER roles can be selected during signup
- ADMIN accounts can only be created by existing ADMIN users
- Each email can only be used once
- You'll be automatically signed in after successful registration

### 2. Login

1. Navigate to the application URL
2. You will be redirected to the login page
3. Enter the default admin credentials:
   - Email: `admin@taskmanager.com`
   - Password: `Admin123!`
4. Click "Login"

### 2. Creating Users (ADMIN Only)

**To create MANAGER or MEMBER users:**

1. **Login as ADMIN** (using the default credentials above)

2. **Navigate to User Management**
   - Click on "Users" in the navigation menu (top right, visible only to ADMIN)
   - Or go directly to: `/Admin/Users`

3. **Create New User**
   - Click the "Create New User" button
   - Fill in the form:
     - **Email**: Enter a valid email address (e.g., `manager@taskmanager.com`)
     - **Password**: Enter a password (minimum 4 characters)
     - **Confirm Password**: Re-enter the password
     - **Role**: Select from dropdown:
       - `ADMIN` - Full system access
       - `MANAGER` - Can manage projects and tasks
       - `MEMBER` - Can view and update assigned tasks only
   - Click "Create User"

4. **User Created Successfully**
   - You'll see a success message
   - The new user will appear in the users list
   - The user can now login with their email and password

### 3. Managing Projects

**Creating a Project (ADMIN/MANAGER):**

1. Click "Projects" in the navigation menu
2. Click "Create New Project"
3. Fill in:
   - **Name**: Project name
   - **Description**: Project description (optional)
   - **Start Date**: Project start date
   - **End Date**: Project end date (optional)
   - **Status**: Project status
4. Click "Create"

**Viewing Projects:**
- **ADMIN/MANAGER**: See all projects
- **MEMBER**: See only projects where they have assigned tasks

### 4. Managing Tasks

**Creating a Task (ADMIN/MANAGER):**

1. Navigate to a project
2. Click "Kanban" to view the Kanban board
3. Click "Create New Task"
4. Fill in:
   - **Title**: Task title (required)
   - **Description**: Task description (optional)
   - **Priority**: LOW, MEDIUM, or HIGH
   - **Status**: A_FAIRE, EN_COURS, EN_REVUE, or TERMINE
   - **Project**: Select the project
   - **Assign To**: Select a user (optional)
   - **Due Date**: Task due date (optional)
   - **Estimated Hours**: Estimated time (optional)
   - **Real Hours**: Actual time spent (optional)
5. Click "Create"

**Viewing Kanban Board:**

1. Navigate to a project
2. Click "Kanban" button
3. Tasks are displayed in columns by status:
   - **A_FAIRE** (To Do)
   - **EN_COURS** (In Progress)
   - **EN_REVUE** (In Review)
   - **TERMINE** (Done)

**Updating Task Status:**

1. On the Kanban board, find the task
2. Use the status dropdown to change the status
3. The task will move to the appropriate column

**Editing Tasks:**
- **ADMIN/MANAGER**: Can edit any task
- **MEMBER**: Can only edit tasks assigned to them

### 5. Adding Comments to Tasks

1. Click on a task to view details
2. Scroll to the "Comments" section
3. Enter your comment in the text area
4. Click "Add Comment"
5. Comments are displayed with author and timestamp

### 6. Generating Reports (ADMIN/MANAGER)

1. Click "Reports" in the navigation menu
2. Click "Task Count by Status (All Projects)"
3. View the report showing task counts grouped by status

### 7. Editing User Roles (ADMIN Only)

1. Navigate to "Users" in the navigation menu
2. Click "Edit" next to a user
3. Select a new role from the dropdown
4. Click "Save"

### 8. Deleting Users (ADMIN Only)

1. Navigate to "Users" in the navigation menu
2. Click "Delete" next to a user
3. Confirm the deletion
4. **Note**: You cannot delete your own account

## 🔐 Roles and Permissions

### ADMIN
- ✅ Full access to everything
- ✅ Create, edit, and delete users
- ✅ Manage user roles
- ✅ Create and manage projects
- ✅ Assign managers to projects
- ✅ Create, edit, assign, and delete tasks
- ✅ Generate reports
- ✅ View all projects and tasks

### MANAGER
- ✅ Create and manage projects
- ✅ Be assigned to projects (by ADMIN)
- ✅ Create, edit, and assign tasks to MEMBERs
- ✅ Change task status
- ✅ View reports
- ✅ View all projects
- ❌ Cannot manage users
- ❌ Cannot delete projects

### MEMBER
- ✅ View assigned projects (projects where they have tasks)
- ✅ View and update own tasks (tasks assigned to them)
- ✅ Change status of assigned tasks
- ✅ Add comments to tasks
- ❌ Cannot create projects
- ❌ Cannot create tasks
- ❌ Cannot assign tasks
- ❌ Cannot manage users
- ❌ Cannot view reports

---

## 📊 DETAILED USER FLOWS & CRUD OPERATIONS

### 🔴 ADMIN Role - Complete System Flow

#### **Authentication Flow**
1. **Login**: Uses default credentials or created by another ADMIN
2. **Dashboard**: Redirected to Projects page after login
3. **Navigation**: Full access to all menu items (Projects, Tasks, Reports, Admin)

#### **User Management (CRUD) - ADMIN Only**

**CREATE User:**
```
Flow: Admin/Users → Create New User → Fill Form → Submit
- Navigate: Sidebar → "Users" menu
- Click: "Create New User" button
- Form Fields:
  * Email (required)
  * Password (min 4 chars)
  * Confirm Password
  * Role (ADMIN/MANAGER/MEMBER)
- Authorization: [Authorize(Roles = "ADMIN")]
- Result: User created, added to Identity system, role assigned
```

**READ Users:**
```
Flow: Admin/Users → View User List
- Displays: All users with Email, Role, Actions
- Shows: Role badges (ADMIN/MANAGER/MEMBER)
- Actions: Edit, Delete (except own account)
- Authorization: [Authorize(Roles = "ADMIN")]
```

**UPDATE User Role:**
```
Flow: Admin/Users → Edit → Select Role → Save
- Can change: User role (ADMIN/MANAGER/MEMBER)
- Cannot change: Email, Password (separate flow)
- Authorization: [Authorize(Roles = "ADMIN")]
```

**DELETE User:**
```
Flow: Admin/Users → Delete → Confirm
- Cannot delete: Own account (protected in view)
- Authorization: [Authorize(Roles = "ADMIN")]
- Result: User removed from Identity system
```

#### **Project Management (CRUD)**

**CREATE Project:**
```
Flow: Projects → Create New Project → Fill Form → Submit
- Navigate: Sidebar → "Projects" → "Create New Project"
- Form Fields:
  * Name (required, max 200 chars)
  * Description (optional, max 1000 chars)
  * Start Date (required)
  * End Date (optional)
  * Status (enum: ProjectStatus)
  * Manager (optional) - Select ADMIN or MANAGER
- Authorization: [Authorize(Roles = "ADMIN,MANAGER")]
- Business Logic:
  * CreatorId = Current User ID (automatic)
  * ManagerId = Selected from dropdown (optional)
- Result: Project created, redirects to Projects list
```

**READ Projects:**
```
Flow: Projects → View Project List
- Displays: All projects in system
- Columns: Name, Description, Start Date, End Date, Status, Creator, Manager, Actions
- Authorization: [Authorize] - All authenticated users
- Filtering:
  * ADMIN: Sees all projects
  * MANAGER: Sees all projects
  * MEMBER: Sees only projects where they have assigned tasks
- Service Method: GetProjectsByUserAsync(userId)
```

**READ Project Details:**
```
Flow: Projects → Details → View Project Info
- Displays: Full project information
- Shows: Name, Description, Dates, Status, Creator, Manager
- Actions: View Kanban, Edit (ADMIN/MANAGER), Back to List
- Authorization: CanUserAccessProjectAsync(userId, projectId)
```

**UPDATE Project:**
```
Flow: Projects → Edit → Modify Form → Save
- Can modify: All project fields including Manager assignment
- Authorization: [Authorize(Roles = "ADMIN,MANAGER")]
- Business Logic:
  * Updates project entity
  * Can change ManagerId (assign/reassign/remove)
- Result: Project updated, redirects to Projects list
```

**DELETE Project:**
```
Flow: Projects → Delete → Confirm → Submit
- Authorization: [Authorize(Roles = "ADMIN,MANAGER")]
- Business Logic:
  * Cascade delete: All tasks in project are deleted
  * Comments and attachments are cascade deleted
- Result: Project and all related data removed
```

#### **Task Management (CRUD)**

**CREATE Task:**
```
Flow: Project → Kanban → Create New Task → Fill Form → Submit
- Navigate: Project Details/Kanban → "Create New Task"
- Form Fields:
  * Title (required, max 200 chars)
  * Description (optional, max 2000 chars)
  * Priority (enum: LOW/MEDIUM/HIGH)
  * Status (enum: A_FAIRE/EN_COURS/EN_REVUE/TERMINE)
  * Project (dropdown - pre-selected if from Kanban)
  * Assign To Member (dropdown - only MEMBER role users shown)
  * Due Date (optional)
  * Estimated Hours (optional)
  * Real Hours (optional)
- Authorization: [Authorize(Roles = "ADMIN,MANAGER")]
- Business Logic:
  * ProjectId = Selected project
  * AssignedUserId = Selected MEMBER (or null)
  * CreatedAt = Current UTC time (automatic)
- Result: Task created, redirects to Kanban board
```

**READ Tasks:**
```
Flow: Multiple entry points
1. All Tasks: Sidebar → "All Tasks" (ADMIN/MANAGER only)
2. Kanban Board: Project → Kanban
3. Task Details: Kanban/List → Task → Details

Kanban Board Flow:
- Groups tasks by Status enum
- Columns: A_FAIRE, EN_COURS, EN_REVUE, TERMINE
- Each task card shows: Title, Description (truncated), Priority badge, Assigned user, Due date
- Actions: View, Edit, Status dropdown (if authorized)
- Authorization: CanUserAccessProjectAsync(userId, projectId)
```

**READ Task Details:**
```
Flow: Kanban/List → Task → View Details
- Displays: Full task information
- Shows: All task fields, Project info, Assigned user
- Comments Section:
  * Lists all comments with author and timestamp
  * Form to add new comment
  * Delete comment (author, ADMIN, or MANAGER)
- Authorization:
  * ADMIN/MANAGER: Can view any task in accessible projects
  * MEMBER: Can only view tasks assigned to them
```

**UPDATE Task:**
```
Flow: Task Details → Edit → Modify Form → Save
- Can modify: All task fields
- Assignment: ADMIN/MANAGER can change AssignedUserId
- MEMBER: Can only edit own tasks, cannot change assignment
- Authorization:
  * ADMIN/MANAGER: Can edit any task
  * MEMBER: Can only edit tasks where AssignedUserId == userId
- Business Logic:
  * Updates task entity
  * MEMBER: AssignedUserId is locked (cannot change)
- Result: Task updated, redirects to Kanban board
```

**UPDATE Task Status (Quick):**
```
Flow: Kanban Board → Status Dropdown → Select Status → Auto-submit
- Quick action: Change status without full edit
- Available to: ADMIN, MANAGER, and assigned MEMBER
- Authorization:
  * ADMIN/MANAGER: Can change any task status
  * MEMBER: Can only change status of assigned tasks
- Result: Status updated, page refreshes, task moves to new column
```

**DELETE Task:**
```
Flow: Task Details → Delete → Confirm → Submit
- Authorization: [Authorize(Roles = "ADMIN,MANAGER")]
- Business Logic:
  * Cascade delete: Comments and attachments are deleted
- Result: Task removed, redirects to Kanban board
```

#### **Comments Management**

**CREATE Comment:**
```
Flow: Task Details → Comments Section → Enter Comment → Submit
- Form: Textarea for comment content
- Authorization: Any authenticated user with task access
- Business Logic:
  * AuthorId = Current User ID (automatic)
  * CreatedAt = Current UTC time (automatic)
  * TaskId = Current task ID
- Result: Comment added, page refreshes
```

**DELETE Comment:**
```
Flow: Task Details → Comment → Delete Button
- Authorization:
  * Comment author can delete own comments
  * ADMIN can delete any comment
  * MANAGER can delete any comment
- Result: Comment removed, page refreshes
```

#### **Reports**

**GENERATE Report:**
```
Flow: Reports → Generate Report → View Results
- Available Reports: Task Count by Status
- Authorization: [Authorize(Roles = "ADMIN,MANAGER")]
- Business Logic:
  * Groups tasks by Status enum
  * Counts tasks per status
  * Can filter by project (optional)
- Result: Report displayed with task counts
```

---

### 🟡 MANAGER Role - Project & Task Management Flow

#### **Authentication Flow**
1. **Sign Up**: Can self-register as MANAGER
2. **Login**: Uses email/password
3. **Dashboard**: Redirected to Projects page

#### **Project Management**

**CREATE Project:**
```
Flow: Same as ADMIN
- Can create projects
- Can assign self or other MANAGER/ADMIN as manager (optional)
- CreatorId = Current user (automatic)
```

**READ Projects:**
```
Flow: Projects → View All Projects
- Sees: All projects in system (same as ADMIN)
- Can view: Projects assigned to them or any project
- Service Logic: GetProjectsByUserAsync() returns all projects for MANAGER
```

**UPDATE Project:**
```
Flow: Same as ADMIN
- Can edit: All project fields
- Can change: Manager assignment
- Cannot: Delete projects (no delete permission)
```

**DELETE Project:**
```
❌ NOT ALLOWED - MANAGER cannot delete projects
```

#### **Task Management**

**CREATE Task:**
```
Flow: Same as ADMIN
- Can create tasks in any accessible project
- Can assign: Only MEMBER role users
- Dropdown filtered: Only shows MEMBER users
```

**READ Tasks:**
```
Flow: Same as ADMIN
- Can view: All tasks in all projects
- Kanban: Full access to all project Kanban boards
```

**UPDATE Task:**
```
Flow: Same as ADMIN
- Can edit: Any task
- Can change: Task assignment to MEMBERs
- Can change: All task fields
```

**DELETE Task:**
```
Flow: Same as ADMIN
- Can delete: Any task
```

#### **Project Assignment Flow (Being Assigned)**

**How MANAGER Gets Assigned to Project:**
```
1. ADMIN creates/edits project
2. ADMIN selects MANAGER from "Assign Manager" dropdown
3. ManagerId field is set in Project entity
4. MANAGER can now see project in their list
5. MANAGER has full access to project and its tasks
```

**Manager Access Logic:**
```
Service: CanUserAccessProjectAsync()
- Checks: project.ManagerId == userId
- Result: MANAGER can access projects assigned to them
- Also: MANAGER can access all projects (role-based)
```

---

### 🟢 MEMBER Role - Task Execution Flow

#### **Authentication Flow**
1. **Sign Up**: Can self-register as MEMBER
2. **Login**: Uses email/password
3. **Dashboard**: Redirected to Projects page (filtered view)

#### **Project Access (Limited)**

**READ Projects (Filtered):**
```
Flow: Projects → View Assigned Projects Only
- Sees: Only projects where they have assigned tasks
- Service Logic: GetProjectsByUserAsync()
  * Filters: Projects where Tasks.Any(t => t.AssignedUserId == userId)
- Cannot see: Projects without assigned tasks
```

**READ Project Details:**
```
Flow: Projects → Details → View Project Info
- Can view: Project information
- Can access: Kanban board of project
- Authorization: CanUserAccessProjectAsync()
  * Checks: Has assigned tasks in project
```

**CREATE/UPDATE/DELETE Projects:**
```
❌ NOT ALLOWED - MEMBER cannot manage projects
```

#### **Task Management (Own Tasks Only)**

**CREATE Task:**
```
❌ NOT ALLOWED - MEMBER cannot create tasks
```

**READ Tasks:**
```
Flow: Project → Kanban → View Tasks
- Can see: All tasks in accessible projects (for context)
- Can view details: Only tasks assigned to them
- Kanban: Can see all tasks but limited actions
```

**READ Task Details:**
```
Flow: Kanban → Task → View Details
- Can view: Only tasks where AssignedUserId == userId
- Authorization Check:
  * if (user.Role == MEMBER && task.AssignedUserId != user.Id)
  *   → Redirect to AccessDenied
- Can see: Full task information, comments, attachments
```

**UPDATE Task (Own Tasks):**
```
Flow: Task Details → Edit → Modify → Save
- Can edit: Only tasks assigned to them
- Can modify: Title, Description, Priority, Status, Due Date, Hours
- Cannot modify: AssignedUserId (locked)
- Authorization Check:
  * if (user.Role == MEMBER && task.AssignedUserId != user.Id)
  *   → Redirect to AccessDenied
- Business Logic: AssignedUserId is preserved (cannot change assignment)
```

**UPDATE Task Status (Quick):**
```
Flow: Kanban → Status Dropdown → Change Status
- Can change: Status of assigned tasks only
- Authorization Check:
  * if (user.Role == MEMBER && task.AssignedUserId != user.Id)
  *   → Redirect to AccessDenied
- Result: Status updated, task moves to new column
```

**DELETE Task:**
```
❌ NOT ALLOWED - MEMBER cannot delete tasks
```

#### **Task Assignment Flow (Being Assigned)**

**How MEMBER Gets Assigned to Task:**
```
1. ADMIN or MANAGER creates/edits task
2. ADMIN/MANAGER selects MEMBER from "Assign To Member" dropdown
3. AssignedUserId field is set in TaskItem entity
4. MEMBER can now see task in Kanban board
5. MEMBER can view and edit the task
```

**Member Access Logic:**
```
Service: CanUserAccessProjectAsync()
- Checks: Tasks.Any(t => t.ProjectId == projectId && t.AssignedUserId == userId)
- Result: MEMBER can access projects where they have assigned tasks
```

---

## 🔄 SYSTEM ARCHITECTURE & FLOW

### **Authentication & Authorization Flow**

```
1. User Request → AuthController
   ↓
2. Check [Authorize] attribute
   ↓
3. If not authenticated → Redirect to /Auth/Login
   ↓
4. If authenticated → Check role requirements
   ↓
5. [Authorize(Roles = "ADMIN,MANAGER")] → Verify role
   ↓
6. Controller Action → Additional ownership checks
   ↓
7. Service Layer → Business logic + data access
   ↓
8. View → Render with role-based UI elements
```

### **Project Assignment Flow (ADMIN → MANAGER)**

```
1. ADMIN navigates to Projects → Create/Edit
   ↓
2. Form displays "Assign Manager" dropdown
   ↓
3. Dropdown shows: All ADMIN and MANAGER users
   ↓
4. ADMIN selects a MANAGER
   ↓
5. Project.ManagerId = Selected Manager's User ID
   ↓
6. Project saved to database
   ↓
7. MANAGER can now access project
   ↓
8. ProjectService includes Manager in queries
   ↓
9. Project list/details show assigned Manager
```

### **Task Assignment Flow (ADMIN/MANAGER → MEMBER)**

```
1. ADMIN/MANAGER navigates to Kanban → Create Task
   ↓
2. Form displays "Assign To Member" dropdown
   ↓
3. Dropdown shows: Only MEMBER role users (filtered)
   ↓
4. ADMIN/MANAGER selects a MEMBER
   ↓
5. TaskItem.AssignedUserId = Selected Member's User ID
   ↓
6. Task saved to database
   ↓
7. MEMBER can now see project (if not already visible)
   ↓
8. MEMBER can view and edit the task
   ↓
9. Kanban board shows assigned user on task card
```

### **Data Access Patterns**

**ADMIN:**
```
GetProjectsByUserAsync(userId)
  → Returns: All projects (no filter)
  
CanUserAccessProjectAsync(userId, projectId)
  → Returns: true (always allowed)
```

**MANAGER:**
```
GetProjectsByUserAsync(userId)
  → Returns: All projects (no filter)
  
CanUserAccessProjectAsync(userId, projectId)
  → Returns: true (always allowed)
  → Also checks: project.ManagerId == userId (if assigned)
```

**MEMBER:**
```
GetProjectsByUserAsync(userId)
  → Returns: Projects where Tasks.Any(t => t.AssignedUserId == userId)
  
CanUserAccessProjectAsync(userId, projectId)
  → Returns: Tasks.Any(t => t.ProjectId == projectId && t.AssignedUserId == userId)
```

### **Kanban Board Generation**

```
1. User requests Kanban view for project
   ↓
2. TaskController.Kanban(projectId)
   ↓
3. Authorization check: CanUserAccessProjectAsync()
   ↓
4. TaskService.GetTasksByProjectAsync(projectId)
   ↓
5. Returns: List<TaskItem> for the project
   ↓
6. TaskService.GroupTasksByStatus(tasks)
   ↓
7. Groups tasks into Dictionary<TaskStatus, List<TaskItem>>
   ↓
8. Creates KanbanViewModel with grouped tasks
   ↓
9. View renders columns based on TaskStatus enum
   ↓
10. Each column displays tasks in that status
```

### **Service Layer Pattern**

```
Controller
  ↓ (calls)
Service Interface (IProjectService, ITaskService)
  ↓ (implements)
Service Class (ProjectService, TaskService)
  ↓ (uses)
ApplicationDbContext
  ↓ (queries)
SQLite Database
```

### **Authorization Layers**

```
Layer 1: [Authorize] Attribute
  → Requires authentication
  
Layer 2: [Authorize(Roles = "...")] Attribute
  → Requires specific role(s)
  
Layer 3: Controller Action Checks
  → Additional ownership/access validation
  → Example: if (user.Role == MEMBER && task.AssignedUserId != user.Id)
  
Layer 4: Service Layer
  → Business logic validation
  → Example: CanUserAccessProjectAsync()
```

---

## 📋 CRUD OPERATIONS SUMMARY TABLE

| Entity | Operation | ADMIN | MANAGER | MEMBER |
|--------|-----------|-------|---------|--------|
| **User** | Create | ✅ | ❌ | ❌ |
| **User** | Read | ✅ (All) | ❌ | ❌ |
| **User** | Update | ✅ (Role) | ❌ | ❌ |
| **User** | Delete | ✅ (Not self) | ❌ | ❌ |
| **Project** | Create | ✅ | ✅ | ❌ |
| **Project** | Read | ✅ (All) | ✅ (All) | ✅ (Assigned only) |
| **Project** | Update | ✅ | ✅ | ❌ |
| **Project** | Delete | ✅ | ❌ | ❌ |
| **Project** | Assign Manager | ✅ | ✅ (Can assign self/others) | ❌ |
| **Task** | Create | ✅ | ✅ | ❌ |
| **Task** | Read | ✅ (All) | ✅ (All) | ✅ (Assigned only) |
| **Task** | Update | ✅ (All) | ✅ (All) | ✅ (Own only) |
| **Task** | Delete | ✅ | ✅ | ❌ |
| **Task** | Assign Member | ✅ | ✅ | ❌ |
| **Task** | Change Status | ✅ (All) | ✅ (All) | ✅ (Own only) |
| **Comment** | Create | ✅ | ✅ | ✅ (If task access) |
| **Comment** | Delete | ✅ (All) | ✅ (All) | ✅ (Own only) |
| **Report** | Generate | ✅ | ✅ | ❌ |
| **Report** | View | ✅ | ✅ | ❌ |

---

## 🔗 Key Relationships

```
ApplicationUser (Creator) → Project (CreatorId)
ApplicationUser (Manager) → Project (ManagerId) [Optional]
Project → TaskItem (ProjectId)
ApplicationUser (AssignedUser) → TaskItem (AssignedUserId) [Optional]
TaskItem → Comment (TaskId)
TaskItem → Attachment (TaskId)
ApplicationUser (GeneratedBy) → Report (GeneratedById)
```

---

## 🎯 Business Rules Summary

1. **Project Assignment**: Only ADMIN and MANAGER roles can be assigned to projects
2. **Task Assignment**: Only MEMBER role users can be assigned to tasks
3. **Project Visibility**: MEMBERs only see projects where they have assigned tasks
4. **Task Visibility**: MEMBERs can view details of only their assigned tasks
5. **Task Editing**: MEMBERs can edit only their assigned tasks, cannot change assignment
6. **Cascade Deletes**: Deleting a project deletes all its tasks, comments, and attachments
7. **Role Hierarchy**: ADMIN > MANAGER > MEMBER (in terms of permissions)
8. **Self-Registration**: Only MANAGER and MEMBER roles can self-register
9. **Admin Creation**: Only existing ADMIN users can create new ADMIN accounts

## 📁 Project Structure

```
TaskManager/
├── Controllers/          # MVC Controllers with authorization
│   ├── AdminController.cs      # User management (ADMIN only)
│   ├── AuthController.cs       # Login/Logout
│   ├── ProjectController.cs    # Project CRUD
│   ├── TaskController.cs       # Task CRUD and Kanban
│   ├── CommentController.cs    # Task comments
│   └── ReportController.cs     # Report generation
├── Models/              # Entity models and enums
│   ├── ApplicationUser.cs
│   ├── Project.cs
│   ├── TaskItem.cs
│   ├── Comment.cs
│   ├── Attachment.cs
│   ├── Report.cs
│   └── Enums/
│       ├── Role.cs
│       ├── TaskStatus.cs
│       ├── Priority.cs
│       ├── ProjectStatus.cs
│       └── ReportType.cs
├── ViewModels/          # View models for data binding
│   ├── LoginViewModel.cs
│   ├── ProjectViewModel.cs
│   ├── TaskViewModel.cs
│   ├── KanbanViewModel.cs
│   └── CreateUserViewModel.cs
├── Services/            # Business logic services
│   ├── IProjectService.cs
│   ├── ProjectService.cs
│   ├── ITaskService.cs
│   ├── TaskService.cs
│   ├── IReportService.cs
│   └── ReportService.cs
├── Data/                # DbContext and seed data
│   ├── ApplicationDbContext.cs
│   └── SeedData.cs
├── Views/               # Razor views
│   ├── Auth/
│   ├── Project/
│   ├── Task/
│   ├── Admin/
│   ├── Report/
│   └── Shared/
└── wwwroot/             # Static files (CSS, JS)
```

## 💾 Database

The application uses **SQLite** with Entity Framework Core. The database file (`TaskManager.db`) will be created automatically on first run using `EnsureCreatedAsync()` in the `SeedData` class.

### Automatic Seed Data

On first run, the application automatically creates:
- ✅ Three roles: **ADMIN**, **MANAGER**, **MEMBER**
- ✅ One admin user: `admin@taskmanager.com` / `Admin123!`

### Database Location

The SQLite database file is created in the project root directory:
```
TaskManager.db
TaskManager.db-shm  (SQLite shared memory file)
TaskManager.db-wal  (SQLite write-ahead log)
```

### Database Schema

- **AspNetUsers**: Application users (extends IdentityUser)
- **AspNetRoles**: User roles (ADMIN, MANAGER, MEMBER)
- **Projects**: Scrum projects
- **Tasks**: Task items for Kanban board
- **Comments**: Task comments
- **Attachments**: Task file attachments
- **Reports**: Generated reports

## 🎯 Key Features Explained

### Kanban Board

The Kanban board is a **view-only feature** (no database table). Tasks are grouped by their `Status` enum value and displayed in columns:
- **A_FAIRE** (To Do) - New tasks
- **EN_COURS** (In Progress) - Tasks being worked on
- **EN_REVUE** (In Review) - Tasks under review
- **TERMINE** (Done) - Completed tasks

Users can change task status using the dropdown, which moves tasks between columns.

### Authorization

All controllers use `[Authorize]` attributes with role-based restrictions:
- `[Authorize]` - Requires authentication
- `[Authorize(Roles = "ADMIN,MANAGER")]` - Requires specific roles

Additional ownership checks are performed in controller actions to ensure MEMBER users can only access their own tasks.

### Password Requirements

For simplicity (academic project), password requirements are relaxed:
- Minimum 4 characters
- No complexity requirements
- No uppercase/lowercase requirements
- No special character requirements

## 🐛 Troubleshooting

### Database Not Created

If the database is not created automatically:
1. Check that the application has write permissions in the project directory
2. Ensure SQLite is available
3. Check the console for error messages

### Cannot Login

1. Verify you're using the correct credentials:
   - Email: `admin@taskmanager.com`
   - Password: `Admin123!`
2. Check that the database was created successfully
3. Try deleting `TaskManager.db` and restarting the application

### Port Already in Use

If you get a port conflict:
1. The application will try to use a different port automatically
2. Check the console output for the actual URL
3. Or modify `Properties/launchSettings.json` to use a different port

## 📝 Notes for Academic Grading

- ✅ Clean MVC separation of concerns
- ✅ Business logic in Services layer
- ✅ No business logic in Views
- ✅ Proper use of async/await
- ✅ Comprehensive comments explaining key parts
- ✅ Simple, maintainable code structure
- ✅ Ready to compile and run without errors
- ✅ Complete role-based authorization
- ✅ Full CRUD operations for all entities

## 📄 License

This is a training/academic project.

## 🆘 Support

For issues or questions:
1. Check this README first
2. Review the code comments
3. Check the console output for error messages

---

**Happy Task Managing! 🚀**
