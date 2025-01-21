import { PageTemplate } from "@repo/ui/components";
import AbcdWithCategories from "../miniGames/AbcdWithCategories";
import MusicGuess from "../miniGames/MusicGuess";

type Props = {
  miniGameType: "AbcdWithCategories" | "MusicGuess";
  basePath: string;
};

const MiniGameStarted = ({ miniGameType, basePath }: Props) => {
  const renderMiniGame = () => {
    switch (miniGameType) {
      case "AbcdWithCategories":
        return <AbcdWithCategories basePath={basePath} />;
      case "MusicGuess":
        return <MusicGuess basePath={basePath} />;
      default:
        return (
          <PageTemplate>Unknown mini game type: {miniGameType}</PageTemplate>
        );
    }
  };

  return renderMiniGame();
};
export default MiniGameStarted;
