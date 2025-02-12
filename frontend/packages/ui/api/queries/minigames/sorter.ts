// State
export type SorterState = {
  currentRoundId?: string;
  rounds: RoundState[];
};

export type RoundState = {
  roundId: string;
  answers: RoundAnswer[];
};

export type RoundAnswer = {
  playerId: string;
  items: RoundAnswerItem[];
  points: number;
  correctAnswers: number;
};

export type RoundAnswerItem = {
  categoryId: string;
  categoryItemId: string;
  timestamp: Date;
};

// Definition
export type SorterDefinition = {
  rounds: Round[];
};

export type Round = {
  id: string;
  leftCategory: Category;
  rightCategory: Category;
};

export type Category = {
  id: string;
  name: string;
  items: CategoryItem[];
};

export type CategoryItem = {
  id: string;
  text: string;
};
