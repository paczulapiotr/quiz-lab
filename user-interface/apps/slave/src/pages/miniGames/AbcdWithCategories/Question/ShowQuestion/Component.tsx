import { Tile } from "@repo/ui/components";

type Props = {
  questionId: string;
  question: string;
};

const Component = ({ question }: Props) => {
  return <Tile blue text={question} />;
};

export default Component;
