import { useState, useEffect } from 'react';
import { Line } from 'react-chartjs-2';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

import Calendar from "../components/ui/calendar";

export default function ProfilePage() {
  const [userEmail, setUserEmail] = useState<string | null>(null);
  const [userError, setUserError] = useState<string | null>(null);
  const [monitoringType, setMonitoringType] = useState<string>('');
  const [parameterValue, setParameterValue] = useState<number | ''>('');
  const [previousValues, setPreviousValues] = useState<{ date: string; value: number }[]>([]);
  const [monitoringError, setMonitoringError] = useState<string | null>(null);

  const monitoringLabelsChart: Record<string, string> = {
    bloodSugar: 'Poziom cukru',
    magnesium: 'Poziom magnezu',
    iron: 'Poziom żelaza',
    cholesterol: 'Poziom cholesterolu',
    potassium: 'Poziom potasu',
    zinc: 'Poziom cynku',
    calcium: 'Poziom wapnia',
    bloodPressure: 'Ciśnienie tętnicze',
    weight: 'Waga',
  };

  const normsParameters: Record<string, { min: number; max: number }> = { 

    bloodSugar: { min: 70, max: 100 },      //podane w mg/dl
    magnesium: { min: 1.8, max: 2.3 },      //podane w mg/dl
    iron: { min: 60, max: 160 },            //podane w ug/dl
    cholesterol: { min: 140, max: 200 },    //podane w mg/dl
    potassium: { min: 3.5, max: 5.0 },      //podane w mmol/l
    zinc: { min: 0.66, max: 1.10 },         //podane w mg/dl
    calcium: { min: 8.5, max: 10.5 },        //podane w mg/dl
    bloodPressure: { min: 120, max: 80 },    //podane w mmHg
    weight: { min: 50, max: 100 },           //podane w kg

  };

  const analyzeUserStats = (values: { date: string; value: number }[], parameter: string) => {
    if (values.length < 3) return "Brak wystarczających danych do analizy.";
  
    const latestValue = values[values.length - 1].value;
    const previousValue = values[values.length - 2].value;
    const previousValue2 = values[values.length - 3].value;
  
    const norm = normsParameters[parameter];
  
    let status = "";
  
    if (!norm) {
      status = "Brak dostępnych norm dla tego parametru";
    } else if (latestValue > norm.max) {
      status = "Wartość powyżej normy";
    } else if (latestValue < norm.min) {
      status = "Wartość poniżej normy";
    } else {
      status = "Wartość parametru jest w porządku"
    }
  
    
    let trend;
    if (latestValue === previousValue && previousValue === previousValue2){
      trend = "Trend stabilny";
    }else if (previousValue2 > previousValue && previousValue> latestValue){
      trend = "Trend malejący";
    }else if (previousValue2 < previousValue && previousValue < latestValue){
      trend = "Trend rosnący";
    }else if (previousValue2 === previousValue && previousValue > latestValue){
      trend = "Odwrócenie trendu na malejący";
    }else if (previousValue2 === previousValue && previousValue < latestValue){
      trend = "Rozpoczęcie trendu na rosnący";
    }else if (previousValue2 > previousValue && previousValue === latestValue){
      trend = "Malejący trend przechodzący w stabilny";
    } else if (previousValue2 < previousValue && previousValue === latestValue){
      trend = "Rosnący trend przechodzący w stabilny";
    }else if ((previousValue2 > previousValue && previousValue < latestValue) || (previousValue2 < previousValue && previousValue > latestValue)){
      trend = "Trend niestabilny";
    }

  
    return `${status}. ${trend}.`;
  };
  

  const fetchUserData = async () => {
    
    const response = await fetch('http://localhost:5000/users/me', {
      method: 'GET',
      credentials: 'include',
    });

    if (!response.ok) {
      const errorData = await response.json();
      setUserError(errorData.message || 'Nie udało się pobrać danych użytkownika.');
      return;
    }

    const data = await response.json();
    setUserEmail(data.email || 'Nieznany email');
    
  };

  const fetchMonitoringData = async () => {
    if (!monitoringType) return;

    
      const response = await fetch(`http://localhost:5000/api/userstats/${monitoringType}`, {
        method: 'GET',
        credentials: 'include',
      });

      if (!response.ok) {
        throw new Error('Nie udało się pobrać danych');
      }

      const data = await response.json();

      const filteredAndSortedData = data
        .map((item: { date: string; [key: string]: number }) => ({
          date: item.date,
          value: item[monitoringType],
        }))
        .filter(entry => entry.value !== null)
        .sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());

      setPreviousValues(filteredAndSortedData);
    
  };

  const handleMonitoringSubmit = async () => {
    setMonitoringError(null);

    if (!monitoringType || parameterValue === '') {
      setMonitoringError('Proszę wybrać typ monitorowania i podać wartość.');
      return;
    }

    const payload = {
      monitoringType,
      value: parameterValue,
    };

    try {
      const response = await fetch('http://localhost:5000/api/userstats', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        credentials: 'include',
        body: JSON.stringify(payload),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Nie udało się zapisać danych');
      }

      setParameterValue('');
      fetchMonitoringData();
    } catch (error) {
      setMonitoringError('Błąd zapisu: ' + error.message);
    }
  };

  useEffect(() => {
    fetchUserData();
  }, []);

  useEffect(() => {
    fetchMonitoringData();
  }, [monitoringType]);

  const formatter = new Intl.DateTimeFormat('pl-PL', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit'
  });

  const chartData = {
    labels: previousValues.map((entry) => formatter.format(new Date(entry.date))),
    datasets: [
      {
        label: ` ${monitoringLabelsChart[monitoringType] || monitoringType}`,
        data: previousValues.map((entry) => entry.value),
        borderColor: 'rgba(75, 192, 192, 1)',
        backgroundColor: 'rgba(75, 192, 192, 0.2)',
        fill: true,
      },
    ],
  };

  const chartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { display: true },
      tooltip: { mode: 'index', intersect: false },
    },
    scales: {
      x: { title: { display: true, text: 'Data' } },
      y: { title: { display: true, text: 'Wartość' } },
    },
  };

  return (
    <div style={{ padding: '20px' }}>
      <div
        style={{
          marginBottom: '20px',
          padding: '10px',
          backgroundColor: '#e9f7ef',
          borderRadius: '5px',
        }}
      >
        <h3>Witaj, {userEmail || 'Nieznany użytkowniku'}</h3>
        {userError && <p style={{ color: 'red' }}>Błąd: {userError}</p>}
      </div>

      <div style={{ display: "flex", flexWrap: "wrap", justifyContent: "center", gap: "20px" }}>
        <div
          style={{
            width: '350px',
            padding: '20px',
            border: '1px solid #ccc',
            borderRadius: '10px',
            backgroundColor: '#f9f9f9',
          }}
        >
          <h3>Monitorowanie parametrów</h3>
          <div style={{ marginBottom: '10px' }}>
            
            <label>
              Typ:{' '}
              <select
                value={monitoringType}
                onChange={(e) => setMonitoringType(e.target.value)}
                style={{ width: '100%', padding: '5px', marginTop: '5px' }}
              >
                <option value="">Wybierz...</option>
                {Object.entries(monitoringLabelsChart).map(([key, label]) => (
                  <option key={key} value={key}>{label}</option>
                ))}
              </select>
            </label>

          </div>

          {monitoringType && (
            <div style={{ marginBottom: '10px' }}>
              <label>
                Podaj wartość:{' '}
                <input
                  type="number"
                  value={parameterValue}
                  onChange={(e) => setParameterValue(parseFloat(e.target.value) || '')}
                  style={{ width: '100%' }}
                />
              </label>
            </div>

          )}

          <button
            onClick={handleMonitoringSubmit}
            style={{
              padding: '10px',
              backgroundColor: '#28a745',
              color: 'white',
              border: 'none',
              borderRadius: '5px',
              cursor: 'pointer',
              width: '100%',
            }}
          >
            Zapisz
          </button>

          <div style={{ marginTop: '20px' }}>
            <Calendar />
          </div>
        </div>

        <div style={{ flex: 1, minWidth: '300px', maxWidth: '800px', width: '100%' }}>
          <h3>Historia parametrów</h3>
          {monitoringType && previousValues.length > 0 ? (
            <><div style={{ width: '100%', height: '350px' }}>
              <Line data={chartData} options={chartOptions} />
            </div>
            <div style={{ marginTop: "10px", padding: "10px", backgroundColor: "#f9f9f9", borderRadius: "5px" }}>
                <strong>Analiza parametrów:</strong>
                <p>{analyzeUserStats(previousValues, monitoringType)}</p>
              </div></>
            
            

          ) : (
            <div style={{
              width: '100%',
              height: '350px',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              backgroundColor: '#f9f9f9',
              borderRadius: '10px',
              border: '1px solid #ccc'
            }}>
              <p style={{ color: '#555' }}>
                {monitoringType ? 'Brak danych' : 'Wybierz typ parametru, aby zobaczyć wykres'}
              </p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
