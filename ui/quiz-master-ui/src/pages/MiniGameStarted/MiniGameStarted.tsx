import { PageTemplate } from "quiz-common-ui/components";
import AbcdWithCategories from "../miniGames/AbcdWithCategories";

type Props = {
  miniGameType: "AbcdWithCategories";
  basePath: string;
};

const MiniGameStarted = ({ miniGameType, basePath }: Props) => {
  const renderMiniGame = () => {
    switch (miniGameType) {
      case "AbcdWithCategories":
        return <AbcdWithCategories basePath={basePath} />;
      default:
        return (
          <PageTemplate>Unknown mini game type: {miniGameType}</PageTemplate>
        );
    }
  };

  return renderMiniGame();
};
export default MiniGameStarted;
