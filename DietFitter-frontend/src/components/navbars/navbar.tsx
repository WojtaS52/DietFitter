import { Link, useLocation, useNavigate } from "react-router-dom";
import { Navbar } from "flowbite-react";
import "flowbite/dist/flowbite.min.css";
import { useState } from "react";
import { logout } from "../../utils/api/api";

export default function Component() {
  const location = useLocation();
  const navigate = useNavigate();
  const [isOpen, setIsOpen] = useState(false);

  const handleLogout = async () => {
    const response = await logout();

    if (response.status === 200) {
      navigate("/");
    } else {
      console.error("Wylogowanie nie powiodło się.");
    }
  };

  return (
    <Navbar style={{ backgroundColor: "var(--primary-green)" }} className="text-white">
      <Navbar.Brand as={Link} to="/profile" className="cursor-pointer">
        <span className="w-full self-center whitespace-nowrap text-xl font-semibold text-white">
          Diet Fitter
        </span>
      </Navbar.Brand>

      <Navbar.Toggle onClick={() => setIsOpen(!isOpen)} />

      <Navbar.Collapse className={`flex items-center ${isOpen ? "block" : "hidden"} md:flex`}>
        <Navbar.Link
          as={Link}
          to="/profile"
          onClick={() => setIsOpen(false)}
          className={`nav-link text-white font-bold ${
            location.pathname === "/profile" ? "active" : ""
          }`}
        >
          Profil
        </Navbar.Link>

        <Navbar.Link
          as={Link}
          to="/settings"
          onClick={() => setIsOpen(false)}
          className={`nav-link text-white font-bold ${
            location.pathname === "/settings" ? "active" : ""
          }`}
        >
          Ustawienia
        </Navbar.Link>

        <Navbar.Link
          as={Link}
          to="/recommendations"
          onClick={() => setIsOpen(false)}
          className={`nav-link text-white font-bold ${
            location.pathname === "/recommendations" ? "active" : ""
          }`}
        >
          Rekomendacje
        </Navbar.Link>

        <Navbar.Link
          as={Link}
          to="/diet"
          onClick={() => setIsOpen(false)}
          className={`nav-link text-white font-bold ${
            location.pathname === "/diet" ? "active" : ""
          }`}
        >
          Algorytm
        </Navbar.Link>

        <button
          onClick={handleLogout}
          style={{ backgroundColor: "var(--logout-red)" }}
          className={`text-white font-medium transition leading-none rounded 
                      flex items-center justify-center font-bold
                      ${isOpen ? "w-full text-lg py-3 px-6 mt-4" : "w-auto text-xs py-1 px-2 mt-0 md:mt-0"}`}
        >
          Wyloguj się
        </button>
      </Navbar.Collapse>
    </Navbar>
  );
}
