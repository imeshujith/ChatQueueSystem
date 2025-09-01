import { useState } from 'react';

export default function SupportRequestForm({ onRequest }) {
  const [username, setUsername] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      await onRequest(username);
    } catch (err) {
      setError('Failed to create support request.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} style={{ marginBottom: 16 }}>
      <input
        type="text"
        placeholder="Enter your name"
        value={username}
        onChange={e => setUsername(e.target.value)}
        required
        disabled={loading}
      />
      <button type="submit" disabled={loading || !username}>
        {loading ? 'Requesting...' : 'Start Support Chat'}
      </button>
      {error && <div style={{ color: 'red' }}>{error}</div>}
    </form>
  );
}
