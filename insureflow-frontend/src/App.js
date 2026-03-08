import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Link, Navigate } from 'react-router-dom';
import ApplicationsPage from './pages/ApplicationsPage';
import RulesPage from './pages/RulesPage';
import DashboardPage from './pages/DashboardPage';
import LoginPage from './pages/LoginPage';
import ProtectedRoute from './components/ProtectedRoute'; // <-- new
import { AppBar, Toolbar, Typography, Button } from '@mui/material';
import api from './api';

function App() {
  const [token, setToken] = useState(localStorage.getItem('token') || '');
  const [role, setRole] = useState(localStorage.getItem('role') || '');

  useEffect(() => {
    if(token){
      api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    }
  }, [token]);

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('role');
    setToken('');
    setRole('');
  };

  return (
    <Router>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" sx={{ flexGrow: 1 }}>InsureFlow</Typography>

          {token && (
            <>
              <Typography sx={{ marginRight: 2 }}>Role: {role}</Typography>

              {role === 'Admin' && (
                <>
                  <Button color="inherit" component={Link} to="/applications">Applications</Button>
                  <Button color="inherit" component={Link} to="/rules">Rules</Button>
                  <Button color="inherit" component={Link} to="/dashboard">Dashboard</Button>
                </>
              )}

              {role === 'Agent' && (
                <Button color="inherit" component={Link} to="/applications">Applications</Button>
              )}

              {role === 'Underwriter' && (
                <Button color="inherit" component={Link} to="/rules">Rules</Button>
              )}

              <Button color="inherit" onClick={handleLogout}>Logout</Button>
            </>
          )}
        </Toolbar>
      </AppBar>

      <Routes>
        {!token ? (
          <Route path="*" element={<LoginPage setToken={setToken} setRole={setRole} />} />
        ) : (
          <>
            {/* Applications Page */}
            <Route path="/applications" element={
              <ProtectedRoute token={token} role={role} allowedRoles={['Admin','Agent']} redirectTo="/">
                <ApplicationsPage role={role} />
              </ProtectedRoute>
            } />

            {/* Rules Page */}
            <Route path="/rules" element={
              <ProtectedRoute token={token} role={role} allowedRoles={['Admin','Underwriter']} redirectTo="/">
                <RulesPage role={role} />
              </ProtectedRoute>
            } />

            {/* Dashboard Page */}
            <Route path="/dashboard" element={
              <ProtectedRoute token={token} role={role} allowedRoles={['Admin']} redirectTo="/">
                <DashboardPage />
              </ProtectedRoute>
            } />

            {/* Default fallback */}
            <Route path="*" element={<Navigate to={
              role === 'Admin' ? '/applications' : role === 'Agent' ? '/applications' : '/rules'
            } />} />
          </>
        )}
      </Routes>
    </Router>
  );
}

export default App;