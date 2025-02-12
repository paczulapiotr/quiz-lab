import { Tile } from "../Tile";

type Props = {
  title: string;
  secondaryText: string;
};

const CenteredInstruction = ({ secondaryText, title }: Props) => {
  return (
    <div
      style={{
        marginTop: "auto",
        marginBottom: "auto",
      }}
    >
      <Tile blue text={title} />
      <p
        style={{
          position: "fixed",
          display: "block",
          top: "6rem",
          left: 0,
          width: "100%",
          zIndex: 1,
          padding: "1rem",
          textAlign: "center",
          color: "white",
          background: "rgba(60, 60, 60, 0.5)",
        }}
      >
        {secondaryText}
      </p>
    </div>
  );
};

export default CenteredInstruction;
