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
import CustomSnackbar from '../../components/ui/snackbar';

// eslint-disable-next-line @typescript-eslint/no-explicit-any
function Copyright(props: any) {
    return (
        <Typography variant="body2" color="text.secondary" align="center" {...props}>
            {'Copyright © '}
            <Link color="inherit" href="https://mui.com/">
                DietFitter
            </Link>{' '}
            {new Date().getFullYear()}
            {'.'}
        </Typography>
    );
}

const theme = createTheme();



export default function SignIn() {
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

        const payload = {
            email: data.get('email'),
            password: data.get('password'),
            twoFactorCode: "string",
            twoFactorRecoveryCode: "string",
        };

        
        const response = await fetch('http://localhost:5000/login?useCookies=true&useSessionCookies=true', {
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
            throw new Error(`Login failed: ${response.status} - ${error}`);
        }

        setSnackbar({
            open: true,
            message: 'Logowanie powiodło się!',
            severity: 'success',
        });

        setTimeout(() => {
            navigate('/profile');
        }, 2000); 
        
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
                        Logowanie
                    </Typography>
                    <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 1 }}>
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            id="email"
                            label="Email"
                            name="email"
                            autoComplete="email"
                            autoFocus
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
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="password"
                            label="Hasło"
                            type="password"
                            id="password"
                            autoComplete="current-password"
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
                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            sx={{ mt: 3, mb: 2, backgroundColor: 'var(--primary-green)' }}
                        >
                            Zaloguj się
                        </Button>
                        <Grid container>
                            <Grid item xs>
                                <Link href="#" variant="body2" sx={{ color: 'var(--primary-green)' }}>
                                    Zapomniałeś hasła?
                                </Link>
                            </Grid>
                            <Grid item>
                                <Link href="#" variant="body2" sx={{ color: 'var(--primary-green)' }}>
                                    {"Nie masz konta? Zarejestruj się"}
                                </Link>
                            </Grid>
                        </Grid>
                    </Box>
                </Box>
                <Copyright sx={{ mt: 8, mb: 4 }} />

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
