import BombIcon from "@/assets/icons/bomb.svg";
import { motion as m } from "motion/react";
import { FlyingObjects, SimpleModal } from "quiz-common-ui/components";
import { useState } from "react";
import { PowerPlaysEnum } from "../../../PowerPlays/types";
import styles from "./Bombs.module.scss";

type Props = {
  powerPlays: PowerPlaysEnum[];
};

const Bombs = ({ powerPlays }: Props) => {
  const [bombDetonated, setBombDetonated] = useState<boolean>(false);
  const bombsCount =
    powerPlays.filter((x) => x === PowerPlaysEnum.Bombs).length || 5;

  return (
    <>
      {bombsCount > 0 && (
        <FlyingObjects count={bombsCount} speed={10} rotationSpeed={5}>
          <img
            className={styles.bomb}
            src={BombIcon}
            width={150}
            alt="bomb"
            onMouseDown={() => setBombDetonated(true)}
          />
        </FlyingObjects>
      )}
      <SimpleModal
        isOpen={bombDetonated}
        onAfterOpen={() => setTimeout(() => setBombDetonated(false), 3_000)}
      >
        <m.div
          initial={{ opacity: 1 }}
          animate={{ opacity: 0 }}
          transition={{ duration: 3 }}
          style={{
            backgroundColor: "white",
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            width: "100%",
            height: "100%",
          }}
        >
          <h1>BUM</h1>
        </m.div>
      </SimpleModal>
    </>
  );
};

export default Bombs;
