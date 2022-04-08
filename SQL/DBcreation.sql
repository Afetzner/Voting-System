CREATE TABLE Voter (
	VoterId int,
	LastName varchar(64) NOT NULL,
	FirstName varchar(32) NOT NULL,
	MiddleName varchar(32),
	LicenceNumber varchar(32) NOT NULL,
	PRIMARY KEY (VoterId)
)

CREATE TABLE Administrator(
	AdminId int,
	UserName varchar(32) NOT NULL,
	Password varchar(32) NOT NULL,
	PRIMARY KEY (adminId)
)

CREATE TABLE Election (
	ElectionId int,
	State varchar(2) NOT NULL,
	District int NOT NULL,
	StartDate date NOT NULL,
	EndDate date NOT NULL
	PRIMARY KEY (ElectionId)
)

CREATE TABLE Candidate (
	CandidateId int,
	LastName varchar(64) NOT NULL,
	FirstName varchar(32) NOT NULL,
	PRIMARY KEY (CandidateId)
) 

CREATE TABLE CandidateElection (
	CandidateElectionId int,
	CandidateId int,
	ElectionId int,
	PRIMARY KEY (CandidateElectionId),
	FOREIGN KEY (CandidateId) REFERENCES Candidate(CandidateId),
	FOREIGN KEY (ElectionId) REFERENCES Election(ElectionId),
)

CREATE TABLE ElectionVote (
	ElectionVoteId int,
	VoterId int,
	ElectionId int,
	CandidateId int,
	PRIMARY KEY (ElectionVoteId),
	FOREIGN KEY (VoterId) REFERENCES Voter(VoterId),
	FOREIGN KEY (ElectionId) REFERENCES Election(ElectionId),
	FOREIGN KEY (CandidateId) REFERENCES Candidate(CandidateId)
)