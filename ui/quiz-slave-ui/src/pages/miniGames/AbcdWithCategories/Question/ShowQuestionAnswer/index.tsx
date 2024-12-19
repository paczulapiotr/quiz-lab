type Props = {
  gameId: string;
};

const ShowQuestionAnswer = ({ gameId }: Props) => {
  return <div>{"ShowQuestionAnswer: " + gameId}</div>;
};

export default ShowQuestionAnswer;
