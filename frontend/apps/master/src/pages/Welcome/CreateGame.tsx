import { useSearchGameNames } from "@repo/ui/api/queries/useSearchGameNames";
import { createGame, GameLanguage } from "@repo/ui/api/requests/game";
import { useRoom } from "@repo/ui/contexts/RoomContext";
import { useState } from "react";
import styles from "./CreateGame.module.scss";
import classNames from "classnames";
import { Tile } from "@repo/ui/components";

export type Props = { className?: string };

const CreateGame = ({ className }: Props) => {
  const { room } = useRoom();
  const [opt, setOpts] = useState<string>();
  const [size, setSize] = useState<number>();
  const [posting, setPosting] = useState(false);
  const { data } = useSearchGameNames();

  const onSelect = async () => {
    const selected = data?.find((game) => game.identifier === opt);

    if (size == null || selected == null || room == null) return;

    setPosting(true);
    try {
      await createGame(
        size!,
        selected!.identifier,
        room!.code,
        GameLanguage.EN,
      );
    } finally {
      setPosting(false);
    }
  };

  return (
    <div className={classNames(styles.input, className)}>
      {data && (
        <>
          <Tile blue text={"Room: " + room?.code} />
          <select
            disabled={posting}
            className={styles.select}
            value={opt}
            onChange={(e) => setOpts(e.target.value)}
          >
            <option disabled value={undefined} selected={opt == null}>
              {"Select game"}
            </option>
            {data.map((game) => (
              <option key={game.identifier} value={game.identifier}>
                {game.name}
              </option>
            ))}
          </select>
          <select
            disabled={posting}
            className={styles.select}
            value={size}
            onChange={(e) => setSize(Number(e.target.value))}
          >
            <option disabled selected={size == null}>
              {"Select size"}
            </option>
            {[1, 2, 3, 4, 5, 6].map((num) => (
              <option key={num} value={num}>
                {num}
              </option>
            ))}
          </select>
          <button
            className={styles.button}
            onClick={onSelect}
            disabled={posting}
          >
            {"Create game"}
          </button>
        </>
      )}
    </div>
  );
};

export default CreateGame;
