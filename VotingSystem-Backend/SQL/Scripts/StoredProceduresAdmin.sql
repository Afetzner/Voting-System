DELIMITER $$
DROP PROCEDURE IF EXISTS afetzner.add_admin $$
DROP PROCEDURE IF EXISTS afetzner.delete_admin $$
DROP PROCEDURE IF EXISTS afetzner.get_admin $$
DROP PROCEDURE IF EXISTS afetzner.check_admin_serial $$

-- Adds an admin
CREATE PROCEDURE afetzner.add_admin (
	IN `v_username` varchar(31),
    IN `v_password` varchar(31),
    IN `v_serialNumber` varchar(9),
    OUT `v_collision` bool)
BEGIN
	START TRANSACTION;
	SET `v_collision` = EXISTS (SELECT 1 FROM user WHERE username = `v_username`);
    
	INSERT INTO user(username, password) 
    SELECT `v_username`, `v_password`
    WHERE NOT `v_collision`;
    
    INSERT INTO admin (user_id, serial_number)
    SELECT LAST_INSERT_ID(), `v_serialNumber`
    WHERE NOT `v_collision`;
    COMMIT;
END
$$

-- removes an admin
CREATE PROCEDURE afetzner.delete_admin (
	IN `v_serialNumber` varchar(9))
BEGIN
	START TRANSACTION;
	DELETE admin, user FROM admin 
    JOIN user ON admin.user_id = user.user_id
    WHERE admin.serial_number = `v_serialNumber`;
    COMMIT;
END
$$

-- Gets an admins's serial number from their login
CREATE PROCEDURE afetzner.get_admin (
	IN `v_username` varchar(31),
    IN `v_password` varchar(31),
    OUT `v_serialNumber` varchar(9))
BEGIN
	SELECT serial_number INTO `v_serialNumber` 
    FROM admin 
    JOIN user ON admin.user_id = user.user_id
    WHERE user.username = `v_username` AND user.password = `v_password`
	LIMIT 1;
END
$$

-- returns true if a serial number is assocaited with an admin
CREATE PROCEDURE afetzner.check_admin_serial (
	IN `v_serialNumber` varchar(9),
    OUT `v_occupied` bool)
BEGIN
	SET `v_occupied` = EXISTS (SELECT 1 FROM admin WHERE serial_number = `v_serialNumber`);
END
$$
DELIMITER ;