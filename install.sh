#!/bin/bash

GREEN="\033[0;32m"
NO_COLOUR="\033[0m"

function finished () { echo -e "${GREEN}DONE${NO_COLOUR}"; }

echo -n "Creating connect4 user... "
sudo mysql --execute="CREATE USER 'connect4'@'localhost' IDENTIFIED BY 'connect4';"
finished

echo -n "Creating connect4 database... "
sudo mysql --execute="CREATE DATABASE IF NOT EXISTS connect4;"
finished

echo -n "Granting the required privileges to the connect4 user... "
sudo mysql --execute="GRANT ALL PRIVILEGES ON connect4.* TO 'connect4'@'localhost';"
sudo mysql --execute="FLUSH PRIVILEGES;"
finished

echo -n "Creating the user table... "
sudo mysql --user=connect4 --password=connect4 --execute="USE connect4; DROP TABLE IF EXISTS users; 
                                                          CREATE TABLE users(
                                                              id INT NOT NULL AUTO_INCREMENT,
                                                              name VARCHAR(50) NOT NULL,
                                                              password VARCHAR(50) NOT NULL,
                                                              games_played INT DEFAULT 0,
                                                              games_won INT DEFAULT 0,
                                                              games_lost INT DEFAULT 0,
                                                              PRIMARY KEY (id));"
finished

dotnet publish --configuration Release
