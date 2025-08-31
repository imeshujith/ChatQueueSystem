#!/bin/bash

# Set default OFFICE_END_HOUR if not provided
: ${OFFICE_END_HOUR:=23:00:00}

# Clean, restore, reset database, build, and run the project
set -e

dotnet clean

dotnet restore

cd src

cd ChatQueueSystem.API

dotnet build

OFFICE_END_HOUR=$OFFICE_END_HOUR dotnet run