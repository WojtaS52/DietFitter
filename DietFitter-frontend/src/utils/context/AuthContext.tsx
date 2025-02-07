import { IUserType } from '../../types/types';
import { createContext } from 'react';

type AuthContextType = {
  user?: IUserType | null;
  setUser: (user: IUserType | null) => void;
};

export const AuthContext = createContext<AuthContextType>(
  {} as AuthContextType,
);