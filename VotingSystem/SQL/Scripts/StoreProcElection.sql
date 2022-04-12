-- Ignore errors, they're not errors. 

/* Election table
	ElectionId int AUTO_INCREMENT,
	State varchar(2) NOT NULL,
	District int NOT NULL,
	StartDate date NOT NULL,
	EndDate date NOT NULL,
	PRIMARY KEY (ElectionId)
*/

DROP PROCEDURE IF EXISTS afetzner.add_election;
DROP PROCEDURE IF EXISTS afetzner.remove_election;
DROP PROCEDURE IF EXISTS afetzner.get_election_info_from_id;
DROP PROCEDURE IF EXISTS afetzner.get_election_id_from_info;

CREATE PROCEDURE afetzner.add_election(
		IN varStateAbbrev varchar(2),
		IN varDistrictNum int,
        IN varStartDate DATE,
        IN varEndDate DATE)
	INSERT INTO Election(State, District, StartDate, EndDate) 
    VALUES (varStateAbbrev, varDistrictNum, varStartDate, varEndDate);

CREATE PROCEDURE afetzner.remove_election(
		IN varElectionId int)
	DELETE FROM Election WHERE ElectionId = varElectionId;

CREATE PROCEDURE afetzner.get_election_info_from_id(
		IN varElectionId int)
	SELECT State, District, StartDate, EndDate FROM Election 
    WHERE electionId = varElectionId;
    
CREATE PROCEDURE afetzner.get_election_id_from_info(
		IN varStateAbbrev varchar(2),
        IN varDistrictNum int,
        IN varActiveDate DATE,
        OUT varElectionId int)
	SELECT Election.electionId INTO varElectionId FROM Election WHERE 
		Election.State = varStateAbbrev AND
        Election.DistrictNum = varDistrictNum AND
        Election.StartDate <= varActiveDate AND
        Election.EndDate >= varActiveDate;
	