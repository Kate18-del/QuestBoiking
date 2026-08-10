﻿/*!40101 SET NAMES utf8mb4 */;
/*!40101 SET FOREIGN_KEY_CHECKS=0 */;

-- Auto-backup: 15.06.2026 16:50:53

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
) ENGINE=InnoDB AUTO_INCREMENT=209 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('158', '1', 'Побег из тюрьмы', '20.05.2025 10:00:00', '20.05.2025 11:00:00', 'Александров Максим', '+79161234567', '2', '6', '3', '5', '2500,00', 'True', '19.05.2025 14:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('159', '2', 'Убийство в отеле', '20.05.2025 12:00:00', '20.05.2025 13:15:00', 'Кузнецова Елена', '+79169876543', '2', '8', '4', '6', '3000,00', 'True', '18.05.2025 10:15:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('160', '3', 'Волшебный лес', '21.05.2025 11:00:00', '21.05.2025 11:45:00', 'Дмитриев Сергей', '+79261234567', '2', '7', '5', '6', '2000,00', 'True', '20.05.2025 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('161', '4', 'Ограбление банка', '21.05.2025 14:00:00', '21.05.2025 15:20:00', 'Васильева Ольга', '+79269876543', '2', '8', '4', '5', '3200,00', 'True', '19.05.2025 16:45:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('162', '5', 'Секрет пирамиды', '22.05.2025 10:00:00', '22.05.2025 11:10:00', 'Николаев Павел', '+79031234567', '2', '7', '3', '4', '2800,00', 'True', '21.05.2025 11:20:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('163', '6', 'Космическая станция', '22.05.2025 15:00:00', '22.05.2025 16:25:00', 'Морозова Татьяна', '+79039876543', '2', '8', '5', '6', '3300,00', 'True', '20.05.2025 8:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('164', '7', 'Сокровища пиратов', '23.05.2025 11:00:00', '23.05.2025 11:50:00', 'Борисов Андрей', '+79171234567', '2', '7', '4', '5', '2200,00', 'True', '22.05.2025 13:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('165', '8', 'Шпионская миссия', '23.05.2025 16:00:00', '23.05.2025 17:10:00', 'Григорьева Мария', '+79179876543', '1', NULL, '3', '4', '2900,00', 'True', '23.05.2025 10:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('166', '9', 'Магическая академия', '24.05.2025 10:00:00', '24.05.2025 10:55:00', 'Соколов Игорь', '+79041234567', '2', '8', '5', '6', '2400,00', 'True', '23.05.2025 15:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('167', '10', 'Лаборатория будущего', '24.05.2025 13:00:00', '24.05.2025 14:15:00', 'Федорова Алина', '+79049876543', '2', '7', '3', '4', '3100,00', 'True', '22.05.2025 9:45:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('168', '11', 'Поиск Атлантиды', '24.05.2025 16:00:00', '24.05.2025 17:05:00', 'Кириллов Денис', '+79181234567', '2', '8', '4', '5', '2600,00', 'True', '24.05.2025 12:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('169', '12', 'Дело Шерлока Холмса', '25.05.2025 11:00:00', '25.05.2025 12:00:00', 'Антонова Светлана', '+79189876543', '2', '7', '3', '4', '2700,00', 'True', '24.05.2025 14:20:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('170', '1', 'Побег из тюрьмы', '25.05.2025 14:00:00', '25.05.2025 15:00:00', 'Тихонов Виктор', '+79051234567', '1', NULL, '4', '5', '2500,00', 'True', '25.05.2025 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('171', '2', 'Убийство в отеле', '27.05.2025 10:00:00', '27.05.2025 11:15:00', 'Романова Ирина', '+79059876543', '2', '8', '5', '6', '3000,00', 'True', '26.05.2025 11:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('172', '3', 'Волшебный лес', '27.05.2025 13:00:00', '27.05.2025 13:45:00', 'Белов Артем', '+79191234567', '2', '7', '4', '6', '2000,00', 'True', '25.05.2025 16:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('173', '4', 'Ограбление банка', '28.05.2025 15:00:00', '28.05.2025 16:20:00', 'Жукова Наталья', '+79199876543', '2', '8', '3', '5', '3200,00', 'True', '27.05.2025 10:15:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('174', '5', 'Секрет пирамиды', '28.05.2025 17:00:00', '28.05.2025 18:10:00', 'Медведев Олег', '+79061234567', '3', '7', '2', '4', '2800,00', 'False', '26.05.2025 8:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('175', '6', 'Космическая станция', '30.05.2025 11:00:00', '30.05.2025 12:25:00', 'Орлова Дарья', '+79069876543', '2', '8', '6', '6', '3300,00', 'True', '29.05.2025 14:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('176', '7', 'Сокровища пиратов', '30.05.2025 14:00:00', '30.05.2025 14:50:00', 'Петухов Алексей', '+79201234567', '2', '7', '5', '5', '2200,00', 'True', '29.05.2025 16:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('177', '8', 'Шпионская миссия', '31.05.2025 10:00:00', '31.05.2025 11:10:00', 'Степанова Екатерина', '+79209876543', '2', '8', '4', '4', '2900,00', 'True', '30.05.2025 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('178', '9', 'Магическая академия', '31.05.2025 12:30:00', '31.05.2025 13:25:00', 'Захаров Михаил', '+79071234567', '2', '7', '6', '6', '2400,00', 'True', '29.05.2025 18:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('179', '10', 'Лаборатория будущего', '31.05.2025 15:00:00', '31.05.2025 16:15:00', 'Яковлева Марина', '+79079876543', '1', NULL, '3', '4', '3100,00', 'True', '31.05.2025 10:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('180', '11', 'Поиск Атлантиды', '02.06.2025 10:00:00', '02.06.2025 11:05:00', 'Громов Владимир', '+79161234567', '2', '8', '5', '5', '2600,00', 'True', '01.06.2025 15:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('181', '12', 'Дело Шерлока Холмса', '02.06.2025 14:00:00', '02.06.2025 15:00:00', 'Полякова Людмила', '+79169876543', '2', '7', '4', '4', '2700,00', 'True', '01.06.2025 11:20:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('182', '1', 'Побег из тюрьмы', '04.06.2025 11:00:00', '04.06.2025 12:00:00', 'Щербаков Константин', '+79261234567', '2', '8', '5', '5', '2500,00', 'True', '03.06.2025 9:45:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('183', '2', 'Убийство в отеле', '04.06.2025 15:00:00', '04.06.2025 16:15:00', 'Фомина Валерия', '+79269876543', '1', NULL, '3', '6', '3000,00', 'True', '04.06.2025 12:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('184', '3', 'Волшебный лес', '05.06.2025 10:00:00', '05.06.2025 10:45:00', 'Давыдов Руслан', '+79031234567', '2', '7', '4', '6', '2000,00', 'True', '04.06.2025 16:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('185', '4', 'Ограбление банка', '05.06.2025 13:00:00', '05.06.2025 14:20:00', 'Никифорова Анастасия', '+79039876543', '2', '8', '5', '5', '3200,00', 'True', '04.06.2025 14:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('186', '5', 'Секрет пирамиды', '07.06.2025 10:00:00', '07.06.2025 11:10:00', 'Воронцов Дмитрий', '+79171234567', '2', '7', '3', '4', '2800,00', 'True', '06.06.2025 10:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('187', '6', 'Космическая станция', '07.06.2025 12:00:00', '07.06.2025 13:25:00', 'Лебедева Кристина', '+79179876543', '2', '8', '4', '6', '3300,00', 'True', '05.06.2025 17:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('188', '7', 'Сокровища пиратов', '07.06.2025 15:00:00', '07.06.2025 15:50:00', 'Терехов Станислав', '+79041234567', '2', '7', '5', '5', '2200,00', 'True', '06.06.2025 14:45:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('189', '8', 'Шпионская миссия', '08.06.2025 11:00:00', '08.06.2025 12:10:00', 'Соболева Елизавета', '+79049876543', '2', '8', '3', '4', '2900,00', 'True', '07.06.2025 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('190', '9', 'Магическая академия', '08.06.2025 14:00:00', '08.06.2025 14:55:00', 'Калашников Артур', '+79181234567', '1', NULL, '5', '6', '2400,00', 'True', '08.06.2025 10:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('191', '10', 'Лаборатория будущего', '10.06.2025 10:00:00', '10.06.2025 11:15:00', 'Гусева Полина', '+79189876543', '2', '7', '4', '4', '3100,00', 'True', '09.06.2025 15:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('192', '11', 'Поиск Атлантиды', '10.06.2025 13:00:00', '10.06.2025 14:05:00', 'Комаров Григорий', '+79051234567', '2', '8', '4', '5', '2600,00', 'True', '09.06.2025 11:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('193', '12', 'Дело Шерлока Холмса', '11.06.2025 11:00:00', '11.06.2025 12:00:00', 'Афанасьева Юлия', '+79059876543', '2', '7', '3', '4', '2700,00', 'True', '10.06.2025 16:45:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('194', '1', 'Побег из тюрьмы', '11.06.2025 15:00:00', '11.06.2025 16:00:00', 'Лавров Борис', '+79191234567', '3', '8', '2', '5', '2500,00', 'False', '10.06.2025 9:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('195', '2', 'Убийство в отеле', '13.06.2025 10:00:00', '13.06.2025 11:15:00', 'Зайцева Тамара', '+79199876543', '2', '7', '5', '6', '3000,00', 'True', '12.06.2025 14:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('196', '3', 'Волшебный лес', '13.06.2025 12:30:00', '13.06.2025 13:15:00', 'Савельев Николай', '+79061234567', '2', '8', '6', '6', '2000,00', 'True', '12.06.2025 17:20:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('197', '4', 'Ограбление банка', '14.06.2025 10:00:00', '14.06.2025 11:20:00', 'Игнатова Дарья', '+79069876543', '2', '7', '4', '5', '3200,00', 'True', '13.06.2025 10:15:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('198', '5', 'Секрет пирамиды', '14.06.2025 12:00:00', '14.06.2025 13:10:00', 'Тарасов Илья', '+79201234567', '1', NULL, '4', '4', '2800,00', 'True', '14.06.2025 8:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('199', '6', 'Космическая станция', '14.06.2025 14:30:00', '14.06.2025 15:55:00', 'Хохлова Виктория', '+79209876543', '2', '8', '5', '6', '3300,00', 'True', '13.06.2025 15:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('200', '7', 'Сокровища пиратов', '15.06.2025 11:00:00', '15.06.2025 11:50:00', 'Мельников Сергей', '+79071234567', '2', '7', '3', '5', '2200,00', 'True', '14.06.2025 13:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('201', '8', 'Шпионская миссия', '15.06.2025 14:00:00', '15.06.2025 15:10:00', 'Кудрявцева Инна', '+79079876543', '1', NULL, '4', '4', '2900,00', 'True', '15.06.2025 9:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('202', '9', 'Магическая академия', '17.06.2025 10:00:00', '17.06.2025 10:55:00', 'Горбачев Андрей', '+79161234567', '2', '8', '6', '6', '2400,00', 'True', '16.06.2025 11:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('203', '10', 'Лаборатория будущего', '17.06.2025 13:00:00', '17.06.2025 14:15:00', 'Логинова Светлана', '+79169876543', '2', '7', '3', '4', '3100,00', 'True', '16.06.2025 15:45:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('204', '11', 'Поиск Атлантиды', '18.06.2025 11:00:00', '18.06.2025 12:05:00', 'Филиппов Максим', '+79261234567', '2', '8', '5', '5', '2600,00', 'True', '17.06.2025 9:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('205', '12', 'Дело Шерлока Холмса', '18.06.2025 14:00:00', '18.06.2025 15:00:00', 'Артемьева Алена', '+79269876543', '2', '7', '4', '4', '2700,00', 'True', '17.06.2025 14:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('206', '1', 'Побег из тюрьмы', '19.06.2025 10:00:00', '19.06.2025 11:00:00', 'Максимов Денис', '+79031234567', '2', '8', '3', '5', '2500,00', 'True', '18.06.2025 16:00:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('207', '2', 'Убийство в отеле', '19.06.2025 12:30:00', '19.06.2025 13:45:00', 'Копылова Елена', '+79039876543', '1', NULL, '6', '6', '3000,00', 'True', '19.06.2025 8:30:00');
INSERT INTO `orders` (`ID`, `ServiceID`, `ServiceName`, `StartTime`, `EndTime`, `ClientName`, `ClientPhone`, `StatusID`, `UserID`, `ParticipantsCount`, `MaxPeople`, `TotalPrice`, `IsActive`, `DateOfAdmission`) VALUES ('208', '3', 'Волшебный лес', '19.06.2025 15:00:00', '19.06.2025 15:45:00', 'Родионов Петр', '+79171234567', '2', '7', '4', '6', '2000,00', 'True', '18.06.2025 10:00:00');

DROP TABLE IF EXISTS `roles`;
CREATE TABLE `roles` (
  `RoleID` int NOT NULL AUTO_INCREMENT,
  `RoleName` varchar(50) NOT NULL,
  PRIMARY KEY (`RoleID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `roles` (`RoleID`, `RoleName`) VALUES ('1', 'Администратор');
INSERT INTO `roles` (`RoleID`, `RoleName`) VALUES ('2', 'Менеджер');

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

DROP TABLE IF EXISTS `statuses`;
CREATE TABLE `statuses` (
  `StatusID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`StatusID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `statuses` (`StatusID`, `Name`) VALUES ('1', 'Новый');
INSERT INTO `statuses` (`StatusID`, `Name`) VALUES ('2', 'Выполнен');
INSERT INTO `statuses` (`StatusID`, `Name`) VALUES ('3', 'Отменен');

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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `users` (`UserID`, `Login`, `Password`, `FIO`, `IDRole`) VALUES ('6', 'admin', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', 'Иванов Иван Иванович', '1');
INSERT INTO `users` (`UserID`, `Login`, `Password`, `FIO`, `IDRole`) VALUES ('7', 'manager', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', 'Петров Петр Петрович', '2');
INSERT INTO `users` (`UserID`, `Login`, `Password`, `FIO`, `IDRole`) VALUES ('8', 'manager2', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', 'Сидорова Анна Сергеевна', '2');

/*!40101 SET FOREIGN_KEY_CHECKS=1 */;
