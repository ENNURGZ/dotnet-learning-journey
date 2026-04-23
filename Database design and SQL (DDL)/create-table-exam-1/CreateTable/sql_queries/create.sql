CREATE TABLE Brand (
    BrandId INTEGER PRIMARY KEY,
    Name VARCHAR UNIQUE NOT NULL
);

CREATE TABLE Manufacturer (
    ManufacturerId INTEGER PRIMARY KEY,
    Name VARCHAR UNIQUE NOT NULL
);

CREATE TABLE ScreenResolution (
    ScreenResolutionId INTEGER PRIMARY KEY,
    Width INTEGER NOT NULL CHECK (Width>0 and Width<10000),
    Height INTEGER NOT NULL CHECK (Height>0 and Height<10000)
);

CREATE TABLE MatrixType (
    MatrixTypeId INTEGER PRIMARY KEY,
    Name VARCHAR UNIQUE NOT NULL
);

CREATE TABLE Owner (
    OwnerId INTEGER PRIMARY KEY,
    FirstName VARCHAR NOT NULL,
    LastName VARCHAR NOT NULL
);

CREATE TABLE Position (
    PositionId INTEGER PRIMARY KEY,
    Name VARCHAR UNIQUE NOT NULL
);

CREATE TABLE OrderState (
    OrderStateId INTEGER PRIMARY KEY,
    Name VARCHAR UNIQUE NOT NULL
);

CREATE TABLE Employee (
    EmployeeId INTEGER PRIMARY KEY,
    FirstName VARCHAR NOT NULL,
    LastName VARCHAR NOT NULL,
    PositionId INTEGER NOT NULL,
    FOREIGN KEY (PositionId) REFERENCES Position(PositionId)
);

CREATE TABLE PhoneModel (
    PhoneModelId INTEGER PRIMARY KEY,
    Name VARCHAR NOT NULL,
    RamSize INTEGER NOT NULL CHECK (RamSize>0 and RamSize<10000),
    RomSize INTEGER NOT NULL CHECK (RomSize>0 and RomSize<100000),
    BrandId INTEGER NOT NULL,
    ManufacturerId INTEGER NOT NULL,
    ScreenResolutionId INTEGER NOT NULL,
    MatrixTypeId INTEGER NOT NULL,
    FOREIGN KEY (BrandId) REFERENCES Brand(BrandId),
    FOREIGN KEY (ManufacturerId) REFERENCES Manufacturer(ManufacturerId),
    FOREIGN KEY (ScreenResolutionId) REFERENCES ScreenResolution(ScreenResolutionId),
    FOREIGN KEY (MatrixTypeId) REFERENCES MatrixType(MatrixTypeId)
);

CREATE TABLE Smartphone (
    SmartphoneId INTEGER PRIMARY KEY,
    PhoneModelId INTEGER NOT NULL,
    OwnerId INTEGER NOT NULL,
    ManufactureYear INTEGER NOT NULL CHECK (ManufactureYear>1990 and ManufactureYear<2100),
    FOREIGN KEY (PhoneModelId) REFERENCES PhoneModel(PhoneModelId),
    FOREIGN KEY (OwnerId) REFERENCES Owner(OwnerId)
);

CREATE TABLE SmartphoneImei (
    SmartphoneImeiId INTEGER PRIMARY KEY,
    SmartphoneId INTEGER NOT NULL,
    ImeiNumber VARCHAR UNIQUE NOT NULL,
    FOREIGN KEY (SmartphoneId) REFERENCES Smartphone(SmartphoneId)
);

CREATE TABLE OwnerPhone (
    OwnerPhoneId INTEGER PRIMARY KEY,
    OwnerId INTEGER NOT NULL,
    PhoneNumber VARCHAR NOT NULL,
    FOREIGN KEY (OwnerId) REFERENCES Owner(OwnerId)
);

CREATE TABLE Receipt (
    ReceiptId INTEGER PRIMARY KEY,
    SmartphoneId INTEGER NOT NULL,
    EmployeeId INTEGER NOT NULL,
    MalfunctionDescription VARCHAR NOT NULL,
    FOREIGN KEY (SmartphoneId) REFERENCES Smartphone(SmartphoneId),
    FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId)
);

CREATE TABLE Repair (
    RepairId INTEGER PRIMARY KEY,
    ReceiptId INTEGER NOT NULL,
    EmployeeId INTEGER NOT NULL,
    Description VARCHAR NOT NULL,
    FOREIGN KEY (ReceiptId) REFERENCES Receipt(ReceiptId),
    FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId)
);

CREATE TABLE RepairInvoice (
    RepairInvoiceId INTEGER PRIMARY KEY,
    RepairId INTEGER NOT NULL,
    AmountToPay DECIMAL NOT NULL CHECK (AmountToPay>=0 and AmountToPay<1000000),
    FOREIGN KEY (RepairId) REFERENCES Repair(RepairId)
);

CREATE TABLE RepairState (
    RepairStateId INTEGER PRIMARY KEY,
    RepairId INTEGER NOT NULL,
    OrderStateId INTEGER NOT NULL,
    FOREIGN KEY (RepairId) REFERENCES Repair(RepairId),
    FOREIGN KEY (OrderStateId) REFERENCES OrderState(OrderStateId)
);