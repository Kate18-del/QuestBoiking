﻿/*!40101 SET NAMES utf8mb4 */;
/*!40101 SET FOREIGN_KEY_CHECKS=0 */;

-- Backup created: 2026-06-08 13:52:55

-- Table structure for `categories`

DROP TABLE IF EXISTS `categories`;
CREATE TABLE `categories` (
  `CategoriesID` int NOT NULL AUTO_INCREMENT,
  `Categorie` varchar(100) NOT NULL,
  PRIMARY KEY (`CategoriesID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data for `categories`
INSERT INTO `categories` (`CategoriesID`, `Categorie`) VALUES ('1', 'Приключенческие');
INSERT INTO `categories` (`CategoriesID`, `Categorie`) VALUES ('2', 'Детективные');
INSERT INTO `categories` (`CategoriesID`, `Categorie`) VALUES ('3', 'Фэнтези');
INSERT INTO `categories` (`CategoriesID`, `Categorie`) VALUES ('4', 'Научная фантастика');

-- Table structure for `clients`

DROP TABLE IF EXISTS `clients`;
CREATE TABLE `clients` (
  `ClientID` int NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(50) NOT NULL,
  `LastName` varchar(50) NOT NULL,
  `Surname` varchar(50) DEFAULT NULL,
  `PhoneNumber` varchar(20) NOT NULL,
  `Age` int DEFAULT NULL,
  PRIMARY KEY (`ClientID`),
  UNIQUE KEY `PhoneNumber` (`PhoneNumber`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data for `clients`
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('1', 'Алексей', 'Смирнов', 'Владимирович', '+79161111111', '25');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('2', 'Мария', 'Кузнецова', 'Игоревна', '+79162222222', '30');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('3', 'Дмитрий', 'Попов', 'Александрович', '+79163333333', '22');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('4', 'Елена', 'Васильева', 'Дмитриевна', '+79164444444', '28');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('5', 'Артем', 'Петров', 'Сергеевич', '+79165555555', '35');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('6', 'Ольга', 'Соколова', 'Андреевна', '+79166666666', '26');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('7', 'Сергей', 'Михайлов', 'Викторович', '+79167777777', '32');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('8', 'Анна', 'Новикова', 'Павловна', '+79168888888', '29');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('9', 'Владимир', 'Федоров', 'Олегович', '+79169999999', '31');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('10', 'Ирина', 'Морозова', 'Николаевна', '+79161010101', '27');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('11', 'Андрей', 'Волков', 'Иванович', '+79161110101', '24');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('12', 'Наталья', 'Алексеева', 'Сергеевна', '+79161212121', '33');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('13', 'Павел', 'Лебедев', 'Дмитриевич', '+79161313131', '26');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('14', 'Юлия', 'Семенова', 'Анатольевна', '+79161414141', '30');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('15', 'Кирилл', 'Егоров', 'Валерьевич', '+79161515151', '28');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('16', 'Татьяна', 'Павлова', 'Владимировна', '+79161616161', '25');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('17', 'Максим', 'Козлов', 'Алексеевич', '+79161717171', '29');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('18', 'Екатерина', 'Степанова', 'Игоревна', '+79161818181', '31');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('19', 'Александр', 'Николаев', 'Петрович', '+79161919191', '34');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('20', 'Светлана', 'Орлова', 'Викторовна', '+79162020202', '26');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('21', 'Иван', 'Андреев', 'Сергеевич', '+79162121212', '23');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('22', 'Людмила', 'Макарова', 'Дмитриевна', '+79162222223', '32');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('23', 'Григорий', 'Никитин', 'Андреевич', '+79162323232', '30');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('24', 'Виктория', 'Захарова', 'Павловна', '+79162424242', '27');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('25', 'Роман', 'Зайцев', 'Владимирович', '+79162525252', '29');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('26', 'Маргарита', 'Соловьева', 'Александровна', '+79162626262', '28');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('27', 'Станислав', 'Борисов', 'Игоревич', '+79162727272', '31');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('28', 'Алина', 'Яковлева', 'Викторовна', '+79162828282', '24');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('29', 'Константин', 'Григорьев', 'Олегович', '+79162929292', '33');
INSERT INTO `clients` (`ClientID`, `FirstName`, `LastName`, `Surname`, `PhoneNumber`, `Age`) VALUES ('30', 'Валерия', 'Романова', 'Сергеевна', '+79163030303', '26');

-- Table structure for `difficultylevels`

DROP TABLE IF EXISTS `difficultylevels`;
CREATE TABLE `difficultylevels` (
  `DifficultyID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`DifficultyID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data for `difficultylevels`
INSERT INTO `difficultylevels` (`DifficultyID`, `Name`) VALUES ('1', 'Легкий');
INSERT INTO `difficultylevels` (`DifficultyID`, `Name`) VALUES ('2', 'Средний');
INSERT INTO `difficultylevels` (`DifficultyID`, `Name`) VALUES ('3', 'Сложный');

-- Table structure for `orders`

DROP TABLE IF EXISTS `orders`;
CREATE TABLE `orders` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `ClientID` int NOT NULL,
  `Article` int NOT NULL,
  `DateOfAdmission` datetime NOT NULL,
  `DueDate` datetime DEFAULT NULL,
  `StatusID` int NOT NULL,
  `UserID` int DEFAULT NULL,
  `ParticipantsCount` int DEFAULT NULL,
  `TotalPrice` decimal(10,2) DEFAULT NULL,
  `ScheduleID` int DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `ClientID` (`ClientID`),
  KEY `Article` (`Article`),
  KEY `StatusID` (`StatusID`),
  KEY `UserID` (`UserID`),
  KEY `ScheduleID` (`ScheduleID`),
  CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`ClientID`) REFERENCES `clients` (`ClientID`),
  CONSTRAINT `orders_ibfk_2` FOREIGN KEY (`Article`) REFERENCES `services` (`Article`),
  CONSTRAINT `orders_ibfk_3` FOREIGN KEY (`StatusID`) REFERENCES `statuses` (`StatusID`),
  CONSTRAINT `orders_ibfk_4` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`),
  CONSTRAINT `orders_ibfk_5` FOREIGN KEY (`ScheduleID`) REFERENCES `schedule` (`ScheduleID`)
) ENGINE=InnoDB AUTO_INCREMENT=202 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data for `orders`
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('152', '1', '1', '27.04.2026 9:30:00', '27.04.2026 10:00:00', '2', '8', '4', '10000,00', '1');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('153', '2', '2', '27.04.2026 10:00:00', '27.04.2026 11:30:00', '2', '8', '3', '9000,00', '2');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('154', '3', '3', '27.04.2026 11:00:00', '27.04.2026 13:00:00', '1', '8', '1', '2000,00', '3');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('155', '4', '4', '27.04.2026 12:00:00', '27.04.2026 14:30:00', '1', '7', '2', '6400,00', '4');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('156', '5', '5', '27.04.2026 13:00:00', '27.04.2026 16:00:00', '1', '8', '2', '5600,00', '5');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('157', '6', '6', '27.04.2026 14:00:00', '27.04.2026 18:00:00', '2', '7', '4', '13200,00', '6');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('158', '7', '7', '28.04.2026 9:00:00', '28.04.2026 10:00:00', '2', '8', '1', '2200,00', '7');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('159', '8', '8', '28.04.2026 10:00:00', '28.04.2026 11:30:00', '1', '7', '3', '8700,00', '8');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('160', '9', '9', '28.04.2026 11:00:00', '28.04.2026 13:00:00', '2', '8', '3', '7200,00', '9');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('161', '10', '10', '28.04.2026 12:00:00', '28.04.2026 14:30:00', '1', '7', '1', '3100,00', '10');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('162', '11', '11', '28.04.2026 13:00:00', '28.04.2026 16:00:00', '2', '8', '2', '5200,00', '11');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('163', '12', '12', '28.04.2026 14:00:00', '28.04.2026 18:00:00', '1', '7', '4', '10800,00', '12');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('164', '13', '1', '28.04.2026 15:00:00', '28.04.2026 19:30:00', '2', '8', '3', '7500,00', '13');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('165', '14', '2', '29.04.2026 9:00:00', '29.04.2026 10:00:00', '2', '8', '2', '6000,00', '14');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('166', '15', '3', '29.04.2026 10:00:00', '29.04.2026 11:30:00', '1', '7', '4', '8000,00', '15');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('167', '16', '4', '29.04.2026 11:00:00', '29.04.2026 13:00:00', '2', '8', '1', '3200,00', '16');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('168', '17', '5', '29.04.2026 12:00:00', '29.04.2026 14:30:00', '1', '7', '3', '8400,00', '17');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('169', '18', '6', '29.04.2026 13:00:00', '29.04.2026 16:00:00', '2', '8', '5', '16500,00', '18');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('170', '19', '7', '29.04.2026 14:00:00', '29.04.2026 18:00:00', '3', '7', '2', '4400,00', '19');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('171', '20', '8', '30.04.2026 9:00:00', '30.04.2026 10:00:00', '2', '8', '1', '2900,00', '20');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('172', '21', '9', '30.04.2026 10:00:00', '30.04.2026 11:30:00', '1', '7', '3', '7200,00', '21');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('173', '22', '10', '30.04.2026 11:00:00', '30.04.2026 13:00:00', '2', '8', '2', '6200,00', '22');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('174', '23', '11', '30.04.2026 12:00:00', '30.04.2026 14:30:00', '1', '7', '2', '5200,00', '23');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('175', '24', '12', '30.04.2026 13:00:00', '30.04.2026 16:00:00', '2', '8', '1', '2700,00', '24');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('176', '25', '1', '30.04.2026 14:00:00', '30.04.2026 17:30:00', '1', '7', '4', '10000,00', '25');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('177', '26', '2', '30.04.2026 15:00:00', '30.04.2026 19:00:00', '2', '8', '2', '6000,00', '26');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('178', '27', '3', '01.05.2026 9:00:00', '01.05.2026 10:00:00', '2', '8', '3', '6000,00', '27');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('179', '28', '4', '01.05.2026 10:00:00', '01.05.2026 11:30:00', '1', '7', '1', '3200,00', '28');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('180', '29', '5', '01.05.2026 11:00:00', '01.05.2026 13:00:00', '2', '8', '2', '5600,00', '29');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('181', '30', '6', '01.05.2026 12:00:00', '01.05.2026 14:30:00', '1', '7', '5', '16500,00', '30');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('182', '1', '7', '01.05.2026 13:00:00', '01.05.2026 16:00:00', '3', '8', '3', '6600,00', '31');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('183', '2', '8', '01.05.2026 14:00:00', '01.05.2026 17:30:00', '2', '7', '2', '5800,00', '32');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('184', '3', '9', '01.05.2026 15:00:00', '01.05.2026 19:00:00', '1', '8', '4', '9600,00', '33');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('185', '4', '10', '02.05.2026 9:00:00', '02.05.2026 10:00:00', '2', '7', '1', '3100,00', '34');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('186', '5', '11', '02.05.2026 9:30:00', '02.05.2026 11:30:00', '1', '8', '3', '7800,00', '35');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('187', '6', '12', '02.05.2026 10:00:00', '02.05.2026 13:00:00', '2', '7', '2', '5400,00', '36');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('188', '7', '1', '02.05.2026 11:00:00', '02.05.2026 14:30:00', '1', '8', '5', '12500,00', '37');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('189', '8', '2', '02.05.2026 12:00:00', '02.05.2026 16:00:00', '2', '7', '4', '12000,00', '38');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('190', '9', '3', '02.05.2026 13:00:00', '02.05.2026 17:30:00', '1', '8', '2', '4000,00', '39');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('191', '10', '4', '02.05.2026 14:00:00', '02.05.2026 18:30:00', '2', '7', '3', '9600,00', '40');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('192', '11', '5', '02.05.2026 15:00:00', '02.05.2026 20:00:00', '1', '8', '1', '2800,00', '41');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('193', '12', '6', '03.05.2026 9:00:00', '03.05.2026 10:00:00', '2', '7', '4', '13200,00', '42');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('194', '13', '7', '03.05.2026 10:00:00', '03.05.2026 11:30:00', '1', '8', '2', '4400,00', '43');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('195', '14', '8', '03.05.2026 11:00:00', '03.05.2026 13:00:00', '2', '7', '1', '2900,00', '44');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('196', '15', '9', '03.05.2026 12:00:00', '03.05.2026 14:30:00', '3', '8', '3', '7200,00', '45');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('197', '16', '10', '03.05.2026 13:00:00', '03.05.2026 16:00:00', '2', '7', '3', '9300,00', '46');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('198', '17', '11', '03.05.2026 14:00:00', '03.05.2026 17:30:00', '1', '8', '2', '5200,00', '47');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('199', '18', '12', '03.05.2026 15:00:00', '03.05.2026 19:00:00', '2', '7', '1', '2700,00', '48');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('200', '19', '1', '03.05.2026 9:30:00', '03.05.2026 10:00:00', '1', '8', '2', '5000,00', '42');
INSERT INTO `orders` (`ID`, `ClientID`, `Article`, `DateOfAdmission`, `DueDate`, `StatusID`, `UserID`, `ParticipantsCount`, `TotalPrice`, `ScheduleID`) VALUES ('201', '20', '2', '03.05.2026 10:30:00', '03.05.2026 11:30:00', '2', '7', '2', '6000,00', '14');

-- Table structure for `schedule`

DROP TABLE IF EXISTS `schedule`;
CREATE TABLE `schedule` (
  `ScheduleID` int NOT NULL AUTO_INCREMENT,
  `ServiceID` int NOT NULL,
  `StartTime` datetime NOT NULL,
  `EndTime` datetime NOT NULL,
  `MaxSlots` int DEFAULT '6',
  `BookedSlots` int DEFAULT '0',
  `IsActive` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`ScheduleID`),
  KEY `ServiceID` (`ServiceID`),
  CONSTRAINT `schedule_ibfk_1` FOREIGN KEY (`ServiceID`) REFERENCES `services` (`Article`)
) ENGINE=InnoDB AUTO_INCREMENT=106 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data for `schedule`
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('1', '1', '27.04.2026 10:00:00', '27.04.2026 11:00:00', '6', '2', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('2', '2', '27.04.2026 11:30:00', '27.04.2026 12:45:00', '6', '3', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('3', '3', '27.04.2026 13:00:00', '27.04.2026 13:45:00', '6', '1', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('4', '4', '27.04.2026 14:30:00', '27.04.2026 15:50:00', '5', '2', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('5', '5', '27.04.2026 16:00:00', '27.04.2026 17:10:00', '4', '2', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('6', '6', '27.04.2026 18:00:00', '27.04.2026 19:25:00', '6', '4', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('7', '7', '28.04.2026 10:00:00', '28.04.2026 10:50:00', '5', '1', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('8', '8', '28.04.2026 11:30:00', '28.04.2026 12:40:00', '4', '3', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('9', '9', '28.04.2026 13:00:00', '28.04.2026 13:55:00', '6', '3', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('10', '10', '28.04.2026 14:30:00', '28.04.2026 15:45:00', '4', '1', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('11', '11', '28.04.2026 16:00:00', '28.04.2026 17:05:00', '5', '2', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('12', '12', '28.04.2026 18:00:00', '28.04.2026 19:00:00', '4', '4', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('13', '1', '28.04.2026 19:30:00', '28.04.2026 20:30:00', '6', '3', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('14', '2', '29.04.2026 10:00:00', '29.04.2026 11:15:00', '6', '2', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('15', '3', '29.04.2026 11:30:00', '29.04.2026 12:15:00', '6', '4', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('16', '4', '29.04.2026 13:00:00', '29.04.2026 14:20:00', '5', '1', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('17', '5', '29.04.2026 14:30:00', '29.04.2026 15:40:00', '4', '3', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('18', '6', '29.04.2026 16:00:00', '29.04.2026 17:25:00', '6', '5', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('19', '7', '29.04.2026 18:00:00', '29.04.2026 18:50:00', '5', '2', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('20', '8', '30.04.2026 10:00:00', '30.04.2026 11:10:00', '4', '1', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('21', '9', '30.04.2026 11:30:00', '30.04.2026 12:25:00', '6', '3', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('22', '10', '30.04.2026 13:00:00', '30.04.2026 14:15:00', '4', '2', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('23', '11', '30.04.2026 14:30:00', '30.04.2026 15:35:00', '5', '2', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('24', '12', '30.04.2026 16:00:00', '30.04.2026 17:00:00', '4', '1', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('25', '1', '30.04.2026 17:30:00', '30.04.2026 18:30:00', '6', '4', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('26', '2', '30.04.2026 19:00:00', '30.04.2026 20:15:00', '6', '2', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('27', '3', '01.05.2026 10:00:00', '01.05.2026 10:45:00', '6', '3', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('28', '4', '01.05.2026 11:30:00', '01.05.2026 12:50:00', '5', '1', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('29', '5', '01.05.2026 13:00:00', '01.05.2026 14:10:00', '4', '2', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('30', '6', '01.05.2026 14:30:00', '01.05.2026 15:55:00', '6', '5', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('31', '7', '01.05.2026 16:00:00', '01.05.2026 16:50:00', '5', '3', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('32', '8', '01.05.2026 17:30:00', '01.05.2026 18:40:00', '4', '2', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('33', '9', '01.05.2026 19:00:00', '01.05.2026 19:55:00', '6', '4', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('34', '10', '02.05.2026 10:00:00', '02.05.2026 11:15:00', '4', '1', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('35', '11', '02.05.2026 11:30:00', '02.05.2026 12:35:00', '5', '3', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('36', '12', '02.05.2026 13:00:00', '02.05.2026 14:00:00', '4', '2', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('37', '1', '02.05.2026 14:30:00', '02.05.2026 15:30:00', '6', '5', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('38', '2', '02.05.2026 16:00:00', '02.05.2026 17:15:00', '6', '4', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('39', '3', '02.05.2026 17:30:00', '02.05.2026 18:15:00', '6', '2', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('40', '4', '02.05.2026 18:30:00', '02.05.2026 19:50:00', '5', '3', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('41', '5', '02.05.2026 20:00:00', '02.05.2026 21:10:00', '4', '1', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('42', '6', '03.05.2026 10:00:00', '03.05.2026 11:25:00', '6', '6', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('43', '7', '03.05.2026 11:30:00', '03.05.2026 12:20:00', '5', '2', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('44', '8', '03.05.2026 13:00:00', '03.05.2026 14:10:00', '4', '1', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('45', '9', '03.05.2026 14:30:00', '03.05.2026 15:25:00', '6', '3', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('46', '10', '03.05.2026 16:00:00', '03.05.2026 17:15:00', '4', '3', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('47', '11', '03.05.2026 17:30:00', '03.05.2026 18:35:00', '5', '2', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('48', '12', '03.05.2026 19:00:00', '03.05.2026 20:00:00', '4', '1', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('101', '10', '03.04.2026 16:39:18', '03.04.2026 17:54:18', '4', '0', 'False');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('102', '3', '22.04.2026 16:39:18', '22.04.2026 17:24:18', '6', '0', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('103', '11', '22.04.2026 16:39:18', '22.04.2026 17:44:18', '5', '5', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('104', '8', '22.04.2026 17:00:18', '22.04.2026 18:10:18', '4', '0', 'True');
INSERT INTO `schedule` (`ScheduleID`, `ServiceID`, `StartTime`, `EndTime`, `MaxSlots`, `BookedSlots`, `IsActive`) VALUES ('105', '2', '23.04.2026 17:00:18', '23.04.2026 18:15:18', '6', '0', 'True');

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
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data for `services`
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('1', 'Побег из тюрьмы', 'Спланируйте идеальный побег из средневековой тюрьмы', '2500,00', '60', '30', '5', '2', '1');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('2', 'Убийство в отеле', 'Раскройте загадочное убийство в роскошном отеле', '3000,00', '75', '30', '6', '3', '2');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('3', 'Волшебный лес', 'Найдите магический артефакт в зачарованном лесу', '2000,00', '45', '30', '6', '1', '3');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('4', 'Ограбление банка', 'Совершите идеальное ограбление банка', '3200,00', '80', '30', '5', '3', '1');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('5', 'Секрет пирамиды', 'Разгадайте тайны древней пирамиды', '2800,00', '70', '30', '4', '2', '1');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('6', 'Космическая станция', 'Спасите станцию от катастрофы', '3300,00', '85', '30', '6', '3', '4');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('7', 'Сокровища пиратов', 'Найдите клад на заброшенном острове', '2200,00', '50', '30', '5', '1', '1');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('8', 'Шпионская миссия', 'Выполните секретное задание в посольстве', '2900,00', '70', '30', '4', '2', '2');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('9', 'Магическая академия', 'Станьте студентом школы магии', '2400,00', '55', '30', '6', '1', '3');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('10', 'Лаборатория будущего', 'Создайте революционное изобретение', '3100,00', '75', '30', '4', '3', '4');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('11', 'Поиск Атлантиды', 'Найдите затерянный город под водой', '2600,00', '65', '30', '5', '2', '1');
INSERT INTO `services` (`Article`, `Name`, `Description`, `Price`, `Time`, `DayOfTheWeek`, `MaxPeople`, `ISLevel`, `IDCategory`) VALUES ('12', 'Дело Шерлока Холмса', 'Помогите известному детективу раскрыть дело', '2700,00', '60', '30', '4', '2', '2');

-- Table structure for `statuses`

DROP TABLE IF EXISTS `statuses`;
CREATE TABLE `statuses` (
  `StatusID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`StatusID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data for `statuses`
INSERT INTO `statuses` (`StatusID`, `Name`) VALUES ('1', 'Новый');
INSERT INTO `statuses` (`StatusID`, `Name`) VALUES ('2', 'Выполнен');
INSERT INTO `statuses` (`StatusID`, `Name`) VALUES ('3', 'Отменен');

-- Table structure for `users`

DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `UserID` int NOT NULL AUTO_INCREMENT,
  `Login` varchar(50) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `FIO` varchar(150) NOT NULL,
  `IDRole` int DEFAULT NULL,
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `Login` (`Login`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data for `users`
INSERT INTO `users` (`UserID`, `Login`, `Password`, `FIO`, `IDRole`) VALUES ('6', 'admin', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', 'Иванов Иван Иванович', '1');
INSERT INTO `users` (`UserID`, `Login`, `Password`, `FIO`, `IDRole`) VALUES ('7', 'director', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', 'Петров Петр Петрович', '2');
INSERT INTO `users` (`UserID`, `Login`, `Password`, `FIO`, `IDRole`) VALUES ('8', 'manager', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', 'Сидорова Анна Сергеевна', '3');
INSERT INTO `users` (`UserID`, `Login`, `Password`, `FIO`, `IDRole`) VALUES ('18', 'ser', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', 'Секавыавы апрыва Проаавы', '1');
INSERT INTO `users` (`UserID`, `Login`, `Password`, `FIO`, `IDRole`) VALUES ('19', 'myxa', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', 'Мухин Антон Андреевич', '2');

/*!40101 SET FOREIGN_KEY_CHECKS=1 */;
