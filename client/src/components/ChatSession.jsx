import { useEffect, useRef, useState } from 'react';

export default function ChatSession({ sessionId, onInactive }) {
  const [status, setStatus] = useState('pending');
  const [messages, setMessages] = useState([]);
  const [input, setInput] = useState('');
  const pollCount = useRef(0);
  const pollInterval = useRef(null);

  useEffect(() => {
    pollInterval.current = setInterval(async () => {
      try {
        // Replace with your backend polling endpoint
  const res = await fetch(`http://localhost:8008/api/v1/chats/${sessionId}/poll`);
        if (res.ok) {
          const data = await res.json();
          setStatus(data.status);
          setMessages(data.messages || []);
          pollCount.current = 0;
        } else {
          pollCount.current += 1;
          if (pollCount.current >= 3) {
            clearInterval(pollInterval.current);
            onInactive();
          }
        }
      } catch {
        pollCount.current += 1;
        if (pollCount.current >= 3) {
          clearInterval(pollInterval.current);
          onInactive();
        }
      }
    }, 1000);
    return () => clearInterval(pollInterval.current);
  }, [sessionId, onInactive]);

  const sendMessage = async (e) => {
    e.preventDefault();
    if (!input) return;
    // Replace with your backend send message endpoint
  await fetch(`http://localhost:8008/api/v1/chats/${sessionId}/message`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ message: input }),
    });
    setInput('');
  };

  if (status === 'refused') return <div>Chat refused. Please try again later.</div>;
  if (status === 'inactive') return <div>Session inactive.</div>;

  return (
    <div style={{ border: '1px solid #ccc', padding: 16 }}>
      <div>Status: {status}</div>
      <div style={{ minHeight: 100, margin: '8px 0' }}>
        {messages.map((msg, i) => (
          <div key={i}>{msg}</div>
        ))}
      </div>
      {status === 'active' && (
        <form onSubmit={sendMessage}>
          <input
            value={input}
            onChange={e => setInput(e.target.value)}
            placeholder="Type your message..."
          />
          <button type="submit">Send</button>
        </form>
      )}
    </div>
  );
}
