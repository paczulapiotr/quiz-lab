import { useUpdateMiniGame } from "@/api/mutations/useUpdateMiniGame";
import { HeaderTile, Timer } from "@repo/ui/components";
import Times from "@repo/ui/config/times";
import { AbcdActions } from "@repo/ui/minigames/actions";
import { useParams } from "react-router";

type Props = {
  questionId: string;
  question: string;
};

const Component = ({ question }: Props) => {
  const { gameId } = useParams<{ gameId: string }>();
  const { mutate } = useUpdateMiniGame();

  const onTimeUp = () =>
    mutate({
      gameId: gameId!,
      action: AbcdActions.QuestionShowStop,
    });

  return (
    <>
      <HeaderTile title={question} />
      <Timer startSeconds={Times.Abdc.QuestionShowSeconds} onTimeUp={onTimeUp} />
    </>
  );
};

export default Component;
