import React from 'react';
import { Navigate } from 'react-router-dom';

export default function ProtectedRoute({ children, token, allowedRoles, role, redirectTo }) {
  if (!token) return <Navigate to={redirectTo || '/'} />;
  if (!allowedRoles.includes(role)) return <Navigate to={redirectTo || '/'} />;
  return children;
}