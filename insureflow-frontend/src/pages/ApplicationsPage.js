import React, { useState, useEffect } from 'react';
import api from '../api';
import { Box, TextField, Button, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from '@mui/material';

export default function ApplicationsPage({role}) {
  const [applications, setApplications] = useState([]);
  const [form, setForm] = useState({
    businessName: '', businessType: '', annualRevenue: 0,
    employees: 0, yearsInBusiness: 0, claimsHistory: 0
  });

  const fetchApplications = async () => {
    const res = await api.get('/applications');
    setApplications(res.data);
  };

  useEffect(() => { fetchApplications(); }, []);

 const handleSubmit = async (e) => {
  e.preventDefault();
  try {
   await api.post('/applications', {
  ...form,
  Decision: 'Pending'
});
    fetchApplications();
  } catch (err) {
    console.error(err.response?.data || err.message);
    alert('Failed to create application: ' + (err.response?.data?.title || err.message));
  }
};


  const handlePremium = async (id) => {
    const res = await api.get(`/applications/${id}/premium`);
    alert(`Premium: ${res.data.premium}`);
  };

  const handleIssue = async (id) => {
    const res = await api.post(`/applications/${id}/issue`);
    alert(`Policy Issued: ${res.data.policyNumber}`);
    fetchApplications();
  };

  return (
    <Box padding={3}>
      <h2>Applications</h2>

      {/* Only Admin/Agent can create */}
      {(role === 'Admin' || role === 'Agent') && (
        <Box component="form" onSubmit={handleSubmit} display="flex" flexWrap="wrap" gap={2} marginBottom={3}>
          <TextField label="Business Name" value={form.businessName} onChange={e=>setForm({...form, businessName:e.target.value})} />
          <TextField label="Business Type" value={form.businessType} onChange={e=>setForm({...form, businessType:e.target.value})} />
          <TextField label="Annual Revenue" type="number" value={form.annualRevenue} onChange={e=>setForm({...form, annualRevenue:e.target.value})} />
          <TextField label="Employees" type="number" value={form.employees} onChange={e=>setForm({...form, employees:e.target.value})} />
          <TextField label="Years in Business" type="number" value={form.yearsInBusiness} onChange={e=>setForm({...form, yearsInBusiness:e.target.value})} />
          <TextField label="Claims History" type="number" value={form.claimsHistory} onChange={e=>setForm({...form, claimsHistory:e.target.value})} />
          <Button
  type="submit"
  variant="contained"
  color="primary"
  disabled={!form.businessName || !form.businessType || form.annualRevenue <= 0}
>
  Create Application
</Button>
        </Box>
      )}

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Business</TableCell>
              <TableCell>Type</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>Risk</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {applications.map(a => (
              <TableRow key={a.id}>
                <TableCell>{a.id}</TableCell>
                <TableCell>{a.businessName}</TableCell>
                <TableCell>{a.businessType}</TableCell>
                <TableCell>{a.status}</TableCell>
                <TableCell>{a.riskScore}</TableCell>
                <TableCell>
                  {(role === 'Admin' || role === 'Agent') && (
                    <>
                   
                      <Button size="small" onClick={()=>handlePremium(a.id)}>Premium</Button>
                      <Button size="small" onClick={()=>handleIssue(a.id)}>Issue</Button>
                    </>
                  )}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
}