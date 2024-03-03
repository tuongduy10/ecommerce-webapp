import * as React from 'react';
import Button from '@mui/material/Button';
import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';
import { useAlertStore } from 'src/_cores/_store/root-store';
import { useDispatch } from 'react-redux';
import { hideAlert } from 'src/_cores/_reducers/alert.reducer';

export default function MyAlert() {
  const alertStore = useAlertStore();
  const dispatch = useDispatch();

  const handleClose = (event?: React.SyntheticEvent | Event, reason?: string) => {
    if (reason === 'clickaway') {
      return;
    }
    dispatch(hideAlert());
  };

  return (
    <div>
      <Snackbar open={alertStore.open} autoHideDuration={alertStore.duration} onClose={handleClose}>
        <Alert
          onClose={handleClose}
          severity={alertStore.type}
          variant="filled"
          sx={{ width: '100%' }}
        >
          {alertStore.message}
        </Alert>
      </Snackbar>
    </div>
  );
}