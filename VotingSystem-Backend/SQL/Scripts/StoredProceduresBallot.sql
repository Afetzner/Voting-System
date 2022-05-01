DELIMITER $$
DROP PROCEDURE IF EXISTS afetzner.add_ballot $$
DROP PROCEDURE IF EXISTS afetzner.delete_ballot $$
DROP PROCEDURE IF EXISTS afetzner.get_voters_ballot $$
DROP PROCEDURE IF EXISTS afetzner.get_did_voter_participate $$
DROP PROCEDURE IF EXISTS afetzner.get_election_results $$
DROP PROCEDURE IF EXISTS afetzner.check_ballot_serial $$

CREATE PROCEDURE afetzner.add_ballot (
	IN `v_ballotSerial` varchar(9),  
    IN `v_voterSerial` varchar(9),
    IN `v_issueSerial` varchar(9),
    IN `v_choiceNumber` int,
    OUT `v_collision` bool)
BEGIN
	-- Detect if ballot from same voter for same issue exists
	SELECT EXISTS (SELECT 1 FROM ballot WHERE voter_serial = `v_voterSerial` AND issue_serial = `v_issueSerial` LIMIT 1) INTO `v_collision`;
    
    INSERT INTO ballot 
		(ballot_serial_number, 
		voter_serial_number,
		issue_serial_number,
		choice_number,
		voter_id,
		issue_id,
		choice_id)
	SELECT
		(`v_ballotSerial`,
        `v_voterSerial`,
        `v_issueSerial`,
        `v_choiceNumber`,
		(SELECT voter_id FROM voter WHERE voter.serial_number = `v_voterSerial` LIMIT 1),
        (SELECT issue_id FROM issue WHERE issue.serial_number = `v_issueSerial` LIMIT 1),
        (SELECT option_id FROM issue_option WHERE issue_option.option_number = `v_choice_number` LIMIT 1))
	-- Protects against multiple ballots from one voter being entered on any issue
    WHERE NOT `v_collision`;
END
$$

CREATE PROCEDURE afetzner.delete_ballot (
	IN `v_ballotSerial` varchar(9))
BEGIN
	DELETE FROM ballot 
    WHERE ballot.ballot_serial_number = `v_ballotSerial`;
END
$$

-- Get a voters choice for a particual issue
CREATE PROCEDURE afetzner.get_voters_ballot (
	IN `v_voterSerial` varchar(9),
    IN `v_issueSerial` varchar(9),
    OUT `v_ballotSerial` varchar(9),
    OUT `v_choiceNumber` int,
    OUT `v_choiceTitle` varchar(127),
    OUT `v_didVote` bool)
BEGIN
	SET `v_didVote` = EXISTS (SELECT 1 FROM ballot WHERE voter_serial_number = `v_voterSerial` AND issue_serial_number = `v_issueSerial`);
	SELECT ballot_serial_number, choice_number, issue_option.title
    INTO `v_ballotSerial`, `v_choiceNumber`, `v_choiceTitle`
    FROM ballot 
		LEFT JOIN issue_option ON ballot.issue_id = issue_option.issue_id
		LEFT JOIN issue ON ballot.issue_id = issue.issue_id
	WHERE ballot.voter_serial_number = `v_voterSerial` AND ballot.issue_serial_number = `v_issueSerial`
    LIMIT 1;
END
$$
    
-- Return true if a voter participated on a particular issue
CREATE PROCEDURE afetzner.get_did_voter_participate (
	IN `v_voterSerial` varchar(9),
    IN `v_issueSerial` varchar(9),
    OUT `v_didVote` bool)
BEGIN
	SELECT count(*) > 0 INTO `v_didVote`
    FROM ballot 
    WHERE ballot.voter_serial = `v_voterSerial` AND ballot.issue_serial = `v_issueSerial` 
    LIMIT 1;
END
$$

-- Return total voter per option on an issue
CREATE PROCEDURE afetzner.get_election_results (
	IN `v_issueSerial` varchar(9),
    OUT `v_count` int)
BEGIN
	SELECT count(ballot_id) INTO `v_count`
    FROM ballot
    WHERE ballot.issue_serial = `v_issueSerial`
    GROUP BY ballot.choice_id;
END
$$

-- Returns if a ballot serial number is already in use
CREATE PROCEDURE afetzner.check_ballot_serial (
	IN `v_ballotSerial` varchar(9),
    OUT `v_occupied` bool)
BEGIN
	SET `v_occupied` = EXISTS (SELECT 1 FROM ballot WHERE serial_number = `v_ballotSerial` LIMIT 1);
END
$$
DELIMITER ;