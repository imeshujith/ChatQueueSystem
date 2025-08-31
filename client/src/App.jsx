import React, { useState } from 'react';

function ErrorBoundary({ children }) {
  const [error, setError] = useState(null);
  if (error) {
    return <div style={{ color: 'red' }}>Error: {error.message || error.toString()}</div>;
  }
  return (
    <React.Fragment>
      {React.Children.map(children, child => {
        try {
          return child;
        } catch (e) {
          setError(e);
          return null;
        }
      })}
    </React.Fragment>
  );
}
import './App.css';
import SupportRequestForm from './components/SupportRequestForm';
import ChatSession from './components/ChatSession';
import QueueStatus from './components/QueueStatus';


function App() {
  const [sessionId, setSessionId] = useState(null);
  const [queueLength, setQueueLength] = useState(0);
  const [maxQueue, setMaxQueue] = useState(24);
  const [overflow, setOverflow] = useState(true);
  const [overflowActive, setOverflowActive] = useState(false);
  const [assignment, setAssignment] = useState(null);
  const [refusedReason, setRefusedReason] = useState("");

  const fetchQueueStatus = async () => {
    try {
      const res = await fetch('http://localhost:8008/api/v1/chats/status');
      if (res.ok) {
        const data = await res.json();
        setQueueLength(data.queueLength || 0);
        setMaxQueue(data.maxQueue || 24);
        setOverflow(data.overflowEnabled ?? true);
        setOverflowActive(data.overflowActive ?? false);
      }
    } catch {}
  };

  const handleSupportRequest = async (userId) => {
    setRefusedReason("");
    try {
      const res = await fetch('http://localhost:8008/api/v1/chats', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ userId }),
      });
      if (res.ok) {
        const data = await res.json();
        if (data.status === 'ok' || data.sessionId) {
          setSessionId(data.sessionId || data.id);
          setAssignment(data.assignment || null);
        } else if (data.status === 'refused') {
          setRefusedReason(data.reason || 'Chat refused.');
        }
        fetchQueueStatus();
      } else {
        setRefusedReason('Failed to create support request.');
      }
    } catch {
      setRefusedReason('Failed to create support request.');
    }
  };

  const handleInactive = () => {
    setSessionId(null);
    setAssignment(null);
    setRefusedReason("");
    fetchQueueStatus();
  };

  React.useEffect(() => {
    fetchQueueStatus();
  }, []);

  return (
    <div className="App">
      <h1>Support Chat Demo</h1>
      <QueueStatus
        queueLength={queueLength}
        maxQueue={maxQueue}
        overflow={overflow}
        overflowActive={overflowActive}
      />
      {refusedReason && (
        <div style={{ color: 'red', marginBottom: 8 }}>{refusedReason}</div>
      )}
      <ErrorBoundary>
        {!sessionId ? (
          <>
            <div style={{ color: 'blue', marginBottom: 8 }}>Debug: Rendering SupportRequestForm</div>
            <SupportRequestForm onRequest={handleSupportRequest} />
          </>
        ) : (
          <ChatSession sessionId={sessionId} onInactive={handleInactive} assignment={assignment} />
        )}
      </ErrorBoundary>
    </div>
  );
}

export default App;
