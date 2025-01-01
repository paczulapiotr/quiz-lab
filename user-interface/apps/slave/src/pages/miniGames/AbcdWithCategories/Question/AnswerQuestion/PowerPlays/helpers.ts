import { PowerPlaysEnum } from "../../../PowerPlays/types";

export const applyLetterPowerPlay = (
  text: string,
  powerPlays: PowerPlaysEnum[],
) => {
  const letterPowerPlays = powerPlays.filter(
    (pp) => pp === PowerPlaysEnum.Letters,
  ).length;

  if (letterPowerPlays === 0) return text;

  const lettersToReplace = Math.min(
    Math.max(Math.ceil((text.length * letterPowerPlays) / 6), letterPowerPlays),
    text.length,
  );

  const indices = Array.from({ length: text.length }, (_, i) => i).sort(
    () => Math.random() - 0.5,
  );

  let replacedText = text;

  for (let index = 0; index < lettersToReplace; index++) {
    const i = indices[index];
    replacedText =
      replacedText.substring(0, i) + " " + replacedText.substring(i + 1);
  }

  return replacedText;
};
