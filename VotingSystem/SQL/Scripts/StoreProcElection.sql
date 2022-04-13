-- Ignore errors, they're not errors. 
/* Election table
	ElectionId int AUTO_INCREMENT,
	State varchar(2) NOT NULL,
	District int NOT NULL,
	StartDate date NOT NULL,
	EndDate date NOT NULL,
	PRIMARY KEY (ElectionId)
*/

DELIMITER //
DROP PROCEDURE IF EXISTS afetzner.add_election //
DROP PROCEDURE IF EXISTS afetzner.delete_election //
DROP PROCEDURE IF EXISTS afetzner.get_election_id_from_info //
DROP PROCEDURE IF EXISTS afetzner.get_election_info_from_id //

CREATE PROCEDURE afetzner.add_election(
		IN varStateAbbrev varchar(2),
		IN varDistrictNum int,
        IN varStartDate DATE,
        IN varEndDate DATE,
		OUT varElectionId int)
BEGIN
	INSERT INTO Election(State, District, StartDate, EndDate) 
    VALUES (varStateAbbrev, varDistrictNum, varStartDate, varEndDate);
	SET varElectionId = LAST_INSERT_ID();
END //

CREATE PROCEDURE afetzner.delete_election(
		IN varElectionId int)
BEGIN
	DELETE FROM Election WHERE Election.ElectionId = varElectionId;
END //

CREATE PROCEDURE afetzner.get_election_id_from_info(
		IN varStateAbbrev varchar(2),
        IN varDistrictNum int,
        IN varActiveDate DATE,
        OUT varElectionId int)
BEGIN
	SELECT Election.electionId 
	INTO varElectionId FROM Election 
	WHERE Election.State = varStateAbbrev AND
        Election.DistrictNum = varDistrictNum AND
        Election.StartDate <= varActiveDate AND
        Election.EndDate >= varActiveDate
	LIMIT 1;
END //

CREATE PROCEDURE afetzner.get_election_info_from_id(
		IN varElectionId int,
		OUT varDistrict int,
		OUT varStartDate DATE,
		OUT varEndDate DATE)
BEGIN
	SELECT State, District, StartDate, EndDate 
	INTO varDistrict, varStartDate, varEndDate FROM Election 
    WHERE Election.electionId = varElectionId
	LIMIT 1;
END //

DELIMITER ;