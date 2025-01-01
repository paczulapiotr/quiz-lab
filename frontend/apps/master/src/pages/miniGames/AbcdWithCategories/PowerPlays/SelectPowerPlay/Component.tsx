import { CenteredInstruction, Timer } from "@repo/ui/components";

const Component = () => {
  return (
    <>
      <CenteredInstruction
        title="Wybierz zagrywkę na swoim monitorze"
        secondaryText=""
      />
      <Timer startSeconds={29} />
    </>
  );
};

export default Component;
