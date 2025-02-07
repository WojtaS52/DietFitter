import { useAuthContext } from '../hooks/useAuthContext';
import { getAuthUser } from '../utils/api/api';
import { FC, useEffect, useState } from 'react';
import { Navigate, Outlet, useLocation } from 'react-router-dom';

type ProtectedRouteProps = {
  replacePath?: string;
};

export const ProtectedRoute: FC<
  React.PropsWithChildren & ProtectedRouteProps
> = ({ replacePath = '/signin', children }) => {
  const { user, setUser } = useAuthContext();
  const location = useLocation();
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const getUser = async () => {
      try {
        const response = await getAuthUser(); // Pobieranie użytkownika z backendu
        if (response.status === 200) {
          const userData = response.data;
          setUser(userData);
          console.log('Authenticated user:', userData);
        }
      } catch (error) {
        console.warn('User is unauthenticated');
        setUser(null); // Jeśli użytkownik nie jest zalogowany
      } finally {
        setIsLoading(false); // Ustawienie, że zakończono ładowanie
      }
    };

    getUser();
  }, [setUser]);

  // Wyświetl ekran ładowania, dopóki trwa pobieranie danych
  if (isLoading) {
    return <div>Loading...</div>;
  }

  // Jeśli użytkownik jest zalogowany, renderuj komponenty chronione
  if (user) return children ? <>{children}</> : <Outlet />;

  // Jeśli użytkownik nie jest zalogowany, przekieruj na stronę logowania
  console.log('navigate to', replacePath);
  return <Navigate to={replacePath} state={{ from: location }} replace />;
};
