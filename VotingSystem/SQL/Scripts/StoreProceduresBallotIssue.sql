DELIMITER $$
DROP PROCEDURE IF EXISTS afetzner.add_issue $$
DROP PROCEDURE IF EXISTS afetzner.add_issue_option $$
DROP PROCEDURE IF EXISTS afetzner.delete_issue $$
DROP PROCEDURE IF EXISTS afetzner.delete_issue_option $$
DROP PROCEDURE IF EXISTS afetzner.get_issues $$
DROP PROCEDURE IF EXISTS afetzner.get_options $$
DROP PROCEDURE IF EXISTS afetzner.check_issue_serial $$

CREATE PROCEDURE afetzner.add_issue(
	IN `serialNumber` varchar(9),
    IN `start` DATE,
    IN `end` DATE,
    IN `title` varchar(127),
    IN `description` varchar(255))
BEGIN
	INSERT INTO issue (serial_number, start_date, end_date, title, description)
    SELECT (`serialNumber`, `start`, `end`, `title`, `description`)
    -- Protects against issues with the same title
    WHERE NOT EXISTS (SELECT 1 INTO @collision FROM issue WHERE issue.title = `title`);
END 
$$

CREATE PROCEDURE afetzner.add_issue_option(
	IN `issueSerialNumber` varchar(9),
    IN `title` varchar(127),
    IN `number` int)
BEGIN
	SELECT issue_id INTO @issue_id 
    FROM issue 
    WHERE issue.serial_number = `issueSerialNumber`
    LIMIT 1;
    
	INSERT INTO issue_option(option_number, title, issue_serial, issue_id)
    VALUES (`number`, `title`, `issueSerialNumber`, @issue_id);
END
$$

CREATE PROCEDURE afetzner.delete_issue (
	IN `serialNumber` varchar(9))
BEGIN
	DELETE issue, issue_option
    FROM issue JOIN issue_option ON issue.issue_id = issue_option.issue_id
    WHERE issue.serial_number = `serialNumber`;
END
$$

CREATE PROCEDURE afetzner.delete_issue_option (
	IN `issueSerialNumber` varchar(9),
    IN `optionNumber` int)
BEGIN
	DELETE issue_option
    FROM issue JOIN issue_option ON issue.issue_id = issue_option.issue_id
    WHERE issue.serial_number = `serialNumber`
		AND issue_option.option_number = `optionNumber`;
END
$$

-- Gets all the ballot issues in the DB
CREATE PROCEDURE afetzner.get_issues() 
BEGIN
	SELECT serial_number, start_date, end_date, title, description  
    FROM issue;
END
$$

-- Gets all the ballot options associated with an issue
CREATE PROCEDURE afetzner.get_options(
	IN `serialNumber` varchar(9),
    OUT `count` int)
BEGIN
	SELECT issue_options.option_number, issue_options.title, count(issue_options.option_number) INTO `count`
    FROM issue JOIN issue_options ON issue.issue_id = issue_option.issue_id
    WHERE issue.serial_number = `serialNumber`;
END
$$

CREATE PROCEDURE afetzner.check_issue_serial(
	IN `serialNumber` varchar(9),
    OUT `occupied` bool)
BEGIN
	SELECT count(voter_id) > 0 INTO `occupied` 
    FROM issue 
    WHERE issue.serial_number = `serialNumber`;
END

DELIMITER ;