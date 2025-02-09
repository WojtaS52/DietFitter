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
        const response = await getAuthUser(); 
        if (response.status === 200) {
          const userData = response.data;
          setUser(userData);
          console.log('Authenticated user:', userData);
        }
      // eslint-disable-next-line @typescript-eslint/no-unused-vars
      } catch (error) {
        console.warn('User is unauthenticated');
        setUser(null); 
      } finally {
        setIsLoading(false); 
      }
    };

    getUser();
  }, [setUser]);

 
  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (user) return children ? <>{children}</> : <Outlet />;

  console.log('navigate to', replacePath);
  return <Navigate to={replacePath} state={{ from: location }} replace />;
};
