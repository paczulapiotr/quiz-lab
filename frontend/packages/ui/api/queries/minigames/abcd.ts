import { PowerPlaysEnum } from "../../requests/minigames/types";

export type AbcdState = {
    currentRoundId?: string;
    currentCategoryId?: string;
    currentQuestionId?: string;
    rounds: RoundState[];
};

export type RoundState = {
    roundId: string;
    categoryId: string;
    powerPlays: PowerPlaysDictionary;
    answers: RoundAnswer[];
    selectedCategories: SelectedCategory[];
};

// <playerId, powerPlays>
export type PowerPlaysDictionary = Record<string, ({fromPlayerId:string, powerPlay:PowerPlaysEnum})[]>

export type SelectedCategory = {
    categoryId: string;
    playerIds: string[];
};

export type RoundAnswer = {
    playerId: string;
    answerId?: string;
    isCorrect: boolean;
    timestamp?: Date;
    points: number;
};


export type AbcdDefinition = {
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
    text: string;
    answers: Answer[];
    audioUrl?: string;
};

export type Answer = 
{
    id: string;
    text: string;
    isCorrect: boolean;
}