DELIMITER $$
DROP PROCEDURE IF EXISTS afetzner.add_ballot $$
DROP PROCEDURE IF EXISTS afetzner.delete_ballot $$
DROP PROCEDURE IF EXISTS afetzner.get_voters_ballot $$
DROP PROCEDURE IF EXISTS afetzner.get_did_voter_participate $$
DROP PROCEDURE IF EXISTS afetzner.get_all_results $$
DROP PROCEDURE IF EXISTS afetzner.get_issue_results $$
DROP PROCEDURE IF EXISTS afetzner.check_ballot_serial $$

CREATE PROCEDURE afetzner.add_ballot (
	IN `v_ballotSerial` varchar(9),  
    IN `v_voterSerial` varchar(9),
    IN `v_issueSerial` varchar(9),
    IN `v_choiceNumber` int,
    OUT `v_collision` bool)
BEGIN
	SET `v_collision` = true;
    START TRANSACTION;
	-- Detect if ballot from same voter for same issue exists
	SET `v_collision` = EXISTS (
		SELECT 1 FROM ballot 
        WHERE ballot_serial_number = `v_ballotSerial`
			OR (voter_serial_number = `v_voterSerial` AND issue_serial_number = `v_issueSerial`) LIMIT 1);
    
    INSERT INTO ballot 
		(ballot_serial_number, 
		voter_serial_number,
		issue_serial_number,
		choice_number,
		voter_id,
		issue_id,
		choice_id)
	SELECT
		`v_ballotSerial`,
        `v_voterSerial`,
        `v_issueSerial`,
        `v_choiceNumber`,
		(SELECT user_id FROM user WHERE serial_number = `v_voterSerial` LIMIT 1),
        (SELECT issue_id FROM issue WHERE serial_number = `v_issueSerial` LIMIT 1),
        (SELECT option_id FROM issue_option WHERE issue_serial = `v_issueSerial` AND option_number = `v_choiceNumber` LIMIT 1)
	-- Protects against multiple ballots from one voter being entered on any issue
    WHERE NOT `v_collision`;
    COMMIT;
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
    OUT `v_isNull` bool)
BEGIN
	SET `v_isNull` = NOT EXISTS (SELECT 1 FROM ballot WHERE voter_serial_number = `v_voterSerial` AND issue_serial_number = `v_issueSerial`);
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
    WHERE ballot.voter_serial_number = `v_voterSerial` AND ballot.issue_serial_number = `v_issueSerial` 
    LIMIT 1;
END
$$

-- Return total voter per option on all issues
CREATE PROCEDURE afetzner.get_all_results ()
BEGIN
	SELECT ops.issue, ops.option, IF(ISNULL(votes.count), 0, votes.count) as 'count'
		FROM (SELECT issue.serial_number as 'issue', issue.issue_id, issue_option.option_number as 'option', issue_option.option_id
			FROM issue RIGHT JOIN issue_option ON issue.issue_id = issue_option.issue_id) ops
		LEFT JOIN (SELECT ballot.choice_id, count(*) AS 'count'
			FROM ballot GROUP BY (ballot.choice_id)) votes
		ON votes.choice_id = ops.option_id;
END
$$

-- Retrun votes per option on a single issue
CREATE PROCEDURE afetzner.get_issue_results (
	IN `v_issueSerial` varchar(9))
BEGIN
	SELECT ops.option, IF(ISNULL(votes.count), 0, votes.count) as 'count'
		FROM (SELECT issue.serial_number as 'issue', issue.issue_id, issue_option.option_number as 'option', issue_option.option_id
			FROM issue RIGHT JOIN issue_option ON issue.issue_id = issue_option.issue_id) ops
		LEFT JOIN (SELECT ballot.choice_id, count(*) AS 'count'
			FROM ballot GROUP BY (ballot.choice_id)) votes
		ON votes.choice_id = ops.option_id
        WHERE ops.issue = `v_issueSerial`;
END
$$

-- Returns if a ballot serial number is already in use
CREATE PROCEDURE afetzner.check_ballot_serial (
	IN `v_ballotSerial` varchar(9),
    OUT `v_occupied` bool)
BEGIN
	SET `v_occupied` = EXISTS (SELECT 1 FROM ballot WHERE ballot_serial_number = `v_ballotSerial` LIMIT 1);
END
$$
DELIMITER ;