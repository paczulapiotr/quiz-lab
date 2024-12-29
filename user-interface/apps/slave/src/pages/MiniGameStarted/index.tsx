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
      return <p>Unknown mini game type: {miniGameType}</p>;
  }
};

export default MiniGameStarted;
