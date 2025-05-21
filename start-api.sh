#!/bin/bash

# Start the API server in the background
cd TestTube
dotnet run &

# Store the process ID
API_PID=$!

# Wait for the API to start (adjust sleep time as needed)
echo "Starting API server..."
sleep 10
echo "API server started with PID: $API_PID"

# Run the tests
cd ..
dotnet test TestTube.Tests/TestTube.Tests.csproj

# Kill the API server when done
kill $API_PID
echo "API server stopped"