import React, { useState, useEffect } from "react";
import { Button, Box, Typography, TextField, MenuItem, Paper, FormControlLabel, Checkbox, Tooltip } from "@mui/material";
import { createRecommendation, getAuthUser, getLastUserRecommendation, likeRecommendation } from "../utils/api/api";
import InfoOutlinedIcon from "@mui/icons-material/InfoOutlined";
import CustomSnackbar from "./ui/snackbar";


const Diet: React.FC = () => {
  const [userWeight, setUserWeight] = useState("");
  const [userHeight, setUserHeight] = useState("");
  const [selectedCondition, setSelectedCondition] = useState("");
  const [isVegan, setIsVegan] = useState(false);
  const [recommendation, setRecommendation] = useState<any | null>(null);
  const [loading, setLoading] = useState(false);
  const [userId, setUserId] = useState<string | null>(null);
  const [liking, setLiking] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState<string | null>(null);
  const [snackbarType, setSnackbarType] = useState<"success" | "error" | null>(null);

  const handleSnackbarClose = () => {
    setSnackbarMessage(null);
    setSnackbarType(null);
  };




  useEffect(() => {
    const fetchUserId = async () => {
      const response = await getAuthUser();
      setUserId(response.data.id);
     
    };

    fetchUserId();
  }, []);

  const handleWeightChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    let value = parseFloat(e.target.value);
    if (value < 1) value = 1;
    if (value > 180) value = 180;
    setUserWeight(value.toString());
  };

  const handleHeightChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    let value = parseFloat(e.target.value);
    if (value < 1) value = 1;
    if (value > 240) value = 240;
    setUserHeight(value.toString());
  };

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
        preferredCategory: isVegan ? "Wegańskie" : "Wszystkie",
      });

      setRecommendation(response.data);
    } catch (error) {
      console.error("Błąd podczas pobierania rekomendacji:", error);
      alert("Wystąpił błąd. Spróbuj ponownie.");
    }
    setLoading(false);
  };

  const fetchLastRecommendationId = async () => {
    if (!userId) return null;

    const response = await getLastUserRecommendation(userId);
    return response.data.id;
  };

  const handleLikeLastRecommendation = async () => {
    const lastRecommendationId = await fetchLastRecommendationId();
    if (!lastRecommendationId) {
      setSnackbarMessage("Wystąpił błąd podczas polubienia rekomendacji.");
      setSnackbarType("error");
      return;
    }
    setLiking(true);
    await likeRecommendation(lastRecommendationId);
    setSnackbarMessage("Rekomendacja została polubiona");
    setSnackbarType("success");
    setLiking(false);
  };

  return (
    <Box display="flex" alignItems="flex-start" justifyContent="center" padding={2} gap={4}>
      <Box display="flex" flexDirection="column" gap={2} width="300px">
        <Typography variant="h4" gutterBottom>
          Wypełnij formularz doboru diety
        </Typography>
        <TextField
          label="Waga (kg)"
          type="number"
          value={userWeight}
          onChange={handleWeightChange}
          onInput={(e) => (e.currentTarget.value = e.currentTarget.value.replace(/[^0-9]/g, ""))}
          fullWidth
          required
        />
        <TextField
          label="Wzrost (cm) (opcjonalnie)"
          type="number"
          value={userHeight}
          onChange={handleHeightChange}
          onInput={(e) => (e.currentTarget.value = e.currentTarget.value.replace(/[^0-9]/g, ""))}
          fullWidth
        />
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
        <FormControlLabel control={<Checkbox checked={isVegan} onChange={(e) => setIsVegan(e.target.checked)} sx={{
        "&.Mui-checked": {
          color: "var(--button-green)", 
        },
      }} />} label="Dieta wegańska" />

        <Button variant="contained" sx={{ 
              backgroundColor: "var(--button-green)",
              "&:hover": { backgroundColor: "var(--button-green-hover)" }

            }} onClick={handleSubmit} disabled={loading || !userId} fullWidth>
          {loading ? "Dobieranie" : "Dobierz dietę"}
        </Button>

        {recommendation && (
          <>
          <Button
            variant="contained"
            color="primary"
            onClick={handleLikeLastRecommendation}
            disabled={liking}
            fullWidth
            sx={{ 
              marginTop: 2, 
              backgroundColor: "var(--button-green)",
              "&:hover": { backgroundColor: "var(--button-green-hover)" }

            }}
          >
            {liking ? "Zapisywanie rekomendacji" : "Zapisz rekomendację"}
          </Button>
          
          <Box display="flex" justifyContent="center" alignItems="center" marginTop={1} gap={2}>
            <Tooltip title="W przypadku niepokojących objawów zdrowotnych zalecamy skonsultowanie się z lekarzem, bądź dietetykiem.">
              <InfoOutlinedIcon sx={{ color: "gray", cursor: "pointer" }} />
            </Tooltip>

            <Tooltip title="Jeśli w któryms z posiłków znajduje się np. 1 albo 2 produkty wynika to z ograniczeń danych w bazie. Przepraszam za utrudnienia.">
                <InfoOutlinedIcon sx={{ color: "gray", cursor: "pointer" }} />
            </Tooltip>
          </Box>
        </>
        )}
      </Box>

      {recommendation && (
        <Paper
          elevation={3}
          style={{
            padding: "16px",
            backgroundColor: "var(--diet-white)",
            width: "400px",
            alignSelf: "flex-start", 
          }}
        >
          <Typography variant="h6">Rekomendacja:</Typography>
          {recommendation.$values?.map((meal: any, index: number) => (
            <Box key={index} marginTop={2}>
              <Typography variant="h6">{meal.name}</Typography>
              <ul>
                {meal.items.$values?.map((item: any, idx: number) => (
                  <li key={idx}>
                    {item.food} - {item.grams}g
                  </li>
                ))}
              </ul>
            </Box>
          ))}
        </Paper>
      )}
      {snackbarMessage && (
      <CustomSnackbar 
        message={snackbarMessage} 
        type={snackbarType} 
        onClose={handleSnackbarClose} 
      />
    )}
    </Box>
  );
};

export default Diet;
