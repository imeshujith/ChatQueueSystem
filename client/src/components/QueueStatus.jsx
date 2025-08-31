export default function QueueStatus({ queueLength, maxQueue, overflow, overflowActive }) {
  return (
    <div style={{ margin: '16px 0' }}>
      <div>Queue: {queueLength} / {maxQueue}</div>
      {overflow && (
        <div style={{ color: overflowActive ? 'green' : 'gray' }}>
          Overflow {overflowActive ? 'Active' : 'Inactive'}
        </div>
      )}
    </div>
  );
}
