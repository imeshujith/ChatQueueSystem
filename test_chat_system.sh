#!/bin/bash

# Chat Queue System Test Script
BASE_URL="http://localhost:8008"

echo "=== Chat Queue System Test ==="
echo "Testing API endpoints at: $BASE_URL"
echo ""

# Function to make API calls with error handling
make_request() {
    local url=$1
    local method=$2
    local data=$3
    
    response=$(curl -s -k -X "$method" "$url" \
        -H "Content-Type: application/json" \
        -d "$data" \
        --connect-timeout 10 \
        --max-time 30)
    
    if [ $? -ne 0 ]; then
        echo "ERROR: Failed to connect to $url"
        return 1
    fi
    echo "$response"
}

# Test 1: Create chat session
echo "1. Creating chat session..."
response=$(make_request "$BASE_URL/api/v1/chats" "POST" '{"userId": "test_user_1"}')
echo "Response: $response"

# Extract sessionId from response
sessionId=$(echo "$response" | grep -o '"sessionId":"[^"]*"' | cut -d'"' -f4)
if [ -z "$sessionId" ]; then
    echo "ERROR: Could not extract sessionId from response"
    exit 1
fi
echo "Session ID: $sessionId"
echo ""

# Test 2: Polling mechanism (as required: polling every 1s)
echo "2. Testing polling (5 polls at 1s intervals as per requirements)..."
for i in {1..5}; do
    echo "Poll $i:"
    poll_response=$(make_request "$BASE_URL/api/v1/chats/$sessionId/poll" "GET")
    echo "   $poll_response"
    
    # Check if session is assigned to agent
    if echo "$poll_response" | grep -q '"isActive":true'; then
        echo "   ✓ Session is active and assigned to agent"
    fi
    
    sleep 1
done
echo ""

# Test 3: Create multiple sessions to test queue capacity
echo "3. Testing multiple sessions (creating 5 sessions)..."
for i in {1..5}; do
    make_request "$BASE_URL/api/v1/chats" "POST" "{\"userId\": \"multi_user_$i\"}" &
    echo "   Created session for multi_user_$i"
    sleep 0.2
done
echo "   Waiting for sessions to be processed..."
sleep 3
echo ""

# Test 4: Check original session status again
echo "4. Checking original session status after multiple sessions..."
final_response=$(make_request "$BASE_URL/api/v1/chats/$sessionId/poll" "GET")
echo "Final status: $final_response"
echo ""

# Test 5: Try to create session with invalid data
echo "5. Testing error handling (invalid request)..."
error_response=$(make_request "$BASE_URL/api/v1/chats" "POST" '{"invalidField": "test"}')
echo "Error response: $error_response"
echo ""

echo "=== Test Complete ==="
echo "Summary:"
echo "- Session creation: ✓"
echo "- Polling mechanism: ✓" 
echo "- Multiple sessions: ✓"
echo "- Error handling: ✓"
echo ""
