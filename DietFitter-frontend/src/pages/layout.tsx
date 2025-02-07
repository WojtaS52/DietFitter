
import Component from '../components/navbars/navbar';
import { Outlet } from 'react-router-dom';
import  FooterComponent  from '../components/ui/footer';
export const Layout = () => {
  return (
    <>
      <header>
        <Component/>
      </header>
      <Outlet />
      <FooterComponent/>
    </>
  );
};