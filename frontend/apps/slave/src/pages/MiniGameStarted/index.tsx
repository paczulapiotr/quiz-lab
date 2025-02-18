import { PageTemplate } from "@repo/ui/components";
import AbcdWithCategories from "../miniGames/AbcdWithCategories";
import MusicGuess from "../miniGames/MusicGuess";
import { useParams } from "react-router";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import { MiniGameType } from "@repo/ui/api/requests/minigames/types";
import LettersAndPhrases from "../miniGames/LettersAndPhrases";
import Sorter from "../miniGames/Sorter";
import FamilyFeud from "../miniGames/FamilyFeud";

type Props = {
  basePath: string;
};

const MiniGameStarted = ({ basePath }: Props) => {
  const { gameId } = useParams<{ gameId: string }>();
  const { data } = useGetMiniGame(gameId);
  
  const type = data?.miniGameType ?? MiniGameType.FamilyFeud;

  const renderMiniGame = () => {
    switch (type) {
      case MiniGameType.AbcdWithCategories:
        return <AbcdWithCategories basePath={basePath} />;
      case MiniGameType.MusicGuess:
        return <MusicGuess basePath={basePath} />;
      case MiniGameType.LettersAndPhrases:
        return <LettersAndPhrases basePath={basePath} />;
      case MiniGameType.Sorter:
        return <Sorter basePath={basePath} />;
      case MiniGameType.FamilyFeud:
        return <FamilyFeud basePath={basePath} />;
      default:
        return (
          <PageTemplate>
            Unknown mini game type: {data?.miniGameType}
          </PageTemplate>
        );
    }
  };

  return renderMiniGame();
};

export default MiniGameStarted;
