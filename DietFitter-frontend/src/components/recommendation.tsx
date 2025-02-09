import React, { useEffect, useState } from "react";
import { getAuthUser, getRecommendationLikes } from "../utils/api/api";
import { Checkbox, FormControlLabel } from "@mui/material";

interface MealItem {
  food: string;
  grams: number;
  providedValue: number;
}

interface Meal {
  name: string;
  items: MealItem[];
}

interface Recommendation {
  id: string;
  date: string;
  problem: string;
  selectedCategory: string;
  meals: Meal[];
}

const removeDuplicateMeals = (meals: Meal[]) => {
  const mealMap: { [key: string]: Meal } = {};

  meals.forEach((meal) => {
    if (!mealMap[meal.name]) {
      mealMap[meal.name] = { ...meal, items: [...meal.items] };
    } else {
      meal.items.forEach((item) => {
        const existingItem = mealMap[meal.name].items.find(i => i.food === item.food);
        if (existingItem) {
          existingItem.grams += item.grams;
          existingItem.providedValue += item.providedValue;
        } else {
          mealMap[meal.name].items.push(item);
        }
      });
    }
  });

  return Object.values(mealMap);
};

function Recommendation() {
  const [userId, setUserId] = useState<string | null>(null);
  const [recommendations, setRecommendations] = useState<Recommendation[]>([]);
  const [likedRecommendations, setLikedRecommendations] = useState<Recommendation[]>([]);
  const [showLikedOnly, setShowLikedOnly] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUserId = async () => {
      const response = await getAuthUser();
      setUserId(response.data.id);
    };
    fetchUserId();
  }, []);

  useEffect(() => {
    if (!userId) return;

    const fetchRecommendations = async () => {
    
        const response = await fetch(
          `http://localhost:5000/api/DietRecommendation/user-recommendations/${userId}?limit=1`
        );
        if (!response.ok) throw new Error("Błąd pobierania rekomendacji");

        const data = await response.json();

        const formattedRecommendations = (data.$values || []).map((rec: any) => ({
          ...rec,
          meals: removeDuplicateMeals(rec.meals?.$values?.map((meal: any) => ({
            ...meal,
            items: meal.items?.$values || []
          })) || [])
        }));

        setRecommendations(formattedRecommendations);
        setLoading(false);
     
    };

    const fetchLikedRecommendations = async () => {
      const response = await getRecommendationLikes();
      const likedData = (response.data.$values || []).map((item: any) => ({
        ...item.recommendation,
        meals: removeDuplicateMeals(item.recommendation.meals?.$values?.map((meal: any) => ({
          ...meal,
          items: meal.items?.$values || []
        })) || [])

      }))
      .sort((a: { date: string | number | Date; }, b: { date: string | number | Date; }) => new Date(b.date).getTime()- new Date(a.date).getTime());
      // .sort((a, b) => new Date(b.date).getTime()- new Date(a.date).getTime());
      setLikedRecommendations(likedData);
    };

    fetchRecommendations();
    fetchLikedRecommendations();
  }, [userId]);

  const displayedRecommendations = showLikedOnly ? likedRecommendations : recommendations;

  return (
    <div className="max-w-2xl mx-auto p-6">
    {/* Dynamiczny nagłówek w zależności od showLikedOnly */}
    <h2 className="text-2xl font-semibold text-center mb-4">
      {showLikedOnly ? "Zapisane rekomendacje" : "Ostatnia rekomendacja"}
    </h2>

      <div className="flex justify-center mb-4">
        <FormControlLabel
          control={
            <Checkbox
              checked={showLikedOnly}
              onChange={() => setShowLikedOnly((prev) => !prev)}
              color="primary"
            />
          }
          label="Zapisane rekomendacje"
        />
      </div>

      {loading ? (
        <p className="text-center text-[var(--recommendation-gray)]">Ładowanie rekomendacji...</p>
      ) : displayedRecommendations.length === 0 ? (
        <p className="text-center text-[var(--recommendation-gray)]">Brak rekomendacji do wyświetlenia.</p>
      ) : (
        <div className="space-y-6">
          {displayedRecommendations.map((rec) => (
            <div key={rec.id} className="border border-gray-300 rounded-lg p-6 bg-gray-100 shadow-md">
              <p className="font-bold text-lg">
                {new Date(rec.date).toLocaleDateString()} - <span className="text-[var(--seccondary-gray)]">{rec.problem}</span>
              </p>
              <p className="text-[var(--seccondary-gray)]mb-3">
                Kategoria: <span className="font-semibold">{rec.selectedCategory}</span>
              </p>
              {rec.meals.length > 0 ? (
                rec.meals.map((meal, index) => (
                  <div key={index} className="mt-3">
                    <p className="font-semibold text-[var(--seccondary-gray-hover)]">{meal.name}</p>
                    {meal.items.length > 0 ? (
                      <ul className="list-disc pl-6 text-[var(--seccondary-gray)] space-y-1">
                        {meal.items.map((item, idx) => (
                          <li key={idx} className="ml-2">
                            <span className="font-medium">{item.food}</span> - {item.grams}g ({item.providedValue} kcal)
                          </li>
                        ))}
                      </ul>
                    ) : (
                      <p className="text-[var(--recommendation-gray)]">Brak produktów</p>
                    )}
                  </div>
                ))
              ) : (
                <p className="text-[var(--recommendation-gray)]">Brak posiłków</p>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default Recommendation;
