import React from "react";
import { Box, Paper, Typography } from "@mui/material";
import PersonIcon from "@mui/icons-material/Person";
import PersonAddIcon from "@mui/icons-material/PersonAdd";
import { useNavigate } from "react-router-dom";
import Navbar2 from "../components/navbars/navbar2";
const HomePage: React.FC = () => {
  const navigate = useNavigate();

  return (
    <>
      <Navbar2 />
      <Box
        sx={{
          paddingTop: "64px", 
          textAlign: "center",
          backgroundColor: "#f4f4f4",
          minHeight: "100vh",
        }}
      >
        <Typography
          variant="h3"
          fontWeight="bold"
          sx={{ marginBottom: "1rem", color: "#333" }}
        >
          Witamy na stronie Diet Fitter!
        </Typography>
        <Typography
          variant="body1"
          sx={{ fontSize: "1.25rem", marginBottom: "2rem", color: "#555" }}
        >
          Tutaj znajdziesz wsparcie w doborze diety idealnej dla Ciebie.
        </Typography>

        
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            gap: "2rem",
            flexWrap: "wrap",
          }}
        >
          
          <Paper
            elevation={3}
            sx={{
              width: "200px",
              padding: "1.5rem",
              textAlign: "center",
              cursor: "pointer",
              borderRadius: "8px",
              "&:hover": {
                backgroundColor: "#e6f7e6", 
              },
              transition: "background-color 0.3s ease",
            }}
            onClick={() => navigate("/signin")}
          >
            <PersonIcon sx={{ fontSize: "80px", color: "var(--primary-green)" }} />
            <Typography variant="h5" fontWeight="bold" sx={{ marginTop: "1rem" }}>
              Logowanie
            </Typography>
          </Paper>

          <Paper
            elevation={3}
            sx={{
              width: "200px",
              padding: "1.5rem",
              textAlign: "center",
              cursor: "pointer",
              borderRadius: "8px",
              "&:hover": {
                backgroundColor: "#e6f7e6",
              },
              transition: "background-color 0.3s ease",
            }}
            onClick={() => navigate("/signup")}
          >
            <PersonAddIcon sx={{ fontSize: "80px", color: "var(--primary-green)" }} />
            <Typography variant="h5" fontWeight="bold" sx={{ marginTop: "1rem" }}>
              Rejestracja
            </Typography>
          </Paper>
        </Box>
      </Box>
    </>
  );
};

export default HomePage;
