#!/bin/bash
dotnet clean && dotnet restore && dotnet build && dotnet test && dotnet format && dotnet run --project ChatQueueSystem.API/ChatQueueSystem.API.csproj