import { useEffect, useState } from "react";
import { changePassword, currentEmail, changeEmail, changeUserData, getUserData , getUserDetails, changeUserDetails} from "../utils/api/api";
import CustomSnackbar from "./ui/snackbar";
// import ModeSwitcher from "./ModeSwitcher";
import TextField from "@mui/material/TextField";
import { FormControl, InputLabel, MenuItem, Select} from "@mui/material";
const Settings = () => {
  const [activeForm, setActiveForm] = useState<string | null>(null);
  const [snackbarMessage, setSnackbarMessage] = useState<string | null>(null);
  const [snackbarType, setSnackbarType] = useState<"success" | "error" | null>(null);

  
  const [email, setEmail] = useState<string>("");
  const [name, setName] = useState<string>("");
  const [surname, setSurname] = useState<string>("");
  const [weight, setWeight] = useState<string>("");
  const [height, setHeight] = useState<string>("");
  const [gender, setGender] = useState<string>("");
  const [dateOfBirth, setDateOfBirth] = useState<Date | null>(null);
  const [oldPassword, setOldPassword] = useState<string>("");
  const [newPassword, setNewPassword] = useState<string>("");
  const [confirmPassword, setConfirmPassword] = useState<string>("");

  const handleSnackbarClose = () => {
    setSnackbarMessage(null);
    setSnackbarType(null);
  };

  useEffect(() => {
    if(activeForm === "changeData") {
      const fetchUserData = async () => {
      
        const response = await getUserData();
        if (response.status === 200) {
          setName(response.data.firstName || "");
          setSurname(response.data.lastName || "");
          setWeight(response.data.weight?.toString() || "");
          setHeight(response.data.height?.toString() || "");
        }
        
      };
      fetchUserData();

    }
    if (activeForm === "changeData2") {
      const fetchUserDetails = async () => {
        const response = await getUserDetails();
        if (response.status === 200) {
          setGender(response.data.gender || "");
          // setDateOfBirth(response.data.dateOfBirth ? response.data.dateOfBirth.split("T")[0] : "");
          setDateOfBirth(response.data.dateOfBirth ? new Date(response.data.dateOfBirth) : null);

        }
      };
      fetchUserDetails();
    }
    
  }, [activeForm]);

  const handleChangePassword = async () => {
    if (newPassword !== confirmPassword) {
      setSnackbarMessage("Nowe hasło i potwierdzenie hasła muszą być takie same!");
      setSnackbarType("error");
      return;
    }
    const response = await changePassword({ oldPassword, newPassword });
    if (response.status !== 200) {
      throw new Error("Błąd podczas zmiany hasła");
    }
    setSnackbarMessage("Hasło zostało zmienione pomyślnie!");
    setSnackbarType("success");
    
  };

  const handleChangeEmail = async (newEmail: string) => {
    
    const response = await changeEmail({ newEmail: newEmail });
    if (response.status !== 200) {
      throw new Error("Błąd podczas zmiany e-maila");
    }
    setSnackbarMessage("E-mail został zmieniony pomyślnie!");
    setSnackbarType("success");
    
  };

  const handleChangeUserData = async () => {
    
      const userData = {
          firstName: name,
          lastName: surname,
          weight: parseFloat(weight),
          height: parseFloat(height),
      };
      const response = await changeUserData(userData);
      if (response.status !== 200) {
        throw new Error("Błąd podczas zmiany danych");
      }
      setSnackbarMessage("Dane zostały zmienione pomyślnie!");
   
};

const handleChangeUserDetails = async () => {
  
  const userDetails = {
    gender,
    dateOfBirth: dateOfBirth ? new Date(dateOfBirth) : null,
  };

  const response = await changeUserDetails(userDetails);
  if (response.status !== 200) {
    throw new Error("Nie udało się zaktualizować danych.");
  } 
  setSnackbarMessage("Dane szczegółowe użytkownika zostały zaktualizowane!");

};


  const toggleForm = async (form: string) => {
    if (form !== "changePassword") {
      setOldPassword("");
      setNewPassword("");
      setConfirmPassword("");
    }

    if (form !== "changeEmail") {
      setEmail("");
    }

    if (form !== "changeData") {
      setName("");
      setSurname("");
      setWeight("");
      setHeight("");
    }

    setActiveForm((prev) => (prev === form ? null : form));

    if (form === "changeEmail") {
      
      const response = await currentEmail();
      if (response.status === 200) {
        setEmail(response.data.email);
      } else {
        throw new Error("Nie udało się pobrać obecnego e-maila.");
      }
      
    }
  };

  const renderForm = () => {
    switch (activeForm) {
      case "changePassword":
        return (
          <div className="mt-4">
            <h2 className="text-lg font-semibold mb-4">Zmień hasło</h2>
            <form
              className="flex flex-col gap-4"
              onSubmit={(e) => {
                e.preventDefault();
                handleChangePassword();
              }}
            >
              <TextField
                label="Stare hasło"
                type="password"
                value={oldPassword}
                onChange={(e) => setOldPassword(e.target.value)}
                variant="outlined"
                fullWidth
              />
              <TextField
                label="Nowe hasło"
                type="password"
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
                variant="outlined"
                fullWidth
              />
              <TextField
                label="Potwierdź nowe hasło"
                type="password"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                variant="outlined"
                fullWidth
              />
              <button
                type="submit"
                className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
              >
                Potwierdź
              </button>
            </form>
          </div>
        );
      case "changeEmail":
        return (
          <div className="mt-4">
            <h2 className="text-lg font-semibold mb-4">Zmień email</h2>
            <form
              className="flex flex-col gap-4"
              onSubmit={(e) => {
                e.preventDefault();
                handleChangeEmail(email);
              }}
            >
              <TextField
                type="email"
                label="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                variant="outlined"
                fullWidth
              />
              <button
                type="submit"
                className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
              >
                Potwierdź
              </button>
            </form>
          </div>
        );
        case "changeData2":
          return (
            <div className="mt-4">
              <h2 className="text-lg font-semibold mb-4">Zmień dane szczegółowe</h2>
              <form className="flex flex-col gap-4" onSubmit={(e) => { e.preventDefault(); handleChangeUserDetails(); }}>
              <FormControl fullWidth variant="outlined" sx={{ minWidth: 200 }}>
                  <InputLabel>Płeć</InputLabel>
                  <Select
                    value={gender}
                    onChange={(e) => setGender(e.target.value)}
                    label="Płeć"
                  >
                    <MenuItem value="Nie wybrano">Nie wybrano</MenuItem>
                    <MenuItem value="Mężczyzna">Mężczyzna</MenuItem>
                    <MenuItem value="Kobieta">Kobieta</MenuItem>
                  </Select>
                </FormControl>
                
                <TextField
                  label="Data urodzenia (dd-mm-rrrr)"
                  type="date"
                  value={dateOfBirth ? dateOfBirth.toISOString().split("T")[0] : ""}
                  onChange={(e) => setDateOfBirth(e.target.value ? new Date(e.target.value) : null)}
                  variant="outlined"
                  fullWidth
                />

                <button type="submit" className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600">Potwierdź</button>
              </form>
            </div>
          );



      case "changeData":
        return (
          <div className="mt-4">
            <h2 className="text-lg font-semibold mb-4">Zmień dane</h2>
            <form className="flex flex-col gap-4" onSubmit={(e) => { e.preventDefault(); handleChangeUserData(); }}>
              <TextField
                type="text"
                label="Imię"
                value={name}
                onChange={(e) => setName(e.target.value)}
                variant="outlined"
                fullWidth
              />
              <TextField
                type="text"
                label="Nazwisko"
                value={surname}
                onChange={(e) => setSurname(e.target.value)}
                variant="outlined"
                fullWidth
              />
              <TextField
                  label="Waga (kg)"
                  type="number"
                  value={weight}
                  onChange={(e) => setWeight(e.target.value)}
                  variant="outlined"
                  fullWidth
                />
              <TextField
                type="number"
                label="Wzrost (m)"
                value={height}
                onChange={(e) => setHeight(e.target.value)}
                variant="outlined"
                fullWidth
              />
              <button
                type="submit"
                className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
              >
                Potwierdź
              </button>
            </form>
          </div>
        );
      default:
        return null;
    }
  };

  return (
    <div className="flex flex-col items-center justify-start min-h-screen bg-gray-100">
      <div className="w-full max-w-3xl bg-white p-8 rounded shadow-lg">
        <div className="flex flex-wrap gap-4 justify-center sm:flex-nowrap">
          <button
            onClick={() => toggleForm("changeData")}
            className={`bg-gray-200 hover:bg-gray-300 text-gray-800 px-4 py-2 rounded ${
              activeForm === "changeData" && "bg-gray-300"
            }`}
          >
            Zmień dane
          </button>
          <button
            onClick={() => toggleForm("changePassword")}
            className={`bg-gray-200 hover:bg-gray-300 text-gray-800 px-4 py-2 rounded ${
              activeForm === "changePassword" && "bg-gray-300"
            }`}
          >
            Zmień hasło
          </button>
          <button
            onClick={() => toggleForm("changeEmail")}
            className={`bg-gray-200 hover:bg-gray-300 text-gray-800 px-4 py-2 rounded ${
              activeForm === "changeEmail" && "bg-gray-300"
            }`}
          >
            Zmień email
          </button>

          <button
            onClick={() => toggleForm("changeData2")}
            className={`bg-gray-200 hover:bg-gray-300 text-gray-800 px-4 py-2 rounded ${
              activeForm === "changeData2" && "bg-gray-300"
            }`}
            >
              Zmień dane szczegółowe
            </button>

        </div>
        {renderForm()}
      </div>
     
      {snackbarMessage && (
        <CustomSnackbar message={snackbarMessage} type={snackbarType} onClose={handleSnackbarClose} />
      )}
    </div>
  );
};

export default Settings;
