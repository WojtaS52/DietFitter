// theme/navbarTheme.ts
import { createTheme } from "@mui/material/styles";

const navbarTheme = createTheme({
  palette: {
    primary: {
      main: "#008000", // Kolor tła Navbara (zielony)
    },
    secondary: {
      main: "#ffffff", // Kolor tekstu dla zaznaczonych przycisków
    },
    text: {
      primary: "#ffffff", // Domyślny kolor tekstu
    },
  },
});

export default navbarTheme;
