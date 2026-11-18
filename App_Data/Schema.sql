CREATE TABLE Users (
  UserId INTEGER PRIMARY KEY AUTOINCREMENT,
  Username TEXT NOT NULL UNIQUE,
  PasswordHash TEXT NOT NULL,
  FullName TEXT NOT NULL,
  Email TEXT NOT NULL,
  Phone TEXT NULL,
  Address TEXT NULL,
  Role TEXT NOT NULL DEFAULT 'Member',
  Points INT NOT NULL DEFAULT 0,
  CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
)
GO
CREATE TABLE Categories (
  CategoryId INTEGER PRIMARY KEY AUTOINCREMENT,
  Name TEXT NOT NULL,
  Description TEXT NULL
)
GO
CREATE TABLE Products (
  ProductId INTEGER PRIMARY KEY AUTOINCREMENT,
  CategoryId INT NOT NULL REFERENCES Categories(CategoryId),
  Name TEXT NOT NULL,
  Description TEXT NULL,
  Price DECIMAL(10,2) NOT NULL,
  Stock INT NOT NULL DEFAULT 0,
  ImageUrl TEXT NULL,
  CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
)
GO
CREATE TABLE CartItems (
  CartItemId INTEGER PRIMARY KEY AUTOINCREMENT,
  UserId INT NOT NULL REFERENCES Users(UserId) ON DELETE CASCADE,
  ProductId INT NOT NULL REFERENCES Products(ProductId) ON DELETE CASCADE,
  Quantity INT NOT NULL DEFAULT 1
)
GO
CREATE TABLE Wishlist (
  UserId INT NOT NULL REFERENCES Users(UserId) ON DELETE CASCADE,
  ProductId INT NOT NULL REFERENCES Products(ProductId) ON DELETE CASCADE,
  PRIMARY KEY (UserId, ProductId)
)
GO
CREATE TABLE Orders (
  OrderId INTEGER PRIMARY KEY AUTOINCREMENT,
  UserId INT NOT NULL REFERENCES Users(UserId) ON DELETE CASCADE,
  OrderDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  Total DECIMAL(10,2) NOT NULL,
  Discount DECIMAL(10,2) NOT NULL DEFAULT 0,
  Status TEXT NOT NULL DEFAULT 'Pending',
  PaymentMethod TEXT NOT NULL,
  ShippingAddress TEXT NOT NULL
)
GO
CREATE TABLE OrderItems (
  OrderItemId INTEGER PRIMARY KEY AUTOINCREMENT,
  OrderId INT NOT NULL REFERENCES Orders(OrderId) ON DELETE CASCADE,
  ProductId INT NOT NULL REFERENCES Products(ProductId),
  Quantity INT NOT NULL,
  UnitPrice DECIMAL(10,2) NOT NULL
)
GO
CREATE TABLE Ratings (
  RatingId INTEGER PRIMARY KEY AUTOINCREMENT,
  UserId INT NOT NULL REFERENCES Users(UserId) ON DELETE CASCADE,
  ProductId INT NOT NULL REFERENCES Products(ProductId) ON DELETE CASCADE,
  OrderId INT NOT NULL,
  Stars INT NOT NULL CHECK (Stars BETWEEN 1 AND 5),
  Comment TEXT NULL,
  CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
)
GO
CREATE TABLE Feedback (
  FeedbackId INTEGER PRIMARY KEY AUTOINCREMENT,
  Name TEXT NOT NULL,
  Email TEXT NOT NULL,
  Message TEXT NOT NULL,
  CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
)
GO
CREATE TABLE Notifications (
  NotificationId INTEGER PRIMARY KEY AUTOINCREMENT,
  UserId INT NOT NULL REFERENCES Users(UserId) ON DELETE CASCADE,
  Message TEXT NOT NULL,
  CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
)
GO
CREATE TABLE Coupons (
  Code TEXT PRIMARY KEY,
  Percent INT NOT NULL
)
GO
INSERT INTO Coupons VALUES ('WELCOME10', 10), ('FLASH20', 20)
GO
INSERT INTO Categories (Name, Description) VALUES
 ('Skincare', 'Serums, moisturizers and sun care'),
 ('Makeup', 'Face and lip colour'),
 ('Hair & Body', 'Nourishing care for hair and body'),
 ('Fragrance', 'Signature scents')
GO
INSERT INTO Products (CategoryId, Name, Description, Price, Stock, ImageUrl) VALUES
 (1, 'Vitamin C Brightening Serum', 'Antioxidant serum with 15% vitamin C to even skin tone and restore natural radiance. 30ml.', 2490.00, 25, '~/Images/products/serum.jpg'),
 (1, 'Hydra-Boost Moisturizer', 'Lightweight gel-cream with hyaluronic acid for 48-hour hydration. Suits all skin types. 50ml.', 1890.00, 30, '~/Images/products/moisturizer.jpg'),
 (1, 'Daily Mineral Sunscreen SPF 50', 'Broad-spectrum protection with a weightless, non-greasy finish and no white cast. 50ml.', 1590.00, 40, '~/Images/products/sunscreen.jpg'),
 (1, 'Gentle Foaming Cleanser', 'Sulfate-free cleanser that lifts away makeup and impurities without stripping the skin. 150ml.', 1290.00, 18, '~/Images/products/cleanser.jpg'),
 (2, 'Velvet Matte Lipstick', 'A richly pigmented, comfortable matte lip colour in a soft rosewood shade.', 1390.00, 12, '~/Images/products/lipstick.jpg'),
 (2, 'Skin-Tint Foundation', 'Buildable, natural-finish coverage with SPF 20 and a skin-like feel. 30ml.', 2190.00, 3, '~/Images/products/foundation.jpg'),
 (3, 'Argan Repair Hair Oil', 'Nourishing argan and jojoba oil that tames frizz and adds lasting shine. 60ml.', 1490.00, 35, '~/Images/products/hair-oil.jpg'),
 (4, 'Rose Eau de Parfum', 'Notes of Bulgarian rose, peony and white musk. A soft, lasting signature scent. 50ml.', 3990.00, 20, '~/Images/products/perfume.jpg')
GO
CREATE TRIGGER trg_products_restrict BEFORE DELETE ON Products
WHEN EXISTS (SELECT 1 FROM OrderItems WHERE ProductId = OLD.ProductId)
BEGIN SELECT RAISE(ABORT, 'Product has been ordered'); END
GO
CREATE TRIGGER trg_products_cascade AFTER DELETE ON Products
BEGIN
  DELETE FROM CartItems WHERE ProductId = OLD.ProductId;
  DELETE FROM Wishlist WHERE ProductId = OLD.ProductId;
  DELETE FROM Ratings WHERE ProductId = OLD.ProductId;
END
GO
CREATE TRIGGER trg_categories_restrict BEFORE DELETE ON Categories
WHEN EXISTS (SELECT 1 FROM Products WHERE CategoryId = OLD.CategoryId)
BEGIN SELECT RAISE(ABORT, 'Category still has products'); END
GO
CREATE TRIGGER trg_users_cascade AFTER DELETE ON Users
BEGIN
  DELETE FROM CartItems WHERE UserId = OLD.UserId;
  DELETE FROM Wishlist WHERE UserId = OLD.UserId;
  DELETE FROM Ratings WHERE UserId = OLD.UserId;
  DELETE FROM Notifications WHERE UserId = OLD.UserId;
  DELETE FROM OrderItems WHERE OrderId IN (SELECT OrderId FROM Orders WHERE UserId = OLD.UserId);
  DELETE FROM Orders WHERE UserId = OLD.UserId;
END
GO
