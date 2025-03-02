export enum PowerPlaysEnum {
  Slime = 1,
  Freeze,
  Bombs,
  Letters,
}

export const PowerPlaysNames = {
  [PowerPlaysEnum.Slime]: "Slime",
  [PowerPlaysEnum.Freeze]: "Freeze",
  [PowerPlaysEnum.Bombs]: "Bombs",
  [PowerPlaysEnum.Letters]: "Letters",
};

export enum MiniGameType {
  AbcdWithCategories = 1,
  MusicGuess,
  LettersAndPhrases,
  Sorter,
  FamilyFeud,
}
