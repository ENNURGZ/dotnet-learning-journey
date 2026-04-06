CREATE TABLE Position (
    PositionId INTEGER PRIMARY KEY CHECK (PositionId > 0),
    PositionName VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Employee (
    EmployeeId INTEGER PRIMARY KEY CHECK (EmployeeId > 0),
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE ProjectStatus (
    StatusId INTEGER PRIMARY KEY CHECK (StatusId > 0),
    StatusName VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Project (
    ProjectId INTEGER PRIMARY KEY CHECK (ProjectId > 0),
    ProjectName VARCHAR(100) NOT NULL,
    CreationDate DATE NOT NULL,
    ClosureDate DATE,
    StatusId INTEGER NOT NULL CHECK (StatusId > 0) REFERENCES ProjectStatus(StatusId)
);

CREATE TABLE ProjectAssignment (
    AssignmentId INTEGER PRIMARY KEY CHECK (AssignmentId > 0),
    EmployeeId INTEGER NOT NULL CHECK (EmployeeId > 0) REFERENCES Employee(EmployeeId),
    ProjectId INTEGER NOT NULL CHECK (ProjectId > 0) REFERENCES Project(ProjectId),
    PositionId INTEGER NOT NULL CHECK (PositionId > 0) REFERENCES Position(PositionId)
);

CREATE TABLE TaskStatus (
    StatusId INTEGER PRIMARY KEY CHECK (StatusId > 0),
    StatusName VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Task (
    TaskId INTEGER PRIMARY KEY CHECK (TaskId > 0),
    ProjectId INTEGER NOT NULL CHECK (ProjectId > 0) REFERENCES Project(ProjectId),
    EmployeeId INTEGER NOT NULL CHECK (EmployeeId > 0) REFERENCES Employee(EmployeeId),
    TaskDescription VARCHAR(255) NOT NULL,
    Deadline DATE NOT NULL
);

CREATE TABLE TaskUpdateHistory (
    UpdateId INTEGER PRIMARY KEY CHECK (UpdateId > 0),
    TaskId INTEGER NOT NULL CHECK (TaskId > 0) REFERENCES Task(TaskId),
    StatusId INTEGER NOT NULL CHECK (StatusId > 0) REFERENCES TaskStatus(StatusId),
    UpdateDate DATE NOT NULL,
    EmployeeId INTEGER NOT NULL CHECK (EmployeeId > 0) REFERENCES Employee(EmployeeId)
);
