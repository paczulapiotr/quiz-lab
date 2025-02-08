export type MusicGuessState = {
  currentRoundId: string | null;
  currentCategoryId: string | null;
  currentQuestionId: string | null;
  rounds: RoundState[];
};

export type RoundState = {
  roundId: string;
  categoryId: string;
  answers: RoundAnswer[];
  selectedCategories: SelectedCategory[];
};

export type SelectedCategory = {
  categoryId: string;
  playerIds: string[];
};

export type RoundAnswer = {
  playerId: string;
  answerId: string | null;
  isCorrect: boolean;
  timestamp: string | null;
  points: number;
};

export type MusicGuessDefinition = {
  rounds: Round[];
};

export type Round = {
  id: string;
  categories: Category[];
};

export type Category = {
  id: string;
  name: string;
  questions: Question[];
};

export type Question = {
  id: string;
  text?: string;
  answers: Answer[];
  audioUrl?: string;
};

export type Answer = {
  id: string;
  text?: string;
  isCorrect: boolean;
  points: number;
};
