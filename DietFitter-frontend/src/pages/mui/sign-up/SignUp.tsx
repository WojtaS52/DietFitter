import * as React from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
// import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { useNavigate } from 'react-router-dom';
import CustomSnackbar from '../../../components/ui/snackbar'; 


// eslint-disable-next-line @typescript-eslint/no-explicit-any
function Copyright(props: any) {
  return (
    <Typography variant="body2" color="text.secondary" align="center" {...props}>
      {'Copyright © '}
      <Link color="inherit" href="https://mui.com/">
        Diet Fitter
      </Link>{' '}
      {new Date().getFullYear()}
      {'.'}
    </Typography>
  );
}


const theme = createTheme();

export default function SignUp() {
  
  const navigate = useNavigate();

  const [snackbar, setSnackbar] = React.useState({
    open: false,
    message: '',
    severity: 'info' as 'success' | 'error' | 'info' | 'warning',
  });

  const handleSnackbarClose = () => {
    setSnackbar((prev) => ({ ...prev, open: false }));
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const data = new FormData(event.currentTarget);

    const email = data.get('email');
    const password = data.get('password');
    const confirmPassword = data.get('confirmPassword');

    if (password !== confirmPassword) {
      setSnackbar({
        open: true,
        message: 'Hasła nie są takie same. Spróbuj ponownie.',
        severity: 'warning',
      });
      return;
    }

    const payload = {
      email,
      password,
    };

    try {
      const response = await fetch('http://localhost:5000/register', {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload),
        credentials: 'include',
      });

      if (!response.ok) {
        const error = await response.text();
        throw new Error(`Register failed: ${response.status} - ${error}`);
      }

      setSnackbar({
        open: true,
        message: 'Rejestracja zakończona sukcesem. Możesz się teraz zalogować.',
        severity: 'success',
      });

      setTimeout(() => {
        navigate('/home');
      }, 2000); // 2 sekundy
    } catch (error) {
      console.error('Error:', error);
      setSnackbar({
        open: true,
        message: 'Rejestracja nie powiodła się. Sprawdź dane i spróbuj ponownie.',
        severity: 'error',
      });
    }
  };
 

  return (
    
    <ThemeProvider theme={theme}>
      <Container component="main" maxWidth="xs">
        {/* <CssBaseline /> */}
        <Box
          sx={{
            marginTop: 8,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
          }}
        >
          <Avatar sx={{ m: 1, bgcolor: 'var(--primary-green)' }}>
            <LockOutlinedIcon />
          </Avatar>
          <Typography component="h1" variant="h5">
            Rejestracja
          </Typography>
          <Box component="form" noValidate onSubmit={handleSubmit} sx={{ mt: 3 }}>
            <Grid container spacing={2}>
              <Grid item xs={12}>
                <TextField
                  required
                  fullWidth
                  id="email"
                  label="Email"
                  name="email"
                  autoComplete="email"
                  sx={{
                    '& .MuiOutlinedInput-root': {
                      '&.Mui-focused fieldset': {
                        borderColor: 'var(--primary-green)', 
                      },
                    },
                    '& .MuiInputLabel-root.Mui-focused': {
                    color: 'var(--primary-green)',
                    },
                  }}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  required
                  fullWidth
                  name="password"
                  // label="Hasło - minimum 8 znaków , duża litera, cyfra, znak specjalny"
                  label="Hasło"
                  type="password"
                  id="password"
                  autoComplete="new-password"
                  sx={{
                    '& .MuiOutlinedInput-root': {
                      '&.Mui-focused fieldset': {
                        borderColor: 'var(--primary-green)', 
                      },
                    },

                    '& .MuiInputLabel-root.Mui-focused': {
                    color: 'var(--primary-green)',
                    },

                  }}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  required
                  fullWidth
                  name="confirmPassword"
                  label="Powtórz hasło"
                  type="password"
                  id="confirmPassword"
                  sx={{
                    '& .MuiOutlinedInput-root': {
                      '&.Mui-focused fieldset': {
                        borderColor: 'var(--primary-green)', 
                      },
                    },
                    '& .MuiInputLabel-root.Mui-focused': {
                    color: 'var(--primary-green)',
                    },
                  }}
                />
              </Grid>
            </Grid>
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2, backgroundColor: "var(--primary-green)" }}
            >
              Zarejestruj się
            </Button>
            <Grid container justifyContent="flex-end">
              <Grid item>
                <Link href="#" variant="body2" sx={{
                  color: 'var(--primary-green)', }} onClick={() => navigate('/signin')}>
                  Masz już konto? Zaloguj się
                </Link>
              </Grid>
            </Grid>
          </Box>
        </Box>
        <Copyright sx={{ mt: 5 }} />

        {snackbar.open && (
          <CustomSnackbar
            message={snackbar.message}
            severity={snackbar.severity}
            onClose={handleSnackbarClose}
          />
        )}
      </Container>
    </ThemeProvider>
    
  );
}
