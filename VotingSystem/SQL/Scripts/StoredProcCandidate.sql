-- Ignore errors, they're not errors
/* Candidate table:
	CandidateId int AUTO_INCREMENT,
	LastName varchar(64) NOT NULL,
	FirstName varchar(32) NOT NULL,
	PRIMARY KEY (CandidateId)
*/

DELIMITER //
DROP PROCEDURE IF EXISTS afetzner.add_candidate //
DROP PROCEDURE IF EXISTS afetzner.delete_candidate //
DROP PROCEDURE IF EXISTS afetzner.get_candidate_id_from_info //
DROP PROCEDURE IF EXISTS afetzner.get_candidate_info_from_id //
//

CREATE PROCEDURE afetzner.add_candidate(
		IN LastName varchar(64),
        IN FirstName varchar(32),
		OUT CandidateId int)
BEGIN
	INSERT INTO Candidate(LastName, FirstName)
	VALUES (LastName, FirstName);
	SET CandidateId = LAST_INSERT_ID();
END //
    
CREATE PROCEDURE afetzner.delete_candidate(
		IN CandidateId int)
BEGIN
	DELETE FROM Candidate WHERE Candidate.candidateId = CandidateId;
END //

CREATE PROCEDURE afetzner.get_candidate_id_from_info (
		IN LastName varchar(64),
        IN FirstName varchar(32),
        OUT CandidateId int)
BEGIN
	SELECT CadidateId INTO CandidateId FROM Candidate
	WHERE LastName = LastName AND
		FirstName = FirstName
	LIMIT 1;
END //

CREATE PROCEDURE afetzner.get_candidate_info_from_id (
		IN CandidateId int,
		OUT LastName varchar(32),
		OUT FirstName varchar(16))
BEGIN
	SELECT lastName, firstName 
	INTO LastName, FirstName FROM Candidate
	WHERE CandidateId = CandidateId
	LIMIT 1;
END //
DELIMITER ;