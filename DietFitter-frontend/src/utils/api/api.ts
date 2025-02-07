import axios, { AxiosRequestConfig } from "axios";
import { IUserType } from "../../types/types";

const axiosClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

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

export const getLastUserRecommendation = async (userId: string) => {
  return await axios.get(`/api/algorithm/last-recommendation/${userId}`);
};

export const createRecommendation = async (data: {
  userId: string;
  userWeight: number;
  userHeight?: number;
  selectedCondition: string;
  preferredCategory?: string;
}) => axiosClient.post("/api/algorithm/recommend-diet", data);
