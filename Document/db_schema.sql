-- MySQL dump 10.13  Distrib 5.5.8, for Win64 (x86)
--
-- Host: 127.0.0.1    Database: galant
-- ------------------------------------------------------
-- Server version	5.5.8

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `entities`
--

DROP TABLE IF EXISTS `entities`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `entities` (
  `entity_id` int(11) NOT NULL AUTO_INCREMENT,
  `alias` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `password` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `full_name` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `home_phone` varchar(15) COLLATE utf8_unicode_ci DEFAULT NULL,
  `cell_phone1` varchar(15) COLLATE utf8_unicode_ci DEFAULT NULL,
  `cell_phone2` varchar(15) COLLATE utf8_unicode_ci DEFAULT NULL,
  `type` int(11) NOT NULL DEFAULT '2',
  `address_family` text COLLATE utf8_unicode_ci,
  `address_child` text COLLATE utf8_unicode_ci,
  `comment` text COLLATE utf8_unicode_ci,
  `deposit` decimal(10,2) DEFAULT '0.00',
  `pay_type` int(11) DEFAULT NULL '付款类型\n预付，后附，及时付款',
  `route_station` int(11) DEFAULT NULL COMMENT,
  `able_flag` tinyint(1) NOT NULL DEFAULT '1',
  `last_update_time` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`entity_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `entities`
--

LOCK TABLES `entities` WRITE;
/*!40000 ALTER TABLE `entities` DISABLE KEYS */;
/*!40000 ALTER TABLE `entities` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `event_logs`
--

DROP TABLE IF EXISTS `event_logs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event_logs` (
  `event_id` int(11) NOT NULL AUTO_INCREMENT,
  `paper_id` varchar(8) COLLATE utf8_unicode_ci DEFAULT NULL,
  `insert_time` datetime NOT NULL,
  `relation_entity` int(11) NOT NULL,
  `entity_id` int(11) DEFAULT NULL,
  `at_station` int(11) DEFAULT NULL,
  `event_type` varchar(10) COLLATE utf8_unicode_ci NOT NULL,
  `event_data` text COLLATE utf8_unicode_ci,
  PRIMARY KEY (`event_id`),
  KEY `paper_id_relate` (`paper_id`),
  CONSTRAINT `paper_id_relate` FOREIGN KEY (`paper_id`) REFERENCES `papers` (`paper_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `event_logs`
--

LOCK TABLES `event_logs` WRITE;
/*!40000 ALTER TABLE `event_logs` DISABLE KEYS */;
/*!40000 ALTER TABLE `event_logs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `origin_paper_links`
--

DROP TABLE IF EXISTS `origin_paper_links`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `origin_paper_links` (
  `link_id` int(11) NOT NULL AUTO_INCREMENT,
  `paper_id` varchar(8) COLLATE utf8_unicode_ci DEFAULT NULL,
  `origin_name` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`link_id`),
  KEY `origin_paper_id_relate` (`paper_id`),
  CONSTRAINT `origin_paper_id_relate` FOREIGN KEY (`paper_id`) REFERENCES `papers` (`paper_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `origin_paper_links`
--

LOCK TABLES `origin_paper_links` WRITE;
/*!40000 ALTER TABLE `origin_paper_links` DISABLE KEYS */;
/*!40000 ALTER TABLE `origin_paper_links` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `packages`
--

DROP TABLE IF EXISTS `packages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `packages` (
  `package_id` int(11) NOT NULL AUTO_INCREMENT,
  `product_id` int(11) NOT NULL,
  `count` decimal(8,0) NOT NULL DEFAULT '1',
  `amount` decimal(10,2) NOT NULL,
  `origin_amount` decimal(10,2) NOT NULL,
  `paper_id` varchar(32) COLLATE utf8_unicode_ci NOT NULL,
  `state` int(11) NOT NULL,
  PRIMARY KEY (`package_id`),
  KEY `package_paper_id_relate` (`paper_id`),
  KEY `package_product_id_relate` (`product_id`),
  CONSTRAINT `package_paper_id_relate` FOREIGN KEY (`paper_id`) REFERENCES `papers` (`paper_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `package_product_id_relate` FOREIGN KEY (`product_id`) REFERENCES `products` (`product_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `packages`
--

LOCK TABLES `packages` WRITE;
/*!40000 ALTER TABLE `packages` DISABLE KEYS */;
/*!40000 ALTER TABLE `packages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paper_links`
--

DROP TABLE IF EXISTS `paper_links`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `paper_links` (
  `link_id` int(11) NOT NULL AUTO_INCREMENT,
  `paper_id` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `parent_id` int(11) DEFAULT NULL,
  `able_flag` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`link_id`),
  KEY `link_paper_id_relate` (`paper_id`),
  CONSTRAINT `link_paper_id_relate` FOREIGN KEY (`paper_id`) REFERENCES `papers` (`paper_id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paper_links`
--

LOCK TABLES `paper_links` WRITE;
/*!40000 ALTER TABLE `paper_links` DISABLE KEYS */;
/*!40000 ALTER TABLE `paper_links` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paper_routes`
--

DROP TABLE IF EXISTS `paper_routes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `paper_routes` (
  `step_id` int(11) NOT NULL AUTO_INCREMENT,
  `paper_id` varchar(8) COLLATE utf8_unicode_ci NOT NULL,
  `route_id` int(11) NOT NULL,
  `is_routed` tinyint(1) NOT NULL,
  `able_flag` tinyint(1) NOT NULL,
  PRIMARY KEY (`step_id`),
  KEY `route_paper_id_relate` (`paper_id`),
  CONSTRAINT `route_paper_id_relate` FOREIGN KEY (`paper_id`) REFERENCES `papers` (`paper_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paper_routes`
--

LOCK TABLES `paper_routes` WRITE;
/*!40000 ALTER TABLE `paper_routes` DISABLE KEYS */;
/*!40000 ALTER TABLE `paper_routes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `papers`
--

DROP TABLE IF EXISTS `papers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `papers` (
  `paper_id` varchar(8) COLLATE utf8_unicode_ci NOT NULL,
  `status` int(11) NOT NULL,
  `substate` int(11) NOT NULL,
  `holder` int(11) NOT NULL,
  `bound` int(11) NOT NULL,
  `contact_a` int(11) NOT NULL,
  `contact_b` int(11) NOT NULL,
  `contact_c` int(11) NOT NULL,
  `deliver_a` int(11) DEFAULT NULL,
  `deliver_b` int(11) DEFAULT NULL,
  `deliver_c` int(11) DEFAULT NULL,
  `deliver_a_time` datetime DEFAULT NULL,
  `deliver_b_time` datetime DEFAULT NULL,
  `deliver_c_time` datetime DEFAULT NULL,
  `start_time` datetime DEFAULT NULL,
  `finish_time` datetime DEFAULT NULL,
  `salary` decimal(10,2) DEFAULT NULL,
  `comment` text COLLATE utf8_unicode_ci,
  `type` int(11) DEFAULT NULL,
  `next_route` int(11) DEFAULT NULL,
  `mobile_status` int(11) DEFAULT NULL,
  PRIMARY KEY (`paper_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `papers`
--

LOCK TABLES `papers` WRITE;
/*!40000 ALTER TABLE `papers` DISABLE KEYS */;
/*!40000 ALTER TABLE `papers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `products` (
  `product_id` int(11) NOT NULL AUTO_INCREMENT,
  `product_name` varchar(20) COLLATE utf8_unicode_ci NOT NULL,
  `alias` varchar(20) COLLATE utf8_unicode_ci DEFAULT NULL,
  `amount` decimal(10,2) NOT NULL DEFAULT '0.00',
  `type` int(11) NOT NULL,
  `discretion` text COLLATE utf8_unicode_ci,
  `need_back` tinyint(1) NOT NULL,
  `return_name` varchar(20) COLLATE utf8_unicode_ci DEFAULT NULL,
  `return_value` decimal(10,2) DEFAULT NULL,
  `able_flag` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `roles` (
  `role_id` int(11) NOT NULL AUTO_INCREMENT,
  `entity_id` int(11) NOT NULL,
  `station_id` int(11) DEFAULT NULL,
  `role_type` int(11) NOT NULL,
  PRIMARY KEY (`role_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `routes`
--

DROP TABLE IF EXISTS `routes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `routes` (
  `route_id` int(11) NOT NULL AUTO_INCREMENT,
  `route_name` varchar(32) COLLATE utf8_unicode_ci DEFAULT NULL,
  `from_entity` int(11) NOT NULL,
  `to_entity` int(11) DEFAULT NULL,
  `is_finally` tinyint(1) NOT NULL,
  PRIMARY KEY (`route_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `routes`
--

LOCK TABLES `routes` WRITE;
/*!40000 ALTER TABLE `routes` DISABLE KEYS */;
/*!40000 ALTER TABLE `routes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stores`
--

DROP TABLE IF EXISTS `stores`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `stores` (
  `store_id` int(11) NOT NULL AUTO_INCREMENT,
  `entity_id` int(11) NOT NULL,
  `product_id` int(11) NOT NULL,
  `product_count` decimal(8,0) DEFAULT NULL,
  `bound` int(11) DEFAULT NULL,
  PRIMARY KEY (`store_id`),
  KEY `store_product_id_relate` (`product_id`),
  CONSTRAINT `store_product_id_relate` FOREIGN KEY (`product_id`) REFERENCES `products` (`product_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stores`
--

LOCK TABLES `stores` WRITE;
/*!40000 ALTER TABLE `stores` DISABLE KEYS */;
/*!40000 ALTER TABLE `stores` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2011-01-18  1:18:02
