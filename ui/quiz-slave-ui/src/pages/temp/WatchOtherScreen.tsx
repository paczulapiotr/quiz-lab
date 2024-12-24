import { FlyingSquare, Tile } from "quiz-common-ui/components";

type Props = {
  title: string;
};

const WatchOtherScreen = ({ title }: Props) => {
  return (
    <div
      style={{
        marginTop: "auto",
        marginBottom: "auto",
      }}
    >
      <Tile blue text="Objerzyj prezentację na ekranie na środku sali" />
      <p
        style={{
          position: "fixed",
          display: "block",
          top: "2rem",
          left: 0,
          width: "100%",
          zIndex: 1,
          padding: "1rem",
          textAlign: "center",
          color: "white",
          background: "rgba(60, 60, 60, 0.5)",
        }}
      >
        {title}
      </p>
      <FlyingSquare count={5} />
    </div>
  );
};

export default WatchOtherScreen;
