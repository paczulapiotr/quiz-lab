import { PageTemplate } from "@repo/ui/components";
import AbcdWithCategories from "../miniGames/AbcdWithCategories";
import MusicGuess from "../miniGames/MusicGuess";
import { MiniGameType } from "@repo/ui/api/requests/minigames/types";
import LettersAndPhrases from "../miniGames/LettersAndPhrases";
import Sorter from "../miniGames/Sorter";
import FamilyFeud from "../miniGames/FamilyFeud";
import { useGame } from "@repo/ui/contexts/GameContext";


const MiniGameStarted = () => {
  const { miniGameType } = useGame();

  const renderMiniGame = () => {
    switch (miniGameType) {
      case MiniGameType.AbcdWithCategories:
        return <AbcdWithCategories key={miniGameType}/>;
      case MiniGameType.MusicGuess:
        return <MusicGuess key={miniGameType} />;
      case MiniGameType.LettersAndPhrases:
        return <LettersAndPhrases key={miniGameType} />;
      case MiniGameType.Sorter:
        return <Sorter key={miniGameType} />;
      case MiniGameType.FamilyFeud:
        return <FamilyFeud key={miniGameType} />;
      default:
        return (
          <PageTemplate>Unknown mini game type: {miniGameType}</PageTemplate>
        );
    }
  };

  return renderMiniGame();
};

export default MiniGameStarted;
