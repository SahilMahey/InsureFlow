import React, { useEffect, useState } from 'react';
import api from '../api';
import { Box, Paper, Typography } from '@mui/material';
import { PieChart, Pie, Cell, Tooltip, Legend, BarChart, Bar, XAxis, YAxis, CartesianGrid } from 'recharts';

export default function DashboardPage() {
  const [apps, setApps] = useState([]);

  const fetchApplications = async () => {
    const res = await api.get('/applications');
    setApps(res.data);
  };

  useEffect(() => { fetchApplications(); }, []);

  const approved = apps.filter(a => a.status === 'Approved').length;
  const rejected = apps.filter(a => a.status === 'Rejected').length;
  const avgRisk = apps.length ? (apps.reduce((sum, a) => sum + a.riskScore, 0)/apps.length).toFixed(2) : 0;

  const pieData = [
    { name: 'Approved', value: approved },
    { name: 'Rejected', value: rejected },
  ];

  const barData = apps.map(a => ({ name: a.businessName, risk: a.riskScore }));

  const COLORS = ['#4caf50', '#f44336'];

  return (
    <Box padding={3}>
      <Typography variant="h4" gutterBottom>Dashboard</Typography>
      <Box display="flex" gap={3} marginBottom={5}>
        <Paper sx={{ flex:1, padding:2 }}><Typography>Total Applications: {apps.length}</Typography></Paper>
        <Paper sx={{ flex:1, padding:2 }}><Typography>Average Risk: {avgRisk}</Typography></Paper>
        <Paper sx={{ flex:1, padding:2 }}><Typography>Approved: {approved} | Rejected: {rejected}</Typography></Paper>
      </Box>

      <Box display="flex" gap={5}>
        <PieChart width={300} height={300}>
          <Pie data={pieData} dataKey="value" nameKey="name" cx="50%" cy="50%" outerRadius={80} fill="#8884d8" label>
            {pieData.map((entry, index) => <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />)}
          </Pie>
          <Tooltip />
          <Legend />
        </PieChart>

        <BarChart width={500} height={300} data={barData}>
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis dataKey="name" />
          <YAxis />
          <Tooltip />
          <Bar dataKey="risk" fill="#1976d2" />
        </BarChart>
      </Box>
    </Box>
  );
}