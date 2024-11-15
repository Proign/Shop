CREATE TABLE IF NOT EXISTS Shops (
    Code TEXT PRIMARY KEY,
    Name TEXT NOT NULL,
    Address TEXT
);

CREATE TABLE IF NOT EXISTS Products (
    Name TEXT NOT NULL,
    ShopCode TEXT,
    Price REAL NOT NULL,
    Quantity INTEGER NOT NULL,
    FOREIGN KEY (ShopCode) REFERENCES Shops(Code)
);