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
	IN `issueSerialNumber` varchar(9),
    IN `start` DATE,
    IN `end` DATE,
    IN `title` varchar(127),
    IN `description` varchar(255),
    OUT `collision` bool)
BEGIN
	-- Detect if issue with same title exists
	SET `collision` =  EXISTS (SELECT 1 FROM issue WHERE issue.title = `title` LIMIT 1);

	INSERT INTO issue (serial_number, start_date, end_date, title, description)
    SELECT (`serialNumber`, `start`, `end`, `title`, `description`)
    -- Protects against issues with the same title
    WHERE NOT `collision`;
END 
$$

-- Add an option to an issue
CREATE PROCEDURE afetzner.add_issue_option(
	IN `issueSerialNumber` varchar(9),
    IN `title` varchar(127),
    IN `number` int,
    OUT `collision` bool)
BEGIN
	-- Detect if already exists an option for this issue with same title or number
	SET `collision` = EXISTS (SELECT 1 FROM issue_option 
			JOIN issue ON issue_option.issue_id = issue.issue_id 
            WHERE issue.serial_number = `issueSerialNumber` AND (issue_option.title = `title` OR issue_option.option_number = `number`)  LIMIT 1); 
            
	INSERT INTO issue_option 
		(option_number, 
        title, 
        issue_serial, 
        issue_id)
    SELECT
		(`number`, 
        `title`, 
        `issueSerialNumber`, 
        (SELECT issue_id FROM issue WHERE issue.serial_number = `issueSerialNumber` LIMIT 1))
	WHERE NOT `collision`;
END
$$

-- Remove an issue and its options
CREATE PROCEDURE afetzner.delete_issue (
	IN `serialNumber` varchar(9))
BEGIN
	DELETE issue, issue_option
    FROM issue JOIN issue_option ON issue.issue_id = issue_option.issue_id
    WHERE issue.serial_number = `serialNumber`;
END
$$

-- Remove an option from a issue (also ballots for that option)
CREATE PROCEDURE afetzner.delete_issue_option (
	IN `issueSerialNumber` varchar(9),
    IN `optionNumber` int)
BEGIN
	DELETE issue_option, ballot
    FROM issue 
    JOIN issue_option ON issue.issue_id = issue_option.issue_id
    JOIN ballot ON issue_option.option_id = ballot.option_id
    WHERE issue.serial_number = `serialNumber`
		AND issue_option.option_number = `optionNumber`;
END
$$

-- Gets all the ballot issues in the DB
CREATE PROCEDURE afetzner.get_issues(
	OUT `serialNumber` varchar(9),
    OUT `start` DATE,
    OUT `end` DATE,
    OUT `title` varchar(127),
    OUT `description` varchar(255))
BEGIN
	SELECT serial_number, start_date, end_date, title, description  
    INTO `serialNumber`, `start`, `end`, `title`, `description`
    FROM issue;
END
$$

-- Gets all the ballot options associated with an issue, also number of options
CREATE PROCEDURE afetzner.get_options(
	IN `serialNumber` varchar(9),
    OUT `number` int,
    OUT `title` varchar(127),
    OUT `count` int)
BEGIN
	SELECT issue_options.option_number, issue_options.title INTO `number`, `title`
    FROM issue JOIN issue_option ON issue.issue_id = issue_option.issue_id
    WHERE issue.serial_number = `serialNumber`;
    
    SELECT count(*) INTO `count`
    FROM issue_option 
    WHERE issue_option.serial_number = `serialNumber`;
END
$$

-- Returns if an issue serial number is already in use
CREATE PROCEDURE afetzner.check_issue_serial(
	IN `serialNumber` varchar(9),
    OUT `occupied` bool)
BEGIN
	SET `occupied` = EXISTS (SELECT 1 FROM issue WHERE issue.serial_number = `serialNumber`);
END

DELIMITER ;