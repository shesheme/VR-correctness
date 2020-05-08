-- --------------------------------------------------------
-- 호스트:                          127.0.0.1
-- 서버 버전:                        8.0.19 - MySQL Community Server - GPL
-- 서버 OS:                        Win64
-- HeidiSQL 버전:                  10.3.0.5771
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- vrcorrectness 데이터베이스 구조 내보내기
CREATE DATABASE IF NOT EXISTS `vrcorrectness` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `vrcorrectness`;

-- 테이블 vrcorrectness.tb_member 구조 내보내기
CREATE TABLE IF NOT EXISTS `tb_member` (
  `MemSeq` int NOT NULL AUTO_INCREMENT COMMENT '회원번호',
  `Id` varchar(15) NOT NULL COMMENT '회원아이디',
  `Pw` varchar(20) NOT NULL COMMENT '회원비밀번호',
  PRIMARY KEY (`MemSeq`),
  UNIQUE KEY `Id` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='회원의 아이디와 비밀번호를 저장하며 회원번호(MemSeq)는 자동증가하는 PK이다.\r\n회원번호(MemSeq)는 TB_Record의 MemSeq과 1대1로 매칭한다.';

-- 내보낼 데이터가 선택되어 있지 않습니다.

-- 테이블 vrcorrectness.tb_poscorrection 구조 내보내기
CREATE TABLE IF NOT EXISTS `tb_poscorrection` (
  `PosSeq` int NOT NULL AUTO_INCREMENT COMMENT '자세번호',
  `PosName` varchar(20) NOT NULL,
  `posX` int NOT NULL,
  `posY` int NOT NULL,
  `posZ` int NOT NULL,
  `rotX` int NOT NULL,
  `rotY` int NOT NULL,
  `rotZ` int NOT NULL,
  PRIMARY KEY (`PosSeq`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='자세교정을 위한 자세들의 좌표들을 관리하는 테이블이다.';

-- 내보낼 데이터가 선택되어 있지 않습니다.

-- 테이블 vrcorrectness.tb_record 구조 내보내기
CREATE TABLE IF NOT EXISTS `tb_record` (
  `MemSeq` int NOT NULL COMMENT '회원번호',
  `RecData` text NOT NULL COMMENT '회원의 자세교정 기록',
  PRIMARY KEY (`MemSeq`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='회원기록 테이블 : 회원번화와 교정기록데이터를 저장하며\r\n회원번호(MemSeq)는 회원정보테이블(TB_Member)와 1대1 관계를 맺는다.\r\n그리고 교정기록 데이터인 RecData는 회원의 교정기록데이터인 XML데이터를 저장하는데 사용';

-- 내보낼 데이터가 선택되어 있지 않습니다.

-- 프로시저 vrcorrectness.usp_add_Member 구조 내보내기
DELIMITER //
CREATE PROCEDURE `usp_add_Member`(
	IN in_Id VARCHAR(15),
	IN in_Pw VARCHAR(20),
	OUT out_returnValue INT
)
BEGIN
	IF in_Id IS NOT NULL AND in_Pw IS NOT NULL THEN
		IF NOT EXISTS (SELECT * FROM tb_member WHERE Id = in_Id LIMIT 1) THEN
			INSERT INTO tb_member VALUES(in_Id, in_Pw);
			INSERT INTO tb_record VALUES((SELECT MemSeq FROM tb_member WHERE id = in_Id LIMIT 1), 'No Data');
			SET out_returnValue = 1;
		ELSE
			SET out_returnValue = 0;
		END IF;
	ELSE 
		SET out_returnValue = 0;
	END IF;
END//
DELIMITER ;

-- 프로시저 vrcorrectness.usp_get_PosCorrection 구조 내보내기
DELIMITER //
CREATE PROCEDURE `usp_get_PosCorrection`(
	IN in_PosSeq INT,
	OUT out_posX INT,
	OUT out_posY INT,
	OUT out_posZ INT,
	OUT out_rotX INT,
	OUT out_rotY INT,
	OUT out_rotZ INT
)
BEGIN
	SET out_posX = (SELECT posX FROM tb_poscorrection WHERE PosSeq = in_PosSeq LIMIT 1);
	SET out_posY = (SELECT posY FROM tb_poscorrection WHERE PosSeq = in_PosSeq LIMIT 1);
	SET out_posZ = (SELECT posZ FROM tb_poscorrection WHERE PosSeq = in_PosSeq LIMIT 1);
	SET out_rotX = (SELECT rotX FROM tb_poscorrection WHERE PosSeq = in_PosSeq LIMIT 1);
	SET out_rotY = (SELECT rotY FROM tb_poscorrection WHERE PosSeq = in_PosSeq LIMIT 1);
	SET out_rotZ = (SELECT rotZ FROM tb_poscorrection WHERE PosSeq = in_PosSeq LIMIT 1);
END//
DELIMITER ;

-- 프로시저 vrcorrectness.usp_get_Record 구조 내보내기
DELIMITER //
CREATE PROCEDURE `usp_get_Record`(
	IN in_MemSeq INT,
	OUT out_RecData TEXT
)
BEGIN
	IF in_MemSeq IS NOT NULL THEN
		IF EXISTS (SELECT * FROM tb_record WHERE MemSeq = in_MemSeq LIMIT 1) THEN
			SET out_RecData = (SELECT RecData FROM tb_record WHERE MemSeq = in_MemSeq LIMIT 1);
		END IF;
	END IF;
END//
DELIMITER ;

-- 프로시저 vrcorrectness.usp_login_Member 구조 내보내기
DELIMITER //
CREATE PROCEDURE `usp_login_Member`(
	IN in_Id VARCHAR(15),
	IN in_Pw VARCHAR(20),
	OUT out_MemSeq INT
)
BEGIN
	IF in_Id IS NOT NULL AND in_Pw IS NOT NULL THEN
		IF NOT EXISTS (SELECT * FROM tb_member WHERE Id = in_Id LIMIT 1) THEN
			SET out_MemSeq = (SELECT MemSeq FROM tb_member WHERE Id = in_Id LIMIT 1);
		ELSE
			SET out_MemSeq = 0;
		END IF;
	ELSE 
		SET out_MemSeq = 0;
	END IF;
END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
