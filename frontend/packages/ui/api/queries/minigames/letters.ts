export type LettersState = {
  currentRoundId?: string;
  currentGuessingPlayerId?: string;
  rounds: RoundState[];
};

export type RoundState = {
  roundId: string;
  answers: RoundAnswer[];
};

export type RoundAnswer = {
  playerId: string;
  letter: string;
  isCorrect: boolean;
  timestamp?: string;
  points: number;
};

export type LettersDefinition = {
  rounds: Round[];
};

export type Round = {
  id: string;
  phrase: string;
};
