-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: localhost    Database: questbooking
-- ------------------------------------------------------
-- Server version	8.0.39

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `CategoriesID` int NOT NULL AUTO_INCREMENT,
  `Categorie` varchar(100) NOT NULL,
  PRIMARY KEY (`CategoriesID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (1,'Приключенческие'),(2,'Детективные'),(3,'Фэнтези'),(4,'Научная фантастика');
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clients`
--

DROP TABLE IF EXISTS `clients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clients`
--

LOCK TABLES `clients` WRITE;
/*!40000 ALTER TABLE `clients` DISABLE KEYS */;
INSERT INTO `clients` VALUES (1,'Алексей','Смирнов','Владимирович','+79161111111',25),(2,'Мария','Кузнецова','Игоревна','+79162222222',30),(3,'Дмитрий','Попов','Александрович','+79163333333',22),(4,'Елена','Васильева','Дмитриевна','+79164444444',28),(5,'Артем','Петров','Сергеевич','+79165555555',35),(6,'Ольга','Соколова','Андреевна','+79166666666',26),(7,'Сергей','Михайлов','Викторович','+79167777777',32),(8,'Анна','Новикова','Павловна','+79168888888',29),(9,'Владимир','Федоров','Олегович','+79169999999',31),(10,'Ирина','Морозова','Николаевна','+79161010101',27),(11,'Андрей','Волков','Иванович','+79161110101',24),(12,'Наталья','Алексеева','Сергеевна','+79161212121',33),(13,'Павел','Лебедев','Дмитриевич','+79161313131',26),(14,'Юлия','Семенова','Анатольевна','+79161414141',30),(15,'Кирилл','Егоров','Валерьевич','+79161515151',28),(16,'Татьяна','Павлова','Владимировна','+79161616161',25),(17,'Максим','Козлов','Алексеевич','+79161717171',29),(18,'Екатерина','Степанова','Игоревна','+79161818181',31),(19,'Александр','Николаев','Петрович','+79161919191',34),(20,'Светлана','Орлова','Викторовна','+79162020202',26),(21,'Иван','Андреев','Сергеевич','+79162121212',23),(22,'Людмила','Макарова','Дмитриевна','+79162222223',32),(23,'Григорий','Никитин','Андреевич','+79162323232',30),(24,'Виктория','Захарова','Павловна','+79162424242',27),(25,'Роман','Зайцев','Владимирович','+79162525252',29),(26,'Маргарита','Соловьева','Александровна','+79162626262',28),(27,'Станислав','Борисов','Игоревич','+79162727272',31),(28,'Алина','Яковлева','Викторовна','+79162828282',24),(29,'Константин','Григорьев','Олегович','+79162929292',33),(30,'Валерия','Романова','Сергеевна','+79163030303',26);
/*!40000 ALTER TABLE `clients` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `difficultylevels`
--

DROP TABLE IF EXISTS `difficultylevels`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `difficultylevels` (
  `DifficultyID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`DifficultyID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `difficultylevels`
--

LOCK TABLES `difficultylevels` WRITE;
/*!40000 ALTER TABLE `difficultylevels` DISABLE KEYS */;
INSERT INTO `difficultylevels` VALUES (1,'Легкий'),(2,'Средний'),(3,'Сложный');
/*!40000 ALTER TABLE `difficultylevels` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
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
  PRIMARY KEY (`ID`),
  KEY `ClientID` (`ClientID`),
  KEY `Article` (`Article`),
  KEY `StatusID` (`StatusID`),
  KEY `UserID` (`UserID`),
  CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`ClientID`) REFERENCES `clients` (`ClientID`),
  CONSTRAINT `orders_ibfk_2` FOREIGN KEY (`Article`) REFERENCES `services` (`Article`),
  CONSTRAINT `orders_ibfk_3` FOREIGN KEY (`StatusID`) REFERENCES `statuses` (`StatusID`),
  CONSTRAINT `orders_ibfk_4` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (1,1,1,'2025-01-15 18:00:00','2025-01-15 19:00:00',2,3,4,10000.00),(2,2,2,'2025-01-16 19:30:00','2025-01-16 20:45:00',2,4,5,15000.00),(3,3,3,'2025-01-17 17:00:00','2025-01-17 17:45:00',2,3,6,12000.00),(4,4,4,'2025-01-18 20:00:00','2025-01-18 21:20:00',1,5,3,9600.00),(5,5,5,'2025-01-19 18:30:00','2025-01-19 19:40:00',1,4,4,11200.00),(6,6,6,'2025-01-20 19:00:00','2025-01-20 20:25:00',1,3,5,16500.00),(7,7,7,'2025-01-21 16:30:00','2025-01-21 17:20:00',2,5,4,8800.00),(8,8,8,'2025-01-22 21:00:00','2025-01-22 22:15:00',2,4,6,17400.00),(9,9,9,'2025-01-23 18:00:00','2025-01-23 18:55:00',1,3,5,12000.00),(10,10,10,'2025-01-24 19:30:00','2025-01-24 20:45:00',1,5,3,9300.00),(11,11,1,'2025-01-25 17:30:00','2025-01-25 18:30:00',2,4,4,10000.00),(12,12,2,'2025-01-26 20:00:00','2025-01-26 21:15:00',2,3,6,18000.00),(13,13,3,'2025-01-27 18:30:00','2025-01-27 19:15:00',1,5,5,10000.00),(14,14,4,'2025-01-28 19:00:00','2025-01-28 20:20:00',1,4,4,12800.00),(15,15,5,'2025-01-29 17:00:00','2025-01-29 18:10:00',2,3,3,8400.00),(16,16,6,'2025-01-30 20:30:00','2025-01-30 21:55:00',2,5,5,16500.00),(17,17,7,'2025-01-31 18:00:00','2025-01-31 18:50:00',1,4,6,13200.00),(18,18,8,'2025-02-01 19:30:00','2025-02-01 20:55:00',1,3,4,13200.00),(19,19,9,'2025-02-02 17:30:00','2025-02-02 18:25:00',2,5,5,12000.00),(20,20,10,'2025-02-03 20:00:00','2025-02-03 21:15:00',2,4,3,9300.00),(21,21,1,'2025-02-04 18:30:00','2025-02-04 19:30:00',1,3,4,10000.00),(22,22,2,'2025-02-05 19:00:00','2025-02-05 20:15:00',1,5,5,15000.00),(23,23,3,'2025-02-06 17:00:00','2025-02-06 17:45:00',2,4,6,12000.00),(24,24,4,'2025-02-07 20:30:00','2025-02-07 21:50:00',2,3,4,12800.00),(25,25,5,'2025-02-08 18:00:00','2025-02-08 19:10:00',1,5,3,8400.00),(26,26,6,'2025-02-09 19:30:00','2025-02-09 20:55:00',2,4,5,16500.00),(27,27,7,'2025-02-10 17:30:00','2025-02-10 18:20:00',2,3,4,8800.00),(28,28,8,'2025-02-11 20:00:00','2025-02-11 21:15:00',2,5,6,17400.00),(29,29,9,'2025-02-12 18:30:00','2025-02-12 19:25:00',1,4,5,12000.00),(30,30,10,'2025-02-13 19:00:00','2025-02-13 20:15:00',1,3,3,9300.00),(31,1,11,'2025-02-14 17:00:00','2025-02-14 18:05:00',2,5,4,10400.00),(32,2,12,'2025-02-15 20:30:00','2025-02-15 21:30:00',2,4,5,13500.00),(33,3,11,'2025-02-16 18:00:00','2025-02-16 19:05:00',1,3,3,7800.00),(34,4,12,'2025-02-17 19:30:00','2025-02-17 20:30:00',1,5,4,10800.00),(35,5,1,'2025-02-18 17:30:00','2025-02-18 18:30:00',2,4,5,12500.00),(36,6,2,'2025-02-19 20:00:00','2025-02-19 21:15:00',2,3,6,18000.00),(37,7,3,'2025-02-20 18:30:00','2025-02-20 19:15:00',1,5,4,8000.00),(38,8,4,'2025-02-21 19:00:00','2025-02-21 20:20:00',1,4,3,9600.00),(39,9,5,'2025-02-22 17:00:00','2025-02-22 18:10:00',2,3,4,11200.00),(40,10,6,'2025-02-23 20:30:00','2025-02-23 21:55:00',2,5,5,16500.00),(41,11,7,'2025-02-24 18:00:00','2025-02-24 18:50:00',1,4,6,13200.00),(42,12,8,'2025-02-25 19:30:00','2025-02-25 20:55:00',1,3,4,11600.00),(43,13,9,'2025-02-26 17:30:00','2025-02-26 18:25:00',2,5,5,12000.00),(44,14,10,'2025-02-27 20:00:00','2025-02-27 21:15:00',2,4,3,9300.00),(45,15,11,'2025-02-28 18:30:00','2025-02-28 19:35:00',1,3,4,10400.00),(46,16,12,'2025-03-05 20:00:00','2025-03-05 21:30:00',1,5,5,13500.00),(47,17,1,'2025-03-01 17:00:00','2025-03-01 18:00:00',2,4,4,10000.00),(48,18,2,'2025-03-02 20:30:00','2025-03-02 21:45:00',1,3,6,18000.00),(49,19,3,'2025-03-03 18:00:00','2025-03-03 18:45:00',3,5,5,10000.00),(50,20,4,'2025-03-04 19:30:00','2025-03-04 20:50:00',2,4,4,12800.00);
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `services`
--

DROP TABLE IF EXISTS `services`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `services` (
  `Article` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `Description` text,
  `Price` decimal(10,2) NOT NULL,
  `Time` int DEFAULT NULL,
  `DayOfTheWeek` int DEFAULT NULL,
  `Picture` varchar(255) DEFAULT NULL,
  `MaxPeople` int DEFAULT NULL,
  `ISLevel` int DEFAULT NULL,
  `IDCategory` int DEFAULT NULL,
  PRIMARY KEY (`Article`),
  KEY `ISLevel` (`ISLevel`),
  KEY `IDCategory` (`IDCategory`),
  CONSTRAINT `services_ibfk_1` FOREIGN KEY (`ISLevel`) REFERENCES `difficultylevels` (`DifficultyID`),
  CONSTRAINT `services_ibfk_2` FOREIGN KEY (`IDCategory`) REFERENCES `categories` (`CategoriesID`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `services`
--

LOCK TABLES `services` WRITE;
/*!40000 ALTER TABLE `services` DISABLE KEYS */;
INSERT INTO `services` VALUES (1,'Побег из тюрьмы','Спланируйте идеальный побег из средневековой тюрьмы',2500.00,60,30,'room1.jpg',5,2,1),(2,'Убийство в отеле','Раскройте загадочное убийство в роскошном отеле',3000.00,75,30,'room2.jpg',6,3,2),(3,'Волшебный лес','Найдите магический артефакт в зачарованном лесу',2000.00,45,30,'room3.jpg',6,1,3),(4,'Ограбление банка','Совершите идеальное ограбление банка',3200.00,80,30,'room4.jpg',5,3,1),(5,'Секрет пирамиды','Разгадайте тайны древней пирамиды',2800.00,70,30,'room5.jpg',4,2,1),(6,'Космическая станция','Спасите станцию от катастрофы',3300.00,85,30,'room6.jpg',6,3,4),(7,'Сокровища пиратов','Найдите клад на заброшенном острове',2200.00,50,30,'room7.jpg',5,1,1),(8,'Шпионская миссия','Выполните секретное задание в посольстве',2900.00,70,30,'room8.jpg',4,2,2),(9,'Магическая академия','Станьте студентом школы магии',2400.00,55,30,'room9.jpg',6,1,3),(10,'Лаборатория будущего','Создайте революционное изобретение',3100.00,75,30,'room10.jpg',4,3,4),(11,'Поиск Атлантиды','Найдите затерянный город под водой',2600.00,65,30,'room11.jpg',5,2,1),(12,'Дело Шерлока Холмса','Помогите известному детективу раскрыть дело',2700.00,60,30,'room12.jpg',4,2,2);
/*!40000 ALTER TABLE `services` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statuses`
--

DROP TABLE IF EXISTS `statuses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statuses` (
  `StatusID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`StatusID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statuses`
--

LOCK TABLES `statuses` WRITE;
/*!40000 ALTER TABLE `statuses` DISABLE KEYS */;
INSERT INTO `statuses` VALUES (1,'В работе'),(2,'Выполнен'),(3,'Отменен');
/*!40000 ALTER TABLE `statuses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `UserID` int NOT NULL AUTO_INCREMENT,
  `Login` varchar(50) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `FIO` varchar(150) NOT NULL,
  `IDRole` int DEFAULT NULL,
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `Login` (`Login`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (6,'admin','6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b','Иванов Иван Иванович',1),(7,'director','6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b','Петров Петр Петрович',2),(8,'manager','6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b','Сидорова Анна Сергеевна',3),(18,'ser','6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b','Секавыавы апрыва Проаавы',1),(19,'myxa','6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b','Мухин Антон Андреевич',2);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-01-05 13:09:28
