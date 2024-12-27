import instance from "@/api/instance";
import { PowerPlaysEnum } from "@/pages/miniGames/AbcdWithCategories/PowerPlays/types";
import { AxiosResponse } from "axios";

export type GetAppliedPowerPlayResponse = {
  players: {
    playerId: string;
    playerName: string;
    powerPlays: {
      playerId: string;
      playerName: string;
      powerPlay: PowerPlaysEnum;
    }[];
  }[];
};

export const getAppliedPowerPlay = async (gameId: string) =>
  (
    await instance.get<never, AxiosResponse<GetAppliedPowerPlayResponse>>(
      `/game/${gameId}/mini-game/abcd/applied-power-play`,
    )
  ).data;

export type GetCategoriesResponse = {
  categories: {
    id: string;
    text: string;
  }[];
};

export const getCategories = async (gameId: string) =>
  (
    await instance.get<never, AxiosResponse<GetCategoriesResponse>>(
      `/game/${gameId}/mini-game/abcd/categories`,
    )
  ).data;

export type GetPowerPlaysResponse = {
  players: {
    id: string;
    name: string;
  }[];
  powerPlays: PowerPlaysEnum[];
};

export const getPowerPlays = async (gameId: string) =>
  (
    await instance.get<never, AxiosResponse<GetPowerPlaysResponse>>(
      `/game/${gameId}/mini-game/abcd/power-plays`,
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
      `/game/${gameId}/mini-game/abcd/question`,
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
      `/game/${gameId}/mini-game/abcd/question-answer`,
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
      `/game/${gameId}/mini-game/abcd/selected-category`,
    )
  ).data;
