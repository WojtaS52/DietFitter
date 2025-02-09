import { useEffect, useState } from "react";


const ModeSwitcher = () => {

    const [darkMode, setDarkMode] = useState(() => {
        return localStorage.getItem("darkMode") === "true";
      });

      useEffect(() => {
        if (darkMode) {
          document.body.classList.add("dark");
        //   localStorage.setItem("darkMode", "true");
        } else { 
          document.body.classList.remove("dark");
        //   localStorage.setItem("darkMode", "false");
        }

        localStorage.setItem("darkMode", darkMode.toString());
      }, [darkMode]);

      const handleModeChange = () => {
        setDarkMode((prev) => !prev);
      }
      return (
        <button onClick={handleModeChange} className="mode-switcher-button">
          {darkMode ? " Jasny" : " Nocny"}
        </button>
      );

};

export default ModeSwitcher;