import { ReactNode, useState, useEffect } from 'react';
import { IUserType } from '../types/types';
import { AuthContext } from './context/AuthContext';

type Props = {
  children: ReactNode;
};

export const AuthProvider = ({ children }: Props) => {
  const [user, setUser] = useState<IUserType | null>(null);

  useEffect(() => {
    console.log('AuthProvider user state updated:', user);
  }, [user]);

  return (
    <AuthContext.Provider value={{ user, setUser }}>
      {children}
    </AuthContext.Provider>
  );
};
