import { CenteredInstruction, Timer } from "@repo/ui/components";
import Times from "@repo/ui/config/times";

const Component = () => {
  return (
    <>
      <CenteredInstruction
        title="Wybierz zagrywkę na swoim monitorze"
        secondaryText=""
      />
      <Timer startSeconds={Times.Abdc.PowerPlaySelectionSeconds} />
    </>
  );
};

export default Component;
