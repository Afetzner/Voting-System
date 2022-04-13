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

CREATE PROCEDURE afetzner.add_election(
		IN varStateAbbrev varchar(2),
		IN varDistrictNum int,
        IN varStartDate DATE,
        IN varEndDate DATE)
	INSERT INTO Election(State, District, StartDate, EndDate) 
    VALUES (varStateAbbrev, varDistrictNum, varStartDate, varEndDate);
    
CREATE PROCEDURE afetzner.get_election_info_from_id(
		IN varElectionId int)
	SELECT * FROM Election WHERE Election.electionId = varElectionId;
    
CREATE PROCEDURE afetzner.get_election_id_from_info(
		IN varStateAbbrev varchar(2),
        IN varDistrictNum int,
        IN varActiveDate DATE)
	SELECT Election.electionId FROM Election WHERE 
		Election.State = varStateAbbrev AND
        Election.DistrictNum = varDistrictNum AND
        Election.StartDate <= varActiveDate AND
        Election.EndDate >= varActiveDate;
	