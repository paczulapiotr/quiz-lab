import { fileUrl } from "@repo/ui/api/files";
import { useUpdateMiniGame } from "@repo/ui/api/mutations/useUpdateMiniGame";
import { AudioPlayer, HeaderTile, Timer } from "@repo/ui/components";
import Times from "@repo/ui/config/times";
import { useGame } from "@repo/ui/contexts/GameContext";
import { AbcdActions } from "@repo/ui/minigames/actions";

type Props = {
  audioUrl?: string;
  questionId: string;
  question: string;
};

const Component = ({ question, audioUrl }: Props) => {
  const { gameId } = useGame();
  const { mutate } = useUpdateMiniGame();

  const onTimeUp = () =>
    mutate({
      gameId: gameId!,
      action: AbcdActions.QuestionShowStop,
    });

  return (
    <>
      {audioUrl ? <AudioPlayer play src={fileUrl(audioUrl)} /> : null}
      <HeaderTile title={question} />
      <div style={{ flex: 1 }} />
      <Timer
        startSeconds={Times.Abdc.QuestionShowSeconds}
        onTimeUp={onTimeUp}
      />
    </>
  );
};

export default Component;
