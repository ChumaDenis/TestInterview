CREATE TABLE Employee (
      ID INT PRIMARY KEY,
      Name VARCHAR(100),
      ManagerID INT NULL,
      Enable BIT,
      FOREIGN KEY (ManagerID) REFERENCES Employee(ID)
);
