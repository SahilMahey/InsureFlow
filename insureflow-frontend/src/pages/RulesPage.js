import React, { useState, useEffect } from 'react';
import api from '../api';
import { Box, TextField, Button, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from '@mui/material';

export default function RulesPage({role}) {
  const [rules, setRules] = useState([]);
  const [form, setForm] = useState({ field: '', operator: '', value: '', riskPoints: 0 });

  const fetchRules = async () => {
    const res = await api.get('/rules');
    setRules(res.data);
  };

  useEffect(() => { fetchRules(); }, []);

  const handleSubmit = async e => {
    e.preventDefault();
    await api.post('/rules', form);
    setForm({ field: '', operator: '', value: '', riskPoints: 0 });
    fetchRules();
  };

  const handleDelete = async (id) => {
    await api.delete(`/rules/${id}`);
    fetchRules();
  };



  return (
    <Box padding={3}>
      <h2>Underwriting Rules</h2>

      {(role === 'Admin' || role === 'Underwriter') && (
        <Box component="form" onSubmit={handleSubmit} display="flex" flexWrap="wrap" gap={2} marginBottom={3}>
          <TextField label="Field" value={form.field} onChange={e => setForm({ ...form, field: e.target.value })} />
          <TextField label="Operator" value={form.operator} onChange={e => setForm({ ...form, operator: e.target.value })} />
          <TextField label="Value" value={form.value} onChange={e => setForm({ ...form, value: e.target.value })} />
          <TextField label="Risk Points" type="number" value={form.riskPoints} onChange={e => setForm({ ...form, riskPoints: Number(e.target.value) })} />
            <Button
  type="submit"
  variant="contained"
  color="primary"
  disabled={!form.field || !form.operator || !form.value || form.riskPoints < 0}
>
  Add Rule
</Button>
        </Box>
      )}

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Field</TableCell>
              <TableCell>Operator</TableCell>
              <TableCell>Value</TableCell>
              <TableCell>Points</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {rules.map(r => (
              <TableRow key={r.id}>
                <TableCell>{r.id}</TableCell>
                <TableCell>{r.field}</TableCell>
                <TableCell>{r.operator}</TableCell>
                <TableCell>{r.value}</TableCell>
                <TableCell>{r.riskPoints}</TableCell>
                <TableCell>
                  {(role === 'Admin' || role === 'Underwriter') && (
                    <>
                    
                      <Button size="small" color="secondary" onClick={() => handleDelete(r.id)}>Delete</Button>
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