import { useState } from 'react';
import { login } from '../api';

export default function LoginForm() {
  const [email, setEmail] = useState('');
  const [pass, setPass] = useState('');
  const [error, setError] = useState('');

  const submit = async e => {
    e.preventDefault();
    setError('');
    try {
      const { token } = await login(email, pass);
      localStorage.setItem('token', token);
      alert('Login successful!');
      // window.location = '/'; // yönlendirme eklenebilir
    } catch (err) {
      setError('Login failed');
    }
  };

  return (
    <form onSubmit={submit}>
      <input value={email} onChange={e => setEmail(e.target.value)} placeholder="Email" />
      <input type="password" value={pass} onChange={e => setPass(e.target.value)} placeholder="Password" />
      <button type="submit">Login</button>
      {error && <div style={{color:'red'}}>{error}</div>}
    </form>
  );
}
