import React, { useState } from 'react';
import api from '../api';
import { useNavigate } from 'react-router-dom';
import { Box, TextField, Button, Typography } from '@mui/material';

export default function LoginPage({ setToken, setRole }) {
  const [form, setForm] = useState({ username: '', password: '' });
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const res = await api.post('/auth/login', form);
      localStorage.setItem('token', res.data.token); // save token
      localStorage.setItem('role', res.data.role); // save role
      setToken(res.data.token);
      setRole(res.data.role);
      navigate('/applications'); // redirect after login
    } catch (err) {
      const errors = err.response?.data?.errors;
if(errors){
  const messages = Object.values(errors).flat().join(', ');
  setError(messages);
} else {
  setError(err.response?.data?.title || 'Login failed');
}
    }
  };

  return (
    <Box padding={5} maxWidth={400} margin="auto">
      <Typography variant="h4" gutterBottom>Login</Typography>
      {error && <Typography color="error">{error}</Typography>}
      <Box component="form" onSubmit={handleLogin} display="flex" flexDirection="column" gap={2}>
        <TextField label="Username" value={form.username} onChange={e=>setForm({...form, username:e.target.value})} />
        <TextField type="password" label="Password" value={form.password} onChange={e=>setForm({...form, password:e.target.value})} />
        <Button type="submit" variant="contained" color="primary">Login</Button>
      </Box>
    </Box>
  );
}