﻿/*!40101 SET NAMES utf8mb4 */;
/*!40101 SET FOREIGN_KEY_CHECKS=0 */;

-- Backup created: 2026-06-15 18:05:20

-- Table structure for `roles`

DROP TABLE IF EXISTS `roles`;
CREATE TABLE `roles` (
  `RoleID` int NOT NULL AUTO_INCREMENT,
  `RoleName` varchar(50) NOT NULL,
  PRIMARY KEY (`RoleID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data for `roles`
INSERT INTO `roles` (`RoleID`, `RoleName`) VALUES ('1', 'Администратор');
INSERT INTO `roles` (`RoleID`, `RoleName`) VALUES ('2', 'Менеджер');

-- Table structure for `users`

DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `UserID` int NOT NULL AUTO_INCREMENT,
  `Login` varchar(50) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `FIO` varchar(150) NOT NULL,
  `IDRole` int DEFAULT NULL,
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `Login` (`Login`),
  KEY `IDRole` (`IDRole`),
  CONSTRAINT `users_ibfk_1` FOREIGN KEY (`IDRole`) REFERENCES `roles` (`RoleID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data for `users`
INSERT INTO `users` (`UserID`, `Login`, `Password`, `FIO`, `IDRole`) VALUES ('1', 'admin', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', 'Иванов Иван Иванович', '1');
INSERT INTO `users` (`UserID`, `Login`, `Password`, `FIO`, `IDRole`) VALUES ('2', 'manager', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', 'Петров Петр Петрович', '2');
INSERT INTO `users` (`UserID`, `Login`, `Password`, `FIO`, `IDRole`) VALUES ('3', 'manager2', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', 'Сидорова Анна Сергеевна', '2');

-- Table structure for `categories`

DROP TABLE IF EXISTS `categories`;
CREATE TABLE `categories` (
  `CategoriesID` int NOT NULL AUTO_INCREMENT,
  `Categorie` varchar(100) NOT NULL,
  PRIMARY KEY (`CategoriesID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data for `categories`
INSERT INTO `categories` (`CategoriesID`, `Categorie`) VALUES ('1', 'Приключенческие');
INSERT INTO `categories` (`CategoriesID`, `Categorie`) VALUES ('2', 'Детективные');
INSERT INTO `categories` (`CategoriesID`, `Categorie`) VALUES ('3', 'Фэнтези');
INSERT INTO `categories` (`CategoriesID`, `Categorie`) VALUES ('4', 'Научная фантастика');

-- Table structure for `difficultylevels`

DROP TABLE IF EXISTS `difficultylevels`;
CREATE TABLE `difficultylevels` (
  `DifficultyID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`DifficultyID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data for `difficultylevels`
INSERT INTO `difficultylevels` (`DifficultyID`, `Name`) VALUES ('1', 'Легкий');
INSERT INTO `difficultylevels` (`DifficultyID`, `Name`) VALUES ('2', 'Средний');
INSERT INTO `difficultylevels` (`DifficultyID`, `Name`) VALUES ('3', 'Сложный');

-- Table structure for `statuses`

DROP TABLE IF EXISTS `statuses`;
CREATE TABLE `statuses` (
  `StatusID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`StatusID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data for `statuses`
INSERT INTO `statuses` (`StatusID`, `Name`) VALUES ('1', 'Новый');
INSERT INTO `statuses` (`StatusID`, `Name`) VALUES ('2', 'Выполнен');
INSERT INTO `statuses` (`StatusID`, `Name`) VALUES ('3', 'Отменен');

-- Table structure for `services`

DROP TABLE IF EXISTS `services`;
CREATE TABLE `services` (
  `Article` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `Description` text,
  `Price` decimal(10,2) NOT NULL,
  `Time` int DEFAULT NULL,
  `DayOfTheWeek` int DEFAULT NULL,
  `Picture` longblob,
  `MaxPeople` int DEFAULT NULL,
  `ISLevel` int DEFAULT NULL,
  `IDCategory` int DEFAULT NULL,
  PRIMARY KEY (`Article`),
  KEY `ISLevel` (`ISLevel`),
  KEY `IDCategory` (`IDCategory`),
  CONSTRAINT `services_ibfk_1` FOREIGN KEY (`ISLevel`) REFERENCES `difficultylevels` (`DifficultyID`),
  CONSTRAINT `services_ibfk_2` FOREIGN KEY (`IDCategory`) REFERENCES `categories` (`CategoriesID`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data for `services`
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('1', 'Побег из тюрьмы', 'Спланируйте идеальный побег', '2500,00', '60', '30', '5', '2', '1');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('2', 'Убийство в отеле', 'Раскройте загадочное убийство', '3000,00', '75', '30', '6', '3', '2');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('3', 'Волшебный лес', 'Найдите магический артефакт', '2000,00', '45', '30', '6', '1', '3');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('4', 'Ограбление банка', 'Совершите идеальное ограбление', '3200,00', '80', '30', '5', '3', '1');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('5', 'Секрет пирамиды', 'Разгадайте тайны пирамиды', '2800,00', '70', '30', '4', '2', '1');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('6', 'Космическая станция', 'Спасите станцию от катастрофы', '3300,00', '85', '30', '6', '3', '4');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('7', 'Сокровища пиратов', 'Найдите клад на острове', '2200,00', '50', '30', '5', '1', '1');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('8', 'Шпионская миссия', 'Выполните секретное задание', '2900,00', '70', '30', '4', '2', '2');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('9', 'Магическая академия', 'Станьте студентом магии', '2400,00', '55', '30', '6', '1', '3');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('10', 'Лаборатория будущего', 'Создайте изобретение', '3100,00', '75', '30', '4', '3', '4');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('11', 'Поиск Атлантиды', 'Найдите затерянный город', '2600,00', '65', '30', '5', '2', '1');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('12', 'Дело Шерлока Холмса', 'Помогите детективу', '2700,00', '60', '30', '4', '2', '2');

-- Table structure for `orders`

DROP TABLE IF EXISTS `orders`;
CREATE TABLE `orders` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `ServiceID` int NOT NULL,
  `ServiceName` varchar(200) NOT NULL,
  `StartTime` datetime NOT NULL,
  `EndTime` datetime NOT NULL,
  `ClientName` varchar(150) NOT NULL DEFAULT '',
  `ClientPhone` varchar(20) NOT NULL DEFAULT '',
  `StatusID` int NOT NULL DEFAULT '1',
  `UserID` int DEFAULT NULL,
  `ParticipantsCount` int DEFAULT '1',
  `MaxPeople` int DEFAULT '6',
  `TotalPrice` decimal(10,2) DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT '1',
  `DateOfAdmission` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `ServiceID` (`ServiceID`),
  KEY `StatusID` (`StatusID`),
  KEY `UserID` (`UserID`),
  CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`ServiceID`) REFERENCES `services` (`Article`),
  CONSTRAINT `orders_ibfk_2` FOREIGN KEY (`StatusID`) REFERENCES `statuses` (`StatusID`),
  CONSTRAINT `orders_ibfk_3` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=151 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data for `orders`
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('101', '1', 'Побег из тюрьмы', '19.05.2026 10:00:00', '19.05.2026 11:00:00', 'Александров Максим', '+79161234567', '2', NULL, '3', '5', '2500,00', 'True', '18.05.2026 14:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('102', '2', 'Убийство в отеле', '19.05.2026 12:00:00', '19.05.2026 13:15:00', 'Кузнецова Елена', '+79169876543', '2', NULL, '4', '6', '3000,00', 'True', '17.05.2026 10:15:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('103', '3', 'Волшебный лес', '20.05.2026 11:00:00', '20.05.2026 11:45:00', 'Дмитриев Сергей', '+79261234567', '2', NULL, '5', '6', '2000,00', 'True', '19.05.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('104', '4', 'Ограбление банка', '20.05.2026 14:00:00', '20.05.2026 15:20:00', 'Васильева Ольга', '+79269876543', '2', NULL, '4', '5', '3200,00', 'True', '18.05.2026 16:45:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('105', '5', 'Секрет пирамиды', '21.05.2026 10:00:00', '21.05.2026 11:10:00', 'Николаев Павел', '+79031234567', '2', NULL, '3', '4', '2800,00', 'True', '20.05.2026 11:20:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('106', '6', 'Космическая станция', '21.05.2026 15:00:00', '21.05.2026 16:25:00', 'Морозова Татьяна', '+79039876543', '2', NULL, '5', '6', '3300,00', 'True', '19.05.2026 8:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('107', '7', 'Сокровища пиратов', '22.05.2026 11:00:00', '22.05.2026 11:50:00', 'Борисов Андрей', '+79171234567', '2', NULL, '4', '5', '2200,00', 'True', '21.05.2026 13:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('108', '8', 'Шпионская миссия', '22.05.2026 16:00:00', '22.05.2026 17:10:00', 'Григорьева Мария', '+79179876543', '1', NULL, '3', '4', '2900,00', 'True', '22.05.2026 10:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('109', '9', 'Магическая академия', '23.05.2026 10:00:00', '23.05.2026 10:55:00', 'Соколов Игорь', '+79041234567', '2', NULL, '5', '6', '2400,00', 'True', '22.05.2026 15:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('110', '10', 'Лаборатория будущего', '23.05.2026 13:00:00', '23.05.2026 14:15:00', 'Федорова Алина', '+79049876543', '2', NULL, '3', '4', '3100,00', 'True', '21.05.2026 9:45:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('111', '12', 'Дело Шерлока Холмса', '24.05.2026 11:00:00', '24.05.2026 12:00:00', 'Антонова Светлана', '+79189876543', '2', NULL, '3', '4', '2700,00', 'True', '23.05.2026 14:20:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('112', '1', 'Побег из тюрьмы', '24.05.2026 14:00:00', '24.05.2026 15:00:00', 'Тихонов Виктор', '+79051234567', '1', NULL, '4', '5', '2500,00', 'True', '24.05.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('113', '2', 'Убийство в отеле', '26.05.2026 10:00:00', '26.05.2026 11:15:00', 'Романова Ирина', '+79059876543', '2', NULL, '5', '6', '3000,00', 'True', '25.05.2026 11:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('114', '3', 'Волшебный лес', '26.05.2026 13:00:00', '26.05.2026 13:45:00', 'Белов Артем', '+79191234567', '2', NULL, '4', '6', '2000,00', 'True', '24.05.2026 16:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('115', '4', 'Ограбление банка', '27.05.2026 15:00:00', '27.05.2026 16:20:00', 'Жукова Наталья', '+79199876543', '2', NULL, '3', '5', '3200,00', 'True', '26.05.2026 10:15:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('116', '5', 'Секрет пирамиды', '27.05.2026 17:00:00', '27.05.2026 18:10:00', 'Медведев Олег', '+79061234567', '3', NULL, '2', '4', '2800,00', 'False', '25.05.2026 8:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('117', '6', 'Космическая станция', '29.05.2026 11:00:00', '29.05.2026 12:25:00', 'Орлова Дарья', '+79069876543', '2', NULL, '6', '6', '3300,00', 'True', '28.05.2026 14:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('118', '7', 'Сокровища пиратов', '29.05.2026 14:00:00', '29.05.2026 14:50:00', 'Петухов Алексей', '+79201234567', '2', NULL, '5', '5', '2200,00', 'True', '28.05.2026 16:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('119', '8', 'Шпионская миссия', '30.05.2026 10:00:00', '30.05.2026 11:10:00', 'Степанова Екатерина', '+79209876543', '2', NULL, '4', '4', '2900,00', 'True', '29.05.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('120', '9', 'Магическая академия', '30.05.2026 12:30:00', '30.05.2026 13:25:00', 'Захаров Михаил', '+79071234567', '2', NULL, '6', '6', '2400,00', 'True', '28.05.2026 18:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('121', '10', 'Лаборатория будущего', '01.06.2026 10:00:00', '01.06.2026 11:15:00', 'Яковлева Марина', '+79079876543', '1', NULL, '3', '4', '3100,00', 'True', '30.05.2026 10:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('122', '11', 'Поиск Атлантиды', '01.06.2026 14:00:00', '01.06.2026 15:05:00', 'Громов Владимир', '+79161234567', '2', NULL, '5', '5', '2600,00', 'True', '31.05.2026 15:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('123', '12', 'Дело Шерлока Холмса', '03.06.2026 11:00:00', '03.06.2026 12:00:00', 'Полякова Людмила', '+79169876543', '2', NULL, '4', '4', '2700,00', 'True', '31.05.2026 11:20:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('124', '1', 'Побег из тюрьмы', '03.06.2026 15:00:00', '03.06.2026 16:00:00', 'Щербаков Константин', '+79261234567', '2', NULL, '5', '5', '2500,00', 'True', '02.06.2026 9:45:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('125', '2', 'Убийство в отеле', '04.06.2026 10:00:00', '04.06.2026 11:15:00', 'Фомина Валерия', '+79269876543', '1', NULL, '3', '6', '3000,00', 'True', '03.06.2026 12:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('126', '3', 'Волшебный лес', '04.06.2026 13:00:00', '04.06.2026 13:45:00', 'Давыдов Руслан', '+79031234567', '2', NULL, '4', '6', '2000,00', 'True', '03.06.2026 16:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('127', '4', 'Ограбление банка', '06.06.2026 10:00:00', '06.06.2026 11:20:00', 'Никифорова Анастасия', '+79039876543', '2', NULL, '5', '5', '3200,00', 'True', '03.06.2026 14:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('128', '5', 'Секрет пирамиды', '06.06.2026 12:00:00', '06.06.2026 13:10:00', 'Воронцов Дмитрий', '+79171234567', '2', NULL, '3', '4', '2800,00', 'True', '05.06.2026 10:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('129', '6', 'Космическая станция', '06.06.2026 15:00:00', '06.06.2026 16:25:00', 'Лебедева Кристина', '+79179876543', '2', NULL, '4', '6', '3300,00', 'True', '04.06.2026 17:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('130', '7', 'Сокровища пиратов', '07.06.2026 11:00:00', '07.06.2026 11:50:00', 'Терехов Станислав', '+79041234567', '2', NULL, '5', '5', '2200,00', 'True', '05.06.2026 14:45:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('131', '8', 'Шпионская миссия', '07.06.2026 14:00:00', '07.06.2026 15:10:00', 'Соболева Елизавета', '+79049876543', '2', NULL, '3', '4', '2900,00', 'True', '06.06.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('132', '9', 'Магическая академия', '09.06.2026 10:00:00', '09.06.2026 10:55:00', 'Калашников Артур', '+79181234567', '1', NULL, '5', '6', '2400,00', 'True', '07.06.2026 10:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('133', '10', 'Лаборатория будущего', '09.06.2026 13:00:00', '09.06.2026 14:15:00', 'Гусева Полина', '+79189876543', '2', NULL, '4', '4', '3100,00', 'True', '08.06.2026 15:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('134', '11', 'Поиск Атлантиды', '10.06.2026 11:00:00', '10.06.2026 12:05:00', 'Комаров Григорий', '+79051234567', '2', NULL, '4', '5', '2600,00', 'True', '08.06.2026 11:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('135', '12', 'Дело Шерлока Холмса', '10.06.2026 15:00:00', '10.06.2026 16:00:00', 'Афанасьева Юлия', '+79059876543', '2', NULL, '3', '4', '2700,00', 'True', '09.06.2026 16:45:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('136', '1', 'Побег из тюрьмы', '12.06.2026 10:00:00', '12.06.2026 11:00:00', 'Лавров Борис', '+79191234567', '3', NULL, '2', '5', '2500,00', 'False', '09.06.2026 9:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('137', '2', 'Убийство в отеле', '12.06.2026 12:30:00', '12.06.2026 13:45:00', 'Зайцева Тамара', '+79199876543', '2', NULL, '5', '6', '3000,00', 'True', '11.06.2026 14:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('138', '3', 'Волшебный лес', '13.06.2026 10:00:00', '13.06.2026 10:45:00', 'Савельев Николай', '+79061234567', '2', NULL, '6', '6', '2000,00', 'True', '11.06.2026 17:20:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('139', '4', 'Ограбление банка', '13.06.2026 12:00:00', '13.06.2026 13:20:00', 'Игнатова Дарья', '+79069876543', '2', NULL, '4', '5', '3200,00', 'True', '12.06.2026 10:15:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('140', '5', 'Секрет пирамиды', '13.06.2026 14:30:00', '13.06.2026 15:40:00', 'Тарасов Илья', '+79201234567', '1', NULL, '4', '4', '2800,00', 'True', '13.06.2026 8:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('141', '6', 'Космическая станция', '14.06.2026 11:00:00', '14.06.2026 12:25:00', 'Хохлова Виктория', '+79209876543', '2', NULL, '5', '6', '3300,00', 'True', '12.06.2026 15:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('142', '7', 'Сокровища пиратов', '14.06.2026 14:00:00', '14.06.2026 14:50:00', 'Мельников Сергей', '+79071234567', '2', NULL, '3', '5', '2200,00', 'True', '13.06.2026 13:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('143', '8', 'Шпионская миссия', '16.06.2026 10:00:00', '16.06.2026 11:10:00', 'Кудрявцева Инна', '+79079876543', '1', NULL, '4', '4', '2900,00', 'True', '14.06.2026 9:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('144', '9', 'Магическая академия', '16.06.2026 13:00:00', '16.06.2026 13:55:00', 'Горбачев Андрей', '+7 (916) 123-45-67', '3', NULL, '6', '6', '14400,00', 'True', '15.06.2026 11:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('145', '10', 'Лаборатория будущего', '17.06.2026 10:00:00', '17.06.2026 11:15:00', 'Логинова Светлана', '+79169876543', '2', NULL, '3', '4', '3100,00', 'True', '15.06.2026 15:45:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('146', '11', 'Поиск Атлантиды', '17.06.2026 14:00:00', '17.06.2026 15:05:00', 'Филиппов Максим', '+79261234567', '2', NULL, '5', '5', '2600,00', 'True', '16.06.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('147', '12', 'Дело Шерлока Холмса', '18.06.2026 10:00:00', '18.06.2026 11:00:00', 'Артемьева Алена', '+79269876543', '2', NULL, '4', '4', '2700,00', 'True', '16.06.2026 14:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('148', '1', 'Побег из тюрьмы', '18.06.2026 12:30:00', '18.06.2026 13:30:00', 'Максимов Денис', '+79031234567', '2', NULL, '3', '5', '2500,00', 'True', '17.06.2026 16:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('149', '2', 'Убийство в отеле', '19.06.2026 10:00:00', '19.06.2026 11:15:00', 'Копылова Елена', '+79039876543', '1', NULL, '6', '6', '3000,00', 'True', '18.06.2026 8:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('150', '3', 'Волшебный лес', '19.06.2026 15:00:00', '19.06.2026 15:45:00', 'Родионов Петр', '+79171234567', '2', NULL, '4', '6', '2000,00', 'True', '18.06.2026 10:00:00');

/*!40101 SET FOREIGN_KEY_CHECKS=1 */;
