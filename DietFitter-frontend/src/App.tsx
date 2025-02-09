import { BrowserRouter, Route, Routes } from 'react-router-dom';
// import { Navigate } from 'react-router-dom';
import './App.css';
import HomePage from './pages/home_page'; 
import SignIn from './pages/mui/SignIn';
import SignUp from './pages/mui/sign-up/SignUp';
import ProfilePage from './pages/profile_page';
import SettingsPage from './pages/SettingsPage';
import RecommendationPage from './pages/recommendationPage';
import { Layout } from './pages/layout';
import { ProtectedRoute } from './components/ProtectedRoute';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { AuthProvider } from './utils/AuthProvider';
import { ReactNode } from 'react';
import { CssBaseline } from '@mui/material';
import DietPage from './pages/dietPage';
import WelcomePage from './pages/WelcomePage';

const queryClient = new QueryClient();

function AppWithProviders({ children }: { children: ReactNode }) {
  return (
    <BrowserRouter>
      <QueryClientProvider client={queryClient}>
        <AuthProvider>{children}</AuthProvider>
        {/* <ReactQueryDevtools initialIsOpen={false} /> */}
      </QueryClientProvider>
    </BrowserRouter>
  );
}

function App() {
  return (
    <AppWithProviders>
      {/* <Toaster /> */}
      <CssBaseline />
      <Routes>
        {/* Strona główna  */}
        <Route path="/" element={<HomePage />} />
        
        {/* Logowanie */}
        <Route path="/signin" element={<SignIn />} />
        {/* Rejestracja */}
        <Route path="/signup" element={<SignUp />} />
        {/* <Route path="/welcome" element={<WelcomePage />} /> */}

        {/* Authenticated routes */}
        <Route element={<ProtectedRoute children={<Layout />} />}>
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/settings" element={<SettingsPage />} />
          <Route path="/diet" element={<DietPage />} />
          <Route path="/welcome" element={<WelcomePage />} />
          <Route path="/recommendations" element={<RecommendationPage />} />
        </Route>
      </Routes>

    </AppWithProviders>
  );
}

export default App;




