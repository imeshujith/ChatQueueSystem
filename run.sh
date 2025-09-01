#!/bin/bash

# Set default OFFICE_END_HOUR if not provided
: ${OFFICE_END_HOUR:=23:00:00}

# Delete the database file if it exists
DB_FILE="src/ChatQueueSystem.API/chatqueue.db"
if [ -f "$DB_FILE" ]; then
	echo "Deleting existing database: $DB_FILE"
	rm "$DB_FILE"
fi

# Clean, restore, reset database, build, and run the project
set -e

dotnet clean

dotnet restore

cd src

cd ChatQueueSystem.API

dotnet build

OFFICE_END_HOUR=$OFFICE_END_HOUR dotnet run