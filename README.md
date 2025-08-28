# ChatQueueSystem

A .NET-based API for managing chat queues, agents, and teams.

## Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- macOS, Linux, or Windows

## Setup Steps

### 1. Clone the Repository
```
git clone https://github.com/imeshujith/ChatQueueSystem.git
cd ChatQueueSystem
```

### 2. Restore Dependencies
```
dotnet restore
```

### 3. Remove Existing Sample Data (Optional)
If you want to start with a clean database, delete the existing database file:
```
rm ChatQueueSystem.API/chatqueue.db
```

### 4. Run Database Migrations
```
dotnet ef database update --project ChatQueueSystem.Infrastructure --startup-project ChatQueueSystem.API
```

### 5. Run the API
```
cd ChatQueueSystem.API
dotnet run
```
The API will start, usually at `http://localhost:8008`.


### 6. Run Unit Tests
To run all unit tests:
```
dotnet test
```

### 7. Test the API (Optional)
You can use the provided shell script to test the API endpoints:
```
bash test_chat_system.sh
```
Or use tools like Postman or curl to interact with the endpoints.

## Notes
- If you encounter `Queue is full. Chat refused.`, ensure the database is empty or increase queue limits in your code.

## Project Structure
- `ChatQueueSystem.API/` - Main API project
- `ChatQueueSystem.Application/` - Application logic
- `ChatQueueSystem.Domain/` - Domain entities and interfaces
- `ChatQueueSystem.Infrastructure/` - Data access and infrastructure
