-- ============================================
-- 1. РОЛИ
-- ============================================
CREATE TABLE roles (
    RoleID INT NOT NULL AUTO_INCREMENT,
    RoleName VARCHAR(50) NOT NULL,
    PRIMARY KEY (RoleID)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

INSERT INTO roles VALUES 
(1, 'Администратор'),
(2, 'Менеджер');

-- ============================================
-- 2. ПОЛЬЗОВАТЕЛИ
-- ============================================
CREATE TABLE users (
    UserID INT NOT NULL AUTO_INCREMENT,
    Login VARCHAR(50) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    FIO VARCHAR(150) NOT NULL,
    IDRole INT DEFAULT NULL,
    PRIMARY KEY (UserID),
    UNIQUE KEY Login (Login),
    FOREIGN KEY (IDRole) REFERENCES roles (RoleID)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

INSERT INTO users VALUES 
(1, 'admin', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', 'Иванов Иван Иванович', 1),
(2, 'manager', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', 'Петров Петр Петрович', 2),
(3, 'manager2', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', 'Сидорова Анна Сергеевна', 2);

-- ============================================
-- 3. КАТЕГОРИИ
-- ============================================
CREATE TABLE categories (
    CategoriesID INT NOT NULL AUTO_INCREMENT,
    Categorie VARCHAR(100) NOT NULL,
    PRIMARY KEY (CategoriesID)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


-- ============================================
-- 4. УРОВНИ СЛОЖНОСТИ
-- ============================================
CREATE TABLE difficultylevels (
    DifficultyID INT NOT NULL AUTO_INCREMENT,
    Name VARCHAR(50) NOT NULL,
    PRIMARY KEY (DifficultyID)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;



-- ============================================
-- 5. СТАТУСЫ ЗАКАЗОВ
-- ============================================
CREATE TABLE statuses (
    StatusID INT NOT NULL AUTO_INCREMENT,
    Name VARCHAR(50) NOT NULL,
    PRIMARY KEY (StatusID)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


-- ============================================
-- 6. УСЛУГИ (КВЕСТЫ) - пустая
-- ============================================
CREATE TABLE services (
    Article INT NOT NULL AUTO_INCREMENT,
    Name VARCHAR(200) NOT NULL,
    Description TEXT,
    Price DECIMAL(10,2) NOT NULL,
    Time INT DEFAULT NULL,
    DayOfTheWeek INT DEFAULT NULL,
    Picture LONGBLOB DEFAULT NULL,
    MaxPeople INT DEFAULT NULL,
    ISLevel INT DEFAULT NULL,
    IDCategory INT DEFAULT NULL,
    PRIMARY KEY (Article),
    FOREIGN KEY (ISLevel) REFERENCES difficultylevels (DifficultyID),
    FOREIGN KEY (IDCategory) REFERENCES categories (CategoriesID)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================
-- 7. ЗАКАЗЫ (РАСПИСАНИЕ) - пустая
-- ============================================
CREATE TABLE orders (
    ID INT NOT NULL AUTO_INCREMENT,
    ServiceID INT NOT NULL,
    ServiceName VARCHAR(200) NOT NULL,
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NOT NULL,
    ClientName VARCHAR(150) NOT NULL DEFAULT '',
    ClientPhone VARCHAR(20) NOT NULL DEFAULT '',
    StatusID INT NOT NULL DEFAULT 1,
    UserID INT DEFAULT NULL,
    ParticipantsCount INT DEFAULT 1,
    MaxPeople INT DEFAULT 6,
    TotalPrice DECIMAL(10,2) DEFAULT NULL,
    IsActive TINYINT(1) DEFAULT 1,
    DateOfAdmission DATETIME DEFAULT NULL,
    PRIMARY KEY (ID),
    FOREIGN KEY (ServiceID) REFERENCES services (Article),
    FOREIGN KEY (StatusID) REFERENCES statuses (StatusID),
    FOREIGN KEY (UserID) REFERENCES users (UserID)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;