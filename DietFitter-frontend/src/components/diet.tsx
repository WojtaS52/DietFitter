import React, { useState, useEffect } from "react";
import { Button, Box, Typography, TextField, MenuItem, Paper } from "@mui/material";
import { createRecommendation, getAuthUser, getLastUserRecommendation } from "../utils/api/api";

const Diet: React.FC = () => {
  const [showForm, setShowForm] = useState(false);
  const [userWeight, setUserWeight] = useState("");
  const [userHeight, setUserHeight] = useState("");
  const [selectedCondition, setSelectedCondition] = useState("");
  const [preferredCategory, setPreferredCategory] = useState("");
  const [recommendation, setRecommendation] = useState<any | null>(null);
  const [loading, setLoading] = useState(false);
  const [userId, setUserId] = useState<string | null>(null);

 
  useEffect(() => {
    const fetchUserId = async () => {
      try {
        const response = await getAuthUser();
        setUserId(response.data.id); 
      } catch (error) {
        console.error("Błąd podczas pobierania danych użytkownika:", error);
      }
    };

    fetchUserId();
  }, []);

  const handleSubmit = async () => {
    if (!userWeight || !selectedCondition || !userId) {
      alert("Proszę wypełnić wszystkie wymagane pola.");
      return;
    }

    setLoading(true);
    try {
      const response = await createRecommendation({
        userId, 
        userWeight: parseFloat(userWeight),
        userHeight: userHeight ? parseFloat(userHeight) : undefined,
        selectedCondition,
        preferredCategory: preferredCategory || undefined,
      });

      setRecommendation(response.data);
    } catch (error) {
      console.error("Błąd podczas pobierania rekomendacji:", error);
      alert("Wystąpił błąd. Spróbuj ponownie.");
    }
    setLoading(false);
  };

  const handleGetLastRecommendation = async () => {
    if (!userId) {
      alert("Brak ID użytkownika. Upewnij się, że jesteś zalogowany.");
      return;
    }
  
    setLoading(true);
    try {
      const response = await getLastUserRecommendation(userId);
      setRecommendation(response.data);
      console.log("🔍 Otrzymana rekomendacja:", response.data);
    } catch (error) {
      console.error("Błąd podczas pobierania ostatniej rekomendacji:", error);
      alert("Nie znaleziono wcześniejszej rekomendacji.");
    }
    setLoading(false);
  };
  

  return (
    <Box display="flex" flexDirection="column" alignItems="center" justifyContent="center" padding={2}>
      <Typography variant="h4" gutterBottom>
        Wybierz sposób doboru diety
      </Typography>
      {!showForm ? (
        <Box display="flex" gap={2}>
          <Button variant="contained" color="primary" onClick={() => setShowForm(true)}>
            Skorzystaj z algorytmu
          </Button>
          <Button variant="contained" color="secondary" onClick={handleGetLastRecommendation}>Zobacz poprzednią rekomendację</Button>
          {/* <Button variant="contained" color="secondary">Zobacz poprzednią rekomendację</Button> */}
        </Box>

      ) : (
        <Box display="flex" flexDirection="column" gap={2} width="300px">
          <TextField label="Waga (kg)" type="number" value={userWeight} onChange={(e) => setUserWeight(e.target.value)} fullWidth required />
          <TextField label="Wzrost (cm) (opcjonalnie)" type="number" value={userHeight} onChange={(e) => setUserHeight(e.target.value)} fullWidth />
          <TextField label="Wybierz problem zdrowotny" select value={selectedCondition} onChange={(e) => setSelectedCondition(e.target.value)} fullWidth required>
            <MenuItem value="nadwaga">Nadwaga</MenuItem>
            <MenuItem value="niedowaga">Niedowaga</MenuItem>
            <MenuItem value="cukrzyca">Cukrzyca</MenuItem>
            <MenuItem value="insulinoodporność">Insulinooporność</MenuItem>
            <MenuItem value="nadciśnienie">Nadciśnienie</MenuItem>
            <MenuItem value="niedobór żelaza">Niedobór żelaza</MenuItem>
            <MenuItem value="niedobór magnezu">Niedobór magnezu</MenuItem>
            <MenuItem value="niedobór potasu">Niedobór potasu</MenuItem>
            <MenuItem value="niedobór cynku">Niedobór cynku</MenuItem>
            <MenuItem value="niedobór sodu">Niedobór sodu</MenuItem>
            <MenuItem value="niedobór wapnia">Niedobór wapnia</MenuItem>
            <MenuItem value="niedobór witaminy D">Niedobór witaminy D</MenuItem>
          </TextField>
          <TextField label="Wybierz kategorię produktów (opcjonalnie)" select value={preferredCategory} onChange={(e) => setPreferredCategory(e.target.value)} fullWidth>
            <MenuItem value="">Wszystkie</MenuItem>
            <MenuItem value="Wegańskie">Wegańskie</MenuItem>
            <MenuItem value="Mięso">Mięso</MenuItem>
            <MenuItem value="Owoce">Owoce</MenuItem>
            <MenuItem value="Warzywa">Warzywa</MenuItem>
            <MenuItem value="Produkty mleczne">Produkty mleczne</MenuItem>
            <MenuItem value="Ryby">Ryby</MenuItem>
            <MenuItem value="Orzechy i nasiona">Orzechy i nasiona</MenuItem>
          </TextField>
          <Button variant="contained" color="primary" onClick={handleSubmit} disabled={loading || !userId}>
            {loading ? "Przetwarzanie..." : "Potwierdź"}
          </Button>

          {recommendation && (
            <Paper elevation={3} style={{ padding: "16px", backgroundColor: "#d4edda", color: "#155724", marginTop: "16px" }}>
              <Typography variant="h6">Rekomendacja:</Typography>
              {console.log("🔍 Aktualna rekomendacja:", recommendation)}
              {recommendation?.length > 0 ? (
                recommendation.map((item: any, index: number) => (
                  <Typography key={index}>
                    {index + 1}. {item.food} - {item.grams}g ({item.providedValue} mg)
                  </Typography>
                ))
              ) : (
                <Typography>Brak rekomendacji.</Typography>
              )}

            </Paper>
          )}


          
        </Box>
      )}
    </Box>
  );
};

export default Diet;
