CREATE TABLE wather (
  date datetime NOT NULL,
  title char(200) DEFAULT NULL,
  value float DEFAULT NULL COMMENT 'http://meteo.gov.ua/ua/33902/forecast/ukraine/33345',
  PRIMARY KEY (date)
)
ENGINE = INNODB,
AVG_ROW_LENGTH = 257,
CHARACTER SET utf8,
COLLATE utf8_general_ci;