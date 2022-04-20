DELIMITER $$
DROP PROCEDURE IF EXISTS afetzner.add_admin $$
DROP PROCEDURE IF EXISTS afetzner.delete_admin $$
DROP PROCEDURE IF EXISTS afetzner.get_admin_serial $$
DROP PROCEDURE IF EXISTS afetzner.check_admin_serial $$

-- Adds an admin
CREATE PROCEDURE afetzner.add_admin (
	IN `username` varchar(31),
    IN `password` varchar(31),
    OUT `collision` bool)
BEGIN
	SET `collision` = EXISTS (SELECT 1 FROM user WHERE username = `username`);
    
	INSERT INTO User(username, password) 
    SELECT (`username`, `password`)
    WHERE NOT `collision`;
END
$$

-- removes an admin
CREATE PROCEDURE afetzner.delete_admin (
	IN `serialNumber` varchar(9))
BEGIN
	DELETE admin, user FROM admin 
    JOIN user ON admin.user_id = user.user_id
    WHERE admin.serial_number = `serialNumber`;
END
$$

-- Gets an admins's serial number from their login
CREATE PROCEDURE afetzner.get_admin_serial (
	IN `username` varchar(31),
    IN `password` varchar(31),
    OUT `serialNumber` varchar(9))
BEGIN
	SELECT serial_number INTO `serialNumber` 
    FROM admin
    WHERE username = `username` AND password = `password`
	LIMIT 1;
END
$$

-- returns true if a serial number is assocaited with an admin
CREATE PROCEDURE afetzner.check_admin_serial (
	IN `serialNumber` varchar(9),
    OUT `occupied` bool)
BEGIN
	SET `occupied` = EXISTS (SELECT 1 FROM admin WHERE serial_number = `serialNumber`);
END

DELIMITER ;