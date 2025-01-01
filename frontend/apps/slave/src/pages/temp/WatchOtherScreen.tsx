import { CenteredInstruction } from "@repo/ui/components";

type Props = {
  title: string;
};

const WatchOtherScreen = ({ title }: Props) => {
  return (
    <CenteredInstruction
      title="Objerzyj prezentację na ekranie na środku sali"
      secondaryText={title}
    />
  );
};

export default WatchOtherScreen;
