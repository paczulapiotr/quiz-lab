import instance from "@/api/instance";
import { AxiosResponse } from "axios";

export type GetCategoriesResponse = {
  categories: {
    id: string;
    text: string;
  }[];
};

export const getCategories = async (gameId: string) =>
  (
    await instance.get<never, AxiosResponse<GetCategoriesResponse>>(
      `/game/${gameId}/mini-game/music/categories`,
    )
  ).data;


export type GetQuestionResponse = {
  questionId?: string;
  question?: string;
  answers?: {
    id: string;
    text: string;
  }[];
};

export const getQuestion = async (gameId: string) =>
  (
    await instance.get<never, AxiosResponse<GetQuestionResponse>>(
      `/game/${gameId}/mini-game/music/question`,
    )
  ).data;

export type PlayerResult = {
  id: string;
  name: string;
  answerId?: string;
  answerPoints: number;
  roundPoints: number;
};
export type AnswerResult = {
  id: string;
  text: string;
  isCorrect: boolean;
};

export type GetQuestionAnswerResponse = {
  answer?: PlayerResult;
  answers: AnswerResult[];
  players: PlayerResult[];
};

export const getQuestionAnswer = async (gameId: string) =>
  (
    await instance.get<never, AxiosResponse<GetQuestionAnswerResponse>>(
      `/game/${gameId}/mini-game/music/question-answer`,
    )
  ).data;

export type GetSelectedCategoryResponse = {
  categories: {
    id: string;
    text: string;
    isSelected: boolean;
    players: {
      id: string;
      name: string;
    }[];
  }[];
};

export const getSelectedCategory = async (gameId: string) =>
  (
    await instance.get<never, AxiosResponse<GetSelectedCategoryResponse>>(
      `/game/${gameId}/mini-game/music/selected-category`,
    )
  ).data;
