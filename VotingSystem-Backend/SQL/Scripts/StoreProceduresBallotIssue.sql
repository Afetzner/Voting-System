DELIMITER $$
DROP PROCEDURE IF EXISTS afetzner.add_issue $$
DROP PROCEDURE IF EXISTS afetzner.add_issue_option $$
DROP PROCEDURE IF EXISTS afetzner.delete_issue $$
DROP PROCEDURE IF EXISTS afetzner.delete_issue_option $$
DROP PROCEDURE IF EXISTS afetzner.get_issues $$
DROP PROCEDURE IF EXISTS afetzner.get_options $$
DROP PROCEDURE IF EXISTS afetzner.check_issue_serial $$

-- Add an issue 
CREATE PROCEDURE afetzner.add_issue(
	IN `v_serialNumber` varchar(9),
    IN `v_start` DATE,
    IN `v_end` DATE,
    IN `v_title` varchar(127),
    IN `v_description` varchar(255),
    OUT `v_collision` bool)
BEGIN
	-- Detect if issue with same title exists
	SET `v_collision` =  EXISTS (SELECT 1 FROM issue WHERE issue.title = `v_title` OR issue.serial_number = `v_serialNumber` LIMIT 1);
	START TRANSACTION;
	INSERT INTO issue (serial_number, start_date, end_date, title, description)
    SELECT `v_serialNumber`, `v_start`, `v_end`, `v_title`, `v_description`
    -- Protects against issues with the same title
    WHERE NOT `v_collision`;
    COMMIT;
END 
$$

-- Add an option to an issue
CREATE PROCEDURE afetzner.add_issue_option(
	IN `v_issueSerialNumber` varchar(9),
    IN `v_title` varchar(127),
    IN `v_number` int,
    OUT `v_collision` bool)
BEGIN
	-- Detect if already exists an option for this issue with same title or number
	SET `v_collision` = EXISTS (SELECT 1 FROM issue_option 
			JOIN issue ON issue_option.issue_id = issue.issue_id 
            WHERE issue.serial_number = `v_issueSerialNumber` AND (issue_option.title = `v_title` OR issue_option.option_number = `v_number`)  LIMIT 1); 
	START TRANSACTION;
	INSERT INTO issue_option 
		(option_number, 
        title, 
        issue_serial, 
        issue_id)
    SELECT
		`v_number`, 
        `v_title`, 
        `v_issueSerialNumber`, 
        (SELECT issue_id FROM issue WHERE issue.serial_number = `v_issueSerialNumber` LIMIT 1)
	WHERE NOT `v_collision`;
    COMMIT;
END
$$

-- Remove an issue and its options
CREATE PROCEDURE afetzner.delete_issue (
	IN `v_serialNumber` varchar(9))
BEGIN
	START TRANSACTION;
    SELECT issue_id INTO @v_issueId FROM issue WHERE issue.serial_number = `v_serialNumber`;
	DELETE FROM ballot WHERE issue_id = @v_issueId;
	DELETE FROM issue_option WHERE issue_id = @v_issueId;
	DELETE FROM issue WHERE issue_id = @V_issueId;
	COMMIT;
END
$$

-- Remove an option from a issue (also ballots for that option)
CREATE PROCEDURE afetzner.delete_issue_option (
	IN `v_issueSerialNumber` varchar(9),
    IN `v_optionNumber` int)
BEGIN
	START TRANSACTION;
	DELETE ballot, issue_option
    FROM issue 
    JOIN issue_option ON issue.issue_id = issue_option.issue_id
    JOIN ballot ON issue_option.option_id = ballot.option_id
    WHERE issue.serial_number = `v_serialNumber`
		AND issue_option.option_number = `v_optionNumber`;
	COMMIT;
END
$$

-- Gets all the ballot issues in the DB
CREATE PROCEDURE afetzner.get_issues()
BEGIN
	SELECT serial_number, start_date, end_date, title, description  
    FROM issue;
END
$$

-- Gets all the ballot options associated with an issue, also number of options
CREATE PROCEDURE afetzner.get_options(
	IN `v_serialNumber` varchar(9)
    )
BEGIN
	SELECT issue_option.option_number, issue_option.title 
    FROM issue JOIN issue_option ON issue.issue_id = issue_option.issue_id
    WHERE issue.serial_number = `v_serialNumber`;
END
$$

-- Returns if an issue serial number is already in use
CREATE PROCEDURE afetzner.check_issue_serial(
	IN `v_serialNumber` varchar(9),
    OUT `v_occupied` bool)
BEGIN
	SET `v_occupied` = EXISTS (SELECT 1 FROM issue WHERE issue.serial_number = `v_serialNumber`);
END
$$ 
DELIMITER ;