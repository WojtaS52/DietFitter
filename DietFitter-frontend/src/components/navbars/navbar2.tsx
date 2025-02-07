import { Link, useLocation} from "react-router-dom";
import { Navbar } from "flowbite-react";
import 'flowbite/dist/flowbite.min.css';

export default function Navbar2() {
    
  const location = useLocation();

  return (
    <Navbar style={{ backgroundColor: "#008000" }} className="text-white">
      <Navbar.Brand as={Link} to="/home" className="cursor-pointer">
        <span className="w-full self-center whitespace-nowrap text-xl font-semibold text-white">
          Diet Fitter
        </span>
      </Navbar.Brand>
      <Navbar.Toggle />
      <Navbar.Collapse>
        <Navbar.Link
          as={Link}
          to="/settings"
          className={`nav-link ${location.pathname === "/signin" ? "active" : ""}`}
        >
          Zaczynamy
        </Navbar.Link>
      </Navbar.Collapse>
    </Navbar>
  );
}
