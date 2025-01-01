import { PageTemplate } from "@repo/ui/components";
import AbcdWithCategories from "../miniGames/AbcdWithCategories";

type Props = {
  miniGameType: "AbcdWithCategories";
  basePath: string;
};

const MiniGameStarted = ({ miniGameType, basePath }: Props) => {
  switch (miniGameType) {
    case "AbcdWithCategories":
      return <AbcdWithCategories basePath={basePath} />;
    default:
      return (
        <PageTemplate>Unknown mini game type: {miniGameType}</PageTemplate>
      );
  }
};

export default MiniGameStarted;
