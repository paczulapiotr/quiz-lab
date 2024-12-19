export type AbcdWithCategoriesState = {
  miniGameType: 1;
  state?: {
    currentRoundId: string;
    currentCategoryId: string;
    currentQuestionId: string;
    rounds: RoundState[];
  }
}

export type RoundState = {
  roundId: string;
  categoryId: string;
  powerPlays: PowerPlaysDictionary;
  answers: RoundAnswer[];
};

export type RoundAnswer = {
  deviceId: string;
  answerId?: string | null;
  isCorrect: boolean;
  timestamp?: Date | null;
  points: number;
};

export type PowerPlaysDictionary = {
  [deviceId: string]: {
    powerPlay: string;
    sourceDeviceId: string;
  };
}

