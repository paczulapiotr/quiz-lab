import { AnimatePresence } from "motion/react";
import { PropsWithChildren } from "react";

const Animate = ({ children }: PropsWithChildren) => {
  return <AnimatePresence initial={false}>{children}</AnimatePresence>;
};

export default Animate;
