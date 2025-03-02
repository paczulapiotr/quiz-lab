export type FamilyFeudDefinition = {
  rounds: Round[];
};

export type Round = {
  id: string;
  question: string;
  answers: QuestionAnswer[];
};

export type QuestionAnswer = {
  id: string;
  answer: string;
  synonyms: string[];
  points: number;
};

export type FamilyFeudState = {
    currentRoundId?: string;
    currentGuessingPlayerId?: string | null;
    rounds: RoundState[];
};

export type RoundState = {
    roundId: string;
    answers: RoundAnswer[];
};

export type RoundAnswer = {
    playerId: string;
    answer?: string | null;
    matchedAnswerId?: string | null;
    matchedAnswer?: string | null;
    points: number;
    timestamp?: string;
};