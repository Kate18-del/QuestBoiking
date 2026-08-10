﻿/*!40101 SET NAMES utf8mb4 */;
/*!40101 SET FOREIGN_KEY_CHECKS=0 */;

-- Auto-backup: 12.06.2026 15:06:46

DROP TABLE IF EXISTS `roles`;
CREATE TABLE `roles` (
  `RoleID` int NOT NULL AUTO_INCREMENT,
  `RoleName` varchar(50) NOT NULL,
  PRIMARY KEY (`RoleID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `roles` (`RoleID`, `RoleName`) VALUES ('1', 'Администратор');
INSERT INTO `roles` (`RoleID`, `RoleName`) VALUES ('2', 'Менеджер');
INSERT INTO `roles` (`RoleID`, `RoleName`) VALUES ('3', 'Администратор');
INSERT INTO `roles` (`RoleID`, `RoleName`) VALUES ('4', 'Менеджер');

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
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `users` (`UserID`, `Login`, `Password`, `FIO`, `IDRole`) VALUES ('6', 'admin', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', 'Иванов Иван Иванович', '1');
INSERT INTO `users` (`UserID`, `Login`, `Password`, `FIO`, `IDRole`) VALUES ('7', 'manager', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', 'Петров Петр Петрович', '2');
INSERT INTO `users` (`UserID`, `Login`, `Password`, `FIO`, `IDRole`) VALUES ('8', 'manager2', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', 'Сидорова Анна Сергеевна', '2');

DROP TABLE IF EXISTS `categories`;
CREATE TABLE `categories` (
  `CategoriesID` int NOT NULL AUTO_INCREMENT,
  `Categorie` varchar(100) NOT NULL,
  PRIMARY KEY (`CategoriesID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `categories` (`CategoriesID`, `Categorie`) VALUES ('1', 'Приключенческие');
INSERT INTO `categories` (`CategoriesID`, `Categorie`) VALUES ('2', 'Детективные');
INSERT INTO `categories` (`CategoriesID`, `Categorie`) VALUES ('3', 'Фэнтези');
INSERT INTO `categories` (`CategoriesID`, `Categorie`) VALUES ('4', 'Научная фантастика');

DROP TABLE IF EXISTS `difficultylevels`;
CREATE TABLE `difficultylevels` (
  `DifficultyID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`DifficultyID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `difficultylevels` (`DifficultyID`, `Name`) VALUES ('1', 'Легкий');
INSERT INTO `difficultylevels` (`DifficultyID`, `Name`) VALUES ('2', 'Средний');
INSERT INTO `difficultylevels` (`DifficultyID`, `Name`) VALUES ('3', 'Сложный');

DROP TABLE IF EXISTS `statuses`;
CREATE TABLE `statuses` (
  `StatusID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`StatusID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `statuses` (`StatusID`, `Name`) VALUES ('1', 'Новый');
INSERT INTO `statuses` (`StatusID`, `Name`) VALUES ('2', 'Выполнен');
INSERT INTO `statuses` (`StatusID`, `Name`) VALUES ('3', 'Отменен');

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
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('1', 'Побег из тюрьмы', 'Спланируйте идеальный побег', '2500,00', '60', '30', '5', '2', '1');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('2', 'Убийство в отеле', 'Раскройте загадочное убийство', '3000,00', '75', '30', '6', '3', '2');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('3', 'Волшебный лес', 'Найдите магический артефакт', '2000,00', '45', '30', '6', '1', '3');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('4', 'Ограбление банка', 'Совершите идеальное ограбление', '3200,00', '80', '30', '5', '3', '1');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('5', 'Секрет пирамиды', 'Разгадайте тайны пирамиды', '2800,00', '70', '30', '4', '2', '1');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('6', 'Космическая станция', 'Спасите станцию от катастрофы', '3300,00', '85', '30', '6', '3', '4');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('7', 'Сокровища пиратов', 'Найдите клад на острове', '2200,00', '50', '30', '5', '1', '1');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('8', 'Шпионская миссия', 'Выполните секретное задание', '2900,00', '70', '30', '4', '2', '2');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('9', 'Магическая академия', 'Станьте студентом магии', '2400,00', '55', '30', '6', '1', '3');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('10', 'Лаборатория будущего', 'Создайте изобретение', '3100,00', '75', '30', '9', '3', '4');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('11', 'Поиск Атлантиды', 'Найдите затерянный город', '2600,00', '65', '30', '11', '2', '1');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('12', 'Дело Шерлока Холмса', 'Помогите детективу', '2700,00', '60', '30', '10', '2', '2');

DROP TABLE IF EXISTS `orders`;
CREATE TABLE `orders` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `ServiceID` int NOT NULL,
  `ServiceName` varchar(200) NOT NULL,
  `StartTime` datetime NOT NULL,
  `EndTime` datetime NOT NULL,
  `ClientName` varchar(150) DEFAULT '',
  `ClientPhone` varchar(20) DEFAULT '',
  `StatusID` int DEFAULT '1',
  `UserID` int DEFAULT NULL,
  `ParticipantsCount` int DEFAULT '0',
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
) ENGINE=InnoDB AUTO_INCREMENT=60 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('1', '1', 'Побег из тюрьмы', '25.05.2026 10:00:00', '25.05.2026 11:00:00', 'Смирнов Алексей', '+79161111111', '2', '7', '4', '5', '10000,00', 'True', '25.05.2026 8:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('2', '2', 'Убийство в отеле', '25.05.2026 11:30:00', '25.05.2026 12:45:00', 'Кузнецова Мария', '+79162222222', '2', '7', '3', '6', '9000,00', 'True', '25.05.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('3', '3', 'Волшебный лес', '25.05.2026 13:00:00', '25.05.2026 13:45:00', 'Попов Дмитрий', '+79163333333', '1', '8', '2', '6', '4000,00', 'True', '25.05.2026 10:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('4', '4', 'Ограбление банка', '25.05.2026 14:30:00', '25.05.2026 15:50:00', 'Васильева Елена', '+79164444444', '1', '7', '3', '5', '9600,00', 'True', '25.05.2026 11:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('5', '5', 'Секрет пирамиды', '25.05.2026 16:30:00', '25.05.2026 17:40:00', 'Петров Артем', '+79165555555', '1', '8', '2', '4', '5600,00', 'True', '25.05.2026 12:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('6', '6', 'Космическая станция', '26.05.2026 10:00:00', '26.05.2026 11:25:00', 'Соколова Ольга', '+79166666666', '2', '7', '5', '6', '16500,00', 'True', '26.05.2026 8:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('7', '7', 'Сокровища пиратов', '26.05.2026 11:30:00', '26.05.2026 12:20:00', 'Михайлов Сергей', '+79167777777', '1', '8', '2', '5', '4400,00', 'True', '26.05.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('8', '8', 'Шпионская миссия', '26.05.2026 13:00:00', '26.05.2026 14:10:00', 'Новикова Анна', '+79168888888', '2', '7', '3', '4', '8700,00', 'True', '26.05.2026 10:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('9', '9', 'Магическая академия', '26.05.2026 14:30:00', '26.05.2026 15:25:00', 'Федоров Владимир', '+79169999999', '1', '8', '4', '6', '9600,00', 'True', '26.05.2026 11:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('10', '10', 'Лаборатория будущего', '27.05.2026 10:00:00', '27.05.2026 11:15:00', 'Морозова Ирина', '+79161010101', '2', '7', '2', '4', '6200,00', 'True', '27.05.2026 8:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('11', '11', 'Поиск Атлантиды', '27.05.2026 11:30:00', '27.05.2026 12:35:00', 'Волков Андрей', '+79161110101', '1', '8', '3', '5', '7800,00', 'True', '27.05.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('12', '12', 'Дело Шерлока Холмса', '27.05.2026 13:00:00', '27.05.2026 14:00:00', 'Алексеева Наталья', '+79161212121', '2', '7', '2', '4', '5400,00', 'True', '27.05.2026 10:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('13', '1', 'Побег из тюрьмы', '27.05.2026 14:30:00', '27.05.2026 15:30:00', 'Лебедев Павел', '+79161313131', '1', '8', '4', '5', '10000,00', 'True', '27.05.2026 11:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('14', '2', 'Убийство в отеле', '28.05.2026 10:00:00', '28.05.2026 11:15:00', 'Семенова Юлия', '+79161414141', '2', '7', '5', '6', '15000,00', 'True', '28.05.2026 8:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('15', '3', 'Волшебный лес', '28.05.2026 11:30:00', '28.05.2026 12:15:00', 'Егоров Кирилл', '+79161515151', '1', '8', '6', '6', '12000,00', 'True', '28.05.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('16', '4', 'Ограбление банка', '28.05.2026 13:00:00', '28.05.2026 14:20:00', 'Павлова Татьяна', '+79161616161', '2', '7', '3', '5', '9600,00', 'True', '28.05.2026 10:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('17', '5', 'Секрет пирамиды', '29.05.2026 10:00:00', '29.05.2026 11:10:00', 'Козлов Максим', '+79161717171', '1', '8', '2', '4', '5600,00', 'True', '29.05.2026 8:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('18', '6', 'Космическая станция', '29.05.2026 11:30:00', '29.05.2026 12:55:00', 'Степанова Екатерина', '+79161818181', '2', '7', '4', '6', '13200,00', 'True', '29.05.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('19', '7', 'Сокровища пиратов', '29.05.2026 13:30:00', '29.05.2026 14:20:00', 'Орлова Светлана', '+79162020202', '1', '7', '1', '5', '2200,00', 'True', '29.05.2026 10:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('20', '8', 'Шпионская миссия', '29.05.2026 15:00:00', '29.05.2026 16:10:00', 'Андреев Иван', '+79162121212', '2', '8', '3', '4', '8700,00', 'True', '29.05.2026 11:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('21', '9', 'Магическая академия', '29.05.2026 16:30:00', '29.05.2026 17:25:00', 'Макарова Людмила', '+79162222223', '1', '7', '5', '6', '12000,00', 'True', '29.05.2026 12:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('22', '10', 'Лаборатория будущего', '01.06.2026 10:00:00', '01.06.2026 11:15:00', 'Захарова Виктория', '+79162424242', '2', '8', '3', '4', '9300,00', 'True', '01.06.2026 8:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('23', '11', 'Поиск Атлантиды', '01.06.2026 11:30:00', '01.06.2026 12:35:00', 'Зайцев Роман', '+79162525252', '1', '8', '2', '5', '5200,00', 'True', '01.06.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('24', '12', 'Дело Шерлока Холмса', '01.06.2026 13:00:00', '01.06.2026 14:00:00', 'Соловьева Маргарита', '+79162626262', '2', '7', '1', '4', '2700,00', 'True', '01.06.2026 10:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('25', '1', 'Побег из тюрьмы', '02.06.2026 10:00:00', '02.06.2026 11:00:00', 'Борисов Станислав', '+79162727272', '1', '7', '3', '5', '7500,00', 'True', '02.06.2026 8:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('26', '2', 'Убийство в отеле', '02.06.2026 11:30:00', '02.06.2026 12:45:00', 'Яковлева Алина', '+79162828282', '2', '8', '2', '6', '6000,00', 'True', '02.06.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('27', '3', 'Волшебный лес', '03.06.2026 10:00:00', '03.06.2026 10:45:00', 'Григорьев Константин', '+79162929292', '1', '7', '4', '6', '8000,00', 'True', '03.06.2026 8:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('28', '4', 'Ограбление банка', '03.06.2026 11:30:00', '03.06.2026 12:50:00', 'Романова Валерия', '+79163030303', '2', '8', '5', '5', '16000,00', 'True', '03.06.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('29', '5', 'Секрет пирамиды', '03.06.2026 13:30:00', '03.06.2026 14:40:00', 'Смирнов Алексей', '+79161111111', '1', '7', '2', '4', '5600,00', 'True', '03.06.2026 10:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('30', '6', 'Космическая станция', '04.06.2026 10:00:00', '04.06.2026 11:25:00', 'Кузнецова Мария', '+79162222222', '2', '8', '6', '6', '19800,00', 'True', '04.06.2026 8:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('31', '7', 'Сокровища пиратов', '04.06.2026 11:30:00', '04.06.2026 12:20:00', 'Попов Дмитрий', '+79163333333', '1', '7', '3', '5', '6600,00', 'True', '04.06.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('32', '8', 'Шпионская миссия', '04.06.2026 13:00:00', '04.06.2026 14:10:00', 'Васильева Елена', '+79164444444', '2', '8', '1', '4', '2900,00', 'True', '04.06.2026 10:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('33', '9', 'Магическая академия', '05.06.2026 10:00:00', '05.06.2026 10:55:00', 'Петров Артем', '+79165555555', '1', '7', '4', '6', '9600,00', 'True', '05.06.2026 8:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('34', '10', 'Лаборатория будущего', '05.06.2026 11:30:00', '05.06.2026 12:45:00', 'Соколова Ольга', '+79166666666', '2', '8', '2', '4', '6200,00', 'True', '05.06.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('35', '11', 'Поиск Атлантиды', '05.06.2026 13:00:00', '05.06.2026 14:05:00', 'Михайлов Сергей', '+79167777777', '1', '7', '3', '5', '7800,00', 'True', '05.06.2026 10:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('36', '12', 'Дело Шерлока Холмса', '05.06.2026 14:30:00', '05.06.2026 15:30:00', 'Новикова Анна', '+79168888888', '2', '8', '2', '4', '5400,00', 'True', '05.06.2026 11:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('37', '1', 'Побег из тюрьмы', '08.06.2026 10:00:00', '08.06.2026 11:00:00', 'Федоров Владимир', '+79169999999', '1', '7', '5', '5', '12500,00', 'True', '08.06.2026 8:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('38', '2', 'Убийство в отеле', '08.06.2026 11:30:00', '08.06.2026 12:45:00', 'Морозова Ирина', '+7 (916) 101-01-01', '2', '8', '4', '6', '12000,00', 'True', '08.06.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('39', '3', 'Волшебный лес', '08.06.2026 13:00:00', '08.06.2026 13:45:00', 'Волков Андрей', '+79161110101', '1', '7', '2', '6', '4000,00', 'True', '08.06.2026 10:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('40', '4', 'Ограбление банка', '09.06.2026 10:00:00', '09.06.2026 11:20:00', 'Алексеева Наталья', '+79161212121', '2', '8', '3', '5', '9600,00', 'True', '09.06.2026 8:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('41', '5', 'Секрет пирамиды', '09.06.2026 11:30:00', '09.06.2026 12:40:00', 'Лебедев Павел', '+79161313131', '1', '7', '2', '4', '5600,00', 'True', '09.06.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('42', '6', 'Космическая станция', '09.06.2026 13:00:00', '09.06.2026 14:25:00', 'Семенова Юлия', '+79161414141', '2', '8', '5', '6', '16500,00', 'True', '09.06.2026 10:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('43', '7', 'Сокровища пиратов', '10.06.2026 10:00:00', '10.06.2026 10:50:00', 'Егоров Кирилл', '+79161515151', '1', '7', '4', '5', '8800,00', 'True', '10.06.2026 8:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('44', '8', 'Шпионская миссия', '10.06.2026 11:30:00', '10.06.2026 12:40:00', 'Павлова Татьяна', '+79161616161', '2', '8', '1', '4', '2900,00', 'True', '10.06.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('45', '9', 'Магическая академия', '10.06.2026 13:00:00', '10.06.2026 13:55:00', 'Козлов Максим', '+79161717171', '1', '7', '6', '6', '14400,00', 'True', '10.06.2026 10:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('46', '10', 'Лаборатория будущего', '11.06.2026 10:00:00', '11.06.2026 11:15:00', 'Степанова Екатерина', '+7 (916) 181-81-81', '2', '8', '3', '4', '9300,00', 'True', '11.06.2026 8:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('47', '11', 'Поиск Атлантиды', '11.06.2026 11:30:00', '11.06.2026 12:35:00', 'Орлова Светлана', '+79162020202', '1', '7', '2', '5', '5200,00', 'True', '11.06.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('48', '12', 'Дело Шерлока Холмса', '11.06.2026 13:00:00', '11.06.2026 14:00:00', 'Андреев Иван', '+79162121212', '2', '8', '2', '4', '5400,00', 'True', '11.06.2026 10:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('49', '1', 'Побег из тюрьмы', '15.06.2026 10:00:00', '15.06.2026 11:00:00', 'Макарова Людмила', '+79162222223', '1', '7', '3', '5', '7500,00', 'True', '15.06.2026 8:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('50', '2', 'Убийство в отеле', '15.06.2026 11:30:00', '15.06.2026 12:45:00', 'Захарова Виктория', '+79162424242', '2', '8', '1', '6', '3000,00', 'True', '15.06.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('51', '3', 'Волшебный лес', '15.06.2026 13:00:00', '15.06.2026 13:45:00', 'Зайцев Роман', '+7 (916) 252-52-52', '1', '7', '2', '6', '4000,00', 'True', '15.06.2026 10:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('52', '4', 'Ограбление банка', '16.06.2026 10:00:00', '16.06.2026 11:20:00', 'Соловьева Маргарита', '+79162626262', '2', '8', '3', '5', '9600,00', 'True', '16.06.2026 8:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('53', '5', 'Секрет пирамиды', '16.06.2026 11:30:00', '16.06.2026 12:40:00', 'Борисов Станислав', '+79162727272', '1', '7', '2', '4', '5600,00', 'True', '16.06.2026 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('54', '6', 'Космическая станция', '16.06.2026 13:00:00', '16.06.2026 14:25:00', 'Яковлева Алина', '+79162828282', '2', '8', '5', '6', '16500,00', 'True', '16.06.2026 10:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('56', '12', 'Дело Шерлока Холмса', '11.06.2026 16:47:42', '11.06.2026 17:47:42', 'антон', '+7 (423) 342-32-43', '1', '7', '10', '10', '24300,00', 'True', '08.06.2026 16:47:55');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('57', '3', 'Волшебный лес', '11.06.2026 11:30:16', '11.06.2026 12:15:16', 'Пушков Владимир Алесандрович', '+7 (132) 123-23-12', '1', '7', '5', '6', '10000,00', 'True', '11.06.2026 10:58:59');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('58', '3', 'Волшебный лес', '15.06.2026 14:39:00', '15.06.2026 15:24:00', '', '+7', '1', '7', '6', '6', '12000,00', 'True', '12.06.2026 14:39:32');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('59', '3', 'Волшебный лес', '15.06.2026 17:00:00', '15.06.2026 17:45:00', 'выаыва', '+7 (453) 453-43-53', '1', '7', '1', '6', '2000,00', 'True', '12.06.2026 14:49:53');

/*!40101 SET FOREIGN_KEY_CHECKS=1 */;
