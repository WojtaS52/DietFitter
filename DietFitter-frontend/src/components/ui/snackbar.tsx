import React, { useState } from 'react';
import { Snackbar, Alert, AlertColor } from '@mui/material';

interface SnackbarProps {
  message: string;
  severity?: AlertColor; 
  duration?: number;
  onClose?: () => void; 
}

const CustomSnackbar: React.FC<SnackbarProps> = ({
  message,
  severity = 'info',
  duration = 6000,
  onClose,
}) => {
  const [open, setOpen] = useState(true);

  const handleClose = (_event?: React.SyntheticEvent | Event, reason?: string) => {
    if (reason === 'clickaway') return;
    setOpen(false);
    if (onClose) onClose(); 
  };

  return (
    <Snackbar
      open={open}
      autoHideDuration={duration}
      onClose={handleClose}
      anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
    >
      <Alert
        onClose={handleClose}
        severity={severity}
        variant="filled"
        sx={{ width: '100%' }}
      >
        {message}
      </Alert>
    </Snackbar>
  );
};

export default CustomSnackbar;