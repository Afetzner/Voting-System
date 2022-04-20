DELIMITER $$
DROP PROCEDURE IF EXISTS afetzner.add_ballot $$
DROP PROCEDURE IF EXISTS afetzner.delete_ballot $$
DROP PROCEDURE IF EXISTS afetzner.get_voters_ballot $$
DROP PROCEDURE IF EXISTS afetzner.get_did_voter_participate $$
DROP PROCEDURE IF EXISTS afetzner.get_election_results $$
DROP PROCEDURE IF EXISTS afetzner.check_ballot_serial $$

CREATE PROCEDURE afetzner.add_ballot (
	IN `ballotSerial` varchar(9),  
    IN `voterSerial` varchar(9),
    IN `issueSerial` varchar(9),
    IN `choiceNumber` int,
    OUT `collision` bool)
BEGIN
	-- Detect if ballot from same voter for same issue exists
	SELECT EXISTS (SELECT 1 FROM ballot WHERE voter_serial = `voterSerial` AND issue_serial = `issueSerial` LIMIT 1) INTO `collision`;
    
    INSERT INTO ballot 
		(ballot_serial_number, 
		voter_serial_number,
		issue_serial_number,
		choice_number,
		voter_id,
		issue_id,
		choice_id)
	SELECT
		(`ballotSerial`,
        `voterSerial`,
        `issueSerial`,
        `choiceNumber`,
		(SELECT voter_id FROM voter WHERE voter.serial_number = `voterSerial` LIMIT 1),
        (SELECT issue_id FROM issue WHERE issue.serial_number = `issueSerial` LIMIT 1),
        (SELECT option_id FROM issue_option WHERE issue_option.option_number = `choice_number` LIMIT 1))
	-- Protects against multiple ballots from one voter being entered on any issue
    WHERE NOT `collision`;
END
$$

CREATE PROCEDURE afetzner.delete_ballot (
	IN `ballotSerial` varchar(9))
BEGIN
	DELETE FROM ballot 
    WHERE ballot.ballot_serial_number = `ballotSerial`;
END
$$

-- Get a voters choice for a particual issue
CREATE PROCEDURE afetzner.get_voters_ballot (
	IN `voterSerial` varchar(9),
    IN `issueSerial` varchar(9),
    OUT `ballotSerial` varchar(9),
    OUT `choiceNumber` int,
    OUT `choiceTitle` varchar(127))
BEGIN
	SELECT ballot_serial_number, choice_number, choice_title
    INTO `ballotSerial`, `choiceNumber`, `choiceTitle`
    FROM issue 
		RIGHT JOIN issue_choice ON issue.issue_id = issue_choice.issue_id
		RIGHT JOIN ballot ON ballot.issue_id = issue.issue_id
	WHERE ballot.voter_serial = `voterSerial`
    LIMIT 1;
END
$$
    
-- Return true if a voter participated on a particular issue
CREATE PROCEDURE afetzner.get_did_voter_participate (
	IN `voterSerial` varchar(9),
    IN `issueSerial` varchar(9),
    OUT `didVote` bool)
BEGIN
	SELECT count(*) > 0 INTO `didVote`
    FROM ballot 
    WHERE ballot.voter_serial = `voterSerial` AND ballot.issue_serial = `issueSerial` 
    LIMIT 1;
END
$$

-- Return total voter per option on an issue
CREATE PROCEDURE afetzner.get_election_results (
	IN `issueSerial` varchar(9),
    OUT `count` int)
BEGIN
	SELECT count(ballot_id) INTO `count`
    FROM ballot
    WHERE ballot.issue_serial = `issueSerial`
    GROUP BY ballot.choice_id;
END
$$

-- Returns if a ballot serial number is already in use
CREATE PROCEDURE afetzner.check_ballot_serial (
	IN `ballotSerial` varchar(9)
    OUT `occupied` bool)
BEGIN
	SET `occupied` = EXISTS (SELECT 1 FROM ballot WHERE serial_number = `ballotSerial` LIMIT 1);
END

DELIMITER ;