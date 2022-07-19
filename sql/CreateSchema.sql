CREATE DATABASE u05jyp;
use u05jyp;


Create TABLE city (
cityId INT(10),
city VARCHAR(50),
countryId INT(10),
createDate DATETIME,
createdBy VARCHAR(40),
lastUpdate TIMESTAMP,
lastUpdateBy VARCHAR(40)
);

CREATE TABLE country (
countryId INT(10),
country VARCHAR(50),
createDate DATETIME,
createdBy VARCHAR(40),
lastUpdate TIMESTAMP,
lastUpdateBy VARCHAR(40)
);

CREATE TABLE address (
addressId INT(10),
address VARCHAR(50),
address2 VARCHAR(50),
cityId INT(10),
postalCode VARCHAR(10),
phone VARCHAR(20),
createDate DATETIME,
createdBy VARCHAR(40),
lastUpdate TIMESTAMP,
lastUpdateBy VARCHAR(40)
);

CREATE TABLE customer (
customerId INT(10),
customerName VARCHAR(45),
addressId INT(10),
active TINYINT(1),
createDate DATETIME,
createdBy VARCHAR(40),
lastUpdate TIMESTAMP,
lastUpdateBy VARCHAR(40)
);

CREATE TABLE appointment (
appointmentId INT(10),
customerId INT(10),
userId INT,
title VARCHAR(255),
description TEXT,
location TEXT,
contact TEXT,
type TEXT,
url VARCHAR(255),
start DATETIME,
end DATETIME,
createDate DATETIME,
createdBy VARCHAR(40),
lastUpdate TIMESTAMP,
lastUpdateBy VARCHAR(40)
);

CREATE TABLE user (
userId INT,
userName VARCHAR(50),
password VARCHAR(50),
active TINYINT,
createDate DATETIME,
createdBy VARCHAR(40),
lastUpdate TIMESTAMP,
lastUpdateBy VARCHAR(40)
);

INSERT INTO user (userId, userName, password, active, createDate, createdBy, lastUpdate, lastUpdateBy)
VALUES
(1, "test", "test", 1, '2022-04-22 00:00:00', "LukeMelton", '2022-04-22 00:00:00', "LukeMelton")



INSERT INTO appointment (appointmentId, customerId, start, end, type, userId, createDate, createdBy, 
lastUpdate, lastUpdateBy) VALUES ('1', '1', STR_TO_DATE('4/25/2022 1:30:00 PM, '%m/%d/%Y %h:%i:%s %p')', 
STR_TO_DATE('4/25/2022 1:30:00 PM, '%m/%d/%Y %h:%i:%s %p')', '12', '1', STR_TO_DATE('4/25/2022 11:15:20 PM, 
'%m/%d/%Y %h:%i:%s %p')', 'test', STR_TO_DATE('4/25/2022 11:15:20 PM, '%m/%d/%Y %h:%i:%s %p')', 'test')

