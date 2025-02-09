import axios, { AxiosRequestConfig } from "axios";
import { IUserType } from "../../types/types";

const axiosClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

export const register = async (data: { email: string; password: string }) =>
  axiosClient.post("/register", data, { withCredentials: true });

export const logout = async () => axiosClient.post("/api/account/logout");

export const getAuthUser = async (config?: AxiosRequestConfig) =>
  axiosClient.get<IUserType>("/users/me", config);

export const changePassword = async (data: {
  oldPassword: string;
  newPassword: string;
}) => axiosClient.post("/api/account/change-password", data);

export const changeEmail = async (data: { newEmail: string }) =>
  axiosClient.post("/api/account/change-email", { NewEmail: data.newEmail });

export const currentEmail = async () =>
  axiosClient.get("/api/account/current-email");

export const changeUserData = async (data: {
  firstName: string;
  lastName: string;
  weight: number;
  height: number;
}) => axiosClient.post("/api/userdata/change-data", data);

export const getUserData = async () =>
  axiosClient.get("/api/userdata/current-user");

export const getUserDetails = async () =>
  axiosClient.get("/api/userdetails/current-user");

export const changeUserDetails = async (data: {
  gender: string;
  dateOfBirth: Date | null;
}) => axiosClient.post("/api/userdetails/change-details", data);

export const createRecommendation = async (data: {
  userId: string;
  userWeight: number;
  userHeight?: number;
  selectedCondition: string;
  preferredCategory?: string;
}) => axiosClient.post("/api/DietRecommendation/recommend-diet", data);

export const likeRecommendation = async (recommendationId: number) =>
  axiosClient.post(`/api/UserDietRecommendation/like/${recommendationId}`);

export const getRecommendationLikes = async () =>
  axiosClient.get(`/api/UserDietRecommendation/liked`);

export const getLastUserRecommendation = async (userId: string) => {
  return await axiosClient.get(
    `/api/UserDietRecommendation/user-last-recommendation/${userId}`
  );
};

export const getUserRecommendations = (userId: string, limit: number = 5) => {
  return axiosClient.get(
    `/api/DietRecommendation/user-recommendations/${userId}?limit=${limit}`
  );
};

export const postUserStatistics = async (data: {
  monitoringType: string;
  value: number;
}) => axiosClient.post("/api/userstats", data);
